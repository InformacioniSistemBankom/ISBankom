using System;
using Bankom.Class;

namespace Bankom
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.baza = new Bunifu.Framework.UI.BunifuDropdown();
            this.bazalabel = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.grupa = new Bunifu.Framework.UI.BunifuDropdown();
            this.grupalabel = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.username = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.password = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.porukalabel = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.Novalozinka = new Bunifu.Framework.UI.BunifuThinButton2();
            this.Prekid = new Bunifu.Framework.UI.BunifuThinButton2();
            this.OK = new Bunifu.Framework.UI.BunifuThinButton2();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.newpassword = new Bunifu.Framework.UI.BunifuMaterialTextbox();
            this.pictureBox2 = new Bankom.Class.CircularPictureBox();
            this.newpasswordlabel = new Bunifu.Framework.UI.BunifuCustomLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // baza
            // 
            this.baza.BackColor = System.Drawing.Color.White;
            this.baza.BorderRadius = 3;
            this.baza.ForeColor = System.Drawing.Color.White;
            this.baza.Items = new string[0];
            this.baza.Location = new System.Drawing.Point(525, 129);
            this.baza.Margin = new System.Windows.Forms.Padding(5);
            this.baza.Name = "baza";
            this.baza.NomalColor = System.Drawing.Color.LightGray;
            this.baza.onHoverColor = System.Drawing.Color.LightGray;
            this.baza.selectedIndex = -1;
            this.baza.Size = new System.Drawing.Size(377, 43);
            this.baza.TabIndex = 10;
            this.baza.Visible = false;
            this.baza.onItemSelected += new System.EventHandler(this.bazaonItemSelected);
            
            // 
            // bazalabel
            // 
            this.bazalabel.AutoSize = true;
            this.bazalabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bazalabel.Location = new System.Drawing.Point(520, 100);
            this.bazalabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.bazalabel.Name = "bazalabel";
            this.bazalabel.Size = new System.Drawing.Size(57, 25);
            this.bazalabel.TabIndex = 11;
            this.bazalabel.Text = "Baza";
            this.bazalabel.Visible = false;
            // 
            // grupa
            // 
            this.grupa.BackColor = System.Drawing.Color.White;
            this.grupa.BorderRadius = 3;
            this.grupa.ForeColor = System.Drawing.Color.White;
            this.grupa.Items = new string[0];
            this.grupa.Location = new System.Drawing.Point(525, 204);
            this.grupa.Margin = new System.Windows.Forms.Padding(5);
            this.grupa.Name = "grupa";
            this.grupa.NomalColor = System.Drawing.Color.LightGray;
            this.grupa.onHoverColor = System.Drawing.Color.LightGray;
            this.grupa.selectedIndex = -1;
            this.grupa.Size = new System.Drawing.Size(377, 43);
            this.grupa.TabIndex = 12;
            this.grupa.Visible = false;
            this.grupa.onItemSelected += new System.EventHandler(this.grupa_onItemSelected);
            // 
            // grupalabel
            // 
            this.grupalabel.AutoSize = true;
            this.grupalabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grupalabel.Location = new System.Drawing.Point(520, 175);
            this.grupalabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.grupalabel.Name = "grupalabel";
            this.grupalabel.Size = new System.Drawing.Size(66, 25);
            this.grupalabel.TabIndex = 13;
            this.grupalabel.Text = "Grupa";
            this.grupalabel.Visible = false;
            // 
            // username
            // 
            this.username.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.username.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.username.HintForeColor = System.Drawing.Color.Empty;
            this.username.HintText = "";
            this.username.isPassword = false;
            this.username.LineFocusedColor = System.Drawing.Color.LightGray;
            this.username.LineIdleColor = System.Drawing.Color.Gray;
            this.username.LineMouseHoverColor = System.Drawing.Color.Gray;
            this.username.LineThickness = 4;
            this.username.Location = new System.Drawing.Point(581, 305);
            this.username.Margin = new System.Windows.Forms.Padding(5);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(321, 43);
            this.username.TabIndex = 14;
            this.username.Text = "Korisničko ime";
            this.username.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.username.OnValueChanged += new System.EventHandler(this.username_OnValueChanged);
            this.username.TextChanged += new System.EventHandler(this.username_TextChanged);
            this.username.Enter += new System.EventHandler(this.username_Enter);
            this.username.Leave += new System.EventHandler(this.username_Leave);
            // 
            // password
            // 
            this.password.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.password.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.password.HintForeColor = System.Drawing.Color.Empty;
            this.password.HintText = "";
            this.password.isPassword = true;
            this.password.LineFocusedColor = System.Drawing.Color.LightGray;
            this.password.LineIdleColor = System.Drawing.Color.Gray;
            this.password.LineMouseHoverColor = System.Drawing.Color.Gray;
            this.password.LineThickness = 4;
            this.password.Location = new System.Drawing.Point(581, 378);
            this.password.Margin = new System.Windows.Forms.Padding(5);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(321, 43);
            this.password.TabIndex = 15;
            this.password.Text = "Lozinka";
            this.password.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.password.OnValueChanged += new System.EventHandler(this.password_OnValueChanged);
            this.password.Enter += new System.EventHandler(this.password_Enter);
            // 
            // porukalabel
            // 
            this.porukalabel.AutoSize = true;
            this.porukalabel.Location = new System.Drawing.Point(600, 450);
            this.porukalabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.porukalabel.Name = "porukalabel";
            this.porukalabel.Size = new System.Drawing.Size(0, 17);
            this.porukalabel.TabIndex = 18;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::Bankom.Properties.Resources.icons8_password_26;
            this.pictureBox4.Location = new System.Drawing.Point(521, 378);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(60, 43);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 17;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::Bankom.Properties.Resources.icons8_username_50;
            this.pictureBox3.Location = new System.Drawing.Point(521, 305);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(60, 43);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 16;
            this.pictureBox3.TabStop = false;
            // 
            // Novalozinka
            // 
            this.Novalozinka.ActiveBorderThickness = 1;
            this.Novalozinka.ActiveCornerRadius = 20;
            this.Novalozinka.ActiveFillColor = System.Drawing.SystemColors.MenuHighlight;
            this.Novalozinka.ActiveForecolor = System.Drawing.Color.White;
            this.Novalozinka.ActiveLineColor = System.Drawing.SystemColors.MenuHighlight;
            this.Novalozinka.BackColor = System.Drawing.SystemColors.Control;
            this.Novalozinka.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Novalozinka.BackgroundImage")));
            this.Novalozinka.ButtonText = "Nova lozinka";
            this.Novalozinka.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Novalozinka.Font = new System.Drawing.Font("Century Gothic", 11F);
            this.Novalozinka.ForeColor = System.Drawing.Color.SeaGreen;
            this.Novalozinka.IdleBorderThickness = 1;
            this.Novalozinka.IdleCornerRadius = 20;
            this.Novalozinka.IdleFillColor = System.Drawing.SystemColors.Highlight;
            this.Novalozinka.IdleForecolor = System.Drawing.Color.White;
            this.Novalozinka.IdleLineColor = System.Drawing.SystemColors.Highlight;
            this.Novalozinka.Location = new System.Drawing.Point(755, 486);
            this.Novalozinka.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Novalozinka.Name = "Novalozinka";
            this.Novalozinka.Size = new System.Drawing.Size(148, 48);
            this.Novalozinka.TabIndex = 6;
            this.Novalozinka.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Novalozinka.Click += new System.EventHandler(this.Novalozinka_Click);
            // 
            // Prekid
            // 
            this.Prekid.ActiveBorderThickness = 1;
            this.Prekid.ActiveCornerRadius = 20;
            this.Prekid.ActiveFillColor = System.Drawing.SystemColors.MenuHighlight;
            this.Prekid.ActiveForecolor = System.Drawing.Color.White;
            this.Prekid.ActiveLineColor = System.Drawing.SystemColors.MenuHighlight;
            this.Prekid.BackColor = System.Drawing.SystemColors.Control;
            this.Prekid.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Prekid.BackgroundImage")));
            this.Prekid.ButtonText = "Prekid";
            this.Prekid.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Prekid.Font = new System.Drawing.Font("Century Gothic", 11F);
            this.Prekid.ForeColor = System.Drawing.Color.SeaGreen;
            this.Prekid.IdleBorderThickness = 1;
            this.Prekid.IdleCornerRadius = 20;
            this.Prekid.IdleFillColor = System.Drawing.SystemColors.Highlight;
            this.Prekid.IdleForecolor = System.Drawing.Color.White;
            this.Prekid.IdleLineColor = System.Drawing.SystemColors.Highlight;
            this.Prekid.Location = new System.Drawing.Point(625, 486);
            this.Prekid.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Prekid.Name = "Prekid";
            this.Prekid.Size = new System.Drawing.Size(120, 48);
            this.Prekid.TabIndex = 5;
            this.Prekid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Prekid.Click += new System.EventHandler(this.Prekid_Click);
            // 
            // OK
            // 
            this.OK.ActiveBorderThickness = 1;
            this.OK.ActiveCornerRadius = 20;
            this.OK.ActiveFillColor = System.Drawing.SystemColors.MenuHighlight;
            this.OK.ActiveForecolor = System.Drawing.Color.White;
            this.OK.ActiveLineColor = System.Drawing.SystemColors.MenuHighlight;
            this.OK.BackColor = System.Drawing.SystemColors.Control;
            this.OK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("OK.BackgroundImage")));
            this.OK.ButtonText = "OK";
            this.OK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OK.Font = new System.Drawing.Font("Century Gothic", 11F);
            this.OK.ForeColor = System.Drawing.Color.SeaGreen;
            this.OK.IdleBorderThickness = 1;
            this.OK.IdleCornerRadius = 20;
            this.OK.IdleFillColor = System.Drawing.SystemColors.Highlight;
            this.OK.IdleForecolor = System.Drawing.Color.White;
            this.OK.IdleLineColor = System.Drawing.SystemColors.Highlight;
            this.OK.Location = new System.Drawing.Point(499, 486);
            this.OK.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(120, 48);
            this.OK.TabIndex = 2;
            this.OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Bankom.Properties.Resources.BankomLogin;
            this.pictureBox1.Location = new System.Drawing.Point(1, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(599, 553);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::Bankom.Properties.Resources.icons8_password_26;
            this.pictureBox5.Location = new System.Drawing.Point(521, 433);
            this.pictureBox5.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(60, 43);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 20;
            this.pictureBox5.TabStop = false;
            // 
            // newpassword
            // 
            this.newpassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.newpassword.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newpassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.newpassword.HintForeColor = System.Drawing.Color.Empty;
            this.newpassword.HintText = "";
            this.newpassword.isPassword = true;
            this.newpassword.LineFocusedColor = System.Drawing.Color.LightGray;
            this.newpassword.LineIdleColor = System.Drawing.Color.Gray;
            this.newpassword.LineMouseHoverColor = System.Drawing.Color.Gray;
            this.newpassword.LineThickness = 4;
            this.newpassword.Location = new System.Drawing.Point(581, 433);
            this.newpassword.Margin = new System.Windows.Forms.Padding(5);
            this.newpassword.Name = "newpassword";
            this.newpassword.Size = new System.Drawing.Size(321, 43);
            this.newpassword.TabIndex = 21;
            this.newpassword.Text = "Lozinka";
            this.newpassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.newpassword.OnValueChanged += new System.EventHandler(this.newpassword_OnValueChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Bankom.Properties.Resources.login;
            this.pictureBox2.Location = new System.Drawing.Point(659, 15);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(137, 107);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // newpasswordlabel
            // 
            this.newpasswordlabel.AutoSize = true;
            this.newpasswordlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newpasswordlabel.Location = new System.Drawing.Point(529, 262);
            this.newpasswordlabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.newpasswordlabel.Name = "newpasswordlabel";
            this.newpasswordlabel.Size = new System.Drawing.Size(337, 25);
            this.newpasswordlabel.TabIndex = 22;
            this.newpasswordlabel.Text = "Unesite novu lozinku i potvrdu lozinke";
            this.newpasswordlabel.Visible = false;
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 554);
            this.Controls.Add(this.newpasswordlabel);
            this.Controls.Add(this.newpassword);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.porukalabel);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.grupalabel);
            this.Controls.Add(this.grupa);
            this.Controls.Add(this.bazalabel);
            this.Controls.Add(this.baza);
            this.Controls.Add(this.Novalozinka);
            this.Controls.Add(this.Prekid);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLogin";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private Bunifu.Framework.UI.BunifuThinButton2 OK;
        private Bunifu.Framework.UI.BunifuThinButton2 Prekid;
        private Bunifu.Framework.UI.BunifuThinButton2 Novalozinka;
        private Bunifu.Framework.UI.BunifuDropdown baza;
        private Bunifu.Framework.UI.BunifuCustomLabel bazalabel;
        private Bunifu.Framework.UI.BunifuDropdown grupa;
        private Bunifu.Framework.UI.BunifuCustomLabel grupalabel;
        private Bunifu.Framework.UI.BunifuMaterialTextbox username;
        private Bunifu.Framework.UI.BunifuMaterialTextbox password;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private CircularPictureBox pictureBox2;
        private Bunifu.Framework.UI.BunifuCustomLabel porukalabel;
        private System.Windows.Forms.PictureBox pictureBox5;
        private Bunifu.Framework.UI.BunifuMaterialTextbox newpassword;
        private Bunifu.Framework.UI.BunifuCustomLabel newpasswordlabel;
    }
}