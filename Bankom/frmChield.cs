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

using Bankom.Class;


namespace Bankom

{
    public partial class frmChield : Form
    {
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
        //public DataTable tgg = new DataTable();
        public frmChield()
        {
            InitializeComponent();
            this.Activated += new EventHandler(frmChield_Activated);
        }
        protected override void OnClosed(EventArgs e)
        {           
            ((BankomMDI)this.MdiParent).itemB1_click(this.Text);            
             base.OnClosed(e);
        }
        private void frmChield_Load(object sender, EventArgs e)
        {
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
                    tw.ObradaStabla(this, "1", imedokumenta, DokumentJe);              
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
                            cononf.addFormControls(this, imedokumenta, iddokumenta.ToString(), OOperacija.Text);
                            docref.refreshDokumentBody(this, imedokumenta, iddokumenta.ToString(), DokumentJe);
                            docref.refreshDokumentGrid(this, imedokumenta, iddokumenta.ToString(), "", "",DokumentJe);
                            break;
                        case "P":
                            cononf.addFormControls(this, imedokumenta, iddokumenta.ToString(), OOperacija.Text);
                            clsObradaStablaPtipa procp = new clsObradaStablaPtipa();
                            supit = procp.Proces(imestabla, imedokumenta, idstablo);
                            docref.refreshDokumentGrid(this, imedokumenta, idstablo.ToString(), supit, "1",DokumentJe);
                            break;
                        case "I":                           
                            cononf.addFormControls(this, imedokumenta, iddokumenta.ToString(), OOperacija.Text);
                            string sel = "Select TUD From Upiti Where NazivDokumenta='" + imedokumenta + "' and ime like'GGrr%' AND TUD>0 Order by TUD";
                            Console.WriteLine(sel);
                            DataTable ti = db.ReturnDataTable(sel);
                            clsObradaStablaItipa proci = new clsObradaStablaItipa();
                            for (int j = 0; j < ti.Rows.Count; j++)
                            {
                                supit = proci.Proces(imedokumenta, ti.Rows[j]["TUD"].ToString());
                                    Console.WriteLine(supit);
                                docref.refreshDokumentGrid(this, imedokumenta, idstablo.ToString(), supit, ti.Rows[j]["TUD"].ToString(), DokumentJe);
                            }
                            break;
                        case "S":
                            cononf.addFormControls(this, imestabla, idstablo.ToString(), OOperacija.Text);
                            clsObradaStablaStipa procss = new clsObradaStablaStipa();
                            supit = procss.Proces(imestabla, imedokumenta, Convert.ToInt32(idstablo));
                            docref.refreshDokumentGrid(this, imestabla, idstablo.ToString(), supit,"1", DokumentJe);
                            break;
                    }
                    break;                   
            }
            
            panel1.Top = 0;
        }
        private void frmChield_Activated(object sender, EventArgs e)
        {

            if (VrstaPrikaza != "TreeView" )
            {              
                Boolean provera = true;
                clsProveraDozvola provdoz = new clsProveraDozvola();
                provera = provdoz.ProveriDozvole(imedokumenta,  idstablo.ToString(),Convert.ToString(iddokumenta) ,DokumentJe);
            }
            else
            {
              ////  
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

        private void ToolStripTextPos_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
          

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

          
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
          
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripLblPos_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTexIme_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

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
    }
          
}







