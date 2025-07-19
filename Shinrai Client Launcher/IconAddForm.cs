using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shinrai_Client_Launcher
{
    public partial class IconAddForm : Form
    {
        string selectedPath;
        public IconAddForm()
        {
            InitializeComponent();
        }

        private void btnCreateFolder_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Bir dosya seçin";
            ofd.Filter = "PNG Files (*.png)|*.png";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                selectedPath = ofd.FileName;
                btnSelectApath.Text = selectedPath;

            }
        }

        public static void ConvertPngToMpf(string pngPath, string mpfPath)
        {
            using (Image img = Image.FromFile(pngPath))
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                File.WriteAllBytes(mpfPath, ms.ToArray());
            }
        }

        private void btnConvertToIcon_Click(object sender, EventArgs e)
        {
            ConvertPngToMpf(selectedPath, Application.StartupPath + @"\profiles\icons\" + txtIconName.Text + ".mpf");
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
            this.Close();

        }

        private void IconAddForm_Load(object sender, EventArgs e)
        {

        }
    }
}
