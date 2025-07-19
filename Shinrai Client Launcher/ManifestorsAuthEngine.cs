using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shinrai_Client_Launcher
{
    public static class ManifestorsAuthEngine
    {
        public static string ClientID { get; set; } = "";

        private const string RedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient";

        private static string accessToken = null;
        private static MinecraftProfile profile = null;
        public static string GetAccessToken()
        {
            return accessToken ?? string.Empty;
        }

        public static event Action<string> OnStatusUpdate;

        public static async Task ExchangeCodeAndLoginAsync(string code)
        {
            var token = await ExchangeCodeForTokenAsync(code);
            accessToken = token.AccessToken;

            var xblToken = await GetXboxLiveTokenAsync(accessToken);

            var xstsToken = await GetXSTSTokenAsync(xblToken.Token);

            var mcToken = await GetMinecraftTokenAsync(xstsToken);

            accessToken = mcToken.AccessToken;

            profile = await GetMinecraftProfileAsync(accessToken);

            OnStatusUpdate?.Invoke("Giriş tamamlandı!");
        }

        public static async Task StartEngine()
        {
            if (string.IsNullOrWhiteSpace(ClientID))
                throw new Exception("ClientID ayarlanmadı!");

            OnStatusUpdate?.Invoke("OAuth URL hazırlanıyor...");

            string scope = Uri.EscapeDataString("XboxLive.signin offline_access");

            string authUrl = $"https://login.live.com/oauth20_authorize.srf" +
                $"?client_id={ClientID}" +
                $"&response_type=code" +
                $"&redirect_uri={Uri.EscapeDataString(RedirectUri)}" +
                $"&scope={scope}" +
                $"&state=manifestors_state";

            OnStatusUpdate?.Invoke("Gömülü tarayıcıda giriş sayfası açılıyor...");

            string code = await GetCodeFromEmbeddedBrowserAsync(authUrl);

            if (string.IsNullOrWhiteSpace(code))
                throw new Exception("Authorization code alınamadı.");

            OnStatusUpdate?.Invoke("Authorization code alındı, token alınıyor...");

            await ExchangeCodeAndLoginAsync(code);
        }

        private static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch
            {
                // Hata varsa sessiz geç
            }
        }

        private static async Task<string> GetCodeFromEmbeddedBrowserAsync(string authUrl)
        {
            var form = new MicrosoftAuthForm(authUrl, RedirectUri);
            form.Show();

            string code = await form.WaitForCodeAsync();

            return code;
        }

        private class TokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }

            [JsonPropertyName("refresh_token")]
            public string RefreshToken { get; set; }
        }

        private static async Task<TokenResponse> ExchangeCodeForTokenAsync(string code)
        {
            using var http = new HttpClient();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("client_id", ClientID),
                new KeyValuePair<string,string>("code", code),
                new KeyValuePair<string,string>("grant_type", "authorization_code"),
                new KeyValuePair<string,string>("redirect_uri", RedirectUri)
            });

            var res = await http.PostAsync("https://login.live.com/oauth20_token.srf", content);
            var json = await res.Content.ReadAsStringAsync();

            var token = JsonSerializer.Deserialize<TokenResponse>(json);
            if (token == null || string.IsNullOrEmpty(token.AccessToken))
                throw new Exception("Token alınamadı: " + json);

            return token;
        }

        private class XBLToken
        {
            [JsonPropertyName("Token")]
            public string Token { get; set; }
        }

        private static async Task<XBLToken> GetXboxLiveTokenAsync(string accessToken)
        {
            using var http = new HttpClient();

            var payload = new
            {
                Properties = new
                {
                    AuthMethod = "RPS",
                    SiteName = "user.auth.xboxlive.com",
                    RpsTicket = $"d={accessToken}"
                },
                RelyingParty = "http://auth.xboxlive.com",
                TokenType = "JWT"
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var res = await http.PostAsync("https://user.auth.xboxlive.com/user/authenticate", jsonContent);
            var json = await res.Content.ReadAsStringAsync();

            var token = JsonSerializer.Deserialize<XBLToken>(json);

            if (token == null || string.IsNullOrEmpty(token.Token))
                throw new Exception("Xbox Live token alınamadı: " + json);

            return token;
        }

        private class XSTSToken
        {
            [JsonPropertyName("Token")]
            public string Token { get; set; }

            [JsonPropertyName("DisplayClaims")]
            public DisplayClaims DisplayClaims { get; set; }
        }

        private class DisplayClaims
        {
            [JsonPropertyName("xui")]
            public Xui[] Xui { get; set; }
        }

        private class Xui
        {
            [JsonPropertyName("uhs")]
            public string Uhs { get; set; }
        }

        private static async Task<XSTSToken> GetXSTSTokenAsync(string xblToken)
        {
            using var http = new HttpClient();

            var payload = new
            {
                Properties = new
                {
                    SandboxId = "RETAIL",
                    UserTokens = new[] { xblToken }
                },
                RelyingParty = "rp://api.minecraftservices.com/",
                TokenType = "JWT"
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var res = await http.PostAsync("https://xsts.auth.xboxlive.com/xsts/authorize", jsonContent);
            var json = await res.Content.ReadAsStringAsync();

            var xstsToken = JsonSerializer.Deserialize<XSTSToken>(json);

            if (xstsToken == null || string.IsNullOrEmpty(xstsToken.Token) ||
                xstsToken.DisplayClaims == null || xstsToken.DisplayClaims.Xui == null ||
                xstsToken.DisplayClaims.Xui.Length == 0 || string.IsNullOrEmpty(xstsToken.DisplayClaims.Xui[0].Uhs))
            {
                throw new Exception("XSTS token veya DisplayClaims bilgisi alınamadı.\n" + json);
            }

            return xstsToken;
        }

        private class MinecraftTokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }
        }

        private static async Task<MinecraftTokenResponse> GetMinecraftTokenAsync(XSTSToken xsts)
        {
            using var http = new HttpClient();

            var payload = new
            {
                identityToken = $"XBL3.0 x={xsts.DisplayClaims.Xui[0].Uhs};{xsts.Token}"
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var res = await http.PostAsync("https://api.minecraftservices.com/authentication/login_with_xbox", jsonContent);
            var json = await res.Content.ReadAsStringAsync();

            var mcToken = JsonSerializer.Deserialize<MinecraftTokenResponse>(json);

            if (mcToken == null || string.IsNullOrEmpty(mcToken.AccessToken))
                throw new Exception("Minecraft token alınamadı.");

            return mcToken;
        }

        public class MinecraftProfile
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }
        }

        private static async Task<MinecraftProfile> GetMinecraftProfileAsync(string accessToken)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var res = await http.GetAsync("https://api.minecraftservices.com/minecraft/profile");
            var json = await res.Content.ReadAsStringAsync();

            var profile = JsonSerializer.Deserialize<MinecraftProfile>(json);

            if (profile == null || string.IsNullOrEmpty(profile.Name))
                throw new Exception("Minecraft profiline erişilemedi.");

            return profile;
        }

        public static MinecraftProfile Get()
        {
            if (profile == null)
                throw new Exception("Henüz giriş yapılmadı.");

            return profile;
        }
    }
}
