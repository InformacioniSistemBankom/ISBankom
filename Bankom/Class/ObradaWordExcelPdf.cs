using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using  Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;

namespace Bankom.Class
{
    class ObradaWordExcelPdf
    {


        public static void  OtvoriDokument(string vrstaDokumenta,string putanjaDokumenta,string brDok)
        {

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Program.connectionString);
            putanjaDokumenta = putanjaDokumenta.Replace("ImeServera", builder.DataSource).Replace("FFirma", Program.imeFirme);
            putanjaDokumenta += brDok;
            switch (vrstaDokumenta)
                
            {
                case "W":
                    putanjaDokumenta += ".doc";
                    
                /*    var  wdApp = new Microsoft.Office.Interop.Word.Application

                    {
                        Visible = true,
                        WindowState = WdWindowState.wdWindowStateNormal

                    };
                    Document aDoc = wdApp.Documents.Open(putanjaDokumenta);*/
                    
                    break;
                case "E":
                    putanjaDokumenta += ".xls";
                   
                  /*  var excelApp = new Microsoft.Office.Interop.Excel.Application
                    {
                        WindowState = XlWindowState.xlNormal,
                        Visible = true
                    };
                    var books = excelApp.Workbooks;
                    excelApp.Workbooks.Open(putanjaDokumenta);*/
                    

                    break;
                case "P":
                    putanjaDokumenta += ".pdf";

                break;
            }
            System.Diagnostics.Process.Start(putanjaDokumenta);

        }
    }
}
