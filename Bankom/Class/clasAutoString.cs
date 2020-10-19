using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bankom
{
    class clasAutoString
    {
      

        public void addItems(AutoCompleteStringCollection col3, string ss)
        {

           // string strupit = ss;


            Cursor.Current = Cursors.WaitCursor;
            SqlCommand cmd = new SqlCommand();
            SqlConnection cnn = new SqlConnection(Program.connectionString);
         
            if (cnn.State == ConnectionState.Closed)
            {
                
               cnn= Program.GetConnection();
                
            }


            cmd.Connection = cnn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = ss ;

            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                
                col3.Add(" " + rdr[0]);
            }
            rdr.Close();
            cmd.Dispose();
            cnn.Close();
            Cursor.Current = Cursors.Default;

        }

        
    }
    }

