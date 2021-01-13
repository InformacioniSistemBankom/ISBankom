using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Bankom.Class;

namespace Bankom
{
    public partial class NoviLot : Form
    {

        string currentLot;
        public NoviLot()
        {
            InitializeComponent();
            rucnilot.Enabled = false;
           // izmena.Enabled = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeLot();
        }
        public NoviLot(DataGridViewRow dgrv)
        {
            InitializeComponent();
            rucnilot.Enabled = false;
            button1.Enabled = false;
            this.idArtikal.Enabled = false;
            this.artikli.Enabled = false;
            this.datumProizvodnje.Enabled = false;
            this.skladista.Enabled = false;


            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeLot();
            var db = new DataBaseBroker();
            char oznaka = findFirstLetterInString(dgrv.Cells[0].Value.ToString()) ;
            this.skladista.Text = db.ReturnString("SELECT NazivSkl FROM Skladiste where Oznaka ='" + oznaka +"'",0);
            var idArt = db.ReturnInt("SELECT ID_Artikli from Artikli where NazivArtikla = '" + dgrv.Cells[1].Value.ToString() + "'", 0);
            if (idArt != -1)
            {
                idArtikal.Text = Convert.ToString(idArt);
                artikli.SelectionStart = 0;
            }

            currentLot = dgrv.Cells[0].Value.ToString();
            this.artikli.Text = dgrv.Cells[1].Value.ToString();
            this.datumProizvodnje.Value =(DateTime) dgrv.Cells[2].Value;
            this.datumIsteka.Value = (DateTime)dgrv.Cells[3].Value;
            this.proizvodjaci.Text = dgrv.Cells[4].Value.ToString();
            this.zempro.Text = dgrv.Cells[5].Value.ToString();
            this.lotproizvodjaca.Text = dgrv.Cells[6].Value.ToString();




              

           


            
        }
        private char findFirstLetterInString(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsLetter(str[i])) return str[i];
               
            }
            return "".ToCharArray()[0];

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void InitializeLot()
        {
            skladista.AutoCompleteMode = AutoCompleteMode.None;
            skladista.AutoCompleteSource = AutoCompleteSource.ListItems;

            artikli.AutoCompleteMode = AutoCompleteMode.None;
            artikli.AutoCompleteSource = AutoCompleteSource.ListItems;

            proizvodjaci.AutoCompleteMode = AutoCompleteMode.None;
            proizvodjaci.AutoCompleteSource = AutoCompleteSource.ListItems;


            zempro.AutoCompleteMode = AutoCompleteMode.None;
            zempro.AutoCompleteSource = AutoCompleteSource.ListItems;


            datumIsteka.Format = DateTimePickerFormat.Custom;
            datumProizvodnje.Format = DateTimePickerFormat.Custom;
            datumProizvodnje.CustomFormat = "dd.MM.yyyy";
            datumIsteka.CustomFormat = "dd.MM.yyyy";
            // idArtikal.Enabled = false;

            using (SqlConnection sqlConnection = new SqlConnection(Program.connectionString))
            {

                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT ID_Zemlja, NazivZemlje FROM Zemlja where NazivZemlje <> ''", sqlConnection);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                zempro.DataSource = dataTable;
                zempro.ValueMember = "ID_Zemlja";
                zempro.DisplayMember = "NazivZemlje";
                zempro.SelectedIndex = -1;
                sqlConnection.Close();

            }



            using (SqlConnection sqlConnection = new SqlConnection(Program.connectionString))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT ID_Skladiste, NazivSkl, Oznaka FROM Skladiste WHERE Oznaka <> ''", sqlConnection);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                skladista.DataSource = dataTable;
                skladista.ValueMember = "Oznaka";
                skladista.DisplayMember = "NazivSkl";
                skladista.SelectedIndex = -1;
                sqlConnection.Close();


            }

            using (SqlConnection sqlConnection = new SqlConnection(Program.connectionString))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT  ID_Komitenti, NazivKomitenta FROM Proizvodjaci WHERE NazivKomitenta <> '' ", sqlConnection);
                DataTable dataTable = new DataTable();

                sqlDataAdapter.Fill(dataTable);


                proizvodjaci.DataSource = dataTable;


                proizvodjaci.ValueMember = "ID_Komitenti";
                proizvodjaci.DisplayMember = "NazivKomitenta";
                proizvodjaci.SelectedIndex = -1;
                sqlConnection.Close();

            }
        }

        

        // 
        public string Konverzija_u_yuscii(string word)
        {
            if (word.Trim() == String.Empty)
                return String.Empty;

            char[] unicode = { 'Đ', 'đ', 'Š', 'š', 'Ć', 'ć', 'Č', 'č', 'Ž', 'ž' };
            char[] ascii = { '\\', '|', '[', '{', ']', '}', '^', '~', '@', '`' };

            for (int i = 0; i < Math.Min(unicode.Length, ascii.Length); i++)
            {
                word = word.Replace(ascii[i], unicode[i]);
            }



            return word;
        }

        private void artikli_TextUpdate(object sender, EventArgs e)
        {
            var pom = artikli.Text.ToString();

            if (artikli.Text.Length < 2) return;



            using (SqlConnection sqlConnection = new SqlConnection(Program.connectionString))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("SELECT TOP 10 ID_Artikli, StaraSifra, NazivArtikla FROM Artikli where NazivArtikla like '%" + artikli.Text.ToString() + "%' and CCopy=0", sqlConnection);
                DataTable dataTable = new DataTable();

                sqlDataAdapter.Fill(dataTable);


                artikli.DataSource = dataTable;


                artikli.ValueMember = "NazivArtikla";
                artikli.DisplayMember = "NazivArtikla";
                artikli.SelectedIndex = -1;



                artikli.Text = pom;
                artikli.SelectionStart = artikli.Text.Length;
                sqlConnection.Close();

            }
        }

        private void artikli_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (artikli.SelectedIndex != -1) {
                DataBaseBroker db = new DataBaseBroker();

                var idArt = db.ReturnInt("SELECT ID_Artikli from Artikli where NazivArtikla = '" + artikli.SelectedValue + "'", 0);
                if (idArt != -1)
                {
                    idArtikal.Text = Convert.ToString(idArt);
                    artikli.SelectionStart = 0;
                }

            }

        }

        private void artikli_DropDown(object sender, EventArgs e)
        {
            artikli.SelectedIndex = -1;

        }

    

        private void idArtikal_Leave(object sender, EventArgs e)

        {
            if (Int32.TryParse(idArtikal.Text.ToString(), out int idArt))
            {
                DataBaseBroker db = new DataBaseBroker();

                var NazArt = db.ReturnString("SELECT NazivArtikla from Artikli where ID_Artikli = " + idArt, 0);
                if (NazArt != "" || NazArt != null)
                {
                    artikli.Text = NazArt;
                    artikli.SelectionStart = 0;
                }

            }
        }

       

        private void button1_Click(object sender, EventArgs e)
        {

            var greska = proveraLota();
            if (!String.IsNullOrEmpty(greska)) MessageBox.Show(greska);
            else UpisiNoviLot(Convert.ToInt32(zempro.SelectedValue), Convert.ToInt32(idArtikal.Text), datumProizvodnje.Value, datumIsteka.Value, Convert.ToInt32(proizvodjaci.SelectedValue), lotproizvodjaca.Text, skladista.SelectedValue.ToString());





        }

        public void UpisiNoviLot(int idZemlja,int idArtikal,DateTime datPro, DateTime datIst,int idProizvodjac,string njihovBarKod,string oznakaSkladista)

        {
           
           
            int redniBroj;
            using (SqlConnection sqlConnection = new SqlConnection(Program.connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    

                   command.CommandText = "SELECT MAX(rednibroj) as max FROM Lot where barkod like '%" + oznakaSkladista + "%' AND DatumProizvodnje = '" + datPro.ToLongDateString() + "'";
                   var reader = command.ExecuteScalar();

                    if (!Convert.IsDBNull(reader))
                    {
                        redniBroj = Convert.ToInt32(reader);
                        redniBroj++;
                    }
                    else
                    {
                        redniBroj = 1;
                    }

                    

                    string barKod = datPro.ToShortDateString().Replace(".", string.Empty).Trim() + oznakaSkladista + redniBroj + "A" + idArtikal;

                    if (idProizvodjac == 3 || idProizvodjac == 75)
                    {
                        if (String.IsNullOrEmpty(njihovBarKod) || njihovBarKod.Equals("N"))
                        {
                            njihovBarKod = barKod;
                        }


                    }

                    command.CommandText = $"INSERT INTO Lot ( DatumProizvodnje, RokTrajanja, DatumIsteka, ID_Komitenti, ID_Zemlja, NjihovLot, barkod, ID_Artikli, UUser,RedniBroj) VALUES('{datPro.Date}',0,'{datIst.Date}',{idProizvodjac},{idZemlja},'{njihovBarKod}','{barKod}',{idArtikal},{Program.idkadar},{redniBroj})";
                    command.ExecuteNonQuery();
                    var dialogResult = MessageBox.Show("Kreiran je novi lot!", "", MessageBoxButtons.OKCancel);

                    if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Cancel)
                    {
                        this.Close();
                       
                        //Lotovi.innerReference.dataGridViewPaging1.Initialize(Lotovi.innerReference.count());

                    }






                }


            }

        }

        public void updateLot(string lot, int idZemlja, DateTime datIst, int idProizvodjac, string njihovBarKod)
        {

            if (idProizvodjac == 3 || idProizvodjac == 75)
            {
                if (String.IsNullOrEmpty(njihovBarKod) || njihovBarKod.Equals("N"))
                {
                    njihovBarKod = lot;
                }
            }
                using (SqlConnection sqlConnection = new SqlConnection(Program.connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = $"UPDATE Lot SET  DatumIsteka='{datIst.Date}' ,ID_Komitenti={idProizvodjac}, ID_Zemlja={idZemlja}, NjihovLot='{njihovBarKod}' WHERE barkod = '{lot}'";

                    command.ExecuteNonQuery();
                    var dialogResult = MessageBox.Show("Izmenjen je postojeci lot!", "", MessageBoxButtons.OKCancel);

                    if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Cancel)
                    {
                        this.Close();
                        Lotovi.innerReference.dataGridViewPaging1.Initialize(Lotovi.innerReference.count());

                    }

                }

            }
        }
        private string proveraLota()
        {

            string greska = "";
            if (String.IsNullOrEmpty(idArtikal.Text) || Int32.TryParse(idArtikal.Text, out int number) == false)
            {
                greska += "Neispravan izbor artikla!";
                greska += System.Environment.NewLine;

            }
            if (skladista.SelectedIndex == -1)
            {
                greska += "Neispravan izbor skladišta!";
                greska += System.Environment.NewLine;
            }

            if (proizvodjaci.SelectedIndex == -1)
            {
                greska += "Neispravan izbor proizvodjaca!";
                greska += System.Environment.NewLine;
            }
            if (zempro.SelectedIndex == -1)
            {
                greska += "Neispravan izbor zemlje proizvodjača!";
                greska += System.Environment.NewLine;
            }

            if (datumProizvodnje.Value >= datumIsteka.Value)
            {
                greska += "Datum isteka ne može biti prije datuma proizvodnje!";
                greska += System.Environment.NewLine;
            }

            if ((int)proizvodjaci.SelectedValue != 3 && (int)proizvodjaci.SelectedValue != 75)
            {
                if (String.IsNullOrEmpty(lotproizvodjaca.Text))
                {
                    greska += "Popunite lot proizvodjaca! Ako artikal nema lot unesite veliko latinicno slovo N!";
                    greska += System.Environment.NewLine;
                }
                else
                {
                    string pattern = @"^[a-zA-Z0-9]+$";
                    Regex regex = new Regex(pattern);
                    if (!regex.IsMatch(lotproizvodjaca.Text))
                    {
                        greska += "Lot proizvodjaca moze sadrzati samo slova i brojeve";
                        greska += System.Environment.NewLine;
                    }

                }
            }
            return greska;

        }
        private void prekid_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void izmena_Click(object sender, EventArgs e)
        {
            var greska = proveraLota();
            if (!String.IsNullOrEmpty(greska)) MessageBox.Show(greska);
            //else izmeniLot();
        }

        private void izmena_Click_1(object sender, EventArgs e)
        {
            var greska = proveraLota();
            if (!String.IsNullOrEmpty(greska)) MessageBox.Show(greska);
            else updateLot(currentLot, Convert.ToInt32(zempro.SelectedValue), datumIsteka.Value, Convert.ToInt32(proizvodjaci.SelectedValue), lotproizvodjaca.Text);
        }

        
    }
}
