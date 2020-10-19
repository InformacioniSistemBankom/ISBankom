using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Bankom.Class
{
    class clsEvaluation
    {
        public string Evaluate(string v)
        {
            DataTable dt = new DataTable();
            var r = dt.Compute(v, "");
            return (r.ToString());
        }
    }
}
