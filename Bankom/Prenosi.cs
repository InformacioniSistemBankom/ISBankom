using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bankom.Class;
using System.Net;
using System.Net.Mail;
using System.Text;
namespace Bankom
{
    public partial class Prenosi : Form



    {
        OpenFileDialog ofd;

        int kojaOpcija;
        string kojiDatum;
        string comboText;
        string putanjaFajla;

        public Prenosi()
        {
            InitializeComponent();

            kojiDatum = dateTimePicker1.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);




        }


        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {




        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void Prenosi_Load(object sender, EventArgs e)
        {


            sviNevidljivi();
        }

        private void sviNevidljivi()
        {
            comboBox1.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label7.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            // button3.Visible = false;
            progressBar1.Visible = false;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton2.Checked == true)
            {
                sviNevidljivi();
                kojaOpcija = 2;
                dateTimePicker1.Visible = true;
                label2.Text = "Promet za :";
                label2.Visible = true;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

            if (radioButton4.Checked == true)
            {
                sviNevidljivi();
                kojaOpcija = 4;
                dateTimePicker1.Visible = false;
                label2.Visible = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                sviNevidljivi();
                kojaOpcija = 1;
                dateTimePicker1.Visible = true;
                label2.Text = "Datum isporuke :";
                label2.Visible = true;
            }
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                sviNevidljivi();
                kojaOpcija = 3;
                dateTimePicker1.Visible = true;
                label2.Text = "Promet za :";
                label2.Visible = true;
            }

        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
            {
                sviNevidljivi();
                kojaOpcija = 5;
                dateTimePicker1.Visible = false;
                label2.Visible = false;
            }


        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked == true)
            {
                sviNevidljivi();
                kojaOpcija = 6;
                dateTimePicker1.Visible = false;
                label2.Visible = false;
            }
        }
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked == true)
            {
                sviNevidljivi();
                kojaOpcija = 7;
                dateTimePicker1.Visible = false;
                label2.Visible = false;
            }
        }
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked == true)
            {
                sviNevidljivi();
                kojaOpcija = 8;
                dateTimePicker1.Visible = false;
                label2.Visible = false;
            }
        }
        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked == true)
            {
                sviNevidljivi();
                kojaOpcija = 9;

                dateTimePicker1.Visible = false;
                label2.Visible = false;
            }
        }
        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked == true)
            {
                sviNevidljivi();
                kojaOpcija = 10;
                dateTimePicker1.Visible = false;
                label2.Visible = false;
            }
        }
        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton11.Checked == true)
            {
                sviNevidljivi();
                kojaOpcija = 11;
                dateTimePicker1.Visible = false;
                label2.Visible = false;
            }
        }
        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton12.Checked == true)
            {
                sviNevidljivi();
                kojaOpcija = 12;
                dateTimePicker1.Visible = false;
                label2.Visible = false;
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            ofd = new OpenFileDialog();
            string path = @"\\192.168.1.40\d\prenosi";
            if (Directory.Exists(path))
            {
                ofd.InitialDirectory = path;
            }



            switch (kojaOpcija)
            {
                case 1:

                    ofd.Filter = "excel files (*.xls)|*.xlsx";
                    break;
                case 2:

                    ofd.Filter = "txt files (*" + kojiDatum + ".txt)|*" + kojiDatum + ".txt";
                    break;
                case 3:

                    ofd.Filter = "txt files (*" + kojiDatum + ".txt)|*" + kojiDatum + ".txt";
                    break;
                case 4:
                case 6:
                    ofd.FileName = "Cenovnik*";
                    ofd.Filter = "txt files|*.txt";
                    break;
                case 5:

                    ofd.Filter = "txt files(*Komitenti.txt)|*Komitenti.txt";

                    break;
                case 7:

                    ofd.Filter = "txt files(*KompletiUsluga.txt)|*KompletiUsluga.txt";
                    break;
                case 8:

                    ofd.Filter = "txt files(*Sobe.txt)|*Sobe.txt";
                    break;
                case 9:

                    ofd.Filter = "txt files(*ProdajnaMesta.txt)|*ProdajnaMesta.txt";
                    break;
                case 10:

                    ofd.Filter = "txt files(*Zaposleni.txt)|*Zaposleni.txt";
                    break;
                case 11:

                    ofd.Filter = "txt files(*NaciniPlacanja.txt)|*NaciniPlacanja.txt";
                    break;
                case 12:

                    ofd.Filter = "txt files(*OpcijeCenovnika.txt)|*OpcijeCenovnika.txt";
                    break;


            }
            // ofd.Filter = "txt files(*.txt)|*.txt| files(*.xls)|*.xlsx";
            //ofd.Filter = "txt files(*Komitenti.txt)|*Komitenti.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
                textBox1.Visible = true;
                putanjaFajla = ofd.FileName;
                if (kojaOpcija == 3) textBox1.Enabled = false;
                else textBox1.Enabled = true;
                if (kojaOpcija == 4 || kojaOpcija == 6)
                { // ako su izabrane opcije cenovnik maloprodaje ili recepcije
                    DataBaseBroker db = new DataBaseBroker();
                    string sel = "SELECT NazivSkl FROM SkladisteStavkeView  "
                 + " where OpisSkladista like '%Malo%' or OpisSkladista='Usluge' order by NazivSkl";
                    DataTable dt = db.ReturnDataTable(sel);
                    comboBox1.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!row["NazivSkl"].ToString().Equals("")) comboBox1.Items.Add(row["NazivSkl"].ToString());
                    }
                    label3.Text = "Izaberite objekat";
                    label3.Visible = true;
                    comboBox1.Visible = true;
                    comboBox1.Focus();

                }

                else if (kojaOpcija == 1)
                {  // ako je izbrana opcija narudzbenica kupca
                    DataBaseBroker db = new DataBaseBroker();
                    string sel = "SELECT NazivKom FROM KomitentiStavkeView  "
                   + " order by NazivKom";
                    DataTable dt = db.ReturnDataTable(sel);

                    comboBox1.Items.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        if (!row["NazivKom"].ToString().Equals("")) comboBox1.Items.Add(row["NazivKom"].ToString());
                    }

                    label3.Text = "Izaberite komitenta";
                    label3.Visible = true;
                    comboBox1.Visible = true;
                    comboBox1.Focus();
                }
                else if (kojaOpcija == 7 || kojaOpcija == 9 || kojaOpcija == 10 || kojaOpcija == 12)
                {

                    DataBaseBroker db = new DataBaseBroker();
                    string sel = "SELECT Naziv FROM OrganizacionaStruktura";

                    DataTable dt = db.ReturnDataTable(sel);
                    comboBox1.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!row["Naziv"].ToString().Equals("")) comboBox1.Items.Add(row["Naziv"].ToString());
                    }
                    if (kojaOpcija == 9 || kojaOpcija == 7)
                    {
                        label3.Visible = false;
                        comboBox1.Visible = false;
                    }
                    else
                    {
                        label3.Text = "Izaberite Org.dio";
                        label3.Visible = true;
                        comboBox1.Visible = true;
                        comboBox1.Focus();
                    }

                }

                label4.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            kojiDatum = dateTimePicker1.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        }
        public static Hashtable convertDataTableToHashTable(DataTable dtIn, string keyField, string valueField)
        {
            Hashtable htOut = new Hashtable();
            foreach (DataRow drIn in dtIn.Rows)
            {
                htOut.Add(drIn[keyField].ToString(), drIn[valueField].ToString());
            }
            return htOut;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboText = comboBox1.Text;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            label7.Text = "";
            switch (kojaOpcija)
            {
                case 1:
                    prepisiNarudzbenicuKupca(putanjaFajla, comboText);
                    comboBox1.Text = "";
                    break;
                case 2:
                    prepisiPrometRecepcije(putanjaFajla);

                    break;
                case 3:
                    if (Program.imeFirme == "Hotel Nevski" || Program.imeFirme == "Bankom")
                    {
                        prepisiPromet(putanjaFajla);

                    }
                    else
                    {
                        prepisiPrometNovi(putanjaFajla);
                    }
                    break;
                case 4:
                    prepisiCenovnik(putanjaFajla, comboText);
                    textBox1.Text = "";
                    sviNevidljivi();
                    label7.Visible = true;
                    progressBar1.Visible = true;
                    break;
                case 5:
                    prepisiKomitente(putanjaFajla);
                    break;
                case 6:
                    prepisiCenovnikRecepcije(putanjaFajla, comboText);
                    comboBox1.Text = "";
                    textBox1.Text = "";
                    sviNevidljivi();
                    label7.Visible = true;
                    progressBar1.Visible = true;
                    break;
                case 7:
                    prepisiKomplete(putanjaFajla);
                    comboBox1.Text = "";
                    break;
                case 8:
                    prepisiSobe(putanjaFajla);
                    break;
                case 9:
                    prepisiProdajnaMesta(putanjaFajla);

                    comboBox1.Text = "";
                    break;
                case 10:
                    prepisiZaposlene(putanjaFajla, comboText);
                    comboBox1.Text = "";
                    textBox1.Text = "";
                    sviNevidljivi();
                    break;
                case 11:
                    prepisiNacine(putanjaFajla);
                    break;
                case 12:
                    prepisiOpcijeCenovnika(putanjaFajla);
                    comboBox1.Text = "";
                    break;



            }
        }
        void prepisiPrometNovi(string filepath)
        {
            int pp = 0;
            int ID_DokumentaStablo;
            int PArtikal = 1;
            int rr = 0; // broj prepisanih slogova
            DataBaseBroker db = new DataBaseBroker();
            string del = "DELETE from PrometPreneseni";
            // bool postojiPromet = false;
            bool ispravan = true;
            SqlCommand cmd = new SqlCommand(del);
            db.Comanda(cmd);

            List<PrometSankova> list = new List<PrometSankova>();

            StreamReader sr = new StreamReader(putanjaFajla);
            KonvertujFajluKlasu1(sr, list);


            string sel = "SELECT * from  PDVPrometMaloprodajeTotali where Convert(date,Datum)='" + kojiDatum + "' and ID_Skladiste=" + list[0].ID_Skladiste;
            DataTable dt = db.ReturnDataTable(sel);
            if (dt.Rows.Count != 0)
            {
                MessageBox.Show("Vec je izvrsen prepis prometa za datum " + kojiDatum + " i skladiste " + list[0].NazivSkladista);
                return;
            }
            for (int i = 0; i < list.Count(); i++)
            {
                string sql = "INSERT INTO PrometPreneseni(Datum,ID_Skladiste,NazivSkladista,ID_Artikal,Kolicina,Cena,ID_NacinPlacanja,ID_Komitent,ID_Kartica,ID_Kelner,Racun) VALUES (@Datum,@ID_Skladiste,@NazivSkladista,@ID_Artikal,@Kolicina,@Cena,@ID_NacinPlacanja,@ID_Komitent,@ID_Kartica,@ID_Kelner,@Racun)";

                cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@Datum", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@ID_Skladiste", list[i].ID_Skladiste);
                cmd.Parameters.AddWithValue("@NazivSkladista", list[i].NazivSkladista);
                cmd.Parameters.AddWithValue("@ID_Artikal", list[i].ID_Artikal);
                cmd.Parameters.AddWithValue("@Kolicina", list[i].Kolicina);
                cmd.Parameters.AddWithValue("@Cena", list[i].Cena);
                cmd.Parameters.AddWithValue("@ID_NacinPlacanja", list[i].ID_NacinPlacanja);
                cmd.Parameters.AddWithValue("@ID_Komitent", list[i].ID_Komitent);
                cmd.Parameters.AddWithValue("@ID_Kartica", list[i].ID_Kartica);
                cmd.Parameters.AddWithValue("@Racun", list[i].Racun);
                cmd.Parameters.AddWithValue("@ID_Kelner", list[i].ID_Kelner);




                if (db.Comanda(cmd) == "") rr = rr + 1;
                else MessageBox.Show("Greska u upitu!");


            }
            pp = 0;
            progressBar1.Visible = true;
            // provera da li za prodate artikle postoji normativ 


            if (Program.imeFirme == "Leotar")
            {
                label7.Text = "Provera postojanje normativa u toku!";
                string select = "SELECT * from PrometPreneseni where ID_Artikal > 1 order by ID_Artikal";
                DataTable dt1 = db.ReturnDataTable(select);
                foreach (DataRow row in dt1.Rows)
                {
                    pp = pp + 1;
                    if (PArtikal != Convert.ToInt32(row["ID_Artikal"]))
                    {
                        string poruka = "";

                        if (proveraNormativaICena(Convert.ToInt32(row["ID_Artikal"]), Convert.ToDateTime(row["Datum"]), Convert.ToInt32(row["ID_Skladiste"]), ref poruka) == false)
                        {
                            MessageBox.Show(poruka);
                            ispravan = false;
                        }
                        PArtikal = Convert.ToInt32(row["ID_Artikal"]);
                    }
                    decimal d = 100 * ((decimal)pp / (decimal)rr);
                    progressBar1.Value = Convert.ToInt32(Math.Round(d));
                    this.Refresh();

                }
                if (ispravan == false)
                {
                    if (MessageBox.Show("Ne postoji normativ ili cena za neki artikal!", "Da li zelite nastaviti prepis prometa?", MessageBoxButtons.YesNo) == DialogResult.No) return;
                }
            }

            string nazivusif = "BrDok";
            label7.Text = "Upisivanje prometa maloprodaje u toku!";
            string prometprepisani = "SELECT * FROM PrometPreneseni WHERE ID_Artikal>1 order by Datum,ID_NacinPlacanja,ID_Soba,ID_Komitent,Racun,ID_Artikal";
            pp = 0;
            DataTable dtpro = db.ReturnDataTable(prometprepisani);
            int prolaz = 0;
            int PNacinPlacanja = 0;
            string pOznaka = "";
            DateTime pDatum = new DateTime();
            int pSkladiste = 0;
            int pNeoporezivo = 0;
            int PArtikl = 0;
            double pCena = 0;
            int pKomitent = 0;
            int pSoba = 0;
            int PRacun = 0;
            int Ukolicina = 0;
            int pidDok = 0;
            int PSkladisteProizvodnje = 0;
            foreach (DataRow row in dtpro.Rows)
            {

                if (prolaz > 0) goto labela;

                labela2:
                pDatum = Convert.ToDateTime(row["Datum"]);
                pSkladiste = Convert.ToInt32(row["ID_Skladiste"]);
                string nSkladiste = Convert.ToString(row["NazivSkladista"]);
                PNacinPlacanja = Convert.ToInt32(row["ID_NacinPlacanja"]);
                pSoba = 1;
                pKomitent = Convert.ToInt32(row["ID_Komitent"]);
                PArtikl = Convert.ToInt32(row["id_Artikal"]);
                // PRacun = Convert.ToInt32(row["Racun"]);

                // int pKartica = Convert.ToInt32(row["ID_Kartica"]);
                pCena = Convert.ToDouble(row["Cena"]);

                Ukolicina = 0;
                string pdokument = "PDVPrometMaloprodaje";
                ID_DokumentaStablo = 279;

                int rbr = 0;
                string BrojDok = "";
                int IdDokView;
                clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                IdDokView = os.UpisiDokument(ref BrojDok, nSkladiste, ID_DokumentaStablo, dateTimePicker1.Value.ToString());


                prolaz = prolaz + 1;
                //string insertdokumenta = "INSERT INTO Dokumenta(RedniBroj,ID_KadrovskaEvidencija,ID_Predhodni,ID_DokumentaStablo,BrojDokumenta,Datum,Opis,ID_OrganizacionaStrukturaView,Proknjizeno,MesecPoreza) VALUES (@RedniBroj,@ID_KadrovskaEvidencija,@ID_Predhodni,@ID_DokumentaStablo,@BrojDokumenta,@Datum,@Opis,@ID_OrganizacionaStrukturaView,@Proknjizeno,@MesecPoreza)";
                //cmd = new SqlCommand(insertdokumenta);
                //cmd.Parameters.AddWithValue("@RedniBroj", rbr);
                //cmd.Parameters.AddWithValue("@ID_KadrovskaEvidencija", Program.idkadar);
                //cmd.Parameters.AddWithValue("@ID_Predhodni", 1);
                //md.Parameters.AddWithValue("@BrojDokumenta", BrojDok);
                //cmd.Parameters.AddWithValue("@Datum", dateTimePicker1.Value);
                //cmd.Parameters.AddWithValue("@Opis", nSkladiste);
                //cmd.Parameters.AddWithValue("@ID_OrganizacionaStrukturaView", Program.idOrgDeo);
                //ccmd.Parameters.AddWithValue("@ID_DokumentaStablo", ID_DokumentaStablo);
                //cmd.Parameters.AddWithValue("@Proknjizeno", "NijeProknjizeno");
                //cmd.Parameters.AddWithValue("@MesecPoreza", dateTimePicker1.Value.Month);

                //if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");

                //string selectdokumenta = "SELECT ID_Dokumenta from Dokumenta where BrojDokumenta ='" + BrojDok + "'";

                pidDok = IdDokView;  ///db.ReturnInt(selectdokumenta, 0);

                string insertprometrecepcije = "INSERT INTO PrometMaloprodaje(ID_DokumentaView,ID_OrganizacionaStruktura,ID_Skladiste,UUser,ID_NacinPl)VALUES(@ID_DokumentaView,@ID_OrganizacionaStruktura,@ID_Skladiste,@UUser,@ID_NacinPl)";
                cmd = new SqlCommand(insertprometrecepcije);
                cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
                cmd.Parameters.AddWithValue("@ID_OrganizacionaStruktura", Program.idOrgDeo);
                cmd.Parameters.AddWithValue("@ID_Skladiste", pSkladiste);
                cmd.Parameters.AddWithValue("@UUser", Program.idkadar);
                cmd.Parameters.AddWithValue("@ID_NacinPl", PNacinPlacanja);
                if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");
                labela:




                if (PNacinPlacanja != Convert.ToInt32(row["ID_NacinPlacanja"]))
                {
                    if (Ukolicina > 0) ZapisiSlogPrometaNovi(pidDok, PArtikl, pCena, Ukolicina, pDatum, pSoba, pKomitent);

                    if (Program.imeFirme == "Leotar")
                    {
                        db.ExecuteStoreProcedure("AzurirajCenuSirovinaZaPromet", "IdDokView:" + pidDok);
                    }

                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);

                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PDVPrometMaloprodaje", "IdDokument:" + pidDok);
                    db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:" + pidDok);
                    goto labela2;
                }

                if (PArtikl != Convert.ToInt32(row["ID_Artikal"]) || pCena != Convert.ToDouble(row["Cena"]))
                {


                    ZapisiSlogPrometaNovi(pidDok, PArtikl, pCena, Ukolicina, pDatum, pSoba, pKomitent);
                    Ukolicina = 0;
                    pKomitent = Convert.ToInt32(row["ID_Komitent"]);
                    PArtikl = Convert.ToInt32(row["id_Artikal"]);
                    pCena = Convert.ToDouble(row["Cena"]);

                }
                pp = pp + 1;
                decimal d = 100 * ((decimal)pp / (decimal)dtpro.Rows.Count);
                progressBar1.Value = Convert.ToInt32(Math.Round(d));
                Ukolicina = Ukolicina + Convert.ToInt32(row["Kolicina"]);
                this.Refresh();
                continue;


            }
            if (Ukolicina > 0) ZapisiSlogPrometaNovi(pidDok, PArtikl, pCena, Ukolicina, pDatum, pSoba, pKomitent);

            if (Program.imeFirme == "Leotar")
            {
                db.ExecuteStoreProcedure("AzurirajCenuSirovinaZaPromet", "IdDokView:" + pidDok);
            }


            //db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);

            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PDVPrometMaloprodaje", "IdDokument:" + pidDok);

            db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:" + pidDok);

            label7.Text = "Prepisano slogova:" + pp.ToString();
            label7.Visible = true;

        }

        public void ZapisiSlogPrometaNovi(int pidDok, int pArtikl, double pCena, double pKolicina, DateTime pDatum, int ID_Soba, int ID_Komitent)
        {
            DataBaseBroker db = new DataBaseBroker();
            SqlCommand cmd = new SqlCommand();
            string inspromal = "INSERT into PrometMaloprodajeStavke(ID_DokumentaView,ID_ArtikliView,ProdajnaCena,StvarnaProdajnaCena,Kolicina,ValutaPlacanja,ID_Soba,ID_KomitentiView) VALUES (@ID_DokumentaView,@ID_ArtikliView,@ProdajnaCena,@StvarnaProdajnaCena,@Kolicina,@ValutaPlacanja,@ID_Soba,@ID_KomitentiView)";
            cmd = new SqlCommand(inspromal);
            cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
            cmd.Parameters.AddWithValue("@ID_ArtikliView", pArtikl);
            cmd.Parameters.AddWithValue("@ProdajnaCena", pCena);
            cmd.Parameters.AddWithValue("@StvarnaProdajnaCena", pCena);
            cmd.Parameters.AddWithValue("@Kolicina", pKolicina);
            cmd.Parameters.AddWithValue("@ValutaPlacanja", pDatum);
            cmd.Parameters.AddWithValue("@ID_Soba", ID_Soba);
            cmd.Parameters.AddWithValue("@ID_KomitentiView", ID_Komitent);



            if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");



            /*   Private Sub ZapisiSlogPrometaNovi()
   cmd.CommandType = adCmdText
   cmd.CommandText = "Insert into PrometMaloprodajeStavke (ID_DokumentaView,ID_ArtikliView,ProdajnaCena,StvarnaProdajnaCena," _
                   & "Kolicina,ValutaPlacanja,ID_Soba,Id_KomitentiView) " _
                   & " Values ( " + Str(PidDok) + "," + Str(PArtikal) + "," + Str(Pcena) + "," + Str(Pcena) + "," + Str(Ukolicina) _
                   & ",'" + pDatum + "'," + Str(PSoba) + "," + Str(PKomitent) + ")"
   cmd.Execute
   End Sub*/

        }
        void prepisiPromet(string filepath)
        {
            int pp = 0;
            int ID_DokumentaStablo;
            int PArtikal = 1;
            int rr = 0; // broj prepisanih slogova
            DataBaseBroker db = new DataBaseBroker();
            string del = "DELETE from PrometPreneseni";
            // bool postojiPromet = false;
            bool ispravan = true;
            SqlCommand cmd = new SqlCommand(del);
            db.Comanda(cmd);

            List<PrometSankova> list = new List<PrometSankova>();

            StreamReader sr = new StreamReader(putanjaFajla);
            KonvertujFajluKlasu1(sr, list);


            string sel = "SELECT * from  PDVPrometMaloprodajeTotali where Convert(date,Datum)='" + kojiDatum + "' and ID_Skladiste=" + list[0].ID_Skladiste;
            DataTable dt = db.ReturnDataTable(sel);
            if (dt.Rows.Count != 0)
            {
                MessageBox.Show("Vec je izvrsen prepis prometa za datum " + kojiDatum + " i skladiste " + list[0].NazivSkladista);
                return;
            }
            for (int i = 0; i < list.Count(); i++)
            {
                string sql = "INSERT INTO PrometPreneseni(Datum,ID_Skladiste,NazivSkladista,ID_Artikal,Kolicina,Cena,ID_NacinPlacanja,ID_Komitent,ID_Kartica,ID_Soba,ID_Kelner,ID_SkladistaProizvodnje,Racun,Popust,VrstaSloga,Oznaka,Neoporezivo) VALUES (@Datum,@ID_Skladiste,@NazivSkladista,@ID_Artikal,@Kolicina,@Cena,@ID_NacinPlacanja,@ID_Komitent,@ID_Kartica,@ID_Soba,@ID_Kelner,@ID_SkladistaProizvodnje,@Racun,@Popust,@VrstaSloga,@Oznaka,@Neoporezivo)";

                cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@Datum", dateTimePicker1.Value.Date);
                cmd.Parameters.AddWithValue("@ID_Skladiste", list[i].ID_Skladiste);
                cmd.Parameters.AddWithValue("@NazivSkladista", list[i].NazivSkladista);
                cmd.Parameters.AddWithValue("@ID_Artikal", list[i].ID_Artikal);
                cmd.Parameters.AddWithValue("@Kolicina", list[i].Kolicina);
                cmd.Parameters.AddWithValue("@Cena", list[i].Cena);
                cmd.Parameters.AddWithValue("@ID_NacinPlacanja", list[i].ID_NacinPlacanja);
                cmd.Parameters.AddWithValue("@ID_Komitent", list[i].ID_Komitent);
                cmd.Parameters.AddWithValue("@ID_Kartica", list[i].ID_Kartica);
                cmd.Parameters.AddWithValue("@ID_Soba", list[i].ID_Soba);
                cmd.Parameters.AddWithValue("@ID_Kelner", list[i].ID_Kelner);
                cmd.Parameters.AddWithValue("@ID_SkladistaProizvodnje", list[i].ID_MestoProizvodnje);
                cmd.Parameters.AddWithValue("@Racun", list[i].Racun);
                cmd.Parameters.AddWithValue("@Popust", 0);
                cmd.Parameters.AddWithValue("@VrstaSloga", list[i].VrstaSloga);
                cmd.Parameters.AddWithValue("@Oznaka", list[i].Oznaka);
                cmd.Parameters.AddWithValue("@Neoporezivo", list[i].Neoporezivo);

                if (db.Comanda(cmd) == "") rr = rr + 1;
                else MessageBox.Show("Greska u upitu!");


            }
            pp = 0;
            progressBar1.Visible = true;
            // provera da li za prodate artikle postoji normativ 
            if (Program.imeFirme == "Leotar" || Program.imeFirme.Contains("otel") == true)
            {
                label7.Text = "Provera postojanje normativa u toku!";
                string select = "SELECT * from PrometPreneseni where ID_Artikal > 1 order by ID_Artikal";
                DataTable dt1 = db.ReturnDataTable(select);
                foreach (DataRow row in dt1.Rows)
                {
                    pp = pp + 1;
                    if (PArtikal != Convert.ToInt32(row["ID_Artikal"]))
                    {
                        string poruka = "";

                        if (proveraNormativaICena(Convert.ToInt32(row["ID_Artikal"]), Convert.ToDateTime(row["Datum"]), Convert.ToInt32(row["ID_SkladistaProizvodnje"]), ref poruka) == false)
                        {
                            MessageBox.Show(poruka);
                            ispravan = false;
                        }
                        PArtikal = Convert.ToInt32(row["ID_Artikal"]);
                    }
                    decimal d = 100 * ((decimal)pp / (decimal)rr);
                    progressBar1.Value = Convert.ToInt32(Math.Round(d));

                }
                if (ispravan == false)
                {
                    if (MessageBox.Show("Ne postoji normativ ili cena za neki artikal!", "Da li zelite nastaviti prepis prometa?", MessageBoxButtons.YesNo) == DialogResult.No) return;
                }
            }
            string dokumentastablo = "SELECT * FROM DokumentaStablo where Naziv='PDVPrometMaloprodaje'";
            DataTable dtdoksta = db.ReturnDataTable(dokumentastablo);
            DataRow dr = dtdoksta.Rows[0];
            if (dtdoksta.Rows.Count != 0)
            {

                ID_DokumentaStablo = Convert.ToInt32(dr["ID_DokumentaStablo"]);
            }
            else
            {
                label7.Text = "Nije registrovan dokument promet maloprodaje!";
                return;
            }
            label7.Text = "Upisivanje prometa maloprodaje u toku!";
            string prometprepisani = "SELECT * FROM PrometPreneseni WHERE ID_Artikal>1 order by Datum,Neoporezivo,Oznaka,ID_NacinPlacanja,OznakaValute,ID_Soba,ID_Komitent,ID_Artikal";
            pp = 0;
            DataTable dtpro = db.ReturnDataTable(prometprepisani);
            int prolaz = 0;
            int PNacinPlacanja = 0;
            string pOznaka = "";
            DateTime pDatum = new DateTime();
            int pSkladiste = 0;
            int pNeoporezivo = 0;
            int PArtikl = 0;
            double pCena = 0;
            int pKomitent = 0;
            int pSoba = 0;
            int PRacun = 0;
            int Ukolicina = 0;
            int pidDok = 0;
            int PSkladisteProizvodnje = 0;
            foreach (DataRow row in dtpro.Rows)
            {

                if (prolaz > 0) goto labela;

                labela2: double Ppopust = Convert.ToDouble(row["Popust"]);
                pDatum = Convert.ToDateTime(row["Datum"]);
                pSkladiste = Convert.ToInt32(row["ID_Skladiste"]);
                string nSkladiste = Convert.ToString(row["NazivSkladista"]);
                PNacinPlacanja = Convert.ToInt32(row["ID_NacinPlacanja"]);
                pSoba = Convert.ToInt32(row["ID_Soba"]);
                pKomitent = Convert.ToInt32(row["ID_Komitent"]);
                PArtikl = Convert.ToInt32(row["id_Artikal"]);
                PRacun = Convert.ToInt32(row["Racun"]);
                PSkladisteProizvodnje = Convert.ToInt32(row["ID_SkladistaProizvodnje"]);
                int pKartica = Convert.ToInt32(row["ID_Kartica"]);
                pCena = Convert.ToDouble(row["Cena"]);
                pOznaka = Convert.ToString(row["Oznaka"]);
                pNeoporezivo = Convert.ToInt32(row["Neoporezivo"]);
                Ukolicina = 0;
                string pdokument = "PDVPrometMaloprodaje";
                int rbr = 0;
                string BrojDok = "";
                int IdDokView;
                clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                IdDokView = os.UpisiDokument(ref BrojDok, nSkladiste, ID_DokumentaStablo, dateTimePicker1.Text);

                prolaz = prolaz + 1;
                //string insertdokumenta = "INSERT INTO Dokumenta(RedniBroj,ID_KadrovskaEvidencija,ID_Predhodni,ID_DokumentaStablo,BrojDokumenta,Datum,Opis,ID_OrganizacionaStrukturaView,Proknjizeno,MesecPoreza) VALUES (@RedniBroj,@ID_KadrovskaEvidencija,@ID_Predhodni,@ID_DokumentaStablo,@BrojDokumenta,@Datum,@Opis,@ID_OrganizacionaStrukturaView,@Proknjizeno,@MesecPoreza)";
                //cmd = new SqlCommand(insertdokumenta);
                //cmd.Parameters.AddWithValue("@RedniBroj", rbr);
                //cmd.Parameters.AddWithValue("@ID_KadrovskaEvidencija", Program.idkadar);
                //cmd.Parameters.AddWithValue("@ID_Predhodni", 1);
                //cmd.Parameters.AddWithValue("@ID_DokumentaStablo", ID_DokumentaStablo);
                //cmd.Parameters.AddWithValue("@BrojDokumenta", BrojDok);
                //cmd.Parameters.AddWithValue("@Datum", dateTimePicker1.Value);
                //cmd.Parameters.AddWithValue("@Opis", nSkladiste);
                //cmd.Parameters.AddWithValue("@ID_OrganizacionaStrukturaView", Program.idOrgDeo);
                //cmd.Parameters.AddWithValue("@Proknjizeno", "NijeProknjizeno");
                //cmd.Parameters.AddWithValue("@MesecPoreza", dateTimePicker1.Value.Month);

                //if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");

                //string selectdokumenta = "SELECT ID_Dokumenta from Dokumenta where BrojDokumenta ='" + BrojDok + "'";

                pidDok = IdDokView;   ///db.ReturnInt(selectdokumenta, 0);

                string insertprometrecepcije = "INSERT INTO PrometMaloprodaje(ID_DokumentaView,ID_OrganizacionaStruktura,ID_Skladiste,UUser)VALUES(@ID_DokumentaView,@ID_OrganizacionaStruktura,@ID_Skladiste,@UUser)";
                cmd = new SqlCommand(insertprometrecepcije);
                cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
                cmd.Parameters.AddWithValue("@ID_OrganizacionaStruktura", Program.idOrgDeo);
                cmd.Parameters.AddWithValue("@ID_Skladiste", pSkladiste);
                cmd.Parameters.AddWithValue("@UUser", Program.idkadar);
                if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");
                labela:


                //    while (pDatum == Convert.ToDateTime(row["Datum"]) && pSkladiste == Convert.ToInt32(row["ID_Skladiste"]) && pNeoporezivo == Convert.ToInt32(row["Neoporezivo"])) {

                if ((PNacinPlacanja != Convert.ToInt32(row["ID_NacinPlacanja"]) && Convert.ToString(row["Oznaka"]) != "K" && pOznaka != "G") || (PNacinPlacanja != Convert.ToInt32(row["ID_NacinPlacanja"]) && Convert.ToString(row["Oznaka"]) != "1" & pOznaka != "0"))
                {
                    if (Ukolicina > 0) ZapisiSlogPrometa(pidDok, PArtikl, pCena, Ukolicina, pDatum, pSoba, pKomitent, PSkladisteProizvodnje);
                    ZapisiSlogPlacanjaMaloprodaje(pOznaka, pNeoporezivo, PNacinPlacanja, pidDok);
                    if (Program.imeFirme == "Leotar" || Program.imeFirme.Contains("otel"))
                    {
                        db.ExecuteStoreProcedure("AzurirajCenuSirovinaZaPromet", "IdDokView:" + pidDok);
                    }

                    //db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);

                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PDVPrometMaloprodaje", "IdDokument:" + pidDok);
                    db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:" + pidDok);
                    goto labela2;
                }
                if (PArtikl != Convert.ToInt32(row["ID_Artikal"]) || pCena != Convert.ToDouble(row["Cena"]) || pKomitent != Convert.ToInt32(row["ID_Komitent"]) || pSoba != Convert.ToInt32(row["ID_Soba"]) || (pKomitent > 1 && PRacun != Convert.ToInt32(row["Racun"])))
                {


                    ZapisiSlogPrometa(pidDok, PArtikl, pCena, Ukolicina, pDatum, pSoba, pKomitent, PSkladisteProizvodnje);
                    Ukolicina = 0;
                    Ppopust = Convert.ToDouble(row["Popust"]);
                    pSoba = Convert.ToInt32(row["ID_Soba"]);
                    pKomitent = Convert.ToInt32(row["ID_Komitent"]);
                    PRacun = Convert.ToInt32(row["Racun"]);
                    PArtikl = Convert.ToInt32(row["id_Artikal"]);
                    pCena = Convert.ToDouble(row["Cena"]);
                    PSkladisteProizvodnje = Convert.ToInt32(row["ID_SkladistaProizvodnje"]);
                }
                pp = pp + 1;
                decimal d = 100 * ((decimal)pp / (decimal)dtpro.Rows.Count);
                progressBar1.Value = Convert.ToInt32(Math.Round(d));
                Ukolicina = Ukolicina + Convert.ToInt32(row["Kolicina"]);
                continue;

                // }
            }
            if (Ukolicina > 0) ZapisiSlogPrometa(pidDok, PArtikl, pCena, Ukolicina, pDatum, pSoba, pKomitent, PSkladisteProizvodnje);
            ZapisiSlogPlacanjaMaloprodaje(pOznaka, pNeoporezivo, PNacinPlacanja, pidDok);
            if (Program.imeFirme == "Leotar" || Program.imeFirme.Contains("otel"))
            {
                db.ExecuteStoreProcedure("AzurirajCenuSirovinaZaPromet", "IdDokView:" + pidDok);
            }


            //db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);

            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PDVPrometMaloprodaje", "IdDokument:" + pidDok);
            Timer timer = new Timer();

            db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:" + pidDok);

            label7.Text = "Prepisano slogova:" + pp.ToString();
            label7.Visible = true;


        }

        private void ZapisiSlogPrometa(int pidDok, int pArtikl, double pCena, double pKolicina, DateTime pDatum, int ID_Soba, int ID_Komitent, int pID_SkladisteProizvodnje)
        {
            DataBaseBroker db = new DataBaseBroker();
            SqlCommand cmd = new SqlCommand();
            string inspromal = "INSERT into PrometMaloprodajeStavke(ID_DokumentaView,ID_ArtikliView,ProdajnaCena,StvarnaProdajnaCena,Kolicina,ValutaPlacanja,ID_Soba,ID_KomitentiView,ID_SkladisteProizvodnje) VALUES (@ID_DokumentaView,@ID_ArtikliView,@ProdajnaCena,@StvarnaProdajnaCena,@Kolicina,@ValutaPlacanja,@ID_Soba,@ID_KomitentiView,@ID_SkladisteProizvodnje)";
            cmd = new SqlCommand(inspromal);
            cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
            cmd.Parameters.AddWithValue("@ID_ArtikliView", pArtikl);
            cmd.Parameters.AddWithValue("@ProdajnaCena", pCena);
            cmd.Parameters.AddWithValue("@StvarnaProdajnaCena", pCena);
            cmd.Parameters.AddWithValue("@Kolicina", pKolicina);
            cmd.Parameters.AddWithValue("@ValutaPlacanja", pDatum);
            cmd.Parameters.AddWithValue("@ID_Soba", ID_Soba);
            cmd.Parameters.AddWithValue("@ID_KomitentiView", ID_Komitent);
            cmd.Parameters.AddWithValue("@ID_SkladisteProizvodnje", pID_SkladisteProizvodnje);


            if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");

        }
        private void ZapisiSlogPlacanjaMaloprodaje(string pOznaka, int pNeoporezivo, int ID_NacinPlacanja, int pidDok)
        {
            DataBaseBroker db = new DataBaseBroker();
            DataTable dt;
            string rsnaplate;
            if (pOznaka.Trim() == "O")
            {
                rsnaplate = "select ID_NacinPlacanja,ID_Kartica,sum(cena) as ukupno from PrometPreneseni  "
              + " Where VrstaSloga='T' and Oznaka='" + pOznaka.Trim() + "' and Neoporezivo=" + pNeoporezivo
              + " And ID_NacinPlacanja=" + ID_NacinPlacanja
              + " Group by oznaka,ID_NacinPlacanja,ID_Kartica";
                dt = db.ReturnDataTable(rsnaplate);
                if (dt.Rows.Count == 0)
                {
                    if (pOznaka.Trim() == "K")
                    {

                        rsnaplate = "SELECT ID_NacinPlacanja,ID_Kartica,sum(cena) as ukupno from PrometPreneseni  "
                + " Where VrstaSloga='T' and Oznaka='G' and Neoporezivo=" + pNeoporezivo
                + " Group by oznaka,ID_NacinPlacanja,ID_Kartica";
                        dt = db.ReturnDataTable(rsnaplate);

                    }
                }

            }
            else
            {
                rsnaplate = "select ID_NacinPlacanja,ID_Kartica,sum(cena) as ukupno from PrometPreneseni  "
                  + " Where VrstaSloga='T' and Oznaka='" + pOznaka.Trim() + "' and Neoporezivo=" + pNeoporezivo
                  + " Group by oznaka,ID_NacinPlacanja,ID_Kartica";
                dt = db.ReturnDataTable(rsnaplate);

            }
            foreach (DataRow row in dt.Rows)
            {
                SqlCommand cmd = new SqlCommand();
                string inspromalsta = "INSERT into PrometMaloprodajePlacanjeStavke(ID_DokumentaView,ID_NacinPl,Iznos,ID_Kartica) VALUES (@ID_DokumentaView,@ID_NacinPl,@Iznos,@ID_Kartica)";
                cmd = new SqlCommand(inspromalsta);
                cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
                cmd.Parameters.AddWithValue("@ID_NacinPl", Convert.ToInt32(row["ID_NacinPlacanja"]));
                cmd.Parameters.AddWithValue("@Iznos", Convert.ToDouble(row["Ukupno"]));
                cmd.Parameters.AddWithValue("@ID_Kartica", Convert.ToInt32(row["ID_Kartica"]));


                if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");



            }

        }
        bool proveraNormativaICena(int ID_Artikal, DateTime Datum, int ID_SkladisteProizvodnje, ref string poruka)
        {

            DataBaseBroker db2 = new DataBaseBroker();
            poruka = "";
            // provera postojanja normativa
            string normativ = "SELECT ID_SirovinaView,NazivProizvoda,NazivSirovine from NormativTotali where Id_ProizvodView=" + ID_Artikal;
            DataTable dtnormativ = db2.ReturnDataTable(normativ);
            if (dtnormativ.Rows.Count == 0)
            {
                string nazart = "SELECT NazivArtikla FROM Artikli where ID_Artikli=" + ID_Artikal;
                DataTable dtnazart = db2.ReturnDataTable(nazart);
                string naziv = db2.ReturnString(nazart, 1);
                if (dtnazart.Rows.Count != 0)
                {
                    poruka = "Ne postoji normativ za: " + naziv + " Unesite ga !!!";

                }
                else
                {
                    poruka = "Ne postoji normativ za: " + ID_Artikal.ToString() + " Unesite ga !!!";
                }

                return false;

            }
            // provera postojanja cena sirovina za sirovine 
            foreach (DataRow row in dtnormativ.Rows)
            {
                string cene = "";
                if (Program.imeFirme == "Leotar")
                {
                    cene = " SELECT c.ProsecnaNabavnaCena, a.NazivArtikla  " +
               " FROM ceneartikalanaskladistimapred  as c,artikli as a ,Cenovnik as ce " +
               " Where  ce.ID_Skladiste=" + ID_SkladisteProizvodnje + " And ce.ID_ArtikliView=" + ID_Artikal +
               " AND c.ID_Skladiste=ce.ID_SkladisteProizvodnje " +
               " AND c.ID_ArtikliView =" + Convert.ToInt32(row["ID_SirovinaView"]) +
               " AND c.datum = (select max(datum) from ceneartikalanaskladistimapred " +
               " WHERE Id_Skladiste=ce.ID_SkladisteProizvodnje and datum<='" + Datum + "'" +
               " AND (ulaz <>0 or izlaz <> 0) And ID_ArtikliView=" + Convert.ToInt32(row["ID_SirovinaView"]) + ")" +
               " AND (ulaz <>0 or izlaz <> 0)";

                }
                else
                {

                    cene = " SELECT c.ProsecnaNabavnaCena, a.NazivArtikla ,s.NazivSkl" +
                " FROM ceneartikalanaskladistimapred  as c,artikli as a ,skladiste as s " +
                " Where  c.ID_ArtikliView = a.ID_Artikli " +
                " AND  c.id_skladiste= s.ID_Skladiste " +
                " AND c.datum = (select max(datum) from ceneartikalanaskladistimapred " +
                " WHERE Id_Skladiste=" + ID_SkladisteProizvodnje + " and datum<='" + Datum + "'" +
                " AND (ulaz <>0 or izlaz <> 0) And ID_ArtikliView=" + Convert.ToInt32(row["ID_SirovinaView"]) + ")" +
                " AND (ulaz <>0 or izlaz <> 0) and c.Id_Skladiste=" + ID_SkladisteProizvodnje + " AND c.ID_ArtikliView=" + Convert.ToInt32(row["ID_SirovinaView"]);

                }
                DataTable dtcene = db2.ReturnDataTable(cene);
                if (dtcene.Rows.Count == 0)
                {
                    poruka = " Ne postoji sirovina na stanju: " + row["NazivSirovine"].ToString() + " za proizvod: " + row["NazivProizvoda"].ToString() + " prodat datuma : " + Datum;
                    return false;
                }
                DataRow rowcena = dtcene.Rows[0];
                if (Convert.ToInt32(rowcena["ProsecnaNabavnaCena"]) == 0)
                {
                    poruka = "Cena za: " + row["NazivSirovine"].ToString() + "=0 popravite!!" + "\n" + " sirovina: " + row["NazivProizvoda"].ToString();
                    return false;


                }

            }
            return true;
        }


        void prepisiCenovnik(string filepath, string skladiste)
        {
            DataBaseBroker db = new DataBaseBroker();
            Cenovnik cn = new Cenovnik();
            string select = "";
            DataTable dt;
            int rr = 0;
            if (filepath.Contains("Recepci") == false)
            {
                if (Program.imeFirme == "Bankom")
                {
                    select = " SELECT * from CenovnikMaloprodaja where NazivSkl='" + skladiste + "'";
                }
                else
                {
                    if (Program.imeFirme == "Bioprotein" || Program.imeFirme == "Leotar")
                    {

                        select = "SELECT * from CenovnikPoGrupama where NazivSkl='" + skladiste + "'";
                    }
                    else
                    {
                        select = "SELECT * from CenovnikPoGrupamaMaloprodaja where NazivSkl='" + skladiste + "'";

                    }

                }

            }
            else // jeste recepcija 
            {
                select = " select DISTINCT c.id_skladiste,s.NazivSkl,c.Id_ArtikliView,c.ProdajnaCena,"
                   + " c.ID_SkladisteProizvodnje,sp.NazivSkl as NazivSkladistaProizvodnje, "
                   + " a.NazivArt,a.JedinicaMere,CAST(a.StaraSifra AS int) AS StaraSifra ,"
                   + " a.ID_TarifaPoreza,a.TarifaPoreza as NazivPoreza ,a.PoreskaStopa,a.Grupa "
                   + " From Cenovnik as c,ArtikliTotali as a ,Skladiste as s, Skladiste as sp "
                   + " where c.ID_Skladiste=s.ID_Skladiste and "
                   + " c.ID_ArtikliView >1  and  "
                   + " c.ID_SkladisteProizvodnje=sp.ID_Skladiste and "
                   + " a.ID_ArtikliTotali=c.ID_ArtikliView and "
                   + " s.NazivSkl='" + skladiste + "'"
                   + " order by c.ID_Skladiste,c.ID_ArtikliView";


            }
            dt = db.ReturnDataTable(select);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoje cene u cenovniku za odabranu lokaciju!");
                return;
            }
            progressBar1.Visible = true;
            StreamWriter sw = new StreamWriter(filepath);
            foreach (DataRow row in dt.Rows)
            {
                if (Convert.ToInt32(row["ID_SkladisteProizvodnje"]) == 1)
                {
                    if (row["NazivSkl"].ToString().Contains("Diskont") == true) { }
                    else
                    {
                        MessageBox.Show("Nije popunjeno skladiste proizvodnje za " + row["NazivArt"].ToString() + " Popinte i ponovitepostupak");
                        return;
                    }
                }
                if (DBNull.Value == row["ProdajnaCena"])
                {
                    MessageBox.Show("Unesite cenu za : " + row["NazivArt"].ToString());
                    return;
                }
                else
                {
                    if (Convert.ToInt32(row["ProdajnaCena"]) == 0)
                    {
                        MessageBox.Show("Unesite cenu za : " + row["NazivArt"].ToString());
                        continue;



                    }

                }

                cn.ProdajnaCena = row["ProdajnaCena"].ToString().ToCharArray();
                rr = rr + 1;
                decimal d = 100 * ((decimal)rr / (decimal)dt.Rows.Count);
                progressBar1.Value = Convert.ToInt32(Math.Round(d));
                cn.ID_Skladiste = row["ID_Skladiste"].ToString().ToCharArray();
                cn.ID_SkladisteProizvodnje = row["ID_SkladisteProizvodnje"].ToString().ToCharArray();
                cn.ID_ArtikliView = row["ID_ArtikliView"].ToString().ToCharArray();
                cn.NazivSkladista = row["NazivSkl"].ToString().ToCharArray();
                cn.NazivArtikla = row["NazivArt"].ToString().ToCharArray();
                cn.JedinicaMere = row["JedinicaMere"].ToString().ToCharArray();
                cn.StaraSifra = row["StaraSifra"].ToString().ToCharArray();
                cn.NazivSkladistaProizvodnje = row["NazivSkladistaProizvodnje"].ToString().ToCharArray();
                cn.ID_Poreza = row["ID_TarifaPoreza"].ToString().ToCharArray();
                cn.NazivPoreza = row["NazivPoreza"].ToString().ToCharArray();
                cn.StopaPoreza = row["PoreskaStopa"].ToString().ToCharArray();
                cn.Grupa = row["Grupa"].ToString().ToCharArray();


                string str = new string(cn.ID_ArtikliView);
                sw.Write(
                str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cn.ID_Skladiste);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cn.ProdajnaCena);
                sw.Write(str.PadRight(16, ' '));
                sw.Write(' ');
                str = new string(cn.NazivArtikla);
                sw.Write(str.PadRight(100, ' '));
                sw.Write(' ');
                str = new string(cn.NazivSkladista);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(cn.JedinicaMere);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cn.StaraSifra);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(cn.ID_SkladisteProizvodnje);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cn.NazivSkladistaProizvodnje);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(cn.ID_Poreza);
                sw.Write(str.PadRight(2, ' '));
                sw.Write(' ');
                str = new string(cn.NazivPoreza);
                sw.Write(str.PadRight(40, ' '));
                sw.Write(' ');
                str = new string(cn.StopaPoreza);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cn.Grupa);
                sw.Write(str.PadRight(50, ' '));
                sw.Write(' ');
                sw.Write('\r');
                sw.Write('\n');
            }
            label7.Text = "Prepisano slogova:" + rr.ToString();

            sw.Close();

            if (Program.imeFirme == "Hotel Nevski") return;

            //  File.Encrypt(filepath);
            string mailTo;
            if (Program.imeFirme == "Bankom")
            {
                mailTo = "pero.trklja@bankom.rs";
            }
            else
            {
                mailTo = "breska@bankom.rs";
            }
            //mailTo = "milutin.spaic@bankom.rs";
            string from = "vladimir.veselinovic@bankom.rs";
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);
            message.To.Add(new MailAddress(mailTo));
            //message.Attachments.Add(new Attachment(putanjaFajla + ".enc"));
            message.Subject = "Novi Cenovnik";
            message.BodyEncoding = Encoding.UTF8;
            message.Body = "Promenjene su maloprodajne cene, preuzmite fajl iz foldera Prenosi i prenesite u program maloprodaje.";
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("mail.bankom.rs", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("vladimir.veselinovic@bankom.rs", "@W397r+GR#");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        void prepisiCenovnikRecepcije(string filepath, string skladiste)
        {
            CenovnikRecepcije cr = new CenovnikRecepcije();
            DataBaseBroker db = new DataBaseBroker();
            string cene = "SELECT * from CenovnikView   order by ID_Objekat,ID_KategorijaSobe,ID_VrstaGosta,ID_SkladisteProdaje,ID_ArtikliView";
            int rr = 0;
            DataTable dtcene = db.ReturnDataTable(cene);
            if (dtcene.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoje cene u cenovniku za odabranu lokaciju!");
                return;

            }
            progressBar1.Visible = true;
            StreamWriter sw = new StreamWriter(filepath);
            foreach (DataRow row in dtcene.Rows)
            {
                if (Convert.ToInt32(row["ProdajnaCena"]) == 0)
                {
                    MessageBox.Show("Unesite cenu za : " + row["NazivArtikla"]);
                    continue; ;
                }
                cr.ProdajnaCena = row["ProdajnaCena"].ToString().ToCharArray();
                rr = rr + 1;
                decimal d = 100 * ((decimal)rr / (decimal)dtcene.Rows.Count);
                progressBar1.Value = Convert.ToInt32(Math.Round(d));
                cr.ID_Objekat = row["ID_Objekat"].ToString().ToCharArray();
                if (Convert.ToInt32(row["ID_SkladisteProizvodnje"]) == 1)
                {
                    cr.ID_SkladisteProizvodnje = "131".ToCharArray();
                }
                else
                {
                    cr.ID_SkladisteProizvodnje = row["ID_SkladisteProizvodnje"].ToString().ToCharArray();

                }
                cr.ID_Usluga = row["ID_ArtikliView"].ToString().ToCharArray();
                cr.ID_VrstaGosta = row["ID_VrstaGosta"].ToString().ToCharArray();
                cr.ID_KategorijaSobe = row["ID_KategorijaSobe"].ToString().ToCharArray();
                cr.NazivSkladistaProizvodnje = row["SkladisteProizvodnje"].ToString().ToCharArray();
                cr.NazivUsluge = row["NazivArtikla"].ToString().ToCharArray();
                cr.ID_MestoProdaje = row["ID_SkladisteProdaje"].ToString().ToCharArray();
                cr.JedinicaMere = row["JedinicaMere"].ToString().ToCharArray();
                cr.NazivObjekta = row["NazivObjekta"].ToString().ToCharArray();
                cr.KategorijaSobe = row["KategorijaSobe"].ToString().ToCharArray();
                cr.VrstaGosta = row["VrstaGosta"].ToString().ToCharArray();
                cr.ID_Poreza = row["ID_TarifaPoreza"].ToString().ToCharArray();
                cr.ID_ProizvodView = row["ID_ProizvodView"].ToString().ToCharArray();
                cr.ID_JedinicaMere = row["ID_JedinicaMere"].ToString().ToCharArray();
                cr.NazivPoreza = row["TarifaPoreza"].ToString().ToCharArray();
                cr.StopaPoreza = row["Stopa"].ToString().ToCharArray();
                cr.Grupa = row["Grupa"].ToString().ToCharArray();
                cr.kod = row["kodnormativa"].ToString().ToCharArray();
                cr.UkljucenDaNe = row["YesNo"].ToString().ToCharArray();



                string str = new string(cr.NazivUsluge);
                sw.Write(
                str.PadRight(60, ' '));
                sw.Write(' ');
                str = new string(cr.JedinicaMere);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cr.NazivObjekta);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(cr.KategorijaSobe);
                sw.Write(str.PadRight(40, ' '));
                sw.Write(' ');
                str = new string(cr.ProdajnaCena);
                sw.Write(str.PadRight(16, ' '));
                sw.Write(' ');
                str = new string(cr.VrstaGosta);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(cr.ID_Usluga);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cr.ID_JedinicaMere);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cr.ID_Objekat);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cr.ID_KategorijaSobe);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cr.ID_VrstaGosta);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cr.ID_SkladisteProizvodnje);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cr.NazivSkladistaProizvodnje);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(cr.ID_MestoProdaje);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cr.ID_ProizvodView);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cr.ID_Poreza);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cr.NazivPoreza);
                sw.Write(str.PadRight(30, ' '));
                sw.Write(' ');
                str = new string(cr.StopaPoreza);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(cr.Grupa);
                sw.Write(str.PadRight(30, ' '));
                sw.Write(' ');
                str = new string(cr.kod);
                sw.Write(str.PadRight(1, ' '));
                sw.Write(' ');
                str = new string(cr.UkljucenDaNe);
                sw.Write(str.PadRight(1, ' '));
                sw.Write(' ');

                sw.Write('\r');
                sw.Write('\n');
            }
            label7.Text = "Prepisano slogova:" + rr.ToString();
            label7.Visible = true;

            sw.Close();




        }
        void prepisiSobe(string filepath)
        {

            Sobe sob = new Sobe();
            DataBaseBroker db = new DataBaseBroker();
            int rr = 0;
            string s = "SELECT * from Soba where ID_Soba > 1 order by BrojSobe";
            DataTable dt = db.ReturnDataTable(s);
            progressBar1.Visible = true;
            StreamWriter sw = new StreamWriter(filepath);
            foreach (DataRow row in dt.Rows)
            {
                rr = rr + 1;
                decimal d = 100 * ((decimal)rr / (decimal)dt.Rows.Count);
                progressBar1.Value = Convert.ToInt32(Math.Round(d));

                sob.ID_Soba = row["ID_Soba"].ToString().ToCharArray();
                sob.BrojSobe = row["BrojSobe"].ToString().ToCharArray();
                sob.ID_KategorijaSobe = row["ID_KategorijaSobe"].ToString().ToCharArray();
                sob.ID_Objekat = row["ID_Objekat"].ToString().ToCharArray();
                sob.ID_Status = row["ID_Statusi"].ToString().ToCharArray();
                if (!String.IsNullOrEmpty(row["Sprat"].ToString())) sob.Sprat = row["Sprat"].ToString().ToCharArray(); else sob.Sprat = "1".ToCharArray();
                if (!String.IsNullOrEmpty(row["DodatniLezaj"].ToString())) sob.DodatniLezaj = row["DodatniLezaj"].ToString().ToCharArray(); else sob.DodatniLezaj = "0".ToCharArray();
                if (!String.IsNullOrEmpty(row["DodatniPodaci"].ToString())) sob.DodatniPodaci = row["DodatniPodaci"].ToString().ToCharArray(); else sob.DodatniPodaci = "".ToCharArray();


                string str = new string(sob.ID_Soba);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(sob.BrojSobe);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(sob.ID_KategorijaSobe);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(sob.ID_Objekat);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(sob.ID_Status);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(sob.Sprat);
                sw.Write(str.PadRight(2, ' '));
                sw.Write(' ');
                str = new string(sob.DodatniLezaj);
                sw.Write(str.PadRight(1, ' '));
                sw.Write(' ');
                str = new string(sob.DodatniPodaci);
                sw.Write(str.PadRight(50, ' '));
                sw.Write(' ');

                sw.Write('\r');
                sw.Write('\n');

            }
            label7.Text = "Prepisano soba:" + rr.ToString();
            label7.Visible = true;
            sw.Close();



        }
        void prepisiZaposlene(string filepath, string skladiste)
        {

            Zaposleni zap = new Zaposleni();
            DataBaseBroker db = new DataBaseBroker();
            int idos;
            int rr = 0;
            string os = "SELECT ID_OrganizacionaStruktura from OrganizacionaStruktura where Naziv='" + skladiste + "'";
            DataTable dt1 = db.ReturnDataTable(os);


            DataRow dr = dt1.Rows[0];
            if (dt1.Rows.Count != 0)
            {
                idos = Convert.ToInt32(dr["ID_OrganizacionaStruktura"]);
            }
            else
            {
                label4.Text = "Ne postoji organizaciona struktura sa odabranim nazivom! ";
                return;
            }
            string kadrovskaevidencija = "SELECT k.* from KadrovskaEvidencija as k, KadroviIOrganizacionaStruktura as ko, OrganizacionaStruktura as os "
            + " WHERE ko.ID_OrganizacionaStruktura=os.ID_OrganizacionaStruktura and "
            + "  k.ID_KadrovskaEvidencija=ko.ID_KadrovskaEvidencija and "
            + " os.ID_OrganizacionaStruktura=" + idos
            + " order by ime";
            DataTable dt2 = db.ReturnDataTable(kadrovskaevidencija);
            if (dt2.Rows.Count == 0)
            {
                MessageBox.Show("Pogresan izbor lokacije!");
                return;
            }
            progressBar1.Visible = true;
            StreamWriter sw = new StreamWriter(filepath);
            foreach (DataRow row in dt2.Rows)
            {
                rr = rr + 1;
                decimal d = 100 * ((decimal)rr / (decimal)dt2.Rows.Count);
                progressBar1.Value = Convert.ToInt32(Math.Round(d));

                zap.ID_Zaposleni = row["ID_KadrovskaEvidencija"].ToString().ToCharArray();
                zap.Ime = row["Ime"].ToString().ToCharArray();
                zap.Prezime = row["Prezime"].ToString().ToCharArray();
                zap.Suser = row["Suser"].ToString().ToCharArray();
                zap.Pass = row["Pass"].ToString().ToCharArray();

                string str = new string(zap.ID_Zaposleni);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(zap.Ime);
                sw.Write(str.PadRight(50, ' '));
                sw.Write(' ');
                str = new string(zap.Prezime);
                sw.Write(str.PadRight(50, ' '));
                sw.Write(' ');
                str = new string(zap.Suser);
                sw.Write(str.PadRight(50, ' '));
                sw.Write(' ');
                str = new string(zap.Pass);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');

                sw.Write('\r');
                sw.Write('\n');
            }
            label7.Text = "Prepisano slogova: " + rr.ToString();
            sw.Close();


        }
        void prepisiNacine(string filepath)
        {
            int rr = 0;
            DataBaseBroker db = new DataBaseBroker();
            NaciniPlacanja np = new NaciniPlacanja();
            string nacinPlacanja = "SELECT *  from NaciniPlacanja where ID_NaciniPlacanja > 1 order by ID_NaciniPlacanja";
            DataTable dt = db.ReturnDataTable(nacinPlacanja);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("");
                return;
            }
            progressBar1.Visible = true;
            StreamWriter sw = new StreamWriter(filepath);
            foreach (DataRow row in dt.Rows)
            {
                rr = rr + 1;
                decimal d = 100 * ((decimal)rr / (decimal)dt.Rows.Count);
                progressBar1.Value = Convert.ToInt32(Math.Round(d));
                np.ID_NacinPlacanja = row["ID_NaciniPlacanja"].ToString().ToCharArray();
                np.NacinPlacanja = row["NaciniPlacanja"].ToString().ToCharArray();

                string str = new string(np.ID_NacinPlacanja);
                sw.Write(str.PadRight(5, ' '));
                sw.Write(' ');
                str = new string(np.NacinPlacanja);
                sw.Write(str.PadRight(50, ' '));
                sw.Write(' ');
                sw.Write('\r');
                sw.Write('\n');


            }
            label7.Text = "Prepisano nacina placanja: " + rr;
            //MessageBox.Show("Prepisano nacina placanja" + rr);
            sw.Close();
        }
        void prepisiNarudzbenicuKupca(string filepath, string nazivkom)
        {
            int ID_DokumentaStablo;
            string pDokument = "NarudzbenicaKupca";
            DateTime dt = DateTime.Now;
            string pDatum = dt.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            DataBaseBroker db = new DataBaseBroker();
            string provera = "SELECT * from DokumentaStablo where Naziv = 'NarudzbenicaKupca'";
            DataTable dt1 = db.ReturnDataTable(provera);


            DataRow dr = dt1.Rows[0];
            if (dt1.Rows.Count != 0)
            {
                ID_DokumentaStablo = Convert.ToInt32(dr["ID_DokumentaStablo"]);
            }
            else
            {
                label7.Text = "Nije registrovan dokument narudzbenica kupca! ";
                return;
            }
            label4.Text = "Upisivanje narudzbenice kupca je u toku ";
            string BrojDok = "";
            int IdDokView;
            clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
            IdDokView = os.UpisiDokument(ref BrojDok, nazivkom, ID_DokumentaStablo, pDatum.ToString());

            //string insertdokumenta = "INSERT INTO Dokumenta(RedniBroj,ID_KadrovskaEvidencija,ID_Predhodni,ID_LikvidacijaDokumenta,ID_DokumentaStablo,BrojDokumenta,Datum,Opis,ID_OrganizacionaStrukturaView,Proknjizeno,MesecPoreza) VALUES (@RedniBroj,@ID_KadrovskaEvidencija,@ID_Predhodni,@ID_LikvidacijaDokumenta,@ID_DokumentaStablo,@BrojDokumenta,@Datum,@Opis,@ID_OrganizacionaStrukturaView,@Proknjizeno,@MesecPoreza)";
            //SqlCommand cmd = new SqlCommand(insertdokumenta);
            //cmd.Parameters.AddWithValue("@RedniBroj", rbr);
            //cmd.Parameters.AddWithValue("@ID_KadrovskaEvidencija", Program.idkadar);
            //cmd.Parameters.AddWithValue("@ID_Predhodni", 1);
            //cmd.Parameters.AddWithValue("@ID_LikvidacijaDokumenta", 1);
            //cmd.Parameters.AddWithValue("@ID_DokumentaStablo", ID_DokumentaStablo);
            //cmd.Parameters.AddWithValue("@BrojDokumenta", BrojDok);
            //cmd.Parameters.AddWithValue("@Datum", dt);
            //cmd.Parameters.AddWithValue("@Opis", nazivkom);
            //cmd.Parameters.AddWithValue("@ID_OrganizacionaStrukturaView", Program.idOrgDeo);
            //cmd.Parameters.AddWithValue("@Proknjizeno", "NeKnjiziSe");
            //cmd.Parameters.AddWithValue("@MesecPoreza", dt.Month);

            //if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");
            int piddok;
            //string sql1 = "SELECT ID_Dokumenta from Dokumenta where BrojDokumenta ='" + BrojDok + "'";
            piddok = IdDokView;//db.ReturnInt(sql1, 0); // nulta kolona, a ne prva!!  
            string rskupac = "SELECT ID_KomitentiView from KomitentiView where NazivKom= '" + nazivkom + "'";
            DataTable dtkupac = db.ReturnDataTable(rskupac);
            int idkomitent;
            if (dtkupac.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoji izabrani kupac");
                return;

            }
            else
            {
                DataRow row = dtkupac.Rows[0];
                idkomitent = Convert.ToInt32(row["ID_KomitentiView"]);
            }
            string insertpredracun = "INSERT INTO Predracun(ID_DokumentaView,ID_Skladiste,OpcijaPredracuna,BrUr,ID_KomitentiView,UUser) VALUES (@ID_DokumentaView,@ID_Skladiste,@OpcijaPredracuna,@BrUr,@ID_KomitentiView,@UUser)";
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand(insertpredracun);
            cmd.Parameters.AddWithValue("@ID_DokumentaView", piddok);
            cmd.Parameters.AddWithValue("@ID_Skladiste", 23);
            cmd.Parameters.AddWithValue("@OpcijaPredracuna", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@BrUr", " ");
            cmd.Parameters.AddWithValue("@ID_KomitentiView", idkomitent);
            cmd.Parameters.AddWithValue("@UUser", Program.idkadar);

            if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");



            Microsoft.Office.Interop.Excel.Application MyApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook MyBook = MyApp.Workbooks.Open(filepath);
            Microsoft.Office.Interop.Excel.Worksheet MySheet = MyBook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range MyRange = MySheet.UsedRange;
            //MessageBox.Show(MyRange.Rows.Count.ToString());

            for (int i = 2; i <= MyRange.Rows.Count; i++)
            {


                if (MyRange.Cells[i, 2] != null && MyRange.Cells[i, 2].Value != null && MyRange.Cells[i, 5].Value != null)
                {
                    string artikal = "SELECT ID_ArtikliView,ID_Pakovanje from ArtikliView where StaraSifra= '" + MyRange.Cells[i, 2].Value + "'";
                    DataTable dtartikal = db.ReturnDataTable(artikal);
                    if (dtartikal.Rows.Count == 0)
                    {

                        MessageBox.Show("Ne postoji artikal sa sifrom!" + MyRange.Cells[i, 2].Value);
                    }
                    else
                    {
                        DataRow drart = dtartikal.Rows[0];
                        string insertpredracunstavke = "INSERT INTO PredracunStavke (ID_DokumentaView,Kolicina,ID_ArtikliView,ID_JedinicaMere,KolicinaPoDokumentu,Paleta,UUser) VALUES (@ID_DokumentaView,@Kolicina,@ID_ArtikliView,@ID_JedinicaMere,@KolicinaPoDokumentu,@Paleta,@UUser)";
                        cmd = new SqlCommand(insertpredracunstavke);
                        cmd.Parameters.AddWithValue("@ID_DokumentaView", piddok);
                        cmd.Parameters.AddWithValue("@Kolicina", MyRange.Cells[i, 5].Value);
                        cmd.Parameters.AddWithValue("@ID_ArtikliView", drart["ID_ArtikliView"]);
                        cmd.Parameters.AddWithValue("@ID_JedinicaMere", drart["ID_Pakovanje"]);
                        cmd.Parameters.AddWithValue("@KolicinaPoDokumentu", 1);
                        cmd.Parameters.AddWithValue("@Paleta", 1);
                        cmd.Parameters.AddWithValue("@UUser", Program.idkadar);

                        if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");
                    }

                }


            }

            // GC.Collect();
            // GC.WaitForPendingFinalizers();


            Marshal.ReleaseComObject(MyRange);
            Marshal.ReleaseComObject(MySheet);

            //close and release
            object misValue = System.Reflection.Missing.Value;
            MyBook.Close(false, misValue, misValue);
            Marshal.ReleaseComObject(MyBook);

            //quit and release
            MyApp.Quit();
            Marshal.ReleaseComObject(MyApp);

            //db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + piddok);
            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:NarudzbenicaKupca", "IdDokument:" + piddok);
            label4.Text = "";
            MessageBox.Show("Prepisana narudzbenica !!!");



        }
        void prepisiOpcijeCenovnika(string filepath)
        {
            DataBaseBroker db = new DataBaseBroker();
            OpcijeCenovnika oc = new OpcijeCenovnika();
            int rr = 0;
            string opccen = "SELECT * from  OpcijeCenovnika";
            DataTable dt = db.ReturnDataTable(opccen);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoje definisane opcije cenovnika");
                return;

            }
            progressBar1.Visible = true;
            StreamWriter sw = new StreamWriter(filepath);
            foreach (DataRow row in dt.Rows)
            {
                rr = rr + 1;
                decimal d = 100 * ((decimal)rr / (decimal)dt.Rows.Count);
                progressBar1.Value = Convert.ToInt32(Math.Round(d));
                oc.ID_ArtikliView = row["ID_ArtikliView"].ToString().ToCharArray();
                oc.ID_OpcijeCenovnika = row["ID_OpcijeCenovnika"].ToString().ToCharArray();
                oc.ID_Opcije = row["ID_Opcije"].ToString().ToCharArray();
                oc.ID_OperacijaOpcije = row["ID_OperacijaOpcije"].ToString().ToCharArray();
                oc.Procenat = row["Procenat"].ToString().ToCharArray();
                oc.GranicaOd = row["GranicaOd"].ToString().ToCharArray();
                oc.GranicaDo = row["GranicaDo"].ToString().ToCharArray();
                oc.KodOpcije = row["KodOpcije"].ToString().ToCharArray();

                string str = new string(oc.ID_ArtikliView);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(oc.ID_OpcijeCenovnika);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(oc.ID_Opcije);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(oc.ID_OperacijaOpcije);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(oc.Procenat);
                sw.Write(str.PadRight(5, ' '));
                sw.Write(' ');
                str = new string(oc.GranicaOd);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(oc.GranicaDo);
                sw.Write(str.PadRight(10, ' '));
                sw.Write(' ');
                str = new string(oc.KodOpcije);
                sw.Write(str.PadRight(5, ' '));
                sw.Write(' ');
                sw.Write('\r');
                sw.Write('\n');

            }
            label7.Text = "Prepisano slogova " + rr.ToString();
            sw.Close();

        }
        void prepisiProdajnaMesta(String filepath)
        {
            DataBaseBroker db = new DataBaseBroker();
            ProdajnaMesta pm = new ProdajnaMesta();
            int rr = 0;
            string pmesta = "SELECT * from Skladiste Where OpisSkladista like 'Maloprod%' or OpisSkladista like 'Usluge%' or OpisSkladista like '%staj%'";
            DataTable dt = db.ReturnDataTable(pmesta);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoje definisana mesta prodaje");
                return;

            }
            progressBar1.Visible = true;

            StreamWriter sw = new StreamWriter(filepath);
            foreach (DataRow row in dt.Rows)
            {
                rr = rr + 1;
                decimal d = 100 * ((decimal)rr / (decimal)dt.Rows.Count);
                progressBar1.Value = Convert.ToInt32(Math.Round(d));
                pm.ID_Skladiste = row["ID_Skladiste"].ToString().ToCharArray();
                pm.ID_OpisSkladista = row["ID_OpisSkladista"].ToString().ToCharArray();
                pm.NazivSkl = row["NazivSkl"].ToString().ToCharArray();
                pm.Kapacitet = row["Kapacitet"].ToString().ToCharArray();
                pm.OpisSkladista = row["OpisSkladista"].ToString().ToCharArray();
                pm.ID_Status = row["ID_Statusi"].ToString().ToCharArray();

                string str = new string(pm.ID_Skladiste);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(pm.ID_OpisSkladista);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(pm.NazivSkl);
                sw.Write(str.PadRight(50, ' '));
                sw.Write(' ');
                str = new string(pm.Kapacitet);
                sw.Write(str.PadRight(5, ' '));
                sw.Write(' ');
                str = new string(pm.OpisSkladista);
                sw.Write(str.PadRight(30, ' '));
                sw.Write(' ');
                str = new string(pm.ID_Status);
                sw.Write(str.PadRight(5, ' '));
                sw.Write(' ');

                sw.Write('\r');
                sw.Write('\n');



            }

            label7.Text = "Prepisano slogova:" + rr.ToString();
            label7.Visible = true;
            sw.Close();
        }
        void prepisiKomplete(string filepath)
        {

            DataBaseBroker db = new DataBaseBroker();
            KompletiUsluga ku = new KompletiUsluga();
            int rr = 0;
            string kompleti = "SELECT * from KompletiUslugaTotali where ID_KompletiUslugaTotali > 1";
            DataTable dt = db.ReturnDataTable(kompleti);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoje definisani kompleti usluga");
                return;

            }
            progressBar1.Visible = true;
            StreamWriter sw = new StreamWriter(filepath);
            foreach (DataRow row in dt.Rows)
            {
                rr = rr + 1;
                decimal d = 100 * ((decimal)rr / (decimal)dt.Rows.Count);
                progressBar1.Value = Convert.ToInt32(Math.Round(d));
                ku.ID_ProizvodView = row["ID_ProizvodView"].ToString().ToCharArray();
                ku.ID_SirovinaView = row["ID_SirovinaView"].ToString().ToCharArray();
                ku.KolicinaKompleta = row["KolicinaKompleta"].ToString().ToCharArray();
                ku.KolicinaUsluge = row["Kolicina"].ToString().ToCharArray();
                if (Convert.ToInt32(row["ProcenatUcesca"]) == 1 || Convert.ToInt32(row["ProcenatUcesca"]) == 0) ku.ProcenatUcesca = "100".ToCharArray();
                else ku.ProcenatUcesca = row["ProcenatUcesca"].ToString().ToCharArray();

                string str = new string(ku.ID_ProizvodView);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(ku.ID_SirovinaView);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(ku.KolicinaKompleta);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(ku.KolicinaUsluge);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(ku.ProcenatUcesca);
                sw.Write(str.PadRight(5, ' '));
                sw.Write(' ');

                sw.Write('\r');
                sw.Write('\n');




            }
            label7.Text = "Prepisano slogova:" + rr.ToString();
            label7.Visible = true;
            sw.Close();


        }
        void prepisiKomitente(string filepath)
        {
            DataBaseBroker db = new DataBaseBroker();
            Komitenti kom = new Komitenti();
            progressBar1.Visible = true;
            int rr = 0;
            string destination;
            string source = filepath;

            StreamWriter sw = new StreamWriter(filepath);
            string komitenti = "SELECT * from KomitentiTotali WHERE CCopy=0 ORDER BY NazivKom";
            DataTable dt = db.ReturnDataTable(komitenti);

            foreach (DataRow row in dt.Rows)
            {
                rr = rr + 1;
                decimal d = 100 * ((decimal)rr / (decimal)dt.Rows.Count);
                progressBar1.Value = Convert.ToInt32(Math.Round(d));
                kom.ID_Komitenti = row["ID_KomitentiTotali"].ToString().ToCharArray();
                if (!String.IsNullOrEmpty(row["NazivKom"].ToString())) kom.NazivKomitenta = row["NazivKom"].ToString().ToCharArray(); else kom.NazivKomitenta = " ".ToCharArray();
                if (!String.IsNullOrEmpty(row["Adresa"].ToString())) kom.Adresa = row["Adresa"].ToString().ToCharArray(); else kom.Adresa = " ".ToCharArray();
                if (!String.IsNullOrEmpty(row["Mesto"].ToString())) kom.Mesto = row["Mesto"].ToString().ToCharArray(); else kom.Mesto = " ".ToCharArray();
                if (!String.IsNullOrEmpty(row["Ptt"].ToString())) kom.Ptt = row["Ptt"].ToString().ToCharArray(); else kom.Ptt = " ".ToCharArray();
                if (Program.imeFirme == "Leotar")
                {
                    if (!String.IsNullOrEmpty(row["RegistarskiBroj"].ToString())) kom.PIB = row["RegistarskiBroj"].ToString().ToCharArray(); else kom.PIB = " ".ToCharArray();
                }
                else
                {
                    if (!String.IsNullOrEmpty(row["PIB"].ToString())) kom.PIB = row["PIB"].ToString().ToCharArray(); else kom.PIB = " ".ToCharArray();
                }

                kom.ID_Zemlja = row["ID_Zemlja"].ToString().ToCharArray();


                string str = new string(kom.ID_Komitenti);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(kom.NazivKomitenta);
                sw.Write(str.PadRight(80, ' '));
                sw.Write(' ');
                str = new string(kom.Mesto);
                sw.Write(str.PadRight(50, ' '));
                sw.Write(' ');
                str = new string(kom.Adresa);
                sw.Write(str.PadRight(50, ' '));
                sw.Write(' ');
                str = new string(kom.Ptt);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(kom.PIB);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');
                str = new string(kom.ID_Zemlja);
                sw.Write(str.PadRight(20, ' '));
                sw.Write(' ');

                sw.Write('\r');
                sw.Write('\n');



            }
            string PutanjaKomitenti = "";
            sw.Close();
            StreamReader sr = new StreamReader(@"\\BANKOMW\Repozitorijum\ISBankom\XXXX\xxxx.ini");
            string line;
            line = sr.ReadLine();


            while (line != null)
            {
                if (line != "")
                {
                    char[] separator = { ' ' };
                    string[] slogovi = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (slogovi[0].Length > 15 && slogovi[0].Substring(0, 16) == "PutanjaKomitenti")
                    {
                        PutanjaKomitenti = slogovi[0].Substring(17, 31);
                        break;
                    }
                }
                line = sr.ReadLine();
            }
            if (PutanjaKomitenti != "")
            {

                destination = PutanjaKomitenti + "Komitenti.txt";
                File.Copy(source, destination, true);
            }

            label7.Text = "Zavrsen prepis komitenata:" + rr.ToString();
            label7.Visible = true;
            sr.Close();


        }
        void prepisiPrometRecepcije(string str)
        {


            int pp;
            int ID_DokumentaStablo;

            int rr = 0; // broj prepisanih slogova
            DataBaseBroker db = new DataBaseBroker();
            string del = "DELETE from PrometPreneseni";
            bool postojiPromet = false;
            bool ispravan = true;
            DataTable imapromet = new DataTable();
            SqlCommand cmd = new SqlCommand(del);
            db.Comanda(cmd);

            List<PrometRecepcije> list = new List<PrometRecepcije>();

            StreamReader sr = new StreamReader(putanjaFajla);
            KonvertujFajluKlasu(sr, list);
            sr.Close();


            string sel = "SELECT Datum from PrometRecepcijeTotali where Convert(date,Datum)='" + kojiDatum + "' and ID_Skladiste=" + list[0].ID_Skladiste;
            DataTable dt = db.ReturnDataTable(sel);
            if (dt.Rows.Count != 0)
            {
                if (MessageBox.Show("Već je izvršen prenos prometa za izabrani datum", "Da li zelite ponoviti prepis?", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;

                }
                else
                {
                    postojiPromet = true;
                    string postoji = " SELECT Distinct BrojDokumenta as BrDok,ID_DokumentaView from  PrometRecepcije,Dokumenta where "
                  + " ID_Dokumenta=ID_DokumentaView AND Convert(date,Datum)='" + kojiDatum + "' and ID_Skladiste=" + list[0].ID_Skladiste + " Order by ID_DokumentaView";
                    imapromet = db.ReturnDataTable(postoji);

                }

            }
            for (int i = 0; i < list.Count(); i++)
            {
                string sql = "INSERT INTO PrometPreneseni(Datum,ID_Skladiste,id_Artikal,kolicina,Cena,ID_NacinPlacanja,ID_Komitent,ID_Kartica,ID_Soba,ID_Kelner,NazivSkladista,ID_SkladistaProizvodnje,Racun,Popust,OznakaValute,VrstaSloga,Oznaka,ID_MestoProdaje,Neoporezivo,ID_PosTerminal) VALUES (@Datum,@ID_Skladiste,@id_Artikal,@kolicina,@Cena,@ID_NacinPlacanja,@ID_Komitent,@ID_Kartica,@ID_Soba,@ID_Kelner,@NazivSkladista,@ID_SkladistaProizvodnje,@Racun,@Popust,@OznakaValute,@VrstaSloga,@Oznaka,@ID_MestoProdaje,@Neoporezivo,@ID_PosTerminal)";

                cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@Datum", dateTimePicker1.Value.Date);
                cmd.Parameters.AddWithValue("@ID_Skladiste", list[i].ID_Skladiste);
                cmd.Parameters.AddWithValue("@id_Artikal", list[i].ID_Artikal);
                cmd.Parameters.AddWithValue("@kolicina", list[i].kolicina);
                cmd.Parameters.AddWithValue("@Cena", list[i].cena);
                cmd.Parameters.AddWithValue("@ID_NacinPlacanja", list[i].ID_NacinPlacanja);
                cmd.Parameters.AddWithValue("@ID_Komitent", list[i].ID_Komitent);
                cmd.Parameters.AddWithValue("@ID_Kartica", list[i].ID_Kartica);
                cmd.Parameters.AddWithValue("@ID_Soba", list[i].ID_Soba);
                cmd.Parameters.AddWithValue("@ID_Kelner", list[i].ID_Kelner);
                cmd.Parameters.AddWithValue("@NazivSkladista", list[i].NazivSkladista);
                cmd.Parameters.AddWithValue("@ID_SkladistaProizvodnje", list[i].ID_SkladistaProizvodnje);
                cmd.Parameters.AddWithValue("@Racun", list[i].Racun);
                cmd.Parameters.AddWithValue("@Popust", list[i].Popust);
                cmd.Parameters.AddWithValue("@OznakaValute", list[i].OznakaValute);
                cmd.Parameters.AddWithValue("@VrstaSloga", list[i].VrstaSloga);
                cmd.Parameters.AddWithValue("@Oznaka", list[i].Oznaka);
                cmd.Parameters.AddWithValue("@ID_MestoProdaje", list[i].ID_MestoPredaje);
                cmd.Parameters.AddWithValue("@Neoporezivo", list[i].Neoporezivo);
                cmd.Parameters.AddWithValue("@ID_PosTerminal", list[i].ID_PosTerminal);
                if (db.Comanda(cmd) == "") rr = rr + 1;
                else MessageBox.Show("Greska u upitu!");


            }

            pp = 0;
            progressBar1.Visible = true;
            string provera = "SELECT * from DokumentaStablo where Naziv ='PrometRecepcije'";
            DataTable dt1 = db.ReturnDataTable(provera);


            DataRow dr = dt1.Rows[0];
            if (dt1.Rows.Count != 0)
            {
                ID_DokumentaStablo = Convert.ToInt32(dr["ID_DokumentaStablo"]);
            }
            else
            {
                label4.Text = "Nije registrovan dokument promet recepcije!";
                return;
            }

            int ix = 0;
            label7.Text = "Upisivanje prometa recepcije u toku!";
            /*  prometprepisani.Open " select * from PrometPreneseni where id_artikal>1 order by datum, Oznakavalute,neoporezivo,Oznaka,id_Artikal,ID_NacinPlacanja,ID_Komitent " _
                       , cnn1, adOpenDynamic, adLockOptimistic*/
            provera = "SELECT * from PrometPreneseni where id_Artikal > 1 order by Datum, OznakaValute,Neoporezivo,Oznaka,id_Artikal,ID_NacinPlacanja,ID_Komitent";
            dt = db.ReturnDataTable(provera);
            int prolaz = 0;
            int PArtikal = 1;
            int PKomitent = 0;
            int PNacinPlacanja = 0;
            int Ukolicina = 0;
            int pidDok = 0;
            int PSkladisteProdaje = 0;
            int PSkladisteProizvodnje = 0;
            double Ppopust = 0;
            int PRacun = 0;
            double pCena = 0;
            string pOznaka = "";
            int pNeoporezivo = 0;
            string pOznakaValute = "";
            string BrojDok = "";
            foreach (DataRow row in dt.Rows)
            {
                if (prolaz > 0) goto labela;
                labela2:
                DateTime pDatum = Convert.ToDateTime(row["Datum"]);
                pNeoporezivo = Convert.ToInt32(row["Neoporezivo"]);
                int pSkladiste = Convert.ToInt32(row["ID_Skladiste"]);
                string nSkladiste = Convert.ToString(row["NazivSkladista"]);
                PNacinPlacanja = Convert.ToInt32(row["ID_NacinPlacanja"]);
                PKomitent = Convert.ToInt32(row["ID_Komitent"]);
                PArtikal = Convert.ToInt32(row["id_Artikal"]);
                PRacun = Convert.ToInt32(row["Racun"]);
                pOznakaValute = Convert.ToString(row["OznakaValute"]);
                int pIdVal;
                string upit = "Select ID_SifrarnikValuta   from SifrarnikValuta where OznVal='" + pOznakaValute + "'";
                DataTable dtvaluta = db.ReturnDataTable(upit);
                DataRow dr2 = dtvaluta.Rows[0];
                if (dtvaluta.Rows.Count != 0)
                {
                    pIdVal = Convert.ToInt32(dr2["ID_SifrarnikValuta"]);
                }
                else
                {
                    pIdVal = 1;
                }
                pCena = Convert.ToDouble(row["Cena"]);
                pOznaka = Convert.ToString(row["Oznaka"]);
                PSkladisteProizvodnje = Convert.ToInt32(row["ID_SkladistaProizvodnje"]);
                PSkladisteProdaje = Convert.ToInt32(row["ID_MestoProdaje"]);
                Ppopust = Convert.ToDouble(row["Popust"]);
                Ukolicina = 0;
                string pdokument = "PrometRecepcije";
                if (postojiPromet == true)
                {
                    if (ix < imapromet.Rows.Count)
                    {

                        SqlCommand cmd1 = new SqlCommand();
                        DataRow row1 = imapromet.Rows[ix];// vrsimo prepravku vec unijetog prometa
                        BrojDok = Convert.ToString(row1["BrDok"]);
                        pidDok = Convert.ToInt32(row1["ID_DokumentaView"]);
                        string upit1 = "DELETE from PrometRecepcije Where ID_DokumentaView=" + pidDok;
                        cmd1 = new SqlCommand(upit1);
                        if (db.Comanda(cmd1) != "") MessageBox.Show("Greska prilikom inserta!");
                        upit1 = "DELETE from PrometRecepcijeStavke Where ID_DokumentaView=" + pidDok;
                        cmd1 = new SqlCommand(upit1);
                        if (db.Comanda(cmd1) != "") MessageBox.Show("Greska prilikom inserta!");
                        upit1 = "DELETE from PrometRecepcijePlacanjeStavke Where ID_DokumentaView=" + pidDok;
                        cmd1 = new SqlCommand(upit1);
                        if (db.Comanda(cmd1) != "") MessageBox.Show("Greska prilikom inserta!");
                        prolaz = prolaz + 1;
                        ix = ix + 1;
                        goto UPISZAGLAVLJE;
                    }
                }
                // Ne postoji promet pravimo novi dokument
                int IdDokView;
                clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                IdDokView = os.UpisiDokument(ref BrojDok, nSkladiste, ID_DokumentaStablo, dateTimePicker1.Text);

                pidDok = IdDokView;
                int rbr = 0;

                prolaz = prolaz + 1;

            UPISZAGLAVLJE: string insertprometrecepcije = "INSERT INTO PrometRecepcije(ID_DokumentaView,ID_OrganizacionaStruktura,ID_Skladiste,Neoporezivo,ID_SifrarnikValuta,UUser)VALUES(@ID_DokumentaView,@ID_OrganizacionaStruktura,@ID_Skladiste,@Neoporezivo,@ID_SifrarnikValuta,@UUser)";
                cmd = new SqlCommand(insertprometrecepcije);
                cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
                cmd.Parameters.AddWithValue("@ID_OrganizacionaStruktura", Program.idOrgDeo);
                cmd.Parameters.AddWithValue("@ID_Skladiste", pSkladiste);
                cmd.Parameters.AddWithValue("@Neoporezivo", pNeoporezivo);
                cmd.Parameters.AddWithValue("@ID_SifrarnikValuta", pIdVal);
                cmd.Parameters.AddWithValue("@UUser", Program.idkadar);


                if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");
                labela:

                if (PNacinPlacanja != Convert.ToInt32(row["ID_NacinPlacanja"]))
                {
                    if (Program.imeFirme == "Bankom" && (pOznaka.Trim() == "C" || Convert.ToString(row["Oznaka"]) == "O") || Program.imeFirme == "Leotar" && Convert.ToString(row["Oznaka"]) != "")
                    {
                        if (Ukolicina > 0) ZapisiSlogRecepcije(pidDok, PArtikal, pCena, Ukolicina, Ppopust, Program.idkadar, PKomitent, PRacun, PSkladisteProizvodnje, PSkladisteProdaje);
                        ZapisiSlogPlacanjaRecepcije(pOznaka, pNeoporezivo, pOznakaValute, pidDok);

                        //db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);

                        db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PrometRecepcije", "IdDokument:" + pidDok);
                        goto labela2;
                    }
                }

                if (PArtikal != Convert.ToInt32(row["ID_Artikal"]) || pCena != Convert.ToDouble(row["Cena"]) || PKomitent != Convert.ToInt32(row["ID_Komitent"]) || PSkladisteProdaje != Convert.ToInt32(row["ID_MestoProdaje"]) || Ppopust != Convert.ToInt32(row["Popust"]) || (PKomitent > 1 && PRacun != Convert.ToInt32(row["Racun"])))
                {


                    ZapisiSlogRecepcije(pidDok, PArtikal, pCena, Ukolicina, Ppopust, Program.idkadar, PKomitent, PRacun, PSkladisteProizvodnje, PSkladisteProdaje);
                    Ukolicina = 0;
                    PArtikal = Convert.ToInt32(row["id_Artikal"]);
                    pCena = Convert.ToDouble(row["Cena"]);
                    PKomitent = Convert.ToInt32(row["ID_Komitent"]);
                    PSkladisteProdaje = Convert.ToInt32(row["ID_MestoProdaje"]);
                    Ppopust = Convert.ToDouble(row["Popust"]);
                    PRacun = Convert.ToInt32(row["Racun"]);
                    PNacinPlacanja = Convert.ToInt32(row["ID_NacinPlacanja"]);
                    PSkladisteProizvodnje = Convert.ToInt32(row["ID_SkladistaProizvodnje"]);



                }
                pp = pp + 1;
                decimal d = 100 * ((decimal)pp / (decimal)dt.Rows.Count);
                progressBar1.Value = Convert.ToInt32(Math.Round(d));
                Ukolicina = Ukolicina + Convert.ToInt32(row["Kolicina"]);
                continue;


            }


            if (Ukolicina > 0) ZapisiSlogRecepcije(pidDok, PArtikal, pCena, Ukolicina, Ppopust, Program.idkadar, PKomitent, PRacun, PSkladisteProizvodnje, PSkladisteProdaje);
            ZapisiSlogPlacanjaRecepcije(pOznaka, pNeoporezivo, pOznakaValute, pidDok);

            //db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);

            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PrometRecepcije", "IdDokument:" + pidDok);



            label7.Text = "Prepisano slogova:" + pp.ToString();
            label7.Visible = true;

        }
        public void ZapisiSlogRecepcije(int pidDok, int pArtikal, double pCena, int pKolicina, double pPopust, int radnik, int pkomitent, int pRacun, int pID_SkladisteProizvodnje, int pID_SkladisteProdaje)
        {

            DataBaseBroker db = new DataBaseBroker();
            SqlCommand cmd = new SqlCommand();
            string inspromal = "INSERT into PrometRecepcijeStavke(ID_DokumentaView,ID_ArtikliView,ProdajnaCena,StvarnaProdajnaCena,Kolicina,ProcenatRabata,UUser,ID_KomitentiView,Racun,ID_SkladisteProizvodnje,ID_SkladisteProdaje) VALUES (@ID_DokumentaView,@ID_ArtikliView,@ProdajnaCena,@StvarnaProdajnaCena,@Kolicina,@ProcenatRabata,@UUser,@ID_KomitentiView,@Racun,@ID_SkladisteProizvodnje,@ID_SkladisteProdaje)";
            cmd = new SqlCommand(inspromal);
            cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
            cmd.Parameters.AddWithValue("@ID_ArtikliView", pArtikal);
            cmd.Parameters.AddWithValue("@ProdajnaCena", pCena);
            cmd.Parameters.AddWithValue("@StvarnaProdajnaCena", pCena);
            cmd.Parameters.AddWithValue("@Kolicina", pKolicina);
            cmd.Parameters.AddWithValue("@ProcenatRabata", pPopust);
            cmd.Parameters.AddWithValue("@UUser", 0);
            cmd.Parameters.AddWithValue("@ID_KomitentiView", pkomitent);
            cmd.Parameters.AddWithValue("@Racun", pRacun);
            cmd.Parameters.AddWithValue("@ID_SkladisteProizvodnje", pID_SkladisteProizvodnje);
            cmd.Parameters.AddWithValue("@ID_SkladisteProdaje", pID_SkladisteProdaje);


            if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");




        }
        public void ZapisiSlogPlacanjaRecepcije(string pOznaka, int pNeoporezivo, string OznakaValute, int pidDok)
        {


            DataBaseBroker db = new DataBaseBroker();
            DataTable dt;
            string rsnaplate;


            rsnaplate = "select ID_NacinPlacanja,ID_Kartica,sum(cena) as ukupno,OznakaValute,ID_PosTerminal from PrometPreneseni  "
              + " Where VrstaSloga='T' and Oznaka='" + pOznaka.Trim() + "' and Neoporezivo=" + pNeoporezivo
              + " And OznakaValute='" + OznakaValute.Trim() + "'"
              + " Group by oznaka,ID_NacinPlacanja,ID_Kartica,ID_PosTerminal,OznakaValute";
            dt = db.ReturnDataTable(rsnaplate);
            if (dt.Rows.Count == 0)
            {
                if (pOznaka.Trim() == "K" || pOznaka.Trim() == "1")
                {

                    rsnaplate = "SELECT ID_NacinPlacanja,ID_Kartica,ID_PosTerminal,sum(cena) as ukupno from PrometPreneseni  "
            + " Where VrstaSloga='T' and (Oznaka='G' or Oznaka='0') and Neoporezivo=" + pNeoporezivo
            + " Group by oznaka,ID_NacinPlacanja,ID_PosTerminal,ID_Kartica";
                    dt = db.ReturnDataTable(rsnaplate);

                }
            }

            foreach (DataRow row in dt.Rows)
            {
                SqlCommand cmd = new SqlCommand();
                string inspromalsta = "INSERT into PrometRecepcijePlacanjeStavke(ID_DokumentaView,ID_NacinPl,Iznos,ID_Kartica,ID_PosTerminal) VALUES (@ID_DokumentaView,@ID_NacinPl,@Iznos,@ID_Kartica,@ID_PosTerminal)";
                cmd = new SqlCommand(inspromalsta);
                cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
                cmd.Parameters.AddWithValue("@ID_NacinPl", Convert.ToInt32(row["ID_NacinPlacanja"]));
                cmd.Parameters.AddWithValue("@Iznos", Convert.ToDouble(row["Ukupno"]));
                cmd.Parameters.AddWithValue("@ID_Kartica", Convert.ToInt32(row["ID_Kartica"]));
                cmd.Parameters.AddWithValue("@ID_PosTerminal", Convert.ToInt32(row["ID_PosTerminal"]));

                if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");



            }

        }
        public void KonvertujFajluKlasu1(StreamReader sr, List<PrometSankova> list) // promet sankova
        {
            string line;
            line = sr.ReadLine();


            while (line != null)
            {
                //line.Replace("\0", string.Empty);
                PrometSankova ps = new PrometSankova();
                char[] separator = { ' ' };
                string[] slogovi = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < slogovi.Length; i++)
                {
                    slogovi[i] = slogovi[i].Trim('\0');

                }
                List<string> lista = new List<string>(slogovi);
                lista.Remove("");
                slogovi = lista.ToArray();

                ps.Datum = Convert.ToDateTime(slogovi[0]);
                ps.ID_Skladiste = Convert.ToInt32(slogovi[1]);
                ps.ID_Artikal = Convert.ToInt32(slogovi[2]);
                ps.ID_Komitent = Convert.ToInt32(slogovi[3]);
                ps.Kolicina = Convert.ToDouble(slogovi[4], System.Globalization.CultureInfo.InvariantCulture);
                ps.Cena = double.Parse((slogovi[5]), System.Globalization.CultureInfo.InvariantCulture);
                ps.Popust = double.Parse(slogovi[6], System.Globalization.CultureInfo.InvariantCulture);
                ps.ID_Soba = Convert.ToInt32(slogovi[7]);
                ps.ID_NacinPlacanja = Convert.ToInt32(slogovi[8]);
                ps.ID_Kelner = Convert.ToInt32(slogovi[9]);
                ps.NazivSkladista = slogovi[10] + ' ' + slogovi[11];
                ps.Racun = Convert.ToInt32(slogovi[12]);
                ps.ID_MestoProizvodnje = Convert.ToInt32(slogovi[13]);
                ps.ID_Kartica = Convert.ToInt32(slogovi[14]);
                ps.VrstaSloga = slogovi[15][0];
                ps.Oznaka = slogovi[15][1];
                ps.Neoporezivo = (slogovi[15][2]) - '0';

                list.Add(ps);

                line = sr.ReadLine();
            }


        }

        public void KonvertujFajluKlasu(StreamReader sr, List<PrometRecepcije> list)
        { // promet recepcije
            string line;
            line = sr.ReadLine();


            while (line != null)
            {
                PrometRecepcije pr = new PrometRecepcije();
                char[] separator = { ' ' };
                string[] slogovi = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < slogovi.Length; i++)
                {
                    slogovi[i] = slogovi[i].Trim('\0');

                }
                List<string> lista = new List<string>(slogovi);
                lista.Remove("");
                slogovi = lista.ToArray();


                pr.datum = Convert.ToDateTime(slogovi[0]);
                pr.ID_Skladiste = Convert.ToInt32(slogovi[1]);
                pr.ID_Artikal = Convert.ToInt32(slogovi[2]);
                pr.ID_Komitent = Convert.ToInt32(slogovi[3]);
                pr.kolicina = Convert.ToDouble(slogovi[4], System.Globalization.CultureInfo.InvariantCulture);
                pr.cena = double.Parse((slogovi[5]), System.Globalization.CultureInfo.InvariantCulture);
                pr.Popust = double.Parse(slogovi[6], System.Globalization.CultureInfo.InvariantCulture);
                pr.ID_Soba = Convert.ToInt32(slogovi[7]);
                pr.ID_NacinPlacanja = Convert.ToInt32(slogovi[8]);
                pr.ID_Kelner = Convert.ToInt32(slogovi[9]);
                pr.NazivSkladista = slogovi[10];
                pr.Racun = Convert.ToInt32(slogovi[11]);
                pr.ID_MestoPredaje = Convert.ToInt32(slogovi[12]);
                pr.ID_SkladistaProizvodnje = Convert.ToInt32(slogovi[13]);
                pr.ID_Kartica = Convert.ToInt32(slogovi[14]);
                pr.OznakaValute = slogovi[15];
                pr.VrstaSloga = slogovi[16][0];
                pr.Oznaka = slogovi[16][1];
                pr.Neoporezivo = (slogovi[16][2]) - '0';
                pr.ID_PosTerminal = (slogovi[16][3]) - '0';
                list.Add(pr);

                line = sr.ReadLine();
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            BankomMDI frm = new BankomMDI();
            frm.Show();
        }
    }
}
