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
using System.Data.Sql;
using Bankom.Class;


namespace Bankom

{
    public partial class frmChield : Form
    {
        //Djora 26.09.20
        private int kaunter = 1;

        public string imedokumenta;
        public long iddokumenta;        
        public string imestabla;
        public int idstablo;
        public int idReda = -1;       
        public string imegrida = "";  // borka 25.02.20
        public string brdok;
        public string datum;
        public string DokumentJe;
        public string VrstaPrikaza = "";
        public string operacija = "";
        public int intStart = 0;// rednibroj  sloga od kojeg pocinje page
        public int intUkupno = 0; // ukupan brj stranica
        public  int pageno = 1; //ukupan broj stranica
        public string tUpit = "";
        public int BrRedova = 1;// broj redova na stranici
        private long brojdok = 1;
        //zajedno 12.1.2021.
        public string NazivSkladista;
        public string NazivSkladista1;
        public string NazivSkladista2;
        public string nastavakSkladista1;
        public string nastavakSkladista2;

        public frmChield()
        {
            InitializeComponent();
     //       this.Activated += new EventHandler(frmChield_Activated);
            //this.Activate += new EventHandler(frmChield_Activate);
            this.AutoScroll = true;
        }
        protected override void OnClosed(EventArgs e)
        {           
            ((BankomMDI)this.MdiParent).itemB1_click(this.Name);            
             base.OnClosed(e);
        }
        private void frmChield_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Snow;
            this.AutoScroll = true;
            ////tamara 01.02.2021.
            //this.Text = ".";

            intStart = 0;                
            toolStripStatusPrazno.Text = new String(' ', 150);            
            toolStripStatusprdva.Text = new String(' ', 50);
        
            clscontrolsOnForm cononf = new clscontrolsOnForm();
            clsdokumentRefresh docref = new clsdokumentRefresh();
            DataBaseBroker db = new DataBaseBroker();

            liddok.Text  = iddokumenta.ToString();           
            ldokje.Text = DokumentJe;
            lidstablo.Text = idstablo.ToString();
            limestabla.Text = imestabla;
            limedok.Text = imedokumenta;
            lBrDok.Text = brdok;
            lDatum.Text = datum;            
            panel1.Visible = false;

            switch (VrstaPrikaza)
            {
                case "TreeView":
                    clsTreeProcessing tw = new clsTreeProcessing();
                    tw.ObradaStabla(  this, "1", imedokumenta, DokumentJe);              
                    break;              
                default:
                    string supit = "";
                    switch (DokumentJe)
                    {
                        case "D":
                            if (imedokumenta == "KalkulacijaUlaza" || imedokumenta == "PDVKalkulacijaUlaza")
                            {
                                // punimo tabelu kalk za odabranu kalkulaciju da bismo imali podatke za zavisne troskove
                                string Odakle = "Load";
                                clsObradaKalkulacije okal = new clsObradaKalkulacije();
                                okal.RasporedTroskova(iddokumenta, " ", " ", " ", Odakle);
                                Odakle = "";
                            }
                            cononf.addFormControls(this,imedokumenta, iddokumenta.ToString(), OOperacija.Text);
                            docref.refreshDokumentBody(this,imedokumenta, iddokumenta.ToString(), DokumentJe);
                            docref.refreshDokumentGrid(this,imedokumenta, iddokumenta.ToString(), "", "",DokumentJe);
                            break;
                        case "P":
                           
                            cononf.addFormControls(this,imedokumenta, iddokumenta.ToString(), OOperacija.Text);
                            clsObradaStablaPtipa procp = new clsObradaStablaPtipa();
                            supit = procp.Proces(imestabla, imedokumenta, idstablo);
                            docref.refreshDokumentGrid(this,imedokumenta, idstablo.ToString(), supit, "1",DokumentJe);
                            break;
                        case "I":                           
                            cononf.addFormControls(this,imedokumenta, iddokumenta.ToString(), OOperacija.Text); cononf.addFormControls(this,imestabla, idstablo.ToString(), OOperacija.Text);
                            string sel = "Select TUD From Upiti Where NazivDokumenta='" + imedokumenta + "' and ime like'GGrr%' AND TUD>0 Order by TUD";
                            Console.WriteLine(sel);
                            System.Data.DataTable ti = db.ReturnDataTable(sel);
                            clsObradaStablaItipa proci = new clsObradaStablaItipa();
                            for (int j = 0; j < ti.Rows.Count; j++)
                            {
                                supit = proci.Proces(imedokumenta, ti.Rows[j]["TUD"].ToString());
                                    Console.WriteLine(supit);
                                docref.refreshDokumentGrid(this,imedokumenta, idstablo.ToString(), supit, ti.Rows[j]["TUD"].ToString(), DokumentJe);
                            }
                            break;
                        case "S":
                            
                            clsObradaStablaStipa procss = new clsObradaStablaStipa();
                            supit = procss.Proces(imestabla, imedokumenta, Convert.ToInt32(idstablo));
                            if (supit != "")
                            {
                                cononf.addFormControls( this,imestabla, idstablo.ToString(), OOperacija.Text);
                                docref.refreshDokumentGrid(this, imestabla, idstablo.ToString(), supit, "1", DokumentJe);
                            }
                            break;
                    }
                    break;                   
            }
            
            panel1.Top = 0;
            if (this.Text == "LOT")
            {
                foreach (var pb in this.Controls.OfType<Field>())
                {
                    //if (pb.cTip == 10 || pb.cTip == 8)
                    //     pb.Enabled = false;
                    if (pb.cTip == 10)
                    {
                        if (pb.VrstaKontrole == "combo") pb.comboBox.Enabled = false;
                        else pb.textBox.Enabled = false;
                    }
                    if (pb.cTip == 8) pb.dtp.Enabled = false;
                }
            }

        }
        private void frmChield_Activated(object sender, EventArgs e)
        { 
            Boolean provera = true;
            clsSettingsButtons sb = new clsSettingsButtons();
            clsProveraDozvola provdoz = new clsProveraDozvola();
            if (imestabla == "Artikli" || imestabla == "Komitenti")
            {
                if (OOperacija.Text != "PREGLED")
                    provera = provdoz.ProveriDozvole(imestabla, idstablo.ToString(), Convert.ToString(iddokumenta), DokumentJe);
            }
            else
            {
                if (OOperacija.Text != "PREGLED" && sender.ToString()!= "Bankom.Imenik, Text: frmChield")
                    provera = provdoz.ProveriDozvole(imedokumenta, idstablo.ToString(), Convert.ToString(iddokumenta), DokumentJe);
            }
        }
        private void frmChield_Resize(object sender, EventArgs e)
        {
            //Djora 04.06.19 Resize
            //_form_resize._resize();
        }
        private void ToolStripButtFirst_Click(object sender, EventArgs e)
        {      
            if( intStart==0) { return; }
            pageno = 1;
            intStart = 0;
            ToolStripTextPos.Text= pageno.ToString();
            navigacija(pageno);                           
        }

        private void ToolStripButtNext_Click(object sender, EventArgs e)
        {
            pageno = Convert.ToInt32(ToolStripTextPos.Text);
            if(pageno* this.BrRedova >= intUkupno ) { return; }
            intStart += this.BrRedova;

            navigacija(pageno+1);
            ToolStripTextPos.Text = Convert.ToString(pageno + 1);
        }
        private void ToolStripButtPrev_Click(object sender, EventArgs e)
        {                     
            intStart -= this.BrRedova;
            if (intStart <= 0)
            {
                intStart = 0;
                pageno = 1;
                ToolStripTextPos.Text = Convert.ToString(pageno);
            }
            else
            {
                pageno = Convert.ToInt32(ToolStripTextPos.Text);
                ToolStripTextPos.Text = Convert.ToString(pageno-1);                
            }             
            navigacija(pageno);           
        }

        private void ToolStripButtLast_Click(object sender, EventArgs e)
        {
            pageno = Convert.ToInt32(ToolStripLblPos.Text);   
            intStart = (pageno - 1) * BrRedova;
           
            ToolStripTextPos.Text = ToolStripLblPos.Text;             
            navigacija(pageno);                          
         }

        private void navigacija(int pageno)
        {          
            string tIme =this.imegrida;          
            if (!((this.Controls.Find(tIme, true).FirstOrDefault() is DataGridView dv)))
                MessageBox.Show("Greska ne postoji grid");
            else // postoji grid   
            {
                DataTable dt = new DataTable();
                DataBaseBroker db = new DataBaseBroker();
                int PageSize = this.BrRedova;
                int startIndex = PageSize * (pageno - 1);
                string mUpit = this.tUpit;
                Console.WriteLine(mUpit);
                mUpit += " OFFSET " + startIndex.ToString() + " ROWS FETCH NEXT " + PageSize.ToString() + " ROWS ONLY;";
                Console.WriteLine(mUpit);
                dt = db.ReturnDataTable(mUpit);
                if( dt.Rows.Count>0)
                   dv.DataSource = dt;
                dv.Refresh();
            }
               
            //ToolStripTextPos.Alignment = ToolStripItemAlignment.Right;
        }

        private void ToolStripTextPos_DoubleClick(object sender, EventArgs e)
        {
                pageno = Convert.ToInt32(ToolStripTextPos.Text);
                if (intStart == (pageno - 1) * this.BrRedova) { return; }

                this.intStart = (pageno - 1) * this.BrRedova;

                navigacija(pageno);
        }
        private void ToolStripTextPos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToString() != "Return")
            {
                return;
            }           

            if (this.Text.IndexOf("(") > -1) { return; }

            pageno = Convert.ToInt32(ToolStripTextPos.Text);
            if (intStart == (pageno - 1)* this.BrRedova) { return; }
            
            this.intStart = (pageno - 1) * this.BrRedova;
            navigacija(pageno);
        }

       

        
        private void button1_Click(object sender, EventArgs e)
        {
            brojdok = 1;
            clsPregled cp = new clsPregled();
            cp.Osvezi("prvi", ref brojdok);
            //toolStripLabel1.Text = "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            brojdok -= 1;
            if (brojdok < 1)
                brojdok = 1;
            clsPregled cp = new clsPregled();
            cp.Osvezi("predhodni", ref brojdok);
        }

        private void btnSled_Click(object sender, EventArgs e)
        {
            brojdok += 1;
            clsPregled cp = new clsPregled();
            cp.Osvezi("sledeci", ref brojdok);
        }

        private void btnZad_Click(object sender, EventArgs e)
        {
            clsPregled cp = new clsPregled();
            cp.Osvezi("poslednji", ref brojdok);
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //Djora 19.10.20
        private void frmChield_KeyDown(object sender, KeyEventArgs e)
        {
            //Da si ulogovan kao SA i da u tekstboxu u meniju gore pise 123
            if (Program.imekorisnika == "sa" & Program.Parent.ToolBar.Items["toolStripTextBox1"].Text == "123")
            {
              
                  //Povecanje visine redova
                if (e.KeyCode == Keys.F12)
                {

                    //int brojkolona = (int)(control.Height / control.RowTemplate.Height);
                    //MessageBox.Show(brojkolona.ToString());

                    DataGridView dgv = ActiveControl as DataGridView;
                    dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                    //dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    //dgv.RowTemplate.Height = 80;
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        row.Height = 80;
                    }

                    //dgv.Refresh();
                }

                //Levo +
                if (e.KeyCode == Keys.Right)
                {
                    //Console.WriteLine(this.ActiveControl.Name);
                    this.ActiveControl.Parent.Left = this.ActiveControl.Parent.Left + kaunter;
                }
                //Levo -
                if (e.KeyCode == Keys.Left)
                {
                    this.ActiveControl.Parent.Left = this.ActiveControl.Parent.Left - kaunter;
                }
                //Visina +
                if (e.KeyCode == Keys.Up)
                {
                    this.ActiveControl.Parent.Top = this.ActiveControl.Parent.Top - kaunter;
                }
                //Visina -
                if (e.KeyCode == Keys.Down)
                {
                    this.ActiveControl.Parent.Top = this.ActiveControl.Parent.Top + kaunter;
                }
                //Sirina +
                if (e.KeyCode == Keys.NumPad3 && e.Modifiers == Keys.Control)
                {
                    this.ActiveControl.Width = this.ActiveControl.Width + kaunter;
                }
                //Sirina -
                if (e.KeyCode == Keys.NumPad2 && e.Modifiers == Keys.Control)
                {
                    this.ActiveControl.Width = this.ActiveControl.Width - kaunter;
                }
                //Stavi kaunter na 1
                if (e.KeyCode == Keys.NumPad1 && e.Modifiers == Keys.Control)
                {
                    kaunter = 1;
                }
                //Stavi kaunter na 50
                if (e.KeyCode == Keys.NumPad0 && e.Modifiers == Keys.Control)
                {
                    kaunter = 50;
                }
                // Snimi kordinate i dimenzije u RecnikPodataka u Bazi
                if (e.KeyCode == Keys.F10)
                {
                    //Djora 10.11.20
                    string isd = "";

                    //Djora 10.11.20
                    switch (DokumentJe)
                    {
                        case "D":
                        case "P":
                        case "I":
                            isd = imedokumenta;
                            break;
                        case "S":
                            isd = imestabla;
                            break;
                    }

                    string imeKontrole;

                    Console.WriteLine(imestabla);

                    DataBaseBroker db = new DataBaseBroker();
                    Console.WriteLine(this.ActiveControl.Parent.Name);
                    Console.WriteLine("==========================");
                    Console.WriteLine(this.ActiveControl.Parent.Top);
                    Console.WriteLine(this.ActiveControl.Parent.Left);
                    Console.WriteLine(this.ActiveControl.Parent.Width);
                    Console.WriteLine(this.ActiveControl.Parent.Height);

                    double delW = Program.RacioWith; //Program.RacioWith * 1.4;
                    double delH = Program.RacioHeight; // Program.RacioHeight * 1.4;


                    Console.WriteLine(Math.Round(this.ActiveControl.Top / delH, 0));
                    Console.WriteLine(Math.Round(this.ActiveControl.Left / delW, 0));
                    Console.WriteLine(Math.Round(this.ActiveControl.Width / delW, 0));
                    Console.WriteLine(Math.Round(this.ActiveControl.Height / delH, 0));

                    double pT = Math.Round(this.ActiveControl.Parent.Top / delH, 0);
                    double pL = Math.Round(this.ActiveControl.Parent.Left / delW, 0);
                    double pW = Math.Round(this.ActiveControl.Parent.Width / delW, 0);
                    double pH = Math.Round(this.ActiveControl.Parent.Height / delH, 0);

                    Field ctrls = (Field)this.ActiveControl.Parent;

                    if (ctrls.cIdNaziviNaFormi == "20")
                    {
                        imeKontrole = ctrls.IME;
                        string query = " UPDATE dbo.RecnikPodataka SET cvrh=" + pT + ", clevo=" + pL
                                     + " WHERE(dbo.RecnikPodataka.Dokument = N'" + isd + "') "
                                    + " AND RecnikPodataka.AlijasPolja = N'" + imeKontrole + "'";


                        SqlCommand cmd1 = new SqlCommand(query);
                        db.Comanda(cmd1);

                        query = " UPDATE dbo.Upiti SET cWidth=" + pW
                               + " WHERE Ime = 'GgRr" + ctrls.cTabelaVView + "' "
                              + " AND NazivDokumenta = '" + ctrls.cDokument + "'";

                        cmd1 = new SqlCommand(query);
                        db.Comanda(cmd1);
                    }
                    else
                    {
                        imeKontrole = this.ActiveControl.Name;
                        string query = " UPDATE dbo.RecnikPodataka SET cvrh=" + pT + ", clevo=" + pL + ", cwidth=" + pW + ", cWidthKolone=" + pW
                                     + " WHERE(RecnikPodataka.Dokument = N'" + isd + "') "
                                     + " AND RecnikPodataka.AlijasPolja = N'" + imeKontrole + "'";
                        SqlCommand cmd1 = new SqlCommand(query);
                        db.Comanda(cmd1);
                    }

                }
            }
        }

        private void frmChield_FormClosed(object sender, FormClosedEventArgs e)
        {
            BankomMDI mdi = new BankomMDI();
            FormCollection fc = Application.OpenForms;
            int brojFormi = fc.Count;
            if (brojFormi == 1)
            {
                
                foreach (ToolStripItem t in mdi.ToolBar.Items)
                {
                    Program.Parent.ToolBar.Items[t.Name].Enabled = false;
                }
                Program.Parent.ToolBar.Items["toolStripTextBox1"].Enabled = true;
               

            }
        }
    }
          
}







