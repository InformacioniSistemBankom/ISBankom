
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bankom
{
    public partial class Test : Form
    {
        public AutoCompleteStringCollection DataCollectionTest = new AutoCompleteStringCollection();
        public Test()
        {
            InitializeComponent();
        }

        private void Test_Load(object sender, EventArgs e)
        {
            DataGridView dataGridView1 = new DataGridView();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new System.Drawing.Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new System.Drawing.Size(240, 150);
            dataGridView1.TabIndex = 0;
            

            // reportViewer1.ProcessingMode = ProcessingMode.Remote;

            // Microsoft.Reporting.WebForms.ServerReport serverReport = reportViewer1.ServerReport;

            // // Get a reference to the default credentials  
            // System.Net.ICredentials credentials =
            //     System.Net.CredentialCache.DefaultCredentials;

            // // Get a reference to the report server credentials  
            // ReportServerCredentials rsCredentials =
            //     serverReport.ReportServerCredentials;

            // // Set the credentials for the server report  
            // rsCredentials.NetworkCredentials = credentials;

            // // Set the report server URL and report path  
            // serverReport.ReportServerUrl =
            //     new Uri("https:// <Server Name>/reportserver");
            // serverReport.ReportPath =
            //     "/AdventureWorks Sample Reports/Sales Order Detail";

            // // Create the sales order number report parameter  
            // Microsoft.Reporting.WebForms.ReportParameter salesOrderNumber = new Microsoft.Reporting.WebForms.ReportParameter();
            // salesOrderNumber.Name = "SalesOrderNumber";
            // salesOrderNumber.Values.Add("SO43661");

            // // Set the report parameters for the report  
            //// reportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { salesOrderNumber });

            // // Refresh the report  
            // reportViewer1.RefreshReport();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            

        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            
            //e.Control.KeyPress += new KeyEventHandler(Control_KeyPress);
        }


        private void Control_KeyPress(object sender, KeyEventArgs e)
        {
           
        }

      

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textPib_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textMB_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textRB_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textTK_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }

    }
                //rdrPred.Close();
             
     