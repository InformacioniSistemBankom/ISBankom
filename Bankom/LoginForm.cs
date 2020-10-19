using System;
using Bankom.Class;
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
using System.Xml.Linq;


namespace Bankom
{
    public partial class LoginForm : Form
    {

        public string connectionString = Program.connectionString;
        DataBaseBroker DB = new DataBaseBroker();
        DataTable dtUlaz = new DataTable();
        string strIzborBaze = ""; // status izbora baze
        string strIzborOrganizacionogDela = ""; // status izbora Organizacionog Dela dela
        string strOrgDefaultText = "";// default OrganizacionogDela dela
        string strCurrentbaza = ""; //default baza 
        public LoginForm()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            string ImeKorisnika = "";
            string PassKorisnika = "";
            //int ID_MojaZemlja=4;
            string n = UsernameTextBox.Text;
            lblBaza.Text = "";
            lblGrupa.Text = "";

            if (n.Length == 0)
            {
                label1.Text = "polje username je prazno";
                label1.Visible = true;
                return;
            }
            n = PasswordTextBox.Text;
            if (n.Length == 0)
            {
                label1.Text = "polje za lozinku je prazno";
                label1.Visible = true;
                return;
            }
            using (SqlConnection cnn = new SqlConnection(connectionString))
            {
                if (cnn.State == ConnectionState.Closed) { cnn.Open(); }

                string str = " select  suser,Pass,ID_KadrovskaEvidencija,SifRadnika from KadrovskaEvidencija WITH (NOLOCK) where SUSER = '" + UsernameTextBox.Text.Trim() + "'  and id_kadrovskaevidencija <> 1 	";

                var cmd = new SqlCommand
                {
                    CommandText = str,
                    Connection = cnn
                };



                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    ImeKorisnika = Convert.ToString(rdr[0]);
                    PassKorisnika = Convert.ToString(rdr[1]);
                    Program.idkadar = Convert.ToInt32(rdr[2]);
                    Program.SifRadnika= Convert.ToString(rdr[3]);
                }
                else
                {
                    label1.Text = "Pogresno korisničko ime !";
                    label1.Visible = true;
                    rdr.Close();
                    cmd.Dispose();
                    cnn.Close();
                    return;
                }

                rdr.Close();
                cmd.Dispose();

                string strOrgDeo = "select o.ID_OrganizacionaStruktura,o.ID_OrganizacionaStrukturaStablo,os.Naziv,ID_Zemlja  ";
                strOrgDeo += " from OrganizacionaStruktura as o WITH(NOLOCK) ,organizacionastrukturastablo os WITH(NOLOCK) ";
                strOrgDeo += "  where o.Naziv = '" + CmbOrg.Text + "' and o.ID_OrganizacionaStrukturaStablo=os.ID_OrganizacionaStrukturaStablo   ;";
                ///////////////
                DataSet ds = new DataSet();

                ds = DB.ReturnDS(strOrgDeo);
                DataView dv = ds.Tables[0].DefaultView;

                Program.idOrgDeo = Convert.ToInt32(dv[0]["ID_OrganizacionaStruktura"]);
                Program.idFirme= Convert.ToInt32(dv[0]["ID_OrganizacionaStrukturaStablo"]);
                Program.imeFirme = dv[0]["Naziv"].ToString();


                Program.idOrgDeo = Convert.ToInt32(dv[0]["ID_OrganizacionaStruktura"]);
                Program.idFirme = Convert.ToInt32(dv[0]["ID_OrganizacionaStrukturaStablo"]);
                Program.imeFirme = dv[0]["Naziv"].ToString();


                cnn.Close();

            }

            if (UsernameTextBox.Text != ImeKorisnika)
            {
                label1.Text = "Pogrešno korisničko ime !";
                label1.Visible = true;
                UsernameTextBox.Text = "";
                return;
            }


   

            bool result = PasswordTextBox.Text.Equals(PassKorisnika);
            if (result == false)
            {
                label1.Text = "Pogrešan password !";
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
            //Program.IntLogovanje = -1; // BORKA 09.06.20
        }

        public  void Obrada(string imebaze="" )
        {
         
            strIzborBaze = "";
            strOrgDefaultText = "";
            strCurrentbaza = "";
            cmbBaze.Text = imebaze;
            strCurrentbaza= imebaze; 


            DataView dv = new DataView();
                
               
                
                
                lblGrupa.Visible = false;
                CmbOrg.Visible = false;
                lblBaza.Visible = false;
                cmbBaze.Visible = false;
                CmbOrg.Items.Clear();
                cmbBaze.Items.Clear();

           
           
            if (UsernameTextBox.Text != "")
                {


                    string fileReader;
                    fileReader = File.ReadAllText(Application.StartupPath + @"\XmlLat\xxxx.ini");
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

                              if (strCurrentbaza == "")    strCurrentbaza = result[n].Substring(result[n].IndexOf("=") + 1);
                        }


                        if (result[n] == "IzborBaze=1")
                        {
                            strIzborBaze = result[n];

                          

                        }





                            if (result[n] == "IzborOrganizacionogDela=1") { strIzborOrganizacionogDela = result[n]; }


                            if (result[n].Length > 4 && result[n].Substring(0, 4) == "Baza")
                            {

                            if (strIzborBaze ==  "IzborBaze=1" && strCurrentbaza == "")
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
                        SqlConnection con1;
                        con1 = new SqlConnection(Program.connectionString);
                        if (con1.State == ConnectionState.Closed) { con1.Open(); }


                        SqlCommand cmd = new SqlCommand();
                        SqlDataAdapter da = new SqlDataAdapter();

                        cmd.Connection = con1;
                        cmd.CommandText = "listaDB";
                        cmd.Parameters.Add("@imebaze", SqlDbType.VarChar, 100).Value =  cmbBaze.Text;
                        cmd.CommandType = CommandType.StoredProcedure;



                        da.SelectCommand = cmd;
                        da.Fill(dtUlaz);


                        string[] TobeDistinct = { "DatabaseName" };
                        DataTable dtDistinct = GetDistinctRecords(dtUlaz, TobeDistinct);

                        for (int p = 0; p < dtDistinct.Rows.Count; p++)
                        {
                            if (dtDistinct.Rows[p][0].ToString() != "")
                                cmbBaze.Items.Add(dtDistinct.Rows[p][0].ToString());
                        }
                        //cmbBaze.SelectedIndex = 0;
                        //   
                    }

                    if (strIzborOrganizacionogDela == "IzborOrganizacionogDela=1") // puni combo za org
                    {
                  

                        string[] TobeDistinct = { "NazivJavni" };
                        DataTable dtDistinct = GetDistinctRecords(dtUlaz, TobeDistinct);
                        CmbOrg.Items.Clear();
                        for (int p = 0; p < dtDistinct.Rows.Count; p++)
                        {
                            if (dtDistinct.Rows[p][0].ToString() != "")
                                CmbOrg.Items.Add(dtDistinct.Rows[p][0].ToString());
                        }
                        //CmbOrg.SelectedIndex = 0;
                        //   
                    }

                      DataTable drr = new DataTable();
                      cmbBaze.Visible = false;
                      CmbOrg.Visible = false;

                    switch (strIzborBaze)
                    {
                        case "":
                            cmbBaze.Items.Clear();
                            break;

                        case "IzborBaze=1":
                            lblBaza.Visible = true;
                            cmbBaze.Visible = true;
                        cmbBaze.Text = strCurrentbaza;
                        //cmbBaze.SelectedIndex = 0;
                        break;

                    }

                    switch (strIzborOrganizacionogDela)
                    {
                        case "":
                            CmbOrg.Items.Clear();
                            break;

                        case "IzborOrganizacionogDela=1":
                            lblGrupa.Visible = true;
                            CmbOrg.Visible = true;
                            CmbOrg.SelectedIndex = 0;


                        DataTable IdOrg = DB.ReturnDataTable(" select o.*,os.Naziv as Firma ,os.NazivStampaca, os.PutanjaStampaca,os.Pib from OrganizacionaStruktura as o WITH (NOLOCK) ,organizacionastrukturastablo os WITH (NOLOCK)  where o.Naziv='" + strOrgDefaultText + "' And o.ID_OrganizacionaStrukturaStablo=os.ID_OrganizacionaStrukturaStablo  ;");
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
                        CmbOrg.Text = strOrgDefaultText;
                        break;

                    }

        


                }
                
              
            if (cmbBaze.Text != "")
            {
                Program.NazivBaze = cmbBaze.Text; // izmeniti konekciju i u Program.cs
             //   Program.connectionString = "Data Source=DESKTOP-71PLEMH;Initial Catalog=" + cmbBaze.Text + ";User ID=sa;password=password;";
              //  Program.GetConnection();
            }

    }
        private void UsernameTextBox_TextChanged(object sender, EventArgs e)
            {

            

                if (Control.IsKeyLocked(Keys.CapsLock))
                { label1.Text = "Uključen vam je CapsLock"; }

            }
            private void UsernameTextBox_LostFocus(object sender, EventArgs e)
            {
            
            Obrada("");

        }
            private void LoginForm_Load(object sender, EventArgs e)
            {

            }
            private void ComboBox2_TextChanged(object sender, EventArgs e)

            {
       
             }
            private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
            {
            if ( strCurrentbaza  != "" && cmbBaze.Text !="")
            {
                if (strCurrentbaza != cmbBaze.Text)
                {
                    strCurrentbaza = cmbBaze.Text;
                    Obrada(cmbBaze.Text);
                }
            }

        }

        private void PasswordLabel_Click(object sender, EventArgs e)
            {

            }

            private void CmbBaze_SelectedIndexChanged(object sender, EventArgs e)
            {

            }

            private void BtnPrekid_Click(object sender, EventArgs e)
            {
                Application.Exit();
            }
        public  DataTable GetDistinctRecords(DataTable dt, string[] Columns)
        {
            DataTable dtUniqRecords = new DataTable();
            
                    dtUniqRecords = dt.DefaultView.ToTable(true, Columns);
            
            return dtUniqRecords;

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (UsernameTextBox.Text.Trim() == "" || PasswordTextBox.Text.Trim() == ""  )
            {
                MessageBox.Show("Polja za korisničko ime i password moraju da budu popunjena");
                return;
            }

            string strpass = DB.ReturnString("SELECT Suser  from KadrovskaEvidencija   WHERE Suser = '" + UsernameTextBox.Text + "'", 0);
            string suser = UsernameTextBox.Text.ToLower().Trim();
            strpass = strpass.ToLower().Trim();
            if (strpass != suser)
            {
                MessageBox.Show("Pogrešno korisničko ime, izmena nije moguća");
                return;
            }



            strpass = DB.ReturnString("SELECT pass  from KadrovskaEvidencija   WHERE Suser = '" + UsernameTextBox.Text + "'", 0);
            if  (strpass != PasswordTextBox.Text.Trim())
            {
                MessageBox.Show("Pogrešan password izemna nije moguća");
                return;
            }

            DataTable dt = new DataTable();
            dt = DB.ReturnDataTable("UPDATE KadrovskaEvidencija SET pass ='" + strpass + "' WHERE Suser = '" + txtPotvrda.Text.Trim() + "'");
                if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Vaš novi password je: " + txtPotvrda.Text.Trim());
            }



        }

        private void lblBaza_Click(object sender, EventArgs e)
        {

        }

        private void lblGrupa_Click(object sender, EventArgs e)
        {

        }

        private void UsernameLabel_Click(object sender, EventArgs e)
        {

        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }

}  