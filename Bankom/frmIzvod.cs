using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Bankom.Class;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Bankom
{

    public partial class frmIzvod : Form
    {
        public string strPutanjaPlacanja;
        public string mesecgodina;
        public int IdDokView;
        public string pOdakle = ""; //izbor radiobuttona;
        string strFileName = "";
        string Dokument = "PrometUplata";
        string odabranifajl = "";
        public string KojiPrepis = "";
        int Rr = 0;
        DataBaseBroker db = new DataBaseBroker();
        private DateTime ppdt;

        public frmIzvod()
        {
            InitializeComponent();
        }
        private void Izvod_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            toolStripLabel1.Text = strPutanjaPlacanja;
            toolLabel2.Text = mesecgodina;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;          
        }




        private void LoadFiles(string dir)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Text files (*.txt)|*.txt|Excel Files (*.xls)|*.xls;*.xlsx|Word Files (*.doc;*.docx;*.dot)|*.doc;*.docx;*.dot|Pdf Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
            string path = @"\\Sql2016\d\PlacanjaUplate\";
            //string path = @"D:\\server\PlacanjaUplate\";


            if (Directory.Exists(path))
            {
                openFileDialog.InitialDirectory = path;
            }
            //openFileDialog.WindowStartupLocation= WindowStartupLocation.CenterScreen;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {

                strFileName = openFileDialog.FileName;
                string folderPath = openFileDialog.InitialDirectory;
                this.Text = openFileDialog.SafeFileName;
                switch (openFileDialog.FilterIndex)
                {


                    case 1:
                        odabranifajl = openFileDialog.SafeFileName.ToString();
                        IzvodPoruka.Text = openFileDialog.SafeFileName.ToString();
                        //ProcessStartInfo startInfo = new ProcessStartInfo("notepad.exe");
                        //startInfo.WindowStyle = ProcessWindowStyle.Maximized;
                        //Process.Start(strFileName);

                        break;

                    case 2:
                        var excelApp = new Microsoft.Office.Interop.Excel.Application();
                        excelApp.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlNormal;
                        //excelApp.Top = Top + 108; ;
                        // excelApp.Width = Width-70;
                        string ime = excelApp.Name;
                        //excelApp.Height = Height-70;
                        excelApp.Visible = true;
                        Microsoft.Office.Interop.Excel.Workbooks books = excelApp.Workbooks;
                        //excelApp.Width = Width;
                        //excelApp.Height = Height;

                        excelApp.Workbooks.Open(strFileName);

                        break;
                    case 3:
                        Microsoft.Office.Interop.Word.Application wdApp = new Microsoft.Office.Interop.Word.Application();
                        wdApp.Visible = true;
                        wdApp.WindowState = Microsoft.Office.Interop.Word.WdWindowState.wdWindowStateNormal;
                        Microsoft.Office.Interop.Word.Document aDoc = wdApp.Documents.Open(strFileName);


                        break;
                    case 4:
                        WebBrowser wb = new WebBrowser();
                        wb.Top = Top - 40;
                        wb.Navigate(strFileName);

                        break;
                    case 5:
                        ProcessStartInfo startInfo1 = new ProcessStartInfo("");
                        startInfo1.WindowStyle = ProcessWindowStyle.Maximized;
                        Process.Start(strFileName);
                        break;

                }
                strPutanjaPlacanja = openFileDialog.InitialDirectory ;
            }

            //string[] Files = Directory.GetFiles(dir, "*.*");

            //foreach (string file in Files)
            //{
            //    if (file.Length < 260)
            //    {
            //        FileInfo fi = new FileInfo(file);
            //        TreeNode tds = td.Nodes.Add(fi.Name);
            //        tds.Tag = fi.FullName;
            //        tds.StateImageIndex = 1;
            //    }

            //}

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


      
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            LoadFiles(toolStripLabel1.Text);
        }

        private void toolLabel2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                LoadFiles(strPutanjaPlacanja);
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            pOdakle = "mail";
        }
        private void radioButton2_Click(object sender, EventArgs e)
        {
            pOdakle = "disk";
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            odabranifajl = "";
            IzvodPoruka.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            IzvodPoruka.Text = odabranifajl;
        }
        private void radioButton3_MouseMove(object sender, MouseEventArgs e)
        {
            IzvodPoruka.Text = "Pravi Kursnu Listu koja je ista kao izabrana K. L. za drugi datum";
        }
        private void RadioButton2_MouseMove(object sender, MouseEventArgs e)
        {
            IzvodPoruka.Text = "Ubacuje sve Kursne Liste koje nisu skinute a dosle -mailom";
        }
        private void radioButton3_Click(object sender, EventArgs e)
        {
            pOdakle = "baza";
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == false && radioButton2.Checked == false) { return; }
            //Dokument = strFileName;
            switch (pOdakle)
            {
                case "mail":
                    break;
                case "disk":
                    if (strFileName == "")
                    {
                        IzvodPoruka.Text = "Niste Izabrali sta prepisujete ";
                        return;
                    }



                    if (Dokument == "PrometUplata")
                    {
                        string putanja = strPutanjaPlacanja + odabranifajl;
                        IzvodPoruka.Text = PrepisUPlacanjaNaplate(putanja, Dokument, IdDokView);

              

                    }


                    break;
                case "baza":
                    this.Text = TransKLUDruguIUpisUBazu(dateTimePicker1.Text, dateTimePicker2.Text, strPutanjaPlacanja, IdDokView);
                    break;


            }
        }
        private string TransKLUDruguIUpisUBazu(string datum1, string datum2, string Ddok, int IdDok)
        {
            var lista = new List<string>();
            string transKLUDruguIUpisUBazu = "";


            DataTable RSImaNema = db.ReturnDataTable("select * from KursnaLista where Datum = '" + datum1 + "'");
            if (RSImaNema.Rows.Count == 0)
            {
                transKLUDruguIUpisUBazu = "Nema kursne liste za taj datum u bazi";
            }
            else
            {
                RSImaNema.Dispose();
                RSImaNema = db.ReturnDataTable("select * from KursnaLista where Datum = '" + datum2 + "'");
                if (RSImaNema.Rows.Count > 0)
                {
                    string str = "INSERT INTO KursnaLista(Datum, Paritet, Kupovni, Srednji,";
                    str += " Prodajni, Dogovorni, OBRADA, ID_SifrarnikValuta, Verzija, ";
                    str += " KupovniZaDevize, ProdajniZaDevize,OznVal, ID_Zemlja,ID_DokumentaView, ";
                    str += " UUser, TTime) SELECT '" + datum2 + "', Paritet, Kupovni, Srednji, Prodajni, ";
                    str += " Dogovorni, OBRADA, ID_SifrarnikValuta, Verzija, KupovniZaDevize,";
                    str += " ProdajniZaDevize,OznVal, ID_Zemlja," + IdDok.ToString();
                    str += " , UUser, TTime FROM KursnaLista WHERE Datum = '" + datum1 + "'";

                    lista.Add(str);
                    string rs = db.ExecuteSqlTransaction(lista);
                    lista.Clear();
                    if (rs != "") { transKLUDruguIUpisUBazu = rs; return rs; }
                }
                else { transKLUDruguIUpisUBazu = "Kursna Lista za taj datum vec postoji"; }

            }

            RSImaNema.Dispose();

            return transKLUDruguIUpisUBazu;
        }
        private string PrepisUPlacanjaNaplate(string NazivFajla, string mDok, int mIDdok)
        {
            if (Path.GetDirectoryName(NazivFajla).IndexOf("PlacanjaUplate") > -1 && Path.GetDirectoryName(NazivFajla).IndexOf("Preuzeto") == -1)
            {
                string strPreuzimanjePlacanjaC = NazivFajla.Replace("PlacanjaUplate\\", "PlacanjaUplate\\Preuzeto\\");
                if (File.Exists(strPreuzimanjePlacanjaC) == false) { File.Copy(NazivFajla, strPreuzimanjePlacanjaC); }
            }

          
            string prepisUPlacanjaNaplate = "";
            IZVHALKOMZ MIZ = new IZVHALKOMZ();
            IZVHALKOMS MI = new IZVHALKOMS();
            
            clsOperacije co = new clsOperacije();
            string pDd = "";
            DateTime pddt;            
            int pp = 0;
            string tr = "";        //' nas tekuci racun za koji preuzimamo izvod
            int bi = 0;     //   ' broj izvoda
            int idbanka = 0;  //' ID_bankaview iz pogleda BankaView koji dobijamo citanjem ovog pogleda po broju tekuceg racuna(tr) iz prve stavke
            int ID_Banka = 0;
            string Datotekaz = "";
            string str = "";
            DataTable rsPostoji = new DataTable();
            DataTable prometprepisani = new DataTable();
            List<string[]> lista = new List<string[]>();
            ID_Banka = 1;
            pp = 0;
            Rr = 1;
            bi = 0;



            if (KojiPrepis == "IZVOD")
            {
                Datotekaz = NazivFajla.Replace("_cov.txt",".txt");

                if (File.Exists(Datotekaz) == false) {File.Copy(NazivFajla, Datotekaz);}
                if (File.Exists(Datotekaz) == false) { return ""; }

                string ffileReader = File.ReadAllText(Datotekaz);

                int duzina = ffileReader.Length;

                MIZ.BrojRacuna = ffileReader.Substring(MIZ.VrstaStavke.Length, MIZ.BrojRacuna.Length);
                pDd = ffileReader.Substring(MIZ.VrstaStavke.Length + MIZ.BrojRacuna.Length, MIZ.DatumObrade.Length);
                tr = MIZ.BrojRacuna;
                MIZ.DatumObrade = pDd;
                str = pDd.Substring(0, 2) + "." + pDd.Substring(2, 2) + "." + pDd.Substring(4, 4);
                pddt = Convert.ToDateTime(str);
                MIZ.RedniBrojIzvoda = ffileReader.Substring(ffileReader.Trim().Length - 3, 3);
                MIZ.SumaOdobrenja = ffileReader.Substring(MIZ.VrstaStavke.Length + MIZ.BrojRacuna.Length + MIZ.DatumObrade.Length + MIZ.DatumPredhodnogIzvoda.Length + MIZ.StariSaldo.Length + MIZ.BrojTransakcijaZaduzenja.Length + MIZ.SumaZaduzenja.Length + MIZ.BrojTransakcijaOdobrenja.Length, MIZ.SumaOdobrenja.Length);
                MIZ.SumaZaduzenja = ffileReader.Substring(MIZ.VrstaStavke.Length + MIZ.BrojRacuna.Length + MIZ.DatumObrade.Length + MIZ.DatumPredhodnogIzvoda.Length + MIZ.StariSaldo.Length + MIZ.BrojTransakcijaZaduzenja.Length, MIZ.SumaZaduzenja.Length);
                MIZ.NoviSaldo = ffileReader.Substring(MIZ.VrstaStavke.Length + MIZ.BrojRacuna.Length + MIZ.DatumObrade.Length + MIZ.DatumPredhodnogIzvoda.Length + MIZ.StariSaldo.Length + MIZ.BrojTransakcijaZaduzenja.Length + MIZ.SumaZaduzenja.Length + MIZ.BrojTransakcijaOdobrenja.Length + MIZ.SumaOdobrenja.Length, MIZ.NoviSaldo.Length);
                MIZ.StariSaldo = ffileReader.Substring(MIZ.VrstaStavke.Length + MIZ.BrojRacuna.Length + MIZ.DatumObrade.Length + MIZ.DatumPredhodnogIzvoda.Length, MIZ.StariSaldo.Length);                
                bi = Convert.ToInt16(MIZ.RedniBrojIzvoda);





                DataTable dt = db.ReturnDataTable("Select ID_BankaView, ID_KomitentiView From BankaView Where TekuciRacun = '" + tr.Trim() + "'");
                if (dt.Rows.Count == 0)
                {
                    prepisUPlacanjaNaplate = "Tekuci racun " + tr + " nije registrovan ";
                    return prepisUPlacanjaNaplate;
                }
                else
                {

                    idbanka = (int)dt.Rows[0]["ID_BankaView"];
                    ID_Banka = (int)dt.Rows[0]["id_KomitentiView"];
                }
                dt.Dispose();


                string strParams2 = "";

                strParams2 = "@param1=" + idbanka.ToString() + "`";
                strParams2 += "@param2=" + pddt.ToString("dd.MM.yy") + "`";
                strParams2 += "@param3=" + bi.ToString();

                str = " select * from PlacanjaNaplate where ID_BankaView=" + idbanka.ToString() + " and Datum='" + pddt.ToString("dd.MM.yy") + "'" ;
                str += " and (OznakaKnjizenja='20' or OznakaKnjizenja='40' OR BrojIzvoda=" + bi.ToString() + ")";


                dt = db.ReturnDataTable(str);
                if (dt.Rows.Count > 0)
                {
                    str = "Delete from PlacanjaNaplate where ID_BankaView=@param1 and Datum='@param2";
                    str += "' and (OznakaKnjizenja='20' or OznakaKnjizenja='40' OR BrojIzvoda=@param3)";

                    lista.Clear();
                    lista.Add(new string[] { str, strParams2 });
               
                }


                strParams2 = "";
                strParams2 = "@param1=" + "T" + "`";
                strParams2 += "@param2=" + pddt + "`";
                strParams2 += "@param3=" + Convert.ToDouble(MIZ.SumaOdobrenja.ToString()) / 100 + "`";
                strParams2 += "@param4=" + Convert.ToDouble(MIZ.SumaZaduzenja.ToString()) / 100 + "`";
                strParams2 += "@param5=" + mIDdok + "`";
                strParams2 += "@param6=" + Convert.ToDouble(MIZ.NoviSaldo.ToString()) / 100 + "`";
                strParams2 += "@param7=" + Convert.ToDouble(MIZ.StariSaldo.ToString()) / 100 + "`";                
                strParams2 += "@param8=" + Convert.ToInt32(MIZ.RedniBrojIzvoda.ToString()) + "`";
                strParams2 += "@param9=" + idbanka.ToString();


                str = " INSERT INTO[dbo].[PlacanjaNaplate] ";
                str += " ([VrstaSloga],[Datum],[Uplate],[Isplate],[ID_DokumentaView],[NoviSaldo],[StariSaldo],[BrojIzvoda],[ID_BankaView]) ";
                str += " VALUES(@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9)";
                lista.Clear();
                lista.Add(new string[] { str, strParams2 });
                lista.ToArray();
                string rezultat3 = db.ReturnSqlTransactionParams(lista);
                lista.Clear();

                if (rezultat3 != "") { MessageBox.Show(rezultat3); return "Greska"; } 
                duzina = 0;
               

            
            }
            //////////////////////////////////////////////////////////////////////////////////////
           
        




            int R = 0;            
            NazivFajla =NazivFajla.Replace("*","");
            string[] fileReader = File.ReadAllLines(NazivFajla);
            int countline = fileReader.Length;
            lista.Clear();
            for (R = 0; R <= countline-1; R++)
            {
        pocetak:
                if (R == countline) {
                    break;
                }
                MI = VratiMi(R, NazivFajla);
                //if (MI.OznakaKnjizenja == "10" && MI.SifraPlacanja.Trim() == "") { break; }
                //if ((MI.OznakaKnjizenja == "10") && (MI.SvrhaPlacanja.ToUpper().Trim().IndexOf("TRANSAKCIONI DEPOZ") > 0)) { break; }
                //if (MI.SvrhaPlacanja.ToUpper().Trim().IndexOf("E-BANK") > 0) { break; }
                //if (MI.SvrhaPlacanja.ToUpper().Trim().IndexOf("E-SERV") > 0) { break; }
                //if (MI.SvrhaPlacanja.ToUpper().Trim().IndexOf("NAKNADA ZA VO") > 0) { break; }
                //if (MI.SvrhaPlacanja.ToUpper().Trim().IndexOf("PROVIZIJA") > 0) { break; }
                //if (MI.SvrhaPlacanja.ToUpper().Trim().IndexOf("USLUGE PP") > 0) { break; }
                if ( (co.IsNumeric(MI.OznakaKnjizenja.ToString()) == false ) || MI.OznakaKnjizenja.ToString() == "20" || MI.OznakaKnjizenja.ToString() == "40" )                {
                    R = R+1;goto pocetak;
                }


            

            
           
            if (R <= 1 || idbanka == 1)
                {
//                    if (rsPostoji != null) { rsPostoji = null; }
                    rsPostoji = db.ReturnDataTable(" Select ID_BankaView,ID_KomitentiView From BankaView Where TekuciRacun='" + MI.TekuciRacunKomitenta + "'");
                    // rsPostoji = db.ReturnDataTable(" Select ID_BankaView,ID_KomitentiView From BankaView Where TekuciRacun='" + MI.TekuciRacunKomitenta + "'");
                }

                if (rsPostoji.Rows.Count == 0)
                {
                    prepisUPlacanjaNaplate = "Tekuci racun " + MI.TekuciRacunKomitenta + " nije registrovan ";
                    return prepisUPlacanjaNaplate;
                }
                else
                {
                    idbanka = (int)rsPostoji.Rows[0]["ID_BankaView"];
                    ID_Banka = (int)rsPostoji.Rows[0]["id_KomitentiView"];
                    pDd = MI.DatumObrade;
                    pddt = Convert.ToDateTime((pDd));
                }
           
            ////BRISI///////////////////////////////////////////////
            prometprepisani = db.ReturnDataTable(" select * from PlacanjaNaplate where ID_DokumentaView=1");
            
            DateTime pddt1 = ppdt;
            string strParams4 = "";
            string vrstasloga = "";            
            string datum = "";
            string id_bankaView = "";
            string ispravniPodaciDaNe = prometprepisani.Rows[0]["IspravniPodaciDaNe"].ToString();
            string brojizvoda = prometprepisani.Rows[0]["BrojIzvoda"].ToString();
            string id_dokumentaView = prometprepisani.Rows[0]["ID_DokumentaView"].ToString();
            string oznakaknjizenja = MI.OznakaKnjizenja.ToString() + "`";
            string oblikplacanja = " ";
            string prenesenozaplacanje = "";
            string sifraplacanjasaizvoda ="";
            string id_svrhaplacanjaview = "509"; //' Naplata od kupca
            string id_komitentiView = "";
            string SvrhaPlacanjaSaIzvoda = MI.SifraPlacanja;
            string uplate = "";
            string isplate = "";
            string pozivnabrojsaizvoda = "";

            if (co.IsNumeric(MI.OblikPlacanja) == true)
            {
                oblikplacanja = "2";
            }
            else
            { oblikplacanja = MI.OblikPlacanja; }



            if (co.IsNumeric(MI.SifraPlacanja) == true)
            {
                prenesenozaplacanje = "1";
                sifraplacanjasaizvoda = "21";
                id_svrhaplacanjaview = "1";//  'nedefinisana usluga

                if (SvrhaPlacanjaSaIzvoda == "GOTOVINSKA ISPLATA")
                {
                    sifraplacanjasaizvoda = "66";
                    id_svrhaplacanjaview = "798";
                }
                if (MI.SvrhaPlacanja.ToUpper().Trim() == "NAKNADA ZA PLATNI PROMET" || (MI.SvrhaPlacanja.ToUpper().Trim() == "NAKNADA ZA VODENJE RACUNA"))
                {
                    sifraplacanjasaizvoda = "66";
                    id_svrhaplacanjaview = "798";
                }
                if (MI.SvrhaPlacanja.ToUpper().IndexOf("NAKNADE ZA VO") > 0 || (MI.SvrhaPlacanja.ToUpper().IndexOf("E-BANKING") > 0))
                {
                    sifraplacanjasaizvoda = "66";
                    id_svrhaplacanjaview = "798";
                }
                if (MI.SvrhaPlacanja.ToUpper().IndexOf("NAKNADE PO PARTIJI") > 0 || (MI.SvrhaPlacanja.ToUpper().IndexOf("PROVIZIJA") > 0) || (MI.SvrhaPlacanja.ToUpper().IndexOf("USLUGE PP") > 0))
                {
                    sifraplacanjasaizvoda = "66";
                    id_svrhaplacanjaview = "798";
                }
                if (MI.SvrhaPlacanja.ToUpper().Trim() == "NAPLATA GLAVNICE"
                    || MI.SvrhaPlacanja.ToString().IndexOf("RATE") > 0)
                {
                    sifraplacanjasaizvoda = "76";
                    id_svrhaplacanjaview = "1963";  //'kratkorocni kredit
                    id_komitentiView = ID_Banka.ToString();
                }

                if ((MI.SvrhaPlacanja.ToUpper().IndexOf("PROV") > 0) && (MI.SvrhaPlacanja.ToUpper().IndexOf("DOZ") > 0)
                   || (MI.SvrhaPlacanja.ToUpper().IndexOf("AKREDIT") > 0)) //'provizija za ino
                {
                    sifraplacanjasaizvoda = "21";
                    id_svrhaplacanjaview = "508"; //  'placanje dobavljacu za ino 508
                    id_komitentiView = ID_Banka.ToString();
                }
                if (MI.SvrhaPlacanja.ToUpper().IndexOf("GOTOVINSKA UPLA") > 0)
                {
                    sifraplacanjasaizvoda = "21";
                    id_svrhaplacanjaview = "797";
                }
                if (MI.SvrhaPlacanja.ToUpper().IndexOf("KAMAT") > 0)
                {
                    sifraplacanjasaizvoda = "72";
                    id_komitentiView = ID_Banka.ToString();



                    if (Convert.ToInt32(oznakaknjizenja) == 20)
                    {
                        if (Program.idFirme == 15)
                        {
                            id_svrhaplacanjaview = "2208"; //  'uplata kamate
                        }
                        else
                        {
                            id_svrhaplacanjaview = "4667";//  'uplata kamate
                        }
                    }
                    else
                    {
                        if (Program.idFirme == 15)
                        {
                            id_svrhaplacanjaview = "6597";// 'uplata kamate
                        }
                        else
                        {
                            id_svrhaplacanjaview = "2020"; //  'placanje kamate
                        }

                    }
                }

                if (MI.SvrhaPlacanja.ToUpper().IndexOf("MENIC") > 0 || (MI.ModelPozivaNaBrojZaduzenja.ToString() == "99"))
                {
                    SvrhaPlacanjaSaIzvoda = "973";
                    oblikplacanja = "16";
                    pozivnabrojsaizvoda = MI.PozivNaBrojZaduzenja.ToString().Trim();

                }


                if (Convert.ToInt16(MI.SifraPlacanja) == 48) { id_svrhaplacanjaview = "1748"; }// 'Raspored dobiti
                if (Convert.ToInt16(MI.SifraPlacanja) == 63) { id_svrhaplacanjaview = "1061"; }// ' Prenos sa racuna na racun
                if (Convert.ToInt16(MI.SifraPlacanja) == 86) { id_svrhaplacanjaview = "833"; }// ' Dinarska protivvrednost                  
                                                                                              // If MI.SifraPlacanja = "41" And prometprepisani!OblikPlacanja = 3 Then greska!!!
                if (id_svrhaplacanjaview == "752")
                {
                    isplate = (Convert.ToDouble(MI.iznos.Trim()) / 100).ToString();
                }
                else
                {
                    uplate = (Convert.ToDouble(MI.iznos.Trim()) / 100).ToString();
                }
                int kontrola1 = MI.PozivNaBrojOdobrenja.ToString().IndexOf("/");
                int kontrola0 = MI.PozivNaBrojOdobrenja.ToString().IndexOf("-");
                if ((MI.PozivNaBrojOdobrenja.ToString().Trim() != "") && (MI.PozivNaBrojOdobrenja.ToString().Trim().Length > 9))
                {
                    if (MI.PozivNaBrojOdobrenja.ToString().IndexOf("-") + 1 == 0)  // Then GoTo 11
                    {
                        if (Convert.ToInt16(prometprepisani.Rows[0]["OblikPlacanja"]) != 16)
                        {
                            prometprepisani.Rows[0]["PozivNaBrojSaIzvoda"] = MI.PozivNaBrojOdobrenja.ToString().Trim();
                            prometprepisani.Rows[0]["MestoKomitentaSaIzvoda"] = MI.MestoPrimaoca.ToString().Trim();
                            //prometprepisani.Update;
                            pp = pp + 1;
                        }
                        if (kontrola1 == 0)
                        {
                            if (MI.PozivNaBrojOdobrenja.ToString().Substring(kontrola0, 1) == "-")
                            { MI.PozivNaBrojOdobrenja.ToString().Substring(kontrola0, 1).Replace("-", "/"); }

                        }


                    }

                }
            }
            
                
            DateTime dtt = DateTime.Parse(pddt.ToString("dd.MM.yy"));
            strParams4 = "";
            strParams4 += "@param1=" + "S" + "`";
            strParams4 += "@param2=" + dtt + "`";
            strParams4 += "@param3=" + id_bankaView + "`";
            strParams4 += "@param4=" + ispravniPodaciDaNe + "`"; ;
            strParams4 += "@param5=" + brojizvoda + "`";
            strParams4 += "@param6=" + id_dokumentaView + "`";
            strParams4 += "@param7=" + MI.OznakaKnjizenja.ToString() + "`";
            strParams4 += "@param8=" + oblikplacanja + "`";
            strParams4 += "@param9=" + prenesenozaplacanje + "`"; ;
            strParams4 += "@param10=" + sifraplacanjasaizvoda + "`"; ;
            strParams4 += "@param11=" + id_svrhaplacanjaview + "`"; ;
            strParams4 += "@param12=" + id_komitentiView + "`"; ;
            strParams4 += "@param13=" + SvrhaPlacanjaSaIzvoda + "`"; ;
            strParams4 += "@param14=" + uplate + "`"; ;
            strParams4 += "@param15=" + isplate + "`";
            strParams4 += "@param16=" + pozivnabrojsaizvoda;

            //strParams2 += "@param8=" + Convert.ToInt32(MIZ.RedniBrojIzvoda.ToString()) + "`";

            str = "";
            str = "INSERT INTO[dbo].[PlacanjaNaplate]";
            str += " ([VrstaSloga],[Datum],[ID_BankaView],[IspravniPodaciDaNe],[BrojIzvoda],[ID_DokumentaView],[OznakaKnjizenja],";
            str += " [OblikPlacanja],[PrenesenoZaPlacanje],[SifraPlacanjaSaIzvoda],[ID_SvrhaPlacanjaView],[ID_KomitentiView],";
            str += " [SvrhaPlacanjaSaIzvoda],[Uplate],[Isplate],[PozivNaBrojSaIzvoda])";
            str += " VALUES(@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9,@param10, @param11, @param12,@param13, @param14, @param15, @param16)";
            
            lista.Add(new string[] { str, strParams4 });
                lista.ToArray();


                ///////////////////////////KRAJ MI PETLJE

            }
        
            string rezultat4 = db.ReturnSqlTransactionParams(lista);
            lista.Clear();




            string strParams = "";
            strParams = "@param1=" + idbanka.ToString() + "`";
            strParams += "@param2=" + IdDokView.ToString();

            str = "Update PlacanjaNaplate set ID_KomitentiView=tr.Id_komitentiView,IspravniPodaciDaNe=1 ";
            str += " From PlacanjaNaplate as pn,TekuciRacuniKomitenataStavkeView as tr ";
            str += " where VrstaSloga='S' AND OznakaKnjizenja='20' and ID_BankaView=@param1";
            str += " And pn.BrojTekucegRacunaSaIzvoda=tr.BrojTekucegRacuna  AND pn.ID_KomitentiView=1 AND ID_DokumentaView=@param2";

            lista.Clear();
            lista.Add(new string[] { str, strParams });
            string rs = db.ReturnSqlTransactionParams(lista);
            lista.Clear();
            //////////////////////////////////////////////////////

            strParams = "";
            strParams = "@param1=" + idbanka.ToString() + "`";
            strParams += "@param2=" + IdDokView.ToString();

            str = "";
            str = "Update PlacanjaNaplate set ID_KomitentiView=1 ";
            str += " From PlacanjaNaplate ";
            str += " where VrstaSloga='S' AND ID_BankaView=" + idbanka.ToString();
            str += " And ID_SvrhaPlacanjaView=1061 AND ID_KomitentiView=2 AND ID_DokumentaView=" + IdDokView.ToString();

            lista.Clear();
            lista.Add(new string[] { str, strParams });
            rs = db.ReturnSqlTransactionParams(lista);
            lista.Clear();
            ///////////////////////////////////////////////////////////

            strParams = "";
            strParams = "@param1=" + idbanka.ToString() + "`";
            strParams += "@param2=" + mIDdok;

            str = "";
            str += "Update PlacanjaNaplate set ID_PozivNaBroj=d.Id_DokumentaTotali,ID_SvrhaPlacanjaView=";
            str += " case WHEN d.id_DokumentaStablo=300 or d.id_DokumentaStablo=301 THEN 817 ELSE ID_SvrhaPlacanjaView END ";
            str += " From PlacanjaNaplate as pn,DokumentaTotali as d ";
            str += " where VrstaSloga='S' AND OznakaKnjizenja='20' and ID_BankaView=@param1";
            str += " And ID_DokumentaView=@param2";
            str += " And PozivNaBrojSaIzvoda=d.BrDok ";
            lista.Clear();
            lista.Add(new string[] { str, strParams });
            rs = db.ReturnSqlTransactionParams(lista);
            lista.Clear();
            /////////////////////////////
            MessageBox.Show("Prepisano uplata " + pp);

            return prepisUPlacanjaNaplate;

                }
                




        private IZVHALKOMS VratiMi(int R, string imefajla)
        {
         
            int duzina = R;
            string NazivFajla = imefajla;
            IZVHALKOMS M2 = new IZVHALKOMS();
            string[] lines = File.ReadAllLines(NazivFajla);
            string allLines = String.Join("\r\n", lines);
            
            //FileReader = File.ReadAllText(NazivFajla);
            if (lines.Length == R) { M2 = null; return M2; }
            
            duzina = 0;
            string linija = lines[R].ToString();
            duzina = 0;
            duzina = M2.TekuciRacunPartnera.Length;
            M2.TekuciRacunPartnera = linija.Substring(0,duzina);
            M2.OznakaKnjizenja = linija.Substring(duzina, M2.OznakaKnjizenja.Length);
            duzina += M2.OznakaKnjizenja.Length;
            M2.DatumObrade = lines[R].Substring(duzina, M2.DatumObrade.Length);
            duzina += M2.DatumObrade.Length;
            M2.Storno = lines[R].Substring(duzina, M2.Storno.Length);
            duzina += M2.Storno.Length;
            M2.NazivKomitenta = lines[R].Substring(duzina, M2.NazivKomitenta.Length);
            duzina += M2.NazivKomitenta.Length;
            M2.Prazno = lines[R].Substring(duzina, M2.Prazno.Length);
            duzina += M2.Prazno.Length;
            M2.DatumUplate = lines[R].Substring(duzina, M2.DatumUplate.Length);
            duzina += M2.DatumUplate.Length;
            M2.TekuciRacunKomitenta = lines[R].Substring(duzina, M2.TekuciRacunKomitenta.Length);
            duzina += M2.TekuciRacunKomitenta.Length;
            M2.iznos = lines[R].Substring(duzina, M2.iznos.Length);
            duzina += M2.iznos.Length;
            M2.Prazno1 = lines[R].Substring(duzina, M2.Prazno1.Length);
            duzina += M2.Prazno1.Length;
            M2.OblikPlacanja = lines[R].Substring(duzina, M2.OblikPlacanja.Length);
            duzina += M2.OblikPlacanja.Length;
            M2.SifraPlacanja = lines[R].Substring(duzina, M2.SifraPlacanja.Length);
            duzina += M2.SifraPlacanja.Length;
            M2.Prazno2 = lines[R].Substring(duzina, M2.Prazno2.Length);
            duzina += M2.Prazno2.Length;
            M2.ModelPozivaNaBrojZaduzenja = lines[R].Substring(duzina, M2.ModelPozivaNaBrojZaduzenja.Length);
            duzina += M2.ModelPozivaNaBrojZaduzenja.Length;
            M2.PozivNaBrojZaduzenja = lines[R].Substring(duzina, M2.PozivNaBrojZaduzenja.Length);
            duzina += M2.PozivNaBrojZaduzenja.Length;
            M2.ModelPozivaNaBrojOdobrenja = lines[R].Substring(duzina, M2.ModelPozivaNaBrojOdobrenja.Length);
            duzina += M2.ModelPozivaNaBrojOdobrenja.Length;
            M2.PozivNaBrojOdobrenja = lines[R].Substring(duzina, M2.PozivNaBrojOdobrenja.Length);
            duzina += M2.PozivNaBrojOdobrenja.Length;
            M2.SvrhaPlacanja = lines[R].Substring(duzina, M2.SvrhaPlacanja.Length);
            duzina += M2.SvrhaPlacanja.Length;
            M2.MestoPrimaoca = lines[R].Substring(duzina, M2.MestoPrimaoca.Length);
            duzina += M2.MestoPrimaoca.Length;
            M2.NazivPrimaoca = lines[R].Substring(duzina, M2.NazivPrimaoca.Length);
            duzina += M2.NazivPrimaoca.Length;
            M2.BrojZaReklamaciju = lines[R].Substring(duzina, M2.BrojZaReklamaciju.Length);
            duzina += M2.BrojZaReklamaciju.Length;
            M2.TekuciRacunPrimaoca = lines[R].Substring(duzina, M2.TekuciRacunPrimaoca.Length);
            duzina += M2.TekuciRacunPrimaoca.Length;
            M2.KrajReda = "\r\n";// lines[R].Substring(duzina, M2.KrajReda.Length);
            return M2;


        }
    }
}
    

