namespace Bankom
{
    partial class LoginForm
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
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
            this.cmbBaze = new System.Windows.Forms.ComboBox();
            this.CmbOrg = new System.Windows.Forms.ComboBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.Button1 = new System.Windows.Forms.Button();
            this.BtnPrekid = new System.Windows.Forms.Button();
            this.OK = new System.Windows.Forms.Button();
            this.lblBaza = new System.Windows.Forms.Label();
            this.lblGrupa = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPotvrda = new System.Windows.Forms.TextBox();
            this.lblPotvrda = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.Image = global::Bankom.Properties.Resources.logo;
            this.LogoPictureBox.Location = new System.Drawing.Point(21, 142);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.LogoPictureBox.Size = new System.Drawing.Size(275, 158);
            this.LogoPictureBox.TabIndex = 10;
            this.LogoPictureBox.TabStop = false;
            // 
            // cmbBaze
            // 
            this.cmbBaze.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cmbBaze.FormattingEnabled = true;
            this.cmbBaze.Location = new System.Drawing.Point(348, 28);
            this.cmbBaze.Name = "cmbBaze";
            this.cmbBaze.Size = new System.Drawing.Size(294, 23);
            this.cmbBaze.TabIndex = 18;
            this.cmbBaze.Visible = false;
            this.cmbBaze.SelectedIndexChanged += new System.EventHandler(this.ComboBox2_SelectedIndexChanged);
            this.cmbBaze.TextChanged += new System.EventHandler(this.ComboBox2_TextChanged);
            
            // 
            // CmbOrg
            // 
            this.CmbOrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.CmbOrg.FormattingEnabled = true;
            this.CmbOrg.Location = new System.Drawing.Point(348, 74);
            this.CmbOrg.Name = "CmbOrg";
            this.CmbOrg.Size = new System.Drawing.Size(294, 23);
            this.CmbOrg.TabIndex = 17;
            this.CmbOrg.Visible = false;
            this.CmbOrg.SelectedIndexChanged += new System.EventHandler(this.CmbBaze_SelectedIndexChanged);
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordTextBox.Location = new System.Drawing.Point(348, 191);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(294, 21);
            this.PasswordTextBox.TabIndex = 14;
            this.PasswordTextBox.WordWrap = false;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameTextBox.Location = new System.Drawing.Point(348, 142);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(294, 21);
            this.UsernameTextBox.TabIndex = 12;
            this.UsernameTextBox.TextChanged += new System.EventHandler(this.UsernameTextBox_TextChanged);
            this.UsernameTextBox.LostFocus += new System.EventHandler(this.UsernameTextBox_LostFocus);
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.PasswordLabel.Location = new System.Drawing.Point(345, 165);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(220, 23);
            this.PasswordLabel.TabIndex = 13;
            this.PasswordLabel.Text = "&Lozinka";
            this.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PasswordLabel.Click += new System.EventHandler(this.PasswordLabel_Click);
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.UsernameLabel.Location = new System.Drawing.Point(345, 116);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(220, 23);
            this.UsernameLabel.TabIndex = 11;
            this.UsernameLabel.Text = "&Korisnik";
            this.UsernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.UsernameLabel.Click += new System.EventHandler(this.UsernameLabel_Click);
            // 
            // Button1
            // 
            this.Button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Button1.ForeColor = System.Drawing.Color.Black;
            this.Button1.Location = new System.Drawing.Point(548, 278);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(94, 23);
            this.Button1.TabIndex = 21;
            this.Button1.Text = "Nova lozinka";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // BtnPrekid
            // 
            this.BtnPrekid.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnPrekid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.BtnPrekid.ForeColor = System.Drawing.Color.Black;
            this.BtnPrekid.Location = new System.Drawing.Point(448, 278);
            this.BtnPrekid.Name = "BtnPrekid";
            this.BtnPrekid.Size = new System.Drawing.Size(94, 23);
            this.BtnPrekid.TabIndex = 20;
            this.BtnPrekid.Text = "&Prekid";
            this.BtnPrekid.Click += new System.EventHandler(this.BtnPrekid_Click);
            // 
            // OK
            // 
            this.OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.OK.ForeColor = System.Drawing.Color.Black;
            this.OK.Location = new System.Drawing.Point(348, 277);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(94, 23);
            this.OK.TabIndex = 19;
            this.OK.Text = "OK";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // lblBaza
            // 
            this.lblBaza.AutoSize = true;
            this.lblBaza.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBaza.Location = new System.Drawing.Point(348, 9);
            this.lblBaza.Name = "lblBaza";
            this.lblBaza.Size = new System.Drawing.Size(39, 15);
            this.lblBaza.TabIndex = 22;
            this.lblBaza.Text = "Baza";
            this.lblBaza.Visible = false;
            this.lblBaza.Click += new System.EventHandler(this.lblBaza_Click);
            // 
            // lblGrupa
            // 
            this.lblGrupa.AutoSize = true;
            this.lblGrupa.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrupa.Location = new System.Drawing.Point(348, 55);
            this.lblGrupa.Name = "lblGrupa";
            this.lblGrupa.Size = new System.Drawing.Size(46, 15);
            this.lblGrupa.TabIndex = 23;
            this.lblGrupa.Text = "Grupa";
            this.lblGrupa.Visible = false;
            this.lblGrupa.Click += new System.EventHandler(this.lblGrupa_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(348, 297);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 17);
            this.label1.TabIndex = 24;
            // 
            // txtPotvrda
            // 
            this.txtPotvrda.Location = new System.Drawing.Point(348, 239);
            this.txtPotvrda.Name = "txtPotvrda";
            this.txtPotvrda.Size = new System.Drawing.Size(294, 20);
            this.txtPotvrda.TabIndex = 25;
            this.txtPotvrda.Visible = false;
            // 
            // lblPotvrda
            // 
            this.lblPotvrda.AutoSize = true;
            this.lblPotvrda.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblPotvrda.Location = new System.Drawing.Point(348, 220);
            this.lblPotvrda.Name = "lblPotvrda";
            this.lblPotvrda.Size = new System.Drawing.Size(55, 15);
            this.lblPotvrda.TabIndex = 26;
            this.lblPotvrda.Text = "Potvrda";
            this.lblPotvrda.Visible = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 348);
            this.Controls.Add(this.lblPotvrda);
            this.Controls.Add(this.txtPotvrda);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblGrupa);
            this.Controls.Add(this.lblBaza);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.BtnPrekid);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.cmbBaze);
            this.Controls.Add(this.CmbOrg);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.UsernameTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.LogoPictureBox);
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prijava";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.PictureBox LogoPictureBox;
        internal System.Windows.Forms.ComboBox cmbBaze;
        internal System.Windows.Forms.ComboBox CmbOrg;
        internal System.Windows.Forms.TextBox PasswordTextBox;
        internal System.Windows.Forms.TextBox UsernameTextBox;
        internal System.Windows.Forms.Label PasswordLabel;
        internal System.Windows.Forms.Label UsernameLabel;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.Button BtnPrekid;
        internal System.Windows.Forms.Button OK;
        private System.Windows.Forms.Label lblBaza;
        private System.Windows.Forms.Label lblGrupa;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPotvrda;
        private System.Windows.Forms.Label lblPotvrda;
    }
}