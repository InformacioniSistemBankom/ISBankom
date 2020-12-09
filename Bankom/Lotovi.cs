using In.Sontx.SimpleDataGridViewPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bankom
{
    public partial class Lotovi : Form
    {

        public static Lotovi innerReference;
        public Lotovi()
        {
            InitializeComponent();
            button3.Enabled = false;
            dataGridViewPaging1.RequestQueryData += DataGridViewPaging_RequestQueryData;
            dataGridViewPaging1.Initialize(count());
            innerReference = this;
            innerReference.FormBorderStyle = FormBorderStyle.None;
            
            
            


        }

        private void DataGridViewPaging_RequestQueryData(object sender, RequestQueryDataEventArgs e)
        {
            using (SqlConnection con = new SqlConnection(Program.connectionString))
            {
                con.Open();
                using (var command = con.CreateCommand())
                {
                    if (String.IsNullOrEmpty(pretraga.Text))
                        command.CommandText = "SELECT [Lot] ,[NazivArtikla] ,[DatumProizvodnje] ,[DatumIsteka],[proizvodjac] as Proizvodjac ,[NazivZemlje] ,[NjihovLot]  FROM LotStavkeView order by iid desc OFFSET " + e.PageOffset + " ROWS FETCH NEXT " + e.MaxRecords + " ROWS ONLY";
                    else command.CommandText = "SELECT [Lot] ,[NazivArtikla] ,[DatumProizvodnje] ,[DatumIsteka],[proizvodjac] as Proizvodjac ,[NazivZemlje] ,[NjihovLot]  FROM LotStavkeView WHERE NazivArtikla like'%" + pretraga.Text + "%'  OR proizvodjac like  '%" + pretraga.Text + "%' OR NazivZemlje like  '%" + pretraga.Text + "%' OR Lot like  '%" + pretraga.Text + "%' order by iid desc OFFSET " + e.PageOffset + " ROWS FETCH NEXT " + e.MaxRecords + " ROWS ONLY";
                    dataGridViewPaging1.DataSource = command.ExecuteReader();
                    dataGridViewPaging1.DataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewPaging1.DataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;





                }
            }

        }
        public int count()
        {
            using (SqlConnection sqlConnection = new SqlConnection(Program.connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM LotStavkeView";
                    var reader = command.ExecuteScalar();
                    return Convert.ToInt32(reader);

                }

            }

        }

        private void Lotovi_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            NoviLot noviLot = new NoviLot();
            noviLot.FormBorderStyle = FormBorderStyle.None;

            noviLot.Show();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewPaging1.DataGridView.CurrentCell.RowIndex > -1)
            {
                int idLot;
                using (SqlConnection sqlConnection = new SqlConnection(Program.connectionString))
                {
                    sqlConnection.Open();
                    using (var command = sqlConnection.CreateCommand())
                    {

                        command.CommandText = "SELECT id_Lot FROM Lot where barkod ='" + dataGridViewPaging1.DataGridView.Rows[dataGridViewPaging1.DataGridView.CurrentCell.RowIndex].Cells[0].Value.ToString() + "'";
                        var reader = command.ExecuteScalar();
                        idLot = Convert.ToInt32(reader);

                    }

                }



                       string ime = "Lot";
                       string naslov = "print - " + ime;
                    
                        frmPrint fs = new frmPrint();
                        fs.FormBorderStyle = FormBorderStyle.None;
                        fs.BackColor = System.Drawing.Color.SeaShell;
                        fs.MdiParent = this.MdiParent;
                        fs.Text = naslov;
                        fs.LayoutMdi(MdiLayout.TileVertical);
                        fs.imefajla = ime;
                        fs.intCurrentdok = idLot;
                        fs.kojiprint = "lot";
                        fs.Dock = DockStyle.Fill;
                        BankomMDI pom = (BankomMDI)this.MdiParent;
                        pom.addFormTotoolstrip1(fs, naslov);
                        //pom.updateToolStrip(naslov);
                        fs.Show();
                      

                  
            }
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridViewPaging1_Load(object sender, EventArgs e)
        {

        }

        private void pretraga_TextChanged(object sender, EventArgs e)
        {

        }

        private void pretraga_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                using (SqlConnection sqlConnection = new SqlConnection(Program.connectionString))
                {
                    sqlConnection.Open();
                    using (var command = sqlConnection.CreateCommand())
                    {
                        command.CommandText = "SELECT COUNT(*) FROM LotStavkeView WHERE NazivArtikla like'%" + pretraga.Text + "%'  OR proizvodjac like  '%" + pretraga.Text + "%' OR NazivZemlje like  '%" + pretraga.Text + "%' OR Lot like  '%" + pretraga.Text + "%'";
                        var reader = command.ExecuteScalar();
                        if (Convert.ToInt32(reader) > 30)
                            dataGridViewPaging1.Initialize(Convert.ToInt32(reader));
                        else MessageBox.Show("Za unesenu pretragu nema rezultata!");


                    }


                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridViewPaging1.DataGridView.CurrentCell.RowIndex > -1)
            {
                string cellValue = dataGridViewPaging1.DataGridView.Rows[dataGridViewPaging1.DataGridView.CurrentCell.RowIndex].Cells[0].Value.ToString();
                NoviLot nl = new NoviLot(dataGridViewPaging1.DataGridView.Rows[dataGridViewPaging1.DataGridView.CurrentCell.RowIndex]);
                nl.FormBorderStyle = FormBorderStyle.None;
                nl.Show();
            }

        }

        private void Lotovi_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
