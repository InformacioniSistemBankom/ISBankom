using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bankom.Class;

namespace Bankom
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        public string connectionString = Program.connectionString;
        DataBaseBroker DB = new DataBaseBroker();
        DataTable dtUlaz = new DataTable();
        string strIzborBaze = ""; // status izbora baze
        string strIzborOrganizacionogDela = ""; // status izbora Organizacionog Dela dela
        string strOrgDefaultText = "";// default OrganizacionogDela dela
        string strCurrentbaza = ""; //default baza 
        private int indexCurrentbaza = -1;
        private int indexOrgDefault = -1;
        private bool changePassword = false;
        private Dictionary<string, string> aliasDatabase;

        public  void changeDatabase(string nazivBaze)
        {
            Program.connectionString = "Data Source=bankomw;Initial Catalog=" + aliasDatabase[nazivBaze] + ";User ID=sa;password=password;";
            this.connectionString = Program.connectionString;
            

        }
        private void password_OnValueChanged(object sender, EventArgs e)
        {
            password.isPassword = true;
            
        }

        private void newpassword_OnValueChanged(object sender, EventArgs e)
        {
            newpassword.isPassword = true;
        }

        



        private void password_Enter(object sender, EventArgs e)
        {
            password.isPassword = true;
          
            password.Text = "";
        }

        private void username_Enter(object sender, EventArgs e)
        {
            
            username.Text = "";
        }
        private void Prekid_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Obrada(string imebaze = "")

        {
            strIzborBaze = "";
            strOrgDefaultText = "";
            strCurrentbaza = "";
            baza.Text = imebaze;
            
            strCurrentbaza = imebaze;

            string usrnam = username.Text;
            var dv = new DataView();
            grupalabel.Visible = false;
            grupa.Visible = false;
            bazalabel.Visible = false;
            baza.Visible = false;
            baza.Clear();
            grupa.Clear();

            if (usrnam != "")
            {
                //fileReader = File.ReadAllText(Application.StartupPath + @"\XmlLat\xxxx.ini");

                var fileReader = File.ReadAllText(@"\\BANKOMW\Repozitorijum\ISBankom\XXXX\xxxx.ini");
                string[] separators11 = new[] { "[", "]" };

                int n = 0;

                string struser = usrnam;
                string strobrada = "";
                string[] words = fileReader.Split(separators11, StringSplitOptions.RemoveEmptyEntries);
                for (n = 0; n < words.Length; n++)
                {
                    string cc = words[n].ToLower();

                    if (strobrada != "")
                    {
                        strobrada = words[n];
                        break;

                    }

                    if (cc == struser)
                    {
                        strobrada = words[n];

                    }
                }

                //  char[] separators = { '#','=' };
                char[] separators = { '#' };
                strobrada = strobrada.Replace("\r\n", "#").Replace("\r", "").Replace("\n", "");


                var result = strobrada.Split(separators, StringSplitOptions.None);
                //strIzborBaze statusbaze
                //strCurrentbaza defaultna baza.

                //strIzborOrganizacionogDela = status org deo
                // strOrgDefaultText = defaultna org deo
                for (n = 0; n < result.Length; n++)
                {
                    if (result[n] != "")
                    {

                        if (result[n].Contains("Baza1") == true)
                        {

                            if (strCurrentbaza == "") strCurrentbaza = result[n].Substring(result[n].IndexOf("=") + 1);
                        }


                        if (result[n] == "IzborBaze=1")
                        {
                            strIzborBaze = result[n];



                        }





                        if (result[n] == "IzborOrganizacionogDela=1") { strIzborOrganizacionogDela = result[n]; }


                        if (result[n].Length > 4 && result[n].Substring(0, 4) == "Baza")
                        {

                            if (strIzborBaze == "IzborBaze=1" && strCurrentbaza == "")
                            {
                                strCurrentbaza = result[n].Substring(result[n].IndexOf("=") + 1);
                            }


                        }
                        if ((result[n].Length > 15 && result[n].Substring(0, 16) == "OrganizacioniDeo"))
                        {
                            strOrgDefaultText = result[n].Substring(result[n].IndexOf("=") + 1);
                            //break;
                        }
                    }
                }
                if (strIzborBaze == "IzborBaze=1")  // ako je 1 puni combo
                {
                    
                   var  con1 = new SqlConnection(Program.connectionString);
                    if (con1.State == ConnectionState.Closed) { con1.Open(); }


                    var cmd = new SqlCommand();
                    var da = new SqlDataAdapter();
                    
                    cmd.Connection = con1;
                    cmd.CommandText = "listaDB";
                    cmd.Parameters.Add("@imebaze", SqlDbType.VarChar, 100).Value = imebaze;
                    cmd.CommandType = CommandType.StoredProcedure;



                    da.SelectCommand = cmd;
                    da.Fill(dtUlaz);


                    string[] tobeDistinct = { "DatabaseName" };
                    DataTable dtDistinct = GetDistinctRecords(dtUlaz, tobeDistinct);

                    for (int p = 0; p < dtDistinct.Rows.Count; p++)
                    {
                        if (dtDistinct.Rows[p][0].ToString() != "")
                            if (dtDistinct.Rows[p][0].ToString() == strCurrentbaza)
                                indexCurrentbaza = p;
                        baza.AddItem(dtDistinct.Rows[p][0].ToString());
                        aliasDatabase.Add(dtDistinct.Rows[p][0].ToString(), dtDistinct.Rows[p][0].ToString());
                            
                    }
                   
                }

                if (strIzborOrganizacionogDela == "IzborOrganizacionogDela=1") // puni combo za org
                {


                    string[] TobeDistinct = { "Naziv" };
                    DataTable dtDistinct = GetDistinctRecords(dtUlaz, TobeDistinct);
                    grupa.Clear();
                    
                    for (int p = 0; p < dtDistinct.Rows.Count; p++)
                    {
                        if (dtDistinct.Rows[p][0].ToString() == strOrgDefaultText)
                            indexOrgDefault = p;
                        if (dtDistinct.Rows[p][0].ToString() != "")
                            grupa.AddItem(dtDistinct.Rows[p][0].ToString());
                            
                    }
                     
                }
                DataTable drr = new DataTable();
                baza.Visible = false;
                grupa.Visible = false;

                switch (strIzborBaze)
                {
                    case "":
                        baza.Clear();
                        break;

                    case "IzborBaze=1":
                        bazalabel.Visible = true;
                        baza.Visible = true;
                        baza.selectedIndex = indexCurrentbaza;

                        break;

                }
                switch (strIzborOrganizacionogDela)
                {
                    case "":
                        grupa.Clear();
                        break;

                    case "IzborOrganizacionogDela=1":
                        grupalabel.Visible = true;
                        grupa.Visible = true;
                        grupa.selectedIndex = indexOrgDefault;


                        DataTable IdOrg = DB.ReturnDataTable(" select o.*,os.NazivJavni as Firma ,os.NazivStampaca, os.PutanjaStampaca,os.Pib from OrganizacionaStruktura as o WITH (NOLOCK) ,organizacionastrukturastablo os WITH (NOLOCK)  where o.Naziv='" + strOrgDefaultText + "' And o.ID_OrganizacionaStrukturaStablo=os.ID_OrganizacionaStrukturaStablo  ;");
                        dv = IdOrg.DefaultView;
                        if (dv.Count > 0)
                        {
                            string str = "select v.ID_Sifrarnikvaluta,OznVal,ID_Zemlja from sifrarnikvaluta as v WITH (NOLOCK) ,Zemlja as z WITH (NOLOCK) ";
                            str += " where z.ID_Zemlja=" + Convert.ToString(dv[0]["ID_Zemlja"]);
                            str += " AND v.SifraZemlje=z.SifraZemlje";
                            DataBaseBroker db = new DataBaseBroker();
                            DataTable t = new DataTable();
                            t = db.ReturnDataTable(str);

                            if (t.Rows.Count == 0)
                            {
                                Program.DomacaValuta = "RSD";
                                Program.ID_DomacaValuta = 1;
                                Program.ID_MojaZemlja = 4;
                            }
                            else
                            {
                                Program.DomacaValuta = Convert.ToString(t.Rows[0]["OznVal"]);
                                Program.ID_DomacaValuta = Convert.ToInt32(t.Rows[0]["ID_SifrarnikValuta"]);
                                Program.ID_MojaZemlja = Convert.ToInt32(t.Rows[0]["ID_Zemlja"]);
                            }

                        }
                        grupa.Text = strOrgDefaultText;
                        
                        break;

                }



            }
            if (baza.selectedIndex != -1)
            {
                Program.NazivBaze = baza.selectedValue; // izmeniti konekciju i u Program.cs
                //   Program.connectionString = "Data Source=DESKTOP-71PLEMH;Initial Catalog=" + cmbBaze.Text + ";User ID=sa;password=password;";
                //  Program.GetConnection();
            }



        }
        public DataTable GetDistinctRecords(DataTable dt, string[] Columns)
        {
            DataTable dtUniqRecords = new DataTable();

            dtUniqRecords = dt.DefaultView.ToTable(true, Columns);

            return dtUniqRecords;

        }

        private void username_Leave(object sender, EventArgs e)
        {
           // Obrada("");
           
            aliasDatabase = new Dictionary<string, string>();
           
            baza.Clear();
            grupa.Clear();
            grupa.Visible = false;
            grupalabel.Visible = false;
            baza.Visible = false;
            bazalabel.Visible = false;

            if (username.Text != "")
            {
                //var fileReader = File.ReadAllText(Application.StartupPath + @"\XmlLat\xxxx.ini");

                var fileReader = File.ReadAllText(@"\\BANKOMW\Repozitorijum\ISBankom\XXXX\xxxx.ini");
                string[] separators11 = new[] {"[", "]"};

                int n = 0;

                string struser = username.Text.ToLower();
                string strobrada = "";
                string[] words = fileReader.Split(separators11, StringSplitOptions.RemoveEmptyEntries);
                for (n = 0; n < words.Length; n++)
                {
                    string cc = words[n].ToLower();

                    if (strobrada != "")
                    {
                        strobrada = words[n];
                        break;

                    }

                    if (cc == struser)
                    {
                        strobrada = words[n];

                    }
                }

                //  char[] separators = { '#','=' };
                char[] separators = {'#'};
                strobrada = strobrada.Replace("\r\n", "#").Replace("\r", "").Replace("\n", "");


                var result = strobrada.Split(separators, StringSplitOptions.None);
                int k = -1;
                for (n = 0; n < result.Length; n++)
                {
                    if (result[n] != "")
                    {

                        if (result[n] == "IzborBaze=1")
                        {
                            strIzborBaze = result[n];
                        }

                        if (result[n] == "IzborOrganizacionogDela=1")
                        {
                            strIzborOrganizacionogDela = result[n];
                        }

                       
                        if (result[n].Length > 4 && result[n].Substring(0, 4) == "Baza")
                        {
                            k++;
                            if (result[n].Substring(0, 5) == "Baza1")
                            {
                                indexCurrentbaza = k;
                                strCurrentbaza = result[n].Substring(result[n].IndexOf("=") + 1);
                            }

                           
                            aliasDatabase.Add(result[n].Substring(result[n].IndexOf("-") + 1), result[n].Substring(result[n].IndexOf("=")+1,result[n].IndexOf("-") - result[n].IndexOf("=")-1));
                            baza.AddItem(result[n].Substring(result[n].IndexOf("-") + 1));
                          
                            
                            
                        }

                        if ((result[n].Length > 15 && result[n].Substring(0, 16) == "OrganizacioniDeo"))
                        {
                            strOrgDefaultText = result[n].Substring(result[n].IndexOf("=") + 1);
                            //grupa.AddItem(result[n].Substring(result[n].IndexOf("=") + 1));
                        }
                    }
                }
                
              
                switch (strIzborBaze)
                {
                    case "IzborBaze=0":
                        baza.selectedIndex = k > -1 ?indexCurrentbaza:-1;
                        break;

                    case "IzborBaze=1":
                        bazalabel.Visible = true;
                        baza.Visible = true;
                        baza.selectedIndex = indexCurrentbaza;
                        break;

                }

                switch (strIzborOrganizacionogDela)
                {
                    case "IzborOrganizacionogDela=0":
                        grupa.AddItem(strOrgDefaultText);
                        grupa.selectedIndex = 0;
                        break;

                    case "IzborOrganizacionogDela=1":

                            var query = "SELECT Naziv FROM OrganizacionaStruktura ";
                            var dataTable = DB.ReturnDataTable(query);
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {

                                if (dataTable.Rows[i][0].ToString() == strOrgDefaultText) indexOrgDefault = i;
                                grupa.AddItem(dataTable.Rows[i][0].ToString());

                            }
                        grupalabel.Visible = true;
                        grupa.Visible = true;
                        grupa.selectedIndex = indexOrgDefault;
                        break;
                    

                }
                Console.WriteLine(baza.Text);
                Console.WriteLine(grupa.Text);
                DataSet IdOrg = DB.ReturnDS(
                    " select o.*,os.NazivJavni as Firma ,os.NazivStampaca, os.PutanjaStampaca,os.Pib from OrganizacionaStruktura as o WITH (NOLOCK) ,organizacionastrukturastablo os WITH (NOLOCK)  where o.Naziv='" +
                    grupa.Text + "' And o.ID_OrganizacionaStrukturaStablo=os.ID_OrganizacionaStrukturaStablo  ;");
                DataView dv = IdOrg.Tables[0].DefaultView;


                string str =
                    "select v.ID_Sifrarnikvaluta,OznVal,ID_Zemlja from sifrarnikvaluta as v WITH (NOLOCK) ,Zemlja as z WITH (NOLOCK) ";
                str += " where z.ID_Zemlja=" + Convert.ToString(dv[0]["ID_Zemlja"]);
                str += " AND v.SifraZemlje=z.SifraZemlje";
                DataTable RsValuta = DB.ReturnDataTable(str);
                dv.Dispose();
                dv = RsValuta.DefaultView;

                if (RsValuta.Rows.Count == 0)
                {
                    Program.DomacaValuta = "RSD";
                    Program.ID_DomacaValuta = 1;
                    Program.ID_MojaZemlja = 4;
                }
                else
                {
                    Program.DomacaValuta = Convert.ToString(dv[0]["OznVal"]);
                    Program.ID_DomacaValuta = Convert.ToInt32(dv[0]["ID_SifrarnikValuta"]);
                    Program.ID_MojaZemlja = Convert.ToInt32(dv[0]["ID_Zemlja"]);
                }

                var upit =
              "SELECT ID_Radnik FROM KadrovskaEvidencija WHERE Suser = @param0";
              
                var prDok = DB.ParamsQueryDT(upit, username.Text);
                int ID_Radnik;
                if (prDok.Rows.Count != 0 && prDok.Rows[0]["ID_Radnik"] != System.DBNull.Value)
                {
                    ID_Radnik = Convert.ToInt32(prDok.Rows[0]["ID_Radnik"]);
                    upit = "SELECT ID_Firma,mbr FROM Radnik where ID = @param0";
                    prDok = DB.ParamsQueryDT(upit, ID_Radnik);
                    if (prDok.Rows.Count != 0)
                    {
                        int ID_Firma = Convert.ToInt32(prDok.Rows[0]["ID_Firma"]);
                        int mbr = Convert.ToInt32(prDok.Rows[0]["mbr"]);
                        if(File.Exists(@"\\BANKOMW\organizacija\Pictures\" + ID_Firma + "-" + mbr + ".jpg")) 
                        pictureBox2.Image = Image.FromFile(@"\\BANKOMW\organizacija\Pictures\" + ID_Firma + "-" + mbr + ".jpg");
                       
                        pictureBox2.Refresh();

                    }

                }
                else
                {
                    pictureBox2.Image = global::Bankom.Properties.Resources.login;
                    pictureBox2.Refresh();
                }
               


               
            }
        }
        private void username_TextChanged(object sender, EventArgs e)
        {

            if (Control.IsKeyLocked(Keys.CapsLock))
            { porukalabel.Text = "Uključen vam je CapsLock";
              
            }

        }

        private void OK_Click(object sender, EventArgs e)
            {
            int standardHeight = 600;  //900
            int standardWidth = 800;  //1440
            int presentHeight = Screen.PrimaryScreen.Bounds.Height;//.Bounds.Height;
            int presentWidth = Screen.PrimaryScreen.Bounds.Width;
            float heightRatio = (float)((float)presentHeight / (float)standardHeight);
            float widthRatio = (float)((float)presentWidth / (float)standardWidth);

            Program.RacioWith = (float)widthRatio;
            Program.RacioHeight = (float)heightRatio;
            if (changePassword)
            {
                if (password.Text.Trim().Equals(newpassword.Text.Trim()))
                {
                    DB.ReturnDataTable("UPDATE KadrovskaEvidencija SET pass ='" + newpassword.Text.Trim() +
                                       "' WHERE Suser = '" + username.Text.Trim() + "'");

                    MessageBox.Show("Uspešno ste promenili lozinku!");
                    ReloadLogin();
                    changePassword = false;
                }
                else
                {
                    MessageBox.Show("Ne podudaraju se lozinke,pokušajte ponovo!");
                }


                return;
            }

            string ImeKorisnika = "";
            string PassKorisnika = "";

            string n = username.Text;
            bazalabel.Text = "";
            grupalabel.Text = "";

            if (n.Length == 0)
            {
                porukalabel.Text = "Polje korisničko ime je prazno";
                porukalabel.Visible = true;
                return;
            }
            n = password.Text;
            if (n.Length == 0)
            {
                porukalabel.Text = "Polje za lozinku je prazno";
                porukalabel.Visible = true;
                return;
            }
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                if (cnn.State == ConnectionState.Closed) { cnn.Open(); }

                string str = " select  suser,Pass,ID_KadrovskaEvidencija,SifRadnika from KadrovskaEvidencija WITH (NOLOCK) where SUSER = @username and id_kadrovskaevidencija <> 1 	";

                var usernameParam = new SqlParameter("username", SqlDbType.NVarChar) {Value = username.Text.Trim()};


                var cmd = new SqlCommand
                {
                    CommandText = str,
                    Connection = cnn
                };
                cmd.Parameters.Add(usernameParam);


                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    ImeKorisnika = Convert.ToString(rdr[0]);
                    PassKorisnika = Convert.ToString(rdr[1]);
                    Program.idkadar = Convert.ToInt32(rdr[2]);
                    Program.SifRadnika = Convert.ToString(rdr[3]);
                }
                else
                {
                    porukalabel.Text = "Pogresno korisničko ime !";
                    porukalabel.Visible = true;
                    rdr.Close();
                    cmd.Dispose();
                    cnn.Close();
                    return;
                }

                rdr.Close();
                cmd.Dispose();

                string strOrgDeo = "select o.ID_OrganizacionaStruktura,o.ID_OrganizacionaStrukturaStablo,os.Naziv  ";
                strOrgDeo += " from OrganizacionaStruktura as o WITH(NOLOCK) ,organizacionastrukturastablo os WITH(NOLOCK) ";
                strOrgDeo += "  where o.Naziv = '" + grupa.selectedValue + "' and o.ID_OrganizacionaStrukturaStablo=os.ID_OrganizacionaStrukturaStablo   ;";
                 
                DataSet ds = new DataSet();

                ds = DB.ReturnDS(strOrgDeo);
                DataView dv = ds.Tables[0].DefaultView;

                Program.idOrgDeo = Convert.ToInt32(dv[0]["ID_OrganizacionaStruktura"]);
                Program.idFirme = Convert.ToInt32(dv[0]["ID_OrganizacionaStrukturaStablo"]);
                Program.imeFirme = dv[0]["Naziv"].ToString();
                   


                cnn.Close();

            }

            if (username.Text != ImeKorisnika)
            {
                porukalabel.Text = "Pogrešno korisničko ime !";
                porukalabel.Visible = true;
                username.Text = "";
                return;
            }




            bool result = password.Text.Equals(PassKorisnika);
            if (result == false)
            {
                porukalabel.Text = "Pogrešna lozinka!";
                return;
            }

            Program.imekorisnika = ImeKorisnika;
            Program.IntLogovanje = 1;
            Hide();
            try
            {
                ((BankomMDI)MdiParent).Controls["menuStrip"].Enabled = true;
                ((BankomMDI)MdiParent).Controls["menuStrip1"].Enabled = true;
                ((BankomMDI)MdiParent).Text = Program.imeFirme + "-" + Program.imekorisnika;
            }
            catch { }
            Program.Parent.Text = Program.imeFirme + "-" + Program.imekorisnika;
            Close();

            int godina = DateTime.Now.Year;
            string ssel = " Select DatumPocetkaObrade  from ZakljucenjeKnjiga WITH(NOLOCK) "
                          + " where GodinaZakljucenja=" + (godina - 1).ToString() + " and id_firma =1 ";
            DataBaseBroker dk = new DataBaseBroker();
            DataTable tk = new DataTable();
            tk = dk.ReturnDataTable(ssel);
            if (tk.Rows.Count > 0)
            {
                Program.kDatum = Convert.ToDateTime(tk.Rows[0]["DatumPocetkaObrade"]);
            }
            else
            {
                Program.kDatum = Convert.ToDateTime("01.01." + (godina - 1).ToString());
            }


           
        }

        private void ReloadLogin()
        {

            pictureBox3.Visible = true;
            username.Visible = true;
            username.Text = "Korisničko ime";
            newpasswordlabel.Visible = false;
            newpassword.Visible = false;
            pictureBox5.Visible = false;
        }

        private void bazaonItemSelected(object sender, EventArgs e)
        {
              
             
            string strimebaze = "";
    
            SqlConnection cnn = new SqlConnection(connectionString);
    
    
    
    
            cnn = Program.GetConnection();
            if (cnn == null)
            {
                MessageBox.Show("Greska u konekciji!");
                Application.Exit();
                System.Environment.Exit(1);
            }
    
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT name FROM master.dbo.sysdatabases";
            cmd.Connection = cnn;
            DataSet dss = new DataSet();
            DataView dv = new DataView();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = cmd;
            sda.Fill(dss);
            dv = dss.Tables[0].DefaultView;
            for (int x = 0; x < dv.Count; x++)
            {
                if (aliasDatabase[baza.selectedValue] == Convert.ToString(dv[x][0]))
                {
                    strimebaze = Convert.ToString(dv[x][0]); Program.NazivBaze = strimebaze; break;
                }
            }

            if (strimebaze == "")
            {
                MessageBox.Show("Ne postoji izabrana baza", "info");

                grupa.Clear();

            }
            else 
            {
                
                changeDatabase(baza.selectedValue);
                grupa.Clear();
                var query = "SELECT Naziv FROM OrganizacionaStruktura ";
                var databaseBroker = new DataBaseBroker();
                var dataTable = databaseBroker.ReturnDataTable(query);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {

                    if (dataTable.Rows[i][0].ToString() == strOrgDefaultText)
                        indexOrgDefault = i;
                    if (dataTable.Rows[i][0].ToString() != "")
                        grupa.AddItem(dataTable.Rows[i][0].ToString());

                }

                if (baza.selectedValue == strCurrentbaza) grupa.selectedIndex = indexOrgDefault;

            }




        }

        //Djora 07.07.20
        private void frmLogin_Load(object sender, EventArgs e)
        {
            
            int standardHeight = 600;  //900
            int standardWidth = 800;  //1440
            int presentHeight = Screen.PrimaryScreen.Bounds.Height;//.Bounds.Height;
            int presentWidth = Screen.PrimaryScreen.Bounds.Width;
            float heightRatio = (float)((float)presentHeight / (float)standardHeight);
            float widthRatio = (float)((float)presentWidth / (float)standardWidth);
                       
            Program.RacioWith = (float)widthRatio;
            Program.RacioHeight = (float)heightRatio;

            pictureBox5.Visible = false;
            newpassword.Visible = false;
            newpasswordlabel.Visible = false;

        }



        private void username_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void grupa_onItemSelected(object sender, EventArgs e)
        {

        }
        // 21.09.2020 Korisnici mogu da promijene lozinku 
        private void Novalozinka_Click(object sender, EventArgs e)
        {

            if (username.Text.Trim() == "" || password.Text.Trim() == "")
            {
                MessageBox.Show("Polja za korisničko ime i lozinku moraju da budu popunjena");
                return;
            }

            using (var cnn = new SqlConnection(connectionString))
            {
                if (cnn.State == ConnectionState.Closed) { cnn.Open(); }

                const string str = " select  suser,Pass from KadrovskaEvidencija WITH (NOLOCK) where SUSER = @username and id_kadrovskaevidencija <> 1 	";

                var usernameParam = new SqlParameter("username", SqlDbType.NVarChar) { Value = username.Text.Trim() };


                var cmd = new SqlCommand
                {
                    CommandText = str,
                    Connection = cnn
                };
                cmd.Parameters.Add(usernameParam);


                var rdr = cmd.ExecuteReader();
                if (rdr.Read() && password.Text.Trim().Equals(Convert.ToString(rdr[1])))
                {
                }
                else
                {
                    MessageBox.Show("Pogrešno korisničko ime ili lozinka,izmena nije moguća!");
                    return;

                }

                newpasswordlabel.Visible = true;
                pictureBox5.Visible = true;
                newpassword.Visible = true;
                newpassword.isPassword = true;
                newpassword.Text = "";
                password.Text = "";
                bazalabel.Visible = false;
                baza.Visible = false;
                grupalabel.Visible = false;
                grupa.Visible = false;
                username.Visible = false;
                pictureBox3.Visible = false;
                changePassword = true;

            }
               


           

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
    }
}
