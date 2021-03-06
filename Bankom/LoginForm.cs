﻿using System;
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
        public static string ImeServera;
        public static string FileServer;
        public static string gDokumenti="";
        public static string ReportServer; //adresa report servera steva 04.03.2021.
        public static string ReportSlike; //adresa na kojoj se nalaze slike za header i footer izvestaja steva 04.03.2021.
        private int indexCurrentbaza = -1;
        private int indexOrgDefault = -1;

        private Dictionary<string, string> aliasDatabase;

        public void changeDatabase(string nazivBaze)
        {
            Program.connectionString = "Data Source=" + ImeServera + ";Initial Catalog=" + aliasDatabase[nazivBaze] + ";User ID=sa;password=password;";
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

        private void UsernameTextBox_Leave(object sender, EventArgs e)
        {

            //pictureBox1.Visible = false;
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
                Console.WriteLine(Application.StartupPath);
                //Djora 30.11.20
                //var fileReader = File.ReadAllText(Application.StartupPath+ @"\xxxx.ini");
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
                    if (cc == "logovanje")
                    {

                        string pom = words[n + 1];
                        char[] separators1 = { '#' };
                        pom = pom.Replace("\r\n", "#").Replace("\r", "").Replace("\n", "");


                        var result1 = pom.Split(separators1, StringSplitOptions.None);

                        for (int j = 0; j < result1.Length; j++)
                        {
                            //steva 04.03.2021.
                            if (result1[j].Contains("RptSlike="))
                            {
                                ReportSlike = result1[j].Substring(result1[j].IndexOf("=") + 1);
                                // break;
                            }
                            if (result1[j].Contains("Report="))
                            {
                                ReportServer = result1[j].Substring(result1[j].IndexOf("=") + 1);
                                // break;
                            }
                            //kraj steva 04.03.2021.
                            //Jovana 19.02.21
                            Console.WriteLine(result1[j]);
                            if (result1[j].Contains("Server="))
                            {
                                if ( result1[j].IndexOf("Server=") == 0)
                                   ImeServera = result1[j].Substring(result1[j].IndexOf("=") + 1);
                                else
                                    FileServer = result1[j].Substring(result1[j].IndexOf("=") + 1);
                                //break;
                            }
                            if ((result1[j].Length > 8 && result1[j].Substring(0,9) == "Dokumenti"))
                            {
                                gDokumenti =  result1[j].Substring(result1[j].IndexOf("=") +3);
                            }
                        }
                    }
                }
                //Jovana 19.02.21
                gDokumenti = "\\" + FileServer + gDokumenti;

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
                    " select o.*,o.ID_Zemlja,os.NazivJavni as Firma ,os.NazivStampaca, os.PutanjaStampaca,os.Pib from OrganizacionaStruktura as o WITH (NOLOCK) ,organizacionastrukturastablo os WITH (NOLOCK)  where o.Naziv='" +
                    CmbOrg.Text + "' And o.ID_OrganizacionaStrukturaStablo=os.ID_OrganizacionaStrukturaStablo  ;");
                DataView dv = IdOrg.Tables[0].DefaultView;

                var zemlja = dv[0]["ID_Zemlja"];


                string str ="select v.ID_Sifrarnikvaluta,OznVal,ID_Zemlja from sifrarnikvaluta as v WITH (NOLOCK) ,Zemlja as z WITH (NOLOCK)  where z.ID_Zemlja=" + Convert.ToString(dv[0]["ID_Zemlja"]);
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
                        if (File.Exists(@"\\" + LoginForm.ImeServera + @"\organizacija\Pictures\" + ID_Firma + "-" + mbr + ".jpg"))
                        {
                            pictureBox1.Image = Image.FromFile(@"\\" + LoginForm.ImeServera + @"\organizacija\Pictures\" + ID_Firma + "-" + mbr + ".jpg");
                            pictureBox1.Visible = true;
                        }


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
            int j = 0;

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
            if (CmbOrg.Text.Trim() == "")
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
                    Program.NazivOrg = CmbOrg.Text;



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

                ssel = " SELECT min(Godina) as god FROM Godine where flag=1 AND ID_KadrovskaEvidencija=@param0";

                tk = dk.ParamsQueryDT(ssel, Program.idkadar);
                if (tk.Rows.Count == 0)
                    Program.mGodina = 0;
                else
                    Program.mGodina = Convert.ToInt32(tk.Rows[0]["god"].ToString());
            }
        }

        public List<string> lista = new List<string>();
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
            Console.WriteLine(aliasDatabase[cmbBaze.SelectedItem.ToString()]);
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
                Console.WriteLine(Convert.ToString(dv[x][0]));
                if (aliasDatabase[cmbBaze.SelectedItem.ToString()].Trim() == Convert.ToString(dv[x][0])) 
                {
                    strimebaze = Convert.ToString(dv[x][0]);
                    Program.NazivBaze = strimebaze;
                    break;
                }
            }

            if (strimebaze == "")
            {
                MessageBox.Show("Ne postoji izabrana baza", "info");
                lista.Clear();
                CmbOrg.Items.Clear();

            }
            else
            {

                changeDatabase(cmbBaze.SelectedItem.ToString());
                lista.Clear();
                CmbOrg.Items.Clear();
                CmbOrg.Text = "";
                var query = "SELECT Naziv FROM OrganizacionaStruktura ";
                var databaseBroker = new DataBaseBroker();
                var dataTable = databaseBroker.ReturnDataTable(query);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {

                    if (dataTable.Rows[i][0].ToString() == strOrgDefaultText)
                        indexOrgDefault = i;
                    if (dataTable.Rows[i][0].ToString() != "")
                    {
                        lista.Add(dataTable.Rows[i][0].ToString());
                    }

                }
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
            if (UsernameTextBox.Text != null && PasswordTextBox.Text != null)
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
        private void CmbOrg_DropDown(object sender, EventArgs e)
        {
            CmbOrg.Items.Clear();
            for (int i = 0; i < lista.Count; i++)
                if (lista[i].ToLower().Contains(CmbOrg.Text.ToLower()))
                    CmbOrg.Items.Add(lista[i]);
        }

        private void UsernameTextBox_TextChanged(object sender, EventArgs e)
        {
            //tamara123
            PasswordTextBox.Text = "";
        }
    }
}

