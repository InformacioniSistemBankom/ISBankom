using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bankom.Class
{
    class clsUpisivanjeTabeleDatuma
    {
        public bool UpisiDatume()
        {
            bool UpisiDatume = false;
            string Godina;
            DataBaseBroker db = new DataBaseBroker();
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;

            if (MessageBox.Show("Upisujemo tabelu datuma za " + DateTime.Now.Year.ToString(), " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Godina = Prompt.ShowDialog("", "Unesite Godinu za tabelu datuma ", "Tabela datuma");
                if (Godina == DateTime.Now.Year.ToString() || Godina == DateTime.Now.Year.ToString()+1)
                {
                    db.ExecuteStoreProcedure("PopuniTimeByDay", "Godina:" + Godina);
                    UpisiDatume = true;
                }
            }
                      
            return UpisiDatume;
        }
    }
}