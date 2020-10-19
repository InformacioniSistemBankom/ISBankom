using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Bankom.Class
{
  
    class clsXmlPlacanja
    {
      
        DataBaseBroker db = new DataBaseBroker();
        DataTable rssifdok = new DataTable();        
        public string pDatum = "";

        public string mPozivNaBroj = "";
        public string mPN = "";
        public string Naslov = "";
        public int IdDokumentaStablo = 0;
        public string KiliA = "";
        public string mTekuci = "";


        public void izborPlacanja(int izbor, string mesecgodina) //(0-prevez,1-nagrada,2-plata)
        {
            
            switch (izbor)
            {
                case 0:
                    prevozXml(mesecgodina);  //FORMIRANJE PPPPD PREVOZ
                    break;
                case 1:
                    NagradeXml(mesecgodina); //FORMIRANJE PPPPD NAGRADE
                    break;
                case 2:
                    PlateXml(mesecgodina); ////FORMIRANJE PPPPD PLATE
                    break;
                case 3:
                    UvozPlacanjePlate(); // UVOZ PLATE
                        break;
                case 4:
                    Uvozplacanjeprevoz(); // UVOZ PREVOZ
                    break;                                    
                default:
                    break;


            }
        }
     
        private void Uvozplacanjeprevoz()
        {
            DataBaseBroker db = new DataBaseBroker();
            string pdokument = "PripremaZaPlacanje";
            string KojaObrada = "Prevoz";
            string pdat = "";
            int IdDokView = 0;
            string str = "";
            string mPoruka = "";
            int pbanka;
            bool ins = false;
            var lista = new List<string>();
            lista.Clear();
            var  tekuciRacun= Prompt.ShowDialog("0", "Formiranje naloga za knjizenje prevoza", "Izaberite 0 - prevoz ili 1 - nagrada ");
           
            clsOperacije co = new clsOperacije();
            bool r = co.IsNumeric(tekuciRacun);
            if (r == false) { MessageBox.Show("Nekorektan unos"); return; }
            int pIzbor = Convert.ToInt16(tekuciRacun);
            //pDatum = DateTime.Now.AddDays(-94).ToString("dd.MM.yy"); // za testiranje
            pDatum = DateTime.Now.ToString("dd.MM.yy");
            switch (pIzbor)
            {
                case 0:
                    mPN = "P";
                    break;
                case 1:
                    mPN = "N";
                    break;
                default:
                    MessageBox.Show("Nekorektan unos");
                    return;
                    

            }

            pdat = Prompt.ShowDialog(pdat, "Uvoz prevoza ", "Unesite mesec i godinu za koji uvozimo prevoz ");
            if (pdat == "") { return; }
            tekuciRacun = Prompt.ShowDialog("", "Uvoz prevoza ","Unesite tekuci racun sa kojeg vrsimo isplatu " );
            if (tekuciRacun == "") { return; }
          //  pdat = DateTime.Now.ToString("dd.MM.yy").Substring(3, 2) + DateTime.Now.ToString("dd.MM.yy").Substring(6, 2);
           


            DataTable rsu =  db.ReturnDataTable("Select * from BankaView where Replace(NazivRacuna,'-','')='" + tekuciRacun.Replace("-", "") + "'");

            if (rsu.Rows.Count > 0)
                pbanka = (int)rsu.Rows[0]["ID_BankaView"];
            else
            {
                MessageBox.Show("Pogresan broj tekuceg racuna ");
                return;
            }
            rsu.Dispose();
            var pozivNaBroj = Prompt.ShowDialog("", "Uvoz prevoza ","Unesite poziv na broj dobijen od UPRAVE ZA JAVNA PLACANJA  " );

            if (pozivNaBroj == "" ) { return; }
            str += "Select Brdok,datum,ID_DokumentaStablo,ID_DokumentaTotali from DokumentaTotali,OrganizacionaStrukturaStavkeView ";
            str += " where Dokument='" + pdokument + "'" + " AND Datum='" + pDatum + "' and id_OrganizacionaStrukturaView = ";
            str += "ID_OrganizacionaStrukturaStavkeView  And ID_OrganizacionaStrukturaStablo=" + Program.idFirme.ToString();
            rsu = db.ReturnDataTable(str);

            if (rsu.Rows.Count == 0 )
            {
                rsu.Dispose();
                DataTable rsDokumentaStablo = db.ReturnDataTable("SELECT * From DokumentaStablo where  Naziv='" + pdokument + "'");
                 if (rsDokumentaStablo.Rows.Count > 0 )
                {
                    IdDokumentaStablo = (int)rsDokumentaStablo.Rows[0]["ID_DokumentaStablo"];
                    Naslov = rsDokumentaStablo.Rows[0]["NazivJavni"].ToString();
                }else
                {
                    MessageBox.Show("Nije registrovan dokument " + pdokument + "!!");
                    return;
                }

                var oOpis = "Priprema za placanje " + pDatum;
                string brojDok = "";
                clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                IdDokView = os.UpisiDokument(ref brojDok, oOpis, IdDokumentaStablo, pDatum.ToString());
                ins = true;
            }
            IdDokView = ins ? IdDokView : Convert.ToInt32(rsu.Rows[0]["ID_DokumentaTotali"]);

            KojaObrada = "Prevoz";
            mPoruka = ObradiPlatu(pdat, pDatum, pbanka, IdDokView, tekuciRacun, Program.idkadar, KojaObrada, pozivNaBroj, ref mPoruka);
            MessageBox.Show(mPN == "" ? mPoruka : "Zavrsen uvoz prevoza");
        }
        private void UvozPlacanjePlate()
        {
            DataBaseBroker db = new DataBaseBroker();
            string tekuciRacun = "";
            const string pdokument = "PripremaZaPlacanje";
            string pozivNaBroj = "";
            string mPoruka = "";
            int idDokView=0;
            bool ins = false;

            List<string[]> list = new List<string[]>();



            PONOVO:

            string svrsta = Prompt.ShowDialog("K", "Uvoz plata u placanja", "Unesite vrstu obracuna: A za akontaciju ili K za platu ");
            if (svrsta == "") { return; }
            svrsta = svrsta.ToUpper();

            if (svrsta != "" && svrsta != "a".ToUpper() && svrsta != "K".ToUpper())
            {
                MessageBox.Show("Pogresno unesena vrsta obracuna moze samo A ili K PONOVITE!!!!!");
                { goto PONOVO; }

            }

            
            pDatum = DateTime.Now.ToString("dd.MM.yy");
            //pDatum = DateTime.Now.AddDays(-13).ToString("dd.MM.yy");
            string mg = Prompt.ShowDialog(pDatum.Substring(3, 2) + pDatum.Substring(6, 2), "Uvoz plata u placanja", "Unesite mesec i godinu za koji formiramo uvozimo plate ");
            if (string.IsNullOrEmpty(mg)) { return; }

            clsOperacije co = new clsOperacije();
            co.IsNumeric(mg);
            if (co.IsNumeric(mg) == false) { MessageBox.Show("Nekorektan unos"); return; }
            if (mg.Length != 4) { MessageBox.Show("Nekorektan unos"); return; }


            KiliA = svrsta;
          
            tekuciRacun = Prompt.ShowDialog(tekuciRacun, "Uvoz plata u placanja", "Unesite tekuci racun sa kojeg vrsimo isplatu");
            if (tekuciRacun == "") { return; }
            mTekuci = tekuciRacun;
            DataTable rsu = db.ReturnDataTable("Select * from BankaView where Replace(NazivRacuna,'-','')='" + tekuciRacun.Replace("-", "") + "'");
            if (rsu.Rows.Count == 0)
            {
                MessageBox.Show("Pogresan broj tekuceg racuna ");
                return;
            }

            var pbanka = (int)rsu.Rows[0]["ID_BankaView"];
            rsu.Dispose();
        Outer:
            {
                pozivNaBroj = Prompt.ShowDialog(pozivNaBroj, " Uvoz plata u placanja ", " Unesite poziv na broj dobijen od UPRAVE ZA JAVNA PLACANJA");
            }
            if (pozivNaBroj == "")
            { goto Outer; }

            var str = "Select Brdok,datum,ID_DokumentaStablo,ID_DokumentaTotali from DokumentaTotali,OrganizacionaStrukturaStavkeView ";
            str += " where Dokument='" + pdokument + "'" + " AND Format(Datum,'dd.MM.yy') = '" + pDatum + "' and id_OrganizacionaStrukturaView =";
            str += "ID_OrganizacionaStrukturaStavkeView  And ID_OrganizacionaStrukturaStablo=" + Program.idFirme.ToString();
            rsu = db.ReturnDataTable(str);

            if (rsu.Rows.Count == 0)
            {
                 


                DataTable rsDokumentaStablo = db.ReturnDataTable(" SELECT * From DokumentaStablo where Naziv='" + pdokument + "'");
                if (rsDokumentaStablo.Rows.Count == 0)
                {
                    MessageBox.Show("Nije registrovan dokument " + pdokument + "!!");
                    return;
                }
                
                
                IdDokumentaStablo = (int)rsDokumentaStablo.Rows[0]["ID_DokumentaStablo"];
                Naslov = rsDokumentaStablo.Rows[0]["NazivJavni"].ToString();
                


                var oOpis = "Priprema za placanje " + pDatum;
                string brojDok = "";
                clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                idDokView = os.UpisiDokument(ref brojDok, oOpis, IdDokumentaStablo, pDatum.ToString());
                ins = true;
            }

            idDokView = ins ?idDokView : (int)rsu.Rows[0]["ID_DokumentaTotali"];
           
            const string kojaObrada = "Plate";
            mPN = "";
            mPoruka = ObradiPlatu(mg, pDatum, pbanka, idDokView, tekuciRacun, Program.idkadar, kojaObrada, pozivNaBroj, ref mPoruka);
            MessageBox.Show(mPoruka == "" ? mPoruka : "Zavrsen uvoz plata");
        }

        private string ObradiPlatu(string mesgod, string Datum, int pbanka, int IdDokView, string tr, int KojiRadnik, string KojiPrenos, string PozivNaBroj, ref string mPoruka)
        {
            
            DataBaseBroker db = new DataBaseBroker();
            string obradiPlatu = "";
            string poruka = "";
            string mTekuci = tr;
            mPozivNaBroj = PozivNaBroj;            
            var str = "";
            bool popuni = false;
            
            //PLACANJA:
            //            ' provera da li postoji dokument priprema za placanje u koji cemo vrsiti upis
            str = " select * from PripremazaplacanjeTotali where ID_BankaView=" + pbanka + " and ID_PripremazaplacanjeTotali=" + IdDokView.ToString();
            str += " AND  OznakaKnjizenja='10'  and (sifraplacanja=40 Or sifraplacanja=41 or sifraplacanja=54 or NazivKom LIKE 'PRIVREDNA KOMORA%')";
            DataTable rsu = db.ReturnDataTable(str);

            if (rsu.Rows.Count == 0)
            {
               MessageBox.Show("Procedura: Obradi platu - greska"); return "";
            }


            var lista = new List<string>();

            for (int g = 0; g < rsu.Rows.Count; g++)
            {
                
                lista.Add(" Delete from PlacanjaNaplate where ID_PlacanjaNaplate=" + rsu.Rows[g]["iid"].ToString());
                
            }
            string vrati = db.ExecuteSqlTransaction(lista);
            if (vrati !="") {  lista.Clear(); MessageBox.Show(vrati); }
            lista.Clear();
           db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PripremaZaplacanje", "IdDokument:" + (int)IdDokView);
            DateTime d = DateTime.Now;
            pDatum = d.ToString("dd.MM.yy");

            if (KojiPrenos == "Prevoz")
            {
                popuni = mPN == "P" ? PopuniPrevozUPlacanjaINaplate(IdDokView, pbanka, pDatum, mesgod) : PopuniNagradeUPlacanjaINaplate(IdDokView, pbanka, pDatum, mesgod);
            }


            else
            {
                // if (Popuni == false) 
                
                    popuni = PopuniPlateUPlacanjaINaplate(IdDokView, pbanka, pDatum, mesgod);
                 
                if (popuni == false)
                {
                    mPoruka = poruka;
                    return obradiPlatu;
                }
            }


            //MessageBox.Show("Zavrseno!!");
            obradiPlatu = "";
            return obradiPlatu;



        }
        private bool PopuniPlateUPlacanjaINaplate(int IdDokView, int pbanka, string pDatum, string mesgod)
        {
            var lista = new List<string>();
            string str = "";
            string strParams = "";
            //KiliA je izbor K ili A    mesegod 4cifre mesecgodina
            //idke je program.idkadr
            //idfirme je Program.idOrgDeo.ToString()

            strParams = "";
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView,ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,OznakaKnjizenja,OblikPlacanja,UUser)";
            str += " Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView, 61 as ID_SvrhaPlacanja, sum(nld-pomoc) as Isplate, 1,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML  where  ind='P' AND  id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "'";
            lista.Add(str);
            lista.ToArray();
            // 1.Obra~un zarada - neto zarada bolovanje
            str = "";
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView,ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,OznakaKnjizenja,OblikPlacanja,UUser)";
            str += " Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView, ID_Artikli as ID_SvrhaPlacanja, sum(nld) as Isplate, 1,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML,Artikli  where ind='B' AND  Artikli.StaraSifra=922 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "'  group by ID_Artikli";
            lista.Add(str);
            lista.ToArray();

            //'2. Obra~un zarada-porez bez bolovanja
            str = "";
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,OznakaKnjizenja,OblikPlacanja,UUser) ";
            str += " Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja, sum(porez),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where ind='P' AND  a.Starasifra=52 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
            str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);
            lista.ToArray();
            //      '2a. Obra~un zarada-porez  bolovanje
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,OznakaKnjizenja,OblikPlacanja,UUser) ";
            str += " Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja, sum(porez),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where ind='B' AND  a.Starasifra=925 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
            str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);
            lista.ToArray();

            //  '3. Obra~un zarada-doprinosi pio-radnik
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
            str += " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString();
            str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
                str += " sum(dop1),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where ind='P' AND a.Starasifra=53 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
            str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);
            lista.ToArray();

            //'3a. Obra~un zarada-doprinosi pio-poslodavac
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
            str += " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString();
            str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
            str += " sum(dopp1),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where ind='P' AND a.Starasifra=56 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
            str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);
            lista.ToArray();

            //     '4. Obra~un zarada-doprinosi zdravstvo radnik
                str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
                str += " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString();
                str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
                str += " sum(dop2),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
                str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
                str += " where ind='P' AND a.Starasifra=54 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
                str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);
            lista.ToArray();


            //'4a. Obra~un zarada-doprinosi zdravstvo poslodavac
            str =  " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
            str += " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString();
            str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
            str += " sum(dopp2),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where ind='P' AND a.Starasifra=57 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
            str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);
            lista.ToArray();


            //'5. Obra~un zarada-doprinosi nezaposlenost radnik
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
            str += " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString();
            str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
            str += " sum(dop3),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where ind='P' AND a.Starasifra=55 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
            str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);
            lista.ToArray();


            //'5a. Obra~un zarada-doprinosi nezaposlenost poslodavac

                str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
                str += " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString();
                str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
                str += " sum(dopp3),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
                str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
                str += " where ind='P' AND a.Starasifra=58 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
                str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);
            lista.ToArray();


            // '5b. Obra~un zarada-pomoc
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
            str += " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + ","  + IdDokView.ToString();
            str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
            str += " sum(pomoc),ID_TekuciRacuniKomitenata ,10,3," + Program.idkadar.ToString();
            str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where  a.Starasifra=9298 AND ai.ID_ArtikliView=a.ID_Artikli and t.BrojTekucegRacuna='" + mTekuci;
            str += "' AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli having sum(pomoc)>0";
            lista.Add(str);
            lista.ToArray();
            string resultat = db.ExecuteSqlTransaction(lista);
            lista.Clear();

            if (resultat != "") { MessageBox.Show("Greska-PopuniPlateUPlacanjaINaplate"); return false; } 
            //'''BOLOVANJA

            //      '1. Obra~un zarada-doprinosi pio-radnik
                str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
                str += " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString();
                str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
                str += " sum(dop1),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
                str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
                str += " where ind='B' AND a.Starasifra=2701 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
            str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";
                lista.Add(str);
                lista.ToArray();

            // '1a. Obra~un zarada-doprinosi pio-poslodavac
                str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
                str += " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString();
                str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
                str += " sum(dopp1),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
                str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
                str += " where ind='B' AND a.Starasifra=923 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
                str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);
            lista.ToArray();

            //'2. Obra~un zarada-doprinosi zdravstvo radnik
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
            str+= " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString();
            str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
            str += " sum(dop2),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where ind='B' AND a.Starasifra=2700 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
            str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);
            lista.ToArray();

            //'2a. Obra~un zarada-doprinosi zdravstvo poslodavac
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
            str += " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString();
            str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
            str += " sum(dopp2),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where ind='B' AND a.Starasifra=2699 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
            str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";

            lista.Add(str);
            lista.ToArray();

            //'3. Obra~un zarada-doprinosi nezaposlenost radnik
                str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
                str += " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString();
                str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
                str += " sum(dop3),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
                str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
                str += " where ind='B' AND a.Starasifra=2702 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
                str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);
            lista.ToArray();



            // '3a. Obra~un zarada-doprinosi nezaposlenost poslodavac
            str += " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,";
                str += " OznakaKnjizenja,OblikPlacanja,UUser) Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString();
                str += " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja,";
                str += " sum(dopp3),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
                str += " from PPPPD_XML as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
                str += " where ind='B' AND a.Starasifra=924 AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
            str += " AND p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' group by ID_TekuciRacuniKomitenata,ID_Artikli";

            lista.Add(str);
            lista.ToArray();
            resultat = db.ExecuteSqlTransaction(lista);
            if (resultat != "") { MessageBox.Show("Greska-PopuniPlateUPlacanjaINaplate"); return false; }
            lista.Clear();


            //    DODATAK ZA ANALITIKE
           str = "Delete from PlateAnalitike WHERE ID_BankaView=" + pbanka.ToString() + " AND( Datum='" + pDatum + "' OR + ID_DokumentaView = " + IdDokView.ToString();

///'1. Uplate na tekuce racune radnika
            str = " Insert into PlateAnalitike (Datum,ID_BankaView,ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,BrojTekucegRacuna,";
            str += " OznakaKnjizenja,OblikPlacanja,PozivNaBrojDobavljaca,ModelPozivaNaBrojOdobrenja,NazivKom,Mesto,SifraPlacanja,SifrePlacanja)";
            str += " Select '" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView, 61, nld-obust, nispla,10,2,'" + mPozivNaBroj + "',97,";
            str += " Prezime+ime AS ime,mstan,a.SifraPlacanja,a.SifrePlacanja from PPPPD_XML as p,AnalitikaSifraPlacanjaStavkeView as a  ";
            str += " where a.ID_ArtikliView=61 and ltrim(rtrim(nacinpl))<>'0' AND  id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' Order By ime ";
            lista.Add(str);
            lista.ToArray();


            //  '   Sledeca dva reda idu kao kompenzacija
            //'--------------------------------------------------------
            //'2. Obustave telefoni
            str = " Insert into PlateAnalitike (Datum,ID_BankaView,ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,BrojTekucegRacuna,";
            str += " OznakaKnjizenja,OblikPlacanja,PozivNaBrojDobavljaca,NazivKom,Mesto,SifraPlacanja,SifrePlacanja,ID_KomitentiView)";
            str += " Select '" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView, art.id_artikli, telefon,'" + mTekuci + "',10,3,' ',";
            str += " r.ime,r.mstan,a.SifraPlacanja,a.SifrePlacanja,1 ";
            str += " from plate as p,radnik as r,AnalitikaSifraPlacanjaStavkeView as a,Artikli as art  ";
            str += " where telefon>0 AND art.starasifra=8859 AND art.ID_Artikli=a.ID_ArtikliView AND p.id_firma=r.id_firma and p.mbr=r.mbr ";
            str += " and p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "' Order By r.mbr";
            lista.Add(str);
            lista.ToArray();


            //       '3. Obustave krediti
            str = " Insert into PlateAnalitike (Datum,ID_BankaView,ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,BrojTekucegRacuna,";
            str += " OznakaKnjizenja,OblikPlacanja,PozivNaBrojDobavljaca,NazivKom,Mesto,SifraPlacanja,SifrePlacanja,ID_KomitentiView)";
            str += " Select '" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView, 1301, k.rata,kr.RacKred,10,2,k.PozivNaBroj,";
            str += " kr.NazKred,' ',a.SifraPlacanja,a.SifrePlacanja,1 ";
            str += " from plate as p,radnik as r,AnalitikaSifraPlacanjaStavkeView as a,krediti as k,kreditori as kr  ";
            str += " where  p.krediti>0 AND ispl<iznkred and k.kreditor=kr.sifkred And k.mbr=p.mbr AND a.ID_ArtikliView=1301 AND p.id_firma=k.id_firma AND p.id_firma=r.id_firma and p.mbr=r.mbr and p.id_firma = " + Program.idOrgDeo.ToString();
            str += " and mes='" + mesgod + KiliA + "' Order By r.mbr";
            lista.Add(str);
            lista.ToArray();


            //       '4. Obustava ALIMENTACIJA
            str += " Insert into PlateAnalitike (Datum,ID_BankaView,ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,BrojTekucegRacuna,";
            str += " OznakaKnjizenja,OblikPlacanja,PozivNaBrojDobavljaca,NazivKom,Mesto,SifraPlacanja,SifrePlacanja,ID_KomitentiView)";
            str += " Select '" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView, a.ID_ArtikliView,  sudobu,o.zr,10,2,' ',";
            str += " r.ime,r.mstan,a.SifraPlacanja,a.SifrePlacanja,1";
            str += " from plate as p,radnik as r,AnalitikaSifraPlacanjaStavkeView as a,obustave as o  ";
            str += " where sudobu>0 AND a.ID_ArtikliView=64 AND p.mbr=o.mbr  and p.id_firma = o.id_firma AND p.mbr=r.mbr ";
            str += " and p.id_firma = " + Program.idOrgDeo.ToString() + " and p.mes='" + mesgod + KiliA + "' Order By r.mbr";
            lista.Add(str);
            lista.ToArray();
            resultat = db.ExecuteSqlTransaction(lista);
            lista.Clear();
            if (resultat != "") { MessageBox.Show("Greska-PopuniPlateUPlacanjaINaplate"); return false; }

            //  '4. Obustava Sindikalna clanarina
            DataTable rsp = db.ReturnDataTable("Select sum(clan)as iznos from plate as p where clan>0  and p.id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + mesgod + KiliA + "'");
        if (rsp.Rows.Count == 0) { return true ; }
            if (rsp.Rows[0]["iznos"] == null) { return true; }

            //       ' Sindikat-1 12%  od ukupne sume
            str = " Insert into PlateAnalitike (Datum,ID_BankaView,ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,BrojTekucegRacuna,";
            str += " OznakaKnjizenja,OblikPlacanja,PozivNaBrojDobavljaca,NazivKom,Mesto,SifraPlacanja,SifrePlacanja,ID_KomitentiView)";
            str += " Select '" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView, art.id_artikli," + (Convert.ToInt32(rsp.Rows[0]["iznos"]) * 0.12).ToString() + ",a.racun ,10,2,' ',";
            str += " 'SINDIKAT NEZAVISNOST',' ',a.SifraPlacanja,a.SifrePlacanja,1 ";
            str += " from AnalitikaSifraPlacanjaStavkeView as a,Artikli as art  ";
            str += " where  art.starasifra=9189 AND art.ID_Artikli=a.ID_ArtikliView ";
            lista.Add(str);
            lista.ToArray();


            //       ' Sindikat-2 24% od ukupne sume
            str = " Insert into PlateAnalitike (Datum,ID_BankaView,ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,BrojTekucegRacuna,";
            str += " OznakaKnjizenja,OblikPlacanja,PozivNaBrojDobavljaca,NazivKom,Mesto,SifraPlacanja,SifrePlacanja,ID_KomitentiView)";
            str += " Select '" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView, art.id_artikli," + (Convert.ToInt32(rsp.Rows[0]["iznos"]) * 0.24).ToString() + ",a.racun,10,2,' ',";
            str += " 'SINDIKAT NEZAVISNOST',' ',a.SifraPlacanja,a.SifrePlacanja,1 ";
            str += " from AnalitikaSifraPlacanjaStavkeView as a,Artikli as art  ";
            str += " where  art.starasifra=9190 AND art.ID_Artikli=a.ID_ArtikliView ";
            lista.Add(str);
            lista.ToArray();


            //' Sindikat-2 64%  od ukupne sume
            str = " Insert into PlateAnalitike (Datum,ID_BankaView,ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,BrojTekucegRacuna,";
            str += " OznakaKnjizenja,OblikPlacanja,PozivNaBrojDobavljaca,NazivKom,Mesto,SifraPlacanja,SifrePlacanja,ID_KomitentiView)";
            str += " Select '" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView, art.id_artikli," + (Convert.ToInt32(rsp.Rows[0]["iznos"]) * 0.64).ToString() + ",a.racun,10,2,' ',";
            str += " 'SINDIKAT NEZAVISNOST',' ',a.SifraPlacanja,a.SifrePlacanja,1 ";
            str += " from AnalitikaSifraPlacanjaStavkeView as a,Artikli as art  ";
            str += " where  art.starasifra=9191 AND art.ID_Artikli=a.ID_ArtikliView ";
            lista.Add(str);
            lista.ToArray();
            resultat = db.ExecuteSqlTransaction(lista);
            lista.Clear();
            if (resultat != "") { MessageBox.Show("Greska-PopuniPlateUPlacanjaINaplate"); return false; }
            return true;


        }
        private bool PopuniNagradeUPlacanjaINaplate(int IdDokView, int pbanka, string pDatum, string mesgod)
        {

            ///PLACANJA
            bool PopuniNagradeUPlacanjaINaplate = true;
            string str = "";
            string mstarasifra = "";
            string mstarasifraprevoz = "";
            string resultat = ""; 
            var lista = new List<string>();
            if (Program.idFirme == 5 || Program.idFirme == 6 || Program.idFirme == 9)
            {
                mstarasifra = "3212";//   ' porez na jubilarne nagrade
                mstarasifraprevoz = "3211";//  'jubilarna nagrada
            }

            else
            {
                mstarasifra = "9224";//         ' porez na jubilarne nagrade
                mstarasifraprevoz = "6917";//  ''' 88  'jubilarna nagrada
            }

            //idke = KojiRadnik
            //   '1. Porez na prevoz
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,OznakaKnjizenja,OblikPlacanja,UUser) ";
            str += " Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja, sum(porez),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML_Nagrade as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where a.Starasifra=" + mstarasifra + " AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna  AND Porez>0 ";
            str += " AND p.id_firma = " + Program.idFirme.ToString() + " and mesdop='" + mesgod + "K' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);


            //'1.Ukupan prevoz
            resultat = "";
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,OznakaKnjizenja,OblikPlacanja,UUser) ";
            str += " Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja, sum(nagrade),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML_Nagrade as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where a.Starasifra=" + mstarasifraprevoz + " and nagradaprekoracuna='True' AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
            str += " AND p.id_firma = " + Program.idFirme.ToString() + " and mesdop='" + mesgod + "K' group by ID_TekuciRacuniKomitenata,ID_Artikli";
            lista.Add(str);


            //' DODATAK ZA ANALITIKE
            str = "";
            str = "Delete from PlateAnalitike WHERE ID_BankaView=" + pbanka.ToString() + " AND ID_DokumentaView=" + IdDokView.ToString();
            lista.Add(str);

            //'1. Uplate na tekuce racune radnika
            str = " Insert into PlateAnalitike (Datum,ID_BankaView,ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,BrojTekucegRacuna,";
            str += " OznakaKnjizenja,OblikPlacanja,PozivNaBrojDobavljaca,ModelPozivaNaBrojOdobrenja,NazivKom,Mesto,SifraPlacanja,SifrePlacanja)";
            str += " Select '" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView,a.sifra, Nagrade, nispla,10,2,'" + mPozivNaBroj + "',97,";
            str += " Prezime+ime AS ime,mstan,a.SifraPlacanja,a.SifrePlacanja from PPPPD_XML_Nagrade as p,AnalitikaSifraPlacanjaStavkeView as a  ";
            str += " where Nagrade>0 AND  a.StaraSifra=" + mstarasifraprevoz + " and nagradaprekoracuna='True' AND  id_firma = " + Program.idFirme.ToString() + " and mesdop='" + mesgod + "K' Order By ime ";
            lista.Add(str);

            resultat = db.ExecuteSqlTransaction(lista);
            if (resultat !="") { PopuniNagradeUPlacanjaINaplate = false; }
            lista.Clear();
            return PopuniNagradeUPlacanjaINaplate;
        }

        private bool PopuniPrevozUPlacanjaINaplate(int IdDokView, int pbanka, string pDatum, string mesgod)
        {
            var lista = new List<string>();
            string rezultat = "";

            bool popuniPrevozUPlacanjaINaplate = true;
            string str = "";
            string mstarasifra = "";
            string mstarasifraprevoz = "";
            if (Program.idFirme == 5 || Program.idFirme == 6 || Program.idFirme == 9)
            {
                mstarasifra = "4945";//   ' porez na prevoz
                mstarasifraprevoz = "4946";//  'prevoz
            }
            else
            {
                mstarasifra = "11001";//   ' porez na prevoz
                mstarasifraprevoz = "88";//  'prevoz
            }


            // '1. Porez na prevoz
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,OznakaKnjizenja,OblikPlacanja,UUser) ";
            str += " Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja, sum(porez),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML_Prevoz as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where a.Starasifra=" + mstarasifra + " AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna  AND Porez>0 ";
            str += " AND p.id_firma = " + Program.idFirme.ToString() + " and mesdop='" + mesgod + "K' group by ID_TekuciRacuniKomitenata,ID_Artikli";

            lista.Add(str);

            
            //'1.Ukupan prevoz
            str = " Insert into PlacanjaNaplate (PozivNaBrojSaIzvoda,Datum,ID_BankaView, ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,ID_TekuciRacuniView,OznakaKnjizenja,OblikPlacanja,UUser) ";
            str += " Select '" + mPozivNaBroj + "','" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView,ID_Artikli as ID_SvrhaPlacanja, sum(prevoz),ID_TekuciRacuniKomitenata ,10,2," + Program.idkadar.ToString();
            str += " from PPPPD_XML_Prevoz as p ,VezaAnalitikeISifrePlacanjaZaIzvod as ai,tekuciRacuniKomitenata as t,artikli as a ";
            str += " where a.Starasifra=" + mstarasifraprevoz + " and prevozprekoracuna='True' AND ai.ID_ArtikliView=a.ID_Artikli and ai.racun=t.BrojTekucegRacuna ";
            str += " AND p.id_firma = " + Program.idFirme.ToString() + " and mesdop='" + mesgod + "K' group by ID_TekuciRacuniKomitenata,ID_Artikli";

            lista.Add(str);

            //' DODATAK ZA ANALITIKE
            str = "";
            str = "Delete from PlateAnalitike WHERE ID_BankaView=" + pbanka.ToString() + " AND ID_DokumentaView=" + IdDokView.ToString();
            lista.Add(str);

            str = "";

            //'1. Uplate na tekuce racune radnika
            str = " Insert into PlateAnalitike (Datum,ID_BankaView,ID_DokumentaView,ID_SvrhaPlacanjaView,Isplate,BrojTekucegRacuna,";
            str += " OznakaKnjizenja,OblikPlacanja,PozivNaBrojDobavljaca,ModelPozivaNaBrojOdobrenja,NazivKom,Mesto,SifraPlacanja,SifrePlacanja)";
            str += " Select '" + pDatum + "'," + pbanka.ToString() + "," + IdDokView.ToString() + " as ID_DokumentaView, 61, Prevoz, nispla,10,2,'" + mPozivNaBroj + "',97,";
            str += " Prezime+ime AS ime,mstan,a.SifraPlacanja,a.SifrePlacanja from PPPPD_XML_Prevoz as p,AnalitikaSifraPlacanjaStavkeView as a  ";
            str += " where Prevoz>0 AND  a.StaraSifra=" + mstarasifraprevoz + "  and prevozprekoracuna='True' AND  id_firma = " + Program.idFirme.ToString() + " and mesdop='" + mesgod + "K' Order By ime ";

            lista.Add(str);
            rezultat = db.ExecuteSqlTransaction(lista);
            if (rezultat != "") { lista.Clear(); MessageBox.Show(rezultat); popuniPrevozUPlacanjaINaplate = false; };
            lista.Clear();
            return popuniPrevozUPlacanjaINaplate;


        }
       
       
        private void PlateXml(string mg)
        {

            var lista = new List<string>();
            string vprimanja = mg.Trim().Substring(mg.Length - 1);
            mg  = mg.Trim().Substring(0,mg.Length - 1);
            string pdat = mg; //mesecgodina
            int IdDokumentaStablo = 0;
            string mPoruka = "";
            string rezultat = "";
            string pdokumentf = "KnjizenjePlata";
            int RedniBroj = 0;
            string Naslov = "";
            int idke = Program.idkadar;
            DateTime d = DateTime.Now;
            string pDatum = d.ToString("dd.MM.yy");
            int idFirme = Program.idFirme;
            string str = "";
            int idDokView = 0;
            bool ins = false;
            string pdokument = "PripremaZaPlacanje";
            CultureInfo cult = new CultureInfo("sr-SP-Latin", false);
            str = "Select Brdok, datum, ID_DokumentaStablo, ID_DokumentaTotali from DokumentaTotali, OrganizacionaStrukturaStavkeView  where Dokument = '" + pdokumentf + "' AND Datum = " + pDatum + " AND id_OrganizacionaStrukturaView = ID_OrganizacionaStrukturaStavkeView  And ID_OrganizacionaStrukturaStablo = " + idFirme;
            DataTable rsu = db.ReturnDataTable(str);
            if (rsu.Rows.Count == 0)
            {
                var rsDokumentaStablo =
                    db.ReturnDataTable("SELECT Naziv,ID_DokumentaStablo From DokumentaStablo where Naziv='" +
                                       pdokumentf + "'");

                if (rsDokumentaStablo.Rows.Count > 0)
                {
                    IdDokumentaStablo = Convert.ToInt32(rsDokumentaStablo.Rows[0][1]);
                    Naslov = Convert.ToString(rsDokumentaStablo.Rows[0][0]);
                    rsDokumentaStablo.Dispose();
                }
                else
                {
                    MessageBox.Show("Nije registrovan dokument " + pdokumentf + "!!");
                    rsDokumentaStablo.Dispose();
                    return;
                }

                var OOpis = "Knjizenje plata za mesec " + pdat.Substring(0, 2);

                string brojDok = "";

                clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                 idDokView = os.UpisiDokument(ref brojDok, OOpis, IdDokumentaStablo, pDatum);
                 ins = true;
            }

            var idDokViewf = ins ? idDokView : Convert.ToInt32(rsu.Rows[0]["ID_DokumentaTotali"]);
          
            str = "Delete From finansijskiInterniNalog Where ID_DokumentaView=" + idDokViewf;
            lista.Add(str);
            str = "Delete From finansijskiInterniNalogStavke Where ID_DokumentaView=" + idDokViewf;
            lista.Add(str);            
            rezultat = db.ExecuteSqlTransaction(lista);
            if (rezultat != "") { lista.Clear(); MessageBox.Show("Neuspesan unos" + Environment.NewLine + str); }
            lista.Clear();
            var obPla = PopuniNalogZaKnjizenjePlate(idDokViewf, pdat,vprimanja);
            if (obPla == false) { MessageBox.Show(mPoruka); }
            else
            {
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:FinansijskiInterniNalog", "IdDokument:" + idDokViewf);
                MessageBox.Show("Formiran nalog za knjizenje plata");
               
                PlateXmlSpisak(pdat, pDatum, vprimanja);
                MessageBox.Show("Zavrseno formiranje PPPPD za plate ");
            }

        }
        /// <summary>
        /// Popunimo nalog za knjizenje nagrade, a zatim punimo xml fajl podacima iz baze koji su potrebni za pojedinacnu poresku prijavu 
        /// </summary>
        /// <param name="mg">Parametar mg predstavlja mjesec i godinu u formatu npr "0717" jul mjesec 2017 godine</param>
        private void NagradeXml(string mg)
        {
            var lista = new List<string>();
            string pdat = mg; //mesecgodina
            string mPoruka = "";
            string pdokumentf = "KnjizenjePrevoza";
            int idke = Program.idkadar;
            DateTime d = DateTime.Now;
            string pDatum = d.ToString("dd.MM.yy");
            int idDokView = 0;
            bool ins = false;
            int idFirme = Program.idFirme;

            string pdokument = "PripremaZaPlacanje";
            CultureInfo cult = new CultureInfo("sr-SP-Latin", false);
            string str = "Select Brdok, datum, ID_DokumentaStablo, ID_DokumentaTotali from DokumentaTotali, OrganizacionaStrukturaStavkeView  where Dokument = '" + pdokument + "' AND Datum = " + pDatum + " AND id_OrganizacionaStrukturaView = ID_OrganizacionaStrukturaStavkeView  And ID_OrganizacionaStrukturaStablo = " + idFirme;
            DataTable rsu = db.ReturnDataTable(str);
            if (rsu.Rows.Count == 0)
            {

                str = " SELECT ID_DokumentaStablo  From DokumentaStablo where Naziv='" + pdokumentf + "'";
                var idDokumentaStablo = db.ReturnInt(str, 0);


           
              
                    var OOpis = "Knjizenje nagrada za mesec " + pdat.Substring(0, 2);
              

                string brojDok = "";
                clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                idDokView = os.UpisiDokument(ref brojDok, OOpis, idDokumentaStablo, pDatum);
                ins = true;
            }

            var idDokViewf = ins ? idDokView : Convert.ToInt32(rsu.Rows[0]["ID_DokumentaTotali"]);

            str = "Delete From finansijskiInterniNalog Where ID_DokumentaView=" + idDokViewf.ToString();
            lista.Add(str);
            str = "Delete From finansijskiInterniNalogStavke Where ID_DokumentaView=" + idDokViewf.ToString();
            lista.Add(str);
            var rezultat = db.ExecuteSqlTransaction(lista);
            if (rezultat != "") { lista.Clear(); MessageBox.Show(rezultat); return; }
            lista.Clear();
            bool vrati = PopuniNalogZaKnjizenjeNagrade(idDokViewf, pdat);            
            if (vrati == false )
            {
                DialogResult result2 = MessageBox.Show("produziti ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result2.ToString() == "No") { return; }  vrati = true; 
            }
            
            else
            {
                MessageBox.Show("Formiran nalog za knjiženje nagrade ");
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:FinansijskiInterniNalog", "IdDokument:" + idDokViewf);


                string mesec = DateTime.Now.Month.ToString();
                pdat = mg;
                if (Convert.ToInt32(mesec) < Convert.ToInt32(pdat.Substring(0, 2)))
                {
                    pdat = mesec;
                }
                string prevdat = "20" + pdat.Substring(2, 2) + "-" + pdat.Substring(0, 2);
                DateTime dddd = Convert.ToDateTime(DateTime.Now.ToString());
                pDatum = String.Format("{0:yyyy-MM-dd}", DateTime.Now);

                string prevparametri = "IId=" + Program.idFirme + "&ObracunskiPeriod=" + prevdat + "&DatumPlacanja=" + pDatum;
                // dio koda koji poziva php fajl na web serveru kao sto je radio i stari program
                /*
                var webClient = new WebClient();
               
                var xmlContent=webClient.DownloadString(new Uri(@"http://BANKOM38/ProbaPPPPD/nagrade/nagrade.php?" + prevparametri));


                if (Directory.Exists("c:\\TempXml") == false) { Directory.CreateDirectory("c:\\TempXml"); }
                System.IO.DirectoryInfo di = new DirectoryInfo("c:\\TempXml");
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);
                xmlDoc.Save(@"c:\\TempXml\nagradephp.xml");

                System.Diagnostics.Process.Start(@"c:\\TempXml\nagradephp.xml");
                */
                
                SqlConnection con = new SqlConnection();
                con = Program.con;
                if (con.State == ConnectionState.Closed) { con.Open(); }

                if (Directory.Exists("c:\\TempXml") == false) { Directory.CreateDirectory("c:\\TempXml"); }
                System.IO.DirectoryInfo di = new DirectoryInfo("c:\\TempXml");
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                str = "SELECT Grupa, PIB, Adresa, SifraOpstine, Telefon, Email, MaticniBroj FROM OrganizacionaStrukturaStavkeView WHERE ID_OrganizacionaStrukturaStablo=@param0";
                var dataTable = db.ParamsQueryDT(str, idFirme);
                var dataRow = dataTable.Rows[0];

                DateTime dt = DateTime.Now;
                var strprvi = "<PodaciOPrijavi>";
                strprvi += "<KlijentskaOznakaDeklaracije>"+ Convert.ToString(dataRow["Grupa"])+ " " + pDatum + "</KlijentskaOznakaDeklaracije>"; // xml tag klijentska oznaka deklaracije koju je klijent formirao. Poslata oznaka ce biti vracena POB-u kroz status prijave.Ukoliko je poreski obaveznik ne deklarise,nece se vracati.
                strprvi += "<VrstaPrijave >1</VrstaPrijave>"; // int 1, 1 Opsta prijava, 2 Po sluzbenoj duznosti, 3 Samoprijavljivanje , 4 Po nalazu kontrole , 5 Po odluci suda
                strprvi += "<ObracunskiPeriod >"   + prevdat + "</ObracunskiPeriod>"; // Period za koji se podnosi prijava u formatu xs:gYearMonth
                strprvi += "<DatumPlacanja>" + pDatum + "</DatumPlacanja >"; // datum isplate prihoda
                strprvi += "</PodaciOPrijavi>";


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strprvi);
                xmlDoc.Save(@"c:\\TempXml\0.xml");





                str = "";
                str = "  SELECT top 1 1 as TipIsplatioca , PIB as PoreskiIdentifikacioniBroj,  MaticniBroj as MaticniBrojisplatioca ,Grupa as NazivPrezimeIme ,  SifraOpstine as SedistePrebivaliste,";
                str += " Telefon,Adresa as UlicaIBroj , Email as eMail";
                str += " FROM OrganizacionaStrukturaStavkeView";
                str += " WHERE ID_OrganizacionaStrukturaStablo = " + idFirme + " and MaticniBroj<>0";
                str += " FOR XML PATH('PodaciOIsplatiocu')";
                SqlCommand command = new SqlCommand(str, con);

                XmlReader reader = command.ExecuteXmlReader();
                DataSet ds = new DataSet();
                ds.ReadXml(reader);
                string imexml = "c:\\TempXml\\1.xml";
                ds.WriteXml(@imexml);
                string fileReader;
                fileReader = File.ReadAllText(imexml);
                int brisi = fileReader.IndexOf("<PodaciOIsplatiocu>");
                fileReader = fileReader.Substring(brisi);
                reader.Close();
                ds.Dispose();
                command.Dispose();

                fileReader.ToUpper();
                File.WriteAllText(@imexml, fileReader);

                ///

                str = "	WITH LISTA AS  ( ";
                str += " SELECT 1 as VrstaIdentifikatoraPrimaoca, jmbg as IdentifikatorPrimaoca,";
                str += " Replace(Replace(Replace(Replace(Replace(left(Prezime, 1), ']', N'Ć'), '@', N'Ž'), '[', N'Š'), '\\', N'Đ'),'^', N'Č') as Prezime2,";
                str += " Replace(Replace(Replace(Replace(Replace(Prezime, ']', N'Ć'), '@', N'Ž'), '[', N'Š'), '\\', N'Đ'),'^', N'Č') as Prezime, ";
                str += " Replace(Replace(Replace(Replace(Replace(Ime, ']', N'Ć'), '@', N'Ž'), '[', N'Š'), '\\', N'Đ'),'^', N'Č') as Ime,";
                str += " SifraOpstine as OznakaPrebivalista, 101110000 AS SVP, ";
                str += " FORMAT(Bruto, 'N') AS Bruto, FORMAT(Porez, 'N') as Porez, FORMAT(OsnovicaPorez, 'N') as OsnovicaPorez, 0 as OsnovicaDoprinosi,";
                str += " 0 as PIO, 0 as ZDR, 0 as NEZ, 0 as PIOBen FROM PPPPD_XML_NAGRADE WHERE id_firma = " + idFirme + "  and mesdop = '" + mg + "K' )";
                str += "  SELECT " + " ROW_NUMBER() OVER(ORDER BY Prezime2) as RedniBroj, VrstaIdentifikatoraPrimaoca, IdentifikatorPrimaoca, Prezime, Ime, ";
                str += " OznakaPrebivalista, SVP, Bruto,  OsnovicaPorez, Porez , OsnovicaDoprinosi, PIO, ZDR, NEZ, PIOBen";
                str += " from lista  ORDER BY Prezime2";
                str += "  FOR XML PATH('PodaciOPrihodima'), ROOT('DeklarisaniPrihodi')";
                str = str.Replace("\t", "");

                command = new SqlCommand(str, con);
                reader = command.ExecuteXmlReader();
                ds = new DataSet();
                ds.ReadXml(reader);
                imexml = "c:\\TempXml\\2.xml";
                ds.WriteXml(@imexml);
                fileReader = "";
                fileReader = File.ReadAllText(imexml);
                fileReader = fileReader.Substring(brisi);
                brisi = fileReader.IndexOf("<DeklarisaniPrihodi>");

                fileReader.ToUpper();

                File.WriteAllText(@imexml, fileReader);
                MultipleFilesToSingleFile("c:\\TempXml", "*.xml", "c:\\TempXml\\nagrade.xml");
                MessageBox.Show("Zavrseno formiranje PPPPD za nagrade.XML fajl je sacuvan na C particiji u folderu TempXml");
                System.Diagnostics.Process.Start(@"c:\\TempXml\nagrade.xml");
            }
        }
        /// <summary>
        /// Pravimo XML fajl za plate.
        /// </summary>
        /// <param name="pdat"></param> mjesec i godina za koji radimo plate u formatu mmgg
        /// <param name="pDatum"></param>  datum obracuna
        /// <param name="vprimanja"></param> akontacija ili konacna zarada
        private void PlateXmlSpisak(string pdat, string pDatum, string vprimanja)
        {

            CultureInfo kult = new CultureInfo("sr-SP-Latin", false);
            string prevdat = "20" + pdat.Substring(2, 2) + "-" + pdat.Substring(0, 2);
            DateTime dddd = Convert.ToDateTime(DateTime.Now.ToString());
            pDatum = String.Format("{0: yyyy-MM-dd}", DateTime.Now);

            string prevparametri = "IId=" + Program.idFirme + "&ObracunskiPeriod=" + prevdat + "&DatumPlacanja=" + pDatum + "&vprimanja=" + vprimanja;
            // dio koda koji poziva php fajl na web serveru kao sto je radio i stari program
            /*
            var webClient = new WebClient();

            var xmlContent=webClient.DownloadString(new Uri(@"http://BANKOM38/ProbaPPPPD/plate/plate.php?" + prevparametri));


            if (Directory.Exists("c:\\TempXml") == false) { Directory.CreateDirectory("c:\\TempXml"); }
            System.IO.DirectoryInfo di = new DirectoryInfo("c:\\TempXml");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            xmlDoc.Save(@"c:\\TempXml\platephp.xml");

            System.Diagnostics.Process.Start(@"c:\\TempXml\platephp.xml");
            */
            
            SqlConnection con = new SqlConnection();
                con = Program.con;
                if (con.State == ConnectionState.Closed) { con.Open(); }

                if (Directory.Exists("c:\\TempXml") == false) { Directory.CreateDirectory("c:\\TempXml"); }
                System.IO.DirectoryInfo di = new DirectoryInfo("c:\\TempXml");
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                string str1 = "SELECT Grupa, PIB, Adresa, SifraOpstine, Telefon, Email, MaticniBroj FROM OrganizacionaStrukturaStavkeView WHERE ID_OrganizacionaStrukturaStablo=@param0";
                var dataTable = db.ParamsQueryDT(str1, Program.idFirme);
                var dataRow = dataTable.Rows[0];

                var strprvi = "<PodaciOPrijavi>";
                strprvi += "<KlijentskaOznakaDeklaracije>" + Convert.ToString(dataRow["Grupa"]) + " " + prevdat + "</KlijentskaOznakaDeklaracije>";
                strprvi += "<VrstaPrijave>1</VrstaPrijave>";
                strprvi += "<ObracunskiPeriod>" + prevdat + "</ObracunskiPeriod>";
                strprvi += "<OznakaZaKonacnu>"+ " K " + "</OznakaZaKonacnu>";
                strprvi += "<DatumPlacanja>" + pDatum + "</DatumPlacanja >";
                strprvi += "</PodaciOPrijavi>";


            XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strprvi);
                xmlDoc.Save(@"c:\\TempXml\0.xml");


                str1 = "SELECT * from PPPPD_XML where id_firma =@param0 and mes=@param1";
                dataTable = db.ParamsQueryDT(str1, Program.idFirme, pdat + vprimanja);
                int brZaposlenih = dataTable.Rows.Count > 0 ? dataTable.Rows.Count : 0 ;

                var str = "  SELECT top 1 1 as TipIsplatioca , PIB as PoreskiIdentifikacioniBroj, " + brZaposlenih + " as BrojZaposlenih,  MaticniBroj as MaticniBrojisplatioca ,Grupa as NazivPrezimeIme ,  SifraOpstine as SedistePrebivaliste,";
                str += " Telefon,Adresa as UlicaIBroj , Email as eMail";
                str += " FROM OrganizacionaStrukturaStavkeView";
                str += " WHERE ID_OrganizacionaStrukturaStablo = " + Program.idFirme + " and MaticniBroj<>0";
                str += " FOR XML PATH('PodaciOIsplatiocu')";
                SqlCommand command = new SqlCommand(str, con);

                XmlReader reader = command.ExecuteXmlReader();
                DataSet ds = new DataSet();
                ds.ReadXml(reader);
                string imexml = "c:\\TempXml\\1.xml";
                ds.WriteXml(@imexml);
                var fileReader = File.ReadAllText(imexml);
                int brisi = fileReader.IndexOf("<PodaciOIsplatiocu>");
                fileReader = fileReader.Substring(brisi);
                reader.Close();
                ds.Dispose();
                command.Dispose();
                File.WriteAllText(@imexml, fileReader);
                int BrojKalendarskihDana = System.DateTime.DaysInMonth(DateTime.Now.Year, Convert.ToInt32(pdat.Substring(0, 2)));

                str = "	 WITH LISTA AS  ( ";
                str += " SELECT 1 as VrstaIdentifikatoraPrimaoca, jmbg as IdentifikatorPrimaoca,";
                str += " Replace(Replace(Replace(Replace(Replace(left(Prezime, 1), ']', N'Ć'), '@', N'Ž'), '[', N'Š'), '\\', N'Đ'),'^', N'Č') as Prezime2,";
                str += " Replace(Replace(Replace(Replace(Replace(Prezime, ']', N'Ć'), '@', N'Ž'), '[', N'Š'), '\\', N'Đ'),'^', N'Č') as Prezime, ";
                str += " Replace(Replace(Replace(Replace(Replace(Ime, ']', N'Ć'), '@', N'Ž'), '[', N'Š'), '\\', N'Đ'),'^', N'Č') as Ime,";
                str += " SifraOpstine as OznakaPrebivalista, SVP AS SVP, MesecniFondSati as MesecniFondSati , EfektivniBrojSati as BrojEfektivnihSati , " + BrojKalendarskihDana + " as BrojKalendarskihDana, ";
                str += " FORMAT(Bruto, 'N') AS Bruto, FORMAT(Porez, 'N') as Porez , FORMAT(OsnovicaPorez, 'N') as OsnovicaPorez, FORMAT(OsnovicaDoprinosi, 'N') as OsnovicaDoprinosi,";
                str += " FORMAT(Pio, 'N') as PIO, FORMAT(Zdravstveno, 'N') as ZDR, FORMAT(Nezaposlenost, 'N') as NEZ, FORMAT(0 ,'N') as PIOBen FROM PPPPD_XML WHERE id_firma = " + Program.idFirme + " and mes = '" + pdat + vprimanja +"')";
                str += "  SELECT " + " ROW_NUMBER() OVER(ORDER BY Prezime2) as RedniBroj, VrstaIdentifikatoraPrimaoca ,IdentifikatorPrimaoca, Prezime, Ime, ";
                str += " OznakaPrebivalista, SVP, BrojKalendarskihDana , BrojEfektivnihSati , MesecniFondSati , Bruto, OsnovicaPorez, Porez ,OsnovicaDoprinosi, PIO, ZDR, NEZ, PIOBen";
                str += " from lista  ORDER BY Prezime2";
                str += "  FOR XML PATH('PodaciOPrihodima'), ROOT('DeklarisaniPrihodi')";
                str = str.Replace("\t", "");
                command = new SqlCommand(str, con);
                reader = command.ExecuteXmlReader();
                ds = new DataSet();
                ds.ReadXml(reader);
                imexml = "c:\\TempXml\\2.xml";
                ds.WriteXml(@imexml);
                fileReader = File.ReadAllText(imexml);
                fileReader = fileReader.Substring(brisi);
                File.WriteAllText(@imexml, fileReader);
                switch (vprimanja)
                {
                    case "K":
                        MultipleFilesToSingleFile("c:\\TempXml", "*.xml", "c:\\TempXml\\plate.xml");
                        break;
                    case "A":
                        MultipleFilesToSingleFile("c:\\TempXml", "*.xml", "c:\\TempXml\\akontacije.xml");
                        break;
                             

            }
            MessageBox.Show("Zavrseno formiranje PPPPD za plate.XML fajl je sacuvan na C particiji u folderu TempXml");
            System.Diagnostics.Process.Start(@"c:\\TempXml\plate.xml");

        }
        private void prevozXml(string mg)
        {

            var lista = new List<string>();
            string pdat = mg; //mesecgodina
            int IdDokumentaStablo = 0;
            string mPoruka = "";
            string pdokumentf = "KnjizenjePrevoza";
            string rezultat = "";
            DateTime d = DateTime.Now;
            string pDatum = d.ToString("dd.MM.yy");
            int idFirme = Program.idFirme;
            bool ins = false;
            int IdDokView =  0;
            string pdokument = "PripremaZaPlacanje";
            CultureInfo cult = new CultureInfo("sr-SP-Latin", false);
            string str = "Select Brdok, datum, ID_DokumentaStablo, ID_DokumentaTotali from DokumentaTotali, OrganizacionaStrukturaStavkeView  where Dokument = '" + pdokumentf + "' AND Datum = " + pDatum + " AND id_OrganizacionaStrukturaView = ID_OrganizacionaStrukturaStavkeView  And ID_OrganizacionaStrukturaStablo = " + idFirme;
            DataTable rsu = db.ReturnDataTable(str);
            if (rsu.Rows.Count == 0)
            {
                
                    str = " SELECT ID_DokumentaStablo  From DokumentaStablo where Naziv='" + pdokumentf + "'";
                    IdDokumentaStablo = db.ReturnInt(str, 0);


                    var oOpis = "Knjizenje prevoza za mesec " + pdat.Substring(0, 2);
                    


                    string brojDok = "";
                    clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                    IdDokView = os.UpisiDokument(ref brojDok, oOpis, IdDokumentaStablo, pDatum);
                    ins = true;
            }

            var idDokViewf = ins ? IdDokView : Convert.ToInt32(rsu.Rows[0]["ID_DokumentaTotali"]);       

            str = "Delete From finansijskiInterniNalog Where ID_DokumentaView=" + idDokViewf.ToString();
            lista.Add(str);
            str = "Delete From finansijskiInterniNalogStavke Where ID_DokumentaView=" + idDokViewf.ToString();
            lista.Add(str);
            rezultat = db.ExecuteSqlTransaction(lista);
            if (rezultat != "") { lista.Clear(); MessageBox.Show(rezultat); return; }
            lista.Clear();

            bool vrati = PopuniNalogZaKnjizenjePrevoza(idDokViewf, pdat);
            if (vrati == false) { MessageBox.Show("Neuspesno : PopuniNalogZaKnjizenjePrevoza"); }
            else
            {

                MessageBox.Show("Formiran nalog za prevoz ");
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:FinansijskiInterniNalog", "IdDokument:" + idDokViewf);

                string mesec = "0" + DateTime.Now.Month;

                 pdat = mg;
                 string prevdat = "20" + pdat.Substring(2, 2) + "-" + pdat.Substring(0, 2);
                 DateTime dddd = Convert.ToDateTime(DateTime.Now.ToString());
                 pDatum = String.Format("{0: yyyy-MM-dd}", DateTime.Now);

                string prevparametri = "IId=" + Program.idFirme + "&ObracunskiPeriod=" + prevdat + "&DatumPlacanja=" + pDatum;
                // dio koda koji poziva php fajl na web serveru kao sto je radio i stari program
                /*
                var webClient = new WebClient();
               
                var xmlContent=webClient.DownloadString(new Uri(@"http://BANKOM38/ProbaPPPPD/prevoz/prevoz.php?" + prevparametri));


                if (Directory.Exists("c:\\TempXml") == false) { Directory.CreateDirectory("c:\\TempXml"); }
                System.IO.DirectoryInfo di = new DirectoryInfo("c:\\TempXml");
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);
                xmlDoc.Save(@"c:\\TempXml\prevozphp.xml");

                System.Diagnostics.Process.Start(@"c:\\TempXml\prevozphp.xml");
                */
                
                SqlConnection con = new SqlConnection();
                con = Program.con;
                if (con.State == ConnectionState.Closed) { con.Open(); }

                if (Directory.Exists("c:\\TempXml") == false) { Directory.CreateDirectory("c:\\TempXml"); }
                System.IO.DirectoryInfo di = new DirectoryInfo("c:\\TempXml");
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                str = "SELECT Grupa, PIB, Adresa, SifraOpstine, Telefon, Email, MaticniBroj FROM OrganizacionaStrukturaStavkeView WHERE ID_OrganizacionaStrukturaStablo=@param0";
                var dataTable = db.ParamsQueryDT(str, idFirme);
                var dataRow = dataTable.Rows[0];

                DateTime dt = DateTime.Now;
                var strprvi = "<PodaciOPrijavi>";
                strprvi += "<KlijentskaOznakaDeklaracije>"+ Convert.ToString(dataRow["Grupa"])+ " " + pDatum + "</KlijentskaOznakaDeklaracije>";
                strprvi += "<VrstaPrijave >1</VrstaPrijave>";
                strprvi += "<ObracunskiPeriod >" + prevdat +  "</ObracunskiPeriod>";
                strprvi += "<DatumPlacanja>" + pDatum + "</DatumPlacanja >";
                strprvi += "</PodaciOPrijavi>";


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strprvi);
                xmlDoc.Save(@"c:\\TempXml\0.xml");
                
                    



                str = "";
                str = "  SELECT top 1 1 as TipIsplatioca , PIB as PoreskiIdentifikacioniBroj,  MaticniBroj,Grupa as NazivPrezimeIme ,  SifraOpstine as SedistePrebivaliste,"; 
                str += " Telefon,Adresa as UlicaIBroj , Email as eMail";
                str += " FROM OrganizacionaStrukturaStavkeView";
                str += " WHERE ID_OrganizacionaStrukturaStablo ="+ idFirme +" and MaticniBroj<>0";
                str += " FOR XML PATH('PodaciOIsplatiocu')";
                SqlCommand command = new SqlCommand(str, con);

                XmlReader reader = command.ExecuteXmlReader();
                DataSet ds = new DataSet();
                ds.ReadXml(reader);
                string imexml = "c:\\TempXml\\1.xml";
                ds.WriteXml(@imexml);
                string fileReader;
                fileReader = File.ReadAllText(imexml);
                int brisi = fileReader.IndexOf("<PodaciOIsplatiocu>");
                fileReader = fileReader.Substring(brisi);
                reader.Close();
                ds.Dispose();
                command.Dispose();

                fileReader.ToUpper();
                File.WriteAllText(@imexml, fileReader);

                            
                str = "	WITH LISTA AS  ( ";
                str += " SELECT 1 as VrstaIdentifikatoraPrimaoca, jmbg as IdentifikatorPrimaoca,";
                str += " Replace(Replace(Replace(Replace(Replace(left(Prezime, 1), ']', N'Ć'), '@', N'Ž'), '[', N'Š'), '\\', N'Đ'),'^', N'Č') as Prezime2,";
                str += " Replace(Replace(Replace(Replace(Replace(Prezime, ']', N'Ć'), '@', N'Ž'), '[', N'Š'), '\\', N'Đ'),'^', N'Č') as Prezime, ";
                str += " Replace(Replace(Replace(Replace(Replace(Ime, ']', N'Ć'), '@', N'Ž'), '[', N'Š'), '\\', N'Đ'),'^', N'Č') as Ime,";
                str += " SifraOpstine as OznakaPrebivalista, 101110000 AS SVP, ";
                str += " FORMAT(Bruto, 'N') AS Bruto, FORMAT(OsnovicaPorez, 'N') as OsnovicaPorez, FORMAT(Porez, 'N') as Porez, 0 as OsnovicaDoprinosi,";
                str += " 0 as PIO, 0 as ZDR, 0 as NEZ, 0 as PIOBen FROM PPPPD_XML_PREVOZ WHERE Porez > 0 and id_firma = "+ idFirme +" and mesdop = '" + mg + "K' )";
                str += " SELECT " + " ROW_NUMBER() OVER(ORDER BY Prezime2) as RedniBroj, VrstaIdentifikatoraPrimaoca , IdentifikatorPrimaoca, Prezime, Ime, ";
                str += " OznakaPrebivalista, SVP, Bruto, OsnovicaPorez, Porez ,OsnovicaDoprinosi, PIO, ZDR, NEZ, PIOBen";
                str += " from lista  ORDER BY Prezime2";
                str += "  FOR XML PATH('PodaciOPrihodima'), ROOT('DeklarisaniPrihodi')";
                str.Replace("\t", "");
                command = new SqlCommand(str, con);               
                reader = command.ExecuteXmlReader();
                ds = new DataSet();
                ds.ReadXml(reader);
                imexml = "c:\\TempXml\\2.xml";
                ds.WriteXml(@imexml);
                fileReader = "";
                fileReader = File.ReadAllText(imexml);
                fileReader = fileReader.Substring(brisi);
                brisi = fileReader.IndexOf("<DeklarisaniPrihodi>");

                fileReader.ToUpper();
                
                File.WriteAllText(@imexml, fileReader);

                MultipleFilesToSingleFile("c:\\TempXml", "*.xml",  "c:\\TempXml\\prevoz.xml");
                MessageBox.Show("Zavrseno formiranje PPPPD za prevoz.XML fajl je sacuvan na C particiji u folderu TempXml");
                System.Diagnostics.Process.Start(@"c:\\TempXml\prevoz.xml");
            }




        }

        private void MultipleFilesToSingleFile(string dirPath, string filePattern, string destFile)
        {
            string[] fileAry = Directory.GetFiles(dirPath, filePattern);
            int xx = 0;


            using (TextWriter tw = new StreamWriter(destFile, true))
            {
                foreach (string filePath in fileAry)
                {

                    using (TextReader tr = new StreamReader(filePath))
                    {
                        if (xx == 0)
                        {

                            //tw.WriteLine("<?xml version =" + "\"1.0\" standalone =" + "\"yes\" ?>" + Environment.NewLine +  "<PodaciPoreskeDeklaracije>" + Environment.NewLine +  tr.ReadToEnd());
                            tw.WriteLine("<PodaciPoreskeDeklaracije>" + Environment.NewLine + tr.ReadToEnd());
                        }
                        else
                        {
                            if (xx == fileAry.Length - 1) { tw.WriteLine(tr.ReadToEnd() + "</PodaciPoreskeDeklaracije>"); }
                        }
                        tw.WriteLine(tr.ReadToEnd());
                        tr.Close();
                        tr.Dispose();
                    }

                    xx += 1;
                }

                tw.Close();
                tw.Dispose();
            }
            XmlDocument doc = new XmlDocument();
            string fileReader = File.ReadAllText(destFile);
            doc.Load(new StringReader(fileReader));

            //Create an XML declaration. 
            var xmldecl = doc.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "UTF-8";
           // xmldecl.Standalone = "yes";

            // Add the new node to the document.
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmldecl, root);
            doc.Save(destFile);

        }

        public bool PopuniNalogZaKnjizenjePlate(int fidDok , string datum ,string vprimanja) 
        {
            var lista = new List<string>();
            var str = "";
            var vrati = "";

            str = " IF NOT EXISTS(SELECT 1 FROM FinansijskiInterniNalog WHERE ID_DokumentaView = " + fidDok.ToString() + ") ";
            str  += "Insert into FinansijskiInterniNalog ( ID_DokumentaView,UUser) Select " + fidDok.ToString() + ", " + Program.SifRadnika.ToString();
            lista.Add(str);

            

            //   1. Obracun zarada-neto zarada bez bolovanja
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + ", 61 as ID_ArtikliView, 0, sum(nld), " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML  where  ind='P' AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "'";
            lista.Add(str);            


            //  1a. Obra~un zarada-neto zarada bolovanje
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)"; 
            str += " Select " + fidDok.ToString() + " as ID_DokumentaView, ID_Artikli as ID_ArtikliView, 0, sum(nld), " + Program.SifRadnika.ToString();
            str += " from PPPPD_XML, Artikli  where ind = 'B' AND Artikli.StaraSifra = 922 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes = '" + datum + vprimanja + "' Group By Artikli.ID_Artikli";
            lista.Add(str);
            

            // 3. Obra~un zarada-porez na zaradu  bez bolovanja
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + " , ID_Artikli as ID_ArtikliView,sum(porez),0, " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind='P' AND Artikli.StaraSifra=52 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By Artikli.ID_Artikli";
            lista.Add(str);
          

            // 3a. Obra~un zarada-porez na zaradu  bolovanje
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + ", ID_Artikli as ID_ArtikliView,0,sum(porez), " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind='B' AND Artikli.StaraSifra=925 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By Artikli.ID_Artikli";
            lista.Add(str);

            //  4. Obra~un zarada-doprinosi na zaradu pio radnik
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)"; 
            str += " Select " + fidDok.ToString() + ", ID_Artikli as ID_ArtikliView,sum(dop1),0, " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind='P' AND Artikli.StaraSifra=53 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
            lista.Add(str);

            //  4a. Obra~un zarada-doprinosi na zaradu pio poslodavac
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + ", ID_Artikli as ID_ArtikliView,sum(dopp1),0, " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind='P' AND Artikli.StaraSifra=56 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
            lista.Add(str);

            //  5. Obra~un zarada-doprinosi na zaradu zdravstvo radnik
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + " as ID_DokumentaView, ID_Artikli as ID_ArtikliView,sum(dop2),0, " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind ='P' AND Artikli.StaraSifra=54 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
            lista.Add(str);


            //'5a. Obra~un zarada-doprinosi na zaradu zdravstvo poslodavac
            str += " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + " , ID_Artikli as ID_ArtikliView,sum(dopp2),0, " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind ='P' AND Artikli.StaraSifra=57 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
            lista.Add(str);

            // '6. Obra~un zarada-doprinosi za nezaposlenost radnik
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + " , ID_Artikli as ID_ArtikliView,sum(dop3),0, " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind='P' AND Artikli.StaraSifra=55 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
            lista.Add(str);

            //  '6a. Obra~un zarada-doprinosi za nezaposlenost poslodavac
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + ", ID_Artikli as ID_ArtikliView,sum(dopp3),0, " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind='P' AND Artikli.StaraSifra=58 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
            lista.Add(str);
            vrati = db.ExecuteSqlTransaction(lista);
            if (vrati != "") { lista.Clear(); MessageBox.Show(vrati);return false;  }

                lista.Clear();


            // '7. Obra~un zarada-dotacije i subvencije  na zaradu
            str =  " Select sum(umanji_porez+umanji_dop1+Umanji_dopp1) as Umanji ";
            str += " from  PPPPD_XML  where ind='P' AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' ";
            DataTable rsp = db.ReturnDataTable(str);
            
            if (rsp.Rows.Count > 0)
            {
                str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
                                str += " Select " + fidDok.ToString() + " , ID_Artikli as ID_ArtikliView,sum(umanji_porez+umanji_dop1+Umanji_dopp1),0, " + Program.SifRadnika.ToString();
                str += " from  PPPPD_XML,Artikli  where  Artikli.StaraSifra=9188 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
                lista.Add(str);
            }


            //     'BOLOVANJE
            // '8. Obra~un zarada-doprinosi za pio-bolovanje radnik
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + ", ID_Artikli as ID_ArtikliView,0,sum(dop1), " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind='B' AND Artikli.StaraSifra=2701 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
             
            lista.Add(str);


            //'8a. Obra~un zarada-doprinosi  za pio-bolovanje poslodavac
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str +=  " Select " + fidDok.ToString() + ", ID_Artikli as ID_ArtikliView,0,sum(dopp1), " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind='B' AND Artikli.StaraSifra=923 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
            lista.Add(str);

            // '9. Obra~un zarada-doprinosi za zdravstvo-bolovanje radnik
            str  = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_Komitent,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + " , 1,ID_Artikli as ID_ArtikliView,sum(dop2),0, " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind='B' AND Artikli.StaraSifra=2700 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
            lista.Add(str);
            //'9a. Obra~un zarada-doprinosi za zdravstvo-bolovanje poslodavac

            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_Komitent,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + ", 1,ID_Artikli as ID_ArtikliView,sum(dopp2),0, " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind='B' AND Artikli.StaraSifra=2699 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
            lista.Add(str);

            //            '10. Obra~un zarada-doprinosi za zaposljavanje-bolovanje radnik
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_Komitent,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + ", 1,ID_Artikli as ID_ArtikliView,sum(dop3),0, " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind='B' AND Artikli.StaraSifra=2702 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
            lista.Add(str);

            //          '10a. Obra~un zarada-doprinosi za za zaposljavanje-bolovanje poslodavac
            str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_Komitent,ID_ArtikliView,Iznos,Iznos2,UUser)";
            str += " Select " + fidDok.ToString() + " , 1,ID_Artikli as ID_ArtikliView,sum(dopp3),0, " + Program.SifRadnika.ToString();
            str += " from  PPPPD_XML,Artikli  where ind='B' AND Artikli.StaraSifra=924 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
            lista.Add(str);
            

            //            '11. Obra~un zarada-doprinosi na zaradu +porez+netozarada  bolovanje koji se refundirajud
            if (Program.idOrgDeo == 15)
            {
                str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_Komitent,ID_ArtikliView,Iznos,Iznos2,UUser)";
                str += " Select " + fidDok.ToString() + " as ID_DokumentaView, 1563,ID_Artikli as ID_ArtikliView,sum(nld+porez+pio+zdravstveno+nezaposlenost),0, " + Program.SifRadnika.ToString();
                str += " from  PPPPD_XML,Artikli  where ind='B' AND Artikli.StaraSifra=1164 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
                lista.Add(str);                
            }

            else
            { 
               str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_Komitent,ID_ArtikliView,Iznos,Iznos2,UUser)";
               str += " Select " + fidDok.ToString() + " as ID_DokumentaView, 736,ID_Artikli as ID_ArtikliView,sum(nld+porez+pio+zdravstveno+nezaposlenost),0, " + Program.SifRadnika.ToString();
               str += " from  PPPPD_XML,Artikli  where ind='B' AND Artikli.StaraSifra=1164 AND id_firma = " + Program.idOrgDeo.ToString() + " and mes='" + datum + vprimanja + "' Group By ID_Artikli";
                lista.Add(str);
            }

            vrati = db.ExecuteSqlTransaction(lista);
             if (vrati == "") return true;
            MessageBox.Show(vrati); return false;


        }
        public bool PopuniNalogZaKnjizenjePrevoza(int fidDok, string datum)
        {
            var lista = new List<string>();
             db = new DataBaseBroker(); 
           // int idFirme = db.ReturnInt("select ID_OrganizacionaStrukturaStablo from organizacionastrukturastablo where naziv = '" + Convert.ToString(Program.imeFirme) + "'", 0);
            int idFirme = Program.idFirme;
            var rs = db.ReturnDataTable("Select * from doprinosi where mesdop='" + datum + "K' and id_Firma=" + idFirme);
            if (rs.Rows.Count == 0)
            {
               
                const string mPoruka = "Nisu popunjeni doprinosi za odabrani mesec";
                MessageBox.Show(mPoruka);
                return false; ;
            }

            var str = " IF NOT EXISTS(SELECT 1 FROM FinansijskiInterniNalog WHERE ID_DokumentaView = " + fidDok.ToString() + ") ";
            str += " Insert into FinansijskiInterniNalog ( ID_DokumentaView,UUser)";
            str += " Select " + fidDok.ToString() + " as ID_DokumentaView, " + Program.SifRadnika.ToString();

            var command = new SqlCommand();
            lista.Add(str);

            var artikli = PrevozSifreArtikla();
            
                // 1. Porez na prevoz potrazno
                var str1 = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
                str1 += " Select " + fidDok + " as ID_DokumentaView, ID_Artikli as ID_ArtikliView, 0, sum(porez), " + Program.SifRadnika;
                str1 += " from PPPPD_XML_Prevoz,Artikli ";
                str1 += " where Porez>0 AND  id_firma= " + Program.idFirme + " and mesdop='" + datum + "K' and Artikli.starasifra = " + artikli.Item1;
                str1 += " group by ID_Artikli having sum(porez)<>0 ";
                
                lista.Add(str1);

                //  2. zbir iznosa  za prevoz potrazno
                str = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
                str += " Select " + fidDok + " as ID_DokumentaView, ID_Artikli as ID_ArtikliView,0,sum(PPPPD_XML_Prevoz.prevoz), " + Program.SifRadnika.ToString();
                str += " from  radnik, artikli,PPPPD_XML_Prevoz ";
                str += " where artikli.starasifra = "+artikli.Item2 +" and presrad='' and radnik.mbr=PPPPD_XML_Prevoz.mbr and radnik.ID_Firma=PPPPD_XML_Prevoz.ID_firma and mesdop='" + datum + "K'  and radnik.id_firma = " + Convert.ToString(idFirme) + " group by ID_Artikli ";
                lista.Add(str);

                //  3. Ukupna  suma sa porezom -dugovno
                var str3 = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
                str3 += " Select " + fidDok + " as ID_DokumentaView, ID_Artikli as ID_ArtikliView,sum(PPPPD_XML_Prevoz.prevoz)+(Select case when sum(porez)<0 then 0 else SUM(porez) end from PPPPD_XML_Prevoz ";
                str3 += " where ID_Firma=" + idFirme + " and mesdop ='" + datum + "K' and porez > 0) as Iznos, 0," + Program.SifRadnika;
                str3 += " from radnik,artikli,PPPPD_XML_Prevoz where presrad='' and radnik.mbr=PPPPD_XML_Prevoz.mbr and radnik.ID_Firma=PPPPD_XML_Prevoz.ID_firma  and mesdop ='" + datum + "K' and radnik.id_firma = " + idFirme;
                str3 += " AND starasifra= "+ artikli.Item2 +"   group by id_artikli ";
                lista.Add(str3);
            
            var vrati = db.ExecuteSqlTransaction(lista);

            if (vrati == "") return true;
            MessageBox.Show(vrati); return false;



        }

        public bool PopuniNalogZaKnjizenjeNagrade(int fidDok, string datum)
        {
            
            var lista = new List<string>();
            var vrati = "";
            db= new DataBaseBroker();
            var idFirme = Program.idFirme;
            var rs = db.ReturnDataTable("Select * from doprinosi where mesdop='" + datum + "K' and id_Firma=" + idFirme.ToString());
            if (rs.Rows.Count == 0)
            {
                const string mPoruka = "Nisu popunjeni doprinosi za odabrani mesec";
                MessageBox.Show(mPoruka);
                return false;
            }
            var str = " IF NOT EXISTS(SELECT 1 FROM FinansijskiInterniNalog WHERE ID_DokumentaView = " + fidDok.ToString() + ") ";
            str += " Insert into FinansijskiInterniNalog ( ID_DokumentaView,UUser)";
            str += " Select " + fidDok.ToString() + " as ID_DokumentaView, " + Program.SifRadnika.ToString();

            lista.Add(str);
            var artikli = NagradeSifreArtikla();
                // 1.Porez na nagrade potrazno
                var str1 = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
                str1 += " Select " + fidDok.ToString() + " as ID_DokumentaView, ID_Artikli as ID_ArtikliView, 0, sum(porez), " + Program.SifRadnika.ToString();
                str1 += " from PPPPD_XML_Nagrade,Artikli ";
                str1 += " where   id_firma= " + idFirme.ToString() + " and mesdop='" + datum + "K' and Artikli.starasifra = " + artikli.Item1;
                str1 += " group by ID_Artikli having sum(porez)<>0 ";

                lista.Add(str1);

                // 2. zbir iznosa  za nagrade potrazno
                var str2 = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
                str2 += " Select " + fidDok.ToString() + " as ID_DokumentaView, ID_Artikli as ID_ArtikliView,0,sum(nagrade), " + Program.SifRadnika.ToString();
                str2 += " from  radnik, artikli ";
                str2 += " where artikli.starasifra = " + artikli.Item2 + " and presrad='' and id_firma = " + Convert.ToString(idFirme) + " group by ID_Artikli ";
                lista.Add(str2);

                //  3. Ukupna  suma sa porezom -dugovno
                var str3 = " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,Iznos,Iznos2,UUser)";
                str3 += " Select " + fidDok.ToString() + " as ID_DokumentaView, ID_Artikli as ID_ArtikliView,sum(radnik.nagrade)+(Select case when sum(porez)<0 then 0 else SUM(porez) end from PPPPD_XML_Nagrade ";
                str3 += " where ID_Firma=" + idFirme.ToString() + " and mesdop ='" + datum + "K') as Iznos, 0," + Program.SifRadnika.ToString();
                str3 += " from radnik,artikli where presrad='' and radnik.id_firma = " + idFirme.ToString();
                str3 += " AND starasifra= "+artikli.Item2+"  group by id_artikli ";
                lista.Add(str3);


           

            vrati = db.ExecuteSqlTransaction(lista);
            if (vrati != "") { MessageBox.Show(vrati,"produziti?",MessageBoxButtons.OKCancel);
                return false; }
            lista.Clear();
            return true;

        }

        public Tuple<int, int> NagradeSifreArtikla()
        {
            string nag;
            string por;
            if (Program.idFirme != 15)
            {
                nag = "Jubilarna nagrada";
                por = "Porez na jubilarnu nagradu";
            }
            else
            {
                nag = "Jubilarne nagrade";
                por = "Porez na jubilarne nagrade";
            }

            var nagrada= db.ReturnInt("select StaraSifra from Artikli where NazivArtikla like '%" + nag + "%'", 0);
            var porez = db.ReturnInt("select StaraSifra from Artikli where NazivArtikla like '%" + por + "%'", 0);
            
            return Tuple.Create(porez, nagrada);


        }

        public Tuple<int, int> PrevozSifreArtikla()
        {
            string pre;
            string por;
            if (Program.idFirme != 15)
            {
                pre = "Mesecni prevoz";
                por = "porez na mesecni prevoz";
            }
            else
            {
                pre = "markica";
                por = "porez na mesecni prevoz";
            }

            var prevoz = db.ReturnInt("select StaraSifra from Artikli where NazivArtikla like '%" + pre + "%'", 0);
            var porez = db.ReturnInt("select StaraSifra from Artikli where NazivArtikla like '%" + por + "%'", 0);

            return Tuple.Create(porez, prevoz);

        }

        
    }
}

