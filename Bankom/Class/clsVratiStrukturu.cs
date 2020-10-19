using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankom.Class
{
    public class clsVratiStrukturu
    {
        public static string connstring = Program.connectionString;



        public struct vrGetPageDokOS
           {
                private DataSet ds;
                private string[] strskup;
            public vrGetPageDokOS(DataSet ds, string[] strskup) : this()
            {


                SqlConnection conn = new SqlConnection(connstring);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                if (conn.State == ConnectionState.Closed) { conn.Open(); }
                string strArt = "";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                switch (Convert.ToInt32(strskup[0]))
                {
                    case 1:
                        if (strskup[3] != "")
                        {
                            strArt = "SELECT Grupa,IId AS Sifra,ID_ArtikliTotali as 'Int.sifra' ,ExterniBarkod as 'BarKod', NazivArt as 'Naziv artikla' , TrgovackiNaziv as 'Trgovacki naz.', NazivArtEngleski as 'Engleski naziv', JedinicaMere as 'JM',[ID_ArtikliTotali],TarifaPoreza as  [Tarifa],PoreskaStopa as [Stopa],SifTar as [Naimenovanje] , CCopy as [Vidljiv] ";
                            strArt += "FROM ArtikliTotali";
                            strArt += " where Grupa <> '' and (Grupa like '%" + strskup[3].Trim() + "%' or NazivArt like '%" + strskup[3].Trim() + "%') order by  ID_ArtikliTotali desc OFFSET " + strskup[1].Trim() + " ROWS FETCH NEXT " + strskup[2].Trim() + "  ROWS ONLY";
                        }
                        else
                        {
                            strArt = "SELECT Grupa,IId AS Sifra,ID_ArtikliTotali as 'Int.sifra' ,ExterniBarkod as 'BarKod', NazivArt as 'Naziv artikla' , TrgovackiNaziv as 'Trgovacki naz.', NazivArtEngleski as 'Engleski naziv', JedinicaMere as 'JM',[ID_ArtikliTotali],TarifaPoreza as  [Tarifa],PoreskaStopa as [Stopa],SifTar as [Naimenovanje] , CCopy as [Vidljiv] FROM ArtikliTotali where Grupa <> '' order by  ID_ArtikliTotali desc  OFFSET " + strskup[1].Trim() + " ROWS FETCH NEXT " + strskup[2].Trim() + "  ROWS ONLY";
                        }
                        cmd.CommandText = strArt;
                        cmd.CommandType = CommandType.Text;
                        break;
                        
                    default:

                
                cmd.CommandText = "[dbo].[GetPageOS]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@idsrv", SqlDbType.Int).Value = Convert.ToInt32(strskup[0]);// intCurentDok;
                cmd.Parameters.Add("@pocetak", SqlDbType.Int).Value = Convert.ToInt32(strskup[1]);
                cmd.Parameters.Add("@sledeci", SqlDbType.Int).Value = Convert.ToInt32(strskup[2]);
                cmd.Parameters.Add("@sfind", SqlDbType.VarChar).Value = strskup[3];
                        break;
               }

                da.SelectCommand = cmd;
                da.Fill(ds);
                
                this.ds = ds;
                this.strskup = strskup;

            }
          }

        public struct VrGetPageDok
        {
          
            
            private DataSet ds;            
            private string[] ss;

            public VrGetPageDok(DataSet ds, string[] ss) : this()
            {
                int intMaxPageN = -1;
                CultureInfo provider = CultureInfo.InvariantCulture;
                SqlConnection conn = new SqlConnection(connstring);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                if (conn.State == ConnectionState.Closed) { conn.Open(); }


             

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.CommandText = "[dbo].[GetPageDok]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@idsrv", SqlDbType.Int).Value = Convert.ToInt32(ss[0]);// intCurentDok;
                cmd.Parameters.Add("@pocetak", SqlDbType.Int).Value = Convert.ToInt32(ss[1]);
                cmd.Parameters.Add("@sledeci", SqlDbType.Int).Value = Convert.ToInt32(ss[2]);
                cmd.Parameters.Add("@sfind", SqlDbType.VarChar).Value = ss[3];
                cmd.Parameters.Add("@maxID", SqlDbType.Int).Direction = ParameterDirection.Output;

            
                da.SelectCommand = cmd;
                da.Fill(ds);
                this.ds = ds;
                intMaxPageN = Convert.ToInt32(cmd.Parameters["@maxID"].Value);

                ss[4] = Convert.ToString(intMaxPageN);
             
                this.ds = ds;                
                this.ss = ss;



            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return base.ToString();
            }
        }

    }
}
