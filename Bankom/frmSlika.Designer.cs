
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox1.Location = new System.Drawing.Point(37, 148);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(517, 412);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox1_DragDrop);
            this.pictureBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBox1_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("TimesRoman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.label1.Location = new System.Drawing.Point(163, 343);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Prevucite sliku ovde...";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Snow;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("TimesRoman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.button1.Location = new System.Drawing.Point(493, 38);
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
            this.groupBox1.Location = new System.Drawing.Point(12, 25);
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
            this.cmbNazivSlike.Location = new System.Drawing.Point(200, 40);
            this.cmbNazivSlike.Name = "cmbNazivSlike";
            this.cmbNazivSlike.Size = new System.Drawing.Size(271, 21);
            this.cmbNazivSlike.TabIndex = 4;
            this.cmbNazivSlike.SelectedIndexChanged += new System.EventHandler(this.cmbNazivSlike_SelectedIndexChanged);
            // 
            // frmSlika
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(588, 586);
            this.Controls.Add(this.cmbNazivSlike);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSlika";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Slike";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSlika_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbSkl;
        private System.Windows.Forms.RadioButton rbMagPolje;
        private System.Windows.Forms.RadioButton rbArtikli;
        private System.Windows.Forms.ComboBox cmbNazivSlike;
    }
}