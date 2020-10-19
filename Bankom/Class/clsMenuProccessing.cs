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
    class clsMenuProccessing
    {
        //Program.Parent.menustrip2 menustrip2 = new System.Windows.Forms.MenuStrip();
        public void Proccess()
        {
                DataBaseBroker db = new DataBaseBroker();
                string sselect = "; WITH RekurzivnoStablo (ID_MenuCsStablo,Naziv, NazivJavni,BrDok,Vezan,RedniBroj,ccopy, Level,slave,pd,pp) AS "
                           + "(SELECT e.ID_MenuCsStablo,e.Naziv,e.NazivJavni,e.BrDok, e.Vezan,e.RedniBroj,e.ccopy,0 AS Level, CASE e.vrstacvora WHEN 'f' THEN 0 ELSE 1 END as slave,"
                           + "  PrikazDetaljaDaNe as pd,PrikazPo as pp "
                           + "FROM MenuCsStablo AS e WITH (NOLOCK)"
                           + " UNION ALL "
                           + " SELECT e.ID_MenuCsStablo,e.Naziv,e.NazivJavni, e.BrDok,e.Vezan,e.RedniBroj, e.ccopy,Level +1 ,  CASE e.vrstacvora WHEN 'f' THEN 0 ELSE 1 END as slave,"
                           + "PrikazDetaljaDaNe As pd, PrikazPo As pp "
                           + " FROM MenuCsStablo AS e INNER JOIN RekurzivnoStablo AS d "
                           + " ON e.ID_MenuCsStablo = d.Vezan) "
                           + " SELECT distinct ID_MenuCsStablo as ID, NazivJavni,Naziv,BrDok, Vezan,RedniBroj, slave,pd, pp  FROM RekurzivnoStablo "
                           + " where ccopy=0 order by RedniBroj";
                DataTable ti = db.ReturnDataTable(sselect);
                DataTable tj = ti;
                if (ti.Rows.Count > 0) { }

                //PUNIStablo:

                string Naziv = "";
                string JavniNaziv = "";
                string Parent = "";
                int vveza = 0;
                int i = 0;
                int j = 0;
                int slave;
                int ttag = 0;
                int upisan = 0;
                //var menustrip2 = new System.Windows.Forms.MenuStrip();

                //var pparent = new System.Windows.Forms.ToolStripMenuItem();
                do // po i 
                {
                    if (vveza != Convert.ToInt32(ti.Rows[i]["vezan"]))
                    {
                        vveza = Convert.ToInt32(ti.Rows[i]["vezan"]);
                    }
                    Naziv = ti.Rows[i]["Naziv"].ToString();
                    JavniNaziv = ti.Rows[i]["NazivJavni"].ToString();
                    if (Naziv == "ProcesiranjeDnevnogIzvestaja")
                    {
                        upisan = 1;
                        break;
                    }
                    if (vveza != ttag)
                    {
                        slave = Convert.ToInt32(ti.Rows[i]["slave"]);
                        ttag = Convert.ToInt32(ti.Rows[i]["ID"]);
                        AddMenu(Naziv, JavniNaziv, slave, ttag);
                        if (slave == 0)  // ima decu
                        {
                            j = 0;
                            do //po j // pronadji svu decu
                            {
                                if (Convert.ToInt32(tj.Rows[j]["vezan"]) == ttag)
                                {
                                    Naziv = tj.Rows[j]["Naziv"].ToString();
                                    JavniNaziv = ti.Rows[j]["NazivJavni"].ToString();
                                    slave = 3;   ////    deca                Convert.ToInt32(ti.Rows[j]["slave"]);
                                    AddMenu(Naziv, JavniNaziv, slave, ttag);
                                }
                                else
                                {
                                   
                                }
                                j = j + 1;
                            }
                            while (j < tj.Rows.Count);     /// kraj while po j 
                        }
                    }
                    i = i + 1;
                }
                while (i < ti.Rows.Count);  //kraj while po i


            } // kraj obradastablaNew.
            private void AddMenu(string naziv, string javninaziv, int slave, int ttag)
            {

                //var item = new System.Windows.Forms.ToolStripMenuItem();
                //item.Click += new EventHandler(item_Click);


                //item.Name = naziv;
                //item.Text = javninaziv;
                //item.Tag = ttag;


                //switch (slave)
                //{
                //    case 0: /// ima decu
                //        menuStrip2.Items.Add(item);
                //        BankomMDI.pparent = item;
                //        break;
                //    case 1: /// nema decu
                //        menuStrip2.Items.Add(item);
                //        break;
                //    case 3:  /// deca                    
                //        AddSubMenu(item, pparent.Name);
                //        break;
                //}
                //menuStrip2.Show();


            }
            private void AddSubMenu(ToolStripMenuItem ChildItem, string ParentItem)
            {
                //foreach (ToolStripMenuItem item in menuStrip2.Items)
                //{
                //    if (ParentItem == item.Name.ToString())
                //    {
                //        item.DropDownItems.Add(ChildItem);
                //    }
                //}
            }        

    }
}
