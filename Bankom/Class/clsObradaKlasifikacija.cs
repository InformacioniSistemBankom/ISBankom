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
                MessageBox.Show("Uspešno dodato.");
            }
            else
            {
                MessageBox.Show("Morate uneti naziv novog čvora u tekstualno polje!");

            }


        }

        public void KlasifikacijaBrisanje(string pomIzv,string pomStablo)
        {
            string sa = Program.AktivnaSifraIzvestaja;
            
            if (!String.IsNullOrEmpty(sa.ToString()) || !String.IsNullOrWhiteSpace(sa.ToString()))
            {
                string param0;
                if (pomIzv == "Izvestaji")
                {
                    param0 = sa.Substring(4);
                }
                else param0 = sa;

                string upit1 = "Select id_" + pomStablo + " from " + pomStablo + " where  NazivJavni = @param0";
                DataTable rez1 = db.ParamsQueryDT(upit1,param0);
                param0 =rez1.Rows[0][0].ToString();

                string upit = "Select Count (vezan) from " + pomStablo + " where  vezan = @param0";
                DataTable rez = db.ParamsQueryDT(upit, param0);
                int s = int.Parse(rez.Rows[0][0].ToString());

                if (s > 0)
                {
                    MessageBox.Show("Ne možete obrisati ovaj čvor! Prvo obrišite sve čvorove koji se nalaze u njemu.");
                }
                else
                {
                     param0 = sa.Substring(4);
                    string upit3 = "Delete from " + pomStablo + " where  NazivJavni = @param0";
                    db.ParamsInsertScalar(upit3, param0);
                }


                MessageBox.Show("Uspešno obrisano.");


            }
            else
            {
                MessageBox.Show("Morate izabrati čvor koji želite da obrišete!");

            }
        }

        public void KlasifikacijaIzmena(string d, string pomIzv, string pomStablo)
        {
            string sa = Program.AktivnaSifraIzvestaja;

            if (!String.IsNullOrEmpty(sa.ToString()) || !String.IsNullOrWhiteSpace(sa.ToString()))
            {
                string param0;
              
                if (pomIzv == "Izvestaji")
                {
                    param0 = sa.Substring(4);
                    d = d.Substring(4);
                }
                else param0 = sa;

                string upit = "Select id_" + pomStablo + " from " + pomStablo + " where  NazivJavni = @param0";
                DataTable rez1 = db.ParamsQueryDT(upit, param0);
                string param1 = rez1.Rows[0][0].ToString();

                param0 = d;
                string upit1 = "Update " + pomStablo + " set NazivJavni = @param0 where id_" + pomStablo + "=@param1 ";
                db.ParamsInsertScalar(upit1, param0,param1);

                MessageBox.Show("Uspešno izmenjeno.");

            }
            else
            {
                MessageBox.Show("Morate izabrati čvor koji želite da izmenite!");

            }
        }

        public static string nazivCvora;
        public void KlasifikacijaPremestiGrupu( string pomIzv, string pomStablo)
        {
            string sa = Program.AktivnaSifraIzvestaja;
            if (!String.IsNullOrEmpty(sa.ToString()) || !String.IsNullOrWhiteSpace(sa.ToString()))
            {
                string param0;

                if (pomIzv == "Izvestaji")
                {
                    param0 = sa.Substring(4);
                
                }
                else param0 = sa;
                nazivCvora = param0;
            
                string upit1 = "Update " + pomStablo + " set Ccopy = 1 where NazivJavni = @param0 ";
                db.ParamsInsertScalar(upit1, param0);

                

            }
            else
            {
                MessageBox.Show("Morate izabrati čvor koji želite da premestite!");

            }
        }
        public void KlasifikacijaNovaPozicija(string pomIzv, string pomStablo, string pomMenu)
        {
            string sa = Program.AktivnaSifraIzvestaja;
            
            if (!String.IsNullOrEmpty(sa.ToString()) || !String.IsNullOrWhiteSpace(sa.ToString()))
            {
                string param0;

                if (pomIzv == "Izvestaji")
                {
                    param0 = sa.Substring(4);
                  
                }
                else param0 = sa;
                 string connstring = Program.connectionString;
                SqlConnection con = new SqlConnection(connstring);
                con.Open();
                string upit;
                if (sa == pomMenu)
                {
                    pomStablo = "MenuStablo";
                    upit = "Select id_" + pomStablo + " from " + pomStablo + " where  NazivJavni ='" + param0 + "'";
                }
                else
                {
                    upit = "Select id_" + pomStablo + " from " + pomStablo + " where  NazivJavni ='"+param0+"'";
                }
                SqlCommand cmd = new SqlCommand(upit,con);
                var rez1 = cmd.ExecuteScalar();


         //var rez1 = db.ParamsInsertScalar(upit, param0);
                string param1 = rez1.ToString();

               
                param0 = param1;
                param1 = nazivCvora;

                string upit1 = "Update " + pomStablo + " set vezan = @param0, CCopy = 0 where NazivJavni=@param1";
                db.ParamsInsertScalar(upit1, param0, param1);

                MessageBox.Show("Uspešno izmenjeno.");

            }
            else
            {
                MessageBox.Show("Morate izabrati mesto čvora!");

            }

        }

    }
}
