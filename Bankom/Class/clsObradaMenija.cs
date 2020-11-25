using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bankom.Class
{
    class clsObradaMenija
    {

        DataTable dt = new DataTable();

        public BankomMDI mdi;
       
        public clsObradaMenija (BankomMDI mdi)
        {
            this.mdi = mdi;
          
        }
       

        public void CreateMenu()
        {
            //stil 21.10.2020.

      
         mdi.menuStrip1.BackColor = System.Drawing.Color.Snow;
           mdi.menuStrip1.Font = new System.Drawing.Font("TimesRoman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            mdi.menuStrip1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));

            dt = GetDataSet();

            foreach (DataRow r in dt.Select("MenuParent='1'"))
            {
                CreateMenuItem(r[0].ToString());
            }
        }

        private void CreateMenuItem(string strMenu)
        {
            ToolStripMenuItem t = new ToolStripMenuItem(GetMenuName(strMenu));

            //stil 21.10.2020.
            t.TextAlign = ContentAlignment.MiddleLeft;
            t.AutoSize = false;
            t.Width = 150;
            t.Height = 50;
            t.BackColor = Color.Snow;
            t.Font = new System.Drawing.Font("TimesRoman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            t.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            //
            mdi.menuStrip1.Items.Add(t);
            if (dt.Select("MenuParent='" + strMenu + "'").Length > 0)
            {
                foreach (DataRow r in dt.Select("MenuParent='" + strMenu + "'"))
                {
                    CreateMenuItems(t, r[0].ToString());
                }
            }
            else
            {
                t.Click += new EventHandler(MenuItemClickHandler);
            }
        }

        private string CreateMenuItems(ToolStripMenuItem t, string strMenu)
        {
            if (dt.Select("MenuParent='" + strMenu + "'").Length > 0)
            {
                ToolStripMenuItem t1 = new ToolStripMenuItem(GetMenuName(strMenu));
                //stil 21.10.2020.
                t1.TextAlign = ContentAlignment.TopLeft;
                t1.Height = 70;
                t1.BackColor = Color.Snow;
                t1.Font = new System.Drawing.Font("TimesRoman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                t1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
                //
                t.DropDownItems.Add(t1);
                foreach (DataRow r in dt.Select("MenuParent='" + strMenu + "'"))
                {
                    CreateMenuItems(t1, r[0].ToString());
                }
            }
            else
            {
                ToolStripMenuItem t1 = new ToolStripMenuItem(GetMenuName(strMenu));
                //stil 21.10.2020.
                t1.TextAlign = ContentAlignment.TopLeft;
                t1.Height = 70;
                t1.BackColor = Color.Snow;
                t1.Font = new System.Drawing.Font("TimesRoman", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                t1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
                // Dodat separator 27.10.2020. Ivana
                ToolStripSeparator s1 = new ToolStripSeparator();
                if (strMenu == "91")
                {
                    t.DropDownItems.Add(s1);
                }
                else
                {
                    t1.Click += new EventHandler(MenuItemClickHandler);
                    t.DropDownItems.Add(t1);
                }
                return strMenu;
            }
            return strMenu;
        }

        private string GetMenuName(string strMenuID)
        {
            return dt.Select("MenuID='" + strMenuID + "'")[0][1].ToString();
        }

        string idke = Program.idkadar.ToString();
        string idfirme = Program.idFirme.ToString();
        DataBaseBroker db = new DataBaseBroker();
        private DataTable GetDataSet()
        {
            //DataTable dt = new DataTable("Menu");
            //ovde se koristi MenuStablo tabela i ona ne odgovara php-u vise
            String SQL = ";  WITH RekurzivnoStablo (ID_MeniStablo,Naziv, NazivJavni,Brdok,Vezan,RedniBroj,ccopy, Level,slave,pd,pp) AS " +
                         "  (SELECT e.ID_MeniStablo,e.Naziv,e.NazivJavni,e.Brdok, e.Vezan,e.RedniBroj,e.ccopy,0 AS Level, CASE e.vrstacvora WHEN 'f' THEN 0 ELSE 1 END as slave,  PrikazDetaljaDaNe as pd,PrikazPo as pp " +
                         " FROM MeniStablo AS e WITH(NOLOCK)  where Naziv in (select g.naziv from Grupa as g, KadroviIOrganizacionaStrukturaStavkeView as ko Where(KO.ID_OrganizacionaStruktura = G.ID_OrganizacionaStruktura " +
                         "  Or KO.id_kadrovskaevidencija = G.id_kadrovskaevidencija)  And KO.ID_OrganizacionaStrukturaStablo = " + idfirme + " and ko.id_kadrovskaevidencija=" + idke + ")UNION ALL " +
                         " SELECT e.ID_MeniStablo,e.Naziv,e.NazivJavni,e.BrDok,e.Vezan,e.RedniBroj, e.ccopy,Level + 1 ,  CASE e.vrstacvora WHEN 'f' THEN 0 ELSE 1 END as slave,  PrikazDetaljaDaNe As pd, PrikazPo As pp " +
                         "   FROM MeniStablo AS e WITH(NOLOCK)  INNER JOIN RekurzivnoStablo AS d ON e.ID_MeniStablo = d.Vezan) " +
                         "   SELECT distinct ID_MeniStablo as MenuID, NazivJavni AS  MenuName,Vezan AS MenuParent,'Y' AS MenuEnable,'DEMO' AS USERID,Naziv, RedniBroj FROM RekurzivnoStablo WITH(NOLOCK) where ccopy= 0 and vezan<> 0  order by RedniBroj ";
            dt = db.ReturnDataTable(SQL);
            return dt;
        }

        //Ivana  23.10.2020.

        public string GetMenuNaziv(string strMenuID)
        {
            return dt.Select("MenuName='" + strMenuID + "'")[0][5].ToString();
        }

        private char UzmiSlovo(string s)
        {
            char slovo;
            if (s == "Dokumenta" || s == "Komitenti" || s == "Artikli" || s == "OrganizacionaStruktura")
                slovo = 'S';
            else if (s == "Izvestaj")
                slovo = 'I';
            else if (s == "KlasifikacijaOrgStrukture" || s == "KlasifikacijaArtikla" || s == "KlasifikacijaKomitenata" || s == "KlasifikacijaDokumenata"
                || s == "KlasifikacijaIzvestaja" || s == "KlasifikacijaMenija" || s == "KlasifikacijaPomocnihSifarnika")
                slovo = 'K';
            else
                slovo = 'P';
            return slovo;
        }
      

        //28.10.2020. Ivana
        public string SkiniKlasifikaciju(string s)
        {
            if (s == "KlasifikacijaOrgStrukture")
            {
                Program.pomStablo = "OrganizacionaStrukturaStablo";
                return "OrganizacionaStruktura";
            }
            else if (s == "KlasifikacijaDokumenata")
            {
                Program.pomStablo = "DokumentaStablo";
                return "Dokumenta";
            }
            else if (s == "KlasifikacijaArtikla")
            {
                Program.pomStablo = "ArtikliStablo";
                return "Artikli";
            }
            else if (s == "KlasifikacijaKomitenata")
            {
                Program.pomStablo = "KomitentiStablo";
                return "Komitenti";
            }
            else if (s == "KlasifikacijaIzvestaja")
            {
                Program.pomStablo = "IzvestajStablo";
                Program.pomIzv = "Izvestaj";
                return "Izvestaj";
            }
            else if (s == "KlasifikacijaMenija")
            {
                Program.pomStablo = "MeniStablo";
                return "Meni";
            }
            else
            {
                Program.pomStablo = "PomocniSifarniciStablo";
                return "PomocniSifarnici";
            }
        }

        private bool IsOpen(string s)
        {
            bool pom = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Text == s)
                {
                    pom = true;
                    MessageBox.Show("Već je otvorena ova forma!");
                    f.Focus();
                   // mdi.updateToolStrip(s);
                    break;
                }
            }
            if (pom == false)
            {
                return pom;
            }
            return pom;
        }

     

        public void MenuItemClickHandler(object sender, EventArgs e)
        {
            string s = GetMenuNaziv(((ToolStripMenuItem)sender).Text);
            char slovo = UzmiSlovo(s);
            bool postoji;
            Program.KlasifikacijaSlovo = "";
            switch (s)
            {
                case "Lotovi":
                    if (!IsOpen(s))
                    {
                        Lotovi lotovi = new Lotovi();
                        lotovi.FormBorderStyle = FormBorderStyle.None;
                        lotovi.Text = "Lotovi";
                        lotovi.MdiParent = mdi;
                        lotovi.Dock = DockStyle.Fill;
                        
                        // mdi.WindowState = FormWindowState.Maximized;
                        if (mdi.IzborJezika.Text == "Српски-Ћирилица") { lotovi.Text = mdi.VratiCirlilicu("Lotovi"); }
                        int sirina = (mdi.Width / 100) * 10;
                        mdi.addFormTotoolstrip1(lotovi, "Lotovi");
                      //  mdi.updateToolStrip(s);
                        mdi.SrediFormu();
                        lotovi.Show();
                        

                    }
                    break;
                case "Dokumenta":
                case "Izvestaj":
                case "OsnovniSifarnici":
                case "PomocniSifarnici":
                case "Artikli":
                case "Komitenti":
                case "OrganizacionaStruktura":
                    Program.Parent.ToolBar.Items["Uunos"].Visible = true;
                    Program.Parent.ToolBar.Items["Uunos"].Enabled = true;
                  
                    postoji = IsOpen(s);
                    if (postoji == false)
                    {
                       
                        clsObradaOsnovnihSifarnika co0 = new clsObradaOsnovnihSifarnika();
                        mdi.ShowNewForm(s, 1, s, 1, "", "", slovo.ToString(), "", "TreeView");
                    }

                  

                    break;

                //28.10.2020. Ivana
                case "KlasifikacijaOrgStrukture":
                case "KlasifikacijaArtikla":
                case "KlasifikacijaKomitenata":
                case "KlasifikacijaDokumenata":
                case "KlasifikacijaIzvestaja":
                case "KlasifikacijaMenija":
                case "KlasifikacijaPomocnihSifarnika":
                    Program.Parent.ToolBar.Items["Uunos"].Visible = true;
                    Program.Parent.ToolBar.Items["Uunos"].Enabled = true;
                    Program.KlasifikacijaSlovo = "K";
                    postoji = IsOpen(s);
                    if (postoji == false)
                    {
                        clsObradaOsnovnihSifarnika co0 = new clsObradaOsnovnihSifarnika();
                        mdi.ShowNewForm(SkiniKlasifikaciju(s), 1, SkiniKlasifikaciju(s), 1, "", "", slovo.ToString(), "", "TreeView");
                    }
                  
                    break;
                case "KadroviIOrganizacionaStruktura":                                                   //"DodeljivanjeUlogeKorisniku":
                    mdi.ShowNewForm("", 1, "KadroviIOrganizacionaStruktura", 1, "", "", "P", "", "");
                  
                    break;
                case "Dozvole":
                    mdi.ShowNewForm("", 1, "Dozvole", 1, "", "", "P", "", "");
                  
                    break;
                case "PreuzimanjeKursneListe":
                    KursnaLista kl = new KursnaLista();
                    //Djora 15.09.20
                    kl.MdiParent = mdi;
                    kl.Show();
                    //Djora 15.09.20
                    kl.WindowState = FormWindowState.Maximized;
                    kl.FormBorderStyle = FormBorderStyle.None;
                    mdi.addFormTotoolstrip1(kl, "Preuzimanje Kursne Liste");

                  
                    break;
                case "Prenosi":
                    Form activeChild = mdi.ActiveMdiChild;
                    activeChild.FormBorderStyle = FormBorderStyle.None;
                    if (activeChild != null)
                    {
                        activeChild.Hide();
                    }
                  
                    break;
                case "PlacanjeRateKredita":                                           //"PreuzimanjeRateKredita"
                    Preuzimanja.PreuzimanjeRateKredita();
                  
                    break;
                case "PreuzimanjeManjkovaIViskova":
                    Preuzimanja.PreuzimanjeManjkovaIViskova();
                  
                    break;
                case "PreuzimanjeUplata":                                             //"PreuzimanjeManjkovaIViskova"
                    Preuzimanja.PreuzimanjeUplataKupacaIzBanaka();
                  
                    break;
                case "PrenosNalogaZaPlacanje":
                    //clsPreuzimanja cp = new clsPreuzimanja();//BORKA
                    DateTime DatOd = DateTime.Parse(DateTime.Now.ToString("dd.MM.yy"));///, "dd/MM/yyyy", null);// , CultureInfo.InvariantCulture);
                    string TekuciRacun = "";
                    TekuciRacun = Prompt.ShowDialog("", "Unesite tekuci racun za koji prenosimo naloge ", "Prepisivanje naloga iz PripremeZaPlacanje");
                    if (TekuciRacun == "") { return; }

                    //string vrati = cp.PrepisiNaloge(DatOd.ToShortDateString(), TekuciRacun); //BORKA
                    MessageBox.Show("Zavrseno!!");
                  
                    break;
                case "PreuzimanjeIzvodaIzBanaka":
                    clsPreuzimanja cp = new clsPreuzimanja();
                    string strPreuzimanjePlacanja = cp.preuzimanjeIzvodaizBanaka();
                    if (strPreuzimanjePlacanja == "") { return; }
                    char[] separators = { '#' };
                    frmIzvod childForm = new frmIzvod();
                    childForm.FormBorderStyle = FormBorderStyle.None;
                    childForm.MdiParent = mdi;
                    childForm.strPutanjaPlacanja = strPreuzimanjePlacanja.Split(separators)[0];
                    childForm.mesecgodina = strPreuzimanjePlacanja.Split(separators)[1];
                    childForm.IdDokView = Convert.ToInt32(strPreuzimanjePlacanja.Split(separators)[2]);
                    childForm.KojiPrepis = strPreuzimanjePlacanja.Split(separators)[3];
                    childForm.Show();
                  
                    break;
                case "PrepisPlacanjaIUplataUIzvod":    /// stari je bio ovaj naziv -> "PrepisNaplataIPlacanjaUIzvod" Ivana
                    clsOperacije co = new clsOperacije();
                    clsPreuzimanja cp1 = new clsPreuzimanja();
                    DataBaseBroker db = new DataBaseBroker();
                    string DatOd1 = "";
                    string TekuciRacun1 = "";
                    DataTable rsp = new DataTable();
                    DatOd1 = Prompt.ShowDialog("", "Unesite datum za koji prepisujemo izvod ", "Prepisivanje izvoda iz Placanja i naplata");
                    if (DatOd1 == "") { return; }

                    if (co.IsDateTime("1") == false)
                    { MessageBox.Show("pogresno unesen datum ponovite !!"); return; };

                    TekuciRacun = Prompt.ShowDialog("", "Unesite tekuci racun za koji prepisujete promet ", "Prepisivanje izvoda iz Placanja i naplata");
                    if (TekuciRacun1 == "") { return; }
                    rsp = db.ReturnDataTable("if not exists (select datum from  IzvodTotali  where Datum='" + DatOd1 + "' And Blagajna='" + TekuciRacun.ToString() + "') select 0 else select 1 ");
                    if (rsp.Rows.Count == 0) { MessageBox.Show("Ne postoje podaci za datum i tekuci racun !!"); return; }

                    if (Convert.ToInt16(rsp.Rows[0][0]) == 1)
                    { MessageBox.Show("Vec je izvrsen prepis izvoda za datum " + DatOd1); }
                    else
                    {
                        cp1.izborPReuzimanja(1, DatOd1 + "#" + TekuciRacun);
                    }
                    MessageBox.Show("Zavrseno!!");

                  
                    break;
                case "FormiranjePPPPDzaPlate":
                    DateTime d = DateTime.Now;
                    string pDatum = d.ToString("dd.MM.yy");
                    string mg = Prompt.ShowDialog(pDatum.Substring(3, 2) + pDatum.Substring(6, 2), "Formiranje PPPPD za plate", "Unesite mesec i godinu za koji formiramo " + Environment.NewLine + " PPPPD za plate ");
                    if (string.IsNullOrEmpty(mg)) { return; }
                    clsOperacije co2 = new clsOperacije();
                    bool r = co2.IsNumeric(mg);
                    if (r == false) { MessageBox.Show("Nekorektan unos"); return; }
                    if (mg.Length != 4) { MessageBox.Show("Nekorektan unos"); return; }
                    string svrsta = Prompt.ShowDialog("", "Formiranje PPPPD za plate", "Unesite vrstu obracuna: A za akontaciju ili K za platu ");
                    svrsta = svrsta.ToUpper();
                    if (svrsta != "a".ToUpper() && svrsta != "K".ToUpper())
                    {
                        MessageBox.Show("Pogresno unesena vrsta obracuna moze samo A ili K PONOVITE!!!!!");
                        return;
                    }
                    clsXmlPlacanja cxml = new clsXmlPlacanja();
                    cxml.izborPlacanja(2, mg + svrsta);
                    frmPrint fs = new frmPrint();
                    fs.FormBorderStyle = FormBorderStyle.None;

                    fs.MdiParent = mdi;
                    fs.Text = "plate-" + mg;
                    fs.LayoutMdi(MdiLayout.TileVertical);
                    fs.imefajla = "plate" + mg;
                    fs.kojiprint = "pla";
                    fs.Show();

                    mdi.toolStrip1.Visible = true;

                    ToolStripLabel itemn = new ToolStripLabel();
                    ToolStripButton itemB = new ToolStripButton();
                    ToolStripSeparator itemnsep = new ToolStripSeparator();
                    itemn.Text = "plate-" + mg;
                    itemn.Name = "plate-" + mg;
                    itemB.Image = global::Bankom.Properties.Resources.del12;
                    itemnsep.Name = "plate-" + mg;
                    itemn.Click += new EventHandler(mdi.itemn_click);

                    itemB.Click += new EventHandler(mdi.itemB_click);
                    itemB.Name = "plate-" + mg;

                    mdi.toolStrip1.Items.Add(itemn);
                    mdi.toolStrip1.Items.Add(itemB);
                    mdi.toolStrip1.Items.Add(itemnsep);
                    mdi.LayoutMdi(MdiLayout.TileVertical);
                  
                    break;
                case "PreuzimanjePlata":                               //"UvozPlataUPlacanje"
                    clsXmlPlacanja cls = new clsXmlPlacanja();
                    cls.izborPlacanja(3, "");

                  
                    break;
                case "Prevoz":
                    DateTime d1 = DateTime.Now;
                    string pDatum1 = d1.ToString("dd.MM.yy");

                    string mg1 = Prompt.ShowDialog(pDatum1.Substring(3, 2) + pDatum1.Substring(6, 2), "Formiranje naloga za knjiženje prevoza : ", "Unesite mesec i godinu za koji isplaćujemo prevoz");
                    if (string.IsNullOrEmpty(mg1)) { return; }
                    clsOperacije co1 = new clsOperacije();
                    bool r1 = co1.IsNumeric(mg1);
                    if (r1 == false) { MessageBox.Show("Nekorektan unos"); return; }
                    if (mg1.Length != 4) { MessageBox.Show("Nekorektan unos"); return; }



                    clsXmlPlacanja cxml1 = new clsXmlPlacanja();
                    cxml1.izborPlacanja(0, mg1);
                    frmPrint fs1 = new frmPrint();

                    fs1.MdiParent = mdi;
                    fs1.Text = "prevoz-" + mg1;
                    fs1.LayoutMdi(MdiLayout.TileVertical);
                    fs1.imefajla = "prevoz" + mg1;
                    fs1.Show();

                    mdi.toolStrip1.Visible = true;

                    ToolStripLabel itemn1 = new ToolStripLabel();
                    ToolStripButton itemB1 = new ToolStripButton();
                    ToolStripSeparator itemnsep1 = new ToolStripSeparator();
                    itemn1.Text = "prevoz-" + mg1;
                    itemn1.Name = "prevoz-" + mg1;
                    itemB1.Image = global::Bankom.Properties.Resources.del12;
                    itemnsep1.Name = "prevoz-" + mg1;
                    itemn1.Click += new EventHandler(mdi.itemn_click);

                    itemB1.Click += new EventHandler(mdi.itemB_click);
                    itemB1.Name = "prevoz-" + mg1;

                    mdi.toolStrip1.Items.Add(itemn1);
                    mdi.toolStrip1.Items.Add(itemB1);
                    mdi.toolStrip1.Items.Add(itemnsep1);
                    mdi.LayoutMdi(MdiLayout.TileVertical);

                  
                    break;
                case "Nagrade":
                    DateTime d2 = DateTime.Now;
                    string pDatum2 = d2.ToString("dd.MM.yy");

                    string mg2 = Prompt.ShowDialog(pDatum2.Substring(3, 2) + pDatum2.Substring(6, 2), "Formiranje naloga za knjiženje nagrada :  ", "Unesite mesec i godinu za koji isplaćujemo nagrade");
                    if (string.IsNullOrEmpty(mg2)) { return; }
                    clsOperacije co3 = new clsOperacije();
                    bool r2 = co3.IsNumeric(mg2);
                    if (r2 == false) { MessageBox.Show("Nekorektan unos"); return; }
                    if (mg2.Length != 4) { MessageBox.Show("Nekorektan unos"); return; }



                    clsXmlPlacanja cxml2 = new clsXmlPlacanja();
                    cxml2.izborPlacanja(1, mg2);
                    frmPrint fs2 = new frmPrint();
                    fs2.kojiprint = "nag";
                    fs2.MdiParent = mdi;
                    fs2.Text = "nagrade-" + mg2;
                    fs2.LayoutMdi(MdiLayout.TileVertical);
                    fs2.imefajla = "nagrade" + mg2;
                    fs2.Show();

                    mdi.toolStrip1.Visible = true;

                    ToolStripLabel itemn2 = new ToolStripLabel();
                    ToolStripButton itemB2 = new ToolStripButton();
                    ToolStripSeparator itemnsep2 = new ToolStripSeparator();
                    itemn2.Text = "nagrade-" + mg2;
                    itemn2.Name = "nagrade-" + mg2;
                    itemB2.Image = global::Bankom.Properties.Resources.del12;
                    itemnsep2.Name = "nagrade-" + mg2;
                    itemn2.Click += new EventHandler(mdi.itemn_click);

                    itemB2.Click += new EventHandler(mdi.itemB_click);
                    itemB2.Name = "nagrade-" + mg2;

                    mdi.toolStrip1.Items.Add(itemn2);
                    mdi.toolStrip1.Items.Add(itemB2);
                    mdi.toolStrip1.Items.Add(itemnsep2);
                    mdi.LayoutMdi(MdiLayout.TileVertical);

                  
                    break;
                case "UvozPrevozaUPlacanje":
                    clsXmlPlacanja cls2 = new clsXmlPlacanja();
                    cls2.izborPlacanja(4, "");
                  
                    break;
                case "PrenosiZaProdajnaMesta":
                    Prenosi childForm1 = new Prenosi();

                    childForm1.MdiParent = mdi;

                    // childForm.WindowState = FormWindowState.Maximized;
                    childForm1.Show();
                   // 
                    break;
                case "FaktureRecepcijeZaOdabraneDatume":
                    Preuzimanja.FaktureRecepcijeZaOdabraneDatume();
                   // 
                    break;
                case "FaktureRestoranaZaOdabraneDatume":
                    Preuzimanja.FaktureRestoranaZaOdabraneDatume();
                  
                    break;
                case "Razduzenjesirovinaminibar":
                    Preuzimanja.RazduzenjeSirovinaMiniBar();
                   // 
                    break;
                case "Razduzenjesirovinazaodabraniintervaldatuma":
                    Preuzimanja.RazduzenjeSirovinaZaOdabraniIntervalDatuma();
                  
                    break;
                case "KursnaListaZaCeluGodinu":
                    string GodinaKursa = "";
                    string PocetniDatumKursa = "";
                    int KojiIDDokstablo = 1;
                    string sql = "";
                    long granica = 0;
                    int ret = 1;
                    string ID_DokumentaView = "1";
                    DateTime DatumKursa;
                    DataBaseBroker db2 = new DataBaseBroker();

                    if (MessageBox.Show("Upisujemo kursnu listu za " + (System.DateTime.Now).Year.ToString(), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        GodinaKursa = Prompt.ShowDialog("", "Unesite Godinu za kursnu listu ", "Kursna lista");
                        PocetniDatumKursa = "01.01." + GodinaKursa.Trim();
                        sql = "select ID_DokumentaTotali from  DokumentaTotali where dokument ='KursnaLista' and Datum>@param0";
                        DataTable t = db2.ParamsQueryDT(sql, PocetniDatumKursa);

                        if (t.Rows.Count == 0)
                        {
                            sql = "select ID_DokumentaStablo from DokumentaStablo where Naziv='KursnaLista'";
                            DataTable dt = db2.ParamsQueryDT(sql);
                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show("Nije definisana kursna lista !!!");
                            }
                            else
                            {
                                KojiIDDokstablo = Convert.ToInt32(dt.Rows[0]["ID_DokumentaStablo"]);

                                clsOperacije cOp = new clsOperacije();
                                if (cOp.Prestupna(Convert.ToInt32(GodinaKursa)) == true)
                                    granica = 366;
                                else
                                    granica = 365;

                                int i = 1;

                                for (; i <= granica; i++)
                                {
                                    DatumKursa = Convert.ToDateTime(PocetniDatumKursa).AddDays(i);

                                    clsObradaOsnovnihSifarnika cls3 = new clsObradaOsnovnihSifarnika();
                                    string ParRb = "";

                                    ret = cls3.UpisiDokument(ref ParRb, "Kursna lista " + DatumKursa.Date, KojiIDDokstablo, DatumKursa.ToString());

                                    if (ret == -1)
                                    {
                                        MessageBox.Show("Greska prilikom inserta!");
                                        return;
                                    }
                                    ID_DokumentaView = ret.ToString();

                                    //stavka za domacu valutu
                                    sql = " Insert into KursnaLista(ID_SifrarnikValuta,ID_Zemlja,ID_DokumentaView,datum,paritet,"
                                        + " Kupovni,Srednji,Prodajni,Dogovorni,verzija,KupovniZaDevize,ProdajniZaDevize,OznVal,UUser,TTIME )"
                                        + " Values(@param0,@param1,@param2,@param3,@param4, "
                                        + " @param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14)";

                                    DataTable dkl = db2.ParamsQueryDT(sql, 1, Program.ID_MojaZemlja, ID_DokumentaView, DatumKursa.ToString(), 001,
                                        1, 1, 1, 1, "", 1, 1, Program.DomacaValuta, Program.idkadar.ToString(), (System.DateTime.Now).ToString());

                                    // Druga stavka za eur ako je zemlja bosna

                                    if (Program.ID_MojaZemlja == 38)
                                    {
                                        sql = " Insert into KursnaLista(ID_SifrarnikValuta,ID_Zemlja,ID_DokumentaView,datum,paritet,"
                                            + " Kupovni,Srednji,Prodajni,Dogovorni,verzija,KupovniZaDevize,ProdajniZaDevize,OznVal,UUser,TTIME )"
                                            + " Values(@param0,@param1,@param2,@param3,@param4, "
                                            + " @param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14)";

                                        DataTable dkb = db2.ParamsQueryDT(sql, 1, Program.ID_MojaZemlja, ID_DokumentaView, DatumKursa.ToString(), 001,
                                            1.95583, 1.95583, 1.95583, 1.95583, "", 1.95583, 1.95583, "EUR", Program.idkadar.ToString(), (System.DateTime.Now).ToString());
                                    }
                                    db2.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + ID_DokumentaView);
                                    db2.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:KursnaLista", "IdDokument:" + ID_DokumentaView);
                                }
                                MessageBox.Show("Zavrseno !!!");
                            }
                        }
                        else MessageBox.Show("Vec je unesena kursna lista za datume tekuce godine !!!");


                    }
                  
                    break;
                case "PopunjavanjeTabeleDatuma":
                    string GodinaDatuma = "";
                    string sql3 = "";
                    DataBaseBroker db3 = new DataBaseBroker();

                    if (MessageBox.Show("Upisujemo tabelu datuma za " + (System.DateTime.Now).Year.ToString(), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        GodinaDatuma = Prompt.ShowDialog("", "Unesite Godinu za tabelu datuma ", "Tabela datuma");
                        if (GodinaDatuma == "") { MessageBox.Show("Niste uneli godinu ponovite !!!"); return; }

                        if (GodinaDatuma != (System.DateTime.Now).Year.ToString() && GodinaDatuma != (System.DateTime.Now).AddYears(+1).Year.ToString())
                        {
                            MessageBox.Show("Pogresno unesena godina ponovite");
                            return;
                        }
                        else
                        {
                            sql3 = "select time_id from  Time_by_Day where the_year =@param0 ";
                            DataTable t = db3.ParamsQueryDT(sql3, GodinaDatuma);
                            if (t.Rows.Count == 0)
                            {
                                db3.ExecuteStoreProcedure("PopuniTimeByDay", "Godina:" + GodinaDatuma);
                                MessageBox.Show("Zavrseno !!!");
                            }
                            else MessageBox.Show("Vec je unesena godina!!!");
                        }
                    }
                  
                    break;
                case "ProcesiranjeDnevnogiIzvestaja":

                  
                    break;
                case "ProcesiranjeBrutoBilansa":

                  
                    break;
                case "SpisakDokumenata":
                    mdi.ShowNewForm(" ", 1, "SpisakDokumenata", 1, "", "", "I", "", ""); //SpisakDokumenata
                   // 
                    break;
                case "ZatvaranjeStanjaPoLotu":
                    clsZatvaranjeIOtvaranjeStanja c = new clsZatvaranjeIOtvaranjeStanja();
                    bool pom = c.ObradiZahtev("DA");
                    if (pom)
                        MessageBox.Show("Uspešno završeno!");
                    else
                        MessageBox.Show("Nije uspelo zatvaranje stanja po lot-u!");
                  
                    break;
                case "PocetakGodine":
                    clsZatvaranjeIOtvaranjeStanja c1 = new clsZatvaranjeIOtvaranjeStanja();
                    bool pom1 = c1.ObradiZahtev("NE");
                    if (pom1)
                        MessageBox.Show("Uspešno završeno!");
                    else
                        MessageBox.Show("Neuspešno!");
                  
                    break;
                case "UsaglasavanjeRobeIFinansija":
                    clsKorekcija k = new clsKorekcija();
                    bool pom2 = k.ObradiZahtev();
                    if (pom2)
                        MessageBox.Show("Uspešno završeno!");
                    else
                        MessageBox.Show("Nije uspelo usaglašavanje robe i finansija!");

                   // 
                    break;

                    //case "Dozvole":
                    //    ShowNewForm(" ", 1, "Dozvole", 1, "", "", "P", "", "");
                    //    break;
                    //default:
                    //    break;
            }
        }
        

       

        

        
       
    }
}
