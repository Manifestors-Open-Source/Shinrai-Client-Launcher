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
    public partial class StarterApp : Form
    {
        public StarterApp()
        {
            InitializeComponent();
        }

        private void StarterApp_Load(object sender, EventArgs e)
        {
            Thread.Sleep(5000);
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }
    }
}
