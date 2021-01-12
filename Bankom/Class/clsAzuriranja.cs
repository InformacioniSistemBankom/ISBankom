using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Bankom.Class
{
    class clsAzuriranja
    {
        string str = "";
        string strParams = "";
        List<string[]> lista = new List<string[]>();
        string strTabela = "";
        string dokType = "";
        private int IdDokView = 1;
        string sql = "";
        DataBaseBroker db = new DataBaseBroker();
        private Form forma;
        bool Vrati = false;
        double srednji = 1;
        string Poruka = "";
        string kojidatum = "";
        string IDValuta = "";
        int ret = 0;
        double sKurs = 1;
        double KursiranTrosak = 0;
        double KojiTrosakKalkulacije = 0;
        string TrebaProvera = "0";
        
        public Boolean DodatnaAzuriranja(string Dokument, string IdDokView)
        {
            bool DodatnaAzuriranja = false;
            forma = new Form();
            forma = Program.Parent.ActiveMdiChild;

            switch (Dokument)
            {
                case "KonacniUlazniRacun":
                    kojidatum = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "DatumCarinjenja").Vrednost;
                    IDValuta = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "OznVal").ID.ToString();
                    string kojavaluta = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "OznVal").Vrednost.ToString();
                    clsProveraIspravnosti pi = new clsProveraIspravnosti();
                    Vrati = pi.PostojiKurs(kojidatum, IDValuta, kojavaluta, ref srednji, ref Poruka);
                    if (Vrati == true)
                    {
                        sql = "Update RacunStavke set NabavnaCena=(ProdajnaCena*((100-ProcenatRabata)/100))*@param0"
                            + "  where ID_DokumentaView=@param1 and @param2  not in (Select ID_UlazniRacunCeo from PDVkalkulacijaUlazaTotali) ";
                        ret = db.ParamsInsertScalar(sql, srednji, IdDokView, IdDokView);

                        sql = "update RacunStavke set ProsecnaNabavnaCena=NabavnaCena "
                           + " where ID_DokumentaView=@param0 and ProsecnaNabavnaCena = 0";
                        ret = db.ParamsInsertScalar(sql, IdDokView);

                        sql = "update RacunStavke set Primljeno=kolicina from  RacunStavke as rs "
                           + " where rs.ID_DokumentaView=@param0 and Primljeno=0";
                        ret = db.ParamsInsertScalar(sql, IdDokView);
                    }
                    break;
                case "PrometRecepcije":
                    sql = "Update PrometRecepcijeStavke set ID_SkladisteProizvodnje =cr.ID_SkladisteProizvodnje "
                        + " from PrometRecepcijeStavke as pr,Cenovnikrecepcije as cr "
                         + " where pr.ID_ArtikliView=cr.ID_artikliView and pr.ID_DokumentaView=@param0 and pr.id_skladisteProizvodnje=1";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "PrometUplata":
                    sql = " Update PlacanjaNaplate set IspravniPodaciDaNe=1 "
                         + " Where ID_KomitentiView>1 AND ID_DokumentaView=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "KnjigaIzlaznePoste":
                    sql = " Update IzlaznaPosta set ID_OrganizacionaStrukturaStablo=@param0"
                        + " Where ID_DokumentaView=@param1";
                    ret = db.ParamsInsertScalar(sql, Program.idFirme, IdDokView);

                    break;
                case "MesecniPlanProdaje":
                    // zakucano za skladiste Bioprotein //jovana
                    sql = " Update PlanProdajeStavke set ProdajnaCena=s.PoslednjaProdajnacena from PlanProdajeStavke as ps, "
                        + " PlanProdaje as p,CeNeArtikalanaSkladistimaPred as s "
                        + " Where p.ID_DokumentaView=ps.ID_DokumentaView and s.ID_CeneArtikalaNaSkladistimaPred= "
                        + " (select max(ID_CeneArtikalaNaSkladistimaPred) from dbo.CeneArtikalaNaSkladistimaPred  as ss "
                        + " where ss.ID_ArtikliView=ps.ID_ArtikliView and  year(ss.datum)=p.ReferentnaGodina and ss.izlaz>0 and skl=23)"
                        + " and ps.ProdajnaCena=0 and p.ID_DokumentaView=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "Prijemnica":
                    if (Program.imeFirme == "Bioprotein")
                    {
                        sql = " Update OtpremnicaStavke set ID_MagacinskaPolja=st.ID_MagacinskaPolja "
                            + " from OtpremnicaStavke as os,stanjerobenaskl as st,otpremnica as o "
                            + " where o.id_dokumentaview=os.id_dokumentaview and o.ID_skladiste=st.ID_Skladiste and st.id_magacinskapolja>1 and os.ID_MagacinskaPolja=1 "
                            + " and os.id_artikliview=st.id_artikliview and os.ID_DokumentaView=@param0 and st.ID_DokumentaView<>@param1";
                        ret = db.ParamsInsertScalar(sql, IdDokView, IdDokView);
                    }

                    break;
                case "Trebovanje":
                    if (Program.imeFirme == "Bioprotein")//zasto je ovde otpremnica?
                    {
                        sql = "Update TrebovanjeStavke set ID_MagacinskaPolja=st.ID_MagacinskaPolja "
                            + " from TrebovanjeStavke as os,stanjerobenaskl as st,otpremnica as o "
                            + " where o.id_dokumentaview=os.id_dokumentaview and o.ID_skladiste=st.ID_Skladiste and st.id_magacinskapolja>1 and os.ID_MagacinskaPolja=1 "
                            + " and os.id_artikliview=st.id_artikliview And os.ID_DokumentaView=@param0 and st.ID_DokumentaView<>@param1";
                        ret = db.ParamsInsertScalar(sql, IdDokView, IdDokView);
                    }

                    break;
                case "InterniNalogZaRobu":
                case "PDVInterniNalogZaRobu":
                    if (Program.imeFirme == "Bioprotein")//zasto je ovde otpremnica?
                    {
                        sql = "Update InterniNalogZaRobuStavke set ID_MagacinskaPolja=st.ID_MagacinskaPolja "
                            + " from InterniNalogZaRobuStavke  as os,stanjerobenaskl as st,otpremnica as o "
                            + " where o.id_dokumentaview=os.id_dokumentaview and o.ID_skladiste=st.ID_Skladiste and st.id_magacinskapolja>1 and os.ID_MagacinskaPolja=1 "
                            + " and os.id_artikliview=st.id_artikliview And os.ID_DokumentaView=@param0 and st.ID_DokumentaView<>@param1";
                        ret = db.ParamsInsertScalar(sql, IdDokView, IdDokView);
                    }

                    break;
                case "PrevodjenjeProizvoda":
                    sql = "Update PrevodjenjeProizvodaStavke set ID_ArtikliViewU=l.ID_ArtikliView "
                        + " from PrevodjenjeProizvodaStavke  as ps,LotView as l "
                        + " where ps.ID_LotViewU=l.id_Lotview And ps.ID_DokumentaView=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    sql = "Update PrevodjenjeProizvodaStavke set ProsecnaNabavnaCena=c.ProsecnaNabavnaCena "
                      + " from PrevodjenjeProizvodaStavke  as ps,Cenovnik as c,MagacinskaPolja as m "
                      + " where ps.ID_ArtikliView=c.id_Artikliview and ps.ID_MagacinskaPolja=m.ID_MagacinskaPolja "
                      + " and c.ID_Skladiste =m.ID_Skladiste and ps.ProsecnaNabavnaCena=0 AND ps.ID_DokumentaView=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "NalogZaRazduzenjeAmbalaze":
                    sql = "Update RazduzenjeAmbalazeStavke set ID_Skladiste = m.ID_Skladiste "
                        + " from RazduzenjeAmbalazeStavke as r,MagacinskaPolja as m "
                        + " where r.ID_MagacinskaPolja=m.ID_MagacinskaPolja and ID_DokumentaView = @param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    sql = "Update RazduzenjeAmbalazeStavke Set ProsecnaNabavnaCena=c.ProsecnaNabavnaCena "
                      + " from RazduzenjeAmbalazeStavke as r,cenovnik as c "
                      + " where r.ID_ArtikliView=c.ID_ArtikliView and r.ID_Skladiste=c.ID_Skladiste and r.ProsecnaNabavnaCena=0 "
                      + " and r.ID_DokumentaView=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    sql = "Update RazduzenjeAmbalazeStavke Set UvecanjeCene=(r.Kolicina * r.ProsecnaNabavnaCena / r.KolicinaProizvoda ) "
                     + " from RazduzenjeAmbalazeStavke as r ,RazduzenjeAmbalaze as n ,Nalog As nn "
                     + " where r.ID_DokumentaView=n.ID_DokumentaView and n.ID_Nalog=nn.ID_Nalog  and r.ID_Proizvodi=nn.ID_SirovinaView  "
                     + " and r.ID_DokumentaView=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    sql = "Update RazduzenjeAmbalazeStavke Set ProsecnaNabavnaCenaProizvoda=nn.ProsecnaNabavnaCena+r.UvecanjeCene "
                        + " from RazduzenjeAmbalazeStavke as r ,RazduzenjeAmbalaze as n ,Nalog As nn "
                        + " where r.ID_DokumentaView=n.ID_DokumentaView and n.ID_Nalog=nn.ID_Nalog  and r.ID_Proizvodi=nn.ID_SirovinaView  "
                        + " and r.ID_DokumentaView=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                default:
                    break;

                //////////////////////////////////////////////////////////////////
                case "Artikli":
                    string sqla = "";
                    string sqlg = "";
                    string sqlu = "";
                    string StariNaziv = "";
                    string NoviNaziv = "";
                    DataTable ta = new DataTable();
                    DataTable tg = new DataTable();
                    sqla = "Select * from Artikli  WITH(NOLOCK) where ID_Artikli=@param0";
                    ta = db.ParamsQueryDT(sqla, IdDokView);
                    sqlg = "Select * from ArtikliGrupaPoreza  WITH(NOLOCK) Where ID_ArtikliView=@param0";
                    tg = db.ParamsQueryDT(sqlg, ta.Rows[0]["ID_Artikli"]);
                    if (tg.Rows.Count > 0)
                    {
                        int llast = 0;
                        llast = tg.Rows.Count - 1;
                        //rsgrupa.MoveLast
                        if (tg.Rows[llast]["ID_TarifaPoreza"].ToString() == ta.Rows[0]["ID_TarifaPoreza"].ToString())
                        { }
                        else
                        {
                            string datum = System.DateTime.Now.ToString("dd.MM.yy");
                            //datum = datum.Replace("", "'");values('" + Format(Now, "dd.mm.yy") + "'
                            sqlu = " insert into ArtikliGrupaPoreza(DatumGrupe, ID_ArtikliView, ID_TarifaPoreza) values(@param0,@param1,@param2)";
                            tg = db.ParamsQueryDT(sqlu, datum, ta.Rows[0]["ID_Artikli"].ToString(), ta.Rows[0]["ID_TarifaPoreza"].ToString());
                        }
                    }
                    else
                    {
                        string mgodina = System.DateTime.Now.ToString().Substring(6, 2);
                        string mdatum = "01.01." + mgodina;
                        sqlu= " insert into ArtikliGrupaPoreza(DatumGrupe, ID_ArtikliView, ID_TarifaPoreza) values(@param0,@param1,@param2)";
                        tg = db.ParamsQueryDT(sqlu, mdatum, ta.Rows[0]["ID_Artikli"].ToString(), ta.Rows[0]["ID_TarifaPoreza"].ToString());                  
                    }

                    if (forma.Controls["OOperacija"].Text == "IZMENA")
                    { 
                        sqla = "select NazivArt  as  n from ArtikliTotali  WITH(NOLOCK) where ID_ArtikliTotali=@param0";
                        ta = db.ParamsQueryDT(sqla, IdDokView);
                        StariNaziv = ta.Rows[0]["n"].ToString();

                        sqla = "select NazivArtikla  as  n from Artikli  WITH(NOLOCK) where ID_Artikli=@param0";
                        ta = db.ParamsQueryDT(sqla, IdDokView);
                        NoviNaziv = ta.Rows[0]["n"].ToString();
                        if (StariNaziv.Trim() != NoviNaziv.Trim())
                            db.ExecuteStoreProcedure("PlusAzuriranjeVezanih", "NazivDokumenta: " + Dokument, "IdDokument: " + IdDokView, "StariNaziv: " + StariNaziv, "NoviNaziv:" + NoviNaziv);
                    }
                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + Dokument, "IdDokument:" + IdDokView);                
                    break;
                //-----------------------------------
                case "Komitenti":                   
                    string sqlk = "";
                    string sqlkk = "";
                    string sqli = "";
                    DataTable ti = new DataTable();
                    DataTable tk = new DataTable();
                    DataTable tkk = new DataTable();


                    sqlk = "Select * from Komitenti where ID_Komitenti=@param0";
                    tk = db.ParamsQueryDT(sqlk, IdDokView);
                                                                                                                                                
                    sqlkk = "Select * from VezaKomitentKomercijalisti Where ID_KomitentiView=@param0";
                    sqlkk += " order by datum, id_VezaKomitentKomercijalisti";
                    tkk = db.ParamsQueryDT(sqlkk,IdDokView);
                    if (tkk.Rows.Count > 0)
                    {                      
                        if (tk.Rows[tkk.Rows.Count - 1]["ID_OdgovornoLice1"].ToString() == tk.Rows[0]["ID_OdgovornoLice1"].ToString())
                        { }
                        else
                        {
                                                     
                            string datumFormat = System.DateTime.Now.ToString("dd.MM.yy");
                            if (tkk.Rows[tkk.Rows.Count - 1]["Datum"].ToString() != datumFormat)
                            {
                                sqli = " insert into VezaKomitentKomercijalisti(Datum, ID_KomitentiView, ID_OdgovornoLice1 )Values(@param0,@param1,@param2)";
                                ti = db.ParamsQueryDT(sqli, datumFormat, tk.Rows[0]["ID_KomitentiView"].ToString(), tk.Rows[0]["ID_OdgovornoLice1"].ToString());
                            }
                            else
                            {
                                sqli = "update VezaKomitentKomercijalisti set ID_Odgovornolice1=@param0";
                                sqli += " where ID_VezaKomitentKomercijalisti = @param1";
                                ti = db.ParamsQueryDT(sqli, tk.Rows[0]["ID_OdgovornoLice1"], tkk.Rows[tkk.Rows.Count - 1]["ID_VezaKomitentKomercijalisti"].ToString());
                            }
                        }
                    }
                    else// ne postoji podatak za odgovorno lice
                    {         
                        string ggodina = System.DateTime.Now.ToString().Substring(6, 2);
                        string gdatum = "01.01." + ggodina;
                        sqli = " insert into VezaKomitentKomercijalisti(Datum, ID_KomitentiView, ID_OdgovornoLice1) values(@param0,@param1,@param2)";
                        ti = db.ParamsQueryDT(sqli, gdatum, tk.Rows[0]["ID_KomitentiView"].ToString(), tk.Rows[0]["ID_OdgovornoLice1"].ToString());
                    }

                    if (forma.Controls["OOperacija"].Text == "IZMENA")
                    {
                        sqlk = "select NazivKom  as  n from KomitentiTotali  Where ID_KomitentiTotali=@param0";
                        tk= db.ParamsQueryDT(sqli, IdDokView);
                        StariNaziv = tk.Rows[0]["n"].ToString();
                        sqlk = "select NazivKomitenta  as  n from Komitenti  Where ID_Komitenti=@param0";
                        tk = db.ParamsQueryDT(sqli, IdDokView);
                        NoviNaziv= tk.Rows[0]["n"].ToString();
                        if (StariNaziv.Trim() != NoviNaziv.Trim())
                            db.ExecuteStoreProcedure("PlusAzuriranjeVezanih", "NazivDokumenta: " + Dokument, "IdDokument: " + IdDokView, "StariNaziv: " + StariNaziv, "NoviNaziv:" + NoviNaziv);
                    }
                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + Dokument, "IdDokument:" + IdDokView);          
                    break;            
                    ///////////////////////////////////////////////////////////////////
            }
            DodatnaAzuriranja = true;
            return DodatnaAzuriranja;
        }

        public Boolean DodatnaAzuriranjaPosleUnosa(string Dokument, string IdDokView)
        {
            bool DodatnaAzuriranjaPosleUnosa = false;
            forma = new Form();
            forma = Program.Parent.ActiveMdiChild;
            string SkladisteJe = "";
            string ID_Kalkulacija = "1";
            string rezultat = "";
            string NazivDokumenta = forma.Controls["limedok"].Text;
            clsObradaKalkulacije rt = new clsObradaKalkulacije();
            switch (Dokument)
            {             
                 case "KalkulacijaZavisnihTroskova":
                    if (forma.Controls["OOperacija"].Text == "UNOS")
                    {
                        sql = "delete from KalkulacijaUfStavke where ID_ArtikliView=1 and id_Dokumentaview=@param0";
                        ret = db.ParamsInsertScalar(sql, IdDokView);

                        sql = "select ID_DokumentaView from KalkulacijaUfStavke where id_Dokumentaview=@param0";
                        DataTable dt = db.ParamsQueryDT(sql, IdDokView);
                        if (dt.Rows.Count == 0)
                        {
                            sql = "insert into KalkulacijaUfStavke(id_DokumentaView, ID_ArtikliView) "
                                + " select @param0, r.ID_SirovinaView "
                                + " from KalkulacijaUf as k, ReceptiStavke as r "
                                + " where  k.id_normativview=r.id_DokumentaView and k.ID_DokumentaView=@param1";
                            DataTable dtr = db.ParamsQueryDT(sql, IdDokView, IdDokView);
                        }
                    }
                    break;
                case "PripremaZaPlacanje":
                    sql = "update PlacanjaNaplate set ID_KomitentiView=t.ID_KomitentiView "
                    + " from PlacanjaNaplate as p, TekuciRacuniKomitenata as t "
                    + " where p.ID_KomitentiView =1 AND ID_TekuciRacuniView=ID_TekuciRacuniKomitenata "
                    + " and p.ID_DokumentaView=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "Prijemnica": //sto ovo kad ima ID_DokumentaStablo=501?
                case "Otpremnica": //sto ovo kad ima ID_DokumentaStablo=501?
                case "LotPrijemnica":
                    //SVI PRIJEMI SOJE ZAKLJUCNO SA 31.08.TEKUCE GODINE VODE SE KAO KAO ROD PREDHODNE GODINE  INACE JE ROD TEKUCE GODINE
                    sql = "update otpremnica set godinaroda= (case when month(d.datum)<9 then Year(d.datum)-1 else Year(d.datum) end) "
                   + " from otpremnica as o,OtpremnicaStavke as os ,dokumenta as d  "
                   + " where o.ID_DokumentaView=os.ID_DokumentaView and o.ID_DokumentaView=d.ID_Dokumenta and ID_DokumentaStablo=501 "
                   + " and os.id_artikliview=731 and o.ID_DokumentaView=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "UlazniRacun":
                case "KonacniUlazniRacun":
                    // ako se u ulaznom racunu pored artikala nalaze i zavisni troskovi tada iznos zavisnih troskova rasporedjujemo
                    // po jedinici mere zavisnog troska na artikle ulaznog racuna i uvecavamo nabavnu cenu svih artikala za taj iznos
                    sql = "select a.JedinicaMere FROM RacunStavke as r, ArtikliStavkeView as a "
                        + " where r.ID_ArtikliView=a.ID_ArtikliStavkeView  and (a.JedinicaMere='Kolicina' or "
                        + " a.JedinicaMere='Vrednost' or a.JedinicaMere='UkupanBroj') "
                        + " and a.NazivArt not like '%prefaktur%' and r.ID_DokumentaView=@param0";
                    DataTable dt1 = db.ParamsQueryDT(sql, IdDokView);
                    // postoje troskovi u stavkama
                    if (dt1.Rows.Count != 0)
                    {
                        if (MessageBox.Show("Da li troskovi sa racuna povecavaju nabavnu vrednost artikala sa racuna?", "Zavisni troskovi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            UvecanjeVrednostiZaZavisneTroskove(IdDokView);
                        }
                    }
                    if (Dokument == "KonacniUlazniRacun")
                    {
                        sql = "select SifrarnikValuta.OznVal from Racun, SifrarnikValuta "
                        + " where Racun.ID_SifrarnikValuta = SifrarnikValuta.ID_SifrarnikValuta "
                        + " and SifrarnikValuta.OznVal =@param0 "
                        + " and ID_DokumentaView=@param1";
                        DataTable dt2 = db.ParamsQueryDT(sql, Program.DomacaValuta.Trim(), IdDokView);
                        if (dt2.Rows.Count != 0)
                        {
                            // ulazni racun je u domacoj valuti
                            sql = "select OpisSkladista from skladiste where ID_Skladiste=@param0";
                            DataTable dt3a = db.ParamsQueryDT(sql, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").ID);
                            if (dt3a.Rows.Count != 0)
                            {
                                SkladisteJe = dt3a.Rows[0]["OpisSkladista"].ToString().Trim();
                            }
                            else break;
                            if (SkladisteJe == "Reexport")
                            {
                                sql = "update RacunStavke set PDV=0 "
                                    + " from RacunStavke as rs, ArtikliTotali as at  "
                                    + " where rs.ID_ArtikliView=at.ID_ArtikliTotali "
                                    + " and rs.ID_DokumentaView=@param0";
                                ret = db.ParamsInsertScalar(sql, IdDokView);
                            }
                            else
                            {
                                sql = "update RacunStavke set PDV=Rs.Kolicina*rs.ProdajnaCena*PDVStopa/100*((100-rs.ProcenatRabata)/100)  "
                                    + " from RacunStavke as rs, KonacniUlazniRacunStavkeView as at  "
                                    + " where rs.ID_DokumentaView=at.ID_KonacniUlazniRacunStavkeView and rs.ID_ArtikliView=at.ID_ArtikliView "
                                    + " and rs.ID_DokumentaView=@param0";
                                ret = db.ParamsInsertScalar(sql, IdDokView);
                            }
                        }
                        else
                        {
                            // ulazni racun je u ino valuti i PDV=0
                            sql = "update RacunStavke set PDV=0 "
                                   + " from RacunStavke as rs, ArtikliTotali as at  "
                                   + " where rs.ID_ArtikliView=at.ID_ArtikliTotali "
                                   + " and rs.ID_DokumentaView=@param0";
                            ret = db.ParamsInsertScalar(sql, IdDokView);
                        }
                    }
                    // promena ulaznog racuna prekrije nabavnu cenu pa je doazuriramo rasporedom troskova kroz kalkulaciju ako postoji
                    if (forma.Controls["OOperacija"].Text == "UNOS" || ((Bankom.frmChield)forma).idReda == -1)
                    {
                        sql = "select BrojDokumenta as BD,ID_DokumentaView as IDK "
                            + " from KalkulacijaUf as r WITH (NOLOCK), Dokumenta as d WITH (NOLOCK) "
                            + " where d.ID_Dokumenta=r.ID_DokumentaView and r.ID_UlazniRacunceo=@param0 ";
                        DataTable dt4 = db.ParamsQueryDT(sql, IdDokView);
                        if (dt4.Rows.Count != 0)
                        {
                            ID_Kalkulacija = dt4.Rows[0]["IDK"].ToString();
                        }
                    }
                    else
                    {
                        sql = "select d.BrojDokumenta AS BD, r.ID_DokumentaView AS IDK, kr.ID_ArtikliView, rs.ID_ArtikliView as NoviArtikal "
                           + " from KalkulacijaUf as r WITH (NOLOCK), Dokumenta as d WITH (NOLOCK), " + Dokument.Trim() + "Totali as kr WITH (NOLOCK), RacunStavke as rs WITH (NOLOCK)"
                           + " where  r.ID_DokumentaView = d.ID_Dokumenta and r.ID_UlazniRacunCeo = kr.ID_" + Dokument.Trim() + "Totali "
                           + " and kr.IId = rs.ID_RacunStavke and r.ID_UlazniRacunCeo = " + IdDokView + " and kr.iid = " + Convert.ToString(((Bankom.frmChield)forma).idReda);
                        DataTable dt4 = db.ReturnDataTable(sql);
                        if (dt4.Rows.Count != 0)
                        {
                            ID_Kalkulacija =dt4.Rows[0]["IDK"].ToString();
                            // postoji kalkulacija vezana za ulazni racun
                            sql = "update KalkulacijaUfStavke SET id_ArtikliView=@param0 "
                                 + " where  id_dokumentaview=@param1 "
                                 + " and ID_ArtikliView=@param2";
                            ret = db.ParamsInsertScalar(sql, dt4.Rows[0]["NoviArtikal"].ToString(), ID_Kalkulacija, dt4.Rows[0]["ID_ArtikliView"].ToString());
                        }
                    }
                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + Dokument, "IdDokument:" + IdDokView);
                   
                    
                    rt.RasporedTroskova(Convert.ToInt64(ID_Kalkulacija), "", "", "","");
                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + Dokument, "IdDokument:" + IdDokView);

                    if (Dokument == "KonacniUlazniRacun")
                        db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PDVKalkulacijaUlaza", "IdDokument:" + ID_Kalkulacija);
                    else
                        db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:KalkulacijaUlaza", "IdDokument:" + ID_Kalkulacija);

                    break;
                case "KalkulacijaUlaza":
                case "PDVKalkulacijaUlaza":
                    if (Dokument == "PDVKalkulacijaUlaza")
                    {
                        //sql = "select DISTINCT ID_UlazniRacunCeo AS IdUR from PDVKalkulacijaUlazaTotali with (nolock) "
                        //    + " where ID_PDVKalkulacijaUlazaTotali=@param0";
                        //Console.WriteLine(sql);
                        //DataTable dt5 = db.ParamsQueryDT(sql, IdDokView);
                        //if (dt5.Rows.Count >0)
                        //{
                            string IDUR = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "UlzniRacunBroj").ID;

                            //if (dt5.Rows[0]["IdUR"].ToString() != IDUR) ///BORKA IZMENJENO != U ==
                            //{
                                sql = "select DISTINCT datumcarinjenja, ID_SifrarnikValuta,OznVal from KonacniUlazniRacunTotali WITH (NOLOCK) where ID_KonacniUlazniRacunTotali = @param0";
                        Console.WriteLine(sql);
                                DataTable dt6 = db.ParamsQueryDT(sql, IDUR);
                                if (dt6.Rows.Count > 0)
                                {
                                    if (dt6.Rows[0]["ID_SifrarnikValuta"].ToString() != "1")
                                    {
                                        string kojavaluta = dt6.Rows[0]["OznVal"].ToString(); 
                                        clsProveraIspravnosti cPI = new clsProveraIspravnosti();
                                        Vrati = cPI.PostojiKurs(dt6.Rows[0]["datumcarinjenja"].ToString(), dt6.Rows[0]["ID_SifrarnikValuta"].ToString(),kojavaluta, ref sKurs, ref Poruka);
                                        if (Vrati == true)
                                        {
                                            sql = "update RacunStavke set NabavnaCena=(ProdajnaCena*((100-ProcenatRabata)/100))* @param0 "
                                                + " where  id_dokumentaview=@param1 ";
                                            ret = db.ParamsInsertScalar(sql, sKurs, IDUR);
                                        }
                                        //ret = db.ParamsInsertScalar(sql, sKurs, IdDokView); /// BORKA
                                        db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:KonacniUlazniRacun", "IdDokument:" +IDUR);
                                        db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:" + IDUR);
                                        
                                    }
                                }
                            //}
                        //}
                    }
                    if (Dokument == "KalkulacijaUlaza")
                    {
                        sql = "select ID_UlazniRacunZaUslugeTotali as IId, ID_ArtikliView AS IdUR,FakturnaVrednost as FV from PDVKalkulacijaUlazaTotali with (nolock) "
                            + " where  BrDokPlusNazivArt=@param0";
                        DataTable dt7 = db.ParamsQueryDT(sql, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrDokPlusNazivArt").Vrednost);
                        if (dt7.Rows.Count != 0)
                        {
                            //vidim da nema koda u Vb6 za KalkulacijuUlaza
                        }
                    }
                    else //PDVkalkulacijaulaza BrDokRacunPlusNazivArt
                    {
                        string redracunazausluge = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrDokPlusNazivArt").ID;
                        string  redulaznogracuna= forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrDokRacunPlusNazivArt").ID;

                        sql = "select IId, ID_UlazniRacunZaUslugeCeo as IdUR, ID_ArtikliView as ID_Trosak, datumdpo as datum, FakturnaVrednost as FV "
                            + " from UlazniRacunZaUslugeCeo with (nolock) "
                            + " where  iid=@param0";
                        Console.WriteLine(sql);
                        DataTable dt8 = db.ParamsQueryDT(sql,redracunazausluge );
                        if (dt8.Rows.Count > 0)
                        {
                            sql = "select IId, ID_KonacniUlazniRacunTotali AS IdRacun,ID_ArtikliView "
                                + " from KonacniUlazniRacunTotali with (nolock) "
                                + " where  iid=@param0";
                            DataTable dt9 = db.ParamsQueryDT(sql, redulaznogracuna);
                            if (dt9.Rows.Count >0)
                            {
                                if (forma.Controls["OOperacija"].Text == "UNOS")
                                {
                                    sql = "update kalkulacijaufstavke set ID_Trosak= @param0 "
                                        + " where  id_kalkulacijaufstavke=(select max(id_kalkulacijaufstavke) from kalkulacijaufstavke)";
                                    ret = db.ParamsInsertScalar(sql, dt8.Rows[0]["ID_Trosak"].ToString());
                                    sql = "update kalkulacijaufstavke set ID_ArtikliView= @param0 "
                                        + " where  id_kalkulacijaufstavke=(select max(id_kalkulacijaufstavke) from kalkulacijaufstavke)";
                                    ret = db.ParamsInsertScalar(sql, dt9.Rows[0]["ID_ArtikliView"].ToString());
                                }
                                else
                                {
                                    sql = "update kalkulacijaufstavke set ID_Trosak= @param0 "
                                        + " where  id_kalkulacijaufstavke= @param1";
                                    ret = db.ParamsInsertScalar(sql, dt8.Rows[0]["ID_Trosak"].ToString(), Convert.ToString(((Bankom.frmChield)forma).idReda));

                                    sql = "update kalkulacijaufstavke set ID_ArtikliView= @param0 "
                                        + " where  id_kalkulacijaufstavke= @param1";
                                    ret = db.ParamsInsertScalar(sql, dt9.Rows[0]["ID_ArtikliView"].ToString(), Convert.ToString(((Bankom.frmChield)forma).idReda));
                                }
                            }

                            if (forma.Controls["OOperacija"].Text != "BRISANJE")
                            {
                                sKurs = 1;
                                string kojavaluta = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "UlUslugaOznVal").Vrednost.Trim();
                                if (kojavaluta != "")
                                {
                                    if (kojavaluta != Program.DomacaValuta)
                                    {
                                        sql = "select ID_SifrarnikValuta as IDV from sifrarnikValuta where Oznval= @param0";
                                        DataTable dt10 = db.ParamsQueryDT(sql, kojavaluta);
                                        if (dt10.Rows.Count != 0)
                                        {
                                            clsProveraIspravnosti cPI = new clsProveraIspravnosti();
                                            Vrati = cPI.PostojiKurs(dt8.Rows[0]["datum"].ToString(), dt10.Rows[0]["ID_SifrarnikValuta"].ToString(),kojavaluta, ref sKurs, ref Poruka);
                                            if (Vrati == true)
                                            {
                                                KursiranTrosak = Convert.ToDouble(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "FakturnaVrednost").Vrednost) * sKurs;
                                                string uslovkalk = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "UslovKalkulacije").Vrednost;
                                                string iduslov = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "UslovKalkulacije").ID;
                                                string ulrbroj= forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "UlzniRacunBroj").ID;
                                                double fvred= Convert.ToDouble(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "FakturnaVrednost").Vrednost);

                                                rt.RasporediTRoskoveNaNekiOdNacina(IdDokView,dt8.Rows[0]["iid"].ToString(), dt8.Rows[0]["ID_Trosak"].ToString(), sKurs, uslovkalk, iduslov,ulrbroj, fvred, KursiranTrosak);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    rt.RasporedTroskova(Convert.ToInt64(IdDokView), "", "", "", "");
                    if (Dokument == "PDVKalkulacijaUlaza")
                        db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:KonacniUlazniRacun", "IdDokument:" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "UlzniRacunBroj").ID);
                    else
                        db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:UlazniRacun", "IdDokument:" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "UlzniRacunBroj").ID);

                    db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "UlzniRacunBroj").ID);

                    break;
                case "UlazniAvansniRacun":
                    sql = "update RacunZaUslugeStavke set PDV=FakturnaVrednost*osnovica/(100+osnovica) "
                        + " from RacunZaUslugeStavke as rs, TarifaPoreza as t "
                        + " where  rs.ID_Poreza=t.ID_TarifaPoreza and PDV = 0 and ID_DokumentaView= @param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "UlazniRacunZaUsluge":
                case "PDVUlazniRacunZaUsluge":
                    sql = "update RacunZaUslugeStavke set PDV=FakturnaVrednost*osnovica/100 "
                        + " from RacunZaUslugeStavke as rs, TarifaPoreza as t "
                        + " where  rs.ID_Poreza=t.ID_TarifaPoreza and PDV = 0 and ID_DokumentaView= @param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    if (KojiTrosakKalkulacije > 0)
                        sql = "select ID_ArtikliView AS IdUR, FakturnaVrednost as FV FROM RacunZaUslugeStavke "
                            + " where ID_RacunZaUslugeStavke=@param0";
                    DataTable dt11 = db.ParamsQueryDT(sql, Convert.ToString(((Bankom.frmChield)forma).idReda));
                    if (dt11.Rows.Count != 0)
                    {
                        // treba proveriti ovaj update ne znam idreda kad imamo vise gridova
                        sql = "update KalkulacijaUfStavke set ID_Trosak=@param0 "
                            + " where ID_trosak= @param1 and ID_UlazniRacunZaUslugeCeo =@param2 ";
                        ret = db.ParamsInsertScalar(sql, dt11.Rows[0]["IdUR"].ToString(), KojiTrosakKalkulacije.ToString(), Convert.ToString(((Bankom.frmChield)forma).idReda));

                        if (Dokument == "UlazniRacunZaUsluge")
                            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:UlazniRacunZaUsluge", "IdDokument:" + IdDokView);
                        else
                            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PDVUlazniRacunZaUsluge", "IdDokument:" + IdDokView);

                        sql = "select distinct Id_dokumentaview from KalkulacijaUfStavke "
                            + " where ID_UlazniRacunZaUslugeCeo =@param0";
                        DataTable dt12 = db.ParamsQueryDT(sql, IdDokView);
                        foreach (DataRow row in dt12.Rows)
                        {
                            rt.RasporedTroskova(Convert.ToInt64(ID_Kalkulacija), "", "", "", "");
                            if (Dokument == "UlazniRacunZaUsluge")
                                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:KalkulacijaUlaza", "IdDokument:" + row["Id_dokumentaview"].ToString());
                            else
                                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PDVKalkulacijaUlaza", "IdDokument:" + row["Id_dokumentaview"].ToString());
                        }
                    }

                    break;
                case "ObracunProizvodnje":
                    if (forma.Controls["OOperacija"].Text == "UNOS")
                    {
                        sql = "delete from InterniNalogZaRobuStavke where id_Dokumentaview=@param0";
                        ret = db.ParamsInsertScalar(sql, IdDokView);
                        ObracunProizvodnje(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").ID);
                    }

                    break;
                case "LabaratorijskaAnaliza":
                    sql = "select distinct sum(StandardnaVrednost) as SumaStandardnaVrednost, sum(Vrednost) as SumaVrednost, ID_ReferentniDokument, ID_ArtikliTotali "
                        + " from LabaratorijskaAnalizaCeo "
                        + " where  ObracunNaJUS='DA-Obracun' and id_LabaratorijskaAnalizaCeo=@param0"
                        + " group by ID_ReferentniDokument, ID_ArtikliTotali";
                    DataTable dt13 = db.ParamsQueryDT(sql, IdDokView);
                    if (dt13.Rows.Count != 0)
                    {
                        if (Convert.ToDouble(dt13.Rows[0]["SumaStandardnaVrednost"]) < Convert.ToDouble(dt13.Rows[0]["SumaVrednost"]))
                        {
                            sql = "update RacunStavke set Primljeno=(kolicina* @param0) / @param1 "
                                + " where id_DokumentaView= @param2 and id_ArtikliView= @param3 ";
                            ret = db.ParamsInsertScalar(sql, Convert.ToDouble(dt13.Rows[0]["SumaStandardnaVrednost"]), Convert.ToDouble(dt13.Rows[0]["SumaVrednost"]), dt13.Rows[0]["ID_ReferentniDokument"].ToString(), dt13.Rows[0]["ID_ArtikliTotali"].ToString());

                            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:UlazniRacunZaUsluge", "IdDokument:" + dt13.Rows[0]["ID_ReferentniDokument"].ToString());
                        }
                    }

                    break;
                case "PDVUlazniPredracun":
                case "PDVNarudzbenica":
                    sql = "update PredracunStavke set PDV=Rs.Kolicina*rs.ProdajnaCena*PDVStopa/100*((100-rs.ProcenatRabata)/100) "
                        + " from PredracunStavke as rs, PDVUlazniPredracunStavkeView as at "
                        + " where rs.ID_ArtikliView=at.ID_ArtikliView and rs.ID_DokumentaView=at.ID_PDVUlazniPredracunStavkeView and rs.PDV = 0 and ID_DokumentaView =@param0 ";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "InternaPrijemnica":// ' ??????????????????? cemu ovo sluzi
                    sql = "update RacunStavke SET NabavnaCena=(ProdajnaCena*((100-ProcenatRabata)/100)) "
                        + " where ID_DokumentaView =@param0 ";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "RadniNalog":
                case "NalogZaPoluproizvod":
                case "LotRadniNalog":
                case "LotNalogZaPoluproizvod":
                case "LotNalogZaPoluproizvodEkstrakcija":
                    if (Program.imeFirme == "Bankom" || Program.imeFirme == "Bankom biotehnologija")
                    {
                        if (forma.Controls["OOperacija"].Text == "UNOS")
                        {
                            sql = "update NalogKooperantaSirovineStavke set PlanskaCena=c.Prosecnanabavnacena "
                                + " from Cenovnik as c, NalogKooperanta as r,Recepti as n , NalogKooperantaSirovineStavke as rss "
                                + " where n.id_DokumentaView=rss.ID_NormativView and  c.ID_ArtikliView=n.ID_ProizvodView "
                                + " and c.ID_Skladiste=r.ID_Skladisteu and r.ID_DokumentaView= rss.ID_DokumentaView and r.ID_DokumentaView= @param0 ";
                            ret = db.ParamsInsertScalar(sql, IdDokView);
                        }
                    }
                    else // nije Bankom
                    {
                        sql = "update NalogKooperantaSirovineStavke set PlanskaCena=c.Prosecnanabavnacena "
                               + " from Cenovnik as c, NalogKooperanta as r,Recepti as n "
                               + " wheren.id_DokumentaView=r.ID_NormativView and  c.ID_ArtikliView=n.ID_ProizvodView "
                               + " and c.ID_Skladiste=r.ID_Skladisteu and r.ID_DokumentaView= @param0 ";
                        ret = db.ParamsInsertScalar(sql, IdDokView);
                    }

                    if (Dokument == "RadniNalog" || Dokument == "LotRadniNalog")
                    {
                        if (Dokument == "RadniNalog")
                        {
                            strParams = "@param1=" + IdDokView.ToString() + "`";
                            str = "delete from NalogKooperantaStavke where [ID_DokumentaView] = @param1";
                            lista.Add(new string[] { str, strParams, "NalogKooperantaStavke", dokType, IdDokView.ToString() });
                            lista.ToArray();
                        }
                        if (((Bankom.frmChield)forma).idstablo == 14 || ((Bankom.frmChield)forma).idstablo == 144)
                        {
                            if (((Bankom.frmChield)forma).idstablo == 14)
                            {
                                sql = " select r.ID_SirovinaView, r.Kolicina*(n.BrojSarzi*300)/100 as Kolicina, n.ID_NormativView "
                                     + " from NalogKooperanta as n, ReceptiStavke as r "
                                     + " where n.id_normativview=r.id_DokumentaView and n.ID_DokumentaView=@param0";

                                DataTable dtr = db.ParamsQueryDT(sql, IdDokView);
                                foreach (DataRow row in dtr.Rows)
                                {
                                    strParams = "@param1=" + IdDokView.ToString() + "`";
                                    strParams += "@param2=" + row["ID_SirovinaView"].ToString() + "`";
                                    strParams += "@param3=" + row["Kolicina"].ToString() + "`";
                                    strParams += "@param4=" + row["ID_NormativView"].ToString() + "`";
                                    str = "insert into NalogKooperantaStavke(id_DokumentaView, ID_SirovinaView, Kolicina, ID_NormativView) "
                                        + " values (@param1, @param2, @param3, @param4 )";
                                    lista.Add(new string[] { str, strParams, "NalogKooperantaStavke", dokType, IdDokView.ToString() });
                                    lista.ToArray();
                                }

                                strParams = "@param1=" + IdDokView.ToString() + "`";
                                str = "update NalogKooperantaStavke set [kolicina]=ns.kolicina +(n.kolicina-(n.BrojSarzi*300)) "
                                    + " from nalogkooperanta as n, nalogkooperantastavke as ns "
                                    + " where n.id_dokumentaview=ns.id_DokumentaView and ns.id_sirovinaview=305 and  ns.[ID_DokumentaView] = @param1";
                                lista.Add(new string[] { str, strParams, "NalogKooperantaStavke", dokType, IdDokView.ToString() });
                                lista.ToArray();
                            }
                            else // nema sarze
                            {
                                sql = " select r.ID_SirovinaView, (r.Kolicina/100)*n.kolicina as Kolicina, n.ID_NormativView "
                                   + " from NalogKooperanta as n, ReceptiStavke as r "
                                   + " where n.id_normativview=r.id_DokumentaView and n.ID_DokumentaView=@param0";

                                DataTable dtr1 = db.ParamsQueryDT(sql, IdDokView);
                                foreach (DataRow row in dtr1.Rows)
                                {
                                    strParams = "@param1=" + IdDokView.ToString() + "`";
                                    strParams += "@param2=" + row["ID_SirovinaView"].ToString() + "`";
                                    strParams += "@param3=" + row["Kolicina"].ToString() + "`";
                                    strParams += "@param4=" + row["ID_NormativView"].ToString() + "`";
                                    str = "insert into NalogKooperantaStavke(id_DokumentaView, ID_SirovinaView, Kolicina, ID_NormativView) "
                                        + " values (@param1, @param2, @param3, @param4 )";
                                    lista.Add(new string[] { str, strParams, "NalogKooperantaStavke", dokType, IdDokView.ToString() });
                                    lista.ToArray();
                                }
                            }
                        }   //kraj 14 or 144 

                        if (Program.imeFirme == "Bankom" || Program.imeFirme == "Bankom biotehnologija" || Program.imeFirme == "Bioprotein")
                        {
                            if (Dokument == "RadniNalog")
                            {
                                sql = " select r.ID_SirovinaView,(r.Kolicina/100)*n.kolicina as Kolicina, n.ID_NormativView "
                                    + " from NalogKooperanta as n, ReceptiStavke as r "
                                    + " where n.id_normativview=r.id_DokumentaView and n.ID_DokumentaView=@param0";

                                DataTable dtr2 = db.ParamsQueryDT(sql, IdDokView);
                                foreach (DataRow row in dtr2.Rows)
                                {
                                    strParams = "@param1=" + IdDokView.ToString() + "`";
                                    strParams += "@param2=" + row["ID_SirovinaView"].ToString() + "`";
                                    strParams += "@param3=" + row["Kolicina"].ToString() + "`";
                                    strParams += "@param4=" + row["ID_NormativView"].ToString() + "`";
                                    str = "insert into NalogKooperantaStavke(id_DokumentaView, ID_SirovinaView, Kolicina, ID_NormativView) "
                                        + " values (@param1, @param2, @param3, @param4 )";
                                    lista.Add(new string[] { str, strParams, "NalogKooperantaStavke", dokType, IdDokView.ToString() });
                                    lista.ToArray();
                                }
                            }
                        }
                        strParams = "@param1=" + IdDokView.ToString() + "`";
                        str = "update NalogKooperantaStavke set ProsecnaNabavnaCena=c.ProsecnaNabavnaCena "
                            + " from Cenovnik as c, NalogKooperanta as r, NalogKooperantaStavke as rs "
                            + " where c.ID_ArtikliView=rs.ID_SirovinaView and rs.id_DokumentaView=r.ID_DokumentaView and c.ID_Skladiste=r.ID_SkladisteIz  and r.[ID_DokumentaView] = @param1";
                        lista.Add(new string[] { str, strParams, "NalogKooperantaStavke", dokType, IdDokView.ToString() });
                        lista.ToArray();

                        strParams = "@param1=" + IdDokView.ToString() + "`";
                        strParams += "@param2=" + IdDokView.ToString() + "`";
                        str = "update NalogKooperanta set PlanskaCena= (select( sum(rs.ProsecnaNabavnaCena*rs.kolicina)+ (tr.IznosTroska*r.kolicina))/r.kolicina "
                            + " from NalogKooperanta as r, NalogKooperantaStavke as rs ,TroskoviProizvodnjePoSirovinama as tr, dokumenta As d,recepti as re "
                            + " where  rs.id_DokumentaView=r.ID_DokumentaView and re.ID_ProizvodView= tr.ID_ArtikliView "
                            + " and tr.datumtroska=(select max(datumtroska) from TroskoviProizvodnjePoSirovinama where datumtroska<=d.datum and tr.ID_ArtikliView=ID_ArtikliView) "
                            + " and d.Id_Dokumenta = r.ID_DokumentaView and r.ID_Normativview=re.ID_DokumentaView And r.Id_DokumentaView = rs.ID_DokumentaView"
                            + " and r.[ID_DokumentaView] = @param1"
                            + " group by tr.IznosTroska,r.kolicina) "
                            + " Where ID_DokumentaView =@param2";
                        lista.Add(new string[] { str, strParams, "NalogKooperanta", dokType, IdDokView.ToString() });
                        lista.ToArray();

                        sql = "select oorderby as TrebaProvera from recnikpodataka where dokument=@param0 and oorderby>0 and oorderby<5";
                        DataTable dtr3 = db.ParamsQueryDT(sql, Dokument);
                        if (dtr3.Rows.Count != 0) TrebaProvera = dtr3.Rows[0]["TrebaProvera"].ToString();

                        if (TrebaProvera != "0")
                        {
                            dokType = "";
                            strParams = "";
                            str = "Execute stanje";
                            strTabela = "";
                            lista.Add(new string[] { str, strParams, strTabela, dokType, IdDokView.ToString() });
                            lista.ToArray();
                        }

                      
                        rezultat = db.ReturnSqlTransactionParamsFull(lista);

                        if (rezultat != "") { lista.Clear(); MessageBox.Show(rezultat); return false; }
                        lista.Clear();
                    }

                    if (Dokument == "NalogZaPoluproizvod" || Dokument == "LotNalogZaPoluproizvod")
                    {
                        if (forma.Controls["OOperacija"].Text == "UNOS")
                        {
                            sql = "delete from NalogKooperantaStavke where ID_SirovinaView=1 and id_Dokumentaview=@param0";
                            ret = db.ParamsInsertScalar(sql, IdDokView);

                            sql = "select ID_DokumentaView from NalogKooperantaStavke where id_Dokumentaview=@param0 ";
                            DataTable dtp = db.ParamsQueryDT(sql, IdDokView);
                            if (dtp.Rows.Count == 0)
                            {
                                if (Program.imeFirme == "Bankom" || Program.imeFirme == "Bioprotein")
                                {
                                    sql = "insert into NalogKooperantaStavke(id_DokumentaView, ID_SirovinaView, Kolicina,ID_NormativView ) "
                                        + " select @param0, r.ID_SirovinaView,0 as Kolicina, n.ID_NormativView "
                                        + " from NalogKooperantaSirovineStavke as n, ReceptiStavke as r "
                                        + " where n.id_normativview=r.id_DokumentaView and n.ID_DokumentaView=@param1";
                                }
                                else
                                {
                                    sql = "insert into NalogKooperantaStavke(id_DokumentaView, ID_SirovinaView, Kolicina,ID_NormativView ) "
                                       + " select @param0, r.ID_SirovinaView,0 as Kolicina, n.ID_NormativView "
                                       + " from NalogKooperanta as n, ReceptiStavke as r "
                                       + " where n.id_normativview=r.id_DokumentaView and n.ID_DokumentaView=@param1";                                   
                                }
                                ////DataTable dtp1 = db.ParamsQueryDT(sql, IdDokView, IdDokView);
                                ret = db.ParamsInsertScalar(sql, IdDokView, IdDokView);
                            }
                        }

                        if (Program.imeFirme == "Bankom" || Program.imeFirme == "Bioprotein")
                        {
                            if (NazivDokumenta == "NalogZaPoluproizvod" || NazivDokumenta == "LotNalogZaPoluproizvod")
                            {
                                sql = " select kss.id_Dokumentaview,sum(c.planskacena*ks.kolicina)/count( distinct kss.id_NalogKooperantaSirovineStavke) as SumaPlanskaVred, tr.IznosTroska "
                                    + " from  Cenovnik as c,NalogKooperanta as k, NalogKooperantaStavke as ks ,NalogKooperantaSirovineStavke as kss, "
                                    + " TroskoviProizvodnjePoSirovinama as tr,ReceptiStavke as rs, dokumenta As d,recepti as r "
                                    + " where kss.ID_Normativview = rs.id_dokumentaView And rs.ID_SirovinaView = c.ID_ArtikliView "
                                    + " and r.ID_ProizvodView= tr.ID_ArtikliView and ks.id_DokumentaView=k.ID_DokumentaView "
                                    + " and kss.id_DokumentaView=k.ID_DokumentaView  and d.Id_Dokumenta= k.ID_DokumentaView "
                                    + " and tr.datumtroska=(select max(datumtroska) from TroskoviProizvodnjePoSirovinama where datumtroska<=d.datum and tr.ID_ArtikliView=ID_ArtikliView) "
                                    + " and c.ID_Skladiste=k.ID_SkladisteIz AND c.ID_ArtikliView = ks.ID_SirovinaView  and rs.ID_DokumentaView=r.id_dokumentaView "
                                    + " and ks.kolicina>0 and k.ID_DokumentaView= @param0"
                                    + " group by kss.id_Dokumentaview,tr.IznosTroska";
                                DataTable dtp2 = db.ParamsQueryDT(sql, IdDokView);
                                if (dtp2.Rows.Count != 0)
                                {
                                    sql = " Update NalogKooperantaStavke set ProsecnaNabavnaCena= a.cena from (select (c.PlanskaCena*ks.kolicina /@param0)*(kss.planskacena+@param1)*sum(kss.kolicina)/(ks.kolicina) as cena, "
                                        + " ks.ID_SirovinaView,k.ID_DokumentaView "
                                        + " from Cenovnik as c,NalogKooperanta as k, NalogKooperantaStavke as ks ,NalogKooperantaSirovineStavke as kss "
                                        + " where c.ID_ArtikliView=ks.ID_SirovinaView and kss.id_DokumentaView=k.ID_DokumentaView "
                                        + " and ks.id_DokumentaView=k.ID_DokumentaView and c.ID_Skladiste=k.ID_SkladisteU "
                                        + " and ks.kolicina<>0 and k.ID_DokumentaView=@param2"
                                        + " group by k.ID_DokumentaView,ks.ID_SirovinaView,c.PlanskaCena,ks.kolicina,kss.planskacena) as a "
                                        + " where a.ID_SirovinaView=NalogKooperantaStavke.ID_SirovinaView and NalogKooperantaStavke.ID_DokumentaView=a.ID_DokumentaView "
                                        + " and NalogKooperantaStavke.ID_DokumentaView=@param3";
                                    ret = db.ParamsInsertScalar(sql, dtp2.Rows[0]["SumaPlanskaVred"].ToString(), dtp2.Rows[0]["IznosTroska"].ToString(), IdDokView, IdDokView);
                                }
                            }
                            if (NazivDokumenta == "LotNalogZaDoradu")
                            {
                                sql = " Update NalogKooperantaStavke set ProsecnaNabavnaCena=(nss.planskacena+tr.IznosTroskaDorade) "
                                    + " from NalogKooperantaStavke as ns, NalogKooperantaSirovineStavke As nss, "
                                    + " TroskoviProizvodnjePoSirovinama As tr, Dokumenta As d, Recepti As r "
                                    + " where NS.ID_DokumentaView=nss.ID_DokumentaView and r.ID_DokumentaView=nss.ID_NormativView "
                                    + " and d.ID_dokumenta=ns.ID_DokumentaView AND  tr.ID_ArtikliView=r.ID_ProizvodView "
                                    + " and tr.datumtroska=(select max(datumtroska) from TroskoviProizvodnjePoSirovinama where  datumtroska<=d.datum and ID_ArtikliView=r.ID_ProizvodView) "
                                    + " and NS.Kolicina <> 0 And d.Id_Dokumenta = @param0";
                                ret = db.ParamsInsertScalar(sql, IdDokView);
                            }

                            sql = " update NalogKooperantaSirovineStavke set Cena=( Select distinct ((select sum(kolicina*prosecnanabavnacena) "
                                + " from NalogKooperantaStavke where kolicina<>0 and ID_DokumentaView=rs.id_DokumentaView)/ "
                                + " (select sum(kolicina) from NalogKooperantaSirovineStavke where ID_DokumentaView=r.id_DokumentaView) "
                                + " ) from NalogKooperantaSirovineStavke as r,NalogKooperantaStavke  as rs "
                                + " where rs.id_DokumentaView=r.id_DokumentaView and r.ID_DokumentaView= @param0 ) "
                                + " from  NalogKooperantaSirovineStavke as r "
                                + " Where r.ID_DokumentaView =  @param1";
                            ret = db.ParamsInsertScalar(sql, IdDokView, IdDokView);

                        }
                        else
                        { // zasto ovde skladisteU a gore je SkladisteIz
                            sql = " select k.id_DokumentaView,sum(c.planskacena*ks.kolicina) as SumaPlanskaVred, tr.IznosTroska "
                                + " from Cenovnik as c,NalogKooperanta as k, NalogKooperantaStavke as ks ,TroskoviProizvodnjePoSirovinama as tr, dokumenta As d "
                                + " where ks.ID_DokumentaView = k.ID_DokumentaView and tr.ID_ArtikliView = ks.ID_SirovinaView and d.Id_Dokumenta= k.ID_DokumentaView "
                                + " and tr.datumtroska=(select max(datumtroska) from TroskoviProizvodnjePoSirovinama where datumtroska<=d.datum and tr.ID_ArtikliView=ID_ArtikliView) "
                                + " and c.id_skladiste = k.ID_SkladisteU And c.ID_ArtikliView = ks.ID_SirovinaView "
                                + " and k.ID_DokumentaView= @param0"
                                + " group by k.id_DokumentaView,tr.IznosTroska";
                            DataTable dtp2 = db.ParamsQueryDT(sql, IdDokView);
                            if (dtp2.Rows.Count != 0)
                            {
                                sql = " Update NalogKooperantaStavke set ProsecnaNabavnaCena=(c.PlanskaCena*ks.kolicina /@param0)*(k.planskacena+@param1)*k.kolicina/(ks.kolicina) "
                                    + " from Cenovnik as c,NalogKooperanta as k, NalogKooperantaStavke as ks "
                                    + " where c.ID_ArtikliView=ks.ID_SirovinaView and ks.id_DokumentaView=k.ID_DokumentaView and c.ID_Skladiste=k.ID_SkladisteU "
                                    + "  and ks.kolicina>0 and k.ID_DokumentaView=@param2";
                                ret = db.ParamsInsertScalar(sql, dtp2.Rows[0]["SumaPlanskaVred"].ToString(), dtp2.Rows[0]["IznosTroska"].ToString(), IdDokView);
                            }
                        }
                    }
                    if (Dokument == "LotNalogZaPoluproizvodEkstrakcija")
                    {
                        if (forma.Controls["OOperacija"].Text == "UNOS")
                        {
                            sql = "delete from NalogKooperantaStavke where ID_SirovinaView=1 and id_Dokumentaview=@param0";
                            ret = db.ParamsInsertScalar(sql, IdDokView);
                            sql = "delete from NalogKooperantaSirovinePomocneStavke where ID_SirovinaPomocneView=1 and id_Dokumentaview=@param0";
                            ret = db.ParamsInsertScalar(sql, IdDokView);

                            sql = "select ID_DokumentaView from NalogKooperantaStavke where id_Dokumentaview=@param0 ";
                            DataTable dtp = db.ParamsQueryDT(sql, IdDokView);
                            if (dtp.Rows.Count == 0)
                            {
                                sql = "insert into NalogKooperantaStavke(id_DokumentaView, ID_SirovinaView, Kolicina,ID_NormativView ) "
                                    + " select @param0, r.ID_SirovinaView,0 as Kolicina, n.ID_NormativView "
                                    + " from NalogKooperantaSirovineStavke as n, ReceptiStavke as r "
                                    + " where n.id_normativview=r.id_DokumentaView and n.ID_DokumentaView=@param1";
                                DataTable dtp3 = db.ParamsQueryDT(sql, IdDokView, IdDokView);
                                sql = "insert into NalogKooperantaSirovinePomocneStavke(id_DokumentaView, ID_SirovinaPomocneView, Kolicina,ID_NormativView ) "
                                   + " select @param0, r.ID_SirovinaPomocnaView,(R.kolicina*sum(n.Kolicina)) as Kolicina, n.ID_NormativView "
                                   + " from NalogKooperantaSirovineStavke as n, ReceptiStavkePomocne as r "
                                   + " where n.id_normativview=r.id_DokumentaView and n.ID_DokumentaView=@param1"
                                   + "  group by r.ID_SirovinaPomocnaView,n.ID_NormativView,R.kolicina ";
                                DataTable dtp4 = db.ParamsQueryDT(sql, IdDokView, IdDokView);
                            }
                        }

                        sql = " Update NalogKooperantaSirovinePomocneStavke set Kolicina= ( SELECT sum(NalogKooperantaSirovineStavke.kolicina)*rsp.kolicina "
                            + " from  NalogKooperantaSirovinePomocneStavke,NalogKooperantaSirovineStavke , ReceptiStavkePomocne as rsp "
                            + " where NalogKooperantaSirovineStavke.ID_DokumentaView=NalogKooperantaSirovinePomocneStavke.ID_DokumentaView and rsp.ID_DokumentaView=NalogKooperantaSirovineStavke.ID_NormativView and  "
                            + " rsp.ID_SirovinaPomocnaView = NalogKooperantaSirovinePomocneStavke.ID_SirovinaPomocneView and "
                            + " NalogKooperantaSirovinePomocneStavke.ID_DokumentaView= @param0 "
                            + " group by rsp.kolicina) "
                            + " where NalogKooperantaSirovinePomocneStavke.ID_DokumentaView= @param1 ";
                        ret = db.ParamsInsertScalar(sql, IdDokView, IdDokView);

                        sql = " Update NalogKooperantaSirovinePomocneStavke set ProsecnaNabavnaCena=c.Prosecnanabavnacena "
                          + " from Cenovnik as c,NalogKooperanta as r,  NalogKooperantaSirovinePomocneStavke as rss "
                          + " where c.ID_ArtikliView=rss.ID_SirovinaPomocneView and c.ID_Skladiste=r.ID_SkladisteIz  "
                          + " and r.ID_DokumentaView= rss.ID_DokumentaView  and r.ID_DokumentaView= @param0";
                        ret = db.ParamsInsertScalar(sql, IdDokView);

                        sql = " select kss.id_normativview,sum(c.planskacena*ks.kolicina)/count( distinct kss.id_NalogKooperantaSirovineStavke) as SumaPlanskaVred, "
                            + " tr.IznosTroskaEkstrakcije as IznosTroska, sum(cp.Prosecnanabavnacena*ksps.kolicina)/(count( distinct kss.id_NalogKooperantaSirovineStavke)*sum(kss.kolicina)) as TrosakPomocni"
                            + " from Cenovnik as c,NalogKooperanta as k, NalogKooperantaStavke as ks ,NalogKooperantaSirovineStavke as kss,NalogKooperantaSirovinePomocneStavke as ksps, "
                            + " TroskoviProizvodnjePoSirovinama as tr, dokumenta As d,recepti as r, ReceptiStavkePomocne as rsp,Cenovnik as cp "
                            + " where kss.ID_Normativview = r.id_dokumentaView And ks.ID_SirovinaView = c.ID_ArtikliView "
                            + " and r.ID_ProizvodView= tr.ID_ArtikliView and ks.id_DokumentaView=k.ID_DokumentaView "
                            + " and kss.id_DokumentaView=k.ID_DokumentaView  and ksps.id_DokumentaView=k.ID_DokumentaView and d.Id_Dokumenta= k.ID_DokumentaView "
                            + " and tr.datumtroska=(select max(datumtroska) from TroskoviProizvodnjePoSirovinama where datumtroska<=d.datum and tr.ID_ArtikliView=ID_ArtikliView)"
                            + "and c.ID_Skladiste=k.ID_SkladisteIz AND c.ID_ArtikliView = ks.ID_SirovinaView "
                            + "and rsp.id_dokumentaView=r.id_dokumentaView and rsp.ID_SirovinaPomocnaView =ksps.ID_SirovinaPomocneView and rsp.ID_SirovinaPomocnaView = cp.ID_ArtikliView"
                            + " and cp.ID_Skladiste=k.ID_SkladisteIz  and ks.kolicina>0 and k.ID_DokumentaView= @param0"
                            + " group by kss.id_normativview,tr.IznosTroskaEkstrakcije";
                        DataTable dtp5 = db.ParamsQueryDT(sql, IdDokView);
                        if (dtp5.Rows.Count != 0)
                        {
                            sql = " Update NalogKooperantaStavke set ProsecnaNabavnaCena= a.cena from (select (c.PlanskaCena*ks.kolicina /@param0)*(kss.planskacena+@param1+@param2)*sum(kss.kolicina)/(ks.kolicina) as cena, "
                                + " ks.ID_SirovinaView,k.ID_DokumentaView "
                                + " from Cenovnik as c,NalogKooperanta as k, NalogKooperantaStavke as ks ,NalogKooperantaSirovineStavke as kss "
                                + " where c.ID_ArtikliView=ks.ID_SirovinaView and kss.id_DokumentaView=k.ID_DokumentaView "
                                + " and ks.id_DokumentaView=k.ID_DokumentaView and c.ID_Skladiste=k.ID_SkladisteU "
                                + " and ks.kolicina>0 and k.ID_DokumentaView=@param3 "
                                + " group by k.ID_DokumentaView,ks.ID_SirovinaView,c.PlanskaCena,ks.kolicina,kss.planskacena) as a "
                                + " where a.ID_SirovinaView=NalogKooperantaStavke.ID_SirovinaView and NalogKooperantaStavke.ID_DokumentaView=a.ID_DokumentaView "
                                + " and NalogKooperantaStavke.ID_DokumentaView=@param4";
                            ret = db.ParamsInsertScalar(sql, dtp5.Rows[0]["SumaPlanskaVred"].ToString(), dtp5.Rows[0]["IznosTroska"].ToString(), dtp5.Rows[0]["TrosakPomocni"].ToString(), IdDokView, IdDokView);
                        }
                        sql = " update NalogKooperantaSirovinePomocneStavke set Uuser= @param0 where Id_DokumentaView = @param1";
                        ret = db.ParamsInsertScalar(sql, Program.idkadar, IdDokView);
                    }
                    sql = " update NalogKooperantaStavke set Uuser= @param0 where Id_DokumentaView = @param1";
                    ret = db.ParamsInsertScalar(sql, Program.idkadar, IdDokView);

                    break;
                case "NalogZaRazduzenjeAmbalaze":
                    sql = " Update NalogKooperantaStavke set UvecanjeCene=rs.UvecanjeCene "
                        + " from NalogZaRazduzenjeAmbalazeTotali as rs, NalogKooperantaStavke as ns "
                        + " where rs.ID_Proizvodi=ns.ID_SirovinaView and ns.id_DokumentaView=rs.ID_Nalog and rs.ID_NalogZaRazduzenjeAmbalazeTotali=@param0 ";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "RadniNalog2":
                    sql = " update NalogKooperantaStavke set ProsecnaNabavnaCena=c.ProdajnaCena "
                        + " from Cenovnik as c, NalogKooperanta as n,NalogKooperantaStavke as ns "
                        + " where n.id_DokumentaView=ns.ID_DokumentaView and  c.ID_ArtikliView=ns.ID_SirovinaView "
                        + " and c.ID_Skladiste=n.ID_SkladisteU and ns.ProsecnaNabavnaCena=0 and n.ID_DokumentaView=@param0 ";
                    ret = db.ParamsInsertScalar(sql, IdDokView);
                    //DoradiPotrosnju(IdDokView, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "datum").Vrednost);
                    break;
                case "RazduzenjeBezNormativa":
                    sql = "delete from NalogKooperantaSirovineStavke where ID_SirovinaView=1 and id_Dokumentaview=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);
                    sql = "delete from NalogKooperantaStavke where ID_SirovinaView=1 and id_Dokumentaview=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    sql = " update NalogKooperantaSirovineStavke set ProsecnaNabavnaCena=c.ProsecnaNabavnaCena "
                        + " from Cenovnik as c, NalogKooperanta as n,NalogKooperantaSirovineStavke as ns "
                        + " where n.id_DokumentaView=ns.ID_DokumentaView and  c.ID_ArtikliView=ns.ID_SirovinaView and "
                        + " c.ID_Skladiste=n.ID_SkladisteIz and ns.ProsecnaNabavnaCena=0 and n.ID_DokumentaView=@param0 ";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "DostavnicaUMaloprodaju":
                case "PDVDostavnicaUMaloprodaju":
                case "LotPDVDostavnicaUMaloprodaju":
                    sql = " update RacunStavke set ProdajnaCena=c.ProdajnaCena "
                        + " from Cenovnik as c,Racun as r, RacunStavke as rs  "
                        + " where c.ID_ArtikliView=rs.ID_ArtikliView and rs.id_DokumentaView=r.ID_DokumentaView and c.ID_Skladiste=r.ID_Skladiste2 "
                        + " and rs.ProdajnaCena=0 and r.ID_DokumentaView=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    sql = "select OpisSkladista from skladiste where ID_Skladiste=@param0";
                    DataTable dt3 = db.ParamsQueryDT(sql, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").ID);
                    if (dt3.Rows.Count != 0)
                    {
                        SkladisteJe = dt3.Rows[0]["OpisSkladista"].ToString().Trim();
                    }
                    else break;
                    if (SkladisteJe == "Proizvoda")
                    {
                        sql = "update RacunStavke set ProsecnaNabavnaCena=c.PlanskaCena,NabavnaCena=c.PlanskaCena "
                        + " from Cenovnik as c,Racun as r, RacunStavke as rs  "
                        + " where c.ID_ArtikliView=rs.ID_ArtikliView and rs.id_DokumentaView=r.ID_DokumentaView and c.ID_Skladiste=r.ID_Skladiste "
                        + " and r.ID_DokumentaView=@param0";
                        ret = db.ParamsInsertScalar(sql, IdDokView);
                    }
                    else
                    {
                        sql = "update RacunStavke set ProsecnaNabavnaCena=c.ProsecnaNabavnaCena ,NabavnaCena=c.ProsecnaNabavnaCena "
                            + " from Cenovnik as c,Racun as r, RacunStavke as rs "
                            + " where c.ID_ArtikliView=rs.ID_ArtikliView and rs.id_DokumentaView=r.ID_DokumentaView and c.ID_Skladiste=r.ID_Skladiste "
                            + " and rs.ProsecnaNabavnaCena=0 and r.ID_DokumentaView=@param0";
                        ret = db.ParamsInsertScalar(sql, IdDokView);
                    }
                    break;
                case "Trebovanje":
                    sql = " update TrebovanjeStavke set ProdajnaCena=c.ProsecnaNabavnaCena "
                        + " from Cenovnik as c,Trebovanje as r, TrebovanjeStavke as rs "
                        + " where c.ID_ArtikliView=rs.ID_ArtikliView and rs.id_DokumentaView=r.ID_DokumentaView and c.ID_Skladiste=r.ID_Skladiste "
                        + " and rs.ProdajnaCena=0 and r.ID_DokumentaView=@param0 ";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "PrometMaloprodaje":
                case "PDVPrometMaloprodaje":
                    sql = " update PrometMaloprodajeStavke set StvarnaProdajnaCena=c.ProdajnaCena "
                        + " from Cenovnik as c,PrometMaloprodaje as r, PrometMaloprodajeStavke as rs "
                        + " where c.ID_ArtikliView=rs.ID_ArtikliView and rs.id_DokumentaView=r.ID_DokumentaView and c.ID_Skladiste=r.ID_Skladiste "
                        + " and rs.StvarnaProdajnaCena=0 and r.ID_DokumentaView=@param0 ";
                    // uslov rs.StvarnaProdajnaCena = 0 je zbog mogucnosti da se svesno unese stvarnaprodajnacena  razlicita od prodajne cene
                    ret = db.ParamsInsertScalar(sql, IdDokView);
                    sql = " update PrometMaloprodajeStavke set ProdajnaCena=rs.StvarnaProdajnaCena "
                        + " from  PrometMaloprodajeStavke as rs "
                        + " where rs.ProdajnaCena=0 and rs.ID_DokumentaView= @param0 ";
                    ret = db.ParamsInsertScalar(sql, IdDokView);
                    // popunjavanje planske cene za sve  proizvode  koji se vode po planskim cenama
                    sql = " update PrometMaloprodajeStavke set PlanskaCena=c.PlanskaCena "
                      + " from Cenovnik as c,PrometMaloprodaje as r, PrometMaloprodajeStavke as rs "
                      + " where c.ID_ArtikliView=rs.ID_ArtikliView and rs.id_DokumentaView=r.ID_DokumentaView "
                      + " and c.ID_Skladiste=r.ID_Skladiste and r.ID_DokumentaView=@param0 ";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    sql = "delete from PrometMaloprodajePlacanjeStavke where ID_NacinPl=1 and id_Dokumentaview=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokView);
                    if (Program.imeFirme == "Leotar")
                    {
                        db.ExecuteStoreProcedure("AzurirajCenuSirovinaZaPromet", "IdDokView:" + IdDokView);
                    }

                    break;
                case "Izvod":
                    if (forma.Controls["OOperacija"].Text == "UNOS" || forma.Controls["OOperacija"].Text == "IZMENA")
                    {
                        if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "PozivNaBroj").Vrednost.Contains("123-") == true)
                        {
                            clsMesecPoreza cmp = new clsMesecPoreza();
                            int MesecPoreza = cmp.ObradiMesecPoreza(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost);
                            sql = "update Dokumenta set MesecPoreza=@param0 where id_Dokumenta= @param1";
                            ret = db.ParamsInsertScalar(sql, MesecPoreza.ToString(), IdDokView);
                            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "PozivNaBroj").ID);
                        }
                    }

                    break;
                case "UgovorOOtkupu":
                    sql = " delete from UgovorStavke where ID_ArtikliView=1 AND ID_ArtikliViewIzlaz=1 and ID_DokumentaView=@param0 ";
                    ret = db.ParamsInsertScalar(sql, IdDokView);
                    sql = " Update ugovor set ID_UgovorOOtkupuView=@param0 "
                        + " from Ugovor "
                        + " where ID_DokumentaView=@param1 ";
                    ret = db.ParamsInsertScalar(sql, IdDokView, IdDokView);

                    break;
                case "UgovorOOtkupuAneks":
                    if (forma.Controls["OOperacija"].Text == "UNOS" || forma.Controls["OOperacija"].Text == "IZMENA")
                    {
                        sql = " update Ugovor set id_KomitentiView = UgovorOOtkupuAneksView.id_KomitentiView "
                            + " from UgovorOOtkupuAneksView "
                            + " where Ugovor.ID_DokumentaView=UgovorOOtkupuAneksView.ID_UgovorOOtkupuAneksView and UgovorOOtkupuAneksView.ID_UgovorOOtkupuAneksView=@param0 ";
                        ret = db.ParamsInsertScalar(sql, IdDokView);
                    }

                    break;
                case "PrometRecepcije":
                    sql = "delete from PrometRecepcijeStavke where ID_ArtikliView=1 and ID_DokumentaView=@param0 ";
                    ret = db.ParamsInsertScalar(sql, IdDokView);
                    sql = "delete from PrometRecepcijePlacanjeStavke where ID_Nacinpl=1 and ID_DokumentaView=@param0 ";
                    ret = db.ParamsInsertScalar(sql, IdDokView);

                    break;
                case "PDVPredracun":
                    if (Program.imeFirme == "Bankom")
                    {
                        sql = " Update PredracunStavke set Trosak= PredracunStavke.Kolicina * Predracun.Prevoz /"
                            + " ( select SUM(PredracunStavke.Kolicina) from PredracunStavke where PredracunStavke.ID_DokumentaView =Predracun.ID_DokumentaView) "
                            + " from Predracun,PredracunStavke "
                            + " where PredracunStavke.ID_DokumentaView =Predracun.ID_DokumentaView and PredracunStavke.ID_DokumentaView=@param0 ";
                        ret = db.ParamsInsertScalar(sql, IdDokView);
                    }

                    break;
                default:
                    break;
            }

            if (Dokument == "InternaPrijemnica" || Dokument == "UlazniRacun" || Dokument == "PDVInternaDostavnica" || Dokument == "LotPDVInternaDostavnica"
                || Dokument == "PocetnoStanjeZaRobu" || Dokument == "LotPocetnoStanjeMagacin" || Dokument == "InternaDostavnica")
            {
                sql = " update RacunStavke set Primljeno=kolicina"
                    + " from RacunStavke as rs "
                    + " where Primljeno=0 and rs.ID_DokumentaView=@param0 ";
                ret = db.ParamsInsertScalar(sql, IdDokView);

                sql = " update RacunStavke set NabavnaCena=c.ProsecnaNabavnaCena "
                   + " from Cenovnik as c,Racun as r, RacunStavke as rs "
                   + " where c.ID_ArtikliView=rs.ID_ArtikliView and rs.id_DokumentaView=r.ID_DokumentaView "
                   + " and c.ID_Skladiste=r.ID_Skladiste and rs.NabavnaCena = 0 and r.ID_DokumentaView=@param0";
                ret = db.ParamsInsertScalar(sql, IdDokView);

                sql = " update RacunStavke set ProdajnaCena=c.ProsecnaNabavnaCena "
                    + " from Cenovnik as c,Racun as r, RacunStavke as rs "
                    + " where c.ID_ArtikliView=rs.ID_ArtikliView and rs.id_DokumentaView=r.ID_DokumentaView "
                    + " and c.ID_Skladiste=r.ID_Skladiste and rs.ProdajnaCena = 0 and r.ID_DokumentaView=@param0";
                ret = db.ParamsInsertScalar(sql, IdDokView);
                // azuriranje prosecne nabavne cene ulaznih dokumenata
                sql = "select OpisSkladista from skladiste where ID_Skladiste=@param0";
                DataTable dt3 = db.ParamsQueryDT(sql, forma.Controls.OfType<Field>().FirstOrDefault(n => n.cPolje == "NazivSkl").ID);
                if (dt3.Rows.Count != 0)
                {
                    SkladisteJe = dt3.Rows[0]["OpisSkladista"].ToString().Trim();
                }
                switch (SkladisteJe)
                {
                    case "Tranzit":
                    case "Reexport":
                        // odnosi se na promenu podataka u ulaznom racunu koji je predhodnik izlaznog racuna u tranzitu
                        AzurirajProsecneCeneTranzita(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrDok").Vrednost);

                        break;
                    default: //ostala skladista
                        if (Dokument == "KonacniUlazniRacun" || Dokument == "UlazniRacun" || Dokument == "PocetnoStanjeZaRobu")
                        {
                            sql = " update RacunStavke set ProsecnaNabavnaCena=NabavnaCena "
                                + " where  ProsecnaNabavnaCena = 0 and id_DokumentaView = @param0";
                            ret = db.ParamsInsertScalar(sql, IdDokView);
                        }
                        else //interna prijemnica ,pdvinternsdostavnica
                        {
                            sql = " update RacunStavke set ProsecnaNabavnaCena=ProdajnaCena,NabavnaCena=ProdajnaCena "
                                + " where id_DokumentaView=@param0";
                            ret = db.ParamsInsertScalar(sql, IdDokView);
                        }
                        break;
                }
            }
            // Azuriranje prosecne nabavne cene dokumenata izlaza iz skladista
            if (Dokument == "InternaDostavnica" || Dokument == "Racun" || Dokument == "InoRacun" || Dokument == "InterniNalogZaRobu"
               || Dokument == "Predracun" || Dokument == "KonacniRacun" || Dokument == "PDVPredracun" || Dokument == "PDVInterniNalogZaRobu"
               || Dokument == "PDVInternaDostavnica" || Dokument == "NalogZaIzvoz" || Dokument == "InternaDostavnica" || Dokument == "InoPredracun"
               || Dokument == "LotInterniNalogZaRobu" || Dokument == "LotPDVInterniNalogZaRobu" || Dokument == "LotPDVInternaDostavnica")
            {
                switch (Dokument)
                {
                    case "Predracun":
                    case "PDVPredracun":
                    case "InoPredracun":
                        sql = " update PredRacunStavke set ProsecnaNabCena=c.ProsecnaNabavnaCena  "
                            + " from  Cenovnik as c,PredRacunStavke as rs, PredRacun as R "
                            + " where c.ID_Skladiste = r.ID_Skladiste and c.ID_ArtikliView=rs.ID_ArtikliView "
                            + " and r.ID_DokumentaView=rs.ID_DokumentaView  and r.id_DokumentaView=@param0";
                        ret = db.ParamsInsertScalar(sql, IdDokView);

                        break;
                    case "InterniNalogZaRobu":
                    case "PDVInterniNalogZaRobu":
                    case "LotInterniNalogZaRobu":
                    case "LotPDVInterniNalogZaRobu":
                        sql = "select c.ProsecnaNabavnaCena as pnc,c.PlanskaCena as plc "
                            + " from Cenovnik as c,InterniNalogZaRobuStavke as rs, InterniNalogZaRobu as R "
                            + " where c.ID_Skladiste = r.ID_Skladiste and c.ID_ArtikliView=rs.ID_ArtikliView "
                            + " and r.ID_DokumentaView=rs.ID_DokumentaView and r.ID_DokumentaView=@param0 ";
                        DataTable dt3 = db.ParamsQueryDT(sql, IdDokView);
                        if (dt3.Rows.Count != 0)
                        {
                            sql = "select OpisSkladista from skladiste where ID_Skladiste=@param0";
                            DataTable dt3a = db.ParamsQueryDT(sql, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").ID);
                            if (dt3a.Rows.Count != 0)
                            {
                                SkladisteJe = dt3a.Rows[0]["OpisSkladista"].ToString().Trim();
                            }

                            if (SkladisteJe== "Proizvoda")
                            {
                                if(Convert.ToDouble(dt3.Rows[0]["plc"]) == 0)
                                {
                                    sql = " update InterniNalogZaRobuStavke set JedinicnaCena=c.PlanskaCena "
                                        + " from Cenovnik as c,InterniNalogZaRobuStavke as rs, InterniNalogZaRobu as R "
                                        + "where c.ID_Skladiste = r.ID_Skladiste and c.ID_ArtikliView=rs.ID_ArtikliView "
                                        + " and r.ID_DokumentaView=rs.ID_DokumentaView  and r.id_DokumentaView=@param0";
                                    ret = db.ParamsInsertScalar(sql, IdDokView);
                                }
                            }
                            else
                            {
                                if (Dokument.Contains("PDV") == true)
                                {
                                    sql = " update InterniNalogZaRobuStavke set JedinicnaCena=c.ProsecnaNabavnaCena,ProdajnaCena=c.ProsecnaNabavnaCena "
                                       + " from Cenovnik as c,InterniNalogZaRobuStavke as rs, InterniNalogZaRobu as R "
                                       + "where c.ID_Skladiste = r.ID_Skladiste and c.ID_ArtikliView=rs.ID_ArtikliView "
                                       + " and r.ID_DokumentaView=rs.ID_DokumentaView and rs.ProdajnaCena=0 and r.id_DokumentaView=@param0";
                                    ret = db.ParamsInsertScalar(sql, IdDokView);

                                    if (forma.Controls["OOperacija"].Text == "UNOS")
                                    {
                                        sql = "update InterniNalogZaRobuStavke set JedinicnaCena=ProdajnaCena "
                                            + " from InterniNalogZaRobuStavke as rs "
                                            + "where rs.id_DokumentaView=@param0";
                                        ret = db.ParamsInsertScalar(sql, IdDokView);
                                    }
                                }
                                else
                                {
                                    if ( Program.NazivBaze == "dbbbTestNew2003Bankom")
                                    {
                                        sql = " update InterniNalogZaRobuStavke set JedinicnaCena=c.ProsecnaNabavnaCena,ProdajnaCena=c.ProsecnaNabavnaCena "
                                            + " from Cenovnik as c,InterniNalogZaRobuStavke as rs, InterniNalogZaRobu as r "
                                            + "where c.ID_Skladiste = r.ID_Skladiste and c.ID_ArtikliView=rs.ID_ArtikliView "
                                            + " and r.ID_DokumentaView=rs.ID_DokumentaView and rs.JedinicnaCena=0 and r.id_DokumentaView=@param0";
                                        ret = db.ParamsInsertScalar(sql, IdDokView);
                                    }
                                    else
                                    {
                                        sql = " update InterniNalogZaRobuStavke set JedinicnaCena=c.ProsecnaNabavnaCena "
                                           + " from Cenovnik as c,InterniNalogZaRobuStavke as rs, InterniNalogZaRobu as r "
                                           + "where c.ID_Skladiste = r.ID_Skladiste and c.ID_ArtikliView=rs.ID_ArtikliView "
                                           + " and r.ID_DokumentaView=rs.ID_DokumentaView and rs.JedinicnaCena=0 and r.id_DokumentaView=@param0";
                                        ret = db.ParamsInsertScalar(sql, IdDokView);
                                    }
                                }
                            }
                        }

                            break;
                    default: // Racun,KonacniRacun,InoRacun,InternaDostavnica,PDVInternaDostavnica, LotPDVInternaDostavnica
                        sql = " update RacunStavke set PDV=at.pdv "
                            + " from RacunStavke as rs, KonacniRacunStavkeView as at "
                            + "where rs.ID_DokumentaView=at.ID_KonacniRacunStavkeView and rs.ID_ArtikliView=at.ID_ArtikliView "
                            + " and rs.id_DokumentaView=@param0";
                        ret = db.ParamsInsertScalar(sql, IdDokView);

                        sql = "select OpisSkladista from skladiste where ID_Skladiste=@param0";
                        DataTable dt4 = db.ParamsQueryDT(sql, forma.Controls.OfType<Field>().FirstOrDefault(n => n.cPolje == "NazivSkl").ID);
                        if (dt4.Rows.Count != 0)
                        {
                            SkladisteJe = dt4.Rows[0]["OpisSkladista"].ToString().Trim();
                        }

                        switch (SkladisteJe)
                        {
                            case "Tranzit":
                            case "Reexport":
                                //VezaVrsteTroskaiTroska
                                sql = " select  r.ID_ArtikliView, (CASE sum(r.NabavnaCena*r.kolicina) WHEN 0 THEN 0 ELSE sum(r.NabavnaCena*r.kolicina)/sum(r.kolicina) END) as pc "
                                    + " from RacunStavke as r, ArtikliStavkeView as a, Dokumenta as d, dokumenta as SviPred, dokumenta as Pred "
                                    + " where svipred.RedniBroj=Pred.RedniBroj and svipred.id_DokumentaStablo=Pred.ID_Dokumentastablo "
                                    + " and year(svipred.datum) = year (pred.datum) and r.ID_ArtikliView=a.ID_ArtikliStavkeView "
                                    + " and a.ID_ArtikliStavkeView not in (select ID_Trosak from  VezaVrsteTroskaiTroska) "
                                    + " and r.id_Dokumentaview=svipred.ID_Dokumenta and d.id_Predhodni=pred.ID_Dokumenta "
                                    + " and d.ID_Dokumenta=@param0 "
                                    + " group by  r.ID_ArtikliView ";
                                DataTable dt5 = db.ParamsQueryDT(sql, IdDokView);
                                foreach (DataRow row in dt5.Rows)
                                {
                                    if (row["pc"].ToString() != null)
                                    {
                                        sql = " update RacunStavke set ProsecnaNabavnaCena=@param0 "
                                            + " from racunstavke as r where r.id_dokumentaview=@param1 "
                                            + " and and r.id_artikliview=@param2";
                                        ret = db.ParamsInsertScalar(sql, row["pc"].ToString(),IdDokView, row["ID_ArtikliView"].ToString());
                                    }
                                }

                                if ( Dokument == "Racun" || Dokument == "KonacniRacun")
                                {
                                    sql = " update RacunStavke set ProdajnaCena=ProsecnaNabavnaCena "
                                            + " from Racun as r, RacunStavke as rs "
                                            + "where rs.id_DokumentaView=r.ID_DokumentaView  and rs.ProdajnaCena = 0 and r.ID_DokumentaView=@param0";
                                    ret = db.ParamsInsertScalar(sql,IdDokView);
                                }
                                //prosecna nabavna cena za troskove prevoza
                                sql = " update RacunStavke set ProsecnaNabavnaCena=ProdajnaCena "
                                    + " from racunstavke as r,artiklistavkeview as a "
                                    + " where r.id_artikliview=a.id_artiklistavkeview  and (a.ID_ArtikliStavkeView  in (select ID_Trosak from  VezaVrsteTroskaiTroska)) "
                                    + " and r.ID_DokumentaView=@param0";
                                ret = db.ParamsInsertScalar(sql, IdDokView);

                                break;
                            default: //obicno skladiste
                                sql = " update RacunStavke set ProsecnaNabavnaCena=c.ProsecnaNabavnaCena  "
                                  + " from Cenovnik as c, RacunStavke as rs, Racun as R,KonacniRacunTotali as KR "
                                  + " where c.ID_Skladiste = r.ID_Skladiste and c.ID_ArtikliView=rs.ID_ArtikliView "
                                  + " and (rs.ProsecnaNabavnaCena=0 or (rs.id_artikliview<>kr.id_artikliview and  rs.id_racunstavke=KR.iid and  r.ID_DokumentaView = KR.ID_KonacniRacunTotali)) "
                                  + " and r.ID_DokumentaView = rs.ID_DokumentaView and r.ID_DokumentaView=@param0";
                                ret = db.ParamsInsertScalar(sql, IdDokView);

                                if (Dokument == "InternaDostavnica" || Dokument == "PDVInternaDostavnica" || Dokument == "LotPDVInternaDostavnica")
                                {
                                    sql = "update RacunStavke set ProdajnaCena=ProsecnaNabavnaCena, NabavnaCena=ProsecnaNabavnaCena "
                                        + " from RacunStavke as rs "
                                        + " where rs.ID_DokumentaView =@param0";
                                    ret = db.ParamsInsertScalar(sql, IdDokView);
                                }

                                break;
                        }

                        break;
                }
            }
            //povlacenje avansa za dokumenta koja imaju to polje
            if (Dokument == "KonacniRacun" || Dokument == "PDVIzlazniRacunZaUsluge" || Dokument == "KonacniUlazniRacun"
                || Dokument == "PDVUlazniRacunZaUsluge" || Dokument == "KonacniRacunZaHotel")
            {
                sql = " select Proknjizeno, Opis from dokumentatotali WITH(NOLOCK)"
                    + " where id_dokumentatotali = @param0 ";
                DataTable dt6 = db.ParamsQueryDT(sql, IdDokView);

                if (dt6.Rows.Count != 0)
                {
                    if (dt6.Rows[0]["Proknjizeno"].ToString() != "Proknjizen")
                    {
                        clsAvansi avans = new clsAvansi();
                        avans.PovlacenjeAvansa (forma,Dokument, Convert.ToInt32(IdDokView));
                    }
                }
            }

            if (Dokument == "LimitDugovanja")
            {
                if (forma.Controls["OOperacija"].Text == "UNOS" || forma.Controls["OOperacija"].Text == "IZMENA")
                {
                    sql = " select ID_KomitentiView, Limit from LimitDugovanjaStavke Where ID_DokumentaView =@param0 ";
                    DataTable dt7 = db.ParamsQueryDT(sql, IdDokView);
                    foreach (DataRow row in dt7.Rows)
                    {
                        sql = "Update komitenti set limitdugovanja=@param0 "
                            + " where ID_Komitenti=@param1";
                        ret = db.ParamsInsertScalar(sql, row["Limit"].ToString(), row["ID_KomitentiView"].ToString());
                        db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Komitenti", "IdDokument:" + row["ID_KomitentiView"].ToString());
                    }
                }
            }

            DodatnaAzuriranjaPosleUnosa = true;
            return DodatnaAzuriranjaPosleUnosa;
        }
        private bool UvecanjeVrednostiZaZavisneTroskove(string IdDokView)
        {
            bool UvecanjeVrednostiZaZavisneTroskove = false;
            double KursiranaProdajnaCena = 0;
            forma = new Form();
            forma = Program.Parent.ActiveMdiChild;

            sql = "select r.ID_ArtikliView as ID_Trosak, r.ProdajnaCena, a.JedinicaMere from RacunStavke as r, ArtikliStavkeView as a "
                        + " where r.ID_ArtikliView=a.ID_ArtikliStavkeView and (a.JedinicaMere like 'Kolicina' or a.JedinicaMere like 'Vrednost' or a.JedinicaMere like 'UkupanBroj') "
                        + " and r.ID_DokumentaView=@param0";
            DataTable dt = db.ParamsQueryDT(sql, IdDokView);
            foreach (DataRow row in dt.Rows)
            {
                sKurs = 1;
                string kojavaluta = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "OznVal").Vrednost;
                if (kojavaluta != "")
                {
                    clsProveraIspravnosti cPI = new clsProveraIspravnosti();
                    Vrati = cPI.PostojiKurs(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "DatumCarinjenja").Vrednost, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "OznVal").ID, kojavaluta,ref sKurs, ref Poruka);
                    if (Vrati == true)
                        KursiranaProdajnaCena = Convert.ToDouble(row["ProdajnaCena"]) * sKurs;
                }
                switch (row["JedinicaMere"])
                {
                    case "Kolicina":
                        sql = "select sum(r.Kolicina) as k from RacunStavke as r, ArtikliStavkeView as a "
                            + " where r.ID_ArtikliView=a.ID_ArtikliStavkeView  and a.JedinicaMere not like 'Kolicina' "
                            + " and a.JedinicaMere not like 'Vrednost' and  a.JedinicaMere not like 'UkupanBroj' "
                            + " and r.ID_DokumentaView=@param0";
                        DataTable dt1 = db.ParamsQueryDT(sql, IdDokView);
                        if (dt1.Rows[0]["k"].ToString() != null)
                        {
                            sql = " Update RacunStavke set NabavnaCena= r.NabavnaCena+(@param0 *r.Kolicina/@param1)/r.Kolicina "
                                + " from RacunStavke as r, ArtikliStavkeView as a "
                                + " where r.ID_ArtikliView=a.ID_ArtikliStavkeView and "
                                + " a.JedinicaMere not like 'Kolicina' and a.JedinicaMere not like 'Vrednost' and  a.JedinicaMere not like 'UkupanBroj'  "
                                + " and r.ID_DokumentaView=@param2";
                            ret = db.ParamsInsertScalar(sql, KursiranaProdajnaCena, dt1.Rows[0]["k"].ToString(), IdDokView);
                        }
                        break;
                    case "Vrednost":
                        {
                            sql = "select sum(r.Kolicina*r.NabavnaCena) as k from RacunStavke as r, ArtikliStavkeView as a "
                                + " where r.ID_ArtikliView=a.ID_ArtikliStavkeView  and a.JedinicaMere not like 'Kolicina' "
                                + " and a.JedinicaMere not like 'Vrednost' and  a.JedinicaMere not like 'UkupanBroj'   "
                                + " and r.ID_DokumentaView=@param0";
                            DataTable dt2 = db.ParamsQueryDT(sql, IdDokView);
                            if (dt2.Rows[0]["k"].ToString() != null)
                            {
                                sql = " Update RacunStavke set NabavnaCena= r.NabavnaCena+(@param0 *r.Kolicina*r.NabavnaCena/@param1)/r.Kolicina "
                                    + " from RacunStavke as r, ArtikliStavkeView as a "
                                    + " where r.ID_ArtikliView=a.ID_ArtikliStavkeView and "
                                    + " a.JedinicaMere not like 'Kolicina' and a.JedinicaMere not like 'Vrednost' and  a.JedinicaMere not like 'UkupanBroj'  "
                                    + " and r.ID_DokumentaView=@param2";
                                ret = db.ParamsInsertScalar(sql, KursiranaProdajnaCena, dt2.Rows[0]["k"].ToString(), IdDokView);
                            }
                        }
                        break;
                    case "UkupanBroj":
                        {
                            sql = "select count(*) as k from RacunStavke as r, ArtikliStavkeView as a "
                                + " where r.ID_ArtikliView=a.ID_ArtikliStavkeView  and a.JedinicaMere not like 'Kolicina' "
                                + " and a.JedinicaMere not like 'Vrednost' and  a.JedinicaMere not like 'UkupanBroj'   "
                                + " and r.ID_DokumentaView=@param0";
                            DataTable dt3 = db.ParamsQueryDT(sql, IdDokView);
                            if (dt3.Rows[0]["k"].ToString() != null)
                            {
                                sql = " Update RacunStavke set NabavnaCena= r.NabavnaCena+(@param0 *r.Kolicina/@param1)/r.Kolicina "
                                    + " from RacunStavke as r, ArtikliStavkeView as a "
                                    + " where r.ID_ArtikliView=a.ID_ArtikliStavkeView and "
                                    + " a.JedinicaMere not like 'Kolicina' and a.JedinicaMere not like 'Vrednost' and  a.JedinicaMere not like 'UkupanBroj'  "
                                    + " and r.ID_DokumentaView=@param2";
                                ret = db.ParamsInsertScalar(sql, KursiranaProdajnaCena, dt3.Rows[0]["k"].ToString(), IdDokView);
                            }
                        }
                        break;
                    default:
                        break;
                }
                sql = " Update RacunStavke set NabavnaCena=0 "
                    + " from RacunStavke as r "
                    + " where r.ID_ArtikliView=@param0 and r.ID_DokumentaView=@param1";

                ret = db.ParamsInsertScalar(sql, row["JedinicaMere"].ToString(), IdDokView);
            }

            UvecanjeVrednostiZaZavisneTroskove = true;
            return UvecanjeVrednostiZaZavisneTroskove;
        }

        private bool ObracunProizvodnje(string datum, string id_Skladiste)
        {
            bool ObracunProizvodnje = false;
            string datumOd = "01.01." + Convert.ToDateTime(datum).Year;
            string ID_Artikli = "1";
            int prolaz = 0;
            double Stanje = 0;
            double SaldoIzPrometa = 0;
            double PoslednjaCena = 0;
            double PlanskaCena = 0;

            sql = "select ID_CeneArtikalaNaSkladistimaPred, Skl, NazivSkl, ID_ArtikliView, datum, SN, Brdok, ulaz, izlaz, VrednostNab, ProsecnaNabavnaCena "
                    + " from ceneartikalanaskladistimapred as c "
                    + " where datum <= @param0 and datum >=@param1 and Skl=@param3 "
                    + " order by NazivSkl,ID_ArtikliView,datum,SN,Brdok ";
            DataTable dt = db.ParamsQueryDT(sql, datum, datumOd, Program.idFirme, id_Skladiste);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("");
                return ObracunProizvodnje ;
            }
            else
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (prolaz == 0) ID_Artikli = row["ID_ArtikliView"].ToString();

                    if (ID_Artikli!= row["ID_ArtikliView"].ToString())
                    {
                        if (Stanje < 0)
                        {
                            sql = " Select planskacena from cenovnik where ID_ArtikliView=@param0 And ID_Skladiste=@param1";
                            DataTable dtts = db.ParamsQueryDT(sql, IdDokView);
                        }
                        sql = "Insert into InterniNalogZaRobuStavke(ID_DokumentaView, ID_ArtikliView, KolicinaUlaz, JedinicnaCena ,KolicinaIzlaz) )"
                        + " values(@param0, @param1,@param2,@param3)";
                        DataTable dtt = db.ParamsQueryDT(sql, ((Bankom.frmChield)forma).iddokumenta, ID_Artikli, PoslednjaCena);

                    }
                        if (Convert.ToDouble(row["ulaz"]) == 0 && Convert.ToDouble(row["izlaz"]) == 0)
                        {
                            Stanje += Convert.ToDouble(row["ulaz"]) - Convert.ToDouble(row["izlaz"]);
                            SaldoIzPrometa += Convert.ToDouble(row["VrednostNab"]);
                            PoslednjaCena = Convert.ToDouble(row["ProsecnaNabavnaCena"]);
                        }
                  

                    ID_Artikli = row["ID_ArtikliView"].ToString();
                    prolaz += 1;
                }
            }
            sql = "Insert into InterniNalogZaRobuStavke(ID_DokumentaView, ID_ArtikliView, KolicinaUlaz, JedinicnaCena ,KolicinaIzlaz) )"
                          + " values(@param0, @param1,@param2,@param3)";
            DataTable dts = db.ParamsQueryDT(sql, ((Bankom.frmChield)forma).iddokumenta, ID_Artikli,  PoslednjaCena);

            ObracunProizvodnje = true;
            return ObracunProizvodnje;
        }
        private bool AzurirajProsecneCeneTranzita(string BrDok)
        {
            bool AzurirajProsecneCeneTranzita = false;

            BrDok = ReplaceFirstOccurrence(BrDok,"-", " ");
            BrDok = ReplaceFirstOccurrence(BrDok, "-", " ");
            BrDok = BrDok.Substring(1, BrDok.IndexOf("-"));

            sql = "select  id_Dokumentatotali ,Dokument  from Dokumentatotali as d "
                + " where  (d.Dokument='KonacniRacun' or d.Dokument='InoRacun') "
                + " and d.predhodni like @param0 ";
            DataTable dt5 = db.ParamsQueryDT(sql, BrDok);
            foreach (DataRow row in dt5.Rows)
            {
                sql = "select  r.ID_ArtikliView, (CASE sum(r.NabavnaCena*r.kolicina) WHEN 0 THEN 0 ELSE sum(r.NabavnaCena*r.kolicina)/sum(r.kolicina) END) as pc "
                    + " from  RacunStavke as r, ArtikliStavkeView as a, Dokumenta as d, dokumenta as SviPred, dokumenta as Pred "
                    + " where  svipred.RedniBroj=Pred.RedniBroj and svipred.id_DokumentaStablo=Pred.ID_Dokumentastablo and year(svipred.datum)=year(pred.datum) "
                    + " and r.ID_ArtikliView=a.ID_ArtikliStavkeView and a.JedinicaMere not like 'Kolicina' and a.JedinicaMere not like 'Vrednost' and  a.JedinicaMere not like 'UkupanBroj' "
                    + " and r.id_Dokumentaview=svipred.ID_Dokumenta and d.id_Predhodni=pred.ID_Dokumenta "
                    + " and d.ID_Dokumenta=@param0 "
                    + " group by  r.ID_ArtikliView ";
                DataTable dt6 = db.ParamsQueryDT(sql, row["id_Dokumentatotali"].ToString());
                foreach (DataRow row1 in dt6.Rows)
                {
                    if (row1["pc"].ToString() != null)
                    {
                        sql = " update RacunStavke set ProsecnaNabavnaCena=@param0 "
                            + " from racunstavke as r where r.id_dokumentaview=@param1 "
                            + " and and r.id_artikliview=@param2";
                        ret = db.ParamsInsertScalar(sql, row1["pc"].ToString(), row["id_Dokumentatotali"].ToString(), row1["ID_ArtikliView"].ToString());
                    }
                }
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + row["Dokument"].ToString(), "IdDokument:" + row["id_Dokumentatotali"].ToString());
                db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:" + row["id_Dokumentatotali"].ToString());
            }

            AzurirajProsecneCeneTranzita = true;
            return AzurirajProsecneCeneTranzita;
        }

        public static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
        {
            int Place = Source.IndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }
    }
}
