using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankom
{
    class clsVratiKratkoIme
    {
        public static string value = "";
        public string Vrati(string ime,string izvestaj)
        {

            string value = string.Empty;
            using (SqlConnection cnn = new SqlConnection(Program.connectionString))
                value = NewMethod(ime, izvestaj, value, cnn);

            //value = klase.GetValue(key).ToString();

            return value;
        }

        private static string NewMethod(string ime, string izvestaj, string value, SqlConnection cnn)
        {

            string strstablo = "";
            {
                if (cnn.State == ConnectionState.Closed) cnn.Open();

                switch (izvestaj)
                {
                    case "":
                        strstablo = "select naziv FROM DokumentaStablo  where NazivJavni = '" + ime.ToLower() + "'";
                        break;
                    case "Komitenti":
                        strstablo = "select naziv FROM KomitentiStablo  where NazivJavni = '" + ime.ToLower() + "'";
                        break;
                    case "PomocniSifarnici":
                        strstablo = "select naziv FROM PomocniSifarniciStablo  where NazivJavni = '" + ime.ToLower() + "'";
                        break;

                }
                //strstablo = "select naziv FROM DokumentaStablo  where NazivJavni = '" + ime.ToLower() + "'";


                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = strstablo;
                cmd.Connection = cnn;

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    value = Convert.ToString(rdr[0]);
                }


                rdr.Close();
                cmd.Dispose();
                //value = strstablo;
                //if (izvestaj != "")
                //{
                //    string kontrolaImenaDokumenta = "SELECT Naziv,UlazniIzlazni   FROM [SifarnikDokumenta]   WHERE NAZIV = '" + strstablo + "'";

                //    SqlCommand cmd1 = new SqlCommand();
                //    cmd1.CommandType = CommandType.Text;
                //    cmd1.Connection = cnn;
                //    cmd1.CommandText = kontrolaImenaDokumenta;
                //    SqlDataReader rdrbroj = cmd1.ExecuteReader();
                //    if (rdrbroj.Read())
                //    {
                //        if (value != Convert.ToString(rdrbroj[1])) { value = Convert.ToString(rdrbroj[1]); }
                //    }

                //    rdrbroj.Close();
                //    cmd1.Dispose();




                //    cnn.Close();
                //}


            }

            return value;
        }
    }
}
