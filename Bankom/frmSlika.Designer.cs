﻿
namespace Bankom
{
    partial class frmSlika
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSlika));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbSkl = new System.Windows.Forms.RadioButton();
            this.rbMagPolje = new System.Windows.Forms.RadioButton();
            this.rbArtikli = new System.Windows.Forms.RadioButton();
            this.cmbNazivSlike = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(37, 148);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(642, 412);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox1_DragDrop);
            this.pictureBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBox1_DragEnter);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("TimesRoman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.label1.Location = new System.Drawing.Point(226, 330);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Prevucite sliku ovde...";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Snow;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("TimesRoman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.button1.Location = new System.Drawing.Point(494, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "Snimi";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSkl);
            this.groupBox1.Controls.Add(this.rbMagPolje);
            this.groupBox1.Controls.Add(this.rbArtikli);
            this.groupBox1.Font = new System.Drawing.Font("TimesRoman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.groupBox1.Location = new System.Drawing.Point(37, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(182, 94);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Izaberite kategoriju slike:";
            // 
            // rbSkl
            // 
            this.rbSkl.AutoSize = true;
            this.rbSkl.Location = new System.Drawing.Point(6, 65);
            this.rbSkl.Name = "rbSkl";
            this.rbSkl.Size = new System.Drawing.Size(98, 23);
            this.rbSkl.TabIndex = 2;
            this.rbSkl.TabStop = true;
            this.rbSkl.Tag = "Skladiste";
            this.rbSkl.Text = "Skladi{te";
            this.rbSkl.UseVisualStyleBackColor = true;
            // 
            // rbMagPolje
            // 
            this.rbMagPolje.AutoSize = true;
            this.rbMagPolje.Location = new System.Drawing.Point(6, 42);
            this.rbMagPolje.Name = "rbMagPolje";
            this.rbMagPolje.Size = new System.Drawing.Size(164, 23);
            this.rbMagPolje.TabIndex = 1;
            this.rbMagPolje.TabStop = true;
            this.rbMagPolje.Tag = "MagacinskaPolja";
            this.rbMagPolje.Text = "Magacinsko polje";
            this.rbMagPolje.UseVisualStyleBackColor = true;
            // 
            // rbArtikli
            // 
            this.rbArtikli.AutoSize = true;
            this.rbArtikli.Location = new System.Drawing.Point(6, 19);
            this.rbArtikli.Name = "rbArtikli";
            this.rbArtikli.Size = new System.Drawing.Size(83, 23);
            this.rbArtikli.TabIndex = 0;
            this.rbArtikli.TabStop = true;
            this.rbArtikli.Tag = "Artikli";
            this.rbArtikli.Text = "Artikal";
            this.rbArtikli.UseVisualStyleBackColor = true;
            // 
            // cmbNazivSlike
            // 
            this.cmbNazivSlike.FormattingEnabled = true;
            this.cmbNazivSlike.Location = new System.Drawing.Point(230, 83);
            this.cmbNazivSlike.Name = "cmbNazivSlike";
            this.cmbNazivSlike.Size = new System.Drawing.Size(449, 27);
            this.cmbNazivSlike.TabIndex = 4;
            this.cmbNazivSlike.SelectedIndexChanged += new System.EventHandler(this.cmbNazivSlike_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Snow;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("TimesRoman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.button2.Location = new System.Drawing.Point(596, 22);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(83, 29);
            this.button2.TabIndex = 5;
            this.button2.Text = "Prika`i";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("TimesRoman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.label2.Location = new System.Drawing.Point(226, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "Naziv slike:";
            // 
            // frmSlika
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(713, 586);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.cmbNazivSlike);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("TimesRoman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmSlika";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSlika_FormClosed);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmSlika_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton rbSkl;
        public System.Windows.Forms.RadioButton rbMagPolje;
        public System.Windows.Forms.RadioButton rbArtikli;
        public System.Windows.Forms.ComboBox cmbNazivSlike;
        public System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.Label label2;
    }
}