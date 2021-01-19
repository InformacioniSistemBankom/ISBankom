using Bankom.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bankom
{
    public partial class frmSlika : Form
    {
        public ToolStripButton dugme;
        public frmSlika(ToolStripButton btn)
        {
            dugme = btn;

            InitializeComponent();
            pictureBox1.AllowDrop = true;
            foreach (RadioButton rb in groupBox1.Controls)
                rb.CheckedChanged += RadioButton_CheckedChanged;
        }
        public frmSlika()
        {
            InitializeComponent();
            pictureBox1.AllowDrop = true;
            foreach (RadioButton rb in groupBox1.Controls)
                rb.CheckedChanged += RadioButton_CheckedChanged;
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        string imeSlike = "";
        public void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] niz = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (niz.Count() > 1)
                MessageBox.Show("Možete uneti samo jednu sliku!");
            else
            {
                pictureBox1.Image = Image.FromFile(niz[0]);
                pictureBox1.BringToFront();
            }
        }
        string nazivFoldera = "";
        public void PuniCombo(string rbNaziv)
        {
            string kolona = "";
            nazivFoldera = rbNaziv;
            switch (rbNaziv)
            {
                case "Artikli":
                    kolona = "NazivArtikla";
                    break;
                case "MagacinskaPolja":
                    kolona = "NazivPolja";
                    break;
                case "Skladiste":
                    kolona = "NazivSkl";
                    break;
            }
            string upit = "Select "+  kolona + " from " + rbNaziv + " where CCopy=0";
            dt = db.ParamsQueryDT(upit);
            for (int i = 0; i < dt.Rows.Count; i++)
                cmbNazivSlike.Items.Add(dt.Rows[i][0].ToString());
        }

        DataBaseBroker db = new DataBaseBroker();
        DataTable dt = new DataTable();
        private void button1_Click(object sender, EventArgs e)
        {
            string tipFajla = "Slike";
            if (pictureBox1.Image == null)
                MessageBox.Show("Neophodno je da unesete sliku!");
            else
            {
                if (File.Exists(@"\\SQL2016\\ISDokumenta2019\\" + Program.imeFirme + "\\" + tipFajla + "\\" + nazivFoldera + "\\" + imeSlike + ".jpg"))
                {
                    if (MessageBox.Show("Slika već postoji! Da li želite da je zamenite novom slikom?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (nazivFoldera == "")
                            MessageBox.Show("Izaberite kategoriju slike!");
                        else if (imeSlike.Trim() == "")
                            MessageBox.Show("Neispravan naziv slike!");
                        else
                        {
                            pictureBox1.Image.Save(@"\\SQL2016\\ISDokumenta2019\\" + Program.imeFirme + "\\" + tipFajla + "\\" + nazivFoldera + "\\" + imeSlike + ".jpg");
                            MessageBox.Show("Uspesno ste sačuvali sliku.");
                            dugme.Enabled = true;
                            this.Close();
                        }
                    }
                }
                else
                {
                    if (nazivFoldera == "")
                        MessageBox.Show("Izaberite kategoriju slike!");
                    else if (imeSlike.Trim() == "")
                        MessageBox.Show("Neispravan naziv slike!");
                    else
                    {
                        pictureBox1.Image.Save(@"\\SQL2016\\ISDokumenta2019\\" + Program.imeFirme + "\\" + tipFajla + "\\" + nazivFoldera + "\\" + imeSlike + ".jpg");
                        MessageBox.Show("Uspesno ste sačuvali sliku.");
                        
                        dugme.Enabled = true;
                        this.Close();    
                    }
                }
            } 
        }
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            cmbNazivSlike.Items.Clear();
            foreach (RadioButton rb in groupBox1.Controls)
                if (rb.Checked)
                    PuniCombo(rb.Tag.ToString());
        }

        private void cmbNazivSlike_SelectedIndexChanged(object sender, EventArgs e)
        {
            imeSlike = cmbNazivSlike.SelectedItem.ToString();
        }

        private void frmSlika_FormClosed(object sender, FormClosedEventArgs e)
        {
            dugme.Enabled = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
