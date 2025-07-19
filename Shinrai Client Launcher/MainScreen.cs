using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            guna2ComboBox1.SelectedItem = "1.21.7";
            AddProfilesToCombobox();
            labelUsername.Text = Form1.SelectedAccountName;
            labelAccounttype.Text = Form1.SelectedAccountType.ToString();
            ProfilPictureBox.Image = LoadSkin(Form1.SelectedAccountName);
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
    }
}
