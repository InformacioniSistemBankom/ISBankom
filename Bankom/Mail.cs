using Bankom.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace Bankom
{
    public partial class Mail : Form
    {
        public Mail()
        {
            InitializeComponent();
        }
        string mejl = "";
        DataBaseBroker db = new DataBaseBroker();
        DataTable rez = new DataTable();
        string kojiprint;
        string imefajla;
        string ParamZaStampu;
        public Mail(string kojiprint, string imefajla, string ParamZaStampu)
        {
            this.kojiprint = kojiprint;
            this.imefajla = imefajla;
            this.ParamZaStampu = ParamZaStampu;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Djora 11.01.21 poc --------------------------------
            WebClient client = new WebClient();
            client.UseDefaultCredentials = true;
            string putanjaPdf = "http://192.168.1.4/ReportServer/Pages/ReportViewer.aspx?%2fIzvestaji%2f" + kojiprint + imefajla + "&rs:Command=Render&rs:Format=PDF" + "&database=" + Program.NazivBaze + "&Firma=" + Program.imeFirme + ParamZaStampu;  // "&rs%3aFormat=PDF";
            //string putanjaPdf = "http://192.168.1.4/ReportServer/Pages/ReportViewer.aspx?%2fIzvestaji%2f" + kojiprint + imefajla + "&rs:Command=Render&rs:Format=PDF";
            byte[] bytes = client.DownloadData(putanjaPdf);
            MemoryStream ms = new MemoryStream(bytes);
            SmtpClient server = new SmtpClient("mail.bankom.rs");
            server.EnableSsl = true;
            server.UseDefaultCredentials = false;
            server.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage mail = new MailMessage();
            //ivana 9.2.2021. imacemo u bazi sve mejlove napisane, nece ici na ovaj nacin
            string upit = "Select r.E_mail, r.Pass from Radnik as r inner join  KadrovskaEvidencija as k on r.Id_kadrovskaEvidencija = k.Id_KadrovskaEvidencija where k.Suser = @param0";
            DataTable dt = db.ParamsQueryDT(upit, Program.imekorisnika);
            if (dt.Rows.Count > 0)
            {
                string adresa = dt.Rows[0][0].ToString();
                server.Credentials = new NetworkCredential(adresa, dt.Rows[0][1].ToString());
                mail.From = new MailAddress(adresa);
                if (mejl != "")
                {
                    mail.To.Add(mejl);
                    mail.Subject = textBox1.Text;
                    mail.Body = richTextBox1.Text;
                    //mail.BodyFormat = MailFormat.Html;
                    mail.Attachments.Add(new Attachment(ms, imefajla + ".pdf"));
                    try
                    {
                        server.Send(mail);
                    }
                    catch (SmtpFailedRecipientException error)
                    {
                        MessageBox.Show("error: " + error.Message + "\nFailing recipient: " + error.FailedRecipient);
                    }
                }
                else
                    MessageBox.Show("Morate uneti e-mail!");
                button1.Enabled = false;
                MessageBox.Show("Uspešno ste poslali e-mail.");
                cmbEmail.Text = "";
                textBox1.Text = "";
                richTextBox1.Text = "";
            }
            //Djora 11.01.21 kraj --------------------------------
        }

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

        private void Mail_Load(object sender, EventArgs e)
        {
            string upit = "select Email from Komitenti where Email<>''";
            rez = db.ParamsQueryDT(upit);
            for (int i = 0; i < rez.Rows.Count; i++)
                cmbEmail.Items.Add(rez.Rows[i][0].ToString());
        }
    }
}
