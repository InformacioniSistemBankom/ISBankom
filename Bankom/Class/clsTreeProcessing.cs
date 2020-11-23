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
using System.Reflection;
namespace Bankom.Class
{
    class clsTreeProcessing
    {
        string JavniNaziv;
        string Naziv;
        string IIdStablo;
        public Form form1 = new Form();
        public TreeView tv = new TreeView();

        public DataBaseBroker db = new DataBaseBroker();
        public string MojeStablo = "";
        public string mDokumentJe;
        public string ime2;
        public void podaciOstablu(Form forma, string iddok, string KojeStablo)
        {
            Form form1 = new Form();
        }
        public void ObradaStabla(Form forma, string iddok, string KojeStablo, string DokumentJe)
        {
            form1 = forma;
            MojeStablo = KojeStablo;
            form1.Controls["limestabla"].Text = KojeStablo;
            form1.Controls["limestabla"].Font = new Font("TimesRoman", 16, FontStyle.Regular); // ivana
            mDokumentJe = DokumentJe;
            //tv.AfterSelect += new TreeViewEventHandler(Tv_AfterSelect);
            tv.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tv_NodeMouseDoubleClick);
            tv.NodeMouseClick += new TreeNodeMouseClickEventHandler(tv_NodeMouseClick);

            int vveza;
            string sselect;
            string idke = Program.idkadar.ToString();
            string idfirme = Program.idFirme.ToString();
            tv.Nodes.Clear();
            //string idfirme = "6";

            //'  If (DokumentJe = "S" And KojeStablo = "Dokumenta") Or KojeStablo = "Izvestaji" Or KojeStablo = "PomocniSifarnici" Then
            if (KojeStablo == "Dokumenta" || KojeStablo == "Izvestaj")  //samo na ova stabla primenjujemo dozvole
            {
                sselect = "; WITH RekurzivnoStablo (ID_" + KojeStablo + "Stablo,Naziv, NazivJavni,Brdok,Vezan,RedniBroj,ccopy, Level,slave,pd,pp) AS "
                    + "(SELECT e.ID_" + KojeStablo + "Stablo,e.Naziv,e.NazivJavni,e.Brdok, e.Vezan,e.RedniBroj,e.ccopy,0 AS Level, CASE e.vrstacvora WHEN 'f' THEN 0 ELSE 1 END as slave, "
                    + " PrikazDetaljaDaNe as pd,PrikazPo as pp"
                    + " FROM " + KojeStablo + "Stablo AS e WITH (NOLOCK) "
                    + " where Naziv in (select g.naziv from Grupa as g,KadroviIOrganizacionaStrukturaStavkeView as ko Where (KO.ID_OrganizacionaStruktura = G.ID_OrganizacionaStruktura "
                    + " Or KO.id_kadrovskaevidencija = G.id_kadrovskaevidencija)  And KO.ID_OrganizacionaStrukturaStablo = " + idfirme + " and ko.id_kadrovskaevidencija=" + idke + " )"
                    + "UNION ALL  SELECT e.ID_" + KojeStablo + "Stablo,e.Naziv,e.NazivJavni,e.BrDok,e.Vezan,e.RedniBroj, e.ccopy,Level +1 ,  CASE e.vrstacvora WHEN 'f' THEN 0 ELSE 1 END as slave, "
                    + " PrikazDetaljaDaNe As pd, PrikazPo As pp  FROM " + KojeStablo + "Stablo  AS e WITH (NOLOCK) "
                    + " INNER JOIN RekurzivnoStablo AS d  ON e.ID_" + KojeStablo + "Stablo = d.Vezan) "
                    + " SELECT distinct ID_" + KojeStablo + "Stablo as ID, NazivJavni,Naziv,"
                    + "BrDok,Vezan,RedniBroj, slave,pd, pp FROM RekurzivnoStablo WITH(NOLOCK) where ccopy= 0  order by RedniBroj";
            }
            else
            {
                sselect = "; WITH RekurzivnoStablo (ID_" + KojeStablo + "Stablo,Naziv, NazivJavni,BrDok,Vezan,RedniBroj,ccopy, Level,slave,pd,pp) AS "
                       + "(SELECT e.ID_" + KojeStablo + "Stablo,e.Naziv,e.NazivJavni,e.BrDok, e.Vezan,e.RedniBroj,e.ccopy,0 AS Level, CASE e.vrstacvora WHEN 'f' THEN 0 ELSE 1 END as slave,"
                       + "  PrikazDetaljaDaNe as pd,PrikazPo as pp "
                       + "FROM " + KojeStablo + "Stablo AS e WITH (NOLOCK)"
                       + " UNION ALL "
                       + " SELECT e.ID_" + KojeStablo + "Stablo,e.Naziv,e.NazivJavni, e.BrDok,e.Vezan,e.RedniBroj, e.ccopy,Level +1 ,  CASE e.vrstacvora WHEN 'f' THEN 0 ELSE 1 END as slave,"
                       + "PrikazDetaljaDaNe As pd, PrikazPo As pp "
                       + " FROM " + KojeStablo + "Stablo AS e INNER JOIN RekurzivnoStablo AS d "
                       + " ON e.ID_" + KojeStablo + "Stablo = d.Vezan) "
                       + " SELECT distinct ID_" + KojeStablo + "Stablo as ID, NazivJavni,Naziv,BrDok, Vezan,RedniBroj, slave,pd, pp  FROM RekurzivnoStablo "
                       + " where ccopy=0 order by RedniBroj";
            }

            Console.WriteLine(sselect);
            DataTable ti = db.ReturnDataTable(sselect);
            DataTable tj = ti;


            //PUNIStablo:

            vveza = 0;

            tv.HideSelection = false;
            TreeNode parent = new TreeNode();

            tv.Nodes.Clear();

            parent.Name = "parent";
            parent.Text = KojeStablo;
            parent.Tag = "-1";
            parent.ImageIndex = 1;
            tv.Top = -5;
            tv.Nodes.Add(parent);
            tv.EndUpdate();
            tv.SelectedNode = parent;

            int i = 0;
            int j = 0;
            do // po i 
            {
                if (vveza != Convert.ToInt32(ti.Rows[i]["vezan"]))
                {
                    vveza = Convert.ToInt32(ti.Rows[i]["vezan"]);
                    if (vveza > 1)// nije nod vezan za root
                    {
                        j = 0;
                        do //po j
                        {
                            if (Convert.ToInt32(tj.Rows[j]["ID"]) == vveza)
                            {
                                string ffind;
                                ffind = tj.Rows[j]["Naziv"].ToString();
                                TreeNode[] tns = tv.Nodes.Find(ffind, true);
                                if (tns.Length > 0)
                                {
                                    tv.SelectedNode = tns[0];
                                    tv.Focus();
                                }
                                break;
                            }
                            j = j + 1;    //rsstabloveza.movenext
                        }
                        while (j < tj.Rows.Count);
                    }
                    else // vveza=1 root
                    {
                        tv.SelectedNode = parent;
                    }
                }

                if (KojeStablo == "Izvestaj")
                {
                    JavniNaziv = ti.Rows[i]["BrDok"].ToString() + "-" + ti.Rows[i]["NazivJavni"].ToString();
                }
                else
                {
                    JavniNaziv = ti.Rows[i]["NazivJavni"].ToString();
                }
                Naziv = ti.Rows[i]["Naziv"].ToString();
                IIdStablo = ti.Rows[i]["ID"].ToString();

                if (vveza != 0)
                {
                    WriteChild(JavniNaziv, Naziv, IIdStablo);
                }
                i = i + 1;

            } while (i < ti.Rows.Count);  //kraj while po i

            tv.Height = forma.Height;
            tv.Width = forma.Width;
            tv.Top = 40;
            tv.Left = 25;
            tv.Font = new Font("TimesRoman", 16, FontStyle.Regular);
            tv.Name = "tv";
            tv.BorderStyle = BorderStyle.None;
            form1.Controls.Add(tv);


            tv.Visible = true;
            tv.HideSelection = true;
            //tv.SelectedNode = null;
            tv.CollapseAll();
            tv.Sort();
        } // kraj obradastablaNew.
        private void WriteChild(string JavniNaziv, string Naziv, string IIdStablo)
        {
            TreeNode nod = new TreeNode()
            {
                Text = JavniNaziv,
                Name = Naziv,
                Tag = IIdStablo
            };

            TreeNode parentNode = tv.SelectedNode;///// ?? tv.Nodes[0];
            if (parentNode != null)
            {
                parentNode.Nodes.Add(nod);
                tv.EndUpdate();
            }
        }
        public void SrediFormu()
        {
          Program.Parent.flowLayoutPanel1.Width = 161;


            Program.Parent.flowLayoutPanel1.Width = 162;
            Program.Parent.flowLayoutPanel1.Width = 0;
            Program.Parent.button1.Location = new Point(0, 301);

        }

        private void tv_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
                //Djora 10.09.20
                Program.AktivnaForma = e.Node.Text.Substring(e.Node.Text.IndexOf("-") + 1).Replace(" ", "");

                if (tv.SelectedNode != null)
                {
                    if (Convert.ToInt32(tv.SelectedNode.Tag) > 1)
                    {
                        if (MojeStablo == "Izvestaj")
                        {

                            //string pravoime = tv.SelectedNode.Name.Substring(4);
                            string ime = tv.SelectedNode.Name;

                            string sql = " select s.ulazniizlazni as NazivDokumenta,NacinRegistracije as nr,"
                                      + " Knjizise,Izvor  from  SifarnikDokumenta as s"
                                      + "  Where s.naziv=@param0";

                            DataTable t = db.ParamsQueryDT(sql, ime);
                            if (t.Rows.Count > 0)
                            {
                                //Djora 10.09.20
                                int crta = tv.SelectedNode.Text.IndexOf("-");
                                if (crta > 0)
                                {
                                    //Program.AktivnaSifraIzvestaja = t.Rows["NazivDokumeta"] ;
                                    Program.AktivnaSifraIzvestaja = tv.SelectedNode.Text.Substring(0, crta);
                                }
                                else { Program.AktivnaSifraIzvestaja = ""; }


                                if (t.Rows[0]["nr"].ToString().ToUpper() == "B") // izvestaj je u bazi
                                {
                                    Program.Parent.ShowNewForm(MojeStablo, Convert.ToInt32(tv.SelectedNode.Tag), tv.SelectedNode.Name, 1, "", "", mDokumentJe, "", "");
                                }
                                else // izvestaj je excel
                                {
                                    string iddok = (tv.SelectedNode.Tag).ToString();
                                    string naslov = "print - " + ime;
                                    Boolean odgovor = false;
                                    odgovor = Program.Parent.DalijevecOtvoren("I", naslov, ime);
                                    if (odgovor == false)
                                    {
                                        frmPrint fs = new frmPrint();
                                        fs.BackColor = Color.Snow;
                                    fs.FormBorderStyle = FormBorderStyle.None;
                                        fs.MdiParent = Program.Parent;
                                        fs.Text = ime;
                                        fs.intCurrentdok = Convert.ToInt32(iddok); //id
                                        fs.LayoutMdi(MdiLayout.TileVertical);
                                        fs.imefajla = ime;  //ime  InoRacun
                                        fs.kojiprint = "rpt";
                                        fs.kojinacin = "E";
                                        fs.izvor = t.Rows[0]["Izvor"].ToString();
                                        fs.Show();
                                        Program.Parent.addFormTotoolstrip1(fs, naslov);
                                    }
                                }
                            }
                            SrediFormu();
                        }
                        else
                        {
                            //Djora 10.09.20
                            Program.AktivnaSifraIzvestaja = "";

                            Program.Parent.ShowNewForm(MojeStablo, Convert.ToInt32(tv.SelectedNode.Tag), tv.SelectedNode.Name, 1, "", "", mDokumentJe, "", "");
                            SrediFormu();
                        }
                    }
                }
            }

        //zajedno 28.10.2020.
        public void tv_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SrediFormu();
            Program.Parent.ToolBar.Items["Uunos"].Visible = true;
            Program.Parent.ToolBar.Items["Uunos"].Enabled = true;

            Program.Parent.ToolBar.Items["toolStripTextBox1"].Enabled = true;
            Program.Parent.ToolBar.Items["toolStripTextBox1"].Visible = true;

            Program.AktivnaSifraIzvestaja = e.Node.Text.ToString();
            Program.IdSelektovanogCvora = int.Parse(e.Node.Tag.ToString());


            

        }
      
    }
    
}















       


   


   

    
    

