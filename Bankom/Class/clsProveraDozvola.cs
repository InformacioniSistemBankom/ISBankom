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
    class clsProveraDozvola
    {
        Boolean proveragodine = false;
        string oggod = "0";
        string dokument = "";
        string idstablo = "";
        string IDDok = "";
        string DokumentJe = "";
        Form form1 = new Form();
        Boolean provera = false;
        Boolean ZakljucenaGodina = false;
        string pStatus = "1";           //nije proknjizen

       

        public Boolean ProveriDozvole(String pdokument, string pidstablo, string pIDDok, string pDokumentJe)
        {
           
            if (pdokument.Trim() == "")
            {
                return false;
            }

            int idfirme = Program.idFirme; 

            string idke = Program.idkadar.ToString();

            ZakljucenaGodina = false;
            form1 = Program.Parent.ActiveMdiChild;
            string ssel = "";
            dokument = pdokument;
            idstablo = pidstablo;
            IDDok = pIDDok;
            DokumentJe = pDokumentJe;
            DataBaseBroker db = new DataBaseBroker();

            if (DokumentJe == "D" || dokument == "Dokumenta")
            {
                if (DokumentJe == "D")
                {
                    ssel = "Select  OgraniceneGodine From dokumentaStablo WHERE ID_DokumentaStablo = " + (idstablo).ToString();
                }
                else
                {
                    ssel = "Select OgraniceneGodine From dokumentaStablo WHERE Naziv='" + dokument + "'";
                }

                DataTable ts = new DataTable();
                ts = db.ReturnDataTable(ssel);
                if (ts.Rows.Count > 0)
                {
                    if (ts.Rows[0]["OgraniceneGodine"].ToString() == "1")   // ogranicene godine//iz tabele godine vrsi saznajemo koje su dozvoljene godine
                    {
                        proveragodine = true; // vrsi se provera godine dokumenta           
                    }
                }
            }

            clsSettingsButtons sb = new clsSettingsButtons();
            sb.ToolBarItemsEnDis();  //podesi enable i disable booton - u toolbar - u

            string SStatus = "";
            string opis = "";
            string idorg = Convert.ToString(Program.idOrgDeo);  //"11";  
            if (dokument == "Dokumenta" || DokumentJe == "D")
            {
                // provera statusa dokumenta u vezi sa knjizenjem
                DataTable tsst = new DataTable(); // tabela vezana za status
                ssel = "select Proknjizeno,Opis from dokumentatotali WITH(NOLOCK) where id_dokumentatotali=" + IDDok;
                tsst = db.ReturnDataTable(ssel);
                if (tsst.Rows.Count > 0)
                {
                    if (tsst.Rows[0]["Proknjizeno"].ToString() != null)
                    {
                        SStatus = tsst.Rows[0]["Proknjizeno"].ToString();
                    }
                    if (tsst.Rows[0]["Opis"] != null)
                    {
                        opis = tsst.Rows[0]["Opis"].ToString();
                    }
                    if (SStatus == "Storniran")
                    {
                        provera = true;
                        return provera;
                    }
                }
            }
//// provera dozvola iz tabele Grupa                       
             DataTable td = new DataTable(); // tabela Grupa  vezana za dozvole
             string sel = " SELECT naziv as Naziv,"
            + "max(Unos) as Unos,max(Izmena) as Izmena,max(Brisanje) as Brisanje,max(Storno) as Storno,max(Pregled) as Pregled,max(klik) as klik,max(knjizenje)as knjizenje,max(Proknjizen) as Proknjizen,max(Potpis) as Potpis, "
            + "max(Plati) as Plati FROM  Grupa WITH (NOLOCK)  WHERE  ((ID_OrganizacionaStruktura=" + idorg + " AND ID_KadrovskaEvidencija=1 ) OR  "
            + "(ID_OrganizacionaStruktura=1 AND ID_KadrovskaEvidencija=" + idke + " ))  AND Naziv = '" + dokument + "'group by naziv order by naziv";
           td = db.ReturnDataTable(sel);
////Ako dokument postoji u dozvolama
           if (td.Rows.Count > 0)
           {
                if (td.Rows[0]["klik"].ToString() == "0")
                {
                    provera = false;
                }
                else
                {
                    provera = true;
                }               

                if (DokumentJe == "S" && dokument == "Dokumenta")
                {                    
                    if (SStatus == "Proknjizen")
                    {
                        if (td.Rows[0]["storno"].ToString() == "1")
                        {
                            Program.Parent.ToolBar.Items["Sstorno"].Enabled = true;

                            
                        }
                        else
                        {
                            Program.Parent.ToolBar.Items["Sstorno"].Enabled = false;
                         
                        }
                    }
                }       

                if (td.Rows[0]["izmena"].ToString() == "1")
                {
                    Program.Parent.ToolBar.Items["Iizmena"].Enabled = true;
                  
                }
                else
                {
                    Program.Parent.ToolBar.Items["Iizmena"].Enabled = false;
                
                }

                if (td.Rows[0]["unos"].ToString() == "1")
                {
                    Program.Parent.ToolBar.Items["Uunos"].Enabled = true;

                }
                else
                {
                    Program.Parent.ToolBar.Items["Uunos"].Enabled = false;
                }
                if (td.Rows[0]["Brisanje"].ToString() == "1")
                {
                    Program.Parent.ToolBar.Items["Bbrisanje"].Enabled = true;
               
                }
                else
                {
                    Program.Parent.ToolBar.Items["Bbrisanje"].Enabled = false;
                 
                }

                if ((SStatus.IndexOf("tvaranje") > 0) || (idstablo == "24"))
                {
                    provera = true;
                }              
             

                if (SStatus != "Proknjizen")                 //'STATUS NIJE PROKNJIZEN pStatus=1
                {
                    if (SStatus == "Odobren" || SStatus == "Potpisan")
                    {
                        if (td.Rows[0]["potpis"].ToString() == "0")
                        {
                            pStatus = "0";
                        }
                    }                 

                    if (td.Rows[0]["Knjizenje"].ToString() == "0")   // NIJE DOZVOLJENO KNJIZENJE ONEMOGUCAVAMO BUTTON ZA KNJIZENJE
                    {
                        //Program.Parent.ToolBar.Items["Kknjzi"].Visible = false;
                        //Program.Parent.ToolBar.Items["Kknjzi"].Enabled = false;
                    }
                    else                                      //KORISNIK IMA PRAVO DA KNJIZI OMOGUCAVAMO BUTTON ZA KNJIZENJE
                    {

                        Program.Parent.ToolBar.Items["Kknjzi"].Visible = true;
                        Program.Parent.ToolBar.Items["Kknjzi"].Enabled = true;
                       
                    }
                    if (td.Rows[0]["potpis"].ToString() == "0")   // NIJE DOZVOLJENA IZMENA STATUSA ONEMOGUCAVAMO BUTTON ZA PROMENU STATUSA
                    {
                        Program.Parent.ToolBar.Items["Oodobri"].Visible = false;
                        Program.Parent.ToolBar.Items["OOdobri"].Enabled = false;
                     
                    }
                    else                                      ///KORISNIK IMA PRAVO DA MENJA STATUS DOKUMENTA OMOGUCAVAMO BUTTON ZA PROMENU STATUSA
                    {
                        Program.Parent.ToolBar.Items["Oodobri"].Visible = true;
                        Program.Parent.ToolBar.Items["OOdobri"].Enabled = true;
                    }

                }
                else  ////'STATUS JESTE PROKNJIZEN
                {
                    if (td.Rows[0]["proknjizen"].ToString() == "1") { }    // 'Or ProveraGodine = False Then  'prijavljenom USER-u dozvoljen rad sa proknjizenim dokumentima
                                                                           ////  ostaje pStatus = "1" dokument nije proknjizen;ponasamo se tako jer imamo dozvolu za rad sa proknjizenim dokumentima

                    else                           ///rsDozvola!Proknjizen =0 prijavljeno USER-u nije dozvoljen rad sa proknjizenim dokumentima
                    {
                        pStatus = "0";               // jeste proknjizen dokument pStatus=0

                        if (DokumentJe == "S" )
                            if (dokument == "Dokumenta")  // jesu dokumenta
                            {
                                 Program.Parent.ToolBar.Items["Iizmena"].Enabled = false;
                                 Program.Parent.ToolBar.Items["Bbrisanje"].Enabled = false;
                                 Program.Parent.ToolBar.Items["Uunos"].Enabled = true;
                               ///// promenjeno na True da bi se mogao izabrati UNOS iako je podignut proknjizen dokument
                                if (form1.Controls["Ooperacija"].Text == "Unos")
                                 {
                                    pStatus = "1";                             ///// vracamo status da nije proknjizen
                                 }
                            }
                            else                                            ///// nisu dokumenta nego ostali sifarnici
                            {
                                 Program.Parent.ToolBar.Items["Iizmena"].Enabled = false;
                                 Program.Parent.ToolBar.Items["Bbrisanje"].Enabled = false;
                                 Program.Parent.ToolBar.Items["Uunos"].Enabled = false;
                            
                            }
                    }
                }
                //POCETAK dodatni dugmici uslovljeni otvorenim dokumentom
                if (DokumentJe == "D")
                {
                    switch (dokument)
                    {
                        case "Nalog1450":
                            if (td.Rows[0]["plati"].ToString() == "1")
                            {
                                Program.Parent.ToolBar.Items["Pplati"].Visible = true;
                                Program.Parent.ToolBar.Items["Pplati"].Enabled = true;
                       
                            }

                            else
                            {
                                Program.Parent.ToolBar.Items["Pplati"].Visible = false;
                                Program.Parent.ToolBar.Items["Pplati"].Enabled = false;
                             
                            }
                            break;
                        case "PDVPredracun":
                        case "InoPredracun":
                        case "PDVPonuda":
                        case "KonacniRacun":
                        case "InoRacun":
                        case "LotOtpremnica":
                            Program.Parent.ToolBar.Items["Kkalki"].Visible = true;
                            Program.Parent.ToolBar.Items["Kkalki"].Enabled = true;
                         
                            if (dokument == "InoRacun" || dokument == "LotOtpremnica")
                            {
                                Program.Parent.ToolBar.Items["Pporeklo"].Visible = true;
                                Program.Parent.ToolBar.Items["Pporeklo"].Enabled = true;
                            
                            }
                            if (dokument == "PDVPredracun")
                            {
                                Program.Parent.ToolBar.Items["PpredlogCena"].Enabled = true;
                                Program.Parent.ToolBar.Items["PpredlogCena"].Visible = true;
                              
                                if (td.Rows[0]["potpis"].ToString() == "1")
                                {

                                    Program.Parent.ToolBar.Items["Oodobri"].Visible = true;
                                    Program.Parent.ToolBar.Items["Oodobri"].Enabled = true;
                                  
                                }
                                else
                                {
                                    Program.Parent.ToolBar.Items["Oodobri"].Visible = false;
                                    Program.Parent.ToolBar.Items["Oodobri"].Enabled = false;
                              
                                }
                            }
                            break;
                        case "Dobit":
                            Program.Parent.ToolBar.Items["Pppppd"].Visible = true;
                            Program.Parent.ToolBar.Items["Pppppd"].Enabled = true;
                         
                            break;
                        case "PotencijalKupca":
                        case "PotencijalKupcaFsh":
                            Program.Parent.ToolBar.Items["Ppotencijal"].Visible = true;
                            Program.Parent.ToolBar.Items["Ppotencijal"].Enabled = true;
                         
                            break;
                        case "KonacniUlazniRacun":
                            Program.Parent.ToolBar.Items["Kkalku"].Visible = true;
                            Program.Parent.ToolBar.Items["Kkalku"].Enabled = true;
                            Program.Parent.ToolBar.Items["Oorigin"].Visible = true;
                            Program.Parent.ToolBar.Items["Oorigin"].Enabled = true;
                    
                            break;
                        case "PDVUlazniRacunZaUsluge":
                            Program.Parent.ToolBar.Items["Oorigin"].Visible = true;
                            Program.Parent.ToolBar.Items["Oorigin"].Enabled = true;
                        
                            break;
                        case "KnjigaIzlaznePoste":
                            Program.Parent.ToolBar.Items["Kknjiga"].Visible = true;
                            Program.Parent.ToolBar.Items["Kknjiga"].Enabled = true;
                         
                            break;
                        case "Narudzbenica":
                        case "NalogZaUtovar":
                            break;
                    }  ////' KRAJ dodatni dugmici uslovljeni otvorenim dokumentem
                }
                else   //// DOKUMENTJE<>"D"
                {
                    if (DokumentJe == "S" && dokument == "Dokumenta")
                    {
                        if (idstablo == "32" || idstablo == "38" || idstablo == "300")
                        {
                            if (td.Rows[0]["potpis"].ToString() == "1")
                            {
                                Program.Parent.ToolBar.Items["Oodobri"].Visible = true;
                                Program.Parent.ToolBar.Items["Oodobri"].Enabled = true;
                                
                            }
                            else
                            {
                                Program.Parent.ToolBar.Items["Oodobri"].Visible = false;
                                Program.Parent.ToolBar.Items["Oodobri"].Enabled = false;
                            
                            }
                        }
                        else
                        {
                            Program.Parent.ToolBar.Items["Oodobri"].Visible = false;
                            Program.Parent.ToolBar.Items["Oodobri"].Enabled = false;
                 
                        }


                    }  //KRAJ DOKUMENTJE="S" && dokument == "Dokumenta"
                }     //KRAJ    DOKUMENTJE<>"D"
            }        // KRAJ POSTOJI U DOZVOLAMA
            else     ///ne postoji u dozvolama
            {
                provera = false;
            }   ////Ako postoji u dozvolama KRAJ


            if ((DokumentJe == "D") || (dokument == "NalogGlavneKnjige") || dokument == "KonacniUlazniRacun" || (DokumentJe == "S" && dokument == "Dokumenta"))
            {

                if (proveragodine == false)
                {
                    Program.Parent.ToolBar.Items["Uunos"].Enabled = true;
                    Program.Parent.ToolBar.Items["Iizmena"].Enabled = true;
                    Program.Parent.ToolBar.Items["Bbrisanje"].Enabled = true;
               
                }

                //string pDatum;
                if (form1.Controls["Ooperacija"].Text != "Pregled")
                {

                    //string.Format("{0:dd.MM.yy}", System.DateTime.Now.ToString());

                    if (form1.Controls["lDatum"].Text.Trim() == "")
                    {
                        provera = false;
                        provera = true;
                    }
                    else
                    {
                        DateTime dDatum = Convert.ToDateTime(form1.Controls["lDatum"].Text); // datum sa dokumenta
                        DateTime tDatum = DateTime.Now;     // systemski datum


                        ////  // Dokument je iz godine razlicite od tekuce godine i datum dokumenta je manji od datuma zakljucenja knjiga
                        /// PROVERITI SLEDECI IF NIJE DOBAR ????????????????????????????????? BORKA
                        if ((dDatum).Year != (tDatum).Year && dDatum <  Program.kDatum)
                        {
                            if (dokument == "NalogGlavneKnjige")
                            {
                                //(SStatus.IndexOf("tvaranje") > 0
                                if (opis.IndexOf("95") > 0)   //ObracunKredita moze da se knjizi nezavisno od godine
                                {
                                    ////                  With frmMain.Toolbar2
                                    Program.Parent.ToolBar.Items["Uunos"].Enabled = false;
                                    Program.Parent.ToolBar.Items["Iizmena"].Enabled = true;
                                    Program.Parent.ToolBar.Items["Bbrisanje"].Enabled = false;
                            
                                    provera = true;
                                }
                                else                             //Nije ObracunKredita a jeste nalogglavneknjige iz zakljucene godine
                                {
                                    ////                   With frmMain.Toolbar2
                                    Program.Parent.ToolBar.Items["Uunos"].Enabled = false;
                                    Program.Parent.ToolBar.Items["Iizmena"].Enabled = false;
                                    Program.Parent.ToolBar.Items["Bbrisanje"].Enabled = false;
                             
                                    ZakljucenaGodina = true;
                                    provera = true;
                                }
                            }
                            else                                 //nije nalogglavneknjige i nije obracunkredita a dokument se odnosi na predhodne godine
                            {
                                if (dokument == "Dokumenta" || DokumentJe == "D")
                                    form1.Controls["Ooperacija"].Text = "";
                                ZakljucenaGodina = true;
                                provera = false;
                            }
                            if (form1.Controls["Ooperacija"].Text != "")
                            {
                                form1.Controls["Ooperacija"].Text = "";
                                provera = false;
                            }
                        }// KRAJ razlicite godine
                    }// KRAJ DokumentJe="D" or (DokumentJe="S" and Dokument="Dokumenta")
                }  //KRAJ Operacijadokumenta<>"Pregled"      


                IzmeneDisableEnablePolja(dokument, DokumentJe);

                //Program.Parent.ToolBar.Refresh;

                return provera;
            }
            return provera;
        }
        ///prekidac za CommandButton-e ToolBar-a, ona sto su bila Enabled postaju Disabled, i obrnuto
        public void IzmeneDisableEnablePolja(string dokument,string dokumentje )
        {
            //   Dim rstStablo As ADODB.Recordset
            //     Dim ImePolja As String

            //     If DokumentJe = "D" || DokumentJe = "S" Then    // ima smisla samo za dokumenta i sifarnik dokumenata
            //        Set rstStablo = RsRecnikPodataka
            //        rstStablo.filter = "  TabIndex> -1 and Dokument = '" + Dokument + "'"
            //        While Not rstStablo.EOF
            //              If InStr(rstStablo!PPolje, "ID_") = 0 Then
            //                 If rstStablo!Zoom And ZakljucenaGodina = False Then
            //                    fform.Controls("ct" + rstStablo!PPolje).EnDis = True
            //                    If frmMain.Toolbar2.Buttons("cmdIzmena").Enabled = False Then frmMain.Toolbar2.Buttons("cmdIzmena").Enabled = True
            //                 Else
            //                     If form1.Controls["Ooperacija"].Text == "Pregled" Then
            //                        fform.Controls("ct" + rstStablo!PPolje).EnDis = True
            //                        fform.Controls("ct" + Trim(rstStablo!PPolje)).TabStop = True

            //                        If Not IsNull(rstStablo!Izborno) Then
            //                           fform.Controls("ct" + Trim(rstStablo!PPolje)).Visible = False
            //                           fform.Controls("ct" + Trim(rstStablo!PPolje)).Move rstStablo!levo* pOfset, rstStablo!vrh* pOfsety, rstStablo!Width* pOfset, rstStablo!Height* pOfsety
            //                           fform.Controls("ct" + Trim(rstStablo!PPolje)).Izborno = rstStablo!Izborno
            //                           fform.Controls("ct" + Trim(rstStablo!PPolje)).Visible = True
            //                        End If
            //                     Else
            //                         If pStatus = "0" Then // dokument je proknjizen
            //                            fform.Controls("ct" + rstStablo!PPolje).EnDis = False 'Not Not fform.Controls(ImePolja).EnDis ' vvvvvvvvvvvvvv
            //                         Else                  // dokument nije proknjizen
            //                             If Trim(rstStablo!StornoiUpdate) = "D" Then
            //                                fform.Controls("ct" + rstStablo!PPolje).EnDis = False
            //                             Else
            //                                fform.Controls("ct" + rstStablo!PPolje).EnDis = True
            //                             End If
            //                        End If
            //                     End If
            //                 End If
            //              End If

            //              rstStablo.MoveNext
            //        Wend
            //        Set rstStablo = Nothing
            //     End If
        }

    }
}
