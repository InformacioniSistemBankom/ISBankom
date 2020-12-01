using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Djora 25.09.19 28.11.19
namespace Bankom.Class
{
    class clsObradaOsnovnihSifarnika
    {
        string sql = "";
        string sss = "";
        Form forma = new Form();

        //Djora 16.09.20 ovo spustiti nize jer se kreiraju instance prilikom klika na Dokumenta , a ne koristi se nigde, samo opterecujemo memoriju
        DataBaseBroker db = new DataBaseBroker();
        DataTable t = new DataTable();
        clsdokumentRefresh dokr = new clsdokumentRefresh();

        public int UpisiDokument(ref string BrojDok, string Opis, int IdDokumentaStablo, string Datum)
        {
            clsOperacije co = new clsOperacije();
            string strTabela = "Dokumenta";
            string strParams = "";
            List<string[]> lista = new List<string[]>();
            string dokType = "";
            string str = "";
            string rezultat = "";
            string Proknjizeno = "";

            int newID = 0;
            forma = Program.Parent.ActiveMdiChild;
            string NazivDokumenta = "";
            int ParRb = 0;
            string knjizise = "";

            BrojDok = KreirajBrDokNovi(ref ParRb, Datum, IdDokumentaStablo, "UNOS");
            clsMesecPoreza mp = new clsMesecPoreza();
            int MesecPoreza = mp.ObradiMesecPoreza(Datum);

            sql = "Select s.KnjiziSe,s.Naziv from SifarnikDokumenta as s,DokumentaStablo as d  where d.ID_DokumentaStablo=" + IdDokumentaStablo;
            sql += " AND s.naziv =d.Naziv";
            t = db.ReturnDataTable(sql);
            if (t.Rows.Count > 0)
            {
                knjizise = t.Rows[0]["KnjiziSe"].ToString();
                NazivDokumenta = t.Rows[0]["Naziv"].ToString();
                if (knjizise.ToUpper().Contains("N") == true)
                {
                    Proknjizeno = "NeKnjiziSe";
                }
                else Proknjizeno = "NijeProknjizeno";

            }
            dokType = "D";
            strParams = "";
            strParams = "@param1=" + ParRb.ToString() + "`";
            strParams += "@param2=" + Program.idkadar.ToString() + "`";
            strParams += "@param3=" + IdDokumentaStablo.ToString() + "`";
            strParams += "@param4=" + BrojDok + "`";
            strParams += "@param5=" + Datum + "`";
            strParams += "@param6=" + Opis + "`";
            strParams += "@param7=" + Program.idOrgDeo + "`";
            strParams += "@param8=" + Proknjizeno + "`";
            strParams += "@param9=" + Convert.ToString(MesecPoreza) + "`";

            str = "Insert Into Dokumenta ( [RedniBroj], [ID_KadrovskaEvidencija],";
            str += " [ID_DokumentaStablo], [BrojDokumenta], [Datum], [Opis],";
            str += " [ID_OrganizacionaStrukturaView],[Proknjizeno],[MesecPoreza])";
            str += " values(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9)";
            lista.Add(new string[] { str, strParams, strTabela, dokType, "" });
            lista.ToArray();
            dokType = "D";
            strParams = "";
            str = "Execute TotaliZaDokument 'Dokumenta'," + "'tttt'";
            lista.Add(new string[] { str, strParams, strTabela, dokType, "" });
            lista.ToArray();
            rezultat = db.ReturnSqlTransactionParamsFull(lista);

            if (rezultat != "")
                if (co.IsNumeric(rezultat.Trim()) == true)
                    newID = Convert.ToInt32(rezultat);
                else
                    MessageBox.Show("Greaka kod upisa Dokumenta za: " + NazivDokumenta + "!!");
            return (newID);
        }
        public string DodajRestrikcije(string Dokument, string TUD)
        {
            string res = "";
            string ss = "Select * from RecnikPodataka where Dokument=@param0 AND Tud=@param1 AND ltrim(rtrim(Restrikcije))<>''";
            DataTable tt = db.ParamsQueryDT(ss, Dokument, TUD);
            for (int rr = 0; rr < tt.Rows.Count; rr++)
            {
                if (tt.Rows[rr]["Restrikcije"].ToString().ToUpper().Contains("BRDOK") == true)
                {
                }
                else
                {
                    if (string.IsNullOrEmpty(tt.Rows[rr]["Restrikcije"].ToString()))
                    {
                    }
                    else
                        if (tt.Rows[rr]["Restrikcije"].ToString().Trim() != "")
                        res += tt.Rows[rr]["Restrikcije"].ToString().Trim() + " AND ";
                }
            }
            if (res.Trim() != "")
                res = res.Substring(0, res.Length - 5);
            return res;
        }
        //vraca broj dokumenta i azurira referenciranu promenljivu ParRb
        public string KreirajBrDokNovi(ref int RedniBroj, string Ddtm, int IdDokStablo, string operacija)
        {
            DataTable dt = new DataTable();
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;
            string sql;
            string Vrati = "";
            string Tekuca = DateTime.Parse(Ddtm).Year.ToString();
            string SkracenaTekuca = Tekuca.Substring(2, 2);

            switch (operacija)
            {
                case "UNOS":

                    //Ako je dokument KontrolnikIzvoza ili KontrolnikUvoza, onda se redni broj nastavlja (ne ide od jedan na pocetku godine)
                    if (IdDokStablo == 5 || IdDokStablo == 6)
                    {
                        sql = " select max(d.RedniBroj) as UK from Dokumenta as d WITH(NOLOCK), OrganizacionaStruktura as o2 WITH(NOLOCK), OrganizacionaStruktura as o1 WITH(NOLOCK) "
                            + " where  o1.id_OrganizacionaStruktura =" + Program.idOrgDeo.ToString() + " and "
                            + " o1.id_OrganizacionaStrukturaStablo = o2.id_OrganizacionaStrukturaStablo"
                            + " and o2.ID_OrganizacionaStruktura=d.ID_OrganizacionaStrukturaView"
                            + " and d.ID_DokumentaStablo=" + IdDokStablo.ToString()
                            + " and (BrojDokumenta not like '%/S%') and (BrojDokumenta not like '%/SI%') and (BrojDokumenta not like '%PodBr%')";
                    }
                    else
                    {

                        sql = " select max(d.RedniBroj) as UK from Dokumenta as d WITH(NOLOCK), OrganizacionaStruktura as o2 WITH(NOLOCK), OrganizacionaStruktura as o1 WITH(NOLOCK) "
                           + " where  o1.id_OrganizacionaStruktura = " + Program.idOrgDeo.ToString() + " and "
                           + " o1.id_OrganizacionaStrukturaStablo = o2.id_OrganizacionaStrukturaStablo"
                           + " and o2.ID_OrganizacionaStruktura=d.ID_OrganizacionaStrukturaView"
                           + " and Year(d.Datum)= " + Tekuca
                           + " and d.ID_DokumentaStablo=" + IdDokStablo.ToString()
                           + " and (BrojDokumenta not like '%/S%') and (BrojDokumenta not like '%PodBr%')";
                    }
                    Console.WriteLine(sql);
                    dt = db.ReturnDataTable(sql);
                    //dt = db.ParamsQueryDT(sql, IdOrg.ToString(), Tekuca, IdDokStablo.ToString());
                    //if (dt.Rows.Count == 0) { }
                    //Ako je redni broj jednak NULL
                    if (string.IsNullOrEmpty(dt.Rows[0]["UK"].ToString()))
                    {
                        RedniBroj = 1;
                    }
                    else
                    {
                        RedniBroj = Convert.ToInt32(dt.Rows[0]["UK"]) + 1;
                    }

                    if (Program.imeFirme == "Bankom" || Program.imeFirme == "FinaBeo")
                    {
                        Vrati = IdDokStablo.ToString() + "-" + RedniBroj.ToString() + "-" + Program.SifRadnika + "/" + SkracenaTekuca + "-" + Program.idOrgDeo.ToString();
                    }
                    else
                    {
                        Vrati = IdDokStablo.ToString() + "-" + RedniBroj.ToString() + "-" + Program.SifRadnika + "/" + Tekuca + "-" + Program.idOrgDeo.ToString();
                    }

                    break;
                case "UNOS PODBROJA":
                    string nBrDok = "";
                    string mBrDok = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrDok").Vrednost;
                    string mRb = "";

                    if (mBrDok.Contains("PodBr") == true)
                    {
                        int aaa = mBrDok.IndexOf("PodBr");
                        string bbb = mBrDok.Substring(0, mBrDok.IndexOf("PodBr") - 1);

                        mBrDok = mBrDok.Substring(0, mBrDok.IndexOf("PodBr") - 1);

                    }
                    sql = "Select RedniBroj from Dokumenta Where BrojDokumenta= '" + mBrDok + "'";
                    Console.WriteLine(sql);
                    t = db.ParamsQueryDT(sql, mBrDok);
                    if (t.Rows.Count > 0) { mRb = t.Rows[0]["RedniBroj"].ToString(); }
                    sql = " select count(*) as u from Dokumenta  WITH(NOLOCK) where BrojDokumenta Not like '%S%' and BrojDokumenta like @param0";
                    t = db.ParamsQueryDT(sql, mBrDok + "%");

                    nBrDok = mBrDok + "-PodBr." + t.Rows[0]["U"].ToString();


                    RedniBroj = Convert.ToInt32(mRb);
                    Vrati = nBrDok;
                    break;
            }
            return (Vrati);
        }
        public string KB_97(string inTekst)    //' vraca kontrolni broj po modulu 97
        {
            decimal varInput_Value = 0;
            decimal varResult = 0;
            decimal inModel = 97;
            clsOperacije co = new clsOperacije();

            if (inTekst.Trim() == "") return ("");

            if (co.IsNumeric(inTekst) == false) return ("");

            varInput_Value = Convert.ToInt32(inTekst) * 100;
            varResult = Convert.ToDecimal((varInput_Value / inModel) - Math.Truncate(varInput_Value / inModel));
            varResult = inModel + 1 - varResult * inModel;
            return (Convert.ToString(Convert.ToInt32(varResult)));
        }
        public string FormatirajRacun(string racun)
        {
            string fmt = "00000000000000";
            string formatString = " {0,14:" + fmt + "}";
            string mracun = "";
            racun = racun.Replace("-", "");

            double intValue = Convert.ToDouble(racun.Substring(3, racun.Length - 5));
            Console.WriteLine(intValue.ToString(fmt));
            mracun = intValue.ToString(fmt);

            mracun = racun.Substring(0, 2) + intValue.ToString(fmt) + racun.Substring(racun.Length - 2, 2);
            Console.WriteLine(mracun);
            return (mracun);
        }
        public void SortirajGrid(ref DataGridView control, string NazivKlona, string DokumentJe)
        {
            forma = Program.Parent.ActiveMdiChild;
            string mUpit = "";
            DataTable tg = new DataTable();
            string KojiSegment = control.Name.Substring(4);

            clsdokumentRefresh docref = new clsdokumentRefresh();
            if (((Bankom.frmChield)forma).statusStrip1.Visible == false)
                tg = (DataTable)control.DataSource; // U data source-grida su prisutni svi slogovi
            else
            {
                mUpit = ((Bankom.frmChield)forma).tUpit;
                Console.WriteLine(mUpit);
                tg = db.ReturnDataTable(mUpit);    ///((Bankom.frmChield)forma).tgg;//// (DataTable)control.DataSource;
                int brrec = tg.Rows.Count;
            }

            DataTable dtOut = null;
            string sortExpression = string.Format("{0} {1}", Program.colname, Program.smer);
            tg.DefaultView.Sort = sortExpression;
            dtOut = tg.DefaultView.ToTable();
            Console.WriteLine(dtOut.Rows.Count);
            tg = dtOut;
            // Jovana 23.11.20 dodala da samo za Izvestaje ide u osvezavanje salda
            if (DokumentJe == "I")
            {
                sql = "Select AlijasPolja as Polje,FormulaForme from Recnikpodataka WHERE  Dokument = @param0 AND(FormulaForme IS NOT NULL AND FormulaForme <> N'')";
                sql += " And TabelaVview='" + Program.imegrida.Substring(4) + "'";
                Console.WriteLine(sql);
                t = db.ParamsQueryDT(sql, NazivKlona);
                for (int i = 0; i < t.Rows.Count; i++)
                {
                    this.OsveziSalda(ref tg, t.Rows[i]["Polje"].ToString(), t.Rows[i]["formulaforme"].ToString());
                }
                //if (((Bankom.frmChield)forma).statusStrip1.Visible == false)
                //    this.DodajRedniBroj(ref tg, KojiSegment, DokumentJe);
                //for( int i=0;i<10;i++)
                //{
                //    Console.WriteLine(tg.Rows[i][Program.colname].ToString());
                //}

                control.DataSource = docref.ShowPage(forma, 1, tg);

                sss = "Select DISTINCT TUD from RecnikPodataka WHERE  Dokument = @param0  And TabelaVview='" + Program.imegrida.Substring(4) + "'";
                Console.WriteLine(sss);
                DataTable t1 = db.ParamsQueryDT(sss, NazivKlona);
                string TUD = t1.Rows[0]["TUD"].ToString();

                docref.setingWidthOfColumns(NazivKlona, control, TUD);


                //if (NazivKlona == "LotStanjeRobeM")
                //    docref.colorofrows(control);
            }
            control.DataSource = tg;
            if (NazivKlona == "LotStanjeRobeM")
                docref.colorofrows(control);
            control.Refresh();
        }
        public void DodajRedniBroj(ref DataTable tg, string KojiSegment, string DokumentJe)
        {
            // AKO U RECNIKU POSTOJI POLJE SA ID_NaziviNaFormi=20 KOJE IMA SIRINU VECU OD 0 TO POLJE JE NAMENJENO DA SE U NJEGA PROGRAMSKI UPISE REDNI BROJ STAVKE.TO RADI KOD KOJI SLEDI
            string RbPolje = "";
            DataTable dt = new DataTable();
            string ssel = " SELECT AlijasPolja as APolja from recnikpodataka where (id_nazivinaformi=20  OR id_nazivinaformi=25) AND (WidthKolone>0) AND TabelaVView=@param0";
            dt = db.ParamsQueryDT(ssel, KojiSegment);
            if (dt.Rows.Count > 0) // postoji kolona za redni broj
            {
                if (DokumentJe == "I")
                {
                    RbPolje = dt.Rows[0]["APolja"].ToString();
                }
                else
                {
                    RbPolje = "ID_GgRr" + (KojiSegment).Trim();
                }
                //dodajemo redni broj 
                if (tg.Rows.Count > 0)
                {
                    for (int k = 0; k < tg.Rows.Count; k++)
                    {
                        if (DokumentJe == "D")
                        {
                            tg.Rows[k][RbPolje] = tg.Rows.Count - k;
                        }
                        else
                        {
                            tg.Rows[k][RbPolje] = k + 1;
                        }
                    }
                }
            }
        }
        public void OsveziSalda(ref DataTable tg, string KojePolje, string KojaFormula)
        {
            decimal KojaVrednost = 0;
            string prviclan = "";
            string drugiclan = "";
            DataBaseBroker db = new DataBaseBroker();
            //DataTable t = new DataTable();
            clsOperacije co = new clsOperacije();
            //t = tg;
            for (int i = 0; i < tg.Rows.Count; i++)
            {
                if (KojaFormula.Contains("-") == true)
                {
                    prviclan = KojaFormula.Substring(0, KojaFormula.IndexOf("-"));
                    drugiclan = KojaFormula.Substring(KojaFormula.IndexOf("-") + 1);
                    KojaVrednost += Convert.ToDecimal(tg.Rows[i][prviclan].ToString()) - Convert.ToDecimal(tg.Rows[i][drugiclan].ToString());
                }
                else
                {
                    if (KojaFormula.Contains("/") == true)
                    {
                        prviclan = KojaFormula.Substring(0, KojaFormula.IndexOf("/"));
                        drugiclan = KojaFormula.Substring(KojaFormula.IndexOf("/") + 1);
                        if (Convert.ToDouble(tg.Rows[i][drugiclan].ToString()) > 0.01)
                            KojaVrednost += Convert.ToDecimal(tg.Rows[i][prviclan].ToString()) / Convert.ToDecimal(tg.Rows[i][drugiclan].ToString());
                    }
                    else
                    {
                        if (KojaFormula.Contains("+") == true)
                        {
                            prviclan = KojaFormula.Substring(0, KojaFormula.IndexOf("+"));
                            drugiclan = KojaFormula.Substring(KojaFormula.IndexOf("+") + 1);
                            if (co.IsNumeric(drugiclan.Trim()) == true)
                                KojaVrednost += Convert.ToDecimal(tg.Rows[i][prviclan].ToString()) + Convert.ToDecimal(drugiclan);
                            else
                                KojaVrednost += Convert.ToDecimal(tg.Rows[i][prviclan].ToString()) + Convert.ToDecimal(tg.Rows[i][drugiclan].ToString());
                        }
                        else
                            KojaVrednost = 0;
                    }
                    //else
                    //    KojaVrednost += KojaVrednost + Convert.ToDecimal(t.Rows[i][KojaFormula].ToString());
                }
                tg.Rows[i][KojePolje] = Convert.ToString(KojaVrednost);
            }
            //tg = t; 
        }
        public Boolean Iosi(string DoDatuma, string Konto, int Komitent)
        {
            Boolean vrati = true;
            int KRAJ = 0;
            string OdDatuma = "";
            string sselect = "";
            decimal pom = 0;
            ////Dim rsKumulativ As New ADODB.Recordset
            ////rsKumulativ.CursorLocation = adUseClient
            ////Dim cmd As New ADODB.Command
            ////Set cmd.ActiveConnection = cnn1
            ////cmd.CommandType = adCmdText
            ////cmd.CommandTimeout = 0


            OdDatuma = "01.01." + Convert.ToString(Convert.ToDateTime(DoDatuma).Year);
            sql = "Drop table Iosi";
            int rez = db.ReturnInt(sql, 0);

            sselect = " select g.iidd as iid,g.konto,k.Nazivkom,s.oznval,g.datum,g.opisknj,g.duguje,";
            sselect += "g.potrazuje,g.saldo,k.Adresa,k.Mesto,k.Ptt,ko.AvPZnak, g.NazivOrg, g.id_komitentiview,k.PIB ";
            sselect += " INTO iosi  ";
            sselect += " FROM GlavnaKnjiga as g WITH (NOLOCK), KomitentiTotali as k WITH (NOLOCK),Kontni as ko WITH(NOLOCK),SifrarnikValuta as s WITH(NOLOCK) ";
            sselect += " where g.NazivOrg = '" + Program.imeFirme + "' and ko.konto = g.konto ";
            sselect += " AND s.ID_SifrarnikValuta=g.ID_SifrarnikValuta ";
            sselect += " and k.ID_KomitentiTotali= g.ID_KomitentiView  and g.OpisKnj not like '%/s%' ";
            sselect += " and datum>='" + OdDatuma.Trim() + "' and datum<='" + DoDatuma.Trim() + "'";
            if (Komitent > 1)
                sselect += " and k.ID_KomitentiTotali=" + Komitent.ToString();
            if (Konto.Trim() != "")
                sselect += " and g.Konto='" + Konto.Trim() + "'";
            SqlCommand cmd = new SqlCommand(sselect);
            string message = db.Comanda(cmd);
            if (message != "")
                MessageBox.Show(message);


            sselect = "";
            if (Konto.Substring(0, 1) == "2" || Konto.Substring(0, 1) == "1" || Konto.Substring(0, 1) == "0")
                sselect = " SELECT  SUM(duguje - potrazuje) AS saldo From Iosi ";
            else
            {
                if (Konto.Substring(0, 1) == "4")
                    sselect = " SELECT  SUM(potrazuje-duguje ) AS saldo From Iosi ";
            }
            DataTable k = db.ReturnDataTable(sselect);
            if (k.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty((k.Rows[0]["saldo"].ToString())))
                    pom = 0;
                else
                    pom = Convert.ToInt32(k.Rows[0]["saldo"].ToString());
            }
            else
            {
                vrati = false;
            }


            KRAJ = 0;

            sselect = "select * from Iosi order by datum DESC ";
            k = db.ReturnDataTable(sselect);
            for (int i = 0; i < k.Rows.Count; i++)
            {
                if (KRAJ == 1)
                    t.Rows[0]["iid"] = 0;
                else
                if (Konto.Substring(0, 1) == "2" || Konto.Substring(0, 1) == "1" || Konto.Substring(0, 1) == "0")  // dugovna konta
                {
                    if (Convert.ToDecimal(k.Rows[i]["Duguje"].ToString()) >= 0)
                        pom -= Convert.ToDecimal(k.Rows[i]["Duguje"].ToString());

                    if (pom <= 0)
                        if (pom == 0) { }
                        else
                        {
                            pom += Convert.ToDecimal(k.Rows[i]["Duguje"].ToString());
                            k.Rows[i]["Duguje"] = pom;
                        }
                    KRAJ = 1;
                }
                else                                      //potrazna konta
                {
                    if (Convert.ToDecimal(k.Rows[i]["Potrazuje"].ToString()) >= 0)
                    {
                        pom -= Convert.ToDecimal(k.Rows[i]["Potrazuje"].ToString());
                        k.Rows[i]["Duguje"] = k.Rows[i]["Potrazuje"];
                    }
                }
                if (pom <= 0)
                {
                    if (pom == 0) { }
                    else
                    {
                        pom += Convert.ToDecimal(k.Rows[i]["Potrazuje"].ToString());
                        k.Rows[i]["Duguje"] = pom;
                    }
                    KRAJ = 1;
                }

                //rsKumulativ.Update
                //   rsKumulativ.MoveNext
            }

            //    rsKumulativ.Close
            //    cnn1.Execute "delete from Iosi where iid = 0"
            //    cnn1.Execute "delete from Iosi where ( duguje<0.1)" '  and AvPZnak='D'"


            //    rsKumulativ.Open "select * from Iosi ", cnn1
            //    If rsKumulativ.EOF Then Iosi = False


            //    rsKumulativ.Close
            //    Set rsKumulativ = Nothing
            //    Set cmd.ActiveConnection = Nothing


            //Iosi_Error:
            //            MsgBox "Greska"
            return vrati;
        }
    }
}
