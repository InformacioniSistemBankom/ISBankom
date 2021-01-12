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
    class clsdokumentRefresh
    {
        Form forma = Program.Parent.ActiveMdiChild;
        static DataBaseBroker db = new DataBaseBroker();
        DataTable tg = new DataTable();

        public void refreshDokument(Form form1,string KojiSegment, string iddok, string dokument, string TUD, string DokumentJe)
        {
            forma = form1;
            if (KojiSegment.Contains("Stavke") == true)
            {
                refreshDokumentGrid( forma,dokument, iddok, "", TUD, "");
            }
            else
            {
                if (DokumentJe != "I")
                {
                    refreshDokumentBody(forma, dokument, iddok, DokumentJe);
                }
            }
        }
        private static string CreateQuery(string KojiUpit, string KojiSegment, string iddok, string dokument, string DokumentJe) // vraca string
        {
            string UUslov = null;
            string PocetakUpita = null;
            string KrajUpita = null;
            string VN = null; // dokument ima totale
            string NazivKlona = null;

            DataTable tsd = db.ReturnDataTable("Select * from sifarnikdokumenta where naziv='" + dokument + "'");
            NazivKlona = tsd.Rows[0]["UlazniIzlazni"].ToString();
            VN = "NE";
            DataTable rtt = db.ReturnDataTable("Select * from RefreshGrida where ImeUpita='" + KojiSegment + "'");
            if (rtt.Rows.Count == 0) { return KojiUpit; }

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

            if (UUslov != null && UUslov.Trim() != "") // uslov nije prazan
            {
                if (KojiUpit.Contains("WHERE") == false) //  u upitu ne postoji rec WWHERE
                {
                    if (KojiUpit.Contains("ORDER BY") == true || KojiUpit.Contains("GROUP BY") == true)    // u upitu postoje reci ORDER BY ili GROUP BY
                    {
                        if (KojiUpit.IndexOf("ORDER BY") > KojiUpit.IndexOf("GROUP BY") && KojiUpit.IndexOf("GROUP BY") > 0)
                        {
                            KrajUpita = KojiUpit.Substring(KojiUpit.IndexOf("GROUP BY"));
                            PocetakUpita = KojiUpit.Substring(0, KojiUpit.IndexOf("GROUP BY") - 1);
                            KojiUpit = PocetakUpita + " WHERE " + UUslov + " " + KrajUpita;
                        }
                        else
                        {
                            if (KojiUpit.IndexOf("ORDER BY") == -1)
                            {
                                KrajUpita = KojiUpit.Substring(KojiUpit.IndexOf("GROUP BY"));
                                PocetakUpita = KojiUpit.Substring(0, KojiUpit.IndexOf("GROUP BY") - 1);
                                KojiUpit = PocetakUpita + " WHERE " + UUslov + " " + KrajUpita;
                            }
                            else
                            {
                                KrajUpita = KojiUpit.Substring(KojiUpit.IndexOf("ORDER BY"));
                                PocetakUpita = KojiUpit.Substring(0, KojiUpit.IndexOf("ORDER BY") - 1);
                                KojiUpit = PocetakUpita + " WHERE " + UUslov + " " + KrajUpita;
                            }
                        }
                    }
                    else // u upitu ne postoji ni ORDER ni GROUP
                        KojiUpit = KojiUpit + " WHERE " + UUslov;
                }
                else  // uslov sadrzi WHERE  
                    KojiUpit = KojiUpit + " WHERE " + UUslov;
            }
            else //  USLOV JE PRAZAN
            {
                KrajUpita = KojiUpit.Substring(KojiUpit.IndexOf("WHERE") + 5);
                PocetakUpita = KojiUpit.Substring(0, KojiUpit.IndexOf("WHERE") + 5);
                Console.WriteLine(PocetakUpita);
                Console.WriteLine(KrajUpita);
                KojiUpit = PocetakUpita + UUslov + KrajUpita;
                Console.WriteLine(KojiUpit);
            }
            Console.WriteLine(KojiUpit);
            return KojiUpit;
        }

        public void refreshDokumentGrid(Form forma,string dokument, string iddok, string mUpit, string TUD, string DokumentJe)
        {
            Console.WriteLine(mUpit);
           Form  form1 = forma;
            string tUpit = "";
            string tIme = "";
            string tud = "";
            string KojiSegment = "";
            string ss = "";
            int brredova = 0;
            DataTable tt = new DataTable();

            string sql = "Select Naziv,UlazniIzlazni as NazivDokumenta from SifarnikDokumenta  where naziv=@param0";
            tt = db.ParamsQueryDT(sql, dokument);
            if (tt.Rows.Count > 0) { dokument = tt.Rows[0]["NazivDokumenta"].ToString(); } // naziv iz sifarnika dokumenata
            if (TUD.Trim() == "")
            {
                ss = " SELECT TUD, MaxHeight, Levo, Vrh, Width, height, Upit, Ime "
                   + " FROM dbo.Upiti "
                   + " WHERE(NazivDokumenta = @param0 AND Ime Like'GgRr%') "
                   + " AND TUD>0  ORDER BY TUD";
            }
            else
            {
                ss = " SELECT TUD, MaxHeight, Levo, Vrh, Width, height, Upit, Ime "
                        + " FROM dbo.Upiti "
                        + " WHERE(NazivDokumenta = @param0 AND Ime Like'GgRr%') "
                        + " AND TUD =" + TUD;
            }
            Console.WriteLine(ss);

            DataTable t = db.ParamsQueryDT(ss, dokument);

            for (int i = 0; i < t.Rows.Count; i++)
            {
                tIme = t.Rows[i]["Ime"].ToString();
                KojiSegment = tIme.Substring(4, tIme.Length - 4);
                tud = t.Rows[i]["TUD"].ToString();
                tUpit = t.Rows[i]["Upit"].ToString();
                brredova = Convert.ToInt32(t.Rows[i]["MaxHeight"]);
                if (tud == "1")
                    ((Bankom.frmChield)forma).BrRedova = brredova;

                ((Bankom.frmChield)forma).imegrida = tIme;

                if (mUpit.Trim() == "")
                    tUpit = CreateQuery(tUpit, KojiSegment, iddok, dokument, DokumentJe);
                else
                {
                    tUpit = mUpit;
                }
                 ((Bankom.frmChield)forma).tUpit = tUpit; // sacuvana tabela za grid pocetni upit
                Console.WriteLine(tUpit);
                //statusStrip1 - podaci o paging-u
                if (DokumentJe == "D" || DokumentJe == "I")
                    ((Bankom.frmChield)forma).statusStrip1.Visible = false;// NEMA PAGING
                else
                    ((Bankom.frmChield)forma).statusStrip1.Visible = true;//IMA PAGING
                 //    ovde je bila greska 30.12.20. otklonile smo if
                if (/*!*/(((Bankom.frmChield)forma).Controls.Find(tIme, true).FirstOrDefault() is DataGridView dv))

        
                //    MessageBox.Show("Greska ne postoji grid");
                ///*else*/ // postoji grid                     
                {
                    if (((Bankom.frmChield)forma).statusStrip1.Visible == true)
                    {
                        Console.WriteLine(tUpit);
                        PripremiPaging(forma,ref tUpit, iddok);
                    }
                    tg = db.ReturnDataTable(tUpit);  // za datasource grida ako je prosao kroz PripremiPaging  tg samo slogove stranice inace sadrzi sve slogove iz upita

                    PripremiPodatkeZaGrid(forma, dv, dokument, tUpit, Convert.ToInt32(tud), iddok, tIme, DokumentJe);

                    dv.DataSource = tg;
                    if (dv.Columns.Count > 0)
                    {
                        setingWidthOfColumns(dokument, dv, tud.ToString());
                        if (dv.ColumnHeadersVisible == true)
                            AddColumnsText(dv, tud.ToString());
                        if (dokument == "LotStanjeRobeM")
                            colorofrows(dv);
                    }
                }
            }
        }
        private void PripremiPodatkeZaGrid(Form forma, DataGridView dv, string dokument, string tUpit, int tud, string iddok, string tIme, string DokumentJe)
        {
            dv.BackgroundColor = Color.Snow;
      
            string KojiSegment = tIme.Substring(4);
            clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();

            if (DokumentJe == "D" && tud == 1 && (tUpit.ToUpper()).Contains("ORDER") == false)
            {
                if (tUpit.ToUpper().Contains("IID") == true)
                    tUpit = tUpit + " ORDER BY iid DESC ";
            }
            Console.WriteLine(tUpit);

            if (((Bankom.frmChield)forma).intUkupno == 0)// broj slogova u upitu
            {
                ((Bankom.frmChield)forma).intUkupno = tg.Rows.Count; //// db.ReturnInt(" select count(*) from " + tUpit.Substring(intfrom + 5), 0); // koliko ima slogova u upitu
            }

            // DORADA TABELE tg postavljanjem rednog broja i osvezavanjem salda prema zahtevima dokumenta

            // AKO U RECNIKU POSTOJI POLJE SA ID_NaziviNaFormi=20 KOJE IMA SIRINU VECU OD 0 TO POLJE JE NAMENJENO DA SE U NJEGA PROGRAMSKI UPISE REDNI BROJ STAVKE.TO RADI KOD KOJI SLEDI
            os.DodajRedniBroj(ref tg, KojiSegment, DokumentJe);


            // AKO U RECNIKU POSTOJE POLJA SA Podacima u koloni formulaforme izvrsicemo azuriranje tih polja prema zadatoj formuli

            string sss = " Select FormulaForme ,AlijasPolja From RecnikPodataka where Dokument ='" + dokument + "' and  FormulaForme IS NOT NULL     and ltrim(rtrim(FormulaForme)) <>'' and tabelavview=@param0";
            DataTable dt = db.ParamsQueryDT(sss, KojiSegment);
            for (int ii = 0; ii < dt.Rows.Count; ii++)
            {
                if (tg.Rows.Count > 0)
                    os.OsveziSalda(ref tg, dt.Rows[ii]["AlijasPolja"].ToString(), dt.Rows[ii]["FormulaForme"].ToString());
            }
        }
        private void PripremiPaging(Form forma,ref string tUpit, string iddok)
        {
            // POCETAK KODA VEZAN ZA PSGEING
            int intfrom = tUpit.ToUpper().IndexOf("FROM", StringComparison.OrdinalIgnoreCase);
            int intOrder = tUpit.ToUpper().IndexOf("ORDER", StringComparison.OrdinalIgnoreCase);

        if (intOrder > -1)// UPIT SADRZI ORDER BY
           ((Bankom.frmChield)forma).intUkupno = db.ReturnInt(" select count(*) from " + tUpit.Substring(intfrom + 5, intOrder - intfrom - 5), 0);
        else
            ((Bankom.frmChield)forma).intUkupno = db.ReturnInt(" select count(*) from " + tUpit.Substring(intfrom + 5), 0);
       
        ((Bankom.frmChield)forma).toolStripTextBroj.Text = iddok;
        ((Bankom.frmChield)forma).statusStrip1.Visible = true;
        string strstart = "";
        int pageno = 0;
        if (((Bankom.frmChield)forma).intUkupno > 0)
        {
                if (((Bankom.frmChield)forma).BrRedova > 0)
                    pageno = ((Bankom.frmChield)forma).intUkupno / ((Bankom.frmChield)forma).BrRedova;
            }
            if (pageno * ((Bankom.frmChield)forma).BrRedova < ((Bankom.frmChield)forma).intUkupno) pageno = pageno + 1;
            string strFind = "";
            strFind = ((Bankom.frmChield)forma).toolStripTextFind.Text;
            ((Bankom.frmChield)forma).ToolStripLblPos.Text = Convert.ToString(pageno);
            ((Bankom.frmChield)forma).toolStripTexIme.Text = ((Bankom.frmChield)forma).Controls["limedok"].Text;
            if (((Bankom.frmChield)forma).intStart < 0) ((Bankom.frmChield)forma).intStart = 0;
            if (((Bankom.frmChield)forma).intUkupno > 0 && ((Bankom.frmChield)forma).intStart >= ((Bankom.frmChield)forma).intUkupno)
            {
                ((Bankom.frmChield)forma).intStart = ((Bankom.frmChield)forma).intUkupno - ((Bankom.frmChield)forma).BrRedova;
                strstart = Convert.ToString(((Bankom.frmChield)forma).intStart);
            }
            if (((Bankom.frmChield)forma).intUkupno > ((Bankom.frmChield)forma).BrRedova)
            {
                strstart = "";
                frmChield activeChild1 = (frmChield)forma.ActiveMdiChild;
                if (activeChild1 != null) // da li se nalazimo  na forma ako je odgovor DA nije null radi se o PAGING-u
                {
                    strFind = activeChild1.toolStripTextFind.Text.ToString();
                    strstart = activeChild1.ToolStripTextPos.Text.ToString();
                    ((Bankom.frmChield)activeChild1).ToolStripLblPos.Text = Convert.ToString(((Bankom.frmChield)activeChild1).intUkupno);
                    if (((Bankom.frmChield)activeChild1).intUkupno > 0 && ((Bankom.frmChield)activeChild1).intStart >= ((Bankom.frmChield)activeChild1).intUkupno)
                    {
                        ((Bankom.frmChield)activeChild1).intStart = ((Bankom.frmChield)activeChild1).intUkupno - ((Bankom.frmChield)forma).BrRedova;
                        strstart = Convert.ToString(((Bankom.frmChield)activeChild1).intStart);
                    }
                }
                else
                {
                    strFind = ((Bankom.frmChield)forma).toolStripTextFind.Text.ToString();
                    strstart = Convert.ToString(((Bankom.frmChield)forma).intStart);
                    ((Bankom.frmChield)forma).ToolStripLblPos.Text = Convert.ToString(pageno);
                    if (((Bankom.frmChield)forma).intUkupno > 0 && ((Bankom.frmChield)forma).intStart >= ((Bankom.frmChield)forma).intUkupno)
                    {
                        ((Bankom.frmChield)forma).intStart = ((Bankom.frmChield)forma).intUkupno - ((Bankom.frmChield)forma).BrRedova;
                        strstart = Convert.ToString(((Bankom.frmChield)forma).intStart);
                    }
                }
                switch (strFind)
                {
                    case "":
                    case null:
                        if (intOrder > -1) { tUpit += " OFFSET " + strstart + " ROWS FETCH NEXT " + ((Bankom.frmChield)forma).BrRedova + " ROWS ONLY;"; }
                        Console.WriteLine(tUpit);
                        break;
                    default:
                        if (intOrder > -1)
                        {
                            string[] separating = new[] { "<", ">" };
                            string[] words = strFind.Trim().Split(separating, System.StringSplitOptions.RemoveEmptyEntries);
                            strFind = "";
                            foreach (var word in words)
                            {
                                if (word.Trim() != "") { strFind += " and  " + word.Replace(":", " like '%") + "%'"; }
                            }
                            tUpit = tUpit.Replace("order", strFind + " order");
                            intfrom = tUpit.IndexOf("from", StringComparison.OrdinalIgnoreCase);
                            intOrder = tUpit.IndexOf("order", StringComparison.OrdinalIgnoreCase);
                            if (activeChild1 != null)
                            {
                                DataTable tu = db.ReturnDataTable(tUpit);
                                int intukupno = tu.Rows.Count;
                                activeChild1.intUkupno = intukupno;
                                if (((Bankom.frmChield)forma).BrRedova > 0)
                                {
                                    float broj = intukupno / ((Bankom.frmChield)forma).BrRedova;
                                    activeChild1.ToolStripLblPos.Text = Convert.ToString(intukupno / ((Bankom.frmChield)forma).BrRedova);
                                }
                            }
                            DataTable tu1 = db.ReturnDataTable(tUpit);
                            int intukupno1 = tu1.Rows.Count;
                            ((Bankom.frmChield)forma).ToolStripLblPos.Text = Convert.ToString(intukupno1 / ((Bankom.frmChield)forma).BrRedova);
                            tUpit += " OFFSET " + strstart + " ROWS FETCH NEXT " + ((Bankom.frmChield)forma).BrRedova + " ROWS ONLY;";
                        }
                        break;
                }
            }
        }

        public DataTable ShowPage(Form forma, int pageNumber, DataTable tg)
        {
            DataTable dt = new DataTable();

            int PageSize = ((Bankom.frmChield)forma).BrRedova;
            int startIndex = PageSize * (pageNumber - 1);
            int endindex = 0;

            foreach (DataColumn colunm in tg.Columns)
            {
                dt.Columns.Add(colunm.ColumnName);
            }

            if (((Bankom.frmChield)forma).statusStrip1.Visible == true)
                endindex = startIndex + PageSize;
            else
                endindex = ((Bankom.frmChield)forma).intUkupno;

            var result1 = tg.AsEnumerable().Where((s, k) => (k >= startIndex && k <= endindex));
            foreach (var item in result1)
            {
                dt.ImportRow(item);
            }
            return dt;
        }
        public void setingWidthOfColumns(string dokument, DataGridView dv, string tud)
        {
            dv.BackgroundColor = Color.Snow;
            //Djora 26.09.20
            //string sel = "SELECT  AlijasPolja, T.Tip,WidthKolone, width,T.Format,T.CSharp,T.Alajment  FROM dbo.RecnikPodataka AS R1,TipoviPodataka AS T ";
            //sel += " WHERE  R1.ID_TipoviPodataka=T.ID_TipoviPodataka  AND ";
            //sel += " TUD = @param0 AND Dokument = @param1  AND ";
            //sel += "(width>0 OR ID_NaziviNaFormi = 20 OR ID_NaziviNaFormi = 25 OR AlijasPolja LIKE 'IId%' OR ";
            //sel += "LTRIM(RTRIM(AlijasPolja)) = 'ID_' + LTRIM(RTRIM(AlijasTabele))   or (Izborno <> '' and alijaspolja like 'ID_'+ Izborno)) ORDER BY TabIndex ";

            string sel = "SELECT  AlijasPolja, T.Tip,cWidthKolone as WidthKolone, width,T.Format,T.CSharp,T.Alajment  FROM dbo.RecnikPodataka AS R1,TipoviPodataka AS T ";
            sel += " WHERE  R1.ID_TipoviPodataka=T.ID_TipoviPodataka  AND ";
            sel += " TUD = @param0 AND Dokument = @param1  AND ";
            sel += "(width>0 OR ID_NaziviNaFormi = 20 OR ID_NaziviNaFormi = 25 OR AlijasPolja LIKE 'IId%' OR ";
            sel += "LTRIM(RTRIM(AlijasPolja)) = 'ID_' + LTRIM(RTRIM(AlijasTabele))   or (Izborno <> '' and alijaspolja like 'ID_'+ Izborno)) ORDER BY TabIndex ";

            Console.WriteLine(sel);
            DataTable t2 = db.ParamsQueryDT(sel, tud, dokument);
            // zajedno 4.1.2021. bilo je     for (int i = 0; i < dv.ColumnCount; i++)
            for (int i = 0; i < t2.Rows.Count; i++)
            {
                //Djora 26.09.20
                //double ofset = Program.RacioWith * 1.3333333333333333;
                double ofset = Program.RacioWith;
                // Console.WriteLine(sel);
                //Console.WriteLine(t2.Rows[i]["Format"].ToString());
                int sirina = (int)Convert.ToDouble(Convert.ToDouble(t2.Rows[i]["WidthKolone"].ToString()) * ofset);
                if (sirina == 0)
                    dv.Columns[i].Visible = false;
                else
                    dv.Columns[i].Visible = true;

                dv.Columns[i].Width = sirina;



                dv.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                dv.BackgroundColor = Color.Snow;
                if (t2.Rows[i]["Format"].ToString() != "@" && t2.Rows[i]["Format"].ToString() != "0" && t2.Rows[i]["Format"].ToString().Trim() != "")
                    dv.Columns[i].DefaultCellStyle.Format = t2.Rows[i]["Format"].ToString();

                switch (t2.Rows[i]["Alajment"].ToString())
                {
                    case "0":
                        dv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dv.BackgroundColor = Color.Snow;
                        break;
                    case "1":
                        dv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dv.BackgroundColor = Color.Snow;
                        break;
                    case "2":
                        dv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dv.BackgroundColor = Color.Snow;
                        break;
                }
            }
            dv.Visible = true;
        }
        private void AddColumnsText(DataGridView dg, string tud)
        {
            string tabelavview = dg.Name.Substring(4);
            DataTable myt = (DataTable)dg.DataSource;
            dg.BackgroundColor = Color.Snow;

            // BORKKA PROMENILA ID_jezik poji inace kod promene jezika treba da se upise u Program.ID_Jezik
            if (Program.ID_Jezik == 0)
                Program.ID_Jezik = 3;
            string ss = "Select AlijasPolja as ap,p.Prevod as Naslov,WidthKolone as Width  From  RecnikPodataka as r,prevodi as p Where p.ID_Jezik=" + Program.ID_Jezik.ToString() + " And  r.ID_NaziviNaFormi=p.ID_NaziviNaFormi ";
            ss += "and r.TabelaVView = @param0 AND tud=@param1  ORDER BY TabIndex";
            Console.WriteLine(ss);
            DataTable t = new DataTable();
            t = db.ParamsQueryDT(ss, tabelavview, tud);
            //Console.WriteLine(t.Rows.Count);
            //Console.WriteLine(dg.Columns.Count);
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                if (i > t.Rows.Count - 1) { break; }

                if (dg.Columns[i].Name == t.Rows[i]["ap"].ToString())
                {
                    if (dg.Columns[i].Name == "ID_" + tabelavview)
                        dg.Columns[i].HeaderText = "Rb";
                    else
                        dg.Columns[i].HeaderText = t.Rows[i]["Naslov"].ToString();
                }
            }
        }
        public void colorofrows(DataGridView dg)
        {
            dg.BackgroundColor = Color.Snow;
            DataTable myt = (DataTable)dg.DataSource;
            for (int i = 0; i < dg.Rows.Count; i++)
            {
                Console.WriteLine(myt.Rows[i]["color"].ToString());
                switch (myt.Rows[i]["color"].ToString().Trim())
                {
                    case "1,0":
                        dg.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
                        break;
                    case "2,0":
                        dg.Rows[i].DefaultCellStyle.BackColor = Color.DimGray;
                        break;
                    case "3,0":
                        dg.Rows[i].DefaultCellStyle.BackColor = Color.Black;
                        break;
                }
            }
        }
        public void refreshDokumentBody( Form forma,string dokument, string iddok, string DokumentJe)
        {
            DataBaseBroker db = new DataBaseBroker();
            DataTable tt = new DataTable();
        
            string sql = "Select Naziv,UlazniIzlazni as NazivDokumenta from SifarnikDokumenta  where naziv=@param0";

            tt = db.ParamsQueryDT(sql, dokument);
            if (tt.Rows.Count > 0) { dokument = tt.Rows[0]["NazivDokumenta"].ToString(); }

            writeFrom(forma, dokument, iddok, DokumentJe);
            CalculatedField(forma, dokument, iddok);
        }
        private void writeFrom(Form forma, string dokument, string iddok, string DokumentJe)
        {
            string Uslov = "";
            string tUpit = "";
            string[] separators = new[] { "," };


            string selu = " SELECT Upit,ime  FROM dbo.Upiti  WHERE(NazivDokumenta = @param0" + ")";
            selu += " AND Ime LIKE N'ggrr%'  AND (TUD = 0)";
            DataTable tt = db.ParamsQueryDT(selu, dokument);

            for (int i = 0; i < tt.Rows.Count; i++)
            {
                string KojiSegment = tt.Rows[i]["ime"].ToString();
                KojiSegment = KojiSegment.Substring(4, KojiSegment.Length - 4);
                string mUpit = tt.Rows[i]["Upit"].ToString();

                if (DokumentJe == "I")
                {
                    Uslov = Program.WWhere;
                    clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                    string res = os.DodajRestrikcije(dokument, "0");

                    if (res.Trim() != "")
                        Uslov += " AND " + res;
                    mUpit += Uslov;
                    tUpit = mUpit;
                }
                else
                    tUpit = CreateQuery(mUpit, KojiSegment, iddok, dokument, DokumentJe);

                Console.WriteLine(tUpit);
                DataTable dt = db.ReturnDataTable(tUpit);
                if (dt.Rows.Count > 0)
                {
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        if (dt.Columns[k].ColumnName.ToUpper().ToString().Contains("ID_") == false)
                        {
                            Field ctrls = (Field)forma.Controls[dt.Columns[k].ColumnName];
                            //Field ctrls = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == dt.Columns[k].ColumnName.ToString());
                            if (ctrls != null)
                            {
                                ctrls.ID = "1";
                                ctrls.Vrednost = dt.Rows[0][dt.Columns[k].ColumnName].ToString();

                                 switch (ctrls.VrstaKontrole)
                                {
                                    case "tekst":
                                        if (ctrls.IME == "BrDok")
                                        {
                                            forma.Controls["lBrDok"].Text = ctrls.Vrednost;
                                        }
                                        string sadrzaj = dt.Rows[0][dt.Columns[k].ColumnName].ToString();
                                        clsFormInitialisation fi = new clsFormInitialisation();
                                        ctrls.textBox.Text = fi.FormatirajPolje(sadrzaj, ctrls.cTip);


                                        if (dokument == "IzlazniJCI")
                                        {
                                            Console.WriteLine(dt.Rows[0][dt.Columns[k].ColumnName].ToString());
                                            if (dt.Columns[k].ColumnName.ToString() == "DatumIstupa" && (string.IsNullOrEmpty(dt.Rows[0][dt.Columns[k].ColumnName].ToString())))
                                                break;
                                            else
                                            {
                                                if (dt.Columns[k].ColumnName.ToString() == "DatumIstupa")
                                                {
                                                    DateTime mdat = Convert.ToDateTime(dt.Rows[0][dt.Columns[k].ColumnName].ToString());
                                                    ctrls.textBox.Text = mdat.ToString("dd.MM.yy");
                                                }
                                                else
                                                    ctrls.textBox.Text = dt.Rows[0][dt.Columns[k].ColumnName].ToString();
                                            }
                                        }
                                        break;
                                    case "datum":
                                        ctrls.dtp.Text = string.Format("{0:dd.MM.yy}", dt.Rows[0][dt.Columns[k].ColumnName].ToString());
                                        if (ctrls.IME == "Datum")
                                        {
                                            forma.Controls["lDatum"].Text = ctrls.dtp.Text;
                                        }
                                        break;
                                    case "combo":
                                        //ivana 24.12.2020.
                                        if (ctrls.IME.Contains("NazivSkl"))
                                        {
                                            if(ctrls.IME.Length==8)
                                            Program.NazivSkladista = dt.Rows[0][dt.Columns[k].ColumnName].ToString();
                                            else if (ctrls.IME.Substring(8) == Program.nastavakSkladista1)
                                                Program.NazivSkladista1 = dt.Rows[0][dt.Columns[k].ColumnName].ToString();
                                            else
                                                Program.NazivSkladista2 = dt.Rows[0][dt.Columns[k].ColumnName].ToString();
                                        }
                                        ctrls.comboBox.Text = dt.Rows[0][dt.Columns[k].ColumnName].ToString();
                                        string kojiid;
                                        kojiid = "ID_" + ctrls.cAlijasTabele;
                                        for (int kk = 0; kk <= dt.Columns.Count - 1; kk++)
                                        {
                                            if (dt.Columns[kk].ColumnName.ToString() == kojiid)
                                            {
                                                string ccc = dt.Rows[0][dt.Columns[kk].ColumnName].ToString();
                                                ctrls.ID = dt.Rows[0][dt.Columns[kk].ColumnName].ToString();
                                                break;
                                            }
                                        }
                                        
                                        break;
                                    case "cek":
                                        if (dt.Rows[0][dt.Columns[k].ColumnName].ToString() == "1")
                                            ctrls.cekboks.Checked = true;
                                       else
                                           ctrls.cekboks.Checked = false;
                                       break;

                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var ctrls in forma.Controls.OfType<Field>().Where(g => String.Equals(g.cSegment, KojiSegment)))
                    {
                        ctrls.ID = "1";
                        switch (ctrls.VrstaKontrole)
                        {
                            case "tekst":
                                if (ctrls.IME == "BrDok")
                                {
                                    ctrls.Text = ((Bankom.frmChield)forma).brdok;
                                    ctrls.Vrednost = ctrls.Text;
                                    ctrls.textBox.Text = ctrls.Text;
                                }
                                break;
                            case "datum":
                                if (ctrls.IME == "Datum")
                                {
                                    ctrls.dtp.Text = ((Bankom.frmChield)forma).datum;
                                    ctrls.dtp.Value = Convert.ToDateTime(ctrls.dtp.Text);
                                }
                                break;
                            case "cek":

                                break;
                        }
                    }
                }
            }
        }
        public Double CalculatedField(Form forma, string dokument, string iddok)
        {
            CcalculatedVal cv = new CcalculatedVal();
            clsEvaluation cev = new clsEvaluation();
            DataBaseBroker db = new DataBaseBroker();

            string KojaValuta;
            KojaValuta = "RSD";
            string KojePolje;
            double mvrednost = 0;
            string formula = "";
            Dictionary<string, string> Broj = new Dictionary<string, string>();

            foreach (var pb in forma.Controls.OfType<Field>())
            {
                if (pb.cPolje.Contains("IzvodiSe") == true)
                {
                    if (pb.cFormulaForme.Trim() == "")
                    {
                        pb.Vrednost = "0";
                    }
                    else
                    {
                        formula = pb.cFormulaForme.Trim();
                        string aaa = cv.CalculateValue(forma, formula); //textBox.Text = Convert.ToString(float.Parse(dt.Rows[j][dt.Columns[k].ColumnName].ToString()).ToString("###,##0.00"));
                        string rez = cev.Evaluate(aaa);
                        pb.Vrednost = Convert.ToString(rez);
                        if (pb.Vrednost.Trim() != "")
                        {
                            //pb.textBox.Text = Convert.ToString(float.Parse(rez.ToString()).ToString("###,##0.00"));
                            //Jovana 05.01.21
                            clsFormInitialisation fi = new clsFormInitialisation();
                            pb.textBox.Text = fi.FormatirajPolje(rez.ToString(), pb.cTip);

                        }
                    }
                }
            }

            foreach (var ctrls in forma.Controls.OfType<Field>())
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

            foreach (var pb in forma.Controls.OfType<Field>())
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
                    foreach (var ct in forma.Controls.OfType<Field>())
                    {
                        if (ct.IME == KojePolje)
                        {
                            if (ct.Vrednost.Trim() != "")
                            {
                                mvrednost = Convert.ToDouble(ct.Vrednost)*100;
                                // jovana 05.01.21
                                mvrednost = Convert.ToInt64(mvrednost);
                                if (mvrednost != 0)
                                {
                                    Console.WriteLine(mvrednost);
                                     pb.textBox.Text = cv.Slovima(mvrednost, KojaValuta);
                                    // jovana 05.01.21
                                    //Broj =  db.ExecuteStoreProcedure("BrojSlovima", "KojiBroj:" + mvrednost, "KojaValuta:"+ KojaValuta, "Slovima:") ;
                                    //pb.textBox.Text = Broj["@Slovima"].Trim();
                                }
                            }
                        }
                    }
                }
            }
            return mvrednost;
            //neki komentar 01.12.2020.
        }
    }
}
