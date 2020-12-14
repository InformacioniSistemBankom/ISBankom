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
    public partial class LoginForm : Form
    {
        public LoginForm()
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
 
        private Dictionary<string, string> aliasDatabase;

        public void changeDatabase(string nazivBaze)
        {
            Program.connectionString = "Data Source=bankomw;Initial Catalog=" + aliasDatabase[nazivBaze] + ";User ID=sa;password=password;";
            this.connectionString = Program.connectionString;


        }
      
        private void password_Enter(object sender, EventArgs e)
        {

            PasswordTextBox.Text = "";
        }

        private void username_Enter(object sender, EventArgs e)
        {
            UsernameTextBox.Text = "";
        }
        

        //private void Obrada(string imebaze = "")

        //{
        //    strIzborBaze = "";
        //    strOrgDefaultText = "";
        //    strCurrentbaza = "";
        //    cmbBaze.Text = imebaze;

        //    strCurrentbaza = imebaze;

        //    string usrnam = UsernameTextBox.Text;
        //    var dv = new DataView();
        //    lblBaza.Visible = false;
        //    CmbOrg.Visible = false;
        //    lblBaza.Visible = false;
        //    cmbBaze.Visible = false;
        //    cmbBaze.Items.Clear();
        //    CmbOrg.Items.Clear();

        //    if (usrnam != "")
        //    {
        //        //fileReader = File.ReadAllText(Application.StartupPath + @"\XmlLat\xxxx.ini");

        //        var fileReader = File.ReadAllText(@"\\BANKOMW\Repozitorijum\ISBankom\XXXX\xxxx.ini");
        //        string[] separators11 = new[] { "[", "]" };

        //        int n = 0;

        //        string struser = usrnam;
        //        string strobrada = "";
        //        string[] words = fileReader.Split(separators11, StringSplitOptions.RemoveEmptyEntries);
        //        for (n = 0; n < words.Length; n++)
        //        {
        //            string cc = words[n].ToLower();

        //            if (strobrada != "")
        //            {
        //                strobrada = words[n];
        //                break;

        //            }

        //            if (cc == struser)
        //            {
        //                strobrada = words[n];

        //            }
        //        }

        //        //  char[] separators = { '#','=' };
        //        char[] separators = { '#' };
        //        strobrada = strobrada.Replace("\r\n", "#").Replace("\r", "").Replace("\n", "");


        //        var result = strobrada.Split(separators, StringSplitOptions.None);
        //        //strIzborBaze statusbaze
        //        //strCurrentbaza defaultna baza.

        //        //strIzborOrganizacionogDela = status org deo
        //        // strOrgDefaultText = defaultna org deo
        //        for (n = 0; n < result.Length; n++)
        //        {
        //            if (result[n] != "")
        //            {

        //                if (result[n].Contains("Baza1") == true)
        //                {

        //                    if (strCurrentbaza == "") strCurrentbaza = result[n].Substring(result[n].IndexOf("=") + 1);
        //                }


        //                if (result[n] == "IzborBaze=1")
        //                {
        //                    strIzborBaze = result[n];



        //                }
                        

        //                if (result[n] == "IzborOrganizacionogDela=1") { strIzborOrganizacionogDela = result[n]; }


        //                if (result[n].Length > 4 && result[n].Substring(0, 4) == "Baza")
        //                {

        //                    if (strIzborBaze == "IzborBaze=1" && strCurrentbaza == "")
        //                    {
        //                        strCurrentbaza = result[n].Substring(result[n].IndexOf("=") + 1);
        //                    }


        //                }
        //                if ((result[n].Length > 15 && result[n].Substring(0, 16) == "OrganizacioniDeo"))
        //                {
        //                    strOrgDefaultText = result[n].Substring(result[n].IndexOf("=") + 1);
        //                    //break;
        //                }
        //            }
        //        }
        //        if (strIzborBaze == "IzborBaze=1")  // ako je 1 puni combo
        //        {

        //            var con1 = new SqlConnection(Program.connectionString);
        //            if (con1.State == ConnectionState.Closed) { con1.Open(); }


        //            var cmd = new SqlCommand();
        //            var da = new SqlDataAdapter();

        //            cmd.Connection = con1;
        //            cmd.CommandText = "listaDB";
        //            cmd.Parameters.Add("@imebaze", SqlDbType.VarChar, 100).Value = imebaze;
        //            cmd.CommandType = CommandType.StoredProcedure;



        //            da.SelectCommand = cmd;
        //            da.Fill(dtUlaz);


        //            string[] tobeDistinct = { "DatabaseName" };
        //            DataTable dtDistinct = GetDistinctRecords(dtUlaz, tobeDistinct);

        //            for (int p = 0; p < dtDistinct.Rows.Count; p++)
        //            {
        //                if (dtDistinct.Rows[p][0].ToString() != "")
        //                    if (dtDistinct.Rows[p][0].ToString() == strCurrentbaza)
        //                        indexCurrentbaza = p;
        //                cmbBaze.Items.Add(dtDistinct.Rows[p][0].ToString());
        //                aliasDatabase.Add(dtDistinct.Rows[p][0].ToString(), dtDistinct.Rows[p][0].ToString());

        //            }

        //        }

        //        if (strIzborOrganizacionogDela == "IzborOrganizacionogDela=1") // puni combo za org
        //        {


        //            string[] TobeDistinct = { "Naziv" };
        //            DataTable dtDistinct = GetDistinctRecords(dtUlaz, TobeDistinct);
        //            CmbOrg.Items.Clear();

        //            for (int p = 0; p < dtDistinct.Rows.Count; p++)
        //            {
        //                if (dtDistinct.Rows[p][0].ToString() == strOrgDefaultText)
        //                    indexOrgDefault = p;
        //                if (dtDistinct.Rows[p][0].ToString() != "")
        //                    CmbOrg.Items.Add(dtDistinct.Rows[p][0].ToString());

        //            }

        //        }
        //        DataTable drr = new DataTable();
        //        cmbBaze.Visible = false;
        //        CmbOrg.Visible = false;

        //        switch (strIzborBaze)
        //        {
        //            case "":
        //                cmbBaze.Items.Clear();
        //                break;

        //            case "IzborBaze=1":
        //                lblBaza.Visible = true;
        //                cmbBaze.Visible = true;
        //                cmbBaze.SelectedIndex = indexCurrentbaza;

        //                break;

        //        }
        //        switch (strIzborOrganizacionogDela)
        //        {
        //            case "":
        //                CmbOrg.Items.Clear();
        //                break;

        //            case "IzborOrganizacionogDela=1":
        //                lblGrupa.Visible = true;
        //                CmbOrg.Visible = true;
        //                CmbOrg.SelectedIndex = indexOrgDefault;


        //                DataTable IdOrg = DB.ReturnDataTable(" select o.*,os.NazivJavni as Firma ,os.NazivStampaca, os.PutanjaStampaca,os.Pib from OrganizacionaStruktura as o WITH (NOLOCK) ,organizacionastrukturastablo os WITH (NOLOCK)  where o.Naziv='" + strOrgDefaultText + "' And o.ID_OrganizacionaStrukturaStablo=os.ID_OrganizacionaStrukturaStablo  ;");
        //                dv = IdOrg.DefaultView;
        //                if (dv.Count > 0)
        //                {
        //                    string str = "select v.ID_Sifrarnikvaluta,OznVal,ID_Zemlja from sifrarnikvaluta as v WITH (NOLOCK) ,Zemlja as z WITH (NOLOCK) ";
        //                    str += " where z.ID_Zemlja=" + Convert.ToString(dv[0]["ID_Zemlja"]);
        //                    str += " AND v.SifraZemlje=z.SifraZemlje";
        //                    DataBaseBroker db = new DataBaseBroker();
        //                    DataTable t = new DataTable();
        //                    t = db.ReturnDataTable(str);

        //                    if (t.Rows.Count == 0)
        //                    {
        //                        Program.DomacaValuta = "RSD";
        //                        Program.ID_DomacaValuta = 1;
        //                        Program.ID_MojaZemlja = 4;
        //                    }
        //                    else
        //                    {
        //                        Program.DomacaValuta = Convert.ToString(t.Rows[0]["OznVal"]);
        //                        Program.ID_DomacaValuta = Convert.ToInt32(t.Rows[0]["ID_SifrarnikValuta"]);
        //                        Program.ID_MojaZemlja = Convert.ToInt32(t.Rows[0]["ID_Zemlja"]);
        //                    }

        //                }
        //                CmbOrg.Text = strOrgDefaultText;

        //                break;

        //        }



        //    }
        //    if (cmbBaze.SelectedIndex != -1)
        //    {
        //        Program.NazivBaze = cmbBaze.SelectedValue.ToString(); // izmeniti konekciju i u Program.cs
        //        //   Program.connectionString = "Data Source=DESKTOP-71PLEMH;Initial Catalog=" + cmbBaze.Text + ";User ID=sa;password=password;";
        //        //  Program.GetConnection();
        //    }



        //}
        //public DataTable GetDistinctRecords(DataTable dt, string[] Columns)
        //{
        //    DataTable dtUniqRecords = new DataTable();

        //    dtUniqRecords = dt.DefaultView.ToTable(true, Columns);

        //    return dtUniqRecords;

        //}

        private void UsernameTextBox_Leave(object sender, EventArgs e)
        {
           

            aliasDatabase = new Dictionary<string, string>();

            cmbBaze.Items.Clear();
            CmbOrg.Items.Clear();
            CmbOrg.Visible = false;


            lblGrupa.Visible = false;
            cmbBaze.Visible = false;
            lblBaza.Visible = false;

            if (UsernameTextBox.Text != "")
            {
                //var fileReader = File.ReadAllText(Application.StartupPath + @"\XmlLat\xxxx.ini");

                var fileReader = File.ReadAllText(@"\\BANKOMW\Repozitorijum\ISBankom\XXXX\xxxx.ini");
                string[] separators11 = new[] { "[", "]" };

                int n = 0;

                string struser = UsernameTextBox.Text.ToLower();
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
                            

                            aliasDatabase.Add(result[n].Substring(result[n].IndexOf("-") + 1), result[n].Substring(result[n].IndexOf("=") + 1, result[n].IndexOf("-") - result[n].IndexOf("=") - 1));
                            cmbBaze.Items.Add(result[n].Substring(result[n].IndexOf("-") + 1));
                            

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
                        cmbBaze.SelectedIndex = k > -1 ? indexCurrentbaza : -1;
                        break;

                    case "IzborBaze=1":
                        lblBaza.Visible = true;
                        cmbBaze.Visible = true;
                        cmbBaze.SelectedIndex = indexCurrentbaza;
                        break;

                }

                switch (strIzborOrganizacionogDela)
                {
                    case "IzborOrganizacionogDela=0":
                        CmbOrg.Items.Add(strOrgDefaultText);
                        CmbOrg.SelectedIndex = 0;
                        break;

                    case "IzborOrganizacionogDela=1":

                        var query = "SELECT Naziv FROM OrganizacionaStruktura ";
                        var dataTable = DB.ReturnDataTable(query);
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {

                            if (dataTable.Rows[i][0].ToString() == strOrgDefaultText) indexOrgDefault = i;
                            CmbOrg.Items.Add(dataTable.Rows[i][0].ToString());

                        }
                        lblGrupa.Visible = true;
                        CmbOrg.Visible = true;
                        CmbOrg.SelectedIndex = indexOrgDefault;
                        break;


                }



                //novo 08.12.2020. zajedno
                CmbOrg.Items.Clear();
                var query1 = "SELECT Naziv FROM OrganizacionaStruktura ";
                var databaseBroker = new DataBaseBroker();
                var dataTable1 = databaseBroker.ReturnDataTable(query1);
                for (int i = 0; i < dataTable1.Rows.Count; i++)
                {

                    if (dataTable1.Rows[i][0].ToString() == strOrgDefaultText)
                        indexOrgDefault = i;
                    if (dataTable1.Rows[i][0].ToString() != "")
                        CmbOrg.Items.Add(dataTable1.Rows[i][0].ToString());

                }


                Console.WriteLine(cmbBaze.Text);
                Console.WriteLine(CmbOrg.Text);
                DataSet IdOrg = DB.ReturnDS(
                    " select o.*,os.NazivJavni as Firma ,os.NazivStampaca, os.PutanjaStampaca,os.Pib from OrganizacionaStruktura as o WITH (NOLOCK) ,organizacionastrukturastablo os WITH (NOLOCK)  where o.Naziv='" +
                    CmbOrg.Text + "' And o.ID_OrganizacionaStrukturaStablo=os.ID_OrganizacionaStrukturaStablo  ;");
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

                var upit = "SELECT ID_Radnik FROM KadrovskaEvidencija WHERE Suser = @param0";
                var param0 = UsernameTextBox.Text;
                var prDok = DB.ParamsQueryDT(upit, param0);

             
                if (prDok.Rows.Count != 0 && prDok.Rows[0]["ID_Radnik"] != System.DBNull.Value)
                {
                    
                   int ID_Radnik = Convert.ToInt32(prDok.Rows[0]["ID_Radnik"]);
                    upit = "SELECT ID_Firma,mbr FROM Radnik where ID = @param0";
                    
                    prDok = DB.ParamsQueryDT(upit, ID_Radnik);
                    if (prDok.Rows.Count != 0)
                    {
                        int ID_Firma = Convert.ToInt32(prDok.Rows[0]["ID_Firma"]);
                        int mbr = Convert.ToInt32(prDok.Rows[0]["mbr"]);
                        if (File.Exists(@"\\BANKOMW\organizacija\Pictures\" + ID_Firma + "-" + mbr + ".jpg"))
                            pictureBox1.Image = Image.FromFile(@"\\BANKOMW\organizacija\Pictures\" + ID_Firma + "-" + mbr + ".jpg");

                        pictureBox1.Refresh();

                    }

                }
                




            }
        }
        private void username_TextChanged(object sender, EventArgs e)
        {

            if (Control.IsKeyLocked(Keys.CapsLock))
            {
              

            }

        }

        private int VratiJezik()
        {
            int j=0;

            foreach (RadioButton r in groupBox1.Controls.OfType<RadioButton>())
            {
                if (r.Checked == true)
                {
                    j = int.Parse(r.Tag.ToString());
                }
            }

            return j;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (CmbOrg.SelectedIndex == 0)
            {
                MessageBox.Show("Morate odabrati grupu.");
            }
            else
            {
                //tamara 21.10.2020.

                Program.ID_Jezik = VratiJezik();
                //Djora 26.09.20
                int standardHeight = 1080; // 600;  //900
                int standardWidth = 1920; // 800;  //1440

                int presentHeight = Screen.PrimaryScreen.Bounds.Height;//.Bounds.Height;
                int presentWidth = Screen.PrimaryScreen.Bounds.Width;
                float heightRatio = (float)((float)presentHeight / (float)standardHeight);
                float widthRatio = (float)((float)presentWidth / (float)standardWidth);

                Program.RacioWith = (float)widthRatio;
                Program.RacioHeight = (float)heightRatio;


                string ImeKorisnika = "";
                string PassKorisnika = "";

                string n = UsernameTextBox.Text;
                lblBaza.Text = "";
                lblGrupa.Text = "";

                if (n.Length == 0)
                {
                    MessageBox.Show("Polje korisničko ime je prazno.");

                    return;
                }
                n = PasswordTextBox.Text;
                if (n.Length == 0)
                {
                    MessageBox.Show("Polje za lozinku je prazno.");
                    return;
                }
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    if (cnn.State == ConnectionState.Closed) { cnn.Open(); }

                    string str = " select  suser,Pass,ID_KadrovskaEvidencija,SifRadnika from KadrovskaEvidencija WITH (NOLOCK) where SUSER = @username and id_kadrovskaevidencija <> 1 	";

                    var usernameParam = new SqlParameter("username", SqlDbType.NVarChar) { Value = UsernameTextBox.Text.Trim() };


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
                        MessageBox.Show("Pogrešno korisničko ime.");
                        lblBaza.Visible = false;
                        cmbBaze.Visible = false;
                        lblGrupa.Visible = false;
                        CmbOrg.Visible = false;
                        rdr.Close();
                        cmd.Dispose();
                        cnn.Close();
                        return;
                    }

                    rdr.Close();
                    cmd.Dispose();

                    string strOrgDeo = "select o.ID_OrganizacionaStruktura,o.ID_OrganizacionaStrukturaStablo,os.Naziv  ";
                    strOrgDeo += " from OrganizacionaStruktura as o WITH(NOLOCK) ,organizacionastrukturastablo os WITH(NOLOCK) ";
                    strOrgDeo += "  where o.Naziv = '" + CmbOrg.Text + "' and o.ID_OrganizacionaStrukturaStablo=os.ID_OrganizacionaStrukturaStablo   ;";

                    DataSet ds = new DataSet();

                    ds = DB.ReturnDS(strOrgDeo);
                    DataView dv = ds.Tables[0].DefaultView;

                    Program.imeFirme = dv[0]["Naziv"].ToString();
                    Program.idOrgDeo = Convert.ToInt32(dv[0]["ID_OrganizacionaStruktura"]);
                    Program.idFirme = Convert.ToInt32(dv[0]["ID_OrganizacionaStrukturaStablo"]);




                    cnn.Close();

                }

                if (UsernameTextBox.Text != ImeKorisnika)
                {
                    MessageBox.Show("Pogrešno korisničko ime.");
                    lblBaza.Visible = false;
                    cmbBaze.Visible = false;
                    lblGrupa.Visible = false;
                    CmbOrg.Visible = false;
                    UsernameTextBox.Text = "";
                    return;
                }




                bool result = PasswordTextBox.Text.Equals(PassKorisnika);
                if (result == false)
                {
                    MessageBox.Show("Pogrešna lozinka.");
                    PasswordTextBox.Text = "";
                    return;
                }

                Program.imekorisnika = ImeKorisnika;
                Program.IntLogovanje = 1;
                Hide();

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
        }


        private void CmbBaze_SelectedIndexChanged(object sender, EventArgs e)
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
                if (aliasDatabase[cmbBaze.SelectedItem.ToString()] == Convert.ToString(dv[x][0]))
                {
                    strimebaze = Convert.ToString(dv[x][0]);
                    Program.NazivBaze = strimebaze;
                    break;
                }
            }
           
            if (strimebaze == "")
            {
                MessageBox.Show("Ne postoji izabrana baza", "info");

                CmbOrg.Items.Clear();

            }
            else
            {

                changeDatabase(cmbBaze.SelectedItem.ToString());

                CmbOrg.Items.Clear();
                var query = "SELECT Naziv FROM OrganizacionaStruktura ";
                var databaseBroker = new DataBaseBroker();
                var dataTable = databaseBroker.ReturnDataTable(query);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {

                    if (dataTable.Rows[i][0].ToString() == strOrgDefaultText)
                        indexOrgDefault = i;
                    if (dataTable.Rows[i][0].ToString() != "")
                        CmbOrg.Items.Add(dataTable.Rows[i][0].ToString());

                }

                if (cmbBaze.SelectedItem.ToString() == strCurrentbaza) CmbOrg.SelectedIndex = indexOrgDefault;
                else CmbOrg.SelectedIndex = 0;

            }




        }

        //Djora 07.07.20
        private void frmLogin_Load(object sender, EventArgs e)
        {

            int standardHeight = 1080;  //900
            int standardWidth = 1920;  //1440
            int presentHeight = Screen.PrimaryScreen.Bounds.Height;//.Bounds.Height;
            int presentWidth = Screen.PrimaryScreen.Bounds.Width;
            float heightRatio = (float)((float)presentHeight / (float)standardHeight);
            float widthRatio = (float)((float)presentWidth / (float)standardWidth);

            Program.RacioWith = (float)widthRatio;
            Program.RacioHeight = (float)heightRatio;

        }


        
        private void BtnPrekid_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
           
           toolTip1.SetToolTip(this.pictureBox3, "СРБ");
        }

        private void pictureBox5_MouseHover(object sender, EventArgs e)
        {
            
           toolTip1.SetToolTip(this.pictureBox5, "SRB");
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            
            toolTip1.SetToolTip(this.pictureBox4, "ENG");
        }

        private void pictureBox6_MouseHover(object sender, EventArgs e)
        {
            
            toolTip1.SetToolTip(this.pictureBox6, "RUS");
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            if(UsernameTextBox.Text!=null && PasswordTextBox.Text != null)
            {
                var param0 = UsernameTextBox.Text.Trim();
                var param1 = PasswordTextBox.Text.Trim();

                string upit = "select count (*) from KadrovskaEvidencija where SUSER = @param0 and Pass=@param1";
                var rez = DB.ParamsQueryDT(upit, param0, param1);

                if (Convert.ToInt32(rez.Rows[0][0]) == 1)
                {
                    tbNovaLozinka.Visible = true;
                    lbNovaLozinka.Visible = true;
                    button1.Visible = true;
                }
                else
                {
                    MessageBox.Show("Morate uneti ispravno korisničko ime i staru lozinku!");
                }
            }
            else MessageBox.Show("Morate uneti ispravno korisničko ime i staru lozinku!");


        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblBaza.Visible = false;
            cmbBaze.Visible = false;
            lblGrupa.Visible = false;
            CmbOrg.Visible = false;
            tbNovaLozinka.Visible = true;
            lbNovaLozinka.Visible = true;

            var param0 = UsernameTextBox.Text.Trim();

            string upit = "select Pass, suser from KadrovskaEvidencija where SUSER = @param0 and id_kadrovskaevidencija <> 1 	";
            DataTable rez = DB.ParamsQueryDT(upit, param0);

            if (rez.Rows[0][0].ToString() == PasswordTextBox.Text.Trim())
            {


                var param1 = param0;
                param0 = tbNovaLozinka.Text.Trim();

                string upit1 = "update KadrovskaEvidencija set Pass=@param0 where SUSER = @param1 and id_kadrovskaevidencija <> 1 	";
                DataTable rez1 = DB.ParamsQueryDT(upit1, param0, param1);


                MessageBox.Show("Lozinka uspešno promenjena!");
                PasswordTextBox.Text = "";

            tbNovaLozinka.Visible = false;
            lbNovaLozinka.Visible = false;
            lblBaza.Visible = true;
            cmbBaze.Visible = true;
            lblGrupa.Visible = true;
            CmbOrg.Visible = true;

            }
            else
            {
                MessageBox.Show("Pogrešno korisničko ime ili lozinka,izmena nije moguća!");
            }

            

        }
    }
}
