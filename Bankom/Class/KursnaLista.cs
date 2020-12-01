using Bankom.Class;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Djora 28.11.19
namespace Bankom
{
    public partial class KursnaLista : Form
    {
        public KursnaLista()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Now;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        //Djora 23.09.19
        private void button1_Click(object sender, EventArgs e)
        {
            //Djora 02.12.19
            string Datum = dateTimePicker1.Value.ToString("dd.MM.yy");
            string NazivDokumenta = "KursnaLista";
            string brojDok = "";
            int rbr = 0;

            DataBaseBroker db = new DataBaseBroker();

            //Provera kursne liste
            string sel = " SELECT Id_KursnaLista FROM KursnaLista WHERE datum='" + Datum + "'";

            DataTable t = db.ReturnDataTable(sel);
            if (t.Rows.Count > 0)
            {
                MessageBox.Show("Kursna lista vec postoji !");
                goto Kraj;
            }


            int IdDokView;
            string opis = string.Concat("Kursna lista ", Datum);
            clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
            IdDokView = os.UpisiDokument(ref brojDok, opis, 61, Datum.ToString());
            // upisan dokument u dokumenta i izvrseni totali

            SqlCommand cmd = new SqlCommand();

            //Poziv Web servisa NBA i preuzimanje KursneListe u obliku XML-a
            rs.nbs.webservicesListe.ExchangeRateXmlService proxy2 = new rs.nbs.webservicesListe.ExchangeRateXmlService();
            rs.nbs.webservicesListe.AuthenticationHeader auth = new rs.nbs.webservicesListe.AuthenticationHeader();
            auth.LicenceID = Guid.Parse("15b91077-22f7-4e4c-87cd-c4a3756bee6e");
            auth.UserName = "bankomdoo";
            auth.Password = "bankomgroup";

            proxy2.AuthenticationHeaderValue = auth;

            proxy2.Url = "https://webservices.nbs.rs/CommunicationOfficeService1_0/ExchangeRateXmlService.asmx";

            //Kreiranje data seta na osnovu dobijenog xml-a
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();

            //Djora 19.05.20  primer: datumF = "20200515"
            string datumF = dateTimePicker1.Value.Date.ToString("yyyyMMdd");
            StringReader stringReaderSrednji = new StringReader(proxy2.GetExchangeRateByDate(datumF, 3));  //Srednji kurs
            StringReader stringReaderKupProd = new StringReader(proxy2.GetExchangeRateByDate(datumF, 1));   //Kupovni i prodajni kurs 
            StringReader stringReaderEfektiva = new StringReader(proxy2.GetExchangeRateByDate(datumF, 2));  //za Efektivu


            ds.ReadXml(stringReaderSrednji);
            ds2.ReadXml(stringReaderKupProd);
            ds3.ReadXml(stringReaderEfektiva);

            stringReaderSrednji.Dispose();
            stringReaderKupProd.Dispose();
            stringReaderEfektiva.Dispose();

            //Postavlja kolonu CurrencyCode kao primarni kljuc da bi kasnije, dole, mogo da koristim komandu Find
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = ds.Tables[0].Columns["CurrencyCode"];
            ds.Tables[0].PrimaryKey = keyColumns;

            ds.Tables[0].Columns.Add("KupovniZaDevize", typeof(String));
            ds.Tables[0].Columns.Add("ProdajniZaDevize", typeof(String));
            ds.Tables[0].Columns.Add("ID_SifrarnikValuta", typeof(String));


            string Paritet;
            string Kupovni;
            string Prodajni;
            string Srednji;
            string SifVal;
            string Verzija = "";
            string KupovniZaDevize;
            string ProdajniZaDevize;
            string IDSifVal;
            string OznVal;
            string sql = "";
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection con;
            con = new SqlConnection(Program.connectionString);
            con.Open();
            //Idi kroz r1 i nadji u r2  r1-KupProd  r2-Srednji
            foreach (DataRow r1 in ds2.Tables[0].Rows)
            {
                DataRow r2 = ds.Tables[0].Rows.Find(r1["CurrencyCode"]);
                //Ako nisi nasao onda nista
                if (r2 == null)
                {
                    //ds2.Tables[0].Rows.Add(r1["XID"], r1["X"]);
                }
                else
                {
                    r2["BuyingRate"] = r1["BuyingRate"];
                    r2["SellingRate"] = r1["SellingRate"];
                }
            }


            //Idi kroz r1 i nadji u r2  r1-ZaEfektivu  r2-Srednji
            foreach (DataRow r1 in ds3.Tables[0].Rows)
            {
                DataRow r2 = ds.Tables[0].Rows.Find(r1["CurrencyCode"]);
                //Ako nisi nasao onda nista
                if (r2 == null)
                {
                    //ds2.Tables[0].Rows.Add(r1["XID"], r1["X"]);
                }
                else
                {
                    r2["KupovniZaDevize"] = r1["BuyingRate"];
                    r2["ProdajniZaDevize"] = r1["SellingRate"];
                }
            }


            DataTable dt = db.ReturnDataTable("SELECT id_sifrarnikvaluta, SifVal FROM dbo.SifrarnikValuta");
            DataColumn[] keyColumns2 = new DataColumn[1];
            keyColumns2[0] = dt.Columns["SifVal"];
            dt.PrimaryKey = keyColumns2;

            //Idi kroz r1 i nadji u t  
            foreach (DataRow r1 in ds.Tables[0].Rows)
            {
                DataRow r2 = dt.Rows.Find(r1["CurrencyCodeNumChar"]);
                //Ako nisi nasao onda nista
                if (r2 == null)
                {
                    //ds2.Tables[0].Rows.Add(r1["XID"], r1["X"]);
                }
                else
                {
                    r1["ID_SifrarnikValuta"] = r2["id_sifrarnikvaluta"];

                }
            }


            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                Paritet = dr["Unit"].ToString();
                Kupovni = dr["BuyingRate"].ToString();
                Prodajni = dr["SellingRate"].ToString();
                Srednji = dr["MiddleRate"].ToString();
                SifVal = dr["CurrencyCode"].ToString();
                Verzija = dr["ExchangeRateListNumber"].ToString();
                //Datum = dr["Date"].ToString();
                KupovniZaDevize = (dr["KupovniZaDevize"].ToString() == "") ? "0" : dr["KupovniZaDevize"].ToString();
                ProdajniZaDevize = (dr["ProdajniZaDevize"].ToString() == "") ? "0" : dr["ProdajniZaDevize"].ToString();
                IDSifVal = dr["ID_SifrarnikValuta"].ToString();
                OznVal = dr["CurrencyCodeAlfaChar"].ToString();


                //string sql = "";
                //Insert u KursnaLista
                sql = "insert into KursnaLista(ID_SifrarnikValuta,ID_Zemlja, ID_DokumentaView, Datum, Paritet, Kupovni, Srednji, Prodajni, Dogovorni, KupovniZaDevize, ProdajniZaDevize, OznVal, Verzija) "
                                  + "values(" + IDSifVal + ", 4, " + IdDokView + ", '" + Datum.Trim() + "', " + Paritet + ",  " + Kupovni + ", " + Srednji + ", " + Prodajni + ", " + Srednji + ", " + KupovniZaDevize + ", " + ProdajniZaDevize + ", '" + OznVal + "', " + Verzija + " )";

                da.InsertCommand = new SqlCommand(sql, con);
                da.InsertCommand.ExecuteNonQuery();

            }

            //Unos RSD (domaca valuta)
            sql = "insert into KursnaLista(ID_SifrarnikValuta,ID_Zemlja, ID_DokumentaView, Datum, Paritet, Kupovni, Srednji, Prodajni, Dogovorni, KupovniZaDevize, ProdajniZaDevize, OznVal, Verzija) "
                                              + "values(1, 4, " + IdDokView + ", '" + Datum + "', 1,  1, 1, 1, 1, 1, 1, 'RSD', " + Verzija + " )";

            da.InsertCommand = new SqlCommand(sql, con);
            da.InsertCommand.ExecuteNonQuery();

            //Totali za KurnuListu
            string strTabela = "";
            string dokType = "";
            string strParams = "";
            string str = "";
            List<string[]> lista = new List<string[]>();

            str = "Execute TotaliZaDokument " + NazivDokumenta + "," + IdDokView.ToString();
            lista.Add(new string[] { str, strParams, strTabela, dokType, IdDokView.ToString() });
            lista.ToArray();
            db.ReturnSqlTransactionParamsFull(lista);

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = ds; // dataset
            dataGridView1.DataMember = "ExchangeRate";

            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumentaokumenta", "IdDokument:" + IdDokView.ToString());
            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:KursnaLista", "IdDokument:" + IdDokView.ToString());

        Kraj:;
        }

        private void KursnaLista_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
        }
    }
}