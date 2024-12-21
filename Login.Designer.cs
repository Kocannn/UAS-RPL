namespace UAS_RPL
{
    partial class Login
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
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.inputPassword = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.inputNama = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.radioAdmin = new MetroFramework.Controls.MetroRadioButton();
            this.radioUser = new MetroFramework.Controls.MetroRadioButton();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.SuspendLayout();
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(151, 186);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(121, 44);
            this.metroButton1.TabIndex = 34;
            this.metroButton1.Text = "Login";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // inputPassword
            // 
            // 
            // 
            // 
            this.inputPassword.CustomButton.Image = null;
            this.inputPassword.CustomButton.Location = new System.Drawing.Point(137, 1);
            this.inputPassword.CustomButton.Name = "";
            this.inputPassword.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.inputPassword.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.inputPassword.CustomButton.TabIndex = 1;
            this.inputPassword.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.inputPassword.CustomButton.UseSelectable = true;
            this.inputPassword.CustomButton.Visible = false;
            this.inputPassword.Lines = new string[0];
            this.inputPassword.Location = new System.Drawing.Point(133, 109);
            this.inputPassword.MaxLength = 32767;
            this.inputPassword.Name = "inputPassword";
            this.inputPassword.PasswordChar = '●';
            this.inputPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.inputPassword.SelectedText = "";
            this.inputPassword.SelectionLength = 0;
            this.inputPassword.SelectionStart = 0;
            this.inputPassword.ShortcutsEnabled = true;
            this.inputPassword.Size = new System.Drawing.Size(159, 23);
            this.inputPassword.TabIndex = 31;
            this.inputPassword.UseSelectable = true;
            this.inputPassword.UseSystemPasswordChar = true;
            this.inputPassword.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.inputPassword.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(180, 87);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(63, 19);
            this.metroLabel3.TabIndex = 30;
            this.metroLabel3.Text = "Password";
            // 
            // inputNama
            // 
            // 
            // 
            // 
            this.inputNama.CustomButton.Image = null;
            this.inputNama.CustomButton.Location = new System.Drawing.Point(137, 1);
            this.inputNama.CustomButton.Name = "";
            this.inputNama.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.inputNama.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.inputNama.CustomButton.TabIndex = 1;
            this.inputNama.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.inputNama.CustomButton.UseSelectable = true;
            this.inputNama.CustomButton.Visible = false;
            this.inputNama.Lines = new string[0];
            this.inputNama.Location = new System.Drawing.Point(133, 51);
            this.inputNama.MaxLength = 32767;
            this.inputNama.Name = "inputNama";
            this.inputNama.PasswordChar = '\0';
            this.inputNama.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.inputNama.SelectedText = "";
            this.inputNama.SelectionLength = 0;
            this.inputNama.SelectionStart = 0;
            this.inputNama.ShortcutsEnabled = true;
            this.inputNama.Size = new System.Drawing.Size(159, 23);
            this.inputNama.TabIndex = 27;
            this.inputNama.UseSelectable = true;
            this.inputNama.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.inputNama.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.BackColor = System.Drawing.Color.Transparent;
            this.metroLabel1.ForeColor = System.Drawing.SystemColors.Control;
            this.metroLabel1.Location = new System.Drawing.Point(189, 29);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(45, 19);
            this.metroLabel1.TabIndex = 26;
            this.metroLabel1.Text = "Nama";
            // 
            // radioAdmin
            // 
            this.radioAdmin.AutoSize = true;
            this.radioAdmin.Location = new System.Drawing.Point(134, 148);
            this.radioAdmin.Name = "radioAdmin";
            this.radioAdmin.Size = new System.Drawing.Size(59, 15);
            this.radioAdmin.TabIndex = 25;
            this.radioAdmin.Text = "Admin";
            this.radioAdmin.UseSelectable = true;
            // 
            // radioUser
            // 
            this.radioUser.AutoSize = true;
            this.radioUser.Location = new System.Drawing.Point(244, 148);
            this.radioUser.Name = "radioUser";
            this.radioUser.Size = new System.Drawing.Size(46, 15);
            this.radioUser.TabIndex = 24;
            this.radioUser.Text = "User";
            this.radioUser.UseSelectable = true;
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(99, 247);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(218, 19);
            this.metroLabel2.TabIndex = 35;
            this.metroLabel2.Text = "Belum punya akun? Daftar sekarang";
            this.metroLabel2.Click += new System.EventHandler(this.metroLabel2_Click);
            // 
            // Login
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(427, 385);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.inputPassword);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.inputNama);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.radioAdmin);
            this.Controls.Add(this.radioUser);
            this.Name = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroTextBox inputPassword;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroTextBox inputNama;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroRadioButton radioAdmin;
        private MetroFramework.Controls.MetroRadioButton radioUser;
        private MetroFramework.Controls.MetroLabel metroLabel2;
    }
        #endregion
}