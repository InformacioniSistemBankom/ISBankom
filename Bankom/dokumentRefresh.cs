using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization ;
using System.Drawing;

namespace Bankom.Class
{
    class dokumentRefresh
    {
        Form form1 = new Form();
        static DataBaseBroker db = new DataBaseBroker();
        public void refreshDokument(Form forma, string KojiSegment, string iddok, string dokument, string DokumentJe)
        {
            form1 = forma;
            if (KojiSegment.Contains("Stavke") == true)
            {

                refreshDokumentGrid(form1, dokument, iddok, "", "");
            }
            else
            {
                if (DokumentJe != "I")
                {
                    refreshDokumentBody(form1, dokument, iddok, DokumentJe);

                }
            }
        }
        private static string CreateQuery(string KojiUpit, string KojiSegment, string iddok, string dokument,string DokumentJe) // vraca string
        {
            string UUslov = null;
            string PocetakUpita = null;
            string KrajUpita = null;
            string VN = null; // grid vezan da ne
            string NazivKlona = null;

            DataTable tsd = db.ReturnDataTable("Select * from sifarnikdokumenta where naziv='" + dokument + "'");
            NazivKlona = tsd.Rows[0]["UlazniIzlazni"].ToString();
            DataTable rtt = db.ReturnDataTable("Select * from RefreshGrida where ImeUpita='" + KojiSegment + "'");
            VN = rtt.Rows[0]["TotaliDaNe"].ToString();

            if (iddok != "0")
            {
                if (DokumentJe == "P")
                {
                    UUslov = rtt.Rows[0]["IdDok"].ToString() + ">" + iddok;
                }
                else
                {
                    if (dokument == "NalogZaRazduzenjeAmbalaze" && KojiSegment == "NalogZaRazduzenjeAmbalazeStavke1View")
                    {
                        DataTable rtp = db.ReturnDataTable("Select ID_Predhodni from dokumentaTotali Where ID_DokumentaTotali=" + iddok);
                        UUslov = rtt.Rows[0]["IdDok"].ToString() + "=" + (rtp.Rows[0]["ID_Predhodni"].ToString());
                    }
                    else
                    {
                        UUslov = rtt.Rows[0]["IdDok"].ToString() + "=" + iddok;
                    }
                }
            }

            if (VN == "NE")
            {
                KojiUpit = KojiUpit.Replace(NazivKlona + "Totali", rtt.Rows[0]["ImeUpita"].ToString());
            }


            if (UUslov != null && UUslov.Trim(' ') != "") // uslov nije prazan
            {
                if (KojiUpit.Contains("WHERE") == false) //  u upitu ne postoji rec WWHERE
                {
                    if (KojiUpit.Contains("ORDER BY") == true || KojiUpit.Contains("GROUP BY") == true)    // u upitu postoje reci ORDER BY ili GROUP BY
                    {
                        if (KojiUpit.Contains("GROUP BY") == true && KojiUpit.Contains("ORDER BY") == false)
                        {
                            KrajUpita = KojiUpit.Substring(0, KojiUpit.IndexOf("GROUP BY"));
                            PocetakUpita = KojiUpit.Substring(1, KojiUpit.IndexOf("GROUP BY") - 1);
                        }
                        if (KojiUpit.Contains("ORDER BY") == true && KojiUpit.Contains("GROUP BY") == false)
                        {
                            KrajUpita = KojiUpit.Substring(KojiUpit.IndexOf("ORDER BY"));
                            PocetakUpita = KojiUpit.Substring(0, KojiUpit.IndexOf("ORDER BY") - 1);
                            KojiUpit = string.Concat(PocetakUpita + " WHERE " + UUslov + " " + KrajUpita);
                        }
                        if (KojiUpit.Contains("ORDER BY") == false)
                        {
                            KrajUpita = KojiUpit.Substring(0, KojiUpit.IndexOf("GROUP BY"));
                            PocetakUpita = KojiUpit.Substring(1, KojiUpit.IndexOf("GROUP BY") - 1);
                        }
                        //if (KojiUpit.Contains("ORDER BY") == true)
                        //{
                        //    KrajUpita = KojiUpit.Substring(KojiUpit.IndexOf("ORDER BY"));
                        //    PocetakUpita = KojiUpit.Substring(0, KojiUpit.IndexOf("ORDER BY") - 1);
                        //    KojiUpit = string.Concat(PocetakUpita + " WHERE " + UUslov + " " + KrajUpita);
                        //}
                        //KojiUpit = string.Concat(PocetakUpita, " WHERE ", UUslov, " ", KrajUpita);
                    }
                    else
                    {
                        KojiUpit = string.Concat(KojiUpit, " WHERE ", UUslov);
                    }
                }
                else  // WHERE nije prazan
                {
                    KojiUpit = string.Concat(KojiUpit, " WHERE ", UUslov);
                }
            }  // raj uslov nije prazan
            else //  USLOV JE PRAZAN
            {
                
                KrajUpita = KojiUpit.Substring(KojiUpit.IndexOf("WHERE") + 5);
                PocetakUpita = KojiUpit.Substring(1, KojiUpit.IndexOf("WHERE") + 5);
                KojiUpit = string.Concat(PocetakUpita, UUslov, " AND ", KrajUpita);
            }
            return KojiUpit;
        }
        //
        public void refreshDokumentGrid(Form forma, string dokument, string iddok, string mUpit, string DokumentJe)
        {
            form1 = forma;
            string tUpit;
            string tIme;
            string tud;
            string KojiSegment;
            if (dokument !="Dokumenta")
            { }
            

            //     string xxx = ((Bankom.frmChield)form1).imedokumenta;

            string ss = " SELECT TUD, MaxHeight, Levo, Vrh, Width, height, Upit, Ime "
                                                  + " FROM dbo.Upiti "
                                                  + " WHERE(NazivDokumenta = N'" + dokument + "' AND Ime Like'GgRr%') "
                                                  + " AND(TUD <> 0) ORDER By TUD";
            DataTable t = db.ReturnDataTable(ss);

           
            if (t.Rows.Count > 0)
            {
                for (int i = 0; i <= t.Rows.Count - 1; i++)
                {
                    tIme = t.Rows[i]["Ime"].ToString();
                    KojiSegment = tIme.Substring(4, tIme.Length - 4);
                    tud = t.Rows[i]["TUD"].ToString();
                    //mUpit= t.Rows[i]["Upit"].ToString();
                    if (!(form1.Controls.Find(tIme, true).FirstOrDefault() is DataGridView dv))
                    {
                        MessageBox.Show("Greska ne postoji grid");
                    }
                    else
                    {
                        dv.Visible = true;
                        
                        //if (DokumentJe == "S")

                        if (mUpit.Trim() != "")
                        {
                            tUpit = mUpit;
                            tUpit = tUpit.Replace("SELECT DISTINCT", "SELECT "); //SELECT TOP (200) 
                        }
                        else
                        {
                            tUpit = t.Rows[i]["Upit"].ToString();
                            tUpit = CreateQuery(tUpit, KojiSegment, iddok, dokument,DokumentJe);
                        }
                        if( DokumentJe == "D" && tud == "1")
                        {
                            tUpit= tUpit+" Order by iid DESC ";
                        }
                        
                        string RbPolje = "ID_GgRr" + (KojiSegment).Trim();




                        int intfrom = tUpit.IndexOf("from", StringComparison.OrdinalIgnoreCase); ;
                        int intOrder = tUpit.IndexOf("order", StringComparison.OrdinalIgnoreCase);
                        int intstart = 0;

                        if (form1.Text.ToString().IndexOf("(") == -1)
                        {
                           int intUkupno = ((Bankom.frmChield)form1).intUkupno;
                            if (iddok != "1")
                            {

                                Program.pocetak = 0;
                                if (intOrder > intfrom)
                                {

                                    ((Bankom.frmChield)form1).intUkupno = db.ReturnInt(" select count(*) from " + tUpit.Substring(intfrom + 5, intOrder - intfrom - 5), 0);
                                    ((Bankom.frmChield)form1).ToolStripLblPos.Text = Convert.ToString(((Bankom.frmChield)form1).intUkupno / 25);
                                

                                     switch (dokument)
                                    {
                                        case "Dokumenta":
                                             DataTable tbb = db.ReturnDataTable("select top 1 naziv from DokumentaStablo where ID_DokumentaStablo=" + iddok);
                                            ((Bankom.frmChield)form1).toolStripTexIme.Text = Convert.ToString(tbb.Rows[0][0]);
                                            break;
                                        case "Artikli":
                                            
                                            DataTable tbbart = db.ReturnDataTable("select top 1 * from ArtikliStablo where ID_ArtikliStablo=" + iddok);
                                            ((Bankom.frmChield)form1).toolStripTexIme.Text = Convert.ToString(tbbart.Rows[0][0]);
                                            break;

                                        default:
                                            break;
                                    }

                                    ((Bankom.frmChield)form1).toolStripTextBroj.Text = iddok;


                                }
                                else
                                {
                                    intstart = ((Bankom.frmChield)form1).intStart; // PROVERI DA LI TREBA

                                    ((Bankom.frmChield)form1).intUkupno = db.ReturnInt(" select count(*) from " + tUpit.Substring(intfrom + 5), 0);
                                }
                            }

                        }


                        if (intOrder > -1) { tUpit += " OFFSET " + Program.pocetak.ToString() + " ROWS FETCH NEXT 25 ROWS ONLY;"; }
                        if (mUpit != "") { ((Bankom.frmChield)form1).Controls["statusStrip1"].Visible = true; }
                        DataTable tg = db.ReturnDataTable(tUpit);
                        if (tg.Rows.Count > 0)
                        {
                            if (DokumentJe == "D")
                            {
                                for (int k = 0; k <= tg.Rows.Count - 1; k++)
                                {
                                    //tg.Rows[k][RbPolje].Value = k;
                                }
                            }

                            dv.DataSource = tg;
                            dv.Visible = true;
                        }
                        setingWidthOfColumns(dokument, dv, tud);
                        dv.Visible = true;
                    }
                }

            }
        }
        private void setingWidthOfColumns(string dokument, DataGridView dv, string tud)
        {
            string sel = "SELECT  AlijasPolja, T.Tip,WidthKolone, width,T.Format,T.CSharp,T.Alajment  FROM dbo.RecnikPodataka AS R1,TipoviPodataka AS T "
                                           + " WHERE R1.ID_TipoviPodataka=T.ID_TipoviPodataka  AND " +
                                           " TUD = " + tud + " AND Dokument = N'" + dokument + "'  AND " +
                                            "(widthKolone>0 OR ID_NaziviNaFormi = 20 OR ID_NaziviNaFormi = 25 OR AlijasPolja LIKE 'IId%' OR " +
                                            "LTRIM(RTRIM(AlijasPolja)) = 'ID_' + LTRIM(RTRIM(AlijasTabele))) ORDER BY TabIndex ";
            DataTable t2 = db.ReturnDataTable(sel);
            int k = t2.Rows.Count;
            if (k > 0)
            {
                int j = dv.ColumnCount;
                if (k > j)
                {
                    //MessageBox.Show("Popravite upite nedostaje neka od kolona iz recnika!!!!!");
                }
                else
                {
                    for (int i = 0; i <= t2.Rows.Count - 1; i++)
                    {
                        string NazivKolone = dv.Columns[i].Name.ToUpper();
                        int sirina = (int)Convert.ToDouble(t2.Rows[i]["WidthKolone"].ToString());
                        sirina = sirina * 2;
                        if (sirina == 0)    
                        {
                            dv.Columns[i].Visible = false;
                        }
                        else
                        {
                            dv.Columns[i].Visible = true;
                        }
                        dv.Columns[i].Width = sirina;
                        dv.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                        if (t2.Rows[i]["Format"].ToString() != "@" && t2.Rows[i]["Format"].ToString() != "0" && t2.Rows[i]["Format"].ToString().Trim() != "")
                        {
                            dv.Columns[i].DefaultCellStyle.Format = t2.Rows[i]["Format"].ToString();
                        }
                        switch (t2.Rows[i]["Alajment"].ToString())
                        {
                            case "0":
                                dv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                                break;
                            case "1":
                                dv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                break;
                            case "2":
                                dv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                break;
                        }

                    }
                    dv.Visible = true;
                }
            }
        }
        public void refreshDokumentBody(Form forma, string dokument, string iddok, string DokumentJe)
        {
            writeFrom(forma, dokument, iddok,DokumentJe);
            CalculatedField(forma, dokument, iddok);
        }

        private void writeFrom(Form forma, string dokument, string iddok,string DokumentJe)
        {

            DataBaseBroker db = new DataBaseBroker();
            DataTable tt = db.ReturnDataTable(" SELECT Upit,ime "
                                            + " FROM dbo.Upiti "
                                            + " WHERE(NazivDokumenta = N'" + dokument + "') "
                                            + " AND Ime LIKE N'ggrr%' "
                                            + " AND(TUD = 0)");
            for (int i = 0; i <= tt.Rows.Count - 1; i++)
            {
                string KojiSegment = tt.Rows[i]["ime"].ToString();
                KojiSegment = KojiSegment.Substring(4, KojiSegment.Length - 4);
                string tUpit = CreateQuery(tt.Rows[i]["Upit"].ToString(), KojiSegment, iddok, dokument,DokumentJe);

                DataTable dt = db.ReturnDataTable(tUpit);

                for (int j = 0; j <= dt.Rows.Count - 1; j++)
                {
                    for (int k = 0; k <= dt.Columns.Count - 1; k++)
                    {
                        foreach (var ctrls in forma.Controls.OfType<Field>())
                        {
                            if (ctrls.IME == dt.Columns[k].ColumnName.ToString())
                            {
                                ctrls.ID = "1";
                                ctrls.Vrednost = dt.Rows[j][dt.Columns[k].ColumnName].ToString();   ///.Replace(".", ",");

                                if (ctrls.VrstaKontrole == "tekst")
                                {
                                    clsOperacije co = new clsOperacije();
                                    bool r = co.IsNumeric(dt.Rows[j][dt.Columns[k].ColumnName].ToString());
                                    switch (r)
                                    {
                                        case true:
                                            int tacka = -1;
                                            int tacka0 = dt.Rows[j][dt.Columns[k].ColumnName].ToString().LastIndexOf(".");
                                            if (tacka0 > -1)
                                            {
                                                tacka = dt.Rows[j][dt.Columns[k].ColumnName].ToString().Substring(dt.Rows[j][dt.Columns[k].ColumnName].ToString().LastIndexOf(".")).Length;
                                            }

                                            if (tacka > 1)
                                            {
                                                ctrls.textBox.Text = Convert.ToString(float.Parse(dt.Rows[j][dt.Columns[k].ColumnName].ToString()).ToString("###,##0.000"));
                                            }
                                            else
                                            {

                                                ctrls.textBox.Text = Convert.ToString(float.Parse(dt.Rows[j][dt.Columns[k].ColumnName].ToString()).ToString("###,##0.00"));
                                            }
                                            break;
                                        case false:
                                            ctrls.textBox.Text = String.Format("{0:#,###}", dt.Rows[j][dt.Columns[k].ColumnName].ToString());
                                            break;
                                    }
                                }

                                else if (ctrls.VrstaKontrole == "datum")
                                {
                                    ctrls.dtp.Text = string.Format("{0:dd.mm.yy}", dt.Rows[j][dt.Columns[k].ColumnName].ToString());
                                }
                                else if (ctrls.VrstaKontrole == "combo")
                                {
                                    ctrls.comboBox.Text = dt.Rows[j][dt.Columns[k].ColumnName].ToString();
                                    string kojiid;
                                    kojiid = "ID_" + ctrls.cAlijasTabele;
                                    for (int kk = 0; kk <= dt.Columns.Count - 1; kk++)
                                    {
                                        if (dt.Columns[kk].ColumnName.ToString() == kojiid)
                                        {
                                            ctrls.ID = dt.Rows[j][dt.Columns[kk].ColumnName].ToString();
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
        private Double CalculatedField(Form form1, string dokument, string iddok)
        {
            CcalculatedVal cv = new CcalculatedVal();
            clsEvaluation cev = new clsEvaluation();
      
            string KojaValuta;
            KojaValuta = "RSD";
            string KojePolje;
            //double[] vrednosti;

            double mvrednost = 0;
            decimal vrednost = 0;
            string formula = "";            
            
            foreach (var pb in form1.Controls.OfType<Field>())
            {
                if (pb.cPolje.Contains("IzvodiSe") == true)
                {
                    if (pb.cFormulaForme == "")
                    {
                        pb.Vrednost = "0";

                    }
                    else
                    {                        
                        formula = pb.cFormulaForme;
                        string  aaa = cv.CalculateValue(form1, formula); //textBox.Text = Convert.ToString(float.Parse(dt.Rows[j][dt.Columns[k].ColumnName].ToString()).ToString("###,##0.00"));
                        string rez = cev.Evaluate(aaa);
                        pb.Vrednost = Convert.ToString(rez);
                        if (pb.Vrednost.Trim() != "")
                        {
                            pb.textBox.Text = Convert.ToString(float.Parse(rez.ToString()).ToString("###,##0.00"));
                        }
                    }
                    //break;
                }                
            }

            foreach (var ctrls in form1.Controls.OfType<Field>())    
            {
                if (ctrls.IME == "OznVal")
                {
                    if (ctrls.Vrednost != "RSD")/// DomacaValuta Then
                    {
                        KojaValuta = ctrls.Vrednost;
                        break;
                    }

                }
            }

            foreach (var pb in form1.Controls.OfType<Field>())
            { 
                if (pb.cPolje.Contains("Slovima") == true)
                {
                    if (pb.cFormulaForme == "")
                    {
                        KojePolje = "Suma" + dokument.Trim();
                    }
                    else
                    {
                        KojePolje = pb.cFormulaForme;
                    }

                    foreach (var ct in form1.Controls.OfType<Field>())    
                    {
                        if (ct.IME == KojePolje)
                        {
                                mvrednost = Convert.ToDouble(1)*100;
                             if (mvrednost !=0)                                
                             {
                               pb.textBox.Text = cv.Slovima(mvrednost, KojaValuta);
                        }
                             }                                        
                    }
                }

            }
            return mvrednost;
        }

    }
}


   
