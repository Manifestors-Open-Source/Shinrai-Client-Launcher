using Guna.UI2.WinForms;
using System.Net.Http;
using System.Security.Principal;
using System.Text.Json;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shinrai_Client_Launcher
{
    public partial class Form1 : Form
    {
        int AccountCount = 0;
        public static string SelectedAccountName;
        public static AccountType SelectedAccountType;
        public static string SelectedAccountCode;
        public enum AccountType
        {
            Premium,
            Offline
        }

        private const string RedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient";

        public class AccountEntry
        {
            public string username { get; set; }
            public string type { get; set; }
            public string accessToken { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
        }
        private static readonly HttpClient client = new HttpClient();

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAccountsFromJson("accounts.json");
        }
        private void LoadAccountsFromJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("accounts.json dosyasý bulunamadý.");
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath).Trim();

                if (string.IsNullOrWhiteSpace(json))
                {
                    guna2Panel1.Show();
                    guna2Panel6.Hide();
                    flowLayoutPanel1.Controls.Add(label4);
                    return;
                }

                var accountList = JsonSerializer.Deserialize<List<AccountEntry>>(json);

                if (accountList == null || accountList.Count == 0)
                {
                    guna2Panel1.Show();
                    guna2Panel6.Hide();
                    flowLayoutPanel1.Controls.Add(label4);
                    return;
                }

                guna2Panel1.Show();

                foreach (var acc in accountList)
                {
                    if (Enum.TryParse(acc.type, true, out AccountType typeEnum))
                    {
                        AddUserLabel(acc.username, typeEnum, acc.accessToken);
                    }
                    else
                    {
                        MessageBox.Show($"Geçersiz hesap tipi: {acc.type}");
                    }
                }
            }
            catch (JsonException jex)
            {
                MessageBox.Show($"Geçersiz JSON formatý: {jex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
        }

        private void AddAccountToJson(string filePath, string username, AccountType accountType, string accessToken)
        {
            List<AccountEntry> accountList = new List<AccountEntry>();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath).Trim();
                if (!string.IsNullOrWhiteSpace(json))
                {
                    try
                    {
                        accountList = JsonSerializer.Deserialize<List<AccountEntry>>(json) ?? new List<AccountEntry>();
                    }
                    catch (JsonException)
                    {
                        MessageBox.Show("accounts.json dosyasý bozuk veya geçersiz.");
                        return;
                    }
                }
            }

            if (accountList.Any(acc => acc.username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Bu kullanýcý zaten listede.");
                return;
            }

            // Eðer Premium deðilse accessToken boþ döner
            string tokenToSave = accountType == AccountType.Premium ? accessToken : string.Empty;

            accountList.Add(new AccountEntry
            {
                username = username,
                type = accountType.ToString(),
                accessToken = tokenToSave
            });

            string updatedJson = JsonSerializer.Serialize(accountList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, updatedJson);
        }



        void AddUserLabel(string UserName, AccountType AccountTypeS, string accessToken)
        {
            Guna2Panel guna2PanelC = new Guna2Panel();
            guna2PanelC.BackColor = System.Drawing.Color.Transparent;
            guna2PanelC.FillColor = Color.FromArgb(100, 0, 0, 0);
            guna2PanelC.Size = new Size(323, 116);
            guna2PanelC.BorderRadius = 15;
            flowLayoutPanel1.Controls.Add(guna2PanelC);

            Label UlabelC = new Label();
            UlabelC.AutoSize = false;
            UlabelC.ForeColor = Color.White;
            UlabelC.Size = new Size(215, 29);
            UlabelC.Location = new Point(92, 16);
            UlabelC.Font = new Font("Comfortaa", 14F, FontStyle.Bold);
            UlabelC.Text = UserName;
            guna2PanelC.Controls.Add(UlabelC);

            Label ATlabelC = new Label();
            ATlabelC.AutoSize = false;
            ATlabelC.ForeColor = Color.White;
            ATlabelC.Size = new Size(212, 25);
            ATlabelC.Location = new Point(95, 45);
            ATlabelC.Font = new Font("Comfortaa", 9F, FontStyle.Bold);
            ATlabelC.Text = AccountTypeS.ToString();
            guna2PanelC.Controls.Add(ATlabelC);

            PictureBox pictureBoxC = new PictureBox();
            pictureBoxC.Size = new Size(70, 70);
            pictureBoxC.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBoxC.Location = new Point(23, 23);
            Image img = LoadSkin(UserName);
            pictureBoxC.BackgroundImage = img;
            guna2PanelC.Controls.Add(pictureBoxC);

            Guna2Button guna2ButtonC = new Guna2Button();
            guna2ButtonC.Size = new Size(96, 29);
            guna2ButtonC.Location = new Point(208, 73);
            guna2ButtonC.BorderRadius = 5;
            guna2ButtonC.FillColor = Color.FromArgb(100, 0, 0, 0);
            guna2ButtonC.ForeColor = Color.White;
            guna2ButtonC.Font = new Font("Comfortaa", 9F, FontStyle.Bold);
            guna2ButtonC.Text = "Login";
            guna2ButtonC.Click += (s, args) =>
            {

                if (AccountTypeS == AccountType.Premium && !string.IsNullOrEmpty(accessToken))
                {
                    var account = GetAccountByUsername(UserName); // veya targetUsername

                    if (account != null)
                    {
                        SelectedAccountName = account.username;
                        SelectedAccountType = (AccountType)Enum.Parse(typeof(AccountType), account.type);
                        SelectedAccountCode = account.accessToken;

                        MainScreen mainScreen = new MainScreen();
                        mainScreen.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Kullanýcý bulunamadý.");
                    }




                }
                else
                {
                    SelectedAccountName = UserName;
                    SelectedAccountType = AccountType.Offline;
                    SelectedAccountCode = "";
                    MainScreen mainScreen = new MainScreen();
                    mainScreen.Show();
                    this.Hide();
                }
            };
            guna2PanelC.Controls.Add(guna2ButtonC);
        }

        private AccountEntry GetAccountByUsername(string username)
        {
            string filePath = "accounts.json";

            if (!File.Exists(filePath))
                return null;

            try
            {
                string json = File.ReadAllText(filePath);
                var accountList = JsonSerializer.Deserialize<List<AccountEntry>>(json);

                return accountList?.FirstOrDefault(a =>
                    a.username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return null;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AddAccountToJson("accounts.json", guna2TextBox1.Text, AccountType.Offline, "");
            flowLayoutPanel1.Controls.Clear();
            LoadAccountsFromJson("accounts.json");
            guna2Panel1.Hide();
            guna2Panel6.Show();
        }

        private Image LoadSkin(string username)
        {
            string USR = username;

            if (string.IsNullOrEmpty(USR))
                return Properties.Resources.IconSearch;

            try
            {
                string uuidUrl = $"https://api.mojang.com/users/profiles/minecraft/{USR}";

                var response = client.GetAsync(uuidUrl).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                    return Properties.Resources.IconSearch;

                string json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var doc = JsonDocument.Parse(json);
                string uuid = doc.RootElement.GetProperty("id").GetString();

                string headUrl = $"https://crafatar.com/avatars/{uuid}?size=64&overlay";

                var imageStream = client.GetStreamAsync(headUrl).GetAwaiter().GetResult();
                return Image.FromStream(imageStream);
            }
            catch
            {
                return Properties.Resources.IconSearch;
            }
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button7_Click_1(object sender, EventArgs e)
        {
            guna2Panel6.Hide();
            guna2Panel1.Show();
        }

        private void guna2Button8_Click_1(object sender, EventArgs e)
        {
            guna2Panel6.Show();
            guna2Panel1.Hide();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            buttonLogin_Click();
        }
        private async void buttonLogin_Click()
        {
            try
            {
                ManifestorsAuthEngine.ClientID = "00000000402b5328";
                string scope = Uri.EscapeDataString("XboxLive.signin offline_access");
                string authUrl = $"https://login.live.com/oauth20_authorize.srf" +
                                 $"?client_id={ManifestorsAuthEngine.ClientID}" +
                                 $"&response_type=code" +
                                 $"&redirect_uri={Uri.EscapeDataString(RedirectUri)}" +
                                 $"&scope={scope}" +
                                 $"&state=manifestors_state";

                using var oauthForm = new MicrosoftAuthForm(authUrl, RedirectUri);
                oauthForm.Show();

                string code = await oauthForm.WaitForCodeAsync();

                if (string.IsNullOrEmpty(code))
                {
                    MessageBox.Show("Authorization code alýnamadý.");
                    return;
                }

                await ManifestorsAuthEngine.ExchangeCodeAndLoginAsync(code);

                var profile = ManifestorsAuthEngine.Get();

             
                var accountType = AccountType.Premium;

              
                string tokenToSave = string.IsNullOrEmpty(profile.Name) ? "" : ManifestorsAuthEngine.GetAccessToken();

                AddAccountToJson("accounts.json", profile.Name, accountType, tokenToSave);

                flowLayoutPanel1.Controls.Clear();
                LoadAccountsFromJson("accounts.json");

                guna2Panel1.Hide();
                guna2Panel6.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void guna2Button7_Click_2(object sender, EventArgs e)
        {
            guna2Panel1.Show();
            guna2Panel6.Hide();
        }
    }
}
