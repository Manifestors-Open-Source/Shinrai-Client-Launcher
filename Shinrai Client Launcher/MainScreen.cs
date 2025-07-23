using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shinrai_Client_Launcher
{
    public partial class MainScreen : Form
    {
        public MainScreen()
        {
            InitializeComponent();
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {

        }
        private static readonly HttpClient client = new HttpClient();
        void AddProfilesToCombobox()
        {

            string klasorYolu = @"profiles";
            string[] dosyalar = Directory.GetFiles(klasorYolu, "*.shinrai");

            foreach (string dosyaYolu in dosyalar)
            {
                string dosyaAdi = Path.GetFileName(dosyaYolu);
                guna2ComboBox1.Items.Add(dosyaAdi);
            }

        }
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void MainScreen_Load_1(object sender, EventArgs e)
        {
            LanguageLoad();

            guna2ComboBox1.SelectedItem = "1.21.7";
            AddProfilesToCombobox();
            labelUsername.Text = Form1.SelectedAccountName;
            labelAccounttype.Text = Form1.SelectedAccountType.ToString();
            ProfilPictureBox.Image = LoadSkin(Form1.SelectedAccountName);
        }
        void LanguageLoad()
        {
            Translateable translateable = new Translateable();
            translateable.LoadJson("ch_ma.json");
            txtVersion.Text = translateable.TranslatableText("launcher.mainpage.version");
            txtSettingsProfile.Text = translateable.TranslatableText("launcher.mainpage.startoptions");
            txtSelectAccount.Text = translateable.TranslatableText("launcher.mainpage.startoptions");
            txtLauncherVersion.Text = translateable.TranslatableText("launcher.mainpage.launcherversion");
            txtMotto.Text = translateable.TranslatableText("launcher.loginpage.motto");
            guna2Button7.Text = translateable.TranslatableText("launcher.mainpage.startclient");
        }
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Settings settings = new Settings();
            settings.Show();

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            var javaPath = @"java";  // Eğer Java PATH'teyse sadece "java", değilse tam yolu ver
            var workingDir = @"F:\Shinrai1";  // Proje dizini veya istediğin çalışma dizini

            // Java komut satırı argümanları
            var arguments = "-Xmx2G " +
                            "-Dfabric.dli.config=\"F:\\Shinrai1\\.gradle\\loom-cache\\launch.cfg\" " +
                            "-Dfabric.dli.env=client " +
                            "-Dfabric.dli.main=net.fabricmc.loader.impl.launch.knot.KnotClient " +
                            "-cp \".gradle\\loom-cache\\launch.jar\" " +
                            "net.fabricmc.devlaunchinjector.Main";

            var processInfo = new ProcessStartInfo
            {
                FileName = javaPath,
                Arguments = arguments,
                WorkingDirectory = workingDir,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = processInfo };

            process.OutputDataReceived += (sender, e) => {
                if (!string.IsNullOrEmpty(e.Data))
                    Console.WriteLine("[Java stdout] " + e.Data);
            };

            process.ErrorDataReceived += (sender, e) => {
                if (!string.IsNullOrEmpty(e.Data))
                    Console.WriteLine("[Java stderr] " + e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
        }
    }
}
