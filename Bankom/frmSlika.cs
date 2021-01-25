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
        public string imeSlike = "";
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
        string nazivKategorije= "";
        public void PuniCombo(string rbNaziv)
        {
            string kolona = "";
            nazivFoldera = rbNaziv;
            switch (rbNaziv)
            {
                case "Artikli":
                    kolona = "NazivArtikla";
                    nazivKategorije = " izabrani artikal!";
                    break;
                case "MagacinskaPolja":
                    kolona = "NazivPolja";
                    nazivKategorije = " izabrano magacinsko polje!";
                    break;
                case "Skladiste":
                    kolona = "NazivSkl";
                    nazivKategorije = " izabrano skladište!";
                    break;
            }
            string IdKolona = "ID_" + rbNaziv;
            string upit = "Select "+ IdKolona + "," +  kolona + " from " + rbNaziv + " where CCopy=0";
            dt = db.ParamsQueryDT(upit);
            for (int i = 0; i < dt.Rows.Count; i++)
                cmbNazivSlike.Items.Add(dt.Rows[i][1].ToString());
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
                if (File.Exists(@"\\SQL2016\\ISDokumenta\\" + Program.imeFirme + "\\" + tipFajla + "\\" + nazivFoldera + "\\" + imeSlike + ".jpg"))
                {
                    if (MessageBox.Show("Slika već postoji! Da li želite da je zamenite novom slikom?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (nazivFoldera == "")
                            MessageBox.Show("Izaberite kategoriju slike!");
                        else if (imeSlike.Trim() == "")
                            MessageBox.Show("Neispravan naziv slike!");
                        else
                        {
                            pictureBox1.Image.Save(@"\\SQL2016\\ISDokumenta\\" + Program.imeFirme + "\\" + tipFajla + "\\" + nazivFoldera + "\\" + imeSlike + ".jpg");
                            MessageBox.Show("Uspesno ste sačuvali sliku.");
                            pictureBox1.Image = null;
                            label1.BringToFront();
                            cmbNazivSlike.Text = "";
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
                        pictureBox1.Image.Save(@"\\SQL2016\\ISDokumenta\\" + Program.imeFirme + "\\" + tipFajla + "\\" + nazivFoldera + "\\" + imeSlike + ".jpg");
                        MessageBox.Show("Uspesno ste sačuvali sliku.");
                        pictureBox1.Image = null;
                        label1.BringToFront();
                        cmbNazivSlike.Text = "";
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
            for(int i=0;i<dt.Rows.Count; i++)
                if(dt.Rows[i][1].ToString() == cmbNazivSlike.SelectedItem.ToString())
                    imeSlike = dt.Rows[i][0].ToString();
        }

        public void frmSlika_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(dugme!=null)
                dugme.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tipFajla = "Slike";
            if (nazivFoldera == "")
                MessageBox.Show("Izaberite kategoriju slike!");
            else
            {
                if (File.Exists(@"\\SQL2016\\ISDokumenta\\" + Program.imeFirme + "\\" + tipFajla + "\\" + nazivFoldera + "\\" + imeSlike + ".jpg"))
                {
                    pictureBox1.Image = Image.FromFile(@"\\SQL2016\\ISDokumenta\\" + Program.imeFirme + "\\" + tipFajla + "\\" + nazivFoldera + "\\" + imeSlike + ".jpg");
                    label1.Visible = false;
                }
                else
                    MessageBox.Show("Ne postoji slika za" + nazivKategorije);
            }
        }
        private void frmSlika_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                this.Close();
        }
    }
}
