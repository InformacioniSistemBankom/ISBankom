using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using  Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;
using System.IO;


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

        public static void BrisanjeWordExcel(string brojDok,string vrstaDokumenta,string nazivDokumenta)
        {
            string putanja = "";
            string nazivWorda = brojDok.Replace('/', '%');
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Program.connectionString);
            var imeServera = builder.DataSource;
            putanja = imeServera.ToUpper() == Program.NazivRacunara.ToUpper() ? @"C:" : @"\\" + imeServera;
            putanja = vrstaDokumenta == "W"
                ? putanja + @"\ISdokumenta\" + $"{Program.imeFirme.Trim()}" + @"\Word\" + nazivDokumenta + @"\" + nazivWorda.Trim() + ".doc"
                : putanja + @"\ISdokumenta\" + $"{Program.imeFirme.Trim()}" + @"\Excel\" + nazivDokumenta + @"\" + nazivWorda.Trim() + ".xls";

            if (File.Exists(putanja)) File.Delete(putanja);

        }

        public static void ObradiDokument(string brojDok,string vrstaDokumenta,string nazivDokumenta , string prethodni)
        {
            var db = new DataBaseBroker();
            string putanja = "";
            string nazivDok = "";
            string prethodnik = "";
            bool vecPostoji = false;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Program.connectionString);
            var imeServera = builder.DataSource;
            putanja = imeServera.ToUpper() == Program.NazivRacunara.ToUpper() ? @"C:" : @"\\" + imeServera;
            var parametri = db.ReturnDataTable("SELECT * FROM Parametri WHERE ImeRacunara ='" + Program.NazivRacunara + "'");
            if (parametri.Rows.Count == 0)
            {
                MsgBox.ShowDialog("Nepoznati parametri o Wordu,Excelu, obratiti se administratoru!");
                return;
            }
            switch (vrstaDokumenta)
            {
                case "p":
                case "P":
                    var brDok = brojDok.Replace('/', '-');
                    putanja = putanja + @"\ISdokumenta\" + $"{Program.imeFirme.Trim()}" + @"\Pdf\" + nazivDokumenta + @"\" + brDok.Trim() + ".pdf";
                    System.Diagnostics.Process.Start(putanja);
                    return;
                case "w":
                case "W":
                    string nazivWorda = brojDok.Replace('/', '%');
                    string PIzvestaja = putanja + @"\ISdokumenta\" + $"{Program.imeFirme.Trim()}" + @"\Word\" + nazivDokumenta + @"\";
                    putanja = putanja + @"\ISdokumenta\" + $"{Program.imeFirme.Trim()}" + @"\Word\" + nazivDokumenta + @"\" + nazivWorda.Trim() + ".doc";
                   
                    if (File.Exists(putanja))
                    {
                        nazivDok = putanja.Substring(0, putanja.Length - 4);
                        vecPostoji = true;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(prethodni.Trim()))
                        {
                            if (prethodni.Substring(0, prethodni.IndexOf('-')) == nazivWorda.Substring(0, nazivWorda.IndexOf('-')))
                            {
                                prethodnik = prethodni.Replace('/', '%');
                                if (File.Exists(PIzvestaja.Trim() + prethodnik + ".doc")) nazivDok = PIzvestaja.Trim() + prethodnik;
                                else nazivDok = PIzvestaja + nazivDokumenta;


                            }
                            else
                            {
                                MsgBox.ShowDialog("Pogresan izbor prethodnika");
                                return;
                            }
                        }
                        else
                        {
                            nazivDok = PIzvestaja + nazivDokumenta;
                        }
                    }
                    nazivWorda = PIzvestaja + nazivWorda + ".doc";
                    nazivDok = nazivDok + ".doc";

                    if (!vecPostoji) File.Copy(nazivDok,nazivWorda);
                    System.Diagnostics.Process.Start(nazivWorda);
                    break;

                case "e":
                case "E":
                    string nazivExcela = brojDok.Replace('/', '%') + ".xls";
                    string PPutanja = putanja + @"\ISdokumenta\" + $"{Program.imeFirme.Trim()}" + @"\Excel\" + nazivDokumenta + @"\";
                  

                    if (File.Exists(PPutanja.Trim() + nazivExcela))
                    {
                        nazivDok = PPutanja + nazivExcela;
                        vecPostoji = true;
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(prethodni.Trim()))
                        {
                            if (prethodni.Substring(0, prethodni.IndexOf('-')) == nazivExcela.Substring(0, nazivExcela.IndexOf('-')))
                            {
                                prethodnik = prethodni.Replace('/', '%');
                                if (File.Exists(PPutanja.Trim() + prethodnik + ".xls")) nazivDok = PPutanja.Trim() + prethodnik + ".xls";
                                else nazivDok = PPutanja + nazivDokumenta + ".xls";


                            }
                            else
                            {
                                MsgBox.ShowDialog("Pogresan izbor prethodnika");
                                return;
                            }
                        }
                        else
                        {
                            nazivDok = PPutanja + nazivDokumenta + ".xls";
                        }
                    }

                    nazivDok = PPutanja.Trim() + nazivExcela;

                    if (!vecPostoji) File.Copy(nazivDok, nazivExcela);
                    System.Diagnostics.Process.Start(nazivExcela);


                    break;



            }

        }
       
    }
}
