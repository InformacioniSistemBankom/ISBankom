namespace Bankom
{
    partial class NoviLot
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
            this.label1 = new System.Windows.Forms.Label();
            this.idArtikal = new System.Windows.Forms.TextBox();
            this.artikli = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.skladista = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.datumProizvodnje = new System.Windows.Forms.DateTimePicker();
            this.datumIsteka = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.proizvodjaci = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.zempro = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lotproizvodjaca = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.izmena = new System.Windows.Forms.Button();
            this.rucnilot = new System.Windows.Forms.Button();
            this.prekid = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.label1.Location = new System.Drawing.Point(29, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "Artikal";
            // 
            // idArtikal
            // 
            this.idArtikal.BackColor = System.Drawing.Color.AliceBlue;
            this.idArtikal.Font = new System.Drawing.Font("TimesRoman", 11F);
            this.idArtikal.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.idArtikal.Location = new System.Drawing.Point(27, 44);
            this.idArtikal.Name = "idArtikal";
            this.idArtikal.Size = new System.Drawing.Size(124, 24);
            this.idArtikal.TabIndex = 6;
            this.idArtikal.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.idArtikal.Leave += new System.EventHandler(this.idArtikal_Leave);
            this.idArtikal.Validating += new System.ComponentModel.CancelEventHandler(this.idArtikal_Validating);
            // 
            // artikli
            // 
            this.artikli.BackColor = System.Drawing.Color.AliceBlue;
            this.artikli.Font = new System.Drawing.Font("TimesRoman", 11F);
            this.artikli.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.artikli.FormattingEnabled = true;
            this.artikli.Location = new System.Drawing.Point(157, 44);
            this.artikli.Name = "artikli";
            this.artikli.Size = new System.Drawing.Size(555, 26);
            this.artikli.TabIndex = 21;
            this.artikli.DropDown += new System.EventHandler(this.artikli_DropDown);
            this.artikli.SelectedIndexChanged += new System.EventHandler(this.artikli_SelectedIndexChanged);
            this.artikli.TextUpdate += new System.EventHandler(this.artikli_TextUpdate);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.label2.Location = new System.Drawing.Point(24, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 18);
            this.label2.TabIndex = 22;
            this.label2.Text = "Mesto proizvodnje/prijema";
            // 
            // skladista
            // 
            this.skladista.BackColor = System.Drawing.Color.AliceBlue;
            this.skladista.Font = new System.Drawing.Font("TimesRoman", 11F);
            this.skladista.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.skladista.FormattingEnabled = true;
            this.skladista.Location = new System.Drawing.Point(30, 102);
            this.skladista.Name = "skladista";
            this.skladista.Size = new System.Drawing.Size(680, 26);
            this.skladista.TabIndex = 23;
            this.skladista.SelectedIndexChanged += new System.EventHandler(this.skladista_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.label3.Location = new System.Drawing.Point(24, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 18);
            this.label3.TabIndex = 24;
            this.label3.Text = "Datum proizvodnje";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.label4.Location = new System.Drawing.Point(344, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 18);
            this.label4.TabIndex = 25;
            this.label4.Text = "Datum isteka";
            // 
            // datumProizvodnje
            // 
            this.datumProizvodnje.CalendarForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.datumProizvodnje.CalendarMonthBackground = System.Drawing.Color.Snow;
            this.datumProizvodnje.CalendarTitleBackColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.datumProizvodnje.CalendarTitleForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.datumProizvodnje.CalendarTrailingForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.datumProizvodnje.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold);
            this.datumProizvodnje.Location = new System.Drawing.Point(31, 161);
            this.datumProizvodnje.Name = "datumProizvodnje";
            this.datumProizvodnje.Size = new System.Drawing.Size(277, 24);
            this.datumProizvodnje.TabIndex = 26;
            // 
            // datumIsteka
            // 
            this.datumIsteka.CalendarForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.datumIsteka.CalendarMonthBackground = System.Drawing.Color.Snow;
            this.datumIsteka.CalendarTitleForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.datumIsteka.CalendarTrailingForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.datumIsteka.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold);
            this.datumIsteka.Location = new System.Drawing.Point(350, 161);
            this.datumIsteka.Name = "datumIsteka";
            this.datumIsteka.Size = new System.Drawing.Size(278, 24);
            this.datumIsteka.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.label5.Location = new System.Drawing.Point(29, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 18);
            this.label5.TabIndex = 28;
            this.label5.Text = "Proizvodjac";
            // 
            // proizvodjaci
            // 
            this.proizvodjaci.BackColor = System.Drawing.Color.AliceBlue;
            this.proizvodjaci.Font = new System.Drawing.Font("TimesRoman", 11F);
            this.proizvodjaci.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.proizvodjaci.FormattingEnabled = true;
            this.proizvodjaci.Location = new System.Drawing.Point(30, 215);
            this.proizvodjaci.Name = "proizvodjaci";
            this.proizvodjaci.Size = new System.Drawing.Size(680, 26);
            this.proizvodjaci.TabIndex = 30;
            this.proizvodjaci.DropDown += new System.EventHandler(this.proizvodjaci_DropDown);
            this.proizvodjaci.SelectedIndexChanged += new System.EventHandler(this.proizvodjaci_SelectedIndexChanged);
            this.proizvodjaci.TextUpdate += new System.EventHandler(this.proizvodjaci_TextUpdate);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.label6.Location = new System.Drawing.Point(29, 244);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(162, 18);
            this.label6.TabIndex = 31;
            this.label6.Text = "Zemlja proizvodjaca";
            // 
            // zempro
            // 
            this.zempro.BackColor = System.Drawing.Color.AliceBlue;
            this.zempro.Font = new System.Drawing.Font("TimesRoman", 11F);
            this.zempro.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.zempro.FormattingEnabled = true;
            this.zempro.Location = new System.Drawing.Point(30, 269);
            this.zempro.Name = "zempro";
            this.zempro.Size = new System.Drawing.Size(680, 26);
            this.zempro.TabIndex = 32;
            this.zempro.SelectedIndexChanged += new System.EventHandler(this.zempro_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.label7.Location = new System.Drawing.Point(29, 300);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 18);
            this.label7.TabIndex = 33;
            this.label7.Text = "Lot proizvodjaca";
            // 
            // lotproizvodjaca
            // 
            this.lotproizvodjaca.BackColor = System.Drawing.Color.AliceBlue;
            this.lotproizvodjaca.Font = new System.Drawing.Font("TimesRoman", 11F);
            this.lotproizvodjaca.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.lotproizvodjaca.Location = new System.Drawing.Point(33, 325);
            this.lotproizvodjaca.Name = "lotproizvodjaca";
            this.lotproizvodjaca.Size = new System.Drawing.Size(680, 24);
            this.lotproizvodjaca.TabIndex = 34;
            this.lotproizvodjaca.TextChanged += new System.EventHandler(this.lotproizvodjaca_TextChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Snow;
            this.button1.FlatAppearance.BorderColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.button1.FlatAppearance.BorderSize = 3;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("TimesRoman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.button1.Location = new System.Drawing.Point(33, 417);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 33);
            this.button1.TabIndex = 35;
            this.button1.Text = "Kreiraj";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // izmena
            // 
            this.izmena.BackColor = System.Drawing.Color.Snow;
            this.izmena.FlatAppearance.BorderColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.izmena.FlatAppearance.BorderSize = 3;
            this.izmena.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.izmena.Font = new System.Drawing.Font("TimesRoman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.izmena.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.izmena.Location = new System.Drawing.Point(189, 417);
            this.izmena.Name = "izmena";
            this.izmena.Size = new System.Drawing.Size(114, 33);
            this.izmena.TabIndex = 36;
            this.izmena.Text = "Izmena";
            this.izmena.UseVisualStyleBackColor = false;
            this.izmena.Click += new System.EventHandler(this.izmena_Click_1);
            // 
            // rucnilot
            // 
            this.rucnilot.BackColor = System.Drawing.Color.Snow;
            this.rucnilot.FlatAppearance.BorderColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.rucnilot.FlatAppearance.BorderSize = 3;
            this.rucnilot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rucnilot.Font = new System.Drawing.Font("TimesRoman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rucnilot.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.rucnilot.Location = new System.Drawing.Point(339, 417);
            this.rucnilot.Name = "rucnilot";
            this.rucnilot.Size = new System.Drawing.Size(129, 33);
            this.rucnilot.TabIndex = 37;
            this.rucnilot.Text = "Ručni lot";
            this.rucnilot.UseVisualStyleBackColor = false;
            // 
            // prekid
            // 
            this.prekid.BackColor = System.Drawing.Color.Snow;
            this.prekid.FlatAppearance.BorderColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.prekid.FlatAppearance.BorderSize = 3;
            this.prekid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prekid.Font = new System.Drawing.Font("TimesRoman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prekid.ForeColor =System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.prekid.Location = new System.Drawing.Point(514, 417);
            this.prekid.Name = "prekid";
            this.prekid.Size = new System.Drawing.Size(114, 33);
            this.prekid.TabIndex = 38;
            this.prekid.Text = "Prekid";
            this.prekid.UseVisualStyleBackColor = false;
            this.prekid.Click += new System.EventHandler(this.prekid_Click);
            // 
            // NoviLot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(743, 471);
            this.Controls.Add(this.prekid);
            this.Controls.Add(this.rucnilot);
            this.Controls.Add(this.izmena);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lotproizvodjaca);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.zempro);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.proizvodjaci);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.datumIsteka);
            this.Controls.Add(this.datumProizvodnje);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.skladista);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.artikli);
            this.Controls.Add(this.idArtikal);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NoviLot";
            this.Text = "NoviLot";
            this.Load += new System.EventHandler(this.NoviLot_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox idArtikal;
        private System.Windows.Forms.ComboBox artikli;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox skladista;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker datumProizvodnje;
        private System.Windows.Forms.DateTimePicker datumIsteka;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox proizvodjaci;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox zempro;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox lotproizvodjaca;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button izmena;
        private System.Windows.Forms.Button rucnilot;
        private System.Windows.Forms.Button prekid;
    }
}