using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace Bankom.Class
{
    class clsPreuzimanja
    {
            DataBaseBroker db = new DataBaseBroker();
        string PPrikaz = "";
        string NacinRegistracije = "";
        string Dokument = "";
        string DokumentJe = "";
        string NazivKlona = "";
        string Naslov = "";
        string DDatum = "";
        string Predhodnik = "";
        string KojiPrepis = "";
        string pNalog = "";
        string pPrikaceniFajl = "";
        int IdDokView = 0;
        //string  PutanjaPlacanja = UCase(CitajIniFile("LOGOVANJE", "PutanjaPlacanja"))
        string PutanjaPlacanja = "";
        int IdDokumentaStablo = 0;
      
        public string NazivRacuna { get; private set; }

        public void izborPReuzimanja(int izbor, string mesecgodina) //(0-preuzimanje izvoda iz banaka,1-,2-)
        {

            switch (izbor)
            {
                case 0:
                    preuzimanjeIzvodaizBanaka();  //preuzimanje izvoda iz banaka
                    break;
                case 1:
                    PrepisiIzvod(mesecgodina);
                    break;
                default:
                    break;


            }
        }
        private void PrepisiIzvod(string ulaz)
        {
            int RedniBroj = 0;
            string OOpis = "";
            string BrojDok = "";
            DataTable prometprepisani = new DataTable();
            DataTable rspromet = new DataTable();
            DataTable rstStablo = new DataTable();
            DataTable rs2 = new DataTable();
            DataTable rsd = new DataTable();
            DataTable rsv = new DataTable();
            DataTable rspo = new DataTable();
            DataTable rsp = new DataTable();
            DataTable rsDokumentaStablo = new DataTable();
            int pbanka = 0;
            string NazivBanke = "";
            int BrojIzvoda = 0;
            int PKomitent = 0;
            int Rr = 0;
            string BRD = "";
            string str = "";
            string pdokument = "";
            string strParams = "";
            List<string[]> lista = new List<string[]>();
            char[] separators = { '#' };
            string DatumP = ulaz.Split(separators)[0];
            string KojiRacun = ulaz.Split(separators)[1];

            rspromet = db.ReturnDataTable("Select * from BankaView where Replace(NazivRacuna,'-','')='" + KojiRacun + "'");
            if (rspromet.Rows.Count > 0)
            {
                pbanka = Convert.ToInt32(rspromet.Rows[0]["ID_BankaView"]);
                NazivBanke = rspromet.Rows[0]["NazivBanke"].ToString();
                PKomitent = Convert.ToInt32(rspromet.Rows[0]["id_KomitentiView"]);
            }
            else
            {
                MessageBox.Show("Pogresan broj tekuceg racuna ");
                return;
            }

            str = "SELECT DISTINCT Datum, ID_Blagajna ";
            str += " FROM  IzvodTotali  WHERE ID_Blagajna =" + pbanka.ToString();
            str += " AND  Format(datum,'dd.MM.yy') ='" + DatumP + "' ";

            rspromet = db.ReturnDataTable(str);
            if (rspromet.Rows.Count > 0)
            {
                MessageBox.Show("Vec je izvrsen prepis izvoda za datum:" + DatumP + " i racun:" + KojiRacun);
                return;
            }
            rspromet.Dispose();



            str = "Select * from PlacanjaNaplate as pn,Dokumenta as d  ";
            str += " where  d.ID_Dokumenta=pn.ID_DokumentaView AND ((Isplate>0 and PrenesenoZaPlacanje=1) or Uplate>0 or VrstaSloga='T')";
            str += " and Format(d.datum,'dd.MM.yy') ='" + DatumP + "' And ID_BankaView=" + pbanka.ToString() + " order by ID_SvrhaPlacanjaView,SvrhaPlacanjaSaIzvoda";
            rspromet = db.ReturnDataTable(str);


            str = db.ReturnString("delete IzvodPrepisi", 0);
            prometprepisani = db.ReturnDataTable(" select * from IzvodPrepisi");
            str = "";
            foreach (DataRow row in rspromet.Rows)
            {

                string opis = "";
                strParams = "";
                strParams = "@param1=" + row["Datum"].ToString() + "`";
                strParams += "@param2=" + row["ID_BankaView"] + "`";
                strParams += "@param3=" + row["uplate"] + "`";
                strParams += "@param4=" + row["isplate"] + "`";
                strParams += "@param5=" + row["OznakaKnjizenja"] + "`";
                strParams += "@param6=" + row["VrstaSloga"] + "`";

                if (Convert.ToInt32(row["ID_PozivNaBroj"]) > 1)
                {
                    str = "Select ID_PDVUlazniJciTotali as Iid  from PDVUlazniJciTotali where ID_RacunZaPlacanje=" + row["ID_PozivNaBroj"].ToString();
                    rsp = db.ReturnDataTable(str);
                    if (rsp.Rows.Count > 0) { row["ID_PozivNaBroj"] = rsp.Rows[0]["iid"]; }

                }
                strParams += "@param7=" + row["ID_PozivNaBroj"] + "`";
                strParams += "@param8=" + row["id_KomitentiView"] + "`";
                strParams += "@param9=" + row["ID_SvrhaplacanjaView"] + "`";
                strParams += "@param10=" + row["StariSaldo"] + "`";
                strParams += "@param11=" + row["NoviSaldo"] + "`";
                strParams += "@param12=" + row["BrojIzvoda"] + "`";
                strParams += "@param13=" + row["OblikPlacanja"] + "`";
                opis = row["opis"].ToString();

                if (row["SvrhaPlacanjaSaIzvoda"].ToString().ToUpper().IndexOf("DOZ") > -1
                     || row["SvrhaPlacanjaSaIzvoda"].ToString().ToUpper().IndexOf("AKREDIT") > 0)
                {
                    if (row["SvrhaPlacanjaSaIzvoda"].ToString().Length > 17)
                        opis = row["SvrhaPlacanjaSaIzvoda"].ToString().Substring(17);
                    else { opis = row["SvrhaPlacanjaSaIzvoda"].ToString(); }
                }


                if (row["OblikPlacanja"].ToString().Length == 16)
                { opis = row["PozivNaBrojSaIzvoda"].ToString(); }

                if (Convert.ToInt32(row["ID_Svrhaplacanjaview"]) == 752)
                { opis = row["SvrhaPlacanjaSaIzvoda"].ToString(); }

                strParams += "@param14=" + opis + "`";
                strParams += "@param15=" + Program.idkadar;


                str = " Insert into IzvodPrepisi ( ";
                str += " [Datum],[ID_BankaView],[uplate],[isplate],[OznakaKnjizenja],"; //5
                str += "[VrstaSloga],[ID_PozivNaBroj],"; //2
                str += "[id_KomitentiView],[ID_SvrhaPlacanja],[StariSaldo],"; // 3
                str += "[NoviSaldo],[BrojIzvoda],[OblikPlacanja],[opis],[uuser] ) "; //5                
                str += " values(@param1,@param2,@param3 ,@param4,";
                str += " @param5,@param6,@param7,";
                str += " @param8 ,@param9,@param10,";
                str += " @param11,@param12,@param13 ,@param14,@param15 ) ";
                lista.Add(new string[] { str, strParams });
                /////...
                lista.ToArray();



            }
            string rezultat = db.ReturnSqlTransactionParams(lista);
            lista.Clear();
            if (rezultat != "") { MessageBox.Show(rezultat); return; }
            str = "";
            str = " Select NazivBanke,BrojIzvoda  ";
            str += " from IzvodPrepisi as p,BankaView as b Where p.ID_BankaView=b.ID_BankaView And p.VrstaSloga='T' ";
            prometprepisani.Clear();
            prometprepisani = db.ReturnDataTable(str);

            if (prometprepisani.Rows.Count > 0)
            { BrojIzvoda = Convert.ToInt32(prometprepisani.Rows[0]["BrojIzvoda"]); }
            pdokument = "Izvod";
            str = " SELECT * From DokumentaStablo where Naziv = '" + pdokument + "'";
            rsDokumentaStablo = db.ReturnDataTable(str);

            if (rsDokumentaStablo.Rows.Count > 0)
            {
                IdDokumentaStablo = Convert.ToInt32(rsDokumentaStablo.Rows[0]["ID_DokumentaStablo"]);
            }
            else
            {
                MessageBox.Show("Nije registrovan dokument " + pdokument + "!!");
                return;
            }
            //  clsXmlPlacanja cx = new clsXmlPlacanja();
            // BrojDok = cx.KreirajBrDokNovi(pdokument, RedniBroj, DatumP, Program.idOrgDeo, Program.idkadar, IdDokumentaStablo);
            if (NazivBanke.ToUpper().Contains("INTESA")==true)
            { OOpis = pdokument + " INTESA " + BrojIzvoda.ToString(); }
            else
            {
                OOpis = pdokument + " " + NazivBanke.Substring(1, NazivBanke.IndexOf(" ")) + " " + BrojIzvoda.ToString();
            }

            clsObradaOsnovnihSifarnika cs = new clsObradaOsnovnihSifarnika();
            IdDokumentaStablo = (int)rsDokumentaStablo.Rows[0]["ID_DokumentaStablo"];
            BrojDok = ""; //// cs.KreirajBrDokNovi(ref RedniBroj, DatumP, (int)rsDokumentaStablo.Rows[0]["ID_DokumentaStablo"], "");
            int IdDokview=cs.UpisiDokument(ref BrojDok, OOpis, IdDokumentaStablo, DatumP);


            
            //DateTime dt = Convert.ToDateTime(DatumP);
            //string mesec = Convert.ToString(dt.Month);

            //strParams = "";
            //strParams = "@param1=" + RedniBroj + "`";
            //strParams += "@param2=" + Program.idkadar + "`";
            //strParams += "@param3=" + IdDokumentaStablo + "`";
            //strParams += "@param4=" + BrojDok + "`";
            //strParams += "@param5=" + DatumP.ToString() + "`";
            //strParams += "@param6=" + OOpis + "`";
            //strParams += "@param7=" + Program.idOrgDeo + "`";
            //strParams += "@param8=" + "NijeProknjizeno" + "`";
            //strParams += "@param9=" + mesec;



            //str = "insert into Dokumenta(RedniBroj, ID_KadrovskaEvidencija,";
            //str += " ID_DokumentaStablo, BrojDokumenta, Datum, Opis,";
            //str += " ID_OrganizacionaStrukturaView,Proknjizeno,MesecPoreza)";
            //str += " values(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9)";
            //lista.Clear();
            //lista.Add(new string[] { str, strParams });
            //lista.ToArray();
            //rezultat = db.ReturnSqlTransactionParams(lista);
            //if (rezultat != "") { MessageBox.Show(rezultat); return; }
            //lista.Clear();

            //str = "";
            //str = "select id_dokumenta from dokumenta where BrojDokumenta = '" + BrojDok.ToString() + "'";
            //rsd.Clear();
            //rsd = db.ReturnDataTable(str);
            //IdDokView = Convert.ToInt32(rsd.Rows[0]["Id_Dokumenta"]);


            rs2 = db.ReturnDataTable("SELECT * FROM Izvod Where ID_DokumentaView=1");
            prometprepisani.Clear();
            prometprepisani = db.ReturnDataTable(" Select *  from IzvodPrepisi as p Where  p.VrstaSloga='T' ");

            str = "";
            strParams = "";

            if (prometprepisani.Rows.Count > 0)
            {
                strParams += "@param1=" + prometprepisani.Rows[0]["StariSaldo"] + "`";
                strParams += "@param2=" + prometprepisani.Rows[0]["NoviSaldo"] + "`";
                strParams += "@param3=" + prometprepisani.Rows[0]["isplate"] + "`";
                strParams += "@param4=" + prometprepisani.Rows[0]["uplate"] + "`";
                strParams += "@param5=" + prometprepisani.Rows[0]["BrojIzvoda"] + "`";
            }

            strParams += "@param6=" + prometprepisani.Rows[0]["IdDokView"] + "`";
            strParams += "@param7=" + prometprepisani.Rows[0]["pbanka"] + "`";
            strParams += "@param8=" + DatumP + "`";
            strParams += "@param10=" + Program.idkadar;

            char[] izbor = { '=' };
            string[] izbor2 = strParams.Split(izbor);

            switch (izbor2.Length)
            {
                case 10:

                    str = " INSERT INTO[dbo].[Izvod] ";
                    str += "([KumulativnoDuguje],[KumulativnoPotrazuje],[DPDuguje],[DPPotrazuje],[BrojIzvoda],";
                    str += "[ID_DokumentaView],[id_Blagajna],[Datum],[uuser])";
                    str += " values(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10)";
                    break;
                default:
                    strParams = "";
                    strParams += "@param1=" + prometprepisani.Rows[0]["IdDokView"] + "`";
                    strParams += "@param2=" + prometprepisani.Rows[0]["pbanka"] + "`";
                    strParams += "@param3=" + DatumP + "`";
                    strParams += "@param4=" + Program.idkadar;
                    str = " INSERT INTO[dbo].[Izvod] ";
                    str += "[ID_DokumentaView],[id_Blagajna],[Datum],[uuser])";
                    str += " values(@param1,@param2,@param3,@param4)";
                    break;
            }

            lista.Clear();
            lista.Add(new string[] { str, strParams });
            lista.ToArray();
            rezultat = db.ReturnSqlTransactionParams(lista);
            if (rezultat != "") { MessageBox.Show(rezultat); return; }
            lista.Clear();


            //' zaglavlje
            ///////////////////////
            str = "";
            str = " Select p.*  from IzvodPrepisi as p,BankaView as b Where p.ID_BankaView=b.ID_BankaView ";
            str += " And p.VrstaSloga<>'T' and ID_SvrhaPlacanja <> 752";
            prometprepisani.Clear();

            prometprepisani = db.ReturnDataTable(str);
            str = "SELECT * FROM IzvodStavke where ID_DokumentaView=1 ";
            rs2.Clear();
            rs2 = db.ReturnDataTable(str);
            strParams = "";
            decimal UkupnoNaknada = 0;
            foreach (DataRow row in prometprepisani.Rows)
            {
                strParams += "@param1=" + IdDokView + "`";
                strParams += "@param2=" + row["id_KomitentiView"] + "`";
                strParams += "@param3=" + row["ID_SvrhaPlacanja"] + "`";
                if (Convert.ToInt32(row["OblikPlacanja"]) == 2)
                {
                    strParams += "@param4=5" + "`";
                }
                else
                {
                    strParams += "@param4=" + row["OblikPlacanja"] + "`";
                }
                strParams += "@param5=" + row["opis"] + "`";
                if (Convert.ToInt32(row["ID_NacinPlacanja"]) == 16)
                { strParams += "@param5=" + row["opis"] + "`"; }

                strParams += "@param6=" + row["ID_PozivNaBroj"] + "`";
                strParams += "@param7=" + row["uplate"] + "`";
                strParams += "@param8=" + Math.Round(Convert.ToDecimal(row["isplate"]), 2) + "`";
                strParams += "@param9=" + Program.idkadar;

                str = "";
                str = "INSERT INTO[dbo].[IzvodStavke] ";
                str += " ([ID_DokumentaView],[ID_KomitentiView],[ID_ArtikliView],";
                str += " [ID_NacinPlacanja],[Opis],[ID_PozivNaBroj],[Uplate],[Isplate][uuser])";
                str += " values(@param1,@param2,,@param3,@param4,@param5,@param6,@param7,@param8,@param9)";
                lista.Add(new string[] { str, strParams });
                lista.ToArray();
            }
            rezultat = db.ReturnSqlTransactionParams(lista);
            if (rezultat != "") { MessageBox.Show(rezultat); return; }
            lista.Clear();
            ///////
            prometprepisani.Clear();
            str = " Select p.*  from IzvodPrepisi as p,BankaView as b Where p.ID_BankaView=b.ID_BankaView ";
            str += "  And p.VrstaSloga<>'T' and ID_SvrhaPlacanja = 752 order by Opis ";
            prometprepisani = db.ReturnDataTable(str);
            string mopis = "";
            UkupnoNaknada = 0;
            foreach (DataRow row in prometprepisani.Rows)
            {
                if (mopis == "") { mopis = row["opis"].ToString(); }
                if (row["opis"].ToString() != mopis)
                {
                    if (UkupnoNaknada > 0)
                    {
                        strParams = "";
                        strParams += "@param1=" + IdDokView + "`";
                        strParams += "@param2=" + row["PKomitent"] + "`";
                        strParams += "@param3=752" + "`";
                        strParams += "@param4=5"  + "`";
                        strParams += "@param5=1" + "`";
                        strParams += "@param6=0" + "`";
                        strParams += "@param7=" + UkupnoNaknada  + "`";
                        strParams += "@param8=" + Program.idkadar + "`";
                        strParams += "@param9=" + mopis.ToLower() + "`";

                        str = "INSERT INTO[dbo].[IzvodStavke] ";
                        str += " ([ID_DokumentaView],[id_KomitentiView],[ID_ArtikliView],";
                        str += " [ID_NacinPlacanja],[ID_PozivNaBroj],[Uplate],[Isplate][uuser],[opis])";
                        str += " values(@param1,@param2,,@param3,@param4,@param5,@param6,@param7,@param8,@param9)";

                        lista.Clear();
                        lista.Add(new string[] { str, strParams });
                        lista.ToArray();
                        rezultat = db.ReturnSqlTransactionParams(lista);
                        if (rezultat != "") { MessageBox.Show(rezultat); return; }
                        lista.Clear();
                    }
                    mopis = row["opis"].ToString();
                    UkupnoNaknada = 0;
                
                }
                UkupnoNaknada = UkupnoNaknada + Convert.ToInt32(row["isplate"]);
            }

            if (UkupnoNaknada > 0)
            {
                strParams = "";
                strParams += "@param1=" + IdDokView + "`";
                strParams += "@param2=" + PKomitent + "`";
                strParams += "@param3=752" + "`";
                strParams += "@param4=5" + "`";
                strParams += "@param5=1" + "`";
                strParams += "@param6=0" + "`";
                strParams += "@param7=" + UkupnoNaknada + "`";
                strParams += "@param8=" + Program.idkadar + "`";
                strParams += "@param9=" + mopis.ToLower() + "`";

                str = "INSERT INTO[dbo].[IzvodStavke] ";
                str += " ([ID_DokumentaView],[id_KomitentiView],[ID_ArtikliView],";
                str += " [ID_NacinPlacanja],[ID_PozivNaBroj],[Uplate],[Isplate][uuser],[opis])";
                str += " values(@param1,@param2,,@param3,@param4,@param5,@param6,@param7,@param8,@param9)";

                lista.Clear();
                lista.Add(new string[] { str, strParams });
                lista.ToArray();
                rezultat = db.ReturnSqlTransactionParams(lista);
                if (rezultat != "") { MessageBox.Show(rezultat); return; }
                lista.Clear();

            }
            Dictionary<string, string> rezult = db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + (int)IdDokView);
            Dictionary<string, string> rezult2 = db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:pdokument", "IdDokument:" + (int)IdDokView);

        }
        public string  preuzimanjeIzvodaizBanaka()
        {
            string pDatum = "";
            string pdokument = "";            
            string PPrikaz = "";
            int RedniBroj = 0;
            string BrojDok = "";
            string rezultat = "";            
            int IdDokView = 0;
            
            List<string[]> lista = new List<string[]>();

         

            //list.ToArray();

        DATUMVREME:
            pDatum = Prompt.ShowDialog(DateTime.Now.ToString("dd.MM.yy"), "Unesite datum za koji preuzimamo izvod ", "Preuzimanje uplata od banaka");
            if (pDatum == "") { return ""; }

            clsOperacije co = new clsOperacije();

            bool dt = co.IsDateTime(pDatum);                       
            if (dt == false) { MessageBox.Show("nekorektan unos datuma"); goto DATUMVREME; }
            int godina = Convert.ToInt32(DateTime.Today.Year.ToString().Substring(2, 2));
            if (Convert.ToInt16(pDatum.Substring(pDatum.Length-2)) > godina) { MessageBox.Show("nekorektan unos datuma"); goto DATUMVREME; }
            pdokument = "PrometUplata";
            Dokument = "PrometUplata";
            
            DataTable rssifdok = db.ReturnDataTable("select * from SifarnikDokumenta where  naziv='" + pdokument + "'");

            PPrikaz = rssifdok.Rows[0]["prikaz"].ToString().Trim();
            NacinRegistracije = rssifdok.Rows[0]["NacinRegistracije"].ToString().Trim();
            DokumentJe = rssifdok.Rows[0]["Vrsta"].ToString().Trim();            
            NazivKlona = rssifdok.Rows[0]["UlazniIzlazni"].ToString().Trim();
            string str = "Select Brdok,datum,ID_DokumentaStablo,ID_DokumentaTotali,Predhodni from DokumentaTotali ";
            str += " where Dokument='" + pdokument + "'" + " AND Format(Datum,'dd.MM.yy')='" + pDatum + "'";
            DataTable ProveraDok = db.ReturnDataTable(str);

            if (ProveraDok.Rows.Count > 0)
            {
                str = "SELECT * From DokumentaStablo where  Naziv = '" + pdokument + "'";
                DataTable rsDokumentaStablo = db.ReturnDataTable(str);
                if (rsDokumentaStablo.Rows.Count > 0)
                { IdDokumentaStablo = (int)rsDokumentaStablo.Rows[0]["ID_DokumentaStablo"]; }
                else
                { MessageBox.Show("Nije registrovan dokument " + pdokument + "!!"); return ""; }

                Naslov = rsDokumentaStablo.Rows[0]["NazivJavni"].ToString();
                string  opis = "Uplata na dan " + pDatum;
                clsObradaOsnovnihSifarnika cs = new clsObradaOsnovnihSifarnika();
                IdDokView = cs.UpisiDokument(ref BrojDok, opis, IdDokumentaStablo, pDatum);      
            }
            else
            {
                ProveraDok.Dispose();
                MessageBox.Show("Nije pronadjen dokumenat pod trazenim datumom");
                return "";
            }

            str = "Select Brdok,datum,ID_DokumentaStablo,ID_DokumentaTotali,Predhodni from DokumentaTotali ";
            str += " where Dokument='" + pdokument + "'" + " AND Datum='" + pDatum + "'";
            ProveraDok = db.ReturnDataTable(str);     
            
            pPrikaceniFajl = co.CitajIniFajl("LOGOVANJE","PutanjaPlacanja");
            PutanjaPlacanja = pPrikaceniFajl;            
            Program.BrDok = ProveraDok.Rows[0]["BrDok"].ToString();
            DDatum = pDatum;  //// ProveraDok.Rows[0]["Datum"].ToString();
            Predhodnik = ProveraDok.Rows[0]["Predhodni"].ToString();
            IdDokumentaStablo = (int)ProveraDok.Rows[0]["ID_DokumentaStablo"];
            KojiPrepis = "IZVOD";
            pNalog = NazivKlona;
            return PutanjaPlacanja+"#" + pDatum+ "#" + IdDokView.ToString() + "#" + KojiPrepis;
        }


        public string PrepisiNaloge(string DatumP, string KojiRacun)
        {
            PNHALKOMZ1 NZ1 = new PNHALKOMZ1();
            PNHALKOMZ2 NZ2 = new PNHALKOMZ2();
            PNHALKOMZ NZ = new PNHALKOMZ();
            clsOperacije cop = new clsOperacije();
            DataTable rspromet = new DataTable();
            DataTable rsp = new DataTable();
            int pbanka = 0;
            int Rr = 0;
            FileStream fs;// = new FileStream();

            //Dim fs, fs1
            string red = "";
            string kb = "";
            string mr = "";
            int NRB = 0;
            int BrNal = 0;
            string NBanke = "";
            string Datoteka = "";
            string str = "";
            int IdDokView = 0;
            List<string[]> lista = new List<string[]>();

            str = db.ReturnString("select getdate()", 0);
            str = "Select * from BankaView where Replace(NazivRacuna" + ", '-', '')" + " = '" + KojiRacun + "'";
            rspromet = db.ReturnDataTable(str);

            if (rspromet.Rows.Count > 0)
            { pbanka = (int)rspromet.Rows[0]["ID_BankaView"]; }
            else
            {
                MessageBox.Show("Pogresan broj tekuceg racuna ");
                return "";
            }
            int trazi = rspromet.Rows[0]["NazivBanke"].ToString().IndexOf(" ") - 1;
            NBanke = rspromet.Rows[0]["NazivBanke"].ToString().Substring(0, trazi);
            rspromet.Dispose();



            str = " Select Max(RBPrenosa) As M from PlacanjaNaplate Where ID_BankaView =" + pbanka.ToString() + " and FORMAT (datum,'dd.MM.yy')='" + DatumP + "' and PrenesenoZaPlacanje=1 and OznakaKnjizenja='10'";
            rspromet = db.ReturnDataTable(str);        
            if (rspromet.Rows.Count > 0)
                NRB = 1;
            else
            {
                NRB = (int)rspromet.Rows[0]["m"] + 1;
            }

            long Ukupno = 0;
            int BrojNaloga = 0;
            BrojNaloga = 1;

            str = "Select Count(*) as BrNaloga,Sum(isplate) as Ukupno from NaloziZaPlacanjeView Where ID_BankaView=" + pbanka.ToString();
            str += " and  FORMAT (datum,'dd.MM.yy')='" + DatumP + "' and OznakaKnjizenja='10'";
            rspromet = db.ReturnDataTable(str);

            if (rspromet.Rows[0]["Ukupno"] == null)
            { MessageBox.Show("Ne postoje nepreneseni nalozi!! ");
                return "";
            }
            //PutanjaPlacanja = cop.CitajIniFajl("LOGOVANJE", "PutanjaPlacanja");
            str = DateTime.Parse(DateTime.Now.ToString("dd.MM.yy")).ToString().Substring(0, 8);

            Datoteka = PutanjaPlacanja  + "placanje" + NBanke.Trim() + DateTime.Parse(DateTime.Now.ToString("dd.MM.yy")).ToString().Replace(".", "") + "-" + NRB.ToString().Trim() + "-" + Program.imeFirme + ".txt";
            //Datoteka = @"D:\\server\PlacanjaUplate\" + "placanje" + NBanke.Trim() + str.Replace(".", "") + "-" + NRB.ToString().Trim() + "-" + Program.imeFirme + ".txt";



            Rr = 1;

            NZ1.TekuciRacunKomitenta = KojiRacun;
            NZ1.Naziv = Program.imeFirme + " d.o.o";
            NZ1.Mesto = "Beograd";
            NZ1.DatumValute = DatumP.Replace(".", "");
            NZ1.Prazno = " ";
            NZ1.text = "MULTI E-BANK";
            NZ1.TipStavke = "0";

            Rr = 1;

            red = NZ1.TekuciRacunKomitenta + NZ1.Naziv + NZ1.Mesto + NZ1.DatumValute + NZ1.Prazno + NZ1.text + NZ1.TipStavke;

            //byte[] info = new UTF8Encoding(true).GetBytes(red);
            //fs1.Write(info, 0, info.Length);
            //fs1.Close();



            // 'red sa zbirnom stavkom
            //Rr = 2
            NZ2.TekuciRacunKomitenta = KojiRacun;
            NZ2.Naziv = Program.imeFirme.ToUpper() + " d.o.o";
            NZ2.Mesto = "Beograd";
            //NZ2.ZbirIznosaSvihNaloga = Replace(Ukupno.ToString("0000000000000.00"), ",", "");
            NZ2.ZbirIznosaSvihNaloga = Ukupno.ToString("0000000000000.00").Replace(",", "");
            NZ2.BrojPlatnihNalogaUDatoteci = BrojNaloga.ToString("00000");
            NZ2.Prazno = " ";
            NZ2.TipStavke = "9";


            string red2 = NZ2.TekuciRacunKomitenta + NZ2.Naziv + NZ2.Mesto + NZ2.ZbirIznosaSvihNaloga + NZ2.BrojPlatnihNalogaUDatoteci + NZ2.Prazno + NZ2.TipStavke;
            string[] lines = { red, red2 };
            System.IO.File.WriteAllLines(Datoteka, lines);


            //   datum = convert(datetime, '26.10.18', 4)

            str = "Select ID_DokumentaView,BrojTekucegRacuna, NazivKom,Mesto,SifraPlacanja,SifrePlacanja,PozivNaBrojDobavljaca,PozivNaBroj,ID_pozivNaBroj ,isplate,Datum,ModelPozivaNaBrojOdobrenja ";
            str += "from NaloziZaPlacanjeView  ";
            str += "Where ID_BankaView=" + pbanka.ToString();
            str += " and   FORMAT (datum,'dd.MM.yy')='" + DatumP + "'   And OznakaKnjizenja='10' Order by iid ";

            rspromet = db.ReturnDataTable(str);

            if (rspromet.Rows.Count > 0)
            {
                IdDokView = Convert.ToInt32(rspromet.Rows[0]["ID_DokumentaView"]); 
            }
            else { IdDokView  = 1; }





            string red3 = "";
            foreach (DataRow row in rspromet.Rows)
            {
                Rr = Rr + 1;

            if (row["BrojTekucegRacuna"].ToString() != "")
            {
                str = row["BrojTekucegRacuna"].ToString();
                NZ.TekuciRacunPartnera = NZ.TekuciRacunPartnera = FormatirajRacun(str);
            }

            NZ.NazivPrimaoca = row["NazivKom"].ToString();
            NZ.MestoPrimaoca = row["Mesto"].ToString();
            NZ.PopunjenoSa0 = "0";
            NZ.ModelPozivaNaBrojZaduzenja = "";
            NZ.PozivNaBrojZaduzenja = "";
            NZ.SifraPlacanja = row["SifraPlacanja"].ToString();            
            NZ.ModelPozivaNaBrojOdobrenja = "";
            NZ.PozivNaBrojOdobrenja = row["PozivnaBrojDobavljaca"].ToString();

            if (row["PozivNaBroj"].ToString().IndexOf("121-") > 0)
            {
                rsp = db.ReturnDataTable("Select Distinct ID_PDVUlazniJciTotali as Iid,BrDok from PDVUlazniJciTotali where ID_RacunZaPlacanje=" + row["ID_PozivNaBroj"].ToString());
                if (rsp.Rows.Count > 0)
                {
                    mr = row["PozivnaBrojDobavljaca"].ToString().Replace("-", "");
                        clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                    kb = os.KB_97(mr);

                    NZ.PozivNaBrojOdobrenja = kb.ToString() + "-" + row["PozivnaBrojDobavljaca"].ToString();
                    NZ.SifraPlacanja = "53";
                    NZ.ModelPozivaNaBrojOdobrenja = "97";
                    NZ.SvrhaPlacanja = "Uplata tekucih prihoda";
                }
            }

            if (NZ.SifraPlacanja == "54")
            {
                NZ.ModelPozivaNaBrojOdobrenja = "97";
                NZ.SvrhaPlacanja = "PID-" + Convert.ToDateTime(row).Month.ToString() + "-" + Convert.ToDateTime(row["Datum"]).Year.ToString();
            }


            if (NZ.SifraPlacanja == "40") { NZ.ModelPozivaNaBrojOdobrenja = "97"; }

            NZ.PopunjenoSa5x0 = "00000";
            NZ.Prazno = " ";
            NZ.OblikPlacanja = "2";
            NZ.OblikPlacanja1 = " ";
            NZ.Prazno1 = " ";


            NZ.iznos = Math.Round(Convert.ToDecimal(row["isplate"]), 2).ToString("00000000000.00").Replace(",", "");

            NZ.TipDokumenta = "0";
            NZ.TipStavke = "1";


            red3 += NZ.TekuciRacunPartnera + NZ.NazivPrimaoca + NZ.MestoPrimaoca + NZ.PopunjenoSa0 + NZ.ModelPozivaNaBrojZaduzenja + NZ.PozivNaBrojZaduzenja +
                 NZ.SvrhaPlacanja + NZ.PopunjenoSa5x0 + NZ.Prazno + NZ.OblikPlacanja + NZ.SifraPlacanja + NZ.OblikPlacanja1 + NZ.Prazno1 + NZ.iznos +
                 NZ.ModelPozivaNaBrojOdobrenja + NZ.PozivNaBrojOdobrenja + NZ.DatumValute + NZ.TipDokumenta + NZ.TipStavke + Environment.NewLine ;


                string strParams = "";
                strParams = "@param1=" + pbanka + "`";
                strParams += "@param2=" + IdDokView.ToString() + "`";
                strParams += "@param3=" + 0;

                str = "Update  PlacanjaNaplate set PrenesenoZaPlacanje=1,RBPrenosa=" + NRB.ToString();
                str += " Where ID_BankaView= @param1 and  ID_DokumentaView=@param2  and PrenesenoZaPlacanje=@param3";

                lista.Add(new string[] { str, strParams });                
                lista.ToArray();
                string rezultat = db.ReturnSqlTransactionParams(lista);
                lista.Clear();
                if (rezultat != "") { MessageBox.Show(rezultat);}

                Dictionary<string, string> rezult = db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PripremaZaplacanje", "IdDokument:" + (int)IdDokView);

                strParams = "";
                strParams = "@param1=" + IdDokView + "`";
                strParams += "@param2=" + pbanka;
                str = "DELETE from PlateAnalitike where ID_DokumentaView=@param1 and ID_BankaView=@param2";
                lista.Add(new string[] { str, strParams });
                lista.ToArray();
                 rezultat = db.ReturnSqlTransactionParams(lista);
                lista.Clear();
                if (rezultat != "") { MessageBox.Show(rezultat); }

                //pp = pp + 1
            }
            string[] lineskraj = { red, red2, red3 };
            System.IO.File.WriteAllLines(Datoteka, lineskraj);
            if (File.Exists(Datoteka)  == false) { return ""; }
            
            return " ";
        }

        public string FormatirajRacun(string racun)
        {
           string FormatirajRacun = "";
          string  racunulaz = racun.Replace("-", "");

            FormatirajRacun = racunulaz.Substring(0, 3) + String.Format(racunulaz.Substring(4, racunulaz.Length - 5),"0000000000000") + racun.Substring(racun.Length - 2, 2);

            return FormatirajRacun;
        }  
    }
}