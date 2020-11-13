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
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(151, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Artikal";
            // 
            // idArtikal
            // 
            this.idArtikal.Location = new System.Drawing.Point(153, 72);
            this.idArtikal.Name = "idArtikal";
            this.idArtikal.Size = new System.Drawing.Size(95, 22);
            this.idArtikal.TabIndex = 6;
            this.idArtikal.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.idArtikal.Leave += new System.EventHandler(this.idArtikal_Leave);
            this.idArtikal.Validating += new System.ComponentModel.CancelEventHandler(this.idArtikal_Validating);
            // 
            // artikli
            // 
            this.artikli.FormattingEnabled = true;
            this.artikli.Location = new System.Drawing.Point(254, 70);
            this.artikli.Name = "artikli";
            this.artikli.Size = new System.Drawing.Size(379, 24);
            this.artikli.TabIndex = 21;
            this.artikli.DropDown += new System.EventHandler(this.artikli_DropDown);
            this.artikli.SelectedIndexChanged += new System.EventHandler(this.artikli_SelectedIndexChanged);
            this.artikli.TextUpdate += new System.EventHandler(this.artikli_TextUpdate);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(148, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(205, 20);
            this.label2.TabIndex = 22;
            this.label2.Text = "Mesto proizvodnje/prijema";
            // 
            // skladista
            // 
            this.skladista.FormattingEnabled = true;
            this.skladista.Location = new System.Drawing.Point(152, 122);
            this.skladista.Name = "skladista";
            this.skladista.Size = new System.Drawing.Size(480, 24);
            this.skladista.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(148, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 20);
            this.label3.TabIndex = 24;
            this.label3.Text = "Datum proizvodnje";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(404, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 20);
            this.label4.TabIndex = 25;
            this.label4.Text = "Datum isteka";
            // 
            // datumProizvodnje
            // 
            this.datumProizvodnje.Location = new System.Drawing.Point(153, 172);
            this.datumProizvodnje.Name = "datumProizvodnje";
            this.datumProizvodnje.Size = new System.Drawing.Size(222, 22);
            this.datumProizvodnje.TabIndex = 26;
            // 
            // datumIsteka
            // 
            this.datumIsteka.Location = new System.Drawing.Point(408, 172);
            this.datumIsteka.Name = "datumIsteka";
            this.datumIsteka.Size = new System.Drawing.Size(224, 22);
            this.datumIsteka.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(151, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 20);
            this.label5.TabIndex = 28;
            this.label5.Text = "Proizvodjac";
            // 
            // proizvodjaci
            // 
            this.proizvodjaci.FormattingEnabled = true;
            this.proizvodjaci.Location = new System.Drawing.Point(152, 220);
            this.proizvodjaci.Name = "proizvodjaci";
            this.proizvodjaci.Size = new System.Drawing.Size(480, 24);
            this.proizvodjaci.TabIndex = 30;
            this.proizvodjaci.DropDown += new System.EventHandler(this.proizvodjaci_DropDown);
            this.proizvodjaci.TextUpdate += new System.EventHandler(this.proizvodjaci_TextUpdate);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(151, 247);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(157, 20);
            this.label6.TabIndex = 31;
            this.label6.Text = "Zemlja proizvodjaca";
            // 
            // zempro
            // 
            this.zempro.FormattingEnabled = true;
            this.zempro.Location = new System.Drawing.Point(152, 270);
            this.zempro.Name = "zempro";
            this.zempro.Size = new System.Drawing.Size(480, 24);
            this.zempro.TabIndex = 32;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(151, 297);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 20);
            this.label7.TabIndex = 33;
            this.label7.Text = "Lot proizvodjaca";
            // 
            // lotproizvodjaca
            // 
            this.lotproizvodjaca.Location = new System.Drawing.Point(155, 320);
            this.lotproizvodjaca.Name = "lotproizvodjaca";
            this.lotproizvodjaca.Size = new System.Drawing.Size(480, 22);
            this.lotproizvodjaca.TabIndex = 34;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(155, 387);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 30);
            this.button1.TabIndex = 35;
            this.button1.Text = "Kreiraj";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // izmena
            // 
            this.izmena.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.izmena.Location = new System.Drawing.Point(279, 387);
            this.izmena.Name = "izmena";
            this.izmena.Size = new System.Drawing.Size(92, 30);
            this.izmena.TabIndex = 36;
            this.izmena.Text = "Izmena";
            this.izmena.UseVisualStyleBackColor = true;
            this.izmena.Click += new System.EventHandler(this.izmena_Click_1);
            // 
            // rucnilot
            // 
            this.rucnilot.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rucnilot.Location = new System.Drawing.Point(399, 387);
            this.rucnilot.Name = "rucnilot";
            this.rucnilot.Size = new System.Drawing.Size(103, 30);
            this.rucnilot.TabIndex = 37;
            this.rucnilot.Text = "Ručni lot";
            this.rucnilot.UseVisualStyleBackColor = true;
            // 
            // prekid
            // 
            this.prekid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prekid.Location = new System.Drawing.Point(540, 387);
            this.prekid.Name = "prekid";
            this.prekid.Size = new System.Drawing.Size(92, 30);
            this.prekid.TabIndex = 38;
            this.prekid.Text = "Prekid";
            this.prekid.UseVisualStyleBackColor = true;
            this.prekid.Click += new System.EventHandler(this.prekid_Click);
            // 
            // NoviLot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 450);
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