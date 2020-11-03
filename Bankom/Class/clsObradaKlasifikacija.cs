using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Windows.Forms;

namespace Bankom.Class
{
    class clsObradaKlasifikacija
    {
       
        public string param5 = "";
        public string param1;

        DataBaseBroker db = new DataBaseBroker();
        private string GetIdCvor(string strNazivJavni, string d, string pomIzv, string pomStablo)
        {
            
            string param0 = "";
            if (pomIzv == "Izvestaji")
            {
                param0 = strNazivJavni.Trim().Substring(4);
                param5 = d.Substring(0, 3);
                param1 = d.Substring(4);
            }
            else
            {
                param0 = strNazivJavni;
                param1 = d;
            }
            string upit = "Select id_" + pomStablo + " from " + pomStablo + " where  NazivJavni = @param0";
            DataTable rez = db.ParamsQueryDT(upit, param0);
            string s = rez.Rows[0][0].ToString();
            return s;
            
        }

        public void Klasifikacija_Click(string d, string pomIzv, string pomStablo)
        {
            string s = Program.AktivnaSifraIzvestaja;

            string nazivCvora = d;

            if (!String.IsNullOrEmpty(nazivCvora.ToString()) || !String.IsNullOrWhiteSpace(nazivCvora.ToString()))
            {
                int id = int.Parse(GetIdCvor(s, d,pomIzv,pomStablo));

                string upit1 = " SELECT MAX(RedniBroj) FROM " + pomStablo;
                DataTable rez = db.ParamsQueryDT(upit1);
                int i = int.Parse(rez.Rows[0][0].ToString()) + 1;
                var param0 = param1;
                var param2 = id;
                var param3 = i;
                int param4 = 0;

                string upit = "insert into " + pomStablo + " (Naziv,NazivJavni,Vezan,RedniBroj,CCopy,Brdok) values(@param0, @param1, @param2, @param3,@param4,@param5)";

                db.ParamsInsertScalar(upit, param0, param1, param2, param3, param4, param5);

                clsRefreshForm frm = new clsRefreshForm();
                frm.refreshform();
            }
            else
            {
                MessageBox.Show("Morate uneti naziv novog čvora u tekstualno polje!");

            }





        }
    }
}
