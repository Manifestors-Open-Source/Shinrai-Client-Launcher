using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shinrai_Client_Launcher
{
    public static class ManifestorsAuthEngine
    {
        public static string ClientID { get; set; } = "";
        private const string RedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient";

        private static string accessToken = null;
        private static MinecraftProfile profile = null;

        public static event Action<string> OnStatusUpdate;

        public static async Task StartEngine()
        {
            if (string.IsNullOrWhiteSpace(ClientID))
                throw new Exception("ClientID ayarlanmadı!");

            OnStatusUpdate?.Invoke("OAuth URL hazırlanıyor...");

            string authUrl = $"https://login.live.com/oauth20_authorize.srf" +
                $"?client_id={ClientID}" +
                $"&response_type=code" +
                $"&redirect_uri={Uri.EscapeDataString(RedirectUri)}" +
                $"&scope=XboxLive.signin offline_access" +
                $"&state=manifestors_state";

            OpenBrowser(authUrl);
            OnStatusUpdate?.Invoke("Tarayıcıda giriş sayfası açıldı.\n" +
                                  "Lütfen giriş yapıp izin verin.\n" +
                                  "Sonra açılan sayfada 'code' parametresini kopyalayıp buraya yapıştırın.");

            // Kullanıcıdan manuel olarak authorization code alınır
            string code = await ManualCodeInputAsync();

            if (string.IsNullOrWhiteSpace(code))
                throw new Exception("Authorization code boş olamaz.");

            OnStatusUpdate?.Invoke("Authorization code alındı, token alınıyor...");

            var token = await ExchangeCodeForTokenAsync(code);
            accessToken = token.AccessToken;

            OnStatusUpdate?.Invoke("Xbox Live token alınıyor...");
            var xblToken = await GetXboxLiveTokenAsync(accessToken);

            OnStatusUpdate?.Invoke("XSTS token alınıyor...");
            var xstsToken = await GetXSTSTokenAsync(xblToken.Token);

            OnStatusUpdate?.Invoke("Minecraft token alınıyor...");
            var mcToken = await GetMinecraftTokenAsync(xstsToken);

            accessToken = mcToken.AccessToken;

            OnStatusUpdate?.Invoke("Minecraft profili alınıyor...");
            profile = await GetMinecraftProfileAsync(accessToken);

            OnStatusUpdate?.Invoke("Giriş tamamlandı!");
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

        private static async Task<string> ManualCodeInputAsync()
        {
            Console.WriteLine("Lütfen tarayıcıdaki URL'deki 'code' parametresini buraya yapıştırın:");
            string code = Console.ReadLine();
            return await Task.FromResult(code);
        }

        private class TokenResponse
        {
            public string AccessToken { get; set; }
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

            using var doc = JsonDocument.Parse(json);
            var accessToken = doc.RootElement.GetProperty("access_token").GetString();
            var refreshToken = doc.RootElement.GetProperty("refresh_token").GetString();

            return new TokenResponse { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        private class XBLToken
        {
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

            using var doc = JsonDocument.Parse(json);
            var token = doc.RootElement.GetProperty("Token").GetString();

            return new XBLToken { Token = token };
        }

        private class XSTSToken
        {
            public string Token { get; set; }
            public DisplayClaims DisplayClaims { get; set; }
        }

        private class DisplayClaims
        {
            public Xui[] Xui { get; set; }
        }

        private class Xui
        {
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

            if (xstsToken == null || string.IsNullOrEmpty(xstsToken.Token))
                throw new Exception("XSTS token alınamadı.");

            return xstsToken;
        }

        private class MinecraftTokenResponse
        {
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
            public string Id { get; set; }
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
