using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bankom
{
    public partial class frmLogout : Form
    {
        public frmLogout()
        {
            InitializeComponent();
        }

        private void btnNe_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnDa_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOdjava_Click(object sender, EventArgs e)
        {

        }
    }
}
