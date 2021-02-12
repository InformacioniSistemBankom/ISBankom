using Bankom.Class;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace Bankom
{
    public partial class Print : Form
    {
        public int intCurrentdok = 0;
        public string imefajla = "";
        public string kojiprint= "";
        public string kojinacin = "";
        public string izvor = "";
        //Ivana 13.1.2021.
        DataBaseBroker db = new DataBaseBroker();
        DataTable rez = new DataTable();
        //public string param { get; set; }
        //public string mparam;
        //public string mvred;
        public Print()
        {
            InitializeComponent();

        }
        private void OnClosed(EventArgs e)
        {
            string b = this.Text;

            ((BankomMDI)this.MdiParent).itemB1_click(b);        
            ((BankomMDI)MdiParent).Controls["menuStrip"].Enabled = true;
            ((BankomMDI)MdiParent).Controls["menuStrip"].Enabled = true;
            base.OnClosed(e);
        }
        public string ParamZaStampu;
        private void frmPrint_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            statusPrint.Visible = false;
            string[] separators = new[] { "," };
            string putanja = "";                
            string mid = intCurrentdok.ToString();
            ParamZaStampu = "";
            string baza = "";
            panel1.Width = Width - 136;
            panel1.Height = Height;
            if (izvor == "kocka")
                baza = "";
            else
                baza = Program.NazivBaze;
            if (kojiprint == "rpt")
            {
                if (kojinacin == "")
                {
                    if (string.IsNullOrEmpty(Program.param)) { }
                    else
                    {
                        string[] mparam = Program.param.Split(separators, StringSplitOptions.None);
                        string[] mvred = Program.vred.Split(separators, StringSplitOptions.None);

                        for (int i = 0; i < mparam.Length; i++)
                        {
                            ParamZaStampu += "&" + mparam[i] + "=" + mvred[i];
                        }
                    }
                }
            }
           
            else
            {
                ParamZaStampu += "&id=" + mid;
            }
            switch (kojiprint)

            {
                case "prn":
                    putanja = "http://"+ LoginForm.ImeServera +"/ReportServer/Pages/ReportViewer.aspx?%2fIzvestaji%2fprn" + imefajla + "&rs:Command=Render" + "&database=" + Program.NazivBaze + "&Firma=" + Program.imeFirme + ParamZaStampu;
                    break;
                case "rpt":
                    putanja = "http://" + LoginForm.ImeServera + "/ReportServer/Pages/ReportViewer.aspx?%2fIzvestaji%2frpt" + imefajla + "&rs:Command=Render" + "&database=" + baza + ParamZaStampu;
             
                    break;
                case "lot":
                    putanja = @"http://" + LoginForm.ImeServera + "/ReportServer/Pages/ReportViewer.aspx?%2fIzvestaji%2fprnLot" + "&rs:Command=Render" + "&id_lot=" + intCurrentdok;
                    break;
                case "sif":
                    putanja = "http://" + LoginForm.ImeServera + "/ReportServer/Pages/ReportViewer.aspx?%2fIzvestaji%2fsif" + imefajla + "&rs:Command=Render";

                    break;
                case "pre":
                case "nag":
                case "pla":
                case "ako":
                    statusPrint.Visible = true;
                    LayoutMdi(MdiLayout.TileVertical);
                    webBrowser1.Dock = DockStyle.Fill;
                    webBrowser1.Top = Top - 25;
                    webBrowser1.Width = MdiParent.Width;
                    webBrowser1.Height = MdiParent.Height;

                    
                    putanja =  @"c:\TempXml\prevoz.xml";
                    if (kojiprint == "nag") { putanja = @"c:\TempXml\nagrade.xml"; }
                    if (kojiprint == "pla") { putanja = @"c:\TempXml\plate.xml"; }
                    if (kojiprint == "ako") { putanja = @"c:\TempXml\akontacije.xml"; }
                    if (kojiprint == "pre") { putanja = @"c:\TempXml\prevoz.xml"; }
                    break;
                default:
                    putanja = "http://" + LoginForm.ImeServera + "/ReportServer/Pages/ReportViewer.aspx?/Izvestaji/prn" + imefajla + "&rs:Command=Render&id=" + Convert.ToString(intCurrentdok);
                    break;
            }
            //System.Diagnostics.Process.Start(putanja);
            webBrowser1.Navigate(putanja);
        }   
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            webBrowser1.Print();   
        }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowSaveAsDialog();
        }

        public void btnEmail_Click(object sender, EventArgs e)
        {
            //ivana 12.2.2021.
            Form f = Application.OpenForms["Mail"];
            if (f != null)
                f.Close();
            else
            {
                Mail m = new Mail(kojiprint, imefajla, ParamZaStampu);
                m.ShowDialog();
            }
        }
    }
}

