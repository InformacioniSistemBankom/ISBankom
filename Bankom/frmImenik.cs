using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bankom.Class;

namespace Bankom
{
    public partial class frmImenik : Form
    {
        public frmImenik()
        {
            InitializeComponent();
        }
        public string connectionString = Program.connectionString;



        private void frmImenik_Load(object sender, EventArgs e)
        {



        }

        private void PuniDgrid()
        {
            SqlConnection cnn = new SqlConnection(connectionString);

            cnn.Open();

            string pom = textBox1.Text;


            string query = "Select CeloIme, Firma, Posao, E_mail,Napomene, Pejdzer, Adresa, Mesto, Drzava from [Imenik] where [CeloIme] like @param0 or [Firma] like @param0  or [Posao] like @param0 or [E_mail] like @param0 or [Napomene] like @param0 or [Pejdzer] like @param0 or [Drzava] like @param0 or [Mesto] like @param0 or [Adresa] like @param0 ";


            DataBaseBroker db = new DataBaseBroker();

            DataTable dt = new DataTable();
            dt = db.ParamsQueryDT(query, "%" + pom + "%");
            Console.WriteLine(dt.Rows.Count);

            dataGridView1.DataSource = dt;


            dataGridView1.Columns["Pejdzer"].HeaderText = "Napomena2";
            dataGridView1.Columns["Napomene"].HeaderText = "Napomena1";
            dataGridView1.Columns["CeloIme"].HeaderText = "Ime i prezime";
            dataGridView1.Columns["CeloIme"].Width = 200;
            dataGridView1.Columns["Posao"].HeaderText = "Brojevi telefona";
            dataGridView1.Columns["E_mail"].HeaderText = "E-mail";
            dataGridView1.Columns["E_mail"].Width = 130;
            dataGridView1.Columns["Drzava"].HeaderText = "Dr`ava";



            int rc = dataGridView1.Rows.Count;
            for (int i = 0; i < rc; i++)
            {



                var cell = dataGridView1.Rows[i].Cells[3];
                cell.Style.Font = new Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            }




            cnn.Close();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            BankomMDI frm = new BankomMDI();
            frm.Show();
        
        }



        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection cnn = new SqlConnection(connectionString);

            cnn.Open();

            string pom = textBox1.Text;


            string query = "Select CeloIme, Firma, Posao, E_mail,Napomene, Pejdzer, Adresa, Mesto, Drzava from [Imenik] where [CeloIme] like @param0 or [Firma] like @param0  or [Posao] like @param0 or [E_mail] like @param0 or [Napomene] like @param0 or [Pejdzer] like @param0 or [Drzava] like @param0 or [Mesto] like @param0 or [Adresa] like @param0 ";


            DataBaseBroker db = new DataBaseBroker();

            DataTable dt = new DataTable();
            dt = db.ParamsQueryDT(query, "%" + pom + "%");
            Console.WriteLine(dt.Rows.Count);

            dataGridView1.DataSource = dt;


            dataGridView1.Columns["Pejdzer"].HeaderText = "Napomena2";
            dataGridView1.Columns["Napomene"].HeaderText = "Napomena1";
            dataGridView1.Columns["CeloIme"].HeaderText = "Ime i prezime";
            dataGridView1.Columns["Posao"].HeaderText = "Brojevi telefona";
            dataGridView1.Columns["E_mail"].HeaderText = "E-mail";

            dataGridView1.Columns["Drzava"].HeaderText = "Dr`ava";
            dataGridView1.Columns["CeloIme"].Width = 200;
            dataGridView1.Columns["E_mail"].Width = 130;


            int rc = dataGridView1.Rows.Count;
            for (int i = 0; i < rc; i++)
            {



                var cell = dataGridView1.Rows[i].Cells[3];
                cell.Style.Font = new Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            }




            cnn.Close();


        }

        private void btnBrisanje_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Da li sigurno želite da obrišete podatke? ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    SqlConnection cnn = new SqlConnection(connectionString);

                    cnn.Open();

                    string ime = tbIme.Text;
                    string firma = tbFirma.Text;


                    string query = "delete from [Imenik] where [CeloIme]=@param0 or [Firma]=@param1";

                    DataBaseBroker db = new DataBaseBroker();

                    DataTable dt = new DataTable();
                    dt = db.ParamsQueryDT(query, ime, firma);
                    Console.WriteLine(dt.Rows.Count);

                    dataGridView1.DataSource = dt;


                
                    
                    cnn.Close();

                
                    tbIme.Text = "";
                    tbFirma.Text = "";
                    tbBroj.Text = "";
                    tbEmail.Text = "";
                    tbNapomena1.Text = "";
                    tbNapomena2.Text = "";
                    tbAdresa.Text = "";
                    tbMesto.Text = "";
                    tbDrzava.Text = "";
                }
            }

            PuniDgrid();

        }

        private void btnIzmena_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbIme.Text) && String.IsNullOrEmpty(tbFirma.Text))
            {
                MessageBox.Show("Morate uneti Ime i prezime ili Firmu.");

            }
            else
            {
                if (MessageBox.Show("Da li sigurno želite da izmenite podatke? ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {




                    SqlConnection cnn = new SqlConnection(connectionString);

                    cnn.Open();


                    string ime = tbIme.Text;
                    string firma = tbFirma.Text;
                    string broj = tbBroj.Text;
                    string enail = tbEmail.Text;
                    string nap1 = tbNapomena1.Text;
                    string nap2 = tbNapomena2.Text;
                    string adr = tbAdresa.Text;
                    string grad = tbMesto.Text;
                    string drz = tbDrzava.Text;



                    string query = "UPDATE [Imenik] SET  [Firma]=@param0, [Posao]=@param1, [E_mail]=@param2, [Napomene]=@param3, [Pejdzer]=@param4, [Adresa]=@param5, [Mesto]=@param6, [Drzava]= @param7 WHERE [CeloIme] =@param8; ";

                    DataBaseBroker db = new DataBaseBroker();

                    DataTable dt = new DataTable();
                    dt = db.ParamsQueryDT(query, firma, broj, enail, nap1, nap2, adr, grad, drz, ime);
                    Console.WriteLine(dt.Rows.Count);

                    dataGridView1.DataSource = dt;

               
                    cnn.Close();

                 
                    tbIme.Text = "";
                    tbFirma.Text = "";
                    tbBroj.Text = "";
                    tbEmail.Text = "";
                    tbNapomena1.Text = "";
                    tbNapomena2.Text = "";
                    tbAdresa.Text = "";
                    tbMesto.Text = "";
                    tbDrzava.Text = "";
                }
            }
            PuniDgrid();
        }

        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (dataGridView1.Rows[0] != null)
                {
                    tbIme.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    tbFirma.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                    tbBroj.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    tbEmail.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                    tbNapomena1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                    tbNapomena2.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                    tbAdresa.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                    tbMesto.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                    tbDrzava.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();

                }
                else
                {
                    MessageBox.Show("Red je prazan. Selektujte red sa podacima.");
                }
            }
            else
            {
                MessageBox.Show("Možete selektovati samo jedan red za izmenu.");

            }
        }

        private void btnUnos_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbIme.Text) && String.IsNullOrEmpty(tbFirma.Text))
            {
                MessageBox.Show("Morate uneti Ime i prezime ili Firmu.");

            }
            else
            {
                SqlConnection cnn = new SqlConnection(connectionString);

                cnn.Open();


                string ime = tbIme.Text;
                string firma = tbFirma.Text;
                string broj = tbBroj.Text;
                string enail = tbEmail.Text;
                string nap1 = tbNapomena1.Text;
                string nap2 = tbNapomena2.Text;
                string adr = tbAdresa.Text;
                string grad = tbMesto.Text;
                string drz = tbDrzava.Text;


                string query = "INSERT INTO [Imenik] ([CeloIme], [Firma], [Posao], [E_mail], [Napomene], [Pejdzer], [Adresa], [Mesto], [Drzava])" +
                    "VALUES(@param0, @param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8); ";

                DataBaseBroker db = new DataBaseBroker();

                DataTable dt = new DataTable();
                dt = db.ParamsQueryDT(query, ime, firma, broj, enail, nap1, nap2, adr, grad, drz);
                Console.WriteLine(dt.Rows.Count);

                dataGridView1.DataSource = dt;

               
                cnn.Close();

              
                tbIme.Text = "";
                tbFirma.Text = "";
                tbBroj.Text = "";
                tbEmail.Text = "";
                tbNapomena1.Text = "";
                tbNapomena2.Text = "";
                tbAdresa.Text = "";
                tbMesto.Text = "";
                tbDrzava.Text = "";

            }

            PuniDgrid();
        }


    }
}
