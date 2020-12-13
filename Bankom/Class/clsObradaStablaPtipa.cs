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
    class clsObradaStablaPtipa
    {             // OBUHVACENE SU SVE SPECIFICNOSTI STABLA POMOCNIH SIFARNIKA 
        public string Proces(string Stablo, string Dokument, int IdTree)
        {
            DataBaseBroker db = new DataBaseBroker();
            Form form1 = new Form();
            form1 = Program.Parent.ActiveMdiChild;   
            string sselect = "";
            string mselect = "";
            //form1.Controls["lidstablo"].Text = IdTree.ToString();            
            string uslov = "";
            string Oorder = "";                     

             //((Bankom.frmChield)form1).toolStripTexIme.Text = Dokument;
            mselect = "Select Upit From Upiti Where ime='GgRr" + Dokument + "StavkeView'";
            Console.WriteLine(mselect);
            DataTable tu = db.ReturnDataTable(mselect);        
            sselect = tu.Rows[0]["Upit"].ToString();           
            
            if (sselect.ToUpper().Contains("WHERE") == true)
            {
                uslov = " AND ID_" + Dokument + "StavkeView>1";
            }
            else
            {
                uslov = " WHERE ID_" + Dokument + "StavkeView>1";
            }
          
            Console.WriteLine(sselect);
            int vv = sselect.ToUpper().IndexOf("ORDER BY");
            if (vv == -1)
            {
                //vv = sselect.Length;
            }
            else
            {
                Oorder = sselect.Substring(vv);
                sselect = sselect.Substring(0, vv - 1);
            }
            Console.WriteLine(sselect + " " + uslov + " " + Oorder);
            return sselect +" " + uslov +" " + Oorder;
        }

    }
}
