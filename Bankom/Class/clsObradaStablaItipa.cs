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


namespace Bankom.Class
{
    class clsObradaStablaItipa  // OBUHVACENE SU SVE SPECIFICNOSTI STABLA IZVESTAJA
    {
        string sselect = "";
        string mselect = "";
        string uslov = "";
        string Oorder = "";
        string Ggroup = "";
        string TUD="";
        DataBaseBroker db = new DataBaseBroker();
        //public string Proces(string Stablo, string Dokument, int IdTree)
        //{            
            //Form form1 = new Form();
            //form1 = Program.Parent.ActiveMdiChild;
           

            //mselect = "Select Upit From Upiti Where ime='GgRr" + Dokument + "StavkeView' AND TUD>0 Order by TUD";
            //DataTable tu = db.ReturnDataTable(mselect);
            //sselect = tu.Rows[0]["Upit"].ToString();

            //mselect = "Select idDok From RefreshGrida Where Dokument='" + Dokument+"' and imeupita like N'" +  "%StavkeView'";
            //tu = db.ReturnDataTable(mselect);

            //uslov = " Where " + tu.Rows[0]["idDok"] + "=-1";

            //int vv = sselect.IndexOf("GROUP BY");
            //if (vv > -1)
           
            //{
            //    Ggroup = sselect.Substring(vv);
            //    sselect = sselect.Substring(0, vv - 1);
            //}
            
            //sselect += uslov +" "+ Ggroup;
            //Console.WriteLine(sselect);

        //    return sselect;
        //} 
        public string Proces(string Dokument,string TUD)
        {

            mselect = "Select Ime,Upit From Upiti Where Ime like'GgRr%' AND NazivDokumenta='" + Dokument + "' AND TUD=" +TUD;
            Console.WriteLine(mselect);
            DataTable tu = db.ReturnDataTable(mselect);
            sselect = tu.Rows[0]["Upit"].ToString();

            mselect = "Select idDok From RefreshGrida Where Dokument='" + Dokument + "' and ImeUpita='" + tu.Rows[0]["Ime"].ToString().Substring(4)+"'";
            Console.WriteLine(mselect);
            tu = db.ReturnDataTable(mselect);

            uslov = " Where " + tu.Rows[0]["idDok"] + "=-1";

            int vv = sselect.IndexOf("GROUP BY");
            if (vv > -1)

            {
                Ggroup = sselect.Substring(vv);
                sselect = sselect.Substring(0, vv - 1);
            }

            sselect += uslov + " " + Ggroup;
            Console.WriteLine(sselect);

            return sselect;

        }

    }
}
