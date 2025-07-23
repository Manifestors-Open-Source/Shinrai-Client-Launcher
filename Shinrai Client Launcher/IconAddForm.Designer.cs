namespace Shinrai_Client_Launcher
{
    partial class IconAddForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IconAddForm));
            guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            pictureBox1 = new PictureBox();
            guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            btnSelectApath = new Guna.UI2.WinForms.Guna2Button();
            btnConvertToIcon = new Guna.UI2.WinForms.Guna2Button();
            txtNewIconName = new Label();
            txtOrginalPng = new Label();
            guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            txtNewIcon = new Label();
            txtIconName = new Guna.UI2.WinForms.Guna2TextBox();
            label8 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            guna2Panel2.SuspendLayout();
            SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            guna2BorderlessForm1.BorderRadius = 15;
            guna2BorderlessForm1.ContainerControl = this;
            guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImage = Properties.Resources.LogoVersion2;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Location = new Point(49, 21);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(225, 95);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // guna2Panel2
            // 
            guna2Panel2.BackColor = Color.FromArgb(0, 0, 0, 100);
            guna2Panel2.BorderRadius = 15;
            guna2Panel2.Controls.Add(btnSelectApath);
            guna2Panel2.Controls.Add(btnConvertToIcon);
            guna2Panel2.Controls.Add(txtNewIconName);
            guna2Panel2.Controls.Add(txtOrginalPng);
            guna2Panel2.Controls.Add(guna2Panel3);
            guna2Panel2.Controls.Add(txtNewIcon);
            guna2Panel2.Controls.Add(txtIconName);
            guna2Panel2.CustomizableEdges = customizableEdges19;
            guna2Panel2.FillColor = Color.FromArgb(100, 0, 0, 0);
            guna2Panel2.Location = new Point(12, 131);
            guna2Panel2.Name = "guna2Panel2";
            guna2Panel2.ShadowDecoration.CustomizableEdges = customizableEdges20;
            guna2Panel2.Size = new Size(314, 367);
            guna2Panel2.TabIndex = 30;
            // 
            // btnSelectApath
            // 
            btnSelectApath.BorderRadius = 5;
            btnSelectApath.CustomizableEdges = customizableEdges11;
            btnSelectApath.DisabledState.BorderColor = Color.DarkGray;
            btnSelectApath.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSelectApath.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSelectApath.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSelectApath.FillColor = Color.FromArgb(100, 0, 0, 0);
            btnSelectApath.Font = new Font("Comfortaa", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 162);
            btnSelectApath.ForeColor = Color.White;
            btnSelectApath.Location = new Point(23, 204);
            btnSelectApath.Name = "btnSelectApath";
            btnSelectApath.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnSelectApath.Size = new Size(266, 45);
            btnSelectApath.TabIndex = 28;
            btnSelectApath.Text = "Select A File";
            btnSelectApath.Click += btnCreateFolder_Click;
            // 
            // btnConvertToIcon
            // 
            btnConvertToIcon.BorderRadius = 5;
            btnConvertToIcon.CustomizableEdges = customizableEdges13;
            btnConvertToIcon.DisabledState.BorderColor = Color.DarkGray;
            btnConvertToIcon.DisabledState.CustomBorderColor = Color.DarkGray;
            btnConvertToIcon.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnConvertToIcon.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnConvertToIcon.FillColor = Color.FromArgb(100, 0, 0, 0);
            btnConvertToIcon.Font = new Font("Comfortaa", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 162);
            btnConvertToIcon.ForeColor = Color.White;
            btnConvertToIcon.Location = new Point(23, 298);
            btnConvertToIcon.Name = "btnConvertToIcon";
            btnConvertToIcon.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnConvertToIcon.Size = new Size(266, 45);
            btnConvertToIcon.TabIndex = 27;
            btnConvertToIcon.Text = "Convert to Icon";
            btnConvertToIcon.Click += btnConvertToIcon_Click;
            // 
            // txtNewIconName
            // 
            txtNewIconName.Font = new Font("Comfortaa", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 162);
            txtNewIconName.ForeColor = Color.White;
            txtNewIconName.Location = new Point(0, 73);
            txtNewIconName.Name = "txtNewIconName";
            txtNewIconName.Size = new Size(317, 29);
            txtNewIconName.TabIndex = 17;
            txtNewIconName.Text = "Icon Name";
            txtNewIconName.TextAlign = ContentAlignment.BottomCenter;
            // 
            // txtOrginalPng
            // 
            txtOrginalPng.Font = new Font("Comfortaa", 8.999999F, FontStyle.Bold, GraphicsUnit.Point, 162);
            txtOrginalPng.ForeColor = Color.White;
            txtOrginalPng.Location = new Point(3, 180);
            txtOrginalPng.Name = "txtOrginalPng";
            txtOrginalPng.Size = new Size(314, 21);
            txtOrginalPng.TabIndex = 15;
            txtOrginalPng.Text = "Orginal PNG Format Path";
            txtOrginalPng.TextAlign = ContentAlignment.BottomCenter;
            // 
            // guna2Panel3
            // 
            guna2Panel3.BorderRadius = 5;
            guna2Panel3.CustomizableEdges = customizableEdges15;
            guna2Panel3.FillColor = Color.FromArgb(188, 0, 45);
            guna2Panel3.Location = new Point(112, 48);
            guna2Panel3.Name = "guna2Panel3";
            guna2Panel3.ShadowDecoration.CustomizableEdges = customizableEdges16;
            guna2Panel3.Size = new Size(90, 10);
            guna2Panel3.TabIndex = 17;
            // 
            // txtNewIcon
            // 
            txtNewIcon.Font = new Font("Comfortaa", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            txtNewIcon.ForeColor = Color.White;
            txtNewIcon.Location = new Point(0, 13);
            txtNewIcon.Name = "txtNewIcon";
            txtNewIcon.Size = new Size(314, 29);
            txtNewIcon.TabIndex = 16;
            txtNewIcon.Text = "txtNewIcon";
            txtNewIcon.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtIconName
            // 
            txtIconName.BorderColor = Color.FromArgb(188, 0, 45);
            txtIconName.BorderRadius = 15;
            txtIconName.BorderThickness = 3;
            txtIconName.CustomizableEdges = customizableEdges17;
            txtIconName.DefaultText = "";
            txtIconName.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtIconName.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtIconName.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtIconName.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtIconName.FillColor = Color.FromArgb(200, 0, 45);
            txtIconName.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtIconName.Font = new Font("Product Sans", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            txtIconName.ForeColor = Color.White;
            txtIconName.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtIconName.IconLeftOffset = new Point(1, 0);
            txtIconName.IconLeftSize = new Size(25, 25);
            txtIconName.IconRightSize = new Size(35, 35);
            txtIconName.Location = new Point(23, 107);
            txtIconName.Margin = new Padding(3, 5, 3, 5);
            txtIconName.Name = "txtIconName";
            txtIconName.PlaceholderForeColor = Color.Gainsboro;
            txtIconName.PlaceholderText = "Icon Name";
            txtIconName.SelectedText = "";
            txtIconName.ShadowDecoration.CustomizableEdges = customizableEdges18;
            txtIconName.Size = new Size(266, 34);
            txtIconName.TabIndex = 26;
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Comfortaa", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label8.ForeColor = Color.White;
            label8.Location = new Point(304, 9);
            label8.Name = "label8";
            label8.Size = new Size(23, 29);
            label8.TabIndex = 32;
            label8.Text = "X";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            label8.Click += label8_Click;
            // 
            // IconAddForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BackgroundImage1;
            ClientSize = new Size(339, 510);
            Controls.Add(label8);
            Controls.Add(guna2Panel2);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "IconAddForm";
            Text = "IconAddForm";
            Load += IconAddForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            guna2Panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2Button btnSelectApath;
        private Guna.UI2.WinForms.Guna2Button btnConvertToIcon;
        private Label txtNewIconName;
        private Label txtOrginalPng;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private Label txtNewIcon;
        private Guna.UI2.WinForms.Guna2TextBox txtIconName;
        private Label label8;
    }
}