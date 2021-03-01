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
    class clsRefreshForm
    {
        DataBaseBroker db = new DataBaseBroker();
        Form forma = new Form();

        public void refreshform()
        {
            forma = Program.Parent.ActiveMdiChild;
            //string dokje,string imestabla,string ime,string idstablo,string ident)
            string supit = "";
            string imestabla = "";
            if (forma.Controls["limestabla"].Text!="")
               imestabla=forma.Controls["limestabla"].Text;
            string dokje = forma.Controls["ldokje"].Text;
           
            string ident = forma.Controls["liddok"].Text;
            string idstablo = forma.Controls["lidstablo"].Text;
            string ime = forma.Controls["limedok"].Text;

            clsdokumentRefresh dr = new clsdokumentRefresh();

            switch (dokje)
            {
                case "K":
                    clsTreeProcessing tw = new clsTreeProcessing();
                    tw.ObradaStabla(forma, "1", imestabla, dokje);
                    break;
                case "S":
                    if (idstablo == "1") { break; }
                    clsObradaStablaStipa procs = new clsObradaStablaStipa();
                    supit = procs.Proces(imestabla, ime, Convert.ToInt32(idstablo));
                    if (supit.Trim() != "")
                    {
                        dr.refreshDokumentBody(forma, imestabla, idstablo, dokje);
                        dr.refreshDokumentGrid(forma, imestabla, idstablo, supit, "1", dokje);
                    }
                    break;
                case "P":
                    if (idstablo == "1") { break; }
                    clsObradaStablaPtipa procp = new clsObradaStablaPtipa();
                    supit = procp.Proces(imestabla, ime, Convert.ToInt32(idstablo));
                    dr.refreshDokumentGrid(forma, ime, idstablo, supit, "1", dokje);
                    break;
                case "I":
                    if (idstablo == "1") { break; }
                    string sel = "Select TUD From Upiti Where NazivDokumenta='" + ime + "' and ime like'GGrr%' AND TUD>0 Order by TUD";
                    Console.WriteLine(sel);
                    DataTable ti = db.ReturnDataTable(sel);
                    clsObradaStablaItipa proci = new clsObradaStablaItipa();
                    for (int j = 0; j < ti.Rows.Count; j++)
                    {
                        supit = proci.Proces(ime, ti.Rows[j]["TUD"].ToString());
                        Console.WriteLine(supit);
                        dr.refreshDokumentGrid(forma, ime, idstablo.ToString(), supit, ti.Rows[j]["TUD"].ToString(), dokje);
                    }
                    break;
                default:
                    dr.refreshDokumentBody(forma, ime, ident, dokje);
                    dr.refreshDokumentGrid(forma, ime, ident, "", "", dokje);
                    break;
            }

        }
        
    }
}
