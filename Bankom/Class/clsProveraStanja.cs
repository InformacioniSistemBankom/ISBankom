using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using Bankom.Class;

namespace Bankom.Class
{
    class clsProveraStanja
    {
        public string ProveriStanje(String Dokument, String IdDokumenta, ref SqlConnection con, ref SqlTransaction transaction)
        {
            //DataBaseBroker db = new DataBaseBroker();
            //    Dictionary<string, string> R = new Dictionary<string, string>();
            //    string sql = "";
            //    String TrebaProvera = "";
            //    DataTable t = new DataTable();
            //    sql="SELECT OOrderBy  as trebaprovera from RecnikPodataka where Dokument='"+Dokument+"' And OOrderBy >0";
            //    Console.WriteLine(sql);
            //    t = db.ReturnDataTable(sql);
            //    if (t.Rows.Count == 0) return ("");
            //    TrebaProvera = t.Rows[0]["trebaprovera"].ToString();

            //    if (Dokument.Contains("Lot") == true && TrebaProvera != "3")
            //    {
            //        R=db.ExucuteStoreProcedureDva("ProveraStanja",ref con, ref transaction,"IdDokView:" + IdDokumenta, "RezultatProvereStanja:" );
            //        if (R["@RezultatProvereStanja"].Trim() == "" )
            //        {
            //            R = db.ExucuteStoreProcedureDva("LotProveraStanja",ref con, ref transaction, "IdDokView:" + IdDokumenta, "RezultatProvereStanja:");
            //        }
            //    }
            //    else if ((Dokument.Contains("Lot") == true && TrebaProvera == "3") || TrebaProvera == "3")  // provera samo magacinskog stanja
            //             {
            //             R = db.ExucuteStoreProcedureDva("LotProveraStanja",ref con, ref transaction, "IdDokView:" + IdDokumenta, "RezultatProvereStanja:");
            //             }
            //             else
            //             {
            //                if (TrebaProvera == "4") // provera lotovskih dokumenata koji nemaju prefiks LOT
            //                {
            //                   R = db.ExucuteStoreProcedureDva("ProveraStanja",ref con, ref transaction, "IdDokView:" + IdDokumenta, "RezultatProvereStanja:");
            //                   if (R["@RezultatProvereStanja"].Trim() == "")
            //                   {
            //                     R = db.ExucuteStoreProcedureDva("LotProveraStanja",ref con, ref transaction, "IdDokView:" + IdDokumenta, "RezultatProvereStanja:");
            //                   }
            //                }
            //                else   // Provera NELOTOVSKIH DOKUMENATA
            //                    R = db.ExucuteStoreProcedureDva("ProveraStanja",ref con, ref transaction, "IdDokView:" + IdDokumenta, "RezultatProvereStanja:");
            //             }   

            //        return R["@RezultatProvereStanja"].Trim();
            return "";
        }
    }
}
