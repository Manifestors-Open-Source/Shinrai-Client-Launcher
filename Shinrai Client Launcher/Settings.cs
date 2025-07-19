using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;
using Shinrai_Client_Launcher.ProfileSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shinrai_Client_Launcher
{
    public partial class Settings : Form
    {
        ProfileSystem.Profile SelectedProfile = new ProfileSystem.Profile();
        public enum Lang
        {
            Turkish,
            English,
            German,
            Japanese
        }
        public Settings()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }


        void AddItemsToComboBox()
        {
            AddProfilesToCombobox();
            AddRamSettingsToComboBox();
            AddResoulationToComboBox();
            AddLanguageToCombobox();
            AddIconsToCombobox();
        }

        public System.Drawing.Image LoadManifestorspicturefile(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            using (MemoryStream ms = new MemoryStream(data))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }

        ulong GetDefaultRam()
        {
            ComputerInfo ci = new ComputerInfo();
            ulong totalMemory = ci.TotalPhysicalMemory;
            ulong totalMemoryMB = totalMemory / (1024 * 1024);
            return totalMemoryMB;
        }
        void AddLanguageToCombobox()
        {
            List<Lang> langs = new List<Lang>() { Lang.Japanese, Lang.English, Lang.German, Lang.Turkish };
            foreach (Lang lang in langs)
            {
                cmbLanguage.Items.Add(lang);
            }
            cmbLanguage.SelectedItem = Lang.English;
        }
        void AddRamSettingsToComboBox()
        {

            List<int> Rams = new List<int>() {  1 * 1024,
    2 * 1024,
    4 * 1024,
    8 * 1024,
    16 * 1024,
    32 * 1024};
            foreach (int a in Rams)
            {
                if ((ulong)a < GetDefaultRam())
                {
                    cmbRam.Items.Add(a);
                }
            }
        }

        void AddProfilesToCombobox()
        {

            string klasorYolu = @"profiles";
            string[] dosyalar = Directory.GetFiles(klasorYolu, "*.shinrai");

            foreach (string dosyaYolu in dosyalar)
            {
                string dosyaAdi = Path.GetFileName(dosyaYolu);
                cmbPrfl.Items.Add(dosyaAdi);
            }

        }


        void AddResoulationToComboBox()
        {
            List<string> resolutions = new List<string>()
{
    "800x600",
    "1024x768",
    "1280x720",
    "1280x800",
    "1366x768",
    "1440x900",
    "1600x900",
    "1680x1050",
    "1920x1080",
    "1920x1200",
    "2560x1080",
    "2560x1440",
    "3200x1800",
    "3440x1440",
    "3840x2160",
    "7680x4320"
};

            foreach (string resolution in resolutions)
            {
                cmbResolution.Items.Add(resolution);
            }
            cmbResolution.SelectedItem = "1920x1080";
        }

        private void guna2ComboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProfileIcon.SelectedItem == "Add New Icon")
            {
                IconAddForm ıconAddForm = new IconAddForm();
                ıconAddForm.Show();
                this.Hide();
            }
            else { 
            pbxProfileIcn.BackgroundImage = LoadManifestorspicturefile(System.Windows.Forms.Application.StartupPath + @"\profiles\icons\" + cmbProfileIcon.Text);
            }
        }
        void AddIconsToCombobox()
        {

            string klasorYolu = @"profiles/icons";
            string[] dosyalar = Directory.GetFiles(klasorYolu, "*.mpf");

            foreach (string dosyaYolu in dosyalar)
            {
                string dosyaAdi = Path.GetFileName(dosyaYolu);
                cmbProfileIcon.Items.Add(dosyaAdi);
            }
            cmbProfileIcon.Items.Add("Add New Icon");

        }
        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void btnCreateFolder_Click(object sender, EventArgs e)
        {
            if (cmbLanguage.Text == string.Empty || cmbRam.Text == string.Empty || txtIPAdress.Text == string.Empty || cmbResolution.Text == string.Empty || txtProfileName.Text == string.Empty || cmbProfileIcon.Text == string.Empty)
            {
                MessageBox.Show("Ayarlar Boş Olamaz");
            }
            else
            {
                if (txtProfileName.Text != lblProfileName.Text + ".shinrai")
                {
                    string langStr = cmbLanguage.Text;
                    LSettings.Lang langValue = (LSettings.Lang)Enum.Parse(typeof(LSettings.Lang), langStr);
                    ProfileSystem.LSettings SSettings = new ProfileSystem.LSettings()
                    {
                        Ram = Convert.ToInt16(cmbRam.Text),
                        Resolution = cmbResolution.Text,
                        FullScreen = tswFullScreen.Checked,
                        AutoIP = tswIPState.Checked,
                        AutoIPAdresS = txtIPAdress.Text,
                        Langu = langValue,


                    };
                    ProfileSystem.Profile Sprofile = new ProfileSystem.Profile()
                    {
                        Name = txtProfileName.Text,
                        Icon = cmbProfileIcon.Text,
                        Settingss = SSettings
                    };
                    string jsonString = JsonConvert.SerializeObject(Sprofile, Formatting.Indented);
                    string jsonFilePath = @"profiles\" + txtProfileName.Text + ".shinrai";
                    File.WriteAllText(jsonFilePath, jsonString);
                    MessageBox.Show("Profil Başarı ile Oluşturuldu.");
                }
                else
                {
                    string langStr = cmbLanguage.Text;
                    LSettings.Lang langValue = (LSettings.Lang)Enum.Parse(typeof(LSettings.Lang), langStr);
                    ProfileSystem.LSettings SSettings = new ProfileSystem.LSettings()
                    {
                        Ram = Convert.ToInt16(cmbRam.Text),
                        Resolution = cmbResolution.Text,
                        FullScreen = tswFullScreen.Checked,
                        AutoIP = tswIPState.Checked,
                        AutoIPAdresS = txtIPAdress.Text,
                        Langu = langValue,


                    };
                    ProfileSystem.Profile Sprofile = new ProfileSystem.Profile()
                    {
                        Name = txtProfileName.Text,
                        Icon = cmbProfileIcon.Text,
                        Settingss = SSettings
                    };
                    string jsonString = JsonConvert.SerializeObject(Sprofile, Formatting.Indented);
                    string jsonFilePath = @"profiles\" + lblProfileName.Text + ".shinrai";
                    File.WriteAllText(jsonFilePath, jsonString);
                    MessageBox.Show("Profil Başarı ile kaydedildi.");
                }
            }
        }

        private void cmbRam_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Settings_Load(object sender, EventArgs e)
        {
            AddItemsToComboBox();
        }

        private void cmbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbPrfl_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblProfileName.Text = cmbPrfl.Text.Replace(".shinrai", string.Empty);
            string jsonContent = File.ReadAllText(@"profiles\" + cmbPrfl.SelectedItem);


            Profile profile = JsonConvert.DeserializeObject<Profile>(jsonContent);
            SelectedProfile = profile;
            LoadInformationOfProfile();
        }

        void LoadInformationOfProfile()
        {
            txtProfileName.Text = SelectedProfile.Name;
            cmbProfileIcon.Text = SelectedProfile.Icon;
            pbxProfileIcn.BackgroundImage = LoadManifestorspicturefile(System.Windows.Forms.Application.StartupPath + @"\profiles\icons\" + SelectedProfile.Icon);
            cmbRam.SelectedItem = SelectedProfile.Settingss.Ram;
            cmbResolution.SelectedItem = SelectedProfile.Settingss.Resolution;
            txtIPAdress.Text = SelectedProfile.Settingss.AutoIPAdresS;
            cmbLanguage.SelectedItem = SelectedProfile.Settingss.Langu;
            tswFullScreen.Checked = SelectedProfile.Settingss.FullScreen;
            tswIPState.Checked = SelectedProfile.Settingss.AutoIP;
        }

        private void txtProfileName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainScreen mainScreen = new MainScreen();
            mainScreen.Show();
        }
    }
}
