using Bankom.Class;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using Win32Mapi;
using Microsoft.Win32;
using System.Diagnostics;

namespace Bankom
{
    public partial class Print : Form
    {
        public int intCurrentdok = 0;
        public string imefajla = "";
        public string kojiprint= "";
        public string kojinacin = "";
        public string izvor = "";
        public string nazivForme = "";
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
            //tamara 11.2.2021.
            string b = this.Name;

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
            panel1.Height = Height - 30;
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
            // steva ; izvuci iz connection stringa username i pass
            string[] niz = Program.connectionString.Split(';');
            string DatabaseUsername = niz[2].Split('=')[1];
            string DatabasePassword = niz[3].Split('=')[1];
            switch (kojiprint)
            {
                case "prn":
                    putanja = "http://" + LoginForm.ReportServer + "/ReportServer/Pages/ReportViewer.aspx?%2fIzvestaji%2fprn" + imefajla + "&rs:Command=Render" + "&database=" + Program.NazivBaze + "&server=" + LoginForm.ImeServera + "&username=" + DatabaseUsername + "&password=" + DatabasePassword + "&Firma=" + Program.imeFirme + "&slike=" + LoginForm.ReportSlike + ParamZaStampu;
                    putanja = putanja.Replace("#", "%23").Replace("+", "%2b");//prilagodi specijalne karaktere kod passworda (znak + i # prave probleme)
                    //Ivana 13.1.2021.
                    string upit = "select Email from Komitenti where Email<>''";
                    rez = db.ParamsQueryDT(upit);
                    for (int i = 0; i < rez.Rows.Count; i++)
                        cmbEmail.Items.Add(rez.Rows[i][0].ToString());
                    break;
                case "rpt":
                    putanja = "http://" + LoginForm.ReportServer + "/ReportServer/Pages/ReportViewer.aspx?%2fIzvestaji%2frpt" + imefajla + "&rs:Command=Render" + "&database=" + Program.NazivBaze + "&server=" + LoginForm.ImeServera + "&username=" + DatabaseUsername + "&password=" + DatabasePassword + "&slike=" + LoginForm.ReportSlike + ParamZaStampu;
                    putanja = putanja.Replace("#", "%23").Replace("+", "%2b");//prilagodi specijalne karaktere kod passworda (znak + i # prave probleme)
                    break;
                case "lot":
                    putanja = @"http://" + LoginForm.ReportServer + "/ReportServer/Pages/ReportViewer.aspx?%2fIzvestaji%2fprnLot" + "&rs:Command=Render" + "&id_lot=" + intCurrentdok;
                    break;
                case "sif":
                    putanja = "http://" + LoginForm.ReportServer + "/ReportServer/Pages/ReportViewer.aspx?%2fIzvestaji%2fsif" + imefajla + "&rs:Command=Render";
                    putanja = putanja.Replace("#", "%23").Replace("+", "%2b");//prilagodi specijalne karaktere kod passworda (znak + i # prave probleme)
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
            System.Diagnostics.Process.Start(putanja);
            //System.Diagnostics.Process.Start("C:\\Users\\ivana.jelic.BANKOM\\Desktop\\BrutoBilans.xls");
            //webBrowser1.Navigate(putanja);
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
            //pocetak attachmenta

            OpenFileDialog af = new OpenFileDialog();
            af.Title = "Attach File";
            af.Filter = "Any File (*.*)|*.*";

            //comboAttachm.Enabled = true;


            WebClient client = new WebClient();
            client.UseDefaultCredentials = true;

            //dohvati lokalizaciju (sr-Lat-Sr)
            var culture = System.Globalization.CultureInfo.CurrentCulture;

            //putanja do stampe
            var putanjaPdf = "http://" + LoginForm.ReportServer + "/ReportServer/Pages/ReportViewer.aspx?%2fIzvestaji%2f" + kojiprint + imefajla + "&rs:Command=Render&rs:Format=PDF" + "&database=" + Program.NazivBaze + "&Firma=" + Program.imeFirme + "&slike=" + LoginForm.ReportSlike + "&rs:ParameterLanguage=" + culture.ToString() + ParamZaStampu;
            putanjaPdf = putanjaPdf.Replace("#", "%23").Replace("+", "%2b");//prilagodi specijalne karaktere kod passworda (znak + i # prave probleme)     

            WebClient webClient = new WebClient();
            client.Headers.Add("Accept-Language", culture.ToString());   //podesava da se zarezi i tacke kod brojeva vide u zavisnosti od jezika u browseru
            webClient.UseDefaultCredentials = true;
            // webClient.DownloadFile(putanjaPdf, @"d:\myfile1.pdf");

            //stavi pdf u memoriju
            byte[] bytes = client.DownloadData(putanjaPdf);
            MemoryStream ms = new MemoryStream(bytes);
            //skini pdf u lokalni folder u kome se nalazi projekat
            using (FileStream file = new FileStream(imefajla + ".pdf", FileMode.Create, System.IO.FileAccess.Write))
            {
                ms.Read(bytes, 0, (int)ms.Length);
                file.Write(bytes, 0, bytes.Length);
                ms.Close();


                //Simple Mapi za slanje maila
                var mapi = new SimpleMapi();
                mapi.AddRecipient(name: cmbEmail.SelectedIndex.ToString(), addr: null, cc: false);
                mapi.Attach(filepath: file.Name);    //attachuj pdf
                mapi.Send(subject: "", noteText: ""); // otvori mail client

                //obrisi attachment sa racunara
                file.Dispose();
                File.Delete(@"" + file.Name);
                //Kraj Simple Mapi za slanje maila
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:" + cmbEmail.SelectedIndex.ToString() + "?subject=&body=");
        }
        //</steva, otvori default mail client sa prozorom za pisanje novog maila>
        //Ivana 13.1.2021.
        string mejl = "";
        private void cmbEmail_SelectedIndexChanged(object sender, EventArgs e)
        {
            mejl = cmbEmail.SelectedItem.ToString();
            button1.Enabled = true;
        }

        private void cmbEmail_TextChanged(object sender, EventArgs e)
        {
            mejl = cmbEmail.Text;
            button1.Enabled = true;
        }
    }
}