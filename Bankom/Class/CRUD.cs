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
using Bankom.Class;


namespace Bankom.Class
{
    class CRUD
    {

        private string PoljeGdeSeUpisujeIId;
        private string id_redaLog;
        private string iid;
        private string iddokument;
        private string IndUpisan;
        private string IdDokumentaStablo;
        private string DokumentJe = "";
        private string dokument="";
        private Form forma;
        bool isDoIt = true;
        private string NazivKlona = "";
        DataBaseBroker db = new DataBaseBroker();
        Form Me = Program.Parent.ActiveMdiChild;
        string operacija = "";
        List<string[]> lista = new List<string[]>();
        private string TrebaProvera = "0";
        public bool DoIt(Form forma1, string iddok, string dokument1)
        {
            lista.Clear();
            forma = forma1;
            DokumentJe = forma.Controls["ldokje"].Text;
            string IndUpisan = "";
            string mGrid = "";
            string mIme = "";
            string ime = "";
            long Koliko = 0;
            string Nacinr = "";
            IdDokumentaStablo = forma.Controls["lidstablo"].Text;
            dokument = dokument1;
            operacija = forma.Controls["OOperacija"].Text;
            iddokument = Convert.ToString(((Bankom.frmChield)forma).iddokumenta);
            clsOperacije op = new clsOperacije();
            // POCETAK OBRADE SLOGOVA IZABRANOM OPERACIJOM
            string sql = "";
            DataTable t = new DataTable();
            DataTable dtt = new DataTable();
            sql = " select s.ulazniizlazni as NazivDokumenta,NacinRegistracije as nr,"
                       + " Knjizise "
                       + " from SifarnikDokumenta as s"
                       + "  Where s.naziv=@param0";
            t = db.ParamsQueryDT(sql, dokument);

            //'zapamtimo podatke sa odabranog dokumenta
            if (t.Rows.Count > 0)
            {
                Nacinr = t.Rows[0]["nr"].ToString();
                NazivKlona = t.Rows[0]["NazivDokumenta"].ToString();
            }

            sql = "Select DISTINCT u.Ime,u.Tabela,u.Upit,r.TUD from Upiti  u,RecnikPodataka as r " +
                      " WHERE NazivDokumenta =@param0 AND  TabelaVView =substring(ime,4,len(ime)-3)  AND ime like 'Uu%' Order By r.TUD";
            t = db.ParamsQueryDT(sql, NazivKlona);
            if (t.Rows.Count == 0)
            {
                isDoIt = false;
                MessageBox.Show("Greska");
                return (isDoIt); 
            }
            for (int r = 0; r < t.Rows.Count; r++)
            { 
                if (isDoIt == false) { break; }
                string tabela = t.Rows[r]["Tabela"].ToString();
                ime = t.Rows[r]["Ime"].ToString();    //// upiti.GetValue(upiti.GetOrdinal("Ime")).ToString();
                mGrid = Convert.ToString(((Bankom.frmChield)forma).imegrida);
                mIme = ime.Substring(3, ime.Length - 3);
                string Poruka = "";
                IndUpisan = "0";
                isDoIt = true;
                if (isDoIt == false) return (isDoIt);
                if (r == 0 && operacija == "BRISANJE")
                {
                    if (MessageBox.Show("Da li stvarno zelite brisanje ?", "Brisanje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return (isDoIt);
                    }
                }
                switch (operacija)
                {                 
                    case "BRISANJE":
                          PoljeGdeSeUpisujeIId = "ID_" + t.Rows[r]["Tabela"].ToString().Trim();
                          if (dokument == "Dokumenta")
                                iddokument = Convert.ToString(((Bankom.frmChield)forma).iddokumenta);

                          if (Convert.ToInt32(t.Rows[r]["TUD"]) > 0 && ((Bankom.frmChield)forma).idReda >0)   /// BORKA 27.07.20
                          {
                                isDoIt = Erase(tabela, Convert.ToString(((Bankom.frmChield)forma).idReda));
                                break;
                          }
                        break;
                    case "STORNO":
                        isDoIt = StornirajDokument();
                        if (isDoIt == false)
                            return isDoIt;
                        break;
                    case "UNOS PODBROJA":
                    case "UNOS":
                    case "IZMENA":
                        if (r == 0)
                        {
                            // PROVERE ISPRAVNOSTI PODATAKA POCETAK
                            clsProveraIspravnosti pi = new clsProveraIspravnosti();
                            isDoIt = pi.ProveraOperacija(NazivKlona);
                            if ((DokumentJe == "S" && dokument == "Dokumenta" && IdDokumentaStablo != "61") || (DokumentJe == "D" && IdDokumentaStablo != "29" || (DokumentJe == "D" && IdDokumentaStablo != "290")))
                            {                               
                                isDoIt = pi.ProveraKursa(dokument, ref Poruka);
                                if (isDoIt == false)
                                {
                                    MessageBox.Show(Poruka);
                                    break;
                                }
                            }
                            isDoIt = pi.ProveraObaveznihPolja(NazivKlona);
                            if (isDoIt == false)
                                break;

                            isDoIt = pi.ProveraSadrzaja(NazivKlona, iddokument, Convert.ToString(((Bankom.frmChield)forma).idReda), operacija, ref Poruka);
                            if (isDoIt == false)
                                break;

                            isDoIt = pi.DodatnaProvera();
                            if (isDoIt == false)
                                break;
                        }
                        // PROVERE ISPRAVNOSTI PODATAKA KRAJ

                        // OBRADA ZAGLAVLJA NISU STAVKE
                        if (t.Rows[r]["Ime"].ToString().Contains("Stavke") == false)
                        {
                            // upis zaglavlja
                            PoljeGdeSeUpisujeIId = "ID_DokumentaView";
                            iid = iddokument;
                            id_redaLog = iid;
                            DataTable tt = new DataTable();
                            // da li je vec upisano zaglavlje 0=ne postoji slog u zaglavlju,1=upisan slog u zaglavlje
                            SqlDataReader DaLiJeUpisano = db.ReturnDataReader("if not exists (select ID_DokumentaView from " + tabela + " where " +
                                      "ID_DokumentaView = " + iddok + " ) select 0 else select  1");
                            DaLiJeUpisano.Read();
                            IndUpisan = DaLiJeUpisano[0].ToString();
                            DaLiJeUpisano.Close();
                            DaLiJeUpisano.Dispose();
                        }
                        else //JESU STAVKE
                        {
                            iid = Convert.ToString(((Bankom.frmChield)forma).idReda);
                            PoljeGdeSeUpisujeIId = "ID_" + t.Rows[r]["Tabela"].ToString().Trim();
                        }

                        if (forma.Controls["OOperacija"].Text.Contains("UNOS") == true)
                        {
                            if (IndUpisan == "1")  // slog je upisan pa vrsimo update
                            {
                                isDoIt = Update(t.Rows[r]["Upit"].ToString(), t.Rows[r]["Ime"].ToString(), t.Rows[r]["Tabela"].ToString());
                            }
                            else
                            {    // slog nije upisan pa vrsimo insert                                
                                isDoIt = Insert(t.Rows[r]["Upit"].ToString(), t.Rows[r]["Ime"].ToString(), t.Rows[r]["Tabela"].ToString());
                            }
                        }
                        //if (forma.Controls["OOperacija"].Text.Contains("IZMENA") == true)
                        if(operacija=="IZMENA")
                        {
                            if (iid != "0" && iid != null && iid != "-1")
                            {
                                if (mIme == mGrid.Substring(4) && mIme.Contains("Stavke"))
                                    isDoIt = Update(t.Rows[r]["Upit"].ToString(), t.Rows[r]["Ime"].ToString(), t.Rows[r]["Tabela"].ToString());
                                else
                                {
                                    if (mIme.Contains("Stavke") == false)
                                        isDoIt = Update(t.Rows[r]["Upit"].ToString(), t.Rows[r]["Ime"].ToString(), t.Rows[r]["Tabela"].ToString());
                                }
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;                  
                }// KRAJ swich za OPERACIJU     
              //Sledeci:
            }    // KRAJ OBRADE SLOGOVA ODABRANOM OPERACIJOM
            if (isDoIt == false) return (isDoIt);// DODALA RED  BORKA 12.08.20 
            string imaTotale = "1";
            // da li postoje totali  0= ne nema totale ,1=da ima totale
            SqlDataReader DaLiImaTotale = db.ReturnDataReader("if not exists (select TotaliDaNe from RefreshGrida where Dokument= '" + NazivKlona + "' AND TotaliDaNe ='DA') select 0 else select  1");
            DaLiImaTotale.Read();
            imaTotale = DaLiImaTotale[0].ToString();
            DaLiImaTotale.Close();
            DaLiImaTotale.Dispose();
            if (imaTotale == "1")
            {
                if (DokumentJe == "S")
                {
                    if( operacija=="UNOS") 
                        sql = "EXECUTE TotaliZaDokument " + NazivKlona + "," + "'tttt'";
                    else
                        sql = "Execute TotaliZaDokument'" +NazivKlona + "'," + iddok.ToString();
                }
                else
                    sql = "Execute TotaliZaDokument '" + NazivKlona + "', " + iddok.ToString();                
                lista.Add(new string[] { sql, "", "", "", "" });
                lista.ToArray();
            }
            if (DokumentJe == "D")
            {
                // Field kontrola = (Field)forma.Controls["NazivSkl"];
                //if (kontrola != null) // dokument je robni dokument
                //{
                sql = "select polje from recnikpodataka where dokument=@param0 and polje=@param1";
                dtt = db.ParamsQueryDT(sql, NazivKlona, "NazivSkl");
                if (dtt.Rows.Count > 0)
                {
                     sql = "Execute CeneArtikalaPoSkladistimaIStanje " + iddok.ToString();
                    lista.Add(new string[] { sql, "", "", "", "" });
                    lista.ToArray();

                    sql = "Execute StanjeRobePoLotu " + iddok.ToString();
                    lista.Add(new string[] { sql, "", "", "", "" });
                    lista.ToArray();

                    sql = "select oorderby as TrebaProvera from recnikpodataka where dokument=@param0 and oorderby>0 and oorderby<5";
                    DataTable dt2 = db.ParamsQueryDT(sql, dokument);
                    if (dt2.Rows.Count > 0) TrebaProvera = dt2.Rows[0]["TrebaProvera"].ToString();

                    if (TrebaProvera != "0")
                    {
                        //lista.Add(new string[] { str, strParams, strTabela, dokType, idreda });
                        sql = "Execute stanje";
                        lista.Add(new string[] { sql, "", NazivKlona,"" , iddok.ToString() });
                        lista.ToArray();
                    }
                }
            }
            // u promenljivoj lista nalaze se podaci za odabranu operaciju i operacije realizujemo naredbom koja sledi 
            if (lista.Count > 0)
            {

                string rezultat = db.ReturnSqlTransactionParamsFull(lista);
                Console.WriteLine(rezultat);
                if (rezultat != "" )  /////??????????????????? BORKA NE VALJA ZA INSERT TEBA VIDETI KAKO TO RESITI
                {
                    if (op.IsNumeric(rezultat) == true)
                    {
                        if (dokument1 == "Dokumenta")
                        {
                            iddok = rezultat.Trim();
                        }
                    }
                    else
                    {
                        if (rezultat.Trim() != "")
                        {
                            MessageBox.Show(rezultat);
                            isDoIt = false;
                            return isDoIt;
                        }
                    }                    
                }
            }
            if (isDoIt == false) { return (isDoIt); }
            
            CreateLog(forma, iddok, Convert.ToString(((Bankom.frmChield)forma).idReda), dokument, operacija, ime);
            clsAzuriranja az = new clsAzuriranja();
            az.DodatnaAzuriranja(dokument1, iddok);
            az.DodatnaAzuriranjaPosleUnosa(dokument1, iddok);

            if (forma.Controls["OOperacija"].Text.Contains("IZMENA") == true)
            {
                if (dokument1 == "KonacniUlazniRacun" || dokument1 == "PDVUlazniRacunZaUsluge")
                {
                    if (((Bankom.frmChield)forma).idReda > 0)// samo ako menjamo stavke
                    {
                        clsObradaKalkulacije okal = new clsObradaKalkulacije();
                        okal.RasporedTroskova(Convert.ToInt64(iddok), " ", " ", " ", "");
                       
                    }
                }
            }      


            if ((operacija.Contains("UNOS") == true || operacija == "IZMENA") && DokumentJe == "D")
            {
                if (dokument == "OpisTransakcije") //jovana
                {
                    clsObradaOpisaTransakcije cOT = new clsObradaOpisaTransakcije();
                    isDoIt = cOT.UpisOpisaTransakcijeUTransakcije(forma, iddokument, iid);
                }
            }

            //jovana 25.11.20
            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + NazivKlona, "IdDokument:" + iddok);
            db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:" + iddok);
            db.ExecuteStoreProcedure("StanjeRobePoLotu", "IdDokView:" + iddok);
            if (isDoIt ==false) return (isDoIt);
            ((Bankom.frmChield)forma).idReda = -1;
            return isDoIt;
        } // KRAJ za Doit
        public bool Insert(string upit, string ime, string tabela)
        {
            string sselect;
            string IInsert;
            string SSelectZaInsert;
            string EExecute = "";
            string UVView;
            string field;
            string mSifra = null;
            //int mSif = 0;
            int k = 0;
            string BrojDokumenta = "";
            string datum = "";
            int ParRb = 0;
            string[] separators = new[] { "," };
            string pproknjizeno = "";
            int mesecporeza = 0;
            sselect = upit.Substring(upit.IndexOf("SELECT") + 6, upit.IndexOf("FROM") - 6);
            string[] filds = sselect.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            IInsert = "";
            SSelectZaInsert = "";
            UVView = ime.Substring(3, ime.Length - 3);

            if (dokument == "Dokumenta")
            {
                datum = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost;
                clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                BrojDokumenta = os.KreirajBrDokNovi(ref ParRb, datum, Convert.ToInt32(IdDokumentaStablo), operacija);
                ////////////////////////////////////////////////////// BORKA 27.03.20 pocetak
                clsMesecPoreza mp = new clsMesecPoreza();
                mesecporeza = mp.ObradiMesecPoreza(datum);
                DataTable t = new DataTable();
                string sql = "Select s.KnjiziSe,s.Naziv from SifarnikDokumenta as s,DokumentaStablo as d  where d.ID_DokumentaStablo=" + IdDokumentaStablo;
                sql += " AND s.naziv =d.Naziv";
                t = db.ReturnDataTable(sql);
                if (t.Rows.Count > 0)
                {
                    string knjizise = t.Rows[0]["KnjiziSe"].ToString();
                    string NazivDokumenta = t.Rows[0]["Naziv"].ToString();
                    if (knjizise.ToUpper().Trim() == "N") pproknjizeno = "NeKnjiziSe";
                    else pproknjizeno = "NijeProknjizeno";
                }
                /////////////////////////////////////////////////////////////kraj
            }

            for (k = 0; k < filds.Length; k++)
            {
                field = filds[k].Trim();

                if (field.ToUpper() == "TTIME" || field.ToUpper() == "UUSER")
                    goto sledeci;

                if (field.Contains("ID_") == false)   // nije ID-polje
                {
                    foreach (var pb in forma.Controls.OfType<Field>().Where(g => String.Equals(g.cSegment, UVView)))
                    {
                        if ((field == pb.cPolje && pb.cTabela == tabela && pb.cTabelaVView == UVView) || field == pb.cPolje && pb.cTabelaVView == UVView)
                        {
                            if (dokument == "Dokumenta")
                            {
                                if (pb.cPolje == "BrojDokumenta") { pb.Vrednost = BrojDokumenta; }
                                if (pb.cPolje == "MesecPoreza") { pb.Vrednost = mesecporeza.ToString(); }
                                if (pb.cPolje == "Proknjizeno") { pb.Vrednost = pproknjizeno; }
                                if (pb.cPolje == "RedniBroj") { pb.Vrednost = (ParRb).ToString(); }
                            }
                            if (dokument == "Artikli" && field == "StaraSifra")
                            {
                                if (pb.Vrednost == "")
                                    mSifra = pb.Vrednost;
                                IInsert = IInsert + field + ";";
                                SSelectZaInsert = SSelectZaInsert + mSifra + ";";
                                goto sledeci;
                            }
                            else
                            {
                                if (pb.IME == "DatumIstupa" && string.IsNullOrEmpty(pb.Vrednost.Trim())) 
                                { goto sledeci; }
                                IInsert = IInsert + field + ";";
                                SSelectZaInsert = SSelectZaInsert +  PrevediPolje(pb.Vrednost, pb.TipKontrole.ToString()) + ";";
                                goto sledeci;
                            }
                        }
                    }   // foreach pb
                }  // kraj nije ID polje
                else   // jeste ID_
                {
                    if (field == "ID_DokumentaView")
                    {
                        IInsert = IInsert + field + "; ";
                        SSelectZaInsert = SSelectZaInsert + iddokument + "; ";
                        goto sledeci;
                    };
                    if (field == "ID_KadrovskaEvidencija")
                    {
                        IInsert = IInsert + field + "; ";
                        SSelectZaInsert = SSelectZaInsert + Program.idkadar.ToString() + "; ";
                        goto sledeci;
                    }
                    if (field == "ID_OrganizacionaStrukturaView")
                    {
                        IInsert = IInsert + field + "; ";
                        SSelectZaInsert = SSelectZaInsert + Program.idOrgDeo.ToString() + "; ";
                        goto sledeci;
                    }

                    foreach (var pb in forma.Controls.OfType<Field>().Where(g => String.Equals(g.cSegment, UVView)))
                    {
                        //if (field == "ID_LikvidacijaDokumenta")
                        //    Console.WriteLine(pb.IME);
                        if (pb.cIzborno.Trim() != "")
                        {
                            if ((field.Substring(3) == pb.cAlijasTabele && field.Substring(3) == pb.cIzborno && pb.cIzborno != pb.cTabela)
                                || (pb.cIzborno.ToUpper() == field.Substring(3).ToUpper() && pb.cIzborno != pb.cTabela)
                                || (field.Substring(3) == pb.cAlijasTabele && pb.cIzborno == pb.cTabela))
                            {
                                IInsert = IInsert + field + "; ";
                                SSelectZaInsert = SSelectZaInsert + pb.ID + "; ";
                                goto sledeci;
                            }
                        }
                        else
                        {
                            if (field.Substring(3) == pb.cAlijasTabele && pb.cDokument == "Dokumenta")
                            {
                                IInsert = IInsert + field + "; ";
                                SSelectZaInsert = SSelectZaInsert + pb.ID + "; ";
                                goto sledeci;
                            }
                        }
                    } // kraj foreach pb
                }; // Kraj jeste ID

            sledeci:
                Console.WriteLine(sselect);
                sselect = sselect.Substring(sselect.IndexOf(";") + 1, sselect.Length - sselect.IndexOf(";") - 1);
            } // kraj foreach fild

            if (dokument == "ArtikliOsobine1")
            {
                string aa = dokument;
                if (IInsert.Contains(aa) == false)
                {
                    IInsert = IInsert + "ID_Artikli;";
                    SSelectZaInsert = SSelectZaInsert + iid + "; ";
                }
            }

            if (DokumentJe == "S")
            {
                string stablo = dokument + "Stablo";
                if (IInsert.Contains(stablo) == false)
                {
                    IInsert = IInsert + "ID_" + stablo + ";";
                    switch (dokument)
                    {
                        case "Artikli":
                        case "Komitenti":
                            Field kontrola = forma.Controls.OfType<Field>().FirstOrDefault(n => n.cPolje == "Grupa");
                            if (kontrola.ID != "1")
                                IdDokumentaStablo = Convert.ToString(kontrola.ID);
                            break;
                    }
                    SSelectZaInsert = SSelectZaInsert + IdDokumentaStablo + "; ";
                }
            }

            if (dokument == "KursnaLista")
            {
                if (IInsert.Contains(datum) == false)
                {
                    IInsert = IInsert + "Datum,Verzija;";
                    SSelectZaInsert = SSelectZaInsert + "'" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost + "'; ";
                    SSelectZaInsert = SSelectZaInsert + "'" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Verzija").Vrednost + "'; ";
                }
            }

            if (dokument != "Imenik")
            {
                IInsert = IInsert + "UUser;TTime";
                SSelectZaInsert = SSelectZaInsert + Program.idkadar.ToString() + "; ";
                SSelectZaInsert = SSelectZaInsert + "" + DateTime.Now + ""; //jovana 27.04.20

            }
            else
            {
                SSelectZaInsert = SSelectZaInsert.Substring(0, SSelectZaInsert.Length - 1);
                if (IInsert.Trim() != "")
                    IInsert = IInsert.Substring(0, IInsert.Length - 1);
            }

            if (IInsert.Trim() != "")
                EExecute = EExecute.Replace("Totali", " ");
            EExecute = EExecute + " INSERT INTO " + tabela + "(" + IInsert + ") values(" + SSelectZaInsert + ")";
            Console.WriteLine(EExecute);

            string parametri = "";
            string EExec = CreateExecWithParams(IInsert, SSelectZaInsert, " INSERT INTO", tabela, ref parametri);
            Console.WriteLine(EExec);
            Console.WriteLine(parametri);
            lista.Add(new string[] { EExec, parametri, tabela, "D", "" });
            lista.ToArray();
            return isDoIt;
        }
        
       
        public bool Update(string upit, string ime, string tabela)
        {
            isDoIt = true;
            string sselect;            
            string ffrom;
            string UVView;
            string field;    
            int k = 0;
            string naredba = "";
            string param = "";
            string paramvred = "";
            string[] separators = new[] { "," };           

            sselect = upit.Substring(upit.IndexOf("SELECT") + 6, upit.IndexOf("FROM") - 6).Trim() + ",";
            string[] filds = sselect.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            ffrom = upit.Substring(upit.IndexOf("FROM") + 4);           
            naredba = "UPDATE " + ffrom + " SET  ";
            UVView = ime.Substring(3, ime.Length - 3);
            string uslov = "";
            for (k = 0; k < filds.Length; k++)
            {
                field = filds[k].Trim();
                string fpolje = field.Substring(3).Trim();

                if (field.ToUpper() == "TTIME" || field.ToUpper() == "UUSER")
                    goto SledecePolje;

                if (field.Contains("ID_") == true)   // jeste ID-polje
                {
                    if (field == "ID_DokumentaView")
                    {  
                        param = "@param" + Convert.ToString(k).Trim();
                        naredba += "[" + field + "]=" + param + ",";
                        paramvred += param + "=" + iddokument + "`";                       

                        goto SledecePolje;
                    }

                    foreach (var pb in forma.Controls.OfType<Field>().Where(g => String.Equals(g.cSegment, UVView)))
                    {
                        if (pb.cTabelaVView == UVView)
                        {
                            string aa = pb.IME;
                            string bb = pb.cPolje;
                            if (pb.IME == "Predhodni")
                            {
                                Console.WriteLine(pb.ID);
                            }
                            if (pb.cIzborno.Trim() != "")
                            {
                                if ((fpolje == pb.cAlijasTabele && fpolje == pb.cIzborno && pb.cIzborno != pb.cTabela)
                                    || (pb.cIzborno.ToUpper() == fpolje.ToUpper() && pb.cIzborno != pb.cTabela)
                                    || (fpolje == pb.cAlijasTabele && pb.cIzborno == pb.cTabela))
                                {
                                     param = "@param" + Convert.ToString(k).Trim();
                                     paramvred += param + "=" + pb.ID + "`";
                                     naredba += "[" + field + "]=" + param +",";                                     

                                    goto SledecePolje;
                                }
                            }
                            else
                            {
                                if (fpolje == pb.cAlijasTabele && pb.cDokument == "Dokumenta")
                                {
                                    param = "@param" + Convert.ToString(k).Trim();
                                    naredba += "[" + field + "]=" + param +",";
                                    paramvred +=param+"=" + pb.ID + "`";
                                    goto SledecePolje;
                                }
                            }
                        } // kraj pb.cTabelaVView == UVView
                    }//kraj foreach pb                  
            } //kraj  polje sadrzi ID

                else  // nije ID polje
                {
                    foreach (var pb in forma.Controls.OfType<Field>().Where(g => String.Equals(g.cSegment, UVView)))
                    {
                        if ((field == pb.cPolje && pb.cTabela == tabela && pb.cTabelaVView == UVView) || field == pb.IME && pb.cTabelaVView == UVView)
                        {
                            // if (string.IsNullOrEmpty(mg)) { return; }
                            if (pb.IME=="DatumIstupa" && string.IsNullOrEmpty(pb.Vrednost.Trim()))  { goto SledecePolje;}
                            param = "@param" + Convert.ToString(k).Trim();
                            naredba += "[" + field + "]=" + param +",";
                            paramvred +=param+"=" + PrevediPolje(pb.Vrednost, pb.TipKontrole.ToString()) + "`";

                            goto SledecePolje;
                        }
                    } // kraj foreach pb
                }   // kraj nije ID polje

            SledecePolje:
                sselect = sselect.Substring(sselect.IndexOf(",") + 1, sselect.Length - sselect.IndexOf(",") - 1).Trim();
            } //  kraj sva polja iz upita 

            if (DokumentJe == "S")
            {
                string stablo = dokument + "Stablo";
                if (naredba.Contains(stablo) == false)
                {
                    switch (dokument)
                    {
                        case "Artikli":
                        case "Komitenti":
                            Field kontrola = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Grupa");
                            if (kontrola.ID != "1")                            
                                IdDokumentaStablo = kontrola.ID;                              
                            k++;                             
                            param = "@param" + Convert.ToString(k).Trim();
                            naredba += "[ID_"+ dokument + "Stablo]=" + param + ",";
                            paramvred += param + "=" + IdDokumentaStablo + "`";                               

                            break;
                    }
                }
            }

            if (dokument != "Imenik" && dokument != "Dokumenta")
            {
                k++;
                param = "@param" + Convert.ToString(k).Trim();
                naredba += "[" + "Uuser" + "]=" + param + ",";
                paramvred += param + "="+ Program.idkadar.ToString() + "`";
                k++;
                param = "@param" + Convert.ToString(k).Trim();
                naredba += "[" + "Ttime" + "]=" + param;
                paramvred += param + "=" + DateTime.Now + "`";                
            }
            else
            {
                naredba = naredba.Substring(0, naredba.Length - 1);
            }
            uslov = " WHERE " + PoljeGdeSeUpisujeIId + " = " + iid;
                  
            
            //List<string[]> lista = new List<string[]>();          
         
            naredba += uslov;
            Console.WriteLine(naredba);
            Console.WriteLine(paramvred);

            lista.Add(new string[] { naredba, paramvred, tabela, "D", iddokument });
            lista.ToArray();
            return isDoIt;
        }
        public bool Erase(string tabela, string idreda)
        {
            DataTable tt = new DataTable();
            DataTable t = new DataTable();
            string strParams = "";            
            string strTabela = "";
            string dokType = "";
            string sql = "";
            string str = "";
            string imaTotale = "1";
            bool Erase = true;
            dokType = "";
            string iddk = "1";
            string iid = "";
            string IdStablo = Convert.ToString(((Bankom.frmChield)forma).idstablo);
            string idDokument = Convert.ToString(((Bankom.frmChield)forma).iddokumenta);
            string NazivDokumenta = "";

            // brisemo red iz tabele
            iid =idreda;
            strTabela = tabela;
            strParams = "@param1=" + idreda.ToString() + "`";
            str = "DELETE from " + tabela + " where [ID_" + tabela + "] = @param1";
            lista.Add(new string[] { str, strParams, strTabela, dokType, idreda });
            lista.ToArray();           

            switch (dokument)
            {
                    case "Dokumenta":
                        //brisemo slogove iz pripadajucih tabela to su tabele koje pripadaju dokumentu
                        sql = "Select s.UlazniIzlazni as Naziv from dokumentaStablo as d,SifarnikDokumenta as s where d.naziv=s.Naziv and id_dokumentastablo= @param0";
                        tt = db.ParamsQueryDT(sql,IdStablo);
                        NazivDokumenta = tt.Rows[0]["Naziv"].ToString();
                        sql = "select Distinct Tabela from upiti where ime like '%Uuu%' and NazivDokumenta=@param0";                      
                        tt = db.ParamsQueryDT(sql, NazivDokumenta);
                        if (tt.Rows.Count > 0)
                        {
                            for (int i=0;i<tt.Rows.Count; i++)
                            {
                                dokType = "";
                                strTabela = tt.Rows[0]["Tabela"].ToString();
                                strParams = "@param1=" + idDokument + "`";
                                strTabela = tt.Rows[i]["Tabela"].ToString();
                                str = "DELETE from " + strTabela + " Where [ID_DokumentaView]= @param1";
                                lista.Add(new string[] { str, strParams, strTabela, dokType, idreda });
                                lista.ToArray();            
                            } 

                            imaTotale = "1";
                // da li postoje totali  0=ne postoji slog ,1=da postoje totali
                            SqlDataReader ImaTotale = db.ReturnDataReader("if not exists (select TotaliDaNe from RefreshGrida where Dokument= '" + NazivDokumenta + "' AND TotaliDaNe ='DA') select 0 else select  1");
                            ImaTotale.Read();
                            imaTotale = ImaTotale[0].ToString();
                            ImaTotale.Close();
                            ImaTotale.Dispose();
                //Izvrsenje totala za NazivKlona 
                            if (imaTotale == "1")
                            {
                                strParams = "@param0=" + idDokument.ToString() + "`";
                                strTabela = "";
                                str = "Execute TotaliZaDokument '" + NazivDokumenta + "'," + idDokument;
                                lista.Add(new string[] { str, strParams, strTabela, dokType, "" });
                                lista.ToArray();
                            }
                        }
                        break;

                    case "NalogGlavneKnjige": // dovrsavanje operacije posle brisanja stavke iz naloga glavne knjige
                 //koji je dokument  klnjizen u nalogglavneknjige u stavci koju brisemo
                        sql = " Select KS.ID_DokumentZaKnj as IDDK  "
                            + " from NalogGlavneKnjigeStavke as KS  "
                            + " where  KS.ID_NalogGlavneKnjigeStavke=@param0"; ////+ idreda;
                        tt = db.ParamsQueryDT(sql, idreda);
                        if (tt.Rows.Count > 0)
                            iddk = tt.Rows[0]["IDDK"].ToString();

                //vracanje statusa dokumentu koji smo obrisali iz nalogaglavne knjige
                        dokType = "";
                        strTabela = "Dokumenta";
                        strParams = "@param1=" + "NijeProknjizeno" + "`";
                        strParams += "@param2=" + iddk + "`";
                        str = "update Dokumenta set [proknjizeno]=@param1  where [ID_Dokumenta] = @param2";
                        lista.Add(new string[] { str, strParams, strTabela, dokType, iddk });
                        lista.ToArray();

                 //totali za dokument kome je promenjen status   
                        dokType = "";
                        strParams = "";
                        strTabela = "Dokumenta";
                        str = "Execute TotaliZaDokument'" + strTabela + "'," + iddk;
                        lista.Add(new string[] { str, strParams, strTabela, dokType, iddk });
                        lista.ToArray();

                // brisanje slogova iz tabele glavnaknjiga koji se odnose na dokument koji smo izbrisali iz dokumenta nalogglavneknjige
                        dokType = "";
                        strTabela = "GlavnaKnjiga";
                        strParams = "@param1 =" + iddk + "`";
                        str = "DELETE from " + strTabela + " Where [ID_DokumentaView]= @param1";
                        lista.Add(new string[] { str, strParams, strTabela, dokType, iddk });
                        lista.ToArray();
                        break;
            }

                // dodatak za brisanje zaglavlja dokumenta ako su stavke prazne
            if( DokumentJe=="D")
            {
                    sql = "select Distinct Tabela from upiti where ime like '%Uuu%' and NazivDokumenta=@param0"; //+ NazivDokumenta + " '";                       
                    tt = db.ParamsQueryDT(sql, NazivKlona);
                    if (tt.Rows.Count > 0) // postoji uuu u upitima
                    {
                        if (tt.Rows[0]["Tabela"].ToString().Contains("Stavke") == true) // uuu se odnosi na stavke
                        {
                    // koliko ima stavki 
                            sql = "Select Count(*) as br from " + tt.Rows[0]["Tabela"].ToString() + " where ID_DokumentaView=@param0";
                            t = db.ParamsQueryDT(sql, idDokument);
                            if (t.Rows.Count > 0)
                            {
                                if (Convert.ToInt32(t.Rows[0]["br"]) > 0) { }
                                else
                                {
                                    sql = " DELETE from " + tt.Rows[1]["Tabela"].ToString() + " Where ID_DokumentaView =@param0";
                                    strTabela = tt.Rows[1]["Tabela"].ToString();
                                    strParams = "@param0 =" + idDokument + "`";
                                    str = "DELETE from " + strTabela + " Where [ID_DokumentaView]= @param1";
                                    lista.Add(new string[] { str, strParams, strTabela, dokType, iddk });
                                    lista.ToArray();
                                }
                            }
                        }
                        else
                        {
                            sql = "Select Count(*) as br from " + tt.Rows[1]["Tabela"].ToString() + " where ID_DokumentaView=@param0";
                            t = db.ParamsQueryDT(sql, idDokument);
                            if (t.Rows.Count > 0)
                            {
                                if (Convert.ToInt32(t.Rows[0]["br"]) > 0) { }
                                else
                                {
                                    sql = " DELETE from " + tt.Rows[0]["Tabela"].ToString() + " Where ID_DokumentaView =@param0";
                                    //t = db.ParamsQueryDT(sql, idDokument);
                                    strTabela = tt.Rows[0]["Tabela"].ToString();
                                    strParams = "@param0 =" + idDokument + "`";
                                    str = "DELETE from " + strTabela + " Where [ID_DokumentaView]= @param1";
                                    lista.Add(new string[] { str, strParams, strTabela, dokType, iddk });
                                    lista.ToArray();
                                }
                            }
                        }
                    }
            }                          
            return (Erase);
        } //KRAJ erasa

        public void Check()
        {
        }
        private string CreateExecWithParams(String polja,string value,string naredba,string tabela,ref string parametri )
        {
            string str = "";
            string svekolone = "";
            string zaValue = "";

            string[] kolone = polja.Split(';');
            string[] vrednosti = value.Split(';');

            int i = 0;
            int j = 0;
            foreach (var kolona in kolone)
            {
                if (i < kolone.Length - 1)
                    svekolone += "[" + kolona.Trim() + "],";
                else
                    svekolone += "[" + kolona.Trim() + "]";
                if (kolona == "Datum") j = i;
                i++;
            }   

            i = 0;
            zaValue = "";
            
            foreach (var vrednost in vrednosti)
            {
                if (i == j) Console.WriteLine(vrednost);
                parametri += "@param" + i.ToString().Trim() + "=" + vrednost + "`";
                if (i < vrednosti.Length - 1)
                {
                    zaValue = zaValue += "@param" + i.ToString().Trim() + ",";
                }
                else
                {
                     zaValue = zaValue += "@param" + i.ToString().Trim();
                }
                i++;
            }

            str = naredba+" " + tabela +"("+ svekolone + ")  values(" + zaValue + ")";
           Console.WriteLine(str);
            
             return str;
        }
        private Boolean dovrsiobradu()
        {
            Boolean vrati = true;



            return vrati;
        }
        private  string PrevediPolje(string PPolje, string TTip) // moguce wrednosti su in za insert i up za update
        {
            string Polje;
            string Tip;
            string Prevedi = "";
            if (PPolje == null)
            {
                Prevedi = "";
                return Prevedi;
            }
            Polje = PPolje;
            Tip = TTip;

            switch (Tip)
            {
                case "3":
                case "26": //   ceo broj za ident
                    if (Polje == "")
                        Polje = 1.ToString();
                    Prevedi = " " + Polje + " ";
                    break;
                case "4":
                case "5":
                case "6":
                case "7":
                case "11":
                case "13":
                case "19":
                case "21":
                case "24":  //number  pb.Vrednost = pb.Vrednost.Replace(".", "").Replace(",", ".");
                    if (Polje == "")
                        Polje = 0.ToString();
                    if (Polje.Contains(",")==true)
                    {
                        //if( operacija=="IZMENA")
                        //   Polje = Polje.Replace(".", "");  //.Replace(",", "."); // borka 25.02.20
                        //else
                            Polje = Polje.Replace(".", "");////.Replace(",", "."); // borka 25.02.20
                    }
                    Prevedi = " " + Polje + " ";
                    break;
                case "8":
                case "9":
                case "23":
                     Prevedi = "" + Polje.Trim() + ""; //jovana 27.04.20
                    break;
                case "10":
                case "14":
                case "15":
                case "16":   //text
                     Prevedi = "" + Polje.Trim() + ""; //jovana 27.04.20
                    break;
            }
            return Prevedi;
        }


        public void CreateLog(Form forma, string iddok, string idReda, string dokument, string operacija, string upitiime)
        {

            string sql = "INSERT INTO Log(ID_DokumentaView,ID_UpdateReda,Dokument,Operacija,Ime,UUser) VALUES (@ID_DokumentaView,@ID_UpdateReda,@Dokument,@Operacija,@Ime,@UUser)";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@ID_DokumentaView", iddok);
            cmd.Parameters.AddWithValue("@ID_UpdateReda", idReda);
            cmd.Parameters.AddWithValue("@Dokument", dokument);
            cmd.Parameters.AddWithValue("@Operacija", operacija);
            cmd.Parameters.AddWithValue("@Ime", upitiime);
            cmd.Parameters.AddWithValue("@UUser", Program.idkadar);


            string message = db.Comanda(cmd);
            if (message != "")
            {
                MessageBox.Show("Greška: /n" + message);
            }
        }
        public Boolean StornirajDokument()
        {            
            Boolean Vrati = true;
            //Form Me = Program.Parent.ActiveMdiChild;
                  
            string str = "";
            string strParams = "";
            List<string[]> lista = new List<string[]>();
            string strTabela = "Dokumenta";
            string mBrojDokumenta = "";
            string sBrojDokumenta = "";
            string rezultat = "";
            string IdDokument = "";
            
            DataTable ts = new DataTable();
            
            //con.BeginTrans
            string  BrojDokumenta = ((Bankom.frmChield)Me).brdok;
          
            long IdPred =  ((Bankom.frmChield)Me).iddokumenta;
            int IdDokumentaStablo = ((Bankom.frmChield)Me).idstablo;
            string sql = "Select s.KnjiziSe,s.UlazniIzlazni as Naziv from SifarnikDokumenta as s,DokumentaStablo as d  where d.ID_DokumentaStablo=@param0";
            sql += " AND s.naziv =d.Naziv";
            DataTable t = new DataTable();
            t = db.ParamsQueryDT(sql, IdDokumentaStablo);
            if (t.Rows.Count > 0)           
                NazivKlona = t.Rows[0]["Naziv"].ToString();

            string dokType = "";
            DataTable tss = new DataTable();
            sql = " Select * from Dokumenta where ID_Dokumenta=@param0"; 
            tss = db.ParamsQueryDT(sql, IdPred);

            if (tss.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoji Dokument u tabeli Dokumenta");
                Vrati = false;
                return (Vrati);
            }
            // za slucaj da storniramo dokument koji je bio ispravan slog predhodno izvrsenih storna
            str = " Select * from Dokumenta where BrojDokumenta like  '" + BrojDokumenta + "%/S%'";
            ts = db.ReturnDataTable(str);
            if (ts.Rows.Count > 0)
            {
                int i = 0;
                do
                {
                    mBrojDokumenta = ts.Rows[i]["BrojDokumenta"].ToString();
                    strParams = "";
                    str = "";
                    string iddok = "";
                    dokType = "";
                    if (mBrojDokumenta.Contains("S") == true && mBrojDokumenta.Contains("SI") == false)
                    {
                        iddok = ts.Rows[i]["ID_Dokumenta"].ToString();
                        strParams = "@param1=" + BrojDokumenta + "/S" + "`";
                        strParams += "@param2=" + iddok.ToString() + "`";
                        str = "update Dokumenta set [BrojDokumenta]=@param1  where [ID_Dokumenta] = @param2";
                    }
                    if (mBrojDokumenta.Contains("SI") == true)
                    {
                        iddok = ts.Rows[i]["ID_Dokumenta"].ToString();
                        strParams = "@param1=" + tss.Rows[i]["BrojDokumenta"].ToString() + "/SI" + "`";
                        strParams += "@param2=" + tss.Rows[i]["ID_Dokumenta"].ToString() + "`";
                        str = "update Dokumenta set [BrojDokumenta]=@param1  where [ID_Dokumenta] = @param2";
                    }
                    if (str.Trim() != "")
                    {
                        lista.Add(new string[] { str, strParams, strTabela,dokType,iddok.ToString() });
                        lista.ToArray();
                    
                        str = "Execute TotaliZaDokument 'Dokumenta',"+iddok.ToString();
                        lista.Add(new string[] { str, strParams, strTabela,dokType ,iddok.ToString()});
                        lista.ToArray();
                        
                        str="Execute TotaliZaDokument '" + NazivKlona + "', " + iddok.ToString();
                        lista.Add(new string[] { str, strParams, strTabela, dokType,iddok.ToString() });
                        lista.ToArray();
                    }
                    i = i + 1;
                } while (i < ts.Rows.Count);  //kraj while po 
            }


            //menjamo broj odabranog dokumenta koji zalimo da storniramo
            dokType = "";
            strTabela = "Dokumenta";
            strParams = "";            
            strParams = "@param1="+ BrojDokumenta+"/S" + "`"; 
            strParams += "@param2=" + IdPred.ToString() + "`";
            str = "update Dokumenta set [BrojDokumenta]=@param1  where [ID_Dokumenta] = @param2" ;
            lista.Add(new string[] { str, strParams, strTabela,dokType,"" });
            lista.ToArray();
//0.
            strParams = "";
            str = "Execute TotaliZaDokument 'Dokumenta'," + IdPred.ToString();
            lista.Add(new string[] { str, strParams, strTabela, dokType,"" });
            lista.ToArray();
//1.
            str = "Execute TotaliZaDokument '"+ NazivKlona +"'," + IdPred.ToString();
            lista.Add(new string[] { str, strParams, strTabela, dokType, IdDokument });
            lista.ToArray();
//2.
            // Kraj izmenili smo brojdokumenta u dokumentu koji storniramo pozvali totale za dokumeta i totale za dokument koji storniramo

            // 1.upisujemo dokument koji cemo popuniti negativnim vrednostima = STORNO  DOKUMENT
            string Pproknjizeno = "NijeProknjizeno";

            sBrojDokumenta = BrojDokumenta + "/SI";
            dokType = "S";  // radi se o storno dokumentu
            strParams = "";
            strParams = "@param1=" + tss.Rows[0]["RedniBroj"].ToString() + "`";
            strParams += "@param2=" + Program.idkadar.ToString() + "`";
            strParams += "@param3=" + IdDokumentaStablo.ToString() + "`";
            strParams += "@param4=" + sBrojDokumenta  + "`";
            strParams += "@param5=" + tss.Rows[0]["Datum"].ToString() + "`";
            strParams += "@param6=" + tss.Rows[0]["Opis"].ToString() + "`";
            strParams += "@param7=" + Program.idOrgDeo + "`";
            strParams += "@param8=" + Pproknjizeno + "`";
            strParams += "@param9=" + tss.Rows[0]["MesecPoreza"].ToString() + "`";

            str = "Insert Into Dokumenta ( [RedniBroj], [ID_KadrovskaEvidencija],";
            str += " [ID_DokumentaStablo], [BrojDokumenta], [Datum], [Opis],";
            str += " [ID_OrganizacionaStrukturaView],[Proknjizeno],[MesecPoreza])";
            str += " values(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9)";
           
            lista.Add(new string[] { str, strParams, strTabela,dokType, IdDokument });
            lista.ToArray();
//3.
            dokType = "";
            strParams = "";
            // 2. UPISUJEMO STORNO DOKUMENT U PRIPADAJUCE TABELE POZIVOM STOREDPROCEDURE
            str = "Execute dbo.StornoDokumenta '" + NazivKlona + "', " + IdPred.ToString() + ", 'ssss'"; /// + IdSled.ToString();
            lista.Add(new string[] { str, strParams, strTabela, dokType,"" });
            lista.ToArray();
//4.

            str = "Execute TotaliZaDokument 'Dokumenta'," + "'ssss'";  ////idSled.ToString();
            lista.Add(new string[] { str, strParams, strTabela, dokType,"" });
            lista.ToArray();
//5.
            str = "Execute TotaliZaDokument '"+ NazivKlona + "',"+ "'ssss'";  ////idSled.ToString();
            lista.Add(new string[] { str, strParams, strTabela, dokType, IdDokument });
            lista.ToArray();
//6
            // 3. UPISUJEMO PRAZAN DOKUMENT U DOKUMENTA  DOKUMENT CEMO POPUNITI ISPRAVNIM PODACIMO UMESTO STORNIRANOG DOKUMENTA
            dokType = "P";
            strParams = "";
            strParams = "@param1=" + tss.Rows[0]["RedniBroj"].ToString() + "`";
            strParams += "@param2=" + Program.idkadar.ToString() + "`";
            strParams += "@param3=" + IdDokumentaStablo.ToString() + "`";
            strParams += "@param4=" + BrojDokumenta + "`";
            strParams += "@param5=" + tss.Rows[0]["Datum"].ToString() + "`";
            strParams += "@param6=" + tss.Rows[0]["Opis"].ToString() + "`";
            strParams += "@param7=" + Program.idOrgDeo + "`";
            strParams += "@param8=" + Pproknjizeno + "`";
            strParams += "@param9=" + tss.Rows[0]["MesecPoreza"].ToString() + "`";

            str = "Insert Into Dokumenta ( [RedniBroj], [ID_KadrovskaEvidencija],";
            str += " [ID_DokumentaStablo], [BrojDokumenta], [Datum], [Opis],";
            str += " [ID_OrganizacionaStrukturaView],[Proknjizeno],[MesecPoreza])";
            str += " values(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9)";

            lista.Add(new string[] { str, strParams, strTabela, dokType,"" });
            lista.ToArray();
//7
            strParams = "";
            str = "Execute TotaliZaDokument 'Dokumenta'," + "'pppp'";  ////idSled.ToString();
            lista.Add(new string[] { str, strParams, strTabela, dokType, "" });
            lista.ToArray();
//8
            //'ZAVRSAVANJE OPERACIJE STORNO DOKUMENTA
            //dokType = "";
            //strParams = "";
            //strParams = "@param1=" + Program.idkadar.ToString() + "`";
            //strParams += "@param2=" + Program.idkadar.ToString() + "`";
            //strParams += "@param3=" + sBrojDokumenta + "`";
            //str = "update Dokumenta set [ID_kadrovskaEvidencija]=@param1,[UUser]=@param2 where [BrojDokumenta] = @param3";
            //lista.Add(new string[] { str, strParams, strTabela, dokType, "" });
            //lista.ToArray();
            //9.
            str = "Execute TotaliZaDokument 'Dokumenta'," + "'ssss'";  ////idSled.ToString();
            lista.Add(new string[] { str, strParams, strTabela, dokType,"" });
            lista.ToArray();
//9.
            // poziv potrebnih storedprocedura

            IdDokument = IdPred.ToString();
            str = "Execute CeneArtikalaPoSkladistimaIStanje " + IdPred.ToString();
            lista.Add(new string[] { str, strParams, strTabela, dokType, ""});
            lista.ToArray();
//10.
            str ="Execute StanjeRobePoLotu " + IdPred.ToString();
            lista.Add(new string[] { str, strParams, strTabela, dokType, ""});
            lista.ToArray();
//11.
            str = "Execute CeneArtikalaPoSkladistimaIStanje 'ssss'";  /// + IdSled.ToString();
            lista.Add(new string[] { str, strParams, strTabela, dokType,"" });
            lista.ToArray();
            str = "Execute StanjeRobePoLotu 'ssss'"; /// + IdSled.ToString();
            lista.Add(new string[] { str, strParams, strTabela, dokType, "" });
            lista.ToArray();

//12.Stanje:

            ////9.PROVERA STANJA NAKON STORNO DOKUMENTA POCETAK
            string trebaprovera = "0";
            str = " Select OOrderBy as trebaprovera from RecnikPodataka where  OOrderBy >0 AND Dokument  = @param0"; /////+ NazivDokumenta + "'";
            ts = db.ParamsQueryDT(str, NazivKlona);
            if (ts.Rows.Count > 0)
            {
                trebaprovera = ts.Rows[0]["trebaprovera"].ToString();
                dokType = "";
                strParams = "";
                string IdDokumentview = "";
                strParams += " @param2=" + "`";
                str = "Execute stanje 'ssss'";
                strTabela = NazivKlona;
                lista.Add(new string[] { str, strParams, strTabela, dokType, IdDokumentview });
                lista.ToArray();
            }
            rezultat = db.ReturnSqlTransactionParamsFull(lista);
            if (rezultat != "") { lista.Clear(); MessageBox.Show(rezultat); return false; }
            lista.Clear();
            return Vrati;           
        }
    }
}
