using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;

namespace Bankom.Class
{
    class clsPregled
    {
        public static Form forma = new Form();
        public static long kk=0;
        public static long BrojacDokumenata = 0;

        private string Operacija = "";
        public static string UpitZaPregled = "";
        public static string dokje = "";
        public static string imedokumenta = "";
        private string NazivKlona = "";
        private string sql = "";
        private string filter = "";
        public string IdDokView = "0";
        public string Idstablo = "0";
        DataBaseBroker db = new DataBaseBroker();
        clsProveraDozvola pd = new clsProveraDozvola();
        clsdokumentRefresh cdr = new clsdokumentRefresh();
        public long Pregledaj(string ffilter)
        {
            forma = Program.Parent.ActiveMdiChild;
            BrojacDokumenata = 0;
            dokje = forma.Controls["ldokje"].Text;
            if (dokje == "S")
                imedokumenta = ((Bankom.frmChield)forma).imestabla;
            else
                imedokumenta = forma.Controls["limedok"].Text;
            Idstablo = forma.Controls["lidstablo"].Text;
            
            filter = ffilter;

            sql = "SELECT UlazniIzlazni as NazivKlona from SifarnikDokumenta where Naziv=@param0";
            DataTable dt = db.ParamsQueryDT(sql, imedokumenta);
            if (dt.Rows.Count > 0) NazivKlona = dt.Rows[0]["NazivKlona"].ToString();

            UpitZaPregled = PripremaidDokumentaZaPregled();
            if (UpitZaPregled == "KRAJ") goto PrinudniIzlaz;                

            if (dokje == "S" || dokje == "P")
            {
                    Console.WriteLine(UpitZaPregled);
                    DataTable t = db.ReturnDataTable(UpitZaPregled);
                    if (t.Rows.Count == 0)
                    {
                       MessageBox.Show("Ne postoje podaci za zadate uslove");
                       goto PrinudniIzlaz;
                    }
                  //t.rows.count>0    
                    Console.WriteLine(UpitZaPregled);
                    cdr.refreshDokumentGrid(forma,imedokumenta, "1", UpitZaPregled,"1", dokje);///?????????????????
                    kk = 1;                    
            }
            else// nisu S ili P
            {
                    if (kk > 0)
                    {
                        Operacija = "prvi";
                        Osvezi( Operacija, ref BrojacDokumenata);
                    }
                    else
                    {
                        MessageBox.Show("Ne postoje podaci za zadate uslove");
                    }
            }           
 PrinudniIzlaz:
            return kk;           
        }
        public void Osvezi( string Operacija, ref long BrojDok)
        {
            DataTable dp = new DataTable();
            if (Operacija == "") return;
            if (IdDokView == "0")
            {
                dp = db.ReturnDataTable(UpitZaPregled);
                if(dp.Rows.Count>0)
                   IdDokView = dp.Rows[0]["IdDokumentZaPregled"].ToString();
            }
          
            if (((Bankom.frmChield)forma).panel1.Visible == false)
                ((Bankom.frmChield)forma).panel1.Visible = true;
           
            switch (Operacija)
            {
                case "prvi":
                    if(dp.Rows.Count>0)
                    {
                        IdDokView = dp.Rows[0]["IdDokumentZaPregled"].ToString();
                        ((Bankom.frmChield)forma).lblBroj.Text = "1";
                    }
                    break;
                case "predhodni":
                    if (BrojDok < 1)
                    {
                        BrojDok += 1;
                        return;
                    }
                    IdDokView = dp.Rows[Convert.ToInt32(BrojDok)-1]["IdDokumentZaPregled"].ToString();
                    ((Bankom.frmChield)forma).lblBroj.Text = Convert.ToString(BrojDok);
                    break;
                case "sledeci":
                    if (BrojDok > kk)
                    {
                        BrojDok -= 1;
                        return;
                    }
                    IdDokView = dp.Rows[Convert.ToInt32(BrojDok)-1]["IdDokumentZaPregled"].ToString();
                    ((Bankom.frmChield)forma).lblBroj.Text = Convert.ToString(BrojDok);
                    break;
                case "poslednji":
                    BrojDok = kk;
                    IdDokView = dp.Rows[Convert.ToInt32(kk-1)]["IdDokumentZaPregled"].ToString();
                    ((Bankom.frmChield)forma).lblBroj.Text = Convert.ToString(kk);
                    break;
            }     
                     
            cdr.refreshDokumentBody(forma,imedokumenta, IdDokView, dokje);
            cdr.refreshDokumentGrid(forma,imedokumenta, IdDokView, "","", dokje); 


            pd.ProveriDozvole(imedokumenta, Idstablo, IdDokView, dokje);

        }
        //public void ObrisiZaglavljeIStavkePoljaZaUnos()
        //{
        //    forma = Program.Parent.ActiveMdiChild;
        //    dokje = forma.Controls["ldokje"].Text;
        //    if (dokje == "S")
        //        imedokumenta = ((Bankom.frmChield)forma).imestabla;
        //    else
        //        imedokumenta = forma.Controls["limedok"].Text;
        //    sql = "SELECT UlazniIzlazni as NazivKlona from SifarnikDokumenta where Naziv=@param0";
        //    DataTable dt = db.ParamsQueryDT(sql, imedokumenta);
        //    if (dt.Rows.Count != 0) NazivKlona = dt.Rows[0]["NazivKlona"].ToString();

        //    sql = "SELECT alijaspolja as polje,izborno from RecnikPodataka where  dokument=@param0 and TabIndex>=0 and width>0 and Height > 0 order by tabindex";
        //    DataTable drp = db.ParamsQueryDT(sql, NazivKlona);
            
        //    foreach (DataRow row in drp.Rows)
        //    {
        //        //Console.WriteLine(row["polje"].ToString());
        //        Field pb = (Field)Program.Parent.ActiveMdiChild.Controls[row["polje"].ToString()];
        //        if(pb!=null)
        //        { 
        //                    //pb.ID = "1";
        //                    //pb.Vrednost = "";
        //                    pb.cEnDis = "";
        //                    switch (pb.VrstaKontrole)
        //                    {
        //                        case "tekst":
        //                            pb.textBox.Text = "";
        //                            pb.textBox.Enabled = true;
        //                            break;
        //                        case "datum":
        //                            pb.dtp.Text = "";
        //                            //pb.dtp.CustomFormat = " ";
        //                            pb.dtp.Format = DateTimePickerFormat.Custom;
        //                            pb.dtp.Enabled = true;
        //                            //pb.dtp.CustomFormat = "dd.MM.yy";
        //                            //pb.dtp.Format = DateTimePickerFormat.Custom;
        //                            break;
        //                        case "combo":
        //                            pb.comboBox.Text = "";
        //                            pb.ID = "1";
        //                            pb.comboBox.Enabled = true;
        //                            pb.cIzborno = row["izborno"].ToString();
        //                        break;
        //                    }
        //        }
        //    }
        //    //pd.ProveriDozvole(imedokumenta, Idstablo, "1", dokje);
        //}
        public string PripremaidDokumentaZaPregled()
        {
            string PripremaidDokumentaZaPregled = "";
            string uupit = "";
            string KrajUpita = "";
            string WWhere = "";
            string c1 = "";
            if (dokje == "D")
            {
                if (imedokumenta != "OpisTransakcije" && imedokumenta != "NalogGlavneKnjige")
                    uupit = "SELECT distinct ID_" + NazivKlona.Trim() + "Totali as IdDokumentZaPregled FROM " + NazivKlona.Trim() + "Totali ";
                else
                    uupit = "SELECT distinct ID_" + NazivKlona.Trim() + "StavkeView as IdDokumentZaPregled FROM " + NazivKlona.Trim() + "StavkeView ";
                if (imedokumenta == "UgovorOOtkupu" || imedokumenta == "UgovorOOtkupuAneks")
                    uupit = "SELECT distinct ID_" + NazivKlona.Trim() + "View as IdDokumentZaPregled FROM " + NazivKlona.Trim() + "View ";
            }

            if (dokje == "S" || dokje == "P")
            {
                sql = "select upit from upiti where Ime='GgRr" + NazivKlona.Trim() + "StavkeView'";
                ////Console.WriteLine(sql);
                DataTable dt = db.ReturnDataTable(sql);
                if (dt.Rows.Count > 0) uupit = dt.Rows[0]["upit"].ToString();
                // Jovana 14.01.21 kad postoji order by
                if (uupit.Contains("ORDER BY") == true)
                {
                    KrajUpita = uupit.Substring(uupit.IndexOf("ORDER BY"));
                    uupit = uupit.Substring(0, uupit.IndexOf("ORDER BY") - 1);
                }
            }

            foreach (var pb in forma.Controls.OfType<Field>())
            {                
                if( pb.Height > 1.5 && pb.cPolje.Contains("IzvodiSe") == false && pb.cEnDis.Trim() !="D")
                {
                    //Console.WriteLine(pb.cPolje);
                    c1 = pb.IME;
                    if (pb.Vrednost.Trim() != "")
                    {
                        if ((pb.cTip > 2 && pb.cTip < 8) || pb.cTip == 13 || (pb.cTip > 18 && pb.cTip < 22) || pb.cTip == 26)  // numericka polja
                        {
                            if (pb.cTip == 3 || pb.cTip == 26)
                            {
                                if (Convert.ToDouble(pb.Vrednost) > 1)
                                    WWhere = WWhere + c1 + " = " + pb.Vrednost + " AND ";
                                else
                                if (Convert.ToDouble(pb.Vrednost.Replace(".", "")) > 1)
                                    WWhere = WWhere + c1 + " = " + pb.Vrednost.Replace(".", "") + " AND ";
                            }                      
                        }
                        else // nisu numericka polja
                        {
                            if (pb.cTip == 8 || pb.cTip == 9)
                            {
                                //WWhere = WWhere + c1 + " = " + "'" + pb.Vrednost + "'" + " AND ";  //BORKA UMRTVILA JER MI IZ NEPOZNATOG RAZLOGA U DATUM PUNI DANASNJI DATUM
                            }
                            else
                            {
                                if (pb.cTip == 24) { }
                                else
                                {
                                    if (pb.cIzborno.Trim() != "" && Convert.ToInt32(pb.ID) > 1)
                                    {
                                        if (pb.cIzborno == pb.cTabela)
                                            sql = "select alijaspolja from recnikpodataka where dokument = '" + NazivKlona + "'" + " and alijaspolja = 'ID_" + pb.cAlijasTabele + "'";
                                        else
                                            sql = "select alijaspolja from recnikpodataka where dokument ='" + NazivKlona + "'" + " and alijaspolja =  'ID_" + pb.cIzborno + "'";

                                        DataTable t = db.ReturnDataTable(sql);
                                        if (t.Rows.Count != 0)
                                            WWhere = WWhere + t.Rows[0]["alijaspolja"].ToString() + "=" + pb.ID + " AND ";
                                        else
                                            WWhere = WWhere + c1 + " = " + "'" + pb.Vrednost.Trim() + "'" + " AND ";

                                    }
                                    else
                                    {
                                        if (filter == "P")
                                            WWhere = WWhere + c1 + " LIKE " + "'" + pb.Vrednost.Trim() + "N%'" + " AND ";
                                        else
                                        {
                                            filter = "S";
                                            WWhere = WWhere + c1 + " LIKE " + "N'%" + pb.Vrednost.Trim() + "%'" + " AND ";
                                        }

                                    }
                                }
                            }
                        }                        
                    }
                }
            }

            if (WWhere.Contains("LIKE") == true)
            {
                clsOperacije op = new clsOperacije();
                WWhere = op.AsciiKarakteri(WWhere);
            }

            if (WWhere.Trim() != "")
                WWhere = WWhere.Substring(0, WWhere.Length - 4);
            else
            {
                MessageBox.Show("Niste zadali uslov za pregled!");
                PripremaidDokumentaZaPregled = "KRAJ";
                return PripremaidDokumentaZaPregled;
            }

            if (dokje == "P" && uupit.ToUpper().Contains("WHERE") == true)
                PripremaidDokumentaZaPregled = uupit.Trim() + " AND " + WWhere + " " + KrajUpita;
            else
                PripremaidDokumentaZaPregled = uupit.Trim() + " where " + WWhere + " "+KrajUpita ;

            if (imedokumenta == "Dokumenta")
            {
                IdDokView = "0";
                PripremaidDokumentaZaPregled = PripremaidDokumentaZaPregled + " AND nazivorg like '" + Program.imeFirme + "%'" + " AND YEAR(Datum)>="+Program.mGodina.ToString() + " ORDER BY  ID_DokumentaTotali desc";
                Console.WriteLine(PripremaidDokumentaZaPregled);
            }

            if (dokje == "D")
            {
                IdDokView = "0";
                PripremaidDokumentaZaPregled = PripremaidDokumentaZaPregled + " AND YEAR(Datum) >= "+Program.mGodina.ToString()+ " order by IdDokumentZapregled desc";
                //Console.WriteLine(PripremaidDokumentaZaPregled);
                DataTable kt = db.ReturnDataTable(PripremaidDokumentaZaPregled);
                kk = kt.Rows.Count;
            }
            Console.WriteLine(PripremaidDokumentaZaPregled);

            return PripremaidDokumentaZaPregled;
        }
    }
}
