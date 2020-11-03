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
        BankomMDI instanca = new BankomMDI();
        public string param5 = "";
        public string param1;

        DataBaseBroker db = new DataBaseBroker();
        private string GetIdCvor(string strNazivJavni)
        {
            
            string param0 = "";
            if (instanca.pomIzv == "Izvestaji")
            {
                param0 = strNazivJavni.Trim().Substring(4);
                param5 = instanca.toolStripTextBox1.Text.Substring(0, 3);
                param1 = instanca.toolStripTextBox1.Text.Substring(4);
            }
            else
            {
                param0 = strNazivJavni;
                param1 = instanca.toolStripTextBox1.Text;
            }
            string upit = "Select id_" + instanca.pomStablo + " from " + instanca.pomStablo + " where  NazivJavni =@param0 ";
            DataTable rez = db.ParamsQueryDT(upit, param0);
            string s = rez.Rows[0][0].ToString();
            return s;

        }

        public void Klasifikacija_Click(object sender, EventArgs e)
        {
            string s = Program.AktivnaSifraIzvestaja;

            string nazivCvora = instanca.toolStripTextBox1.Text;

            if (!String.IsNullOrEmpty(nazivCvora.ToString()) && !String.IsNullOrWhiteSpace(nazivCvora.ToString()))
            {
                int id = int.Parse(GetIdCvor(s));

                string upit1 = " SELECT MAX(RedniBroj) FROM " + instanca.pomStablo;
                DataTable rez = db.ParamsQueryDT(upit1);
                int i = int.Parse(rez.Rows[0][0].ToString()) + 1;
                var param0 = param1;
                var param2 = id;
                var param3 = i;
                int param4 = 0;

                string upit = "insert into " + instanca.pomStablo + " (Naziv,NazivJavni,Vezan,RedniBroj,CCopy,Brdok) values(@param0, @param1, @param2, @param3,@param4,@param5)";

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
