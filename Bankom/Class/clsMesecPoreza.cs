using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
namespace Bankom.Class
{
    class clsMesecPoreza
    {
        public int ObradiMesecPoreza( string DatumDokumenta)
        {
            int PMESEC=0;
            int DanPoreza;
            DateTime pttime;
            DateTime pDatum;          
            
            pttime= DateTime.Now;  // tekuci datum
            pDatum = Convert.ToDateTime(DatumDokumenta); //datum dokumenta           
           

            Console.WriteLine(pttime);
            Console.WriteLine(pDatum);            

            if (Program.ID_MojaZemlja == 38)
                DanPoreza = 10;
            else
                DanPoreza = 15;

            if (pDatum.Month != pttime.Month && pttime.Day <= DanPoreza)
            {
                PMESEC = pttime.Month - 1;
                if (PMESEC == 0)
                    PMESEC = 12;
            }
            else
                PMESEC = pttime.Month;

            return (PMESEC);
        }

    }
}
