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
    class clsProveraIspravnosti
    {
        Boolean Vrati = true;
        string ddokument = "";
        string kojidatum = "";
        string sql = "";
        DataBaseBroker db = new DataBaseBroker();
        DataTable t = new DataTable();
        DataTable tt = new DataTable();
        Form forma = new Form();
        string Datum = "";
        string IDValuta = "1"; // ident valute iz sifarnikavaluta        
        string NazivKlona = "";
        int p = 0; //pozicija
        string s = "";// valuta  koju kursiramo
        double srednji = 1;
        string kojidd = ""; //datum po kojem se kursira
        string oznval = Program.DomacaValuta;
        string DokumentJe = "";
        string Operacija = "";
        clsOperacije co = new clsOperacije();

        public Boolean ProveraOperacija(string Dokument)
        {
            Vrati = true;
            string Nacinreg = "";
            string NazivDokumenta = "";
            string DaLiSeKnjizi = "";
            string BrojDokumenta = "";

            forma = Program.Parent.ActiveMdiChild;
            string DokumentJe = Convert.ToString(((Bankom.frmChield)forma).DokumentJe);
            Operacija = Convert.ToString(((Bankom.frmChield)forma).OOperacija.Text).ToUpper();
            //string Dokument = dokument; Convert.ToString(((Bankom.frmChield)forma).imedokumenta);
            string IdDokView = Convert.ToString(((Bankom.frmChield)forma).iddokumenta);
            string IdDokumentStablo = Convert.ToString(((Bankom.frmChield)forma).idstablo);
            DateTime DatumDokumenta = DateTime.Now;
            DateTime DatumUnosaDokumenta;
            string proknjizen = "";
            if (Operacija.Contains("UNOS") == true) return (Vrati);

            if (DokumentJe == "P") return (Vrati);
            
                sql = " select s.ulazniizlazni as NazivDokumenta,d.BrDok,NacinRegistracije as nr,"
                            + " d.Proknjizeno as p ,Knjizise,d.Datum,d.ttime "
                            + " from DokumentaTotali as d , DokumentaStablo as ds , SifarnikDokumenta as s "
                            + " where s.naziv=ds.Naziv and "
                            + " d.ID_DokumentaStablo = ds.ID_DokumentaStablo"
                            + " and d.ID_DokumentaTotali=@param0";// -+ IdDokView

                ///zapamtimo podatke sa odabranog dokumenta dokument
                t = db.ParamsQueryDT(sql, IdDokView);
                if (t.Rows.Count > 0)
                {
                    Nacinreg = t.Rows[0]["nr"].ToString();
                    NazivDokumenta = t.Rows[0]["NazivDokumenta"].ToString();
                    DaLiSeKnjizi = t.Rows[0]["Knjizise"].ToString();
                    BrojDokumenta = t.Rows[0]["BrDok"].ToString();
                    DatumDokumenta = Convert.ToDateTime(t.Rows[0]["Datum"].ToString());
                    DatumUnosaDokumenta = Convert.ToDateTime(t.Rows[0]["ttime"].ToString());
                    proknjizen = t.Rows[0]["p"].ToString();
                }
           
            else NazivDokumenta = Dokument;
            switch (Operacija)
            {
                case "IZMENA":
                    if (NazivDokumenta.Trim() == "")
                    {
                        Vrati = false;
                        MessageBox.Show("Dokument ne postoji u tabeli Dokumenta ne moze se menjati");
                        return (Vrati);
                    }

                    if (((Bankom.frmChield)forma).idReda == -1 && DokumentJe!="D")
                    {
                        MessageBox.Show("Niste odabrali red za izmenu !!! ");
                        Vrati = false;
                        break;
                    }
                    // provera da li se pokusava izmena storniranog dokumenta
                    if (BrojDokumenta.Contains("S") == true)
                    {
                        Vrati = false;
                        MessageBox.Show("Nije dozvoljena izmena storno dokumenta !");
                        break;
                    }
                    // provera da li se pokusava premestanje dokumenta iz godine u godinu
                    switch (Dokument)
                    {
                        case "Dokumenta":

                            if (DatumDokumenta.Year != Convert.ToDateTime(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost).Year)
                            {
                                Vrati = false;
                                MessageBox.Show("NIJE DOZVOLJENO PREMESTANJE dokumenta iz godine u godinu!!!!");
                                break;
                            }
                            break;
                        case "PDVUlazniRacunZaUsluge":
                        case "KonacniUlazniRacun":
                            // provera da li menjamo racun koji jee ukljucen u kalkulaciju
                            sql = " SELECT BrojDokumenta as BD,ID_KalkulacijaUfStavke as IDK  "
                               + " FROM KalkulacijaUfStavke as r WITH (NOLOCK), Dokumenta as d WITH (NOLOCK)"
                               + " WHERE d.ID_Dokumenta=r.ID_DokumentaView and "
                               + " r.ID_UlazniRacunZaUslugeCeo=@param0"
                               + " union "
                               + " SELECT BrojDokumenta as BD,ID_KalkulacijaUf as IDK  "
                               + " FROM KalkulacijaUf as r WITH (NOLOCK), Dokumenta as d WITH (NOLOCK)"
                               + " WHERE d.ID_Dokumenta=r.ID_DokumentaView and "
                               + " r.ID_UlazniRacunCeo=@param0";  /// + IdDokView.ToString();
                            tt = db.ParamsQueryDT(sql, IdDokView);
                            if (tt.Rows.Count > 0)
                            {
                                MessageBox.Show("Nije dozvoljena operacija ukljucen u kalkulaciju br:" + tt.Rows[0]["BD"].ToString());
                                Vrati = false;
                                break;
                            }
                            break;
                    } // KRAJ switch dokument         
                    break;
                case "BRISANJE":
                    if (((Bankom.frmChield)forma).idReda == -1)
                    {
                        MessageBox.Show("Niste odabrali red za brisanje !!! ");
                        Vrati = false;
                        break;
                    }
                    if (Dokument == "Dokumenta")
                    {
                        if (BrojDokumenta.Contains("S") == true)
                        {
                            Vrati = false;
                            MessageBox.Show("Nije Dozvoljeno brisanje storno dokumenta !");
                            break;
                        }

                        ///// provera da li se brise proknjizen dokument
                        if (proknjizen == "Proknjizen")
                        {
                            Vrati = false;
                            MessageBox.Show("Nije dozvoljeno brisanje dokument je proknjizen!!");
                            break;
                        }
                        // provera da li brisemo dokument iz predhodne godine ili arhivirani dokument
                        if (NazivDokumenta.Trim() == "")
                        {
                            Vrati = false;
                            MessageBox.Show("Dokument ne postoji u tabeli Dokumenta ne moze se brisati!!");
                            break;
                        }
                        // provera da li brisemo dokument koji je predhodnik drugog dokumenta
                        sql = "select BrojDokumenta from dokumenta WITH (NOLOCK) where  id_Predhodni=@param0"; // + IdDokView.ToString();
                        tt = db.ParamsQueryDT(sql, IdDokView);
                        if (tt.Rows.Count > 0)
                        {
                            MessageBox.Show("Nije dozvoljeno brisanje, dokument je predhodnik dokumentu: " + tt.Rows[0]["BrojDokumenta"]);
                            Vrati = false;
                            break;
                        }

                        string myDokument = "";
                        // Da li je normativ vec koriscen
                        sql = "Select BrDok,Dokument from DokumentaTotali Where ID_DokumentaTotali=@param0";  /////+ IdDokView.ToString();
                        t = db.ParamsQueryDT(sql, IdDokView);
                        if (t.Rows.Count > 0)
                        {
                            myDokument = t.Rows[0]["Dokument"].ToString();
                        }
                    }
                    switch (Dokument)
                    {
                        case "Dokumenta":

                            if (BrojDokumenta.Contains("S") == true)
                            {
                                Vrati = false;
                                MessageBox.Show("Nije Dozvoljeno brisanje storno dokumenta !");
                                break;
                            }

                            ///// provera da li se brise proknjizen dokument
                            if (proknjizen == "Proknjizen")
                            {
                                Vrati = false;
                                MessageBox.Show("Nije dozvoljena obrada dokument je proknjizen!!");
                                break;
                            }
                            // provera da li brisemo dokument iz predhodne godine ili arhivirani dokument
                            sql = "select BrojDokumenta from dokumenta WITH (NOLOCK) where  id_Dokumenta=@param0"; // + IdDokView.ToString();
                            t = db.ParamsQueryDT(sql, IdDokView);
                            if (t.Rows.Count == 0)
                            {
                                MessageBox.Show("Nije dozvoljeno brisanje, dokument arhiviran " );
                                Vrati = false;
                                break;
                            }
                            // provera da li brisemo dokument koji je predhodnik drugog dokumenta
                            sql = "select BrojDokumenta from dokumenta WITH (NOLOCK) where  id_Predhodni=@param0"; // + IdDokView.ToString();
                            t = db.ParamsQueryDT(sql, IdDokView);
                            if (t.Rows.Count > 0)
                            {
                                MessageBox.Show("Nije dozvoljeno brisanje, dokument je predhodnik dokumentu: " + BrojDokumenta);
                                Vrati = false;
                                break;
                            }                         
                        
                            break;
                        case "PDVUlazniRacunZaUsluge":
                        case "KonacniUlazniRacun":
                            // provera da li menjamo racun koji jee ukljucen u kalkulaciju
                            sql = " SELECT BrojDokumenta as BD,ID_KalkulacijaUfStavke as IDK  "
                               + " FROM KalkulacijaUfStavke as r WITH (NOLOCK), Dokumenta as d WITH (NOLOCK)"
                               + " WHERE d.ID_Dokumenta=r.ID_DokumentaView and "
                               + " r.ID_UlazniRacunZaUslugeCeo=@param0"
                               + " union "
                               + " SELECT BrojDokumenta as BD,ID_KalkulacijaUf as IDK  "
                               + " FROM KalkulacijaUf as r WITH (NOLOCK), Dokumenta as d WITH (NOLOCK)"
                               + " WHERE d.ID_Dokumenta=r.ID_DokumentaView and "
                               + " r.ID_UlazniRacunCeo=@param0";  /// + IdDokView.ToString();
                            t = db.ParamsQueryDT(sql, IdDokView);
                            if (t.Rows.Count > 0)
                            {
                                MessageBox.Show("Nije dozvoljena operacija ukljucen u kalkulaciju br:" + t.Rows[0]["BD"].ToString());
                                Vrati = false;
                                break;
                            }
                            break;
                        case "Normativ":
                            // Da li je normativ vec koriscen
                            string myDokument = "";
                            sql = "Select BrDok,Dokument from DokumentaTotali Where ID_DokumentaTotali=@param0";  /////+ IdDokView.ToString();
                            t = db.ParamsQueryDT(sql, IdDokView);
                            if (t.Rows.Count > 0)
                            {
                                myDokument = t.Rows[0]["Dokument"].ToString(); ///BORKA ?????????????????????????
                            }
                            break;
                        case "Rastavnica":
                            DataTable tt = new DataTable();
                            sql = "select * from NalogKooperanta where ID_NormativView= @param0"; //// + IdDokView.ToString();
                            tt = db.ParamsQueryDT(sql, IdDokView);
                            if (tt.Rows.Count > 0)
                            {
                                MessageBox.Show("Dokument je koriscen, ne moze se brisati!!!");
                                Vrati = false;
                                break;
                            }
                            break;
                        // da li brisemo prijemnicu ili otpremnicu ili narudzbenicu koja je vec koriscena
                        case "LotPrijemnica":
                        case "LotOtpremnica":
                        case "NarudzbenicaKupca":
                            if (Dokument == "LotPrijemnica" || Dokument == "LotOtpremnica")
                            {
                                sql = "select Otpremnica from Racun where Otpremnica like'%" + t.Rows[0]["BrDok"].ToString() + "%'";
                                if (Dokument == "NarudzbenicaKupca")
                                    sql = "select ID_Narudzbenica from Racun where ID_Narudzbenica=" + IdDokView.ToString();
                            }
                            tt = db.ReturnDataTable(sql);
                            if (tt.Rows.Count > 0)
                            {
                                MessageBox.Show("Dokument je koriscen, ne moze se brisati!!!");
                                Vrati = false;
                                break;
                            }
                            break;
                                                 
                        case "NalogGlavneKnjge":
                            // provera ako je NalogGlavneKnjige  da li je prazan
                            sql = "select * from NalogGlavneKnjigeStavke  where ID_DokumentaView=@param0"; ///+ IdDokView.ToString();
                            tt = db.ParamsQueryDT(sql, IdDokView);
                            if (tt.Rows.Count > 0)
                            {
                                MessageBox.Show("Nalog nije prazan, zabranjeno brisanje!!!");
                                Vrati = false;
                                break;
                            }
                            
                            break;

                    } // KRAJ SWITCH PO DOKUMENTU
                    break;
                case "STORNO":
                    if (((Bankom.frmChield)forma).idReda == -1)
                    {
                        MessageBox.Show("Niste odabrali dokument za storno !!! ");
                        Vrati = false;
                        break;
                    }
                    //provera da li se pokusava storno  storniranog dokumenta
                    if (BrojDokumenta.Contains("S") == true)
                    {
                        Vrati = false;
                        MessageBox.Show("Nije Dozvoljen storno storniranog dokumenta !");
                        break;
                    }
                    //provera da li je dokument  proknjizen
                    if (proknjizen != "Proknjizen" && IdDokumentStablo != "501" && IdDokumentStablo != "502" && IdDokumentStablo != "602")
                    {
                        Vrati = false;
                        MessageBox.Show("Dokument nije  proknjizen ne moze storno!!");
                        break;
                    }
                    break;
            } //KRAJ SWITCH OPERACIJA ZA OPERACIJU
            return (Vrati);
        }
        public Boolean ProveraObaveznihPolja(string Dokument) ///'PROVERA DA LI SU UNESENA OBAVEZNA POLJA
        {
            Vrati = true;
            forma = Program.Parent.ActiveMdiChild;
            string DokumentJe = Convert.ToString(((Bankom.frmChield)forma).DokumentJe);
            string Operacija = Convert.ToString(((Bankom.frmChield)forma).OOperacija.Text).ToUpper();
            NazivKlona = ddokument;
            int idreda = ((Bankom.frmChield)forma).idReda;
            string idvlasnik = "";
          
            if ((Operacija.Contains("UNOS") == true || Operacija.Contains("IZMENA") == true) && (DokumentJe != "K" && DokumentJe != "I"))
            {
                sql = "Select * from RecnikPodataka Where Dokument = @param0 AND  ObavezanUnos = 1 AND Tabindex >= 0 ORDER BY TUD, Tabindex";
                t = db.ParamsQueryDT(sql, Dokument);
                DataColumn column = new DataColumn();
                for (int rr=0;rr<t.Rows.Count; rr++)
                {
                        if (DokumentJe == "D" && Convert.ToInt32(t.Rows[rr]["TUD"]) > 0 && Operacija == "IZMENA")//izmena bez podignutog reda
                        {
                            if (Convert.ToUInt32(t.Rows[rr]["TUD"].ToString()) > 0)
                            {   
                                string tIme = t.Rows[rr]["TabelaVView"].ToString();
                                string ttIme = "GgRr" + tIme;
                                if (!(((Bankom.frmChield)forma).Controls.Find(ttIme, true).FirstOrDefault() is DataGridView dv))
                                {
                                    MessageBox.Show("na formi ne postoji tabela:" + tIme);
                                    Vrati = false;
                                    return Vrati;
                                }
                                idreda = Convert.ToInt32(dv.Tag);
                                if (idreda == -1)
                                {
                                    Vrati = true;
                                    return Vrati;
                                }
                            }
                        }
                        else
                        {
                            string polje = t.Rows[rr]["AlijasPolja"].ToString();
                            // forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost
                            string vrednostpolja = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == polje).Vrednost;
                            if (Convert.ToInt32(t.Rows[rr]["ID_TipoviPodataka"]) == 8 || Convert.ToUInt32(t.Rows[rr]["ID_TipoviPodataka"]) == 9)
                            {
                                Vrati = co.IsDateTime(vrednostpolja);
                                if (Vrati == false)
                                {
                                    MessageBox.Show("Neispravan  podatak za: " + polje);
                                    return Vrati;
                                }
                            }
                            if (co.IsNumeric(vrednostpolja) == true)
                            {
                                vrednostpolja = vrednostpolja.Replace(".", "").Replace(",", ".");
                                if (Convert.ToDouble(vrednostpolja) == 0)
                                {
                                    MessageBox.Show("Popunite podatak za: " + polje);
                                    Vrati = false;
                                    return (Vrati);
                                }
                            }
                            else
                            {
                                if (vrednostpolja.Trim() == "")
                                {
                                    ////'OBAVEZAN UNOS NARUDZBENICE KUPCA KOD INORACUNA ODNOSI SE SAMO NA ROBU CIJI JE VLASNIK BANKOM ID_VLASNIKA=2 INACE NE
                                    if (ddokument == "InoRacun" && polje == "Narudzbenica")
                                    {
                                        idvlasnik = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "VlasnikRobe").ID.ToString();
                                        if (Convert.ToUInt32(idvlasnik) != 2)
                                        { }
                                        else
                                        {
                                            MessageBox.Show("Popunite podatak za: " + polje);
                                            Vrati = false;
                                            return (Vrati);
                                        }
                                    }
                                    else //nije InoRacun
                                    {
                                        MessageBox.Show("Popunite podatak za " + polje);
                                        Vrati = false;
                                        return (Vrati);
                                    }
                                }
                            }
                        }                   
                } 
            }
            return Vrati;
        }
        public Boolean ProveraSadrzaja(string Dokument, string iddok, string idReda, string operacija, ref string mPoruka)
        {
            string DokumentJe = Convert.ToString(((Bankom.frmChield)forma).DokumentJe);
            Vrati = true;
            if (DokumentJe == "P") return (Vrati);
            //if (Dokument =="Artikli") return (Vrati);
            char[] separators = { ',' };
            string sql = "";          
            clsObradaOsnovnihSifarnika coo = new clsObradaOsnovnihSifarnika();
            string mdatum = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost;
            switch (Dokument)
            {
                case "PDVUlazniRacunZaUsluge":
                    string nazivkom = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivKom").Vrednost;

                    sql = " SELECT DISTINCT TOP 100 PERCENT BrUr FROM " + NazivKlona + "Totali ";
                    sql += " WHERE BrUr <> '' AND BrUr = '" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrUr").Vrednost + "' ";
                    sql += " AND YEAR(Datum) =" + Convert.ToDateTime(mdatum).Year;
                    sql += " AND BrDok not like '%S%' AND NazivKom = '" + nazivkom + "' ";
                    sql += " AND ID_" + NazivKlona.Trim() + "Totali <> " + iddok;
                    t = db.ReturnDataTable(sql);
                    if (t.Rows.Count > 0)
                    {
                        MessageBox.Show("Evidencija ulaza za racun:" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrUr").Vrednost + " vec postoji");
                        Vrati = false;
                        break;
                    }
                    long idavansnogdokumenta = 1;
                    string avansi = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Avansi").Vrednost.Trim();
                    if (nazivkom.Trim() != "")
                    {
                        if (avansi != "")
                        {
                            sql = "select id_dokumenta from Dokumenta where brojdokumenta = '" + avansi + "'";
                            t = db.ReturnDataTable(sql);
                            if (t.Rows.Count > 0)
                                idavansnogdokumenta = Convert.ToInt32(t.Rows[0]["id_dokumenta"]);
                            if (idavansnogdokumenta > 1)
                            {
                                //If Dokument = "PDVIzlazniRacunZaUsluge" Or Dokument = "KonacniRacun" Or Dokument = "KonacniRacunZaHotel" Then
                                // "select distinct NazivKom from AvansniRacunTotali where id_AvansniRacunTotali = " + Str(idavansnogdokumenta), cnn1, adOpenForwardOnly
                                //Else
                                sql = "select distinct NazivKom from UlazniAvansniRacunTotali where id_UlazniAvansniRacunTotali = " + idavansnogdokumenta.ToString();
                                sql += "union select distinct NazivKom from PDVUlazniPredracunZaUslugeTotali where id_PDVUlazniPredracunZaUslugeTotali = " + idavansnogdokumenta.ToString();
                                t = db.ReturnDataTable(sql);
                                if (t.Rows.Count > 0)
                                {
                                    if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivKom").Vrednost.Trim() != t.Rows[0]["NazivKom"].ToString())
                                    {
                                        MessageBox.Show("Naziv komitenta se razlikuje od naziva komitenta sa izabranog Avansa !");
                                        Vrati = false;
                                        break;
                                    }
                                }
                            }
                        }
                        
                    }
                    break;
                case "KonacniUlazniRacun":
                    string mOtpremnica = "";
                    Boolean DeoDaNe = false;
                    string mbrur = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrUr").Vrednost;
                    string mnazivkom = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivKom").Vrednost;
                    DateTime tdatum = Convert.ToDateTime(mdatum);
                    //Provera duplikata unesenog Originalnog broja ulaznog racuna za godinu u kojoj se unosi i zadatog komitenta, preskacuci prazne brojeve    
                    sql = " SELECT DISTINCT BrUr FROM " + NazivKlona + "Totali "
                       + " WHERE BrUr != ''  AND BrUr = '" + mbrur + "' AND Datum.Year =" + tdatum.Year
                       + " AND BrDok not like '%S%' AND NazivKom = mnazivkom  AND ID_" + NazivKlona.Trim() + "Totali != " + iddok;
                    t = db.ReturnDataTable(sql);
                    if (t.Rows.Count > 0)
                    {
                        MessageBox.Show("Evidencija ulaza za racun:" + mbrur + " vec postoji");
                        Vrati = false;
                        break;
                    }
                    //provera da li je Prijemnica vec iskoristena u nekom konacnom ulaznom racunu

                    string idskladiste = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").ID;
                    sql = "Select OpisSkladista FROM Skladiste where  ID_skladiste=" + idskladiste;
                    t = db.ReturnDataTable(sql);
                    string SkladisteJe = t.Rows[0]["OpisSkladista"].ToString();

                    string prijemnica = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Otpremnica").Vrednost.ToString();
                    string[] Otpremnice = prijemnica.Split(separators);
                    if (prijemnica.Trim() == "")
                    {
                        if (idskladiste == "23" || idskladiste == "31" || idskladiste == "175" || SkladisteJe == "Ambalaza")  //(Skladiste="Bioprotein" Skladiste="FSH Bioprotein")
                        {
                            MessageBox.Show("Obavezan unos prijemnice");
                            Vrati = false;
                            break;
                        }
                    }
                    else // prijemnica nije prazna
                    {
                        if (prijemnica.Contains(",") == true) { }
                        else
                            Otpremnice[0] = prijemnica.Trim();


                        for (int i = 0; i < Otpremnice.Length; i++)
                        {
                            mOtpremnica = Otpremnice[i].ToUpper();
                            DeoDaNe = false;
                            if (mOtpremnica.Contains("DEO") == true)
                            {
                                DeoDaNe = true;
                                mOtpremnica = mOtpremnica.Replace("DEO", "");
                            }
                            string godina = Convert.ToString(Convert.ToDateTime(mdatum).Year);

                            // da li je evidentirana prijemnica
                            sql = "if not exists (select BrDok from DokumentaTotali "
                               + "where  BrDok ='" + mOtpremnica.Trim() + "'"
                               + " and  YEAR(DokumentaTotali.Datum) =" + godina + " ) select 0 else select 1";

                            Console.WriteLine(sql);
                            SqlDataReader ifpostoji = db.ReturnDataReader(sql);
                            ifpostoji.Read();
                            if (Convert.ToInt32(ifpostoji[0]) == 0)
                            {
                                MessageBox.Show("Prijemnica " + mOtpremnica + " pogresna ili nije evidentirana!!!");
                                ifpostoji.Close();
                                Vrati = false;
                                break;
                            }
                            ifpostoji.Close();
                        }

                        //' da li je vec unesen ulazni racun za prijemnicu
                        sql = "if not exists (select Otpremnica from Racun, DokumentaTotali  where Racun.Otpremnica ";
                        sql += " like'%" + mOtpremnica.Trim() + "%'";
                        sql += " AND Racun.ID_DokumentaView !=" + iddok.ToString();
                        sql += " AND DokumentaTotali.BrDok not like'%/S%' And DokumentaTotali.BrDok like'78-%'";
                        sql += " AND Racun.ID_DokumentaView = DokumentaTotali.ID_DokumentaTotali ";
                        sql += " AND YEAR(DokumentaTotali.Datum) =" + Convert.ToDateTime(mdatum).Year + " ) select 0 else select 1";
                        SqlDataReader DaLiPoOtp = db.ReturnDataReader(sql);
                        DaLiPoOtp.Read();
                        if (Convert.ToInt32(DaLiPoOtp[0]) == 1)
                        {
                            MessageBox.Show(" Vec postoji racun za unesenu prijemnicu!!! " + mOtpremnica);
                            DaLiPoOtp.Close();
                            Vrati = false;
                            break;
                        }
                        DaLiPoOtp.Close();
                    }
                    break;
                    
                case "PripremaZaPlacanje":
                    string mrac = "";
                    string midrac = "";
                    Operacija = Convert.ToString(((Bankom.frmChield)forma).OOperacija.Text).ToUpper();
                    if (Operacija == "IZMENA" || Operacija == "BRISANJE")
                    {
                        if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "PlacenoDaNe").Vrednost.ToString() == "DA")
                        {
                            MessageBox.Show("Preneseno na placanje ne moze " + Operacija);
                            Vrati = false;
                            break;
                        }

                    }
                    if (Operacija == "IZMENA" || Operacija == "UNOS")
                    {
                        // provera ispravnosti ziro racuna komitenta
                        midrac = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrojTekucegRacuna").ID.ToString();
                        if (Convert.ToUInt32(midrac) > 1)
                        {
                            mrac = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrojTekucegRacuna").Vrednost;
                            if (mrac.Length < 18)
                                mrac = coo.FormatirajRacun(mrac);

                            //poziv funkcije za proveru kontrolnog broja
                            string kb = mrac.Substring(17, 2);
                            MessageBox.Show("Kb=" + Convert.ToString(kb));
                            if (kb != coo.KB_97(mrac.Substring(0, 16)))
                            {
                                MessageBox.Show("Pogresan broj tekuceg racuna komitenta");
                                Vrati = false;
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Obavezan unos tekuceg racuna komitenta !");
                            Vrati = false;
                            break;
                        }
                        ////' provera ispravnosti ziro racuna banke
                        string trpl = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "TekuciRacunPlacanja").ID.ToString();
                        if (Convert.ToInt32(trpl) > 1)
                        {
                            string mr = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "TekuciRacunPlacanja").Vrednost;
                            if (mr.Length < 18)
                                mr = coo.FormatirajRacun(mr);

                            //'poziv funkcije za proveru kontrolnog broja
                            string kb = mr.Substring(17, 2);
                            if (kb != coo.KB_97(mr.Substring(0, 16)))
                            {
                                MessageBox.Show("Pogresan broj tekuceg racuna placanja");
                                Vrati = false;
                                break;
                            }
                        }
                        //' provera ispravnosti unosa komitenta i iznosa placanja
                        string mid = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "PozivNaBroj").ID.ToString();
                        if (Convert.ToInt32(mid) > 1)
                        {
                            sql = "select distinct ID_PozivNaBroj,NazivKom,Isplate from PozivNaBroj where id_PozivNaBroj = " + mid;
                            DataTable tt = new DataTable();
                            tt = db.ReturnDataTable(sql);
                            if (tt.Rows.Count > 0)

                            {
                                if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivKom").Vrednost.Trim() != tt.Rows[0]["NazivKom"].ToString().Trim())
                                {
                                    MessageBox.Show("Naziv komitenta se razlikuje od naziva komitenta sa izabranog dokumenta !");
                                    Vrati = false;
                                    break;
                                }
                            }


                            if (Convert.ToDecimal(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Isplate").Vrednost) > Convert.ToDecimal(tt.Rows[0]["isplate"].ToString()))
                            {
                                //MessageBox.Show("Uneseni iznos veci od iznosa na dokumentu nastavljate ?", vbYesNo) = vbNo Then
                                MessageBox.Show(" Uneseni iznos veci od iznosa na dokumentu !");
                                Vrati = false;
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Pogresno odabran poziv na broj ili komitent!");
                            Vrati = false;
                            break;

                        }

                        //'provera da li je racun vec placen ; da li se nalazi unesen u pripremi za placanje ili izvodu sa istim iznosom
                        string WWhere = "";
                        double UkupnoPlacanje = 0;
                        double RazlikaPlacanja = 0;

                        UkupnoPlacanje = 0;
                        string ponabr = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "PozivNaBroj").Vrednost;
                        string ponid = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "PozivNaBroj").ID.ToString();
                        WWhere = " WHERE ID_DokumentaView =" + iddok + " AND id_PozivNaBroj = " + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "PozivNaBroj").ID.ToString();
                        if (Operacija == "IZMENA")
                            WWhere += " AND ID_PlacanjaNaplate!=" + idReda;
                        UkupnoPlacanje = Convert.ToDouble(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Isplate").Vrednost);


                        //' provera da li je u danasnjem datumu vec unesen  ovaj dokument za placanje  
                        sql = "select Sum(Isplate) as UIslate from PlacanjaNaplate " + WWhere;
                        t = db.ReturnDataTable(sql);
                        if (string.IsNullOrEmpty(t.Rows[0]["UIslate"].ToString())) { }
                        else
                        {
                            UkupnoPlacanje += Convert.ToDouble(t.Rows[0]["UIslate"].ToString());
                            if (UkupnoPlacanje >= Convert.ToDouble(tt.Rows[0]["Isplate"].ToString()))
                            {
                                MessageBox.Show(" Dokument " + ponabr + " je vec unesen za placanje ");
                                Vrati = false;
                                break;
                            }
                        }

                        //' provera da li je dokument vec placen nekim ranijim izvodom
                        sql = "select Sum(Isplate) as UIslate from IzvodStavke Where ID_PozivNaBroj=" + ponid;
                        t = db.ReturnDataTable(sql);
                        if (string.IsNullOrEmpty(t.Rows[0]["UIslate"].ToString())) { }
                        else
                            UkupnoPlacanje += Convert.ToDouble(t.Rows[0]["UIslate"].ToString());
                        if (UkupnoPlacanje > Convert.ToDouble(tt.Rows[0]["Isplate"].ToString()))
                        {
                            RazlikaPlacanja = UkupnoPlacanje - Convert.ToDouble(tt.Rows[0]["Isplate"].ToString());
                            MessageBox.Show(" Dokument " + ponabr + " je placen vise za " + RazlikaPlacanja.ToString());
                            break;
                        }
                    }

                    break;     ////'KRAJ PROVERE Pripremeza placanj
                case "KonacniRacun":
                    string idskl = "";
                    string skl = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").Vrednost;
                    mdatum = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost;
                    idskl = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").ID;

                    if ((idskl == "23" || idskl == "31" || idskl == "175") && Program.imeFirme == "Bankom")
                    {
                        string otpremnica = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Otpremnica").Vrednost.ToString();
                        string[] otpremnice = otpremnica.Split(separators);
                        if (otpremnica.Trim() == "")
                        {
                            MessageBox.Show("Obavezan unos otpremnice");
                            Vrati = false;
                            break;
                        }
                        else
                        {
                            if (otpremnica.Contains(",") == false)
                                otpremnice[0] = otpremnica.Trim();
                        }

                        for (int i = 0; i < otpremnice.Length; i++)
                        {
                            mOtpremnica = otpremnice[i].ToUpper();
                            DeoDaNe = false;
                            if (mOtpremnica.Contains("DEO") == true)
                            {
                                DeoDaNe = true;
                                mOtpremnica = mOtpremnica.Replace("DEO", "");
                            }

                            // da li je evidentirana otpremnica
                            sql = "if not exists (select BrDok from DokumentaTotali "
                               + "where  BrDok ='" + mOtpremnica.Trim() + "'"
                               + " and year(Datum) = " + (Convert.ToDateTime(mdatum)).Year.ToString() + " ) select 0 else select 1";
                            SqlDataReader ifpostoji = db.ReturnDataReader(sql);

                            ifpostoji.Read();
                            if (Convert.ToInt32(ifpostoji[0]) == 0)
                            {
                                MessageBox.Show("Otpremnica " + mOtpremnica + " pogresna ili nije evidentirana!!!");
                                Vrati = false;
                                ifpostoji.Close();
                                break;
                            }
                            ifpostoji.Close();
                            ifpostoji.Dispose();
                            // provera da li je vec napravljen izlazni racun za uneseni broj otpremnice
                            sql = "if not exists (select Otpremnica from Racun, DokumentaTotali ";
                            sql += "where Otpremnica like'%" + (mOtpremnica).Trim() + "%' and ID_Skladiste =" + idskl;
                            sql += " and ID_DokumentaView<>" + iddok;
                            sql += "And BrDok not like '%/S%' and Racun.ID_DokumentaView = ID_DokumentaTotali ";
                            sql += " and year(Datum) = " + (Convert.ToDateTime(mdatum)).Year.ToString() + " ) select 0 else select 1";

                            SqlDataReader postoji = db.ReturnDataReader(sql);
                            postoji.Read();
                        
                            if (Convert.ToInt32(postoji[0]) == 1)
                            {
                                if (DeoDaNe == false)
                                {
                                    MessageBox.Show("Otpremnica vec postoji!!!");
                                    postoji.Close();
                                    Vrati = false;
                                    break;
                                }
                            }
                            postoji.Close();
                            postoji.Dispose();
                        }
                    }
                    // da li je dobro odabran avansni racun
                    idavansnogdokumenta = 1;
                    avansi = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Avansi").Vrednost.Trim();
                    //nazivkom = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivKom").Vrednost.Trim();
                    //if (nazivkom.Trim() != "")
                    //{
                    if (avansi != "")
                    {
                        sql = "select id_dokumenta from Dokumenta where brojdokumenta = '" + avansi + "'";
                        t = db.ReturnDataTable(sql);
                        if (t.Rows.Count > 0)
                            idavansnogdokumenta = Convert.ToInt32(t.Rows[0]["id_dokumenta"]);
                        if (idavansnogdokumenta > 1)
                        {
                            //If Dokument = "PDVIzlazniRacunZaUsluge" Or Dokument = "KonacniRacun" Or Dokument = "KonacniRacunZaHotel" Then
                            sql = "select distinct NazivKom from AvansniRacunTotali where id_AvansniRacunTotali = " + idavansnogdokumenta.ToString();

                            //sql = "select distinct NazivKom from UlazniAvansniRacunTotali where id_UlazniAvansniRacunTotali = " + idavansnogdokumenta.ToString();
                            //sql += "union select distinct NazivKom from PDVUlazniPredracunZaUslugeTotali where id_PDVUlazniPredracunZaUslugeTotali = " + idavansnogdokumenta.ToString();
                            t = db.ReturnDataTable(sql);
                            if (t.Rows.Count > 0)
                            {
                                if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivKom").Vrednost.Trim() != t.Rows[0]["NazivKom"].ToString())
                                {
                                    MessageBox.Show("Naziv komitenta se razlikuje od naziva komitenta sa izabranog Avansa !");
                                    Vrati=false;
                                    break;
                                }
                            }
                        }
                    }  
                    break;
                case "InoRacun":
                    //''''PROVERA IZBORAINSTRUKCIJE ZA PLACANJE
                    if (Program.imeFirme == "Bankom")
                    {
                        // SKINUTI KOD PRELASKA SQL2016 NE VALJA TABELA INSTRUKCIJE U BANKOMNOVI!!!!! BORKA
                        ////sql = "Select ID_SifrarnikValuta from Instrukcije where ID_Instrukcije=" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "VerzijaInstrukcije").Vrednost;
                        ////t = db.ReturnDataTable(sql);
                        ////if (t.Rows.Count == 0)
                        ////{
                        ////    MessageBox.Show("Pogresan izbor instrukcije za placanje!!!");
                        ////    Vrati = false;
                        ////    return Vrati;
                        ////}

                        ////if (t.Rows[0]["ID_SifrarnikValuta"].ToString() != forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "OznVal").ID)
                        ////{
                        ////    MessageBox.Show("Pogresan izbor instrukcije za placanje!!!");
                        ////    Vrati = false;
                        ////    return Vrati;                        
                        //}
                    }
                    //// da li je dobro odabran avansni racun
                    //idavansnogdokumenta = 1;
                    //avansi = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Avansi").Vrednost.Trim();
                    //if (avansi != "")
                    //{
                    //    sql = "select id_dokumenta from Dokumenta where brojdokumenta = '" + avansi + "'";
                    //    t = db.ReturnDataTable(sql);
                    //    if (t.Rows.Count > 0)
                    //        idavansnogdokumenta = Convert.ToInt32(t.Rows[0]["id_dokumenta"]);
                    //    if (idavansnogdokumenta > 1)
                    //    {
                    //        //If Dokument = "PDVIzlazniRacunZaUsluge" Or Dokument = "KonacniRacun" Or Dokument = "KonacniRacunZaHotel" Then
                    //        sql = "select distinct NazivKom from AvansniRacunTotali where id_AvansniRacunTotali = " + idavansnogdokumenta.ToString();

                    //        //sql = "select distinct NazivKom from UlazniAvansniRacunTotali where id_UlazniAvansniRacunTotali = " + idavansnogdokumenta.ToString();
                    //        //sql += "union select distinct NazivKom from PDVUlazniPredracunZaUslugeTotali where id_PDVUlazniPredracunZaUslugeTotali = " + idavansnogdokumenta.ToString();
                    //        t = db.ReturnDataTable(sql);
                    //        if (t.Rows.Count > 1)
                    //        {
                    //            if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivKom").Vrednost.Trim() != t.Rows[0]["NazivKom"].ToString())
                    //            {
                    //                MessageBox.Show("Naziv komitenta se razlikuje od naziva komitenta sa izabranog Avansa !");
                    //                break;
                    //            }
                    //        }
                    //    }
                    //}

                    break;
                case "NarudzbenicaKupca":
                    if (operacija == "BRISANJE" || operacija == "IZMENA")
                    {
                        int postoji = 0;
                        sql = "if not exists (select ID_Narudzbenica from Racun, DokumentaTotali "
                          + "where ID_Narudzbenica=" + forma.Controls["liddok"].Text
                          + " And Racun.ID_DokumentaView = DokumentaTotali.ID_DokumentaTotali "
                          + "and DokumentaTotali.Brdok not like'%S%' )"
                          + " select 0 else select 1";
                        SqlDataReader ifpostoji = db.ReturnDataReader(sql);
                        ifpostoji.Read();
                        postoji = Convert.ToInt32(ifpostoji[0]);
                        ifpostoji.Close();

                        if (postoji == 1)
                        {
                            mPoruka = ("Narudzbenica ukljucena u prodaju NIJE DOZVOLJENA Operacija!!!");
                            Vrati = false;
                            break;
                        }
                        break;
                    }
                    
                    break;
                case "OpisTransakcije":
                    if (idReda == "-1") break;
                    if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Duguje").Vrednost.Trim() == ""
                        && forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Potrazuje").Vrednost.Trim() == "")
                    {
                        MessageBox.Show("Popunite duguje ili potrazuje!");
                        return false;
                    }

                    sql = "Select alijaspolja as PPolje,Izborno from recnikPodataka where StornoIUpdate <>'D' and tabindex >= 0 and width >0 "
                        + " and dokument=@param0 order by tabindex";
                    DataTable dt = db.ParamsQueryDT(sql, Dokument);
                    foreach (DataRow row in dt.Rows)
                    {
                        if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == row["PPolje"].ToString()).Vrednost.Trim() != ""
                             && row["Izborno"].ToString().Trim() != "")
                        {

                            if (row["PPolje"].ToString() != "Konto" && row["PPolje"].ToString() != "Analitika")
                            {
                                if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == row["PPolje"].ToString()).Vrednost.Contains(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Tabela").Vrednost) == false)
                                {
                                    //Console.WriteLine(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == row["PPolje"].ToString()).Vrednost);
                                    //Console.WriteLine(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Tabela").Vrednost);
                                    if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == row["PPolje"].ToString()).Vrednost == "DomacaValuta" && row["PPolje"].ToString() == "Valuta") { }
                                    else
                                    {
                                        MessageBox.Show("Vrednost polja: " + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == row["PPolje"].ToString()).IME + " nije iz dokumenta: " + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Tabela").Vrednost);
                                        return false;
                                    }
                                }
                            }                            
                       
                        }

                    }

                    break;
                default:
                    break;

            }
            //"KonacniUlazniRacun" Or Dokument = "PDVUlazniRacunZaUsluge" Or Dokument = "KonacniRacun" Or Dokument = "PDVIzlazniRacunZaUsluge" Or Dokument = "KonacniRacunZaHotel" )
            return Vrati; 
        }
        public Boolean ProveraKursa(string Dokument, ref string mPoruka)
        {
            Vrati = true;
            forma = Program.Parent.ActiveMdiChild;
            ddokument = Dokument;
            NazivKlona = Dokument;
            Vrati = ProveriKurs(ref mPoruka);
            string dDokument = Dokument;
            return (Vrati);
        }
        Boolean ProveriKurs(ref string Poruka)
        {
            //Datum = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost;
            if (ddokument == "KonacniUlazniRacun") kojidatum = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "DatumCarinjenja").Vrednost;
            else
            {
                kojidatum = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost;
            }
            if (kojidatum.Trim() == "")
            {
                Vrati = true;
                return (Vrati);
            }
            sql = "Select * from KursnaLista where datum=@param0"; /// + kojidatum + "'";
            DataTable t = new DataTable();
            t = db.ParamsQueryDT(sql, kojidatum);
            if (t.Rows.Count == 0)
            {
                Poruka = "Ne postoji kursnalista za:" + kojidatum;
                Vrati = false;
                return (Vrati);
            }

            sql = "Select UlazniIzlazni from SifarnikDokumenta Where Naziv=@param0"; /// + ddokument + "'";
            t = db.ParamsQueryDT(sql,ddokument);
            if (t.Rows.Count > 0)
                NazivKlona = t.Rows[0]["UlazniIzlazni"].ToString();

            int rr = 0;
            sql = "select * from RecnikPodataka Where  Dokument='" + NazivKlona.Trim() + "' AND  Polje like '%DomVal/%'";
            t = db.ReturnDataTable(sql);
            if (t.Rows.Count == 0)
            {
                Vrati = true;
                return (Vrati);
            }
            srednji = 1;
            string polje = t.Rows[rr]["Polje"].ToString();
            Console.WriteLine(polje);
            char[] separators = new[] { '/' };
            string[] ppolje = polje.Split(separators); 
            s =ppolje[1];//nazivpolja oznaka valute               
            Console.WriteLine(s);
            if (s == "DomVal") { return (Vrati);}
            else
            {
                kojidd = polje.Substring(0, p); // naziv polja datum
                DataTable tt = new DataTable();
                sql = "Select * from RecnikPodataka where dokument='" + NazivKlona.Trim() + "' AND  AlijasPolja = '" + kojidd + "' and tabindex>0";
                tt = db.ReturnDataTable(sql);
                if (tt.Rows.Count > 0)
                {
                    IDValuta = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == s).ID.ToString();
                    string kojavaluta = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == s).Vrednost.ToString();

                    Vrati = PostojiKurs(kojidatum, IDValuta, kojavaluta, ref srednji, ref Poruka);
                    if (Vrati == false)
                    {
                        MessageBox.Show(Poruka);
                        return (Vrati);
                    }
                }
            }
            return Vrati;
        }
        public Boolean PostojiKurs(string kojidatum, string IDValuta,string oznval, ref double SrednjiKurs, ref string mPoruka)
        {
            Boolean vrati = true;
            sql = " SELECT Srednji,OznVal FROM KursnaLista WHERE Datum='"+kojidatum+"'and ID_SifrarnikValuta="+IDValuta;
            DataTable tk = new DataTable();
            tk = db.ReturnDataTable(sql);
            if (tk.Rows.Count == 0)
            {
                mPoruka = "Nedostaje Kursna lista za datum:" + kojidatum+ " i valutu:" + oznval;
                vrati = false;
                return (vrati);
            }
            SrednjiKurs = Convert.ToDouble(tk.Rows[0]["Srednji"]);
            return (vrati);
        }


        public Boolean DodatnaProvera()
        {
            forma = Program.Parent.ActiveMdiChild;
            long IdDokView = ((Bankom.frmChield)forma).iddokumenta;
            string sql = "";
            DataTable t = new DataTable();
            string NazivKlona = "";
            string SkladisteJe = "";
            string nazivpolja = "";
            string pnazivpolja = "";
            string nazivskl = "";
            string idr = "";
            string nazivskliz = "";
            string nazivsklU = "";
            string lot = "";
            string Operacija = forma.Controls["OOperacija"].Text.ToUpper();
            sql = "Select UlazniIzlazni from SifarnikDokumenta Where Naziv=@param0"; /// + ddokument + "'";
            t = db.ParamsQueryDT(sql, ddokument);
            if (t.Rows.Count > 0)
                NazivKlona = t.Rows[0]["UlazniIzlazni"].ToString();

            //Provera datumskih polja
            sql = "Select AlijasPolja as polje from recnikpodataka as r WHERE   Dokument =@param0 AND   width > 0  AND  TabIndex >= 0 AND StornoIUpdate<>'D' ";
            sql += " and (ID_TipoviPodataka = 8 or ID_TipoviPodataka = 9)";
            Console.WriteLine(sql);
            t = db.ParamsQueryDT(sql, NazivKlona);           
            if (t.Rows.Count > 0)
            {
                DateTime dt = new DateTime(); // danasnji datum
                DateTime dk = new DateTime(); // datum zakljucenja knjiga
                int rr = 0;
                do
                {
                    string polje = t.Rows[rr]["polje"].ToString();
                    string Datum = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == polje).Vrednost;

                    dt = Convert.ToDateTime(Datum);
                    dk = Program.kDatum;
                    if (dt.Year != dk.Year)
                    {
                        if (dt < dk)
                        {
                            MessageBox.Show("Proverite vrednost polja: " + polje);
                            Vrati = false;
                            return (Vrati);
                        }
                    }
                    rr = rr + 1;
                } while (rr < t.Rows.Count);
            }

            string mgrid = ((Bankom.frmChield)forma).imegrida;
            int Tud=0;
// Borka lot u LotNalogZaPoluproizvodStavkeView POCETAK
            string iid = Convert.ToString(((Bankom.frmChield)forma).idReda);
            if ((((Bankom.frmChield)forma).Controls.Find(mgrid, true).FirstOrDefault() is DataGridView dv))

            Tud = Convert.ToInt32(dv.Tag);

            string sqs = "";
            DataTable ts = new DataTable();
            switch (NazivKlona)
            {                   
                case "LotNalogZaPoluproizvod":
                case "LotNalogZaPoluproizvodEkstrakcija":
                    if (NazivKlona == "LotNalogZaPoluproizvod")
                    {
                        //PROVERA SAGLASNOSTI LOTA I PROIZVODA U DRUGOM GRIDU
                        if (Convert.ToInt32(iid) > -1) // fform.GridK(2).IdUpdateReda > -1 Then //Ako smo podigli  stavku u drugog grida
                        {
                            Field pbl = (Field)forma.Controls["Lot"];
                            if (pbl != null)
                            {
                                lot = pbl.Vrednost.Substring(0, pbl.Vrednost.IndexOf(","));
                            }
                            //string lot = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Lot").Vrednost;
                            if (lot.Trim() != "")
                            {
                                sql = "Select ID_Artikli from lot where barkod='" + lot + "'";
                                sqs = "Select ID_SirovinaView from NalogKooperantaStavke where ID_NalogKooperantaStavke=" + iid;
                                t = db.ReturnDataTable(sql);  //
                                ts = db.ReturnDataTable(sqs);
                                Console.WriteLine(t.Rows[0]["ID_Artikli"].ToString());
                                Console.WriteLine(ts.Rows[0]["ID_SirovinaView"].ToString());
                                if (ts.Rows[0]["ID_SirovinaView"].ToString() != t.Rows[0]["ID_Artikli"].ToString())
                                {
                                    MessageBox.Show("Ne slazu se Lot i naziv proizvoda");
                                    Vrati = false;
                                    return (Vrati);
                                }
                            }
                        }

                        //PROVERA SAGLASNOSTI LOTA I PROIZVODA U prvom GRIDU
                        string lotproizvoda = "";
                        Field pb = (Field)forma.Controls["LotProizvoda"];
                        if (pb != null)
                        {              
                            lotproizvoda= pb.Vrednost.Substring(0, pb.Vrednost.IndexOf(","));
                        }
                        Field pb1 = (Field)forma.Controls["Receptura"];
                        if (pb1 != null)
                        {
                            idr = pb1.ID;
                        }                           
                        if (Operacija == "IZMENA" && idr == "1") //Ako nismo podigli  stavku  prvog grida ili vrsimo unos
                        { }
                        else
                        {
                            sql = "Select ID_ArtikliView from lotview where barkod ='" + lotproizvoda + "'";
                            sqs = "select DISTINCT ID_ProizvodView from RastavnicaTotali where ID_RastavnicaTotali=" + idr;
                            t = db.ReturnDataTable(sql);
                            ts = db.ReturnDataTable(sqs);
                            if (ts.Rows[0]["ID_ProizvodView"].ToString() != t.Rows[0]["ID_ArtikliView"].ToString())
                            {
                                MessageBox.Show("Ne slazu se Lot i naziv proizvoda");
                                Vrati = false;
                                return (Vrati);
                            }
                        }
                    }
                    // Borka lot u LotNalogZaPoluproizvodStavkeView KRAJ

                    //  Jovana - ekstrakcija 27.11.18
                    //string lot = "";
                    if (NazivKlona == "LotNalogZaPoluproizvodEkstrakcija")
                    {
                        // PROVERA SAGLASNOSTI LOTA I PROIZVODA U TRECEM GRIDU
                        if (Convert.ToInt32(iid) > -1) //Ako smo podigli  stavku u drugog grida
                        {
                            //string lot = "";
                            Field pb = (Field)forma.Controls["Lot"];
                            if (pb != null)
                            {
                                lot = pb.Vrednost;
                            }
                            //string lot = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Lot").Vrednost;
                            sql = "Select ID_Artikli from lot where barkod=@param0";
                            sqs = "Select ID_SirovinaView from NalogKooperantaStavke where ID_NalogKooperantaStavke=@param0"; // + iid;
                            t =  db.ParamsQueryDT(sql,lot);
                            ts = db.ParamsQueryDT(sqs, iid);
                            if (ts.Rows[0]["ID_SirovinaView"].ToString() != t.Rows[0]["ID_Artikli"].ToString())
                            {
                                MessageBox.Show("Ne slazu se Lot i naziv proizvoda");
                                Vrati = false;
                                return (Vrati);
                            }
                        }
                        //PROVERA SAGLASNOSTI LOTA I PROIZVODA U DRUGOM GRIDU
                        if (Convert.ToInt32(iid) > -1)     //Ako smo podigli  stavku u drugog grida
                        {
                            Field pblp = (Field)forma.Controls["LotPomocne"];
                            if (pblp != null)
                            {
                                lot = pblp.Vrednost;
                            }
                            //string lot = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "LotPomocne").Vrednost;
                            sql = "Select ID_Artikli from lot where barkod=@param0";
                            sqs = "Select ID_SirovinaPomocneView from NalogKooperantaSirovinePomocneStavke where ID_NalogKooperantaSirovinePomocneStavke=@param0";
                            t = db.ParamsQueryDT(sql,lot);
                            ts = db.ParamsQueryDT(sqs,iid);
                            if (ts.Rows[0]["ID_SirovinaPomocneView"].ToString() != t.Rows[0]["ID_Artikli"].ToString())
                            {
                                MessageBox.Show("Ne slazu se Lot i naziv pomocne sirovine");
                                Vrati = false;
                                return (Vrati);
                            }

                        }

                        //PROVERA SAGLASNOSTI LOTA I PROIZVODA U prvom GRIDU
                        if ((Convert.ToInt32(iid) > -1 && Operacija == "IZMENA") || (Operacija == "UNOS")) //// '''Ako smo podigli  stavku  prvo grida ili vrsimo unos
                        {
                            Field pblp = (Field)forma.Controls["LotProizvoda"];
                            if (pblp != null)
                            {
                                lot = pblp.Vrednost;
                            }
                            // jovana 04.11.20 u lot treba da bude samo lot isecen , a ne sve treba pblp.text
                            // string lot = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "LotProizvoda").Vrednost;
                            Field pbrc = (Field)forma.Controls["Receptura"];
                            if (pbrc != null)
                            {
                                idr = pbrc.ID;
                            }
                            //idr = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Receptura").ID;
                            sql = "Select ID_Artikli from lot where barkod=@param0";
                            sqs = "select DISTINCT ID_ProizvodView from Rastavnica2Totali where ID_Rastavnica2Totali=@param0";
                            t = db.ParamsQueryDT(sql,lot);
                            ts = db.ParamsQueryDT(sqs,idr);
                            if (ts.Rows[0]["ID_ProizvodView"].ToString() != t.Rows[0]["ID_Artikli"].ToString())
                            {
                                MessageBox.Show("Ne slazu se Lot i naziv sirovine");
                                Vrati = false;
                                return (Vrati);
                            }
                        }
                    }

                    //provera pripadnosti magacinskih polja POCETAK
                    string nazivpolja1 = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPolja1").Vrednost;
                    nazivpolja = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPolja").Vrednost;
                    nazivskliz = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSklIz").Vrednost;
                    nazivsklU = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSklU").Vrednost;
                    string pnaziv1 = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPolja1").label.Text;
                    string pnaziv = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPolja").Vrednost;
                    if (nazivpolja1.Trim() != "")
                        sql = "Select * From MagacinskaPoljaStavkeView where NazivPolja='" + nazivpolja1 + "' And NazivSkl='" + nazivskliz + "'";
                    t = db.ReturnDataTable(sql);
                    if (t.Rows.Count == 0)
                    {
                        MessageBox.Show("Pogresna vrednost: " + nazivpolja1 + " za  polje:" + pnaziv1);
                        Vrati = false;
                        return (Vrati);
                    }
                                       
                    if (nazivpolja.Trim() != "")
                    {
                        sql = "Select * From MagacinskaPoljaStavkeView where NazivPolja='" + nazivpolja + "' And NazivSkl='" + nazivsklU + "'";
                        t = db.ReturnDataTable(sql);
                        if (t.Rows.Count == 0)
                        {
                            Vrati = false;
                            return (Vrati);
                        }
                    }
                    //    '  Jovana - ekstrakcija 27.11.18
                    if (NazivKlona == "LotNalogZaPoluproizvodEkstrakcija")
                    {
                        nazivpolja = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPoljaPomocne").Vrednost;
                        pnazivpolja = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPoljaPomocne").label.Text;

                        nazivskliz = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSklIz").Vrednost;
                        if (nazivpolja.Trim() != "")
                        {
                            sql = "Select * From MagacinskaPoljaStavkeView where NazivPolja='" + nazivpolja + "' And NazivSkl='" + nazivskliz + "'";
                            t = db.ReturnDataTable(sql);
                            if (t.Rows.Count == 0)
                            {
                                MessageBox.Show("Pogresna vrednost: " + nazivpolja + " za  polje:" + pnazivpolja);
                                Vrati = false;
                                return (Vrati);
                            }
                        }

                        if (Operacija == "IZMENA" && ((Bankom.frmChield)forma).idReda == -1) { goto ProveriStanje; }
                    }
                    break;
                case "LotPDVInternaDostavnica":
                    pnazivpolja = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPolja").label.Text;
                    nazivpolja = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPolja").Vrednost;
                    nazivskliz = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSklIzlaz").Vrednost;
                    sql = "Select * From MagacinskaPoljaStavkeView where NazivPolja='" + nazivpolja + "' And NazivSkl='" + nazivskliz + "'";
                    t = db.ReturnDataTable(sql);
                    if (t.Rows.Count == 0)
                    {
                        MessageBox.Show("Pogresna vrednost: " + nazivpolja + " za  polje:" + pnazivpolja);
                        Vrati = false;
                        return (Vrati);
                    }
                    else
                    {
                      
                        string nazivpoljaulaz = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPoljaUlaz").Vrednost;
                        
                        string pnazivpoljaulaz = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPoljaUlaz").label.Text;
                        nazivsklU = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSklUlaz").Vrednost;
                        sql = "Select * From MagacinskaPoljaStavkeView where NazivPolja='" + nazivpoljaulaz + "' And NazivSkl='" + nazivsklU + "'";
                        t = db.ReturnDataTable(sql);
                        if (t.Rows.Count == 0)
                        {
                            MessageBox.Show("Pogresna vrednost: " + nazivpoljaulaz + " za polje:" + pnazivpoljaulaz);
                            Vrati = false;
                            return (Vrati);
                        }
                    }

                    break;
                case "LotInterniNalogZaRobu":
                case "LotPDVInterniNalogZaRobu":
                case "LotOtpremnica":        //   Or NazivKlona = "LotPrijemnica" Or NazivKlona = "LotPDVDostavnicaUMaloprodaju" Then

                    nazivpolja = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPolja").Vrednost;
                    pnazivpolja = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPolja").label.Text;
                    nazivskl = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").Vrednost;
                    string ids = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").ID;

                    if (Convert.ToInt32(ids) == 85) { }/// TUDJA ROBA NA SKLADISTU U OBRENOVCU
                    else
                    {
                        // Jovana 04.12.20
                        sql = "Select * From MagacinskaPoljaStavkeView where NazivPolja='" + nazivpolja + "' And NazivSkl='" + nazivskl + "'";
                        t = db.ReturnDataTable(sql);
                        if (t.Rows.Count == 0)
                        {
                            MessageBox.Show("Pogresna vrednost: " + nazivpolja + " za polje:" + pnazivpolja);
                            Vrati = false;
                            return (Vrati);
                        }
                    }
                    break;

                case "PrevodjenjeProizvoda":
                    string starasifrau = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "StaraSifraU").Vrednost;
                    string starasifra = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "StaraSifra").Vrednost;
                     nazivpolja = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPolja").Vrednost;
                    string nazivpoljau = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPoljaU").Vrednost;
                    string pnazivpoljau = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPoljaU").label.Text;
                    pnazivpolja = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivPolja").label.Text;
                    nazivskl = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").Vrednost;
                    idr = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").ID;
                    if (starasifra != starasifrau)
                    {
                        MessageBox.Show("Pogresno!!!! Nije dozvoljeno prevodjenje izmedju razlicitih artikala    KORISTITE NALOG ZA DORADU!!!");  // "razliciti artikli u prevodjenju", "OK"
                        Vrati = false;
                        return (Vrati);
                    }
                    if (Convert.ToInt32(idr) != 85)    /// 85= TUDJA ROBA NA SKLADISTU U 
                    {
                        sql = "Select * From MagacinskaPoljaStavkeView where NazivPolja='" + nazivpolja + "' And NazivSkl='" + nazivskl + "'";
                        t = db.ReturnDataTable(sql);
                        if (t.Rows.Count == 0)
                        {
                            MessageBox.Show("Pogresna vrednost: " + nazivpolja + " za polje:" + pnazivpolja);
                            Vrati = false;
                            return (Vrati);
                        }
                        else
                        {
                            sql = "Select * From MagacinskaPoljaStavkeView where NazivPolja='" + nazivpoljau + "' And NazivSkl='" + nazivskl + "'";
                            t = db.ReturnDataTable(sql);
                            if (t.Rows.Count == 0)
                            {
                                MessageBox.Show("Pogresna vrednost: " + nazivpoljau + " za polje:" + pnazivpoljau);
                                Vrati = false;
                                return (Vrati);
                            }
                        }
                    }
                    break;
                //  provera pripadnosti magacinskih polja KRAJ

                //'''NALOGZARAZDUZENJEAMBALAZE pocetak
                case "NalogZaRazduzenjeAmbalaze":

                    idr = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivProizvoda").ID;

                    //'''Provera kolicine ambalaze vezano za pakovanje
                    int pakovanje = 0;                    
                    //'    SUBSTRING(Proizvodi.NazivArt, CHARINDEX('(', Proizvodi.NazivArt) + 1, CHARINDEX(' / ', Proizvodi.NazivArt) - CHARINDEX('(', Proizvodi.NazivArt) - 1)
                    sql = "Select NazivArt,SUBSTRING(ArtikliView.NazivArt, CHARINDEX('(', NazivArt) + 1, CHARINDEX('/', NazivArt) - CHARINDEX('(', NazivArt) - 1) as pakovanje  from ArtikliView Where ID_ArtikliView=" + idr;
                    t = db.ReturnDataTable(sql);
                    pakovanje = Convert.ToInt32(t.Rows[0]["pakovanje"]);
                    string kolicina = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Kolicina").Vrednost;
                    string kolicinaproizvoda = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "KolicinaProizvoda").Vrednost;
                    int mpakovanje = Convert.ToInt32(kolicinaproizvoda) / Convert.ToInt32(kolicina);
                    if (pakovanje != mpakovanje)
                    {
                        MessageBox.Show("Pogresna Kolicina ambalaze za proizvod ");
                        Vrati = false;
                        return (Vrati);
                    }

                    sql = "Select s.OpisSkladista from RazduzenjeAmbalazeStavke as ns,MagacinskaPolja as m ,Skladiste as s where ns.ID_DokumentaView=" + (IdDokView).ToString()
                      + " AND ns.ID_MagacinskaPolja=m.ID_MagacinskaPolja AND m.ID_Skladiste=s.ID_Skladiste";
                    t = db.ReturnDataTable(sql);
                    if (t.Rows.Count > 0) { SkladisteJe = t.Rows[0]["OpisSkladista"].ToString(); }
               //'''NALOGZARAZDUZENJEAMBALAZE kraj
                    break;
            }
        ProveriStanje:
            //If Firma <> "Leotar" Then 
            //SkladisteJe = "Tudje ";
            if (SkladisteJe.ToUpper().Contains("USLUG") == true || SkladisteJe.ToUpper().Contains("MALO") == true || SkladisteJe.Trim() == "")
            {
                Vrati = true;
                return (Vrati);
            }
            //else
            //{
            //    Vrati = this.ProveraStanja(NazivKlona, Convert.ToString(IdDokView));
                
            //}
                return (Vrati);        
        }//Kraj DodatnaProvera

        public Boolean ProveraStanja(string Dokument, string IdDok)
        {
                clsProveraStanja PRS = new clsProveraStanja();
                SqlConnection conn = new SqlConnection();
                SqlTransaction transaction;
                transaction = conn.BeginTransaction("trTransaction");
                conn = null;
                string Poruka = PRS.ProveriStanje(Dokument, IdDok, ref conn,ref transaction);
                if (Poruka.Trim() != "")
                {
                    MessageBox.Show(Poruka);
                    Poruka = "";
                    return (false);
                }
                return (true);

        }
        
    }// kraj provera ispravnosti
}
      

 

        
