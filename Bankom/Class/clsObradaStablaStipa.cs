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
    class clsObradaStablaStipa // OBUHVACENE SU SVE SPECIFICNOSTI STABLA OSNOVNIH SIFARNIKA (dokumenta,artikli,komitenti,orgstruktura)
    {
        private DataBaseBroker db = new DataBaseBroker();
        public int IDStablo = 0;
        public Form form1;
        public string dokje = "";
        public string mStablo = "";
        public string Proces(string Stablo, string Dokument, int IdTree)
        {
            DataBaseBroker db = new DataBaseBroker();
            form1 = Program.Parent.ActiveMdiChild;
            string SelectUpit = "";
            IDStablo = IdTree;
            mStablo = Stablo;
            //form1.Controls["lidstablo"].Text = IdTree.ToString();
            string sstablo = "";                 
         
            sstablo = "select VrstaCvora,Naziv from " + mStablo + "Stablo WITH(NOLOCK) where Id_" + mStablo + "Stablo=" + IDStablo.ToString();
            DataTable ts = db.ReturnDataTable(sstablo);
           
            if (mStablo.Trim() == "Dokumenta") 
            {
                if (ts.Rows[0]["VrstaCvora"].ToString() == "f")
                {
                    MessageBox.Show("Izaberite dokument umesto grupe!!");

                    return "";
                }
            }   
   
          //  ((Bankom.frmChield)form1).toolStripTexIme.Text = Dokument;           
            SelectUpit = CreateQuery(Stablo, Dokument);
            return SelectUpit;
        }

        private string CreateQuery(string Stablo,string  Dokument)
        {
            string Ddokument = "";
            string sselect = "";
            string uslov = "";
            string Oorder = "";
            string stab = "";
            string sifusif = "";
            string sifra = "";
            Ddokument = Dokument;
            bool ProveraGodine = false;
            string mselect = "";           
            int idFirme = Program.idFirme;
            string Firma = Program.imeFirme; ////"Bankom";

            stab = Stablo.Trim() + "Stablo";
            if (Stablo != "")
            {
                sifusif = "Sif" + Stablo.Substring(0, 3);
            }
            mselect = "Select Upit From Upiti Where ime='GgRr" + Stablo + "StavkeView'";
           
            DataTable tu = db.ReturnDataTable(mselect);
            if (tu.Rows.Count > 0)
            {                
                    sselect = tu.Rows[0]["Upit"].ToString() + " as s ";
                    if (Stablo == "Komitenti" && Firma != "Leotar")
                    {
                        Oorder = "  order by  s.NazivKom ";
                    }
                    else
                    {
                        Oorder = "  order by  s.id_" + Stablo.Trim() + "Totali desc";
                    }               
                
            }
            if (Stablo == "Dokumenta")
            {
                mselect = "Select sifra,Naziv,PrikazDetaljaDaNe,VrstaCvora,PrikazPo,OgraniceneGodine from " + stab + " WITH(NOLOCK) where ID_" + stab + "=" + Convert.ToString(IDStablo);
                Console.WriteLine(mselect);
            }
            else
            {
                mselect = "Select sifra,Naziv,PrikazDetaljaDaNe,VrstaCvora,PrikazPo from " + stab + " WITH(NOLOCK) where ID_" + stab + "=" + Convert.ToString(IDStablo);
            }
            DataTable ts = db.ReturnDataTable(mselect);
            if (ts.Rows.Count > 0)
            {
                sifra = ts.Rows[0]["sifra"].ToString();
                switch (ts.Rows[0]["PrikazDetaljaDaNe"].ToString())
                {
                    case "0":            // nema tabelu na nivou detalja : izvestaji , pomocni sifarnici 
                        {
                            sselect = "";
                            Oorder = "";
                            break;
                        }
                    case "1":              // ima tabelu na nivou detalja  : dokumenta
                        {
                            if (ts.Rows[0]["VrstaCvora"].ToString() == "d")
                            {
                                switch (ts.Rows[0]["PrikazPo"].ToString()) // Uslov za prikaz pripadajucih clanova
                                {
                                    case "0":              // uslov = firma ;  prikazuju se samo clanovi koji pripadaju firmi(DOKUMENTA)
                                        uslov = " WHERE s.ID_" + stab + "=" + IDStablo.ToString() + " and s.nazivorg=ss.NazivOrg And ss.ID_OrganizacionaStrukturaStablo=" + idFirme.ToString();
                                        sselect = sselect + ",OrganizacionaStrukturaStavkeView as  ss ";
                                        sselect = sselect.Replace("NazivOrg", "s.NazivOrg");
                                        sselect = sselect.Replace("IId", "s.IId");
                                        Console.WriteLine(sselect);
                                        break;
                                    case "1":                //idstablo prikazuju se samo clanovi koji imaju isti IDSTABLO(ista vrsta dokumenata npr svi InoRacuni)

                                        uslov = " WHERE s.ID_" + stab + "=" + IDStablo.ToString();
                                        break;
                                    case "2":               // sve prikazuju se svi podaci bez ogranicenja
                                        uslov = " WHERE s." + sifusif + " like '" + sifra.Trim() + "%'  ";
                                        break;
                                }
                            }
                            else
                            {
                                sselect = "";
                                Oorder = "";
                                break;
                            }
                            break;
                        }
                    case "2":         // ima tabelu na nivou grupe : artikli,komitnti,organizaciona struktura
                        {
                            if (ts.Rows[0]["VrstaCvora"].ToString() == "d")
                            {
                                uslov = " WHERE s.ID_" + stab.ToString() + "=" + IDStablo.ToString() + " and ccopy=0";
                                break;
                            }
                            else
                            {
                                uslov = " WHERE s." + sifusif + " like '" + sifra.ToString() + "%' and s.ccopy=0 ";
                                break;
                            }
                        }
                }
                //treba se vrsiti provera godine dokumenta
                ProveraGodine = false;
                if (Ddokument == "Dokumenta")
                {
                    if (ts.Rows[0]["OgraniceneGodine"].ToString() == "1") //mogu se videti samo one godine koje su dozvoljene za prijavljenog user-a
                    {
                        //if( mGodine.Trim() == "")
                        //{

                        //}
                    }
                }
                else
                {
                    ProveraGodine = true;
                    /// uslov = uslov + " AND " + mGodine.Replace("WHERE", "");
                }
                
                sselect = string.Concat(sselect, uslov, Oorder);
                Console.WriteLine(sselect);
            }

            return sselect;
        }
        

    }
}
