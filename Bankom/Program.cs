using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bankom
{

   public  static class Program
    {
        //Djora 07.07.20
        public static float RacioWith { get; set; }
        public static float RacioHeight { get; set; }

        public static string imekorisnika { get; set; }
        public static string strDatum { get; set; }
        public static string BrDok { get; set; }
        public static string PunoImeDokumenta { get; set; } // punoime dokumenta npr prodaja / ime
        public static int IntLogovanje { get; set; } = -1;   
        public static string NazivBaze { get; set; } = "";
        public static int intAkcija { get; set; } =-1;
        public static DateTime kDatum;   
        public static SqlConnection con;
        public static string[] startPamti { get; set; }
        public static string sPamti { get; set; }
        public static String SifRadnika { get; set; }
        public static int idkadar { get; set; }
        public static string DomacaValuta { get; set; }
        public static int ID_DomacaValuta { get; set; }
        public static int ID_MojaZemlja { get; set; }
        public static int ID_Jezik { get; set; }
        public static int idOrgDeo { get; set; }
        public static int idFirme { get; set; }
        public static string imeFirme{ get; set; }        
        public static BankomMDI Parent = new BankomMDI();
        // parametri za stampu
        public static string param { get; set; }
        // uslov za osvezavanje dokumenta
        public static string WWhere { get; set; }

        public static string vred { get; set; }
        public static string smer { get; set; }
        public static string imegrida { get; set; }
        public static string colname { get; set; }
        public static DataGridView activecontrol { get; set; }

        //public static string connectionString = "Data Source=BANKOM10; Initial Catalog = dbbbTestNew2003Bankom; User Id=sa;password=password; ";
        //public static string connectionString = "Data Source=Sql2016;Initial Catalog=dbbbTestNew2003Bankom;User ID=sa;password=password;";

        public static string connectionString = "Data Source=bankomw;Initial Catalog=BankomVeza;User ID=sa;password=password;";
        public static string NazivRacunara = System.Environment.MachineName;
        public static string UserDomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

        //Djora 10.09.20
        public static string AktivnaForma { get; set; }
        public static string AktivnaSifraIzvestaja { get; set; }
        public static int IdSelektovanogCvora { get; set; }
        public static int IdParentaSelektovanogCvora { get; set; }
        public static string KlasifikacijaSlovo { get; set; }
        public static string pomStablo { get; set; }
        public static string pomIzv{ get; set; }


        [STAThread]

         static void Main()
        {

            Process process = Process.GetCurrentProcess();
            var dupl = (Process.GetProcessesByName(process.ProcessName));
            if (dupl.Length > 1 && MessageBox.Show("Program je vec podignut", "Zatvoriti duplikat?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (var p in dupl)
                {
                    if (p.Id != process.Id)
                        p.Kill();
                }
            }

            Application.EnableVisualStyles();

            Application.Run(new LoginForm());
          
        

            if (IntLogovanje == 1)
            {
                //    GetConnection();

                intAkcija = 0;               
                Parent = new BankomMDI();
                Application.Run(Parent);
            }
        }
        public static SqlConnection GetConnection()
        {
            try
            {
                string str = connectionString;
                
                con = new SqlConnection(str);
                con.Open();
                return con;
            }
            catch
            {
                return null;
            }
        }

        //-- code to make sure to close connection and dispose the object
        public static void Dispose(SqlConnection con)
        {
                con.Dispose();
        }
    }    
}
