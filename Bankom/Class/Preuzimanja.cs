using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Bankom.Class
{
    class Preuzimanja
    {
        public static void PreuzimanjeUplataKupacaIzBanaka()
        {
            var db = new DataBaseBroker();
            labela1:
            if (UnosDatumaPreuzimanjaUplata(out var pDatum)) return;

            if (!DateTime.TryParse(pDatum, out var temp))
            {
                MessageBox.Show("Pogresno unesen datum ponovite!!");
                goto labela1;
            }

            const string pDokument = "PrometUplata";
            const string dokument = "PrometUplata";
            const string rssifdok = "Select * from SifarnikDokumenta Where naziv = @param0";

            var dt = db.ParamsQueryDT(rssifdok, pDokument);
            var dr = dt.Rows[0];
            var nazivKlona = Convert.ToString(dr["UlazniIzlazni"]);
            int IdDokView=0;
            var proveraDok =
                "Select Brdok,datum,ID_DokumentaStablo,ID_DokumentaTotali,Predhodni from DokumentaTotali " +
                " where Dokument=@param0" + " AND Datum=@param1";
  
            var prDok = db.ParamsQueryDT(proveraDok, pDokument, pDatum);
            if (prDok.Rows.Count == 0)
            {
                if (PronadjiIdDokumentaStablo(db, pDokument, out var idDokumentaStablo)) return;

               
                string BrojDok = "";
                string opis = "Uplata na dan " + pDatum;
                clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                IdDokView = os.UpisiDokument(ref BrojDok, opis, 102, pDatum);
            }
                    
            var dDatum = Convert.ToDateTime(prDok.Rows[0]["Datum"]);

            var chieldform = new frmIzvod
            {
                toolLabel1 = {Text = dokument},
                dateTimePicker1 = {Value = dDatum}
                //Tag = IdDokView
            };


            chieldform.ShowDialog();


            //db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + nazivKlona.Trim(),
            //    "IdDokument:" + idDokView);

            var izvor = chieldform.strPutanjaPlacanja;
            var cilj = izvor.Replace("PlacanjaUplate\\", "PlacanjaUplate\\Preuzeto\\");
            File.Copy(izvor, cilj, true);
            File.Delete(izvor);

        }

        private static DataTable ExecuteTotaliDokument(DataBaseBroker db, string brojDok, string pDokument,
            string pDatum)
        {
            var proveraDok = "select id_dokumenta from dokumenta where BrojDokumenta =@param0";
            var prDok = db.ParamsQueryDT(proveraDok, brojDok);
            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta",
                "IdDokument:" + Convert.ToInt32(prDok.Rows[0]["Id_Dokumenta"]));

            proveraDok =
                "Select Brdok,datum,ID_DokumentaStablo,ID_DokumentaTotali,Predhodni from DokumentaTotali " +
                " where Dokument=@param0" + " AND Datum=@param1";
            prDok = db.ParamsQueryDT(proveraDok, pDokument, pDatum);
            return prDok;
        }

        private static bool UnosDatumaPreuzimanjaUplata(out string pDatum)
        {
            pDatum = Prompt.ShowDialog(DateTime.Now.ToShortDateString(), "Preuzimanje uplata kupaca iz banaka",
                "Unesite datum za koji preuzimamo uplate");
            if (pDatum != "") return false;
            MessageBox.Show("Unijeli ste prazan datum!!");
            return true;

        }

        public static void PreuzimanjeManjkovaIViskova()
        {
            var db = new DataBaseBroker();

            if (UnosDokumentaPopisa(out var brDokPopisa))
                return; // trazimo od korisnika da unese BrDok popisa, ako unese prazan broj napustamo funkciju preuzimanja viskova i manjkova

            if (PronadjiDokumentPopisa(db, brDokPopisa, out var idDokView, out var pDokument, out var pDatum))
                return; // provjeravamo da li postoji uneseni broj dokumenta i ako postoji uzimamo odredjene podatke inace napustamo funkciju

            ProveriPostojanje(db, pDatum, brDokPopisa, pDokument,
                idDokView); // proveravamo da nije vec odradjeno preuzimanje manjkova i viskova za dati iddokument ,ako nije pozivamo funkciju koja ce to da odradi

            // ReSharper disable once LocalizableElement
            MessageBox.Show(
                "Zavrseno!!!"); // obavjestavamo korisnika da je uspjesno odradio preuzimanje manjkova i viskova

        }

        private static void ProveriPostojanje(DataBaseBroker db, string pDatum, string brDokPopisa, string pDokument,
            int idDokView)
        {
            const string rsu =
                "Select DokumentaTotali.Brdok,DokumentaTotali.datum,ID_DokumentaStablo,ID_DokumentaTotali from DokumentaTotali, OrganizacionaStrukturaStavkeView " +
                " where   DokumentaTotali.Datum=@param0  and id_OrganizacionaStrukturaView =" +
                "ID_OrganizacionaStrukturaStavkeView   And ID_OrganizacionaStrukturaStablo=@param1 " +
                "and DokumentaTotali.Predhodni = @param2";
            var dt = db.ParamsQueryDT(rsu, pDatum, Program.idFirme, brDokPopisa);
            if (dt.Rows.Count == 0)
            {
                PreuzmiManjkoveIViskove(pDatum, brDokPopisa, pDokument, idDokView);
            }
            else
            {
                // ReSharper disable once LocalizableElement
                MessageBox.Show("Vec su preneseni manjkovi i viskovi za dokument broj " + brDokPopisa.Trim());
            }
        }

        private static bool PronadjiDokumentPopisa(DataBaseBroker db, string brDokPopisa, out int idDokView,
            out string pDokument, out string pDatum)
        {
            const string rsu =
                "Select Brdok,datum,ID_DokumentaStablo,ID_DokumentaTotali,Dokument from DokumentaTotali,OrganizacionaStrukturaStavkeView " +
                " where   BrDok=@param0 AND id_OrganizacionaStrukturaView=" +
                " ID_OrganizacionaStrukturaStavkeView  And ID_OrganizacionaStrukturaStablo=@param1";
            var dt = db.ParamsQueryDT(rsu, brDokPopisa, Program.idFirme);

            if (dt.Rows.Count == 0)
            {
                // ReSharper disable once LocalizableElement
                MessageBox.Show("Nije registrovan dokument broj " + brDokPopisa.Trim());
                idDokView = 0;
                pDokument = null;
                pDatum = null;
                return true;
            }
            else
            {
                var dr = dt.Rows[0];
                idDokView = Convert.ToInt32(dr["ID_DokumentaTotali"]);
                pDokument = Convert.ToString(dr["Dokument"]);
                pDatum = Convert.ToString(dr["Datum"]);
            }

            return false;
        }

        private static bool UnosDokumentaPopisa(out string brDokPopisa)
        {
            brDokPopisa =
                Prompt.ShowDialog("", "Manjkovi i viskovi",
                    "Unesite dokument rekapitulacije popisa"); //od korisnika trazimo da unese broj dokumenta popisa za koji hoce da preuzme manjkove i viskove

            if (brDokPopisa != "")
                return false; // korisnik je unio broj i vracamo su u prethodnu funkciju i nastavljamo izvrsavanje
            // ReSharper disable once LocalizableElement
            MessageBox.Show("Niste unijeli dokument rekapitulacije popisa!!!");
            return true;

        }

        private static void PreuzmiManjkoveIViskove(string datum, string brDok, string pDokument, int idDok)
        {
            var db = new DataBaseBroker();
            const int pArtikal = 0;
            const int presloKalo = 0;
            const double pKolicina = 0;
            var mdokument1 = "";

            var rsmanjak =
                PostaviUpit(pDokument,
                    out var mdokument); // u zavisnosti da li je  dokument sa ili bez lota iz odgovarajuce tabele preuzimamo manjkove

            var dt = db.ParamsQueryDT(rsmanjak, brDok, datum);

            var manj = dt.Rows.Count == 0 ? null : dt.Rows[0];

            if (ProveraDaLiImaKaloZaDokument(brDok, dt)) goto labela1;


            if (PronadjiIdDokumentaStablo(db, mdokument, out var idDokumentaStablo)) goto labela1;

            Debug.Assert(manj != null, nameof(manj) + " != null");

            string brojDok = "";
            string opis = "Popis kalo - " + Convert.ToInt32(manj["NazivSkl"]);
            clsObradaOsnovnihSifarnika cos = new clsObradaOsnovnihSifarnika();
            var iddok = cos.UpisiDokument(ref brojDok, opis, idDokumentaStablo,  datum);
            ////ClsObradaOsnovnihSifarnika(datum, pDokument, idDok, idDokumentaStablo, db, out var rbr, out var brojDok,
            //    //"Popis kalo - " + Convert.ToInt32(manj["NazivSkl"]), "NijeProknjizeno", true);

            var drr = DataRow(db, brojDok);

            //ObradiMesecPoreza(DateTime.Now.ToShortDateString(), Convert.ToInt32(drr["ID_Dokumenta"]), "", datum);

            InsertInterniNalogZaRobu(manj, db, drr);

            mdokument1 = pDokument.Contains("Lot") ? "PDVInterniNalogZaRobu" : "LotPDVInterniNalogZaRobu";

            if (PronadjiIdDokumentaStablo(db, mdokument1, out idDokumentaStablo)) goto labela1;
            opis = "Popis manjak - " + Convert.ToInt32(manj["NazivSkl"]);
            iddok = cos.UpisiDokument(ref brojDok, opis, idDokumentaStablo, datum);

            //clsObradaOsnovnihSifarnika(datum, pDokument, idDok, idDokumentaStablo, db, out rbr, out brojDok,
                //"Popis manjak - " + Convert.ToInt32(manj["NazivSkl"]), "NijeProknjizeno", true);

            var drr1 = DataRow(db, brojDok);

            //ObradiMesecPoreza(DateTime.Now.ToShortDateString(), Convert.ToInt32(drr["ID_Dokumenta"]), "", datum);

            InsertInterniNalogZaRobu(manj, db, drr1);

            PopuniInterniNalogZaRobuStavke(pDokument, dt, pArtikal, pKolicina, presloKalo, db, drr, drr1);

            IzvrsiTotale(db, drr, mdokument, drr1, mdokument1);


            labela1: // unos viska

            rsmanjak = PostaviUpitVisak(pDokument, out mdokument);

            dt = db.ParamsQueryDT(rsmanjak, brDok, datum);

            if (ProveraDaLiImaVisakZaDokument(brDok, dt)) return;

            if (PronadjiIdDokumentaStablo(db, mdokument, out idDokumentaStablo)) return;

            Debug.Assert(manj != null, nameof(manj) + " != null");

            opis = "Popis visak - " + Convert.ToInt32(manj["NazivSkl"]);
            //ClsObradaOsnovnihSifarnika(datum, pDokument, idDok, idDokumentaStablo, db, out rbr, out brojDok,
            //"Popis visak - " + Convert.ToInt32(manj["NazivSkl"]), "NijeProknjizeno", true);
            brojDok = "";
            clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
            var iddokum = os.UpisiDokument(ref brojDok,opis,idDokumentaStablo,datum);
            var drrr1 = DataRow(db, brojDok);

            //ObradiMesecPoreza(DateTime.Now.ToShortDateString(), Convert.ToInt32(drrr1["ID_Dokumenta"]), "", datum);

            InterniNalogZaRobuVisak(db, drrr1, manj);


            PopuniInterniNalogZaRobuStavkeVisak(datum, brDok, pDokument, db, drrr1);

            ObradiTotaleZaVisak(db, drrr1, mdokument1);
        }

        private static void ObradiTotaleZaVisak(DataBaseBroker db, DataRow drrr1, string mdokument1)
        {
            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta",
                "IdDokument:" + Convert.ToInt32(drrr1["Id_Dokumenta"]));
            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta: " + mdokument1,
                "IdDokument:" + Convert.ToInt32(drrr1["Id_Dokumenta"]));
            db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje",
                "IdDokument:" + Convert.ToInt32(drrr1["Id_Dokumenta"]));
            db.ExecuteStoreProcedure("StanjeRobePoLotu", "IdDokument: " + Convert.ToInt32(drrr1["Id_Dokumenta"]));
        }

        private static void PopuniInterniNalogZaRobuStavkeVisak(string datum, string brDok, string pDokument,
            DataBaseBroker db,
            DataRow drrr1)
        {
            if (pDokument.Contains("Lot"))
            {
                const string insertinto =
                    "INSERT INTO  InterniNalogZaRobuStavke ( ID_ArtikliView , ID_DokumentaView , KolicinaIzlaz , KolicinaUlaz , JedinicnaCena , ID_LotView , ID_MagacinskaPolja)" +
                    "SELECT distinct ID_ArtikliView , @param0 ,0 , Visak , NabavnaCena , 1 , 1 " +
                    " FROM  VisakBezLotaView  WHERE Brdok= @param1" +
                    " and Visak > 0 and  datum=@param2 ";
                db.ParamsQueryDS(insertinto, drrr1["ID_Dokumenta"], brDok, datum);
            }
            else
            {
                string insertinto =
                    "INSERT INTO  InterniNalogZaRobuStavke ( ID_ArtikliView , ID_DokumentaView , KolicinaIzlaz , KolicinaUlaz , JedinicnaCena , ID_LotView , ID_MagacinskaPolja)" +
                    "SELECT distinct ID_ArtikliView , @param0 ,0 , Visak , NabavnaCena , ID_LotView , ID_MagacinskaPolja " +
                    " FROM  VisakView  WHERE Brdok= @param1" +
                    " and Visak > 0 and  datum=@param2 ";
                db.ParamsQueryDS(insertinto, drrr1["ID_Dokumenta"], brDok, datum);
            }
        }

        private static void InterniNalogZaRobuVisak(DataBaseBroker db, DataRow drrr1, DataRow manj)
        {
            const string insert =
                "INSERT INTO InterniNalogZaRobu (ID_DokumentaView ,ID_KomitentiView ,ID_SifrarnikValuta,ID_Skladiste,PozivNaBrDok,ID_Analitika)" +
                " Values ( @param0,@param1,@param2,@param3,@param4,@param5)";
            db.ParamsQueryDS(insert, Convert.ToInt32(drrr1["ID_Dokumenta"]), 1, 1,
                Convert.ToInt32(manj["ID_Skladiste"]),
                "Po popisu", 1129);
        }

        private static bool ProveraDaLiImaVisakZaDokument(string brDok, DataTable dt)
        {
            if (dt.Rows.Count != 0) return false;
            MessageBox.Show("Ne postoji visak za dokument: " + brDok);
            return true;

        }

        private static string PostaviUpitVisak(string pDokument, out string mdokument)
        {
            string rsmanjak;
            if (pDokument.Contains("Lot"))
            {
                rsmanjak =
                    "SELECT BrDok ,NazivSkl, datum,StaraSifra,NazivArt,JedinicaMere,ID_ArtikliView,ID_Skladiste, " +
                    " Visak,VrednostVisak,VlasnikRobe " +
                    " FROM  VisakBezLotaView  WHERE Brdok= @param0" +
                    " and Visak > 0 and  datum=@param1 Order by NazivArt";
                mdokument = "InterniNalogZaRobu";
            }
            else
            {
                rsmanjak =
                    "SELECT BrDok ,NazivSkl, datum,StaraSifra,NazivArt,JedinicaMere,Lot,NazivPolja,ID_LotView,ID_MagacinskaPolja,ID_ArtikliView,ID_Skladiste, " +
                    " Visak,VrednostVisak, VlasnikRobe " +
                    " FROM  VisakView  WHERE Brdok= @param0" +
                    " and Visak > 0 and  datum= @param1 Order by NazivArt";
                mdokument = "InterniNalogZaRobu";
            }

            return rsmanjak;
        }

        private static void IzvrsiTotale(DataBaseBroker db, DataRow drr, string mdokument, DataRow drr1,
            string mdokument1)
        {
            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta",
                "IdDokument:" + Convert.ToInt32(drr["Id_Dokumenta"]));
            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta: " + mdokument,
                "IdDokument:" + Convert.ToInt32(drr["Id_Dokumenta"]));
            db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje",
                "IdDokument:" + Convert.ToInt32(drr["Id_Dokumenta"]));
            db.ExecuteStoreProcedure("StanjeRobePoLotu", "IdDokument: " + Convert.ToInt32(drr["Id_Dokumenta"]));
            const string rsPostojiManjak = "select * from InterniNalogZaRobuStavke where ID_DokumentaView= @param0";
            var dtrsPostojiManjak = db.ParamsQueryDT(rsPostojiManjak, drr1["ID_Dokumenta"]);
            if (dtrsPostojiManjak.Rows.Count != 0)
            {
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta",
                    "IdDokument:" + Convert.ToInt32(drr1["Id_Dokumenta"]));
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta: " + mdokument1,
                    "IdDokument:" + Convert.ToInt32(drr1["Id_Dokumenta"]));
                db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje",
                    "IdDokument:" + Convert.ToInt32(drr1["Id_Dokumenta"]));
                db.ExecuteStoreProcedure("StanjeRobePoLotu", "IdDokument: " + Convert.ToInt32(drr1["Id_Dokumenta"]));
            }
            else
            {
                db.ParamsQueryDS("Delete from Dokumenta where ID_Dokumenta=@param0", drr1["Id_Dokumenta"]);
                db.ParamsQueryDS("Delete from InterniNalogZaRobu where ID_DokumentaView = @param0",
                    drr1["Id_Dokumenta"]);
            }
        }

        private static void PopuniInterniNalogZaRobuStavke(string pDokument, DataTable dt, int pArtikal,
            double pKolicina,
            int presloKalo, DataBaseBroker db, DataRow drr, DataRow drr1)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (pArtikal == Convert.ToInt32(row["ID_ArtikliView"])) goto labela2;
                pKolicina = 0;
                presloKalo = 0;
                pArtikal = Convert.ToInt32(row["ID_ArtikliView"]);

                labela2:
                pKolicina += Convert.ToDouble(row["manjak"]);
                if (pKolicina <= Convert.ToDouble(row["DozvoljeniManjak"]))
                {
                    if (pDokument.Contains("Bez"))
                    {
                        const string insert1 =
                            "INSERT INTO  InterniNalogZaRobuStavke ( ID_ArtikliView , ID_DokumentaView , KolicinaIzlaz , KolicinaUlaz , JedinicnaCena , ProdajnaCena, ID_LotView , ID_MagacinskaPolja)" +
                            " values(@param0,@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                        db.ParamsQueryDS(insert1, Convert.ToInt32(row["ID_ArtikliView"]),
                            Convert.ToInt32(drr["ID_Dokumenta"]),
                            Convert.ToInt32(row["manjak"]), 0, Convert.ToInt32(row["NabavnaCena"]),
                            Convert.ToInt32(row["NabavnaCena"]), 1, 1);
                    }
                    else
                    {
                        const string insert1 =
                            "INSERT INTO  InterniNalogZaRobuStavke ( ID_ArtikliView , ID_DokumentaView , KolicinaIzlaz , KolicinaUlaz , JedinicnaCena , ProdajnaCena, ID_LotView , ID_MagacinskaPolja)" +
                            " values(@param0,@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                        db.ParamsQueryDS(insert1, Convert.ToInt32(row["ID_ArtikliView"]),
                            Convert.ToInt32(drr["ID_Dokumenta"]),
                            Convert.ToInt32(row["manjak"]), 0, Convert.ToInt32(row["NabavnaCena"]),
                            Convert.ToInt32(row["NabavnaCena"]), Convert.ToInt32(row["ID_LotView"]),
                            Convert.ToInt32(row["ID_MagacinskaPolja"]));
                    }
                }
                else
                {
                    if (presloKalo == 0)
                    {
                        if (pDokument.Contains("Bez"))
                        {
                            var insert1 =
                                "INSERT INTO  InterniNalogZaRobuStavke ( ID_ArtikliView , ID_DokumentaView , KolicinaIzlaz , KolicinaUlaz , JedinicnaCena , ProdajnaCena, ID_LotView , ID_MagacinskaPolja)" +
                                " values(@param0,@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                            db.ParamsQueryDS(insert1, Convert.ToInt32(row["ID_ArtikliView"]),
                                Convert.ToInt32(drr["ID_Dokumenta"]),
                                Convert.ToInt32(row["manjak"]) - (pKolicina - Convert.ToInt32(row["Dozvoljenimanjak"])),
                                0,
                                Convert.ToInt32(row["NabavnaCena"]), Convert.ToInt32(row["NabavnaCena"]), 1, 1);

                            insert1 =
                                "INSERT INTO  InterniNalogZaRobuStavke ( ID_ArtikliView , ID_DokumentaView , KolicinaIzlaz , KolicinaUlaz , JedinicnaCena , ProdajnaCena, ID_LotView , ID_MagacinskaPolja)" +
                                " values(@param0,@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                            db.ParamsQueryDS(insert1, Convert.ToInt32(row["ID_ArtikliView"]),
                                Convert.ToInt32(drr1["ID_Dokumenta"]), pKolicina - Convert.ToInt32(row["manjak"]), 0,
                                Convert.ToInt32(row["NabavnaCena"]), Convert.ToInt32(row["NabavnaCena"]), 1, 1);
                        }
                        else
                        {
                            var insert1 =
                                "INSERT INTO  InterniNalogZaRobuStavke ( ID_ArtikliView , ID_DokumentaView , KolicinaIzlaz , KolicinaUlaz , JedinicnaCena , ProdajnaCena, ID_LotView , ID_MagacinskaPolja)" +
                                " values(@param0,@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                            db.ParamsQueryDS(insert1, Convert.ToInt32(row["ID_ArtikliView"]),
                                Convert.ToInt32(drr["ID_Dokumenta"]),
                                Convert.ToInt32(row["manjak"]) - (pKolicina - Convert.ToInt32(row["manjak"])), 0,
                                Convert.ToInt32(row["NabavnaCena"]), Convert.ToInt32(row["NabavnaCena"]), 1, 1);

                            insert1 =
                                "INSERT INTO  InterniNalogZaRobuStavke ( ID_ArtikliView , ID_DokumentaView , KolicinaIzlaz , KolicinaUlaz , JedinicnaCena , ProdajnaCena, ID_LotView , ID_MagacinskaPolja)" +
                                " values(@param0,@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                            db.ParamsQueryDS(insert1, Convert.ToInt32(row["ID_ArtikliView"]),
                                Convert.ToInt32(drr1["ID_Dokumenta"]),
                                Convert.ToInt32(row["manjak"]) - (pKolicina - Convert.ToInt32(row["manjak"])), 0,
                                Convert.ToInt32(row["NabavnaCena"]), Convert.ToInt32(row["NabavnaCena"]), 1, 1);
                        }

                        presloKalo = 1;
                    }
                    else
                    {
                        if (pDokument.Contains("Bez"))
                        {
                            const string insert1 =
                                "INSERT INTO  InterniNalogZaRobuStavke ( ID_ArtikliView , ID_DokumentaView , KolicinaIzlaz , KolicinaUlaz , JedinicnaCena , ProdajnaCena, ID_LotView , ID_MagacinskaPolja)" +
                                " values(@param0,@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                            db.ParamsQueryDS(insert1, Convert.ToInt32(row["ID_ArtikliView"]),
                                Convert.ToInt32(drr1["ID_Dokumenta"]), Convert.ToInt32(row["manjak"]), 0,
                                Convert.ToInt32(row["NabavnaCena"]), Convert.ToInt32(row["NabavnaCena"]), 1, 1);
                        }
                        else
                        {
                            const string insert1 =
                                "INSERT INTO  InterniNalogZaRobuStavke ( ID_ArtikliView , ID_DokumentaView , KolicinaIzlaz , KolicinaUlaz , JedinicnaCena , ProdajnaCena, ID_LotView , ID_MagacinskaPolja)" +
                                " values(@param0,@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                            db.ParamsQueryDS(insert1, Convert.ToInt32(row["ID_ArtikliView"]),
                                Convert.ToInt32(drr1["ID_Dokumenta"]), Convert.ToInt32(row["manjak"]), 0,
                                Convert.ToInt32(row["NabavnaCena"]), Convert.ToInt32(row["NabavnaCena"]),
                                Convert.ToInt32(row["ID_LotView"]), Convert.ToInt32(row["ID_MagacinskaPolja"]));
                        }
                    }
                }
            }
        }

        private static bool ProveraDaLiImaKaloZaDokument(string brDok, DataTable dt)
        {
            if (dt.Rows.Count != 0) return false;
            MessageBox.Show("Ne postoji kalo za dokument: " + brDok.Trim());
            return true;

        }

        private static void InsertInterniNalogZaRobu(DataRow manj, DataBaseBroker db, DataRow drr)
        {
            const string insert =
                "INSERT INTO InterniNalogZaRobu (ID_DokumentaView ,ID_KomitentiView ,ID_SifrarnikValuta,ID_Skladiste,PozivNaBrDok,ID_Analitika)" +
                " Values ( @param0,@param1,@param2,@param3,@param4,@param5)";
            Debug.Assert(manj != null, nameof(manj) + " != null");
            db.ParamsQueryDS(insert, Convert.ToInt32(drr["ID_Dokumenta"]), 1, 1, Convert.ToInt32(manj["ID_Skladiste"]),
                "Po popisu", 1128);

        }

        private static DataRow DataRow(DataBaseBroker db, string brojDok)
        {
            const string rsd = "select id_dokumenta from dokumenta where BrojDokumenta = @param0";
            var rstd = db.ParamsQueryDT(rsd, brojDok);
            var drr = rstd.Rows.Count == 0 ? null : rstd.Rows[0];
            return drr;
        }

        private  void ObradiSifarnik(string datum, string pDokument, int idDok,
            int idDokumentaStablo ,string Dok, string oOpis, string proknjizeno,bool mesecporeza)
        {
            clsObradaOsnovnihSifarnika cos = new clsObradaOsnovnihSifarnika();
        //int  rbr = 0;
        int IdDokumenta = 0;
        string BrojDok = "";
          //  brojDok = cls.KreirajBrDokNovi(pDokument, ref rbr, datum, Program.idOrgDeo, long.Parse(Program.SifRadnika),
          //      idDokumentaStablo);
         IdDokumenta = cos.UpisiDokument(ref BrojDok,oOpis , idDokumentaStablo,datum);
                        
        }

        private static bool PronadjiIdDokumentaStablo(DataBaseBroker db, string mdokument, out int idDokumentaStablo)
        {
            const string rsDokumentaStablo = "select ID_DokumentaStablo from DokumentaStablo where Naziv= @param0";
            var dt1 = db.ParamsQueryDT(rsDokumentaStablo, mdokument);

            if (dt1.Rows.Count != 0)
            {
                var dr = dt1.Rows[0];
                idDokumentaStablo = Convert.ToInt32(dr["ID_DokumentaStablo"]);
            }
            else
            {
                MessageBox.Show("Nije registrovan dokument " + mdokument);
                idDokumentaStablo = 0;
                return true;
            }

            return false;
        }

        private static string PostaviUpit(string pDokument, out string mdokument)
        {

            string rsmanjak;
            if (pDokument.Contains("Lot"))
                rsmanjak =
                    "SELECT BrDok ,NazivSkl, datum,StaraSifra,NazivArt,JedinicaMere,ID_ArtikliView,ID_Skladiste, " +
                    " Manjak,VrednostManjak,Ulaz,Kalo,KaloVrednost,OporeziviManjak ,VrednostOporeziviManjak,Uredba,VlasnikRobe,DozvoljeniManjak,NabavnaCena " +
                    " FROM  ManjakBezLotaView  WHERE Brdok= @param0" + " and  datum= @param1 Order by NazivArt";
            else
                rsmanjak =
                    "SELECT BrDok ,NazivSkl, datum,StaraSifra,NazivArt,JedinicaMere,Lot,NazivPolja,ID_LotView,ID_MagacinskaPolja,ID_ArtikliView,ID_Skladiste, " +
                    " Manjak,VrednostManjak,Ulaz,Kalo,KaloVrednost,OporeziviManjak ,VrednostOporeziviManjak,Uredba,VlasnikRobe ,DozvoljeniManjak,NabavnaCena" +
                    " FROM  ManjakView  WHERE Brdok= @param0" + " and  datum= @param1 Order by NazivArt";

            mdokument = pDokument.Contains("Lot") ? "InterniNalogZaRobu" : "LotInterniNalogZaRobu";
            return rsmanjak;
        }

        //public static void ObradiMesecPoreza(string tekuciDatum, int idDok, string brd, string datumDokumenta)
        //{
        //    var db = new DataBaseBroker();
        //    try
        //    {
        //        var pttime = DateTime.ParseExact(tekuciDatum, "dd.mm.yyyy", null);
        //        var pDatum = DateTime.ParseExact(datumDokumenta, "dd.mm.yyyy", null);

        //        var danPoreza = Program.ID_MojaZemlja == 38 ? 10 : 15;

        //        int pmesec;
        //        if (pDatum.Month != pttime.Month && pttime.Day <= danPoreza)
        //        {
        //            pmesec = pttime.Month - 1;
        //            if (pmesec == 0) pmesec = 12;
        //        }
        //        else
        //        {
        //            pmesec = pttime.Month;
        //        }

        //        if (brd != "")
        //        {
        //            const string upd = "UPDATE Dokumenta  SET MesecPoreza=@param0 WHERE BrojDokumenta= @param1";
        //            db.ParamsQueryDS(upd, pmesec, brd);
        //        }
        //        else
        //        {
        //            const string upd = "UPDATE Dokumenta  SET MesecPoreza=@param0 WHERE ID_Dokumenta= @param1";
        //            db.ParamsQueryDS(upd, pmesec, idDok);
        //        }
        //    }
        //    catch (FormatException)
        //    {

        //        MessageBox.Show("Datum nije u validnom formatu.");
        //    }



        //}

        public static void FaktureRecepcijeZaOdabraneDatume()
        {
            DateTime temp;

            labela1:
            if (UnosDatumaOdKojegPrebacujemoFakture(out var datumOd)) return;
            if (!DateTime.TryParse(datumOd, out temp))
            {
                MessageBox.Show("Pogresno unesen datum ponovite!!");
                goto labela1;
            }

            labela2:
            if (UnosDatumaDoKojegPrebacujemoFakture(out var datumDo)) return;
            if (!DateTime.TryParse(datumDo, out temp))
            {
                MessageBox.Show("Pogresno unesen datum ponovite!!");
                goto labela2;
            }

            var odDat = Convert.ToDateTime(datumOd);
            var doDat = Convert.ToDateTime(datumDo);

            var brDan = Convert.ToInt32((doDat - odDat).TotalDays) + 1;
            if (brDan <= 0)
            {
                MessageBox.Show("Pogresno uneseni datumi ponovite!!");
                goto labela1;
            }

            var db = new DataBaseBroker();
            for (var i = 0; i < brDan; i++)
            {
                var select =
                    "SELECT Datum from DokumentaTotali where (ID_DokumentaStablo=225 or ID_DokumentaStablo=401) AND  opis like 'Recepcija%' AND Convert(date,Datum)='" +
                    datumOd + "'";
                var dt = db.ReturnDataTable(select);
                if (dt.Rows.Count != 0)
                    MessageBox.Show("Vec je izvrseno preuzimanje faktura za datum:" + datumOd);
                else
                    PreuzmiFaktureizPrometa(datumOd, "PrometRecepcije");

                odDat = Convert.ToDateTime(datumOd).AddDays(1);
                datumOd = odDat.ToShortDateString();
            }

            MessageBox.Show("Zavrseno!!");
        }

        private static bool UnosDatumaDoKojegPrebacujemoFakture(out string datumDo)
        {
            datumDo = Interaction.InputBox("Unesite datum do kojeg prebacujemo fakture",
                "Prebacivanje faktura recepcije", DateTime.Now.ToShortDateString());

            return datumDo == "";
        }

        private static bool UnosDatumaOdKojegPrebacujemoFakture(out string datumOd)
        {
            datumOd = Interaction.InputBox("Unesite datum od kojeg prebacujemo fakture",
                "Prebacivanje faktura recepcije", DateTime.Now.ToShortDateString());

            return datumOd == "";
        }

        public static void PreuzmiFaktureizPrometa(string datum, string kojipromet)
        {
            long rr = 0;
            bool lastOne = false;

            var db = new DataBaseBroker();
            var opis = kojipromet.Contains("Recepcij") == true ? "Recepcija-" : "Bar-";

            string rspromet = "SELECT DISTINCT Datum, ID_Skladiste,"
                              + " ID_ArtikliView , kolicina  as KK,  ProdajnaCena  as PC ,NazivSkl,NacinPl,ID_NacinPl,ID_KomitentiView,ProcenatRabata,Racun,OznVal,Neoporezivo "
                              + " FROM  " + kojipromet + "Totali  WHERE ID_KomitentiView <>1  and "
                              + "  datum='" + Convert.ToDateTime(datum).ToString("yyyy-dd-MM") + "' Order by Racun";
            var dt = db.ReturnDataTable(rspromet);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoje virmani za datum:" + datum);
                return;
            }

            const string del = "DELETE from PrometPreneseni";
            var cmd = new SqlCommand(del);
            db.Comanda(cmd);
            foreach (DataRow row in dt.Rows)
            {
                const string sql =
                    "INSERT INTO PrometPreneseni(Datum,ID_Skladiste,NazivSkladista,id_Artikal,Popust,Cena,kolicina,ID_Komitent,ID_NacinPlacanja,Racun,OznakaValute,Neoporezivo) VALUES (@Datum,@ID_Skladiste,@NazivSkladista,@id_Artikal,@Popust,@Cena,@kolicina,@ID_Komitent,@ID_NacinPlacanja,@Racun,@OznakaValute,@Neoporezivo)";

                cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@Datum", Convert.ToDateTime(row["Datum"]));
                cmd.Parameters.AddWithValue("@ID_Skladiste", Convert.ToInt32(row["ID_Skladiste"]));
                cmd.Parameters.AddWithValue("@NazivSkladista", Convert.ToString(row["NazivSkl"]));
                cmd.Parameters.AddWithValue("@id_Artikal", Convert.ToInt32(row["ID_ArtikliView"]));
                cmd.Parameters.AddWithValue("@Popust", Convert.ToDouble(row["ProcenatRabata"]));
                cmd.Parameters.AddWithValue("@Cena", Convert.ToDouble(row["pc"]));
                cmd.Parameters.AddWithValue("@kolicina", Convert.ToDouble(row["kk"]));
                cmd.Parameters.AddWithValue("@ID_Komitent", Convert.ToInt32(row["id_KomitentiView"]));
                cmd.Parameters.AddWithValue("@ID_NacinPlacanja", Convert.ToInt32(row["Id_NacinPl"]));
                cmd.Parameters.AddWithValue("@Racun", Convert.ToInt32(row["racun"]));
                cmd.Parameters.AddWithValue("@OznakaValute", Convert.ToString(row["OznVal"]));
                cmd.Parameters.AddWithValue("@Neoporezivo", Convert.ToInt32(row["Neoporezivo"]));

                if (db.Comanda(cmd) == "") rr = rr + 1;
                else MessageBox.Show("Greska u upitu!");
            }

            int pomKom = 0;
            int pomRacun = 0;
            string pomSv = "";
            int pomNp = 0;
            string pDokument = "";
            int pidDok = 0;
            bool tag1 = false;
            bool tag2 = false;
            string prometprepisani =
                " Select ID_Skladiste,k.Id_Komitenti,OznakaValute,kolicina,cena ,k.NazivKomitenta,id_Artikal,Racun,ID_NacinPlacanja,Popust,Neoporezivo "
                + " from prometpreneseni as p,komitenti as k where k.Id_Komitenti=p.Id_Komitent "
                + " order by k.Id_Komitenti,OznakaValute,Racun,Neoporezivo,id_Artikal ";
            DataTable dtpp = db.ReturnDataTable(prometprepisani);

            foreach (DataRow row in dtpp.Rows)
            {

                if (tag1 == true)
                {
                    lastOne = false;
                    if (Convert.ToInt32(row["ID_Komitenti"]) == pomKom && Convert.ToInt32(row["racun"]) == pomRacun &&
                        Convert.ToString(row["OznakaValute"]) == pomSv)
                    {

                        string insertracunstavke =
                            "INSERT INTO RacunStavke(ID_DokumentaView,ID_ArtikliView,ProdajnaCena,ProcenatRabata,Kolicina,uuser) VALUES (@ID_DokumentaView,@ID_ArtikliView,@ProdajnaCena,@ProcenatRabata,@Kolicina,@uuser)";
                        cmd = new SqlCommand(insertracunstavke);
                        cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
                        pomSv = Convert.ToString(row["OznakaValute"]);
                        cmd.Parameters.AddWithValue("@ID_ArtikliView", Convert.ToInt32(row["ID_Artikal"]));
                        cmd.Parameters.AddWithValue("@ProdajnaCena", Convert.ToDouble(row["Cena"]));
                        cmd.Parameters.AddWithValue("@ProcenatRabata", Convert.ToDouble(row["Popust"]));
                        cmd.Parameters.AddWithValue("@Kolicina", Convert.ToDouble(row["Kolicina"]));
                        cmd.Parameters.AddWithValue("@uuser", Program.idkadar);

                        if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");
                        tag1 = true;
                        continue;


                    }

                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);

                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + pDokument, "IdDokument:" + pidDok);
                    tag1 = false;
                }

                if (tag2 == true)
                {
                    lastOne = false;
                    if (Convert.ToInt32(row["ID_Komitenti"]) == pomKom && Convert.ToInt32(row["racun"]) == pomRacun &&
                        Convert.ToString(row["OznakaValute"]) == pomSv && pomNp == Convert.ToInt32(row["Neoporezivo"]))
                    {

                        string insertracunzauslugestavke =
                            "INSERT INTO RacunZaUslugeStavke(ID_DokumentaView,ID_ArtikliView,FakturnaVrednost,uuser,ID_Poreza,ProcenatRabata,Kolicina) VALUES (@ID_DokumentaView,@ID_ArtikliView,@FakturnaVrednost,@uuser,@ID_Poreza,@ProcenatRabata,@Kolicina)";
                        cmd = new SqlCommand(insertracunzauslugestavke);
                        cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
                        cmd.Parameters.AddWithValue("@ID_ArtikliView", Convert.ToInt32(row["ID_Artikal"]));
                        cmd.Parameters.AddWithValue("@FakturnaVrednost",
                            Convert.ToDouble(row["Kolicina"]) * Convert.ToDouble(row["Cena"]) *
                            (1 - Convert.ToDouble(row["Popust"]) / (double) 100));
                        cmd.Parameters.AddWithValue("@uuser", Program.idkadar);
                        string porez = "Select ID_TarifaPoreza from Artikli where ID_Artikli=" +
                                       Convert.ToInt32(row["ID_Artikal"]);
                        DataTable dtporez = db.ReturnDataTable(porez);
                        DataRow drporez = dtporez.Rows[0];
                        if (dtporez.Rows.Count != 0)
                        {

                            cmd.Parameters.AddWithValue("@ID_Poreza", Convert.ToInt32(drporez["ID_TarifaPoreza"]));
                        }


                        cmd.Parameters.AddWithValue("@ProcenatRabata", Convert.ToDouble(row["Popust"]));
                        cmd.Parameters.AddWithValue("@Kolicina", Convert.ToDouble(row["Kolicina"]));

                        if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");
                        tag2 = true;
                        continue;


                    }

                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);

                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + pDokument, "IdDokument:" + pidDok);
                    tag2 = false;

                }

                pomKom = Convert.ToInt32(row["ID_Komitenti"]);
                var pomIdSkladiste = Convert.ToInt32(row["ID_Skladiste"]);
                pomRacun = Convert.ToInt32(row["racun"]);
                pomSv = Convert.ToString(row["OznakaValute"]);
                pomNp = Convert.ToInt32(row["Neoporezivo"]);
                int pIdSifVal;
                if (pomSv.Trim() == "RSD")
                {
                    pDokument = "KonacniRacunZaHotel";
                    pIdSifVal = 1;
                }
                else
                {
                    pDokument = "PDVInoRacunZaUsluge";
                    string rsv = "SELECT ID_SifrarnikValuta from SifrarnikValuta Where OznVal='" +
                                 Convert.ToString(row["OznakaValute"]).Trim() + "'";
                    DataTable dtrsv = db.ReturnDataTable(rsv);
                    DataRow row1 = dtrsv.Rows[0];
                    pIdSifVal = dtrsv.Rows.Count != 0 ? Convert.ToInt32(row1["ID_SifrarnikValuta"]) : 1;
                }

                string dokumentastablo = "SELECT * FROM DokumentaStablo where Naziv='" + pDokument + "'";
                DataTable dtdoksta = db.ReturnDataTable(dokumentastablo);
                DataRow dr = dtdoksta.Rows[0];
                int idDokumentaStablo;
                if (dtdoksta.Rows.Count != 0)
                {

                    idDokumentaStablo = Convert.ToInt32(dr["ID_DokumentaStablo"]);
                }
                else
                {
                    MessageBox.Show("Nije registrovan dokument promet maloprodaje!");
                    return;
                }

                clsObradaOsnovnihSifarnika cls = new clsObradaOsnovnihSifarnika();
                var rbr = 0;
                int IdDokView;
                string BrojDok = "";
                clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                IdDokView = os.UpisiDokument(ref BrojDok, opis, idDokumentaStablo, datum.ToString());



                //string insertdokumenta =
                //    "INSERT INTO Dokumenta(RedniBroj,ID_KadrovskaEvidencija,ID_DokumentaStablo,BrojDokumenta,Datum,Opis,ID_OrganizacionaStrukturaView,Proknjizeno,MesecPoreza) VALUES (@RedniBroj,@ID_KadrovskaEvidencija,@ID_DokumentaStablo,@BrojDokumenta,@Datum,@Opis,@ID_OrganizacionaStrukturaView,@Proknjizeno,@MesecPoreza)";
                //cmd = new SqlCommand(insertdokumenta);
                //cmd.Parameters.AddWithValue("@RedniBroj", rbr);
                //cmd.Parameters.AddWithValue("@ID_KadrovskaEvidencija", Program.idkadar);

                //cmd.Parameters.AddWithValue("@ID_DokumentaStablo", idDokumentaStablo);
                //cmd.Parameters.AddWithValue("@BrojDokumenta", brojDok);
                //cmd.Parameters.AddWithValue("@Datum", Convert.ToDateTime(datum));
                //cmd.Parameters.AddWithValue("@Opis", opis + Convert.ToString(row["NazivKomitenta"]));
                //cmd.Parameters.AddWithValue("@ID_OrganizacionaStrukturaView", Program.idOrgDeo);

                //cmd.Parameters.AddWithValue("@Proknjizeno", "NijeProknjizeno");
                //cmd.Parameters.AddWithValue("@MesecPoreza", Convert.ToDateTime(datum).Month);

                //if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");

                //string selectdokumenta = "SELECT ID_Dokumenta from Dokumenta where BrojDokumenta ='" + brojDok + "'";

                pidDok = IdDokView;  ////db.ReturnInt(selectdokumenta, 0);

                if (Convert.ToString(row["OznakaValute"]).Trim() == "RSD")
                {

                    string insertracun =
                        "INSERT INTO Racun(ID_DokumentaView,ID_Skladiste,ID_SifrarnikValuta,ID_KomitentiView,ID_NacinPlacanja,ID_OrganizacionaStrukturaStavkeView,brur,DatumDpo,Neoporezivo,ValutaPl,uuser) VALUES (@ID_DokumentaView,@ID_Skladiste,@ID_SifrarnikValuta,@ID_KomitentiView,@ID_NacinPlacanja,@ID_OrganizacionaStrukturaStavkeView,@brur,@DatumDpo,@Neoporezivo,@ValutaPl,@uuser)";
                    cmd = new SqlCommand(insertracun);
                    cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
                    cmd.Parameters.AddWithValue("@ID_Skladiste", pomIdSkladiste);
                    cmd.Parameters.AddWithValue("@ID_SifrarnikValuta", pIdSifVal);
                    cmd.Parameters.AddWithValue("@ID_KomitentiView", Convert.ToInt32(row["ID_Komitenti"]));
                    cmd.Parameters.AddWithValue("@ID_NacinPlacanja", Convert.ToInt32(row["ID_NacinPlacanja"]));
                    cmd.Parameters.AddWithValue("@ID_OrganizacionaStrukturaStavkeView", Program.idOrgDeo);
                    cmd.Parameters.AddWithValue("@brur", Convert.ToInt32(row["racun"]));
                    cmd.Parameters.AddWithValue("@DatumDpo", Convert.ToDateTime(datum));
                    cmd.Parameters.AddWithValue("@Neoporezivo", Convert.ToInt32(row["Neoporezivo"]));
                    cmd.Parameters.AddWithValue("@ValutaPl", Convert.ToDateTime(datum).AddDays(8));
                    cmd.Parameters.AddWithValue("@uuser", Program.idkadar);

                    if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");

                    if (Convert.ToInt32(row["ID_Komitenti"]) == pomKom && Convert.ToInt32(row["racun"]) == pomRacun &&
                        Convert.ToString(row["OznakaValute"]) == pomSv)
                    {

                        string insertracunstavke =
                            "INSERT INTO RacunStavke(ID_DokumentaView,ID_ArtikliView,ProdajnaCena,ProcenatRabata,Kolicina,uuser) VALUES (@ID_DokumentaView,@ID_ArtikliView,@ProdajnaCena,@ProcenatRabata,@Kolicina,@uuser)";
                        cmd = new SqlCommand(insertracunstavke);
                        cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
                        pomSv = Convert.ToString(row["OznakaValute"]);
                        cmd.Parameters.AddWithValue("@ID_ArtikliView", Convert.ToInt32(row["ID_Artikal"]));
                        cmd.Parameters.AddWithValue("@ProdajnaCena", Convert.ToDouble(row["Cena"]));
                        cmd.Parameters.AddWithValue("@ProcenatRabata", Convert.ToDouble(row["Popust"]));
                        cmd.Parameters.AddWithValue("@Kolicina", Convert.ToDouble(row["Kolicina"]));
                        cmd.Parameters.AddWithValue("@uuser", Program.idkadar);

                        if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");
                        tag1 = true;
                        continue;


                    }

                    tag1 = false;

                }
                else
                {


                    string insertracunzausluge =
                        "INSERT INTO RacunZaUsluge(ID_DokumentaView,ID_KomitentiView,ID_NacinPlacanja,ID_OrganizacionaStrukturaStavkeView,brur,DatumDpo,ID_SifrarnikValuta,ValutaPl,uuser) VALUES (@ID_DokumentaView,@ID_KomitentiView,@ID_NacinPlacanja,@ID_OrganizacionaStrukturaStavkeView,@brur,@DatumDpo,@ID_SifrarnikValuta,@ValutaPl,@uuser)";
                    cmd = new SqlCommand(insertracunzausluge);
                    cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
                    cmd.Parameters.AddWithValue("@ID_KomitentiView", Convert.ToInt32(row["ID_Komitenti"]));
                    cmd.Parameters.AddWithValue("@ID_NacinPlacanja", 5);
                    cmd.Parameters.AddWithValue("@ID_OrganizacionaStrukturaStavkeView", Program.idOrgDeo);
                    cmd.Parameters.AddWithValue("@brur", Convert.ToInt32(row["racun"]));
                    cmd.Parameters.AddWithValue("@DatumDpo", Convert.ToDateTime(datum));
                    cmd.Parameters.AddWithValue("@ID_SifrarnikValuta", pIdSifVal);
                    cmd.Parameters.AddWithValue("@ValutaPl", Convert.ToDateTime(datum).AddDays(8));
                    cmd.Parameters.AddWithValue("@uuser", Program.idkadar);

                    if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");


                    if (Convert.ToInt32(row["ID_Komitenti"]) == pomKom && Convert.ToInt32(row["racun"]) == pomRacun &&
                        Convert.ToString(row["OznakaValute"]) == pomSv && pomNp == Convert.ToInt32(row["Neoporezivo"]))
                    {

                        string insertracunzauslugestavke =
                            "INSERT INTO RacunZaUslugeStavke(ID_DokumentaView,ID_ArtikliView,FakturnaVrednost,uuser,ID_Poreza,ProcenatRabata,Kolicina) VALUES (@ID_DokumentaView,@ID_ArtikliView,@FakturnaVrednost,@uuser,@ID_Poreza,@ProcenatRabata,@Kolicina)";
                        cmd = new SqlCommand(insertracunzauslugestavke);
                        cmd.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
                        cmd.Parameters.AddWithValue("@ID_ArtikliView", Convert.ToInt32(row["ID_Artikal"]));
                        cmd.Parameters.AddWithValue("@FakturnaVrednost",
                            Convert.ToDouble(row["Kolicina"]) * Convert.ToDouble(row["Cena"]) *
                            (1 - Convert.ToDouble(row["Popust"]) / (double) 100));
                        cmd.Parameters.AddWithValue("@uuser", Program.idkadar);
                        string porez = "Select ID_TarifaPoreza from Artikli where ID_Artikli=" +
                                       Convert.ToInt32(row["ID_Artikal"]);
                        DataTable dtporez = db.ReturnDataTable(porez);
                        DataRow drporez = dtporez.Rows[0];
                        if (dtporez.Rows.Count != 0)
                        {

                            cmd.Parameters.AddWithValue("@ID_Poreza", Convert.ToInt32(drporez["ID_TarifaPoreza"]));
                        }


                        cmd.Parameters.AddWithValue("@ProcenatRabata", Convert.ToDouble(row["Popust"]));
                        cmd.Parameters.AddWithValue("@Kolicina", Convert.ToDouble(row["Kolicina"]));

                        if (db.Comanda(cmd) != "") MessageBox.Show("Greska prilikom inserta!");
                        tag2 = true;
                        continue;


                    }

                    tag2 = false;
                }

                lastOne = true;
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);

                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + pDokument, "IdDokument:" + pidDok);

            }

            if (lastOne == false)
            {
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);

                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + pDokument, "IdDokument:" + pidDok);
            }

        }

        public static void FaktureRestoranaZaOdabraneDatume()
        {

            DateTime temp;

            labela1:
            var datumOd = Microsoft.VisualBasic.Interaction.InputBox("Unesite datum od kojeg prebacujemo fakture",
                "Prebacivanje faktura restorana", DateTime.Now.ToShortDateString());
            if (datumOd == "") return;
            if (!DateTime.TryParse(datumOd, out temp))
            {
                MessageBox.Show("Pogresno unesen datum ponovite!!");
                goto labela1;
            }

            labela2:
            var datumDo = Microsoft.VisualBasic.Interaction.InputBox("Unesite datum do kojeg prebacujemo fakture",
                "Prebacivanje faktura restorana", DateTime.Now.ToShortDateString());

            if (datumDo == "") return;
            if (!DateTime.TryParse(datumDo, out temp))
            {
                MessageBox.Show("Pogresno unesen datum ponovite!!");
                goto labela2;
            }

            var odDat = Convert.ToDateTime(datumOd);
            var doDat = Convert.ToDateTime(datumDo);

            var brDan = Convert.ToInt32((doDat - odDat).TotalDays) + 1;
            if (brDan <= 0)
            {
                MessageBox.Show("Pogresno uneseni datumi ponovite!!");
                goto labela1;
            }

            var db = new DataBaseBroker();
            for (int i = 0; i < brDan; i++)
            {
                var select = "SELECT Datum from Dokumenta where opis like 'Bar%' AND Convert(date,Datum)='" + datumOd +"'";
                var dt = db.ReturnDataTable(select);
                if (dt.Rows.Count != 0)
                {
                    MessageBox.Show("Vec je izvrseno preuzimanje faktura za datum:" + datumOd);
                }
                else
                {
                    PreuzmiFaktureizPrometa(datumOd, "PrometMaloprodaje");
                }

                odDat = Convert.ToDateTime(datumOd).AddDays(1);
                datumOd = odDat.ToShortDateString();
            }

            MessageBox.Show("Zavrseno!!");
        }

        public static void RazduzenjeSirovinaMiniBar()
        {
            string datumOd;
            string datumDo;
            DateTime temp;
            string ProdajnoMesto;
            DateTime odDat;
            DateTime doDat;
            int BrDan;

            ProdajnoMesto =
                Microsoft.VisualBasic.Interaction.InputBox("Unesite prodajno mesto za koje vrsimo razduzenje",
                    "Razduzenje", "Mini bar");
            labela1:
            datumOd = Microsoft.VisualBasic.Interaction.InputBox("Unesite datum od kojeg razduzujemo " + ProdajnoMesto,
                "Razduzenje " + ProdajnoMesto, DateTime.Now.ToShortDateString());
            if (datumOd == "") return;
            if (!DateTime.TryParse(datumOd, out temp))
            {
                MessageBox.Show("Pogresno unesen datum ponovite!!");
                goto labela1;
            }

            labela2:
            datumDo = Microsoft.VisualBasic.Interaction.InputBox("Unesite datum do kojeg razduzujemo " + ProdajnoMesto,
                "Razduzenje " + ProdajnoMesto, DateTime.Now.ToShortDateString());

            if (datumDo == "") return;
            if (!DateTime.TryParse(datumDo, out temp))
            {
                MessageBox.Show("Pogresno unesen datum ponovite!!");
                goto labela2;
            }

            odDat = Convert.ToDateTime(datumOd);
            doDat = Convert.ToDateTime(datumDo);

            BrDan = Convert.ToInt32((doDat - odDat).TotalDays) + 1;
            if (BrDan <= 0)
            {
                MessageBox.Show("Pogresno uneseni datumi ponovite!!");
                goto labela1;
            }


            for (int i = 0; i < BrDan; i++)
            {
                RazduzenjeSirovina(datumOd, "Recepcija", ProdajnoMesto);
                odDat = Convert.ToDateTime(datumOd).AddDays(1);
                datumOd = odDat.ToShortDateString();
            }

            MessageBox.Show("Zavrseno!!");


        }

        private static void RazduzenjeSirovina(string datum, string cegaRazduzenje, string kogaRazduzenje)
        {
            int rr = 0;
            DataBaseBroker db = new DataBaseBroker();
            string rspromet = "";
            if (Program.imeFirme == "Leotar")
            {
                if (cegaRazduzenje.Trim() == "Recepcija")
                {

                    rspromet = "SELECT DISTINCT Datum, ID_SkladisteProizvodnje ,ID_SkladisteProdaje," +
                               " ID_ArtikliView , kolicina, ProdajnaCenaDomVal as StvarnaProdajnaCena, s.NazivSkl,iid " +
                               " FROM PrometRecepcijeTotali as p,skladiste as s " +
                               " WHERE id_skladisteProdaje= s.ID_skladiste and datum='" +
                               Convert.ToDateTime(datum).ToString("yyyy-dd-MM") + "' and NazivskladistaProdaje='" +
                               kogaRazduzenje + "'";
                }
                else
                {
                    rspromet =
                        "SELECT DISTINCT Datum,c.ID_SkladisteProizvodnje ,  p.ID_Skladiste as ID_SkladisteProdaje," +
                        " p.ID_ArtikliView , kolicina, StvarnaProdajnaCena,p.NazivSkl,iid " +
                        " FROM PDVPrometMaloprodajeTotali as p,Cenovnik as c " +
                        " WHERE c.ID_Skladiste=p.ID_Skladiste AND c.ID_ArtikliView=p.ID_ArtikliView AND datum='" +
                        Convert.ToDateTime(datum).ToString("yyyy-dd-MM") + "'";


                }
            }
            else
            {
                if (cegaRazduzenje.Trim() == "Bar")
                {
                    rspromet =
                        "SELECT DISTINCT Datum,ID_SkladisteProizvodnje ,  p.ID_Skladiste as ID_SkladisteProdaje," +
                        " ID_ArtikliView , kolicina, StvarnaProdajnaCena,NazivSkl,iid " +
                        " FROM PDVPrometMaloprodajeTotali as p " +
                        " WHERE datum='" + Convert.ToDateTime(datum).ToString("yyyy-dd-MM") + "'";
                }
                else
                {
                    rspromet = "SELECT DISTINCT Datum, ID_SkladisteProizvodnje ,ID_SkladisteProdaje," +
                               " ID_ArtikliView , kolicina, ProdajnaCenaDomVal as StvarnaProdajnaCena, s.NazivSkl,iid " +
                               " FROM PrometRecepcijeTotali as p,skladiste as s " +
                               " WHERE id_skladisteProdaje= s.ID_skladiste and datum='" +
                               Convert.ToDateTime(datum).ToString("yyyy-dd-MM") + "' and NazivskladistaProdaje='" +
                               kogaRazduzenje + "'";
                }
            }

            DataTable dt = db.ReturnDataTable(rspromet);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoji promet za zadati datum!");
                return;
            }

            string del = "DELETE from PrometPreneseni";
            SqlCommand cmd = new SqlCommand(del);
            db.Comanda(cmd);
            foreach (DataRow row in dt.Rows)
            {
                string sql =
                    "INSERT INTO PrometPreneseni(Datum,ID_Skladiste,ID_SkladistaProizvodnje,NazivSkladista,id_Artikal,Cena,Kolicina) VALUES (@Datum,@ID_Skladiste,@ID_SkladistaProizvodnje,@NazivSkladista,@id_Artikal,@Cena,@Kolicina)";
                cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@Datum", Convert.ToDateTime(row["Datum"]));
                cmd.Parameters.AddWithValue("@ID_Skladiste", Convert.ToInt32(row["ID_SkladisteProdaje"]));
                cmd.Parameters.AddWithValue("@ID_SkladistaProizvodnje",
                    Convert.ToInt32(row["ID_SkladisteProizvodnje"]));
                cmd.Parameters.AddWithValue("@NazivSkladista", Convert.ToString(row["NazivSkl"]));
                cmd.Parameters.AddWithValue("@id_Artikal", Convert.ToInt32(row["ID_ArtikliView"]));
                cmd.Parameters.AddWithValue("@Cena", Convert.ToDouble(row["StvarnaProdajnaCena"]));
                cmd.Parameters.AddWithValue("@Kolicina", Convert.ToDouble(row["Kolicina"]));

                if (db.Comanda(cmd) == "") rr = rr + 1;
                else MessageBox.Show("Greska u upitu!");
            }
            ZapisiPotrosnjuSirovina();
        }
        private static void ZapisiPotrosnjuSirovina()
        {
            DataBaseBroker db = new DataBaseBroker();
            string pDokument;
            int PSkladiste = 0;
            string NSkladiste = "";
            int PPSkladiste = 0;
            int PArtikal = 0;
            double Pcena = 0;
            int UKolicina = 0;
            DateTime pDatum = DateTime.Now;
            string rsd;
            int prolaz = 0;
            bool uslov = false;
            int pidDok = 0;
            string rsp = "Select p.datum,p.ID_Skladiste,p.id_Artikal,p.kolicina,p.Cena , " +
                         " p.id_SkladistaProizvodnje ,sp.nazivSkl as nazivSkladistaProizvodnje,NazivSkladista" +
                         " from PrometPreneseni as p,Skladiste as sp " +
                         " where  p.Id_SkladistaProizvodnje=sp.Id_skladiste" +
                         " ORDER By datum,p.ID_Skladiste,p.ID_SkladistaProizvodnje,p.ID_Artikal ";

            pDokument = "RadniNalog2";
            string BrojDok = "";
            DataTable dt = db.ReturnDataTable(rsp);

            foreach (DataRow row in dt.Rows)
            {
                if (prolaz > 0 && uslov == false) goto DALJE;
                if (uslov == true) uslov = false;
                pDatum = Convert.ToDateTime(row["Datum"]);
                PSkladiste = Convert.ToInt32(row["ID_Skladiste"]);
                NSkladiste = Convert.ToString(row["NazivSkladista"]);
                PPSkladiste = Convert.ToInt32(row["ID_SkladistaProizvodnje"]);
                PArtikal = Convert.ToInt32(row["ID_Artikal"]);
                Pcena = Convert.ToDouble(row["Cena"]);
                UKolicina = 0;
                int ID_DokumentaStablo;
             
                rsd = "select id_Dokumenta from dokumenta as d ,nalogkooperanta as n" +
                      " where ID_dokumenta=ID_DokumentaView and id_skladisteiz =" + PPSkladiste + " and datum='" +
                      Convert.ToDateTime(pDatum).ToString("yyyy-dd-MM") + "'";
                DataTable dt1 = db.ReturnDataTable(rsd);
                DataRow dr1 = dt1.Rows[0];
                if (dt1.Rows.Count == 0)
                {
                    string dokumentastablo = "SELECT * FROM DokumentaStablo where Naziv='" + pDokument + "'";
                    DataTable dtdoksta = db.ReturnDataTable(dokumentastablo);
                    DataRow dr = dtdoksta.Rows[0];
                    if (dtdoksta.Rows.Count != 0)
                    {
                        ID_DokumentaStablo = Convert.ToInt32(dr["ID_DokumentaStablo"]);
                    }
                    else
                    {
                        MessageBox.Show("Nije registrovan dokument promet maloprodaje!");
                        return;
                    }

                    int rbr = 0;
                    int IdDokView;
                    
                    string opis = NSkladiste + "-" + Convert.ToString(row["NazivSkladistaProizvodnje"]);
                    clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                    IdDokView = os.UpisiDokument(ref BrojDok, opis, ID_DokumentaStablo, pDatum.ToString());                    
                    prolaz = prolaz + 1;                    
                    pidDok = IdDokView;  
                }
                else
                {
                    prolaz = prolaz + 1;
                    pidDok = Convert.ToInt32(dr1["Id_Dokumenta"]);
                    string delete = "Delete from nalogkooperanta where ID_DokumentaView=" + pidDok;
                    SqlCommand cmd1 = new SqlCommand(delete);
                    db.Comanda(cmd1);
                    delete = "Delete from nalogkooperantaStavke where ID_DokumentaView=" + pidDok;
                    cmd1 = new SqlCommand(delete);
                    db.Comanda(cmd1);
                    delete = "Delete from NalogKooperantaSirovineStavke where ID_DokumentaView=" + pidDok;
                    cmd1 = new SqlCommand(delete);
                    db.Comanda(cmd1);
                    delete = "Delete from NalogKooperantaSirovineUkupnoStavke where ID_DokumentaView=" + pidDok;
                    cmd1 = new SqlCommand(delete);
                    db.Comanda(cmd1);
                    delete = "Delete from RadniNalog2Totali where ID_RadniNalog2Totali=" + pidDok;
                    cmd1 = new SqlCommand(delete);
                    db.Comanda(cmd1);
                }

                // zaglavlje 
                string insertnk =
                    "INSERT INTO NalogKooperanta(ID_DokumentaView,ID_Skladisteu,ID_Skladisteiz,uuser) VALUES (@ID_DokumentaView,@ID_Skladisteu,@ID_Skladisteiz,@uuser)";
                SqlCommand cmd2 = new SqlCommand(insertnk);
                cmd2.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
                cmd2.Parameters.AddWithValue("@ID_Skladisteu", PSkladiste);
                cmd2.Parameters.AddWithValue("@ID_Skladisteiz", PPSkladiste);
                cmd2.Parameters.AddWithValue("@uuser", Program.idkadar);
                if (db.Comanda(cmd2) != "") MessageBox.Show("Greska prilikom inserta!");
                DALJE:
                if (pDatum == Convert.ToDateTime(row["Datum"]) && PSkladiste == Convert.ToInt32(row["ID_Skladiste"]) &&
                    PPSkladiste == Convert.ToInt32(row["ID_SkladistaProizvodnje"]))
                {
                    if (PArtikal != Convert.ToInt32(row["ID_Artikal"]) || Pcena != Convert.ToDouble(row["Cena"]))
                    {
                        ZapisiSlogPotrosnje(PArtikal, pidDok, UKolicina, Pcena, Program.idkadar);
                        UKolicina = 0;
                        PArtikal = Convert.ToInt32(row["Id_Artikal"]);
                        Pcena = Convert.ToDouble(row["Cena"]);
                    }
                    UKolicina = UKolicina + Convert.ToInt32(row["Kolicina"]);
                    continue;
                }
                else
                {
                    ZapisiSlogPotrosnje(PArtikal, pidDok, UKolicina, Pcena, Program.idkadar);
                    DoradiPotrosnju(pidDok, pDatum);
                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);
                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:RadniNalog2", "IdDokument:" + pidDok);
                    db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:" + pidDok);
                    uslov = true;
                }
            }

            if (uslov == false)
            {
                ZapisiSlogPotrosnje(PArtikal, pidDok, UKolicina, Pcena, Program.idkadar);
                DoradiPotrosnju(pidDok, pDatum);
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:RadniNalog2", "IdDokument:" + pidDok);
                db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:" + pidDok);
            }
        }

        private static void ZapisiSlogPotrosnje(int PArtikal, int pidDok, int UKolicina, double Pcena, int PRadnik)

        {
            DataBaseBroker db = new DataBaseBroker();
            string kojiNormativ = "Select ID_DokumentaView as IIDD from recepti where ID_ProizvodView=" + PArtikal;
            DataTable dt = db.ReturnDataTable(kojiNormativ);
            if (dt.Rows.Count == 0) return;
            DataRow dr = dt.Rows[0];

            string insertnk =
                "INSERT INTO NalogKooperantaStavke(ID_DokumentaView,Kolicina,ID_Normativview,ID_SirovinaView,ProsecnaNabavnaCena,uuser) VALUES (@ID_DokumentaView,@Kolicina,@ID_Normativview,@ID_SirovinaView,@ProsecnaNabavnaCena,@uuser)";
            SqlCommand cmd2 = new SqlCommand(insertnk);
            cmd2.Parameters.AddWithValue("@ID_DokumentaView", pidDok);
            cmd2.Parameters.AddWithValue("@Kolicina", UKolicina);
            cmd2.Parameters.AddWithValue("@ID_Normativview", Convert.ToInt32(dr["IIDD"]));
            cmd2.Parameters.AddWithValue("@ID_SirovinaView", PArtikal);
            cmd2.Parameters.AddWithValue("@ProsecnaNabavnaCena", Pcena);
            cmd2.Parameters.AddWithValue("@uuser", PRadnik);
            if (db.Comanda(cmd2) != "") MessageBox.Show("Greska prilikom inserta!");
        }
        private static void DoradiPotrosnju(int pidDok, DateTime pDatum)
        {
            DataBaseBroker db = new DataBaseBroker();
            string delete = "delete from NalogKooperantaSirovineStavke where id_Dokumentaview=" + pidDok;
            SqlCommand cmd1 = new SqlCommand(delete);
            db.Comanda(cmd1);
            delete = "delete from NalogKooperantaSirovineUkupnoStavke where id_Dokumentaview=" + pidDok;
            cmd1 = new SqlCommand(delete);
            db.Comanda(cmd1);

            string upit =
                " insert into NalogKooperantaSirovineStavke(id_DokumentaView, ID_SirovinaView, Kolicina, ID_NormativView)" +
                " select " + pidDok + " as id_DokumentaView, r.ID_SirovinaView," +
                " (r.Kolicina*n.kolicina) as Kolicina, n.ID_NormativView" +
                " from NalogKooperantaStavke as n, ReceptiStavke as r,Recepti as rr " +
                " where n.ID_DokumentaView=" + pidDok +
                " and n.id_normativview=r.id_DokumentaView" +
                " and n.id_normativview=rr.id_DokumentaView";
            cmd1 = new SqlCommand(upit);
            db.Comanda(cmd1);

            upit = " update NalogKooperantaSirovineStavke set ProsecnaNabavnaCena=c.ProsecnaNabavnaCena " +
                   " from ceneartikalanaskladistimapred as c,NalogKooperanta as r," +
                   " NalogKooperantaSirovineStavke as rs, dokumenta as d " +
                   " where  r.ID_DokumentaView=d.ID_Dokumenta And" +
                   " r.ID_DokumentaView=rs.ID_DokumentaView And " +
                   " c.ID_ArtikliView=ID_SirovinaView And " +
                   " c.Id_skladiste=r.ID_SkladisteIz and" +
                   " c.Datum =(SELECT MAX(datum) from CeneArtikalanaskladistimaPred " +
                   " Where CeneArtikalanaskladistimaPred.ID_ArtikliView = c.ID_ArtikliView " +
                   " AND (Skl = C.Skl) AND datum <= d.Datum AND CeneArtikalanaskladistimaPred.ID_DokumentaView<>" +
                   pidDok + ") " +
                   " and ID_CeneArtikalaNaSkladistimapred=(SELECT max(ID_CeneArtikalaNaSkladistimapred) " +
                   " FROM  CeneArtikalanaskladistimaPred WHERE CeneArtikalanaskladistimaPred.ID_ArtikliView = C.ID_ArtikliView " +
                   " AND (Skl = C.Skl) AND datum = c.Datum ) AND " +
                   " r.ID_DokumentaView=" + pidDok;
            cmd1 = new SqlCommand(upit);
            db.Comanda(cmd1);

            upit =
                " insert into NalogKooperantaSirovineUkupnoStavke(id_DokumentaView, ID_SirovinaView,ProsecnaNabavnaCena ,Kolicina)" +
                " select " + pidDok + " as id_DokumentaView, ID_SirovinaView,ProsecnaNabavnaCena ,sum(Kolicina)" +
                " from NalogKooperantaSirovineStavke as n where n.ID_DokumentaView=" + pidDok +
                " group by id_DokumentaView, ID_SirovinaView,ProsecnaNabavnaCena";
            cmd1 = new SqlCommand(upit);
            db.Comanda(cmd1);
            string update = "update NalogKooperantaSirovineStavke set Uuser=" + Program.idkadar +
                            " where Id_DokumentaView=" + pidDok;
            cmd1 = new SqlCommand(update);
            db.Comanda(cmd1);
            update = "update NalogKooperantaSirovineUkupnoStavke set Uuser=" + Program.idkadar +
                     " where Id_DokumentaView=" + pidDok;
        }
        public static void RazduzenjeSirovinaZaOdabraniIntervalDatuma()
        {
            string datumOd;
            string datumDo;
            DateTime temp;
            DateTime odDat;
            DateTime doDat;
            int BrDan;

            labela1:
            datumOd = Microsoft.VisualBasic.Interaction.InputBox("Unesite datum od kojeg razduzujemo sirovine",
                "Razduzenje sirovina", DateTime.Now.ToShortDateString());
            if (datumOd == "") return;
            if (!DateTime.TryParse(datumOd, out temp))
            {
                MessageBox.Show("Pogresno unesen datum ponovite!!");
                goto labela1;
            }

            labela2:
            datumDo = Microsoft.VisualBasic.Interaction.InputBox("Unesite datum do kojeg razduzujemo sirovine",
                "Razduzenje sirovina", DateTime.Now.ToShortDateString());

            if (datumDo == "") return;
            if (!DateTime.TryParse(datumDo, out temp))
            {
                MessageBox.Show("Pogresno unesen datum ponovite!!");
                goto labela2;
            }

            odDat = Convert.ToDateTime(datumOd);
            doDat = Convert.ToDateTime(datumDo);

            BrDan = Convert.ToInt32((doDat - odDat).TotalDays) + 1;
            if (BrDan <= 0)
            {
                MessageBox.Show("Pogresno uneseni datumi ponovite!!");
                goto labela1;
            }

            for (int i = 0; i < BrDan; i++)
            {
                RazduzenjeSirovina(datumOd, "Bar", "");
                odDat = Convert.ToDateTime(datumOd).AddDays(1);
                datumOd = odDat.ToShortDateString();
            }

            MessageBox.Show("Zavrseno!!");
        }

        public static void PopisNaDan(string datumStanja, int kojeSkladiste)
        {
            var vlasnik = "";
            DateTime datumPopisa;
            var datumOd = "01.01." + Convert.ToDateTime(datumStanja).Year;
            var iddokument = Convert.ToInt32(((frmChield) Program.Parent.ActiveMdiChild).iddokumenta);
            var db = new DataBaseBroker();

            const string message = "Da li zelite obrisati postojece stavke?";
            const string title = "Rekapitulacija popisa bez lot-a ";

            var buttons = MessageBoxButtons.YesNo;
            var result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                const string del = "DELETE FROM RacunStavke WHERE ID_DokumentaView=@param0";
                var cmd2 = new SqlCommand(del);
                cmd2.Parameters.AddWithValue("@param0", iddokument);
                if (db.Comanda(cmd2) != "") MessageBox.Show("Greska prilikom brisanja!");
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + iddokument);
            }

            const string rsPopis =
                "Select DISTINCT id_PopisBezLotaTotali as IdPred,datum,Brdok, ID_VlasnikRobe from PopisBezLotaTotali where id_Skladiste = @param0 And YEAR(datum)= @param1 Order by ID_PopisBezLotaTotali";
            var dt = db.ParamsQueryDT(rsPopis, kojeSkladiste, Convert.ToDateTime(datumStanja).Year);
            var dr = dt.Rows[0];
            const string rsRacun = "Select * from Racun where ID_DokumentaView= @param0";
            var dt1 = db.ParamsQueryDT(rsRacun, iddokument);
            if (dt1.Rows.Count == 0)
            {
                var ds =
                    db.ParamsQueryDS(
                        "INSERT INTO Racun (ID_DokumentaView,ID_Skladiste,ID_VlasnikRobe) VALUES(@param0,@param1,@param2)",
                        iddokument, kojeSkladiste, Convert.ToInt32(dr["ID_VlasnikRobe"]));
            }
            else
            {
                const string update = "Update Racun Set ID_VlasnikRobe=@param0 where ID_DokumentaView=@param1";
                var ds = db.ParamsQueryDS(update, Convert.ToInt32(dr["ID_VlasnikRobe"]), iddokument);
            }

            foreach (DataRow row in dt.Rows)
            {
                datumPopisa = Convert.ToDateTime(row["Datum"]);
                vlasnik = Convert.ToString(row["ID_VlasnikRobe"]);
                var idPred = Convert.ToInt32(row["IdPred"]);
                const string insert =
                    " Insert into RacunStavke(ID_DokumentaView, ID_ArtikliView,Kolicina,DatumPopisa)" +
                    " select @param0 ,ID_ArtikliView,Kolicina,@param1" +
                    " from PopisBezLotaTotali " +
                    " where id_PopisBezLotaTotali = @param2 " +
                    " order by ID_ArtikliView ";
                var ss1 = db.ParamsQueryDS(insert, iddokument, datumPopisa, idPred);
            }
            const string sel = "SELECT sum(Ulaz-Izlaz)as Kol,Sum(VrednostNab) as NabavnaVrednost, ID_ArtikliView " +
                               " from CeneArtikalaNaSkladistimaPred Where   datum <= @param0 and " +
                               " datum >@param1 and skl = @param2" +
                               " and ID_Vlasnika=@param3 " +
                               " group by ID_ArtikliView " +
                               " order by ID_ArtikliView ";
            var rsstanje = db.ParamsQueryDT(sel, Convert.ToDateTime(datumStanja), Convert.ToDateTime(datumOd),
                kojeSkladiste, vlasnik);
            foreach (DataRow row in rsstanje.Rows)
            {
                const string racun = "Select ID_ArtikliView,Kolicina " +
                                     " from RacunStavke Where  ID_DokumentaView = @param0" +
                                     " and ID_ArtikliView = @param1";
                var dt3 = db.ParamsQueryDT(racun, iddokument, Convert.ToInt32(row["ID_ArtikliView"]));
                if (dt3.Rows.Count != 0)
                {
                    const string update = "Update RacunStavke set Primljeno=@param0" +
                                          "WHERE ID_dokumentaView=@param1 and ID_ArtikliView=@param2";
                    var ds = db.ParamsQueryDS(update, Convert.ToInt32(row["Kol"]), iddokument,
                        Convert.ToInt32(row["ID_ArtikliView"]));
                }
                else
                {
                    const string rspopis =
                        "Select d.datum as DatumPopisa from racunStavke as rs,dokumenta as d,Racun as r where r.ID_DokumentaView=rs.ID_DokumentaView And d.ID_Dokumenta=r.ID_DokumentaView and Year(d.datum)=@param0" +
                        " AND d.ID_DokumentaStablo=151 AND r.ID_Skladiste = @param1 AND rs.ID_ArtikliView=@param2";
                    var dtrspopis = db.ParamsQueryDT(rspopis, Convert.ToDateTime(datumStanja).Year, kojeSkladiste,
                        Convert.ToInt32(row["ID_ArtikliView"]));
                    datumPopisa = dtrspopis.Rows.Count != 0
                        ? Convert.ToDateTime(dtrspopis.Rows[0]["DatumPopisa"])
                        : Convert.ToDateTime(datumStanja);

                    const string insert =
                        "Insert into RacunStavke(ID_DokumentaView, ID_ArtikliView,Primljeno, DatumPopisa)" +
                        "values(@param0,@param1,@param2,@param3)";
                    var ds = db.ParamsQueryDS(insert, iddokument,
                        Convert.ToInt32(dtrspopis.Rows[0]["ID_ArtikliView"]), Convert.ToInt32(dtrspopis.Rows[0]["Kol"]),
                        datumPopisa);
                }
            }
            const string rskol = "Select  SUM(VrednostNab) as vred,SUM(Ulaz - Izlaz)  AS kolicina, ID_ArtikliView" +
                                 " from CeneArtikalaNaSkladistimaPred Where   datum <= @param0" +
                                 " and datum >=@param1 and skl = @param2" +
                                 " and ID_Vlasnika=@param3" +
                                 " group by ID_ArtikliView  " +
                                 " having  sum(ulaz-izlaz)<>0 " +
                                 " order by ID_ArtikliView";
            var dt2 = db.ParamsQueryDT(rskol, datumStanja, datumOd, kojeSkladiste, vlasnik);
            foreach (DataRow row in dt2.Rows)
            {
                const string update = "update RacunStavke set NabavnaCena=@param0" +
                                      " from Racun as r, RacunStavke as rs " +
                                      " where rs.ID_ArtikliView =@param1" +
                                      " and rs.id_DokumentaView=r.ID_DokumentaView " +
                                      " and r.ID_Skladiste=@param2" +
                                      " And r.ID_DokumentaView=@param3";
                var ds = db.ParamsQueryDS(update, Convert.ToDouble(row["Vred"]) / Convert.ToDouble(row["Kolicina"]),
                    Convert.ToInt32(row["ID_ArtikliView"]), kojeSkladiste, iddokument);
            }
            const string update1 = "update RacunStavke set RacunStavke.Ulaz= valsum " +
                                   " From RacunStavke " +
                                   " Inner Join " +
                                   " (select sum(Ulaz) as Valsum,ID_ArtikliView " +
                                   " From CeneArtikalaNaSkladistimaPred " +
                                   " where   datum <=  @param0  and datum > @param1 and skl = @param2" +
                                   " and id_Vlasnika=@param3" +
                                   " group by  ID_ArtikliView) as ulaz " +
                                   " on ulaz.ID_ArtikliView=RacunStavke.ID_ArtikliView " +
                                   " and RacunStavke.id_DokumentaView =@param4";
            var ds1 = db.ParamsQueryDS(update1, datumStanja, datumOd, kojeSkladiste, vlasnik, iddokument);
        }
        public static void PopisPoLotuNaDan(string datumStanja, int kojeSkladiste)
        {
            string vlasnik = "";
            var datumPopisa = new DateTime();
            string datumOd = "01.01." + Convert.ToDateTime(datumStanja).Year;
            var iddokument = Convert.ToInt32(((Bankom.frmChield) Program.Parent.ActiveMdiChild).iddokumenta);

            DataBaseBroker db = new DataBaseBroker();

            const string message = "Da li zelite obrisati postojece stavke?";
            const string title = "Rekapitulacija popisa po lot-u";

            const MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            var result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                const string del = "DELETE FROM RacunStavke WHERE ID_DokumentaView=@param0";
                var cmd2 = new SqlCommand(del);
                cmd2.Parameters.AddWithValue("@param0", iddokument);
                if (db.Comanda(cmd2) != "") MessageBox.Show("Greska prilikom inserta!");
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + iddokument);
            }
            var rsPopis =
                "SELECT DISTINCT ID_PopisTotali as IdPred,datum,Brdok, ID_VlasnikRobe from PopisTotali where id_Skladiste = @param0  And YEAR(datum)= @param1 Order by ID_PopisTotali";
            var dtrspopis = db.ParamsQueryDT(rsPopis, kojeSkladiste, Convert.ToDateTime(datumStanja).Year);
            var rsRacun = "SELECT * from Racun where ID_DokumentaView=@param0";
            var dtrsracun = db.ParamsQueryDT(rsRacun, iddokument);

            if (dtrsracun.Rows.Count == 0)
            {
                var ds =
                    db.ParamsQueryDS(
                        "Insert Into Racun (ID_DokumentaView,ID_Skladiste,ID_VlasnikRobe) Values(@param0,@param1,@param2)",
                        iddokument, kojeSkladiste, dtrspopis.Rows[0]["ID_VlasnikRobe"]);
            }
            else
            {
                var ds =
                    db.ParamsQueryDS("Update Racun Set ID_VlasnikRobe = @param0 where ID_DokumentaView= @param1",
                        Convert.ToInt32(dtrspopis.Rows[0]["ID_VlasnikRobe"]), iddokument);
            }
            foreach (DataRow row in dtrspopis.Rows)
            {
                datumPopisa = Convert.ToDateTime(row["Datum"]);
                vlasnik = Convert.ToString(row["ID_VlasnikRobe"]);
                var idPred = Convert.ToInt32(row["IdPred"]);
                const string rsInsert =
                    " INSERT INTO RacunStavke(ID_DokumentaView,ID_ArtikliView,ID_LotView,ID_MagacinskaPolja,Kolicina,DatumPopisa)" +
                    " select @param0,ID_ArtikliView,ID_LotView,ID_MagacinskaPolja,Kolicina,@param1" +
                    " from PopisTotali " +
                    " where ID_PopisTotali = @param2" +
                    " order by ID_ArtikliView,ID_LotView,ID_MagacinskaPolja ";
                var ss = db.ParamsQueryDS(rsInsert, iddokument, datumPopisa, idPred);
            }
            var sel =
                "SELECT sum(Ulaz-Izlaz)as Kol,Sum(VrednostNab) as NabavnaVrednost, ID_ArtikliView,ID_LotView,ID_MagacinskaPolja " +
                " from StanjeRobeNaSkl Where   datum <= @param0 and " +
                " datum >= @param1 and skl = @param2" +
                " AND ID_LotView >1 and ID_Vlasnika=@param3 " +
                " group by ID_ArtikliView,ID_LotView,ID_MagacinskaPolja " +
                " having  sum(ulaz-izlaz)<>0 " +
                " order by ID_ArtikliView,ID_LotView,ID_MagacinskaPolja";
            var rsstanje = db.ParamsQueryDT(sel, Convert.ToDateTime(datumStanja), Convert.ToDateTime(datumPopisa),
                kojeSkladiste, Convert.ToInt32(vlasnik));
            if (rsstanje.Rows.Count != 0)
            {
                foreach (DataRow row in rsstanje.Rows)
                {
                    rsRacun = "Select ID_ArtikliView,ID_LotView,ID_MagacinskaPolja,Kolicina " +
                              " from RacunStavke Where  ID_DokumentaView = @param0" +
                              " and ID_LotView= @param1 " +
                              " and ID_MagacinskaPolja= @param2";
                    dtrsracun = db.ParamsQueryDT(rsRacun, iddokument, Convert.ToInt32(row["ID_LotView"]),
                        Convert.ToInt32(row["ID_MagacinskaPolja"]));
                    if (dtrsracun.Rows.Count != 0)
                    {
                        var ds = db.ParamsQueryDS("Update RacunStavke set Primljeno= @param0 " +
                                                  " WHERE ID_dokumentaView = @param1 and ID_LotView= @param2",
                            Convert.ToDecimal(row["kol"]), iddokument, Convert.ToInt32(row["ID_LotView"]));
                    }
                    else
                    {
                        rsPopis =
                            "Select d.datum as DatumPopisa from racunStavke as rs,dokumenta as d,Racun as r where r.ID_DokumentaView=rs.ID_DokumentaView And d.ID_Dokumenta=r.ID_DokumentaView and Year(d.datum)= @param0 " +
                            " AND d.ID_DokumentaStablo=150 AND r.ID_Skladiste= @param1 AND rs.ID_ArtikliView= @param2";
                        dtrspopis = db.ParamsQueryDT(rsPopis, Convert.ToDateTime(datumStanja).Year, kojeSkladiste,
                            Convert.ToInt32(row["ID_ArtikliView"]));
                        datumPopisa = dtrspopis.Rows.Count != 0
                            ? Convert.ToDateTime(dtrspopis.Rows[0]["DatumPopisa"])
                            : Convert.ToDateTime(datumStanja);

                        DataSet ds = db.ParamsQueryDS(
                            "Insert into RacunStavke (  ID_DokumentaView, ID_ArtikliView, ID_LotView,ID_MagacinskaPolja,Primljeno,DatumPopisa)" +
                            " values(@param0,@param1,@param2,@param3,@param4,@param5)", iddokument,
                            Convert.ToInt32(row["ID_ArtikliView"]), Convert.ToInt32(row["ID_Lotview"]),
                            Convert.ToInt32("ID_MagacinskaPolja"), Convert.ToDecimal(row["kol"]),
                            Convert.ToDateTime(datumPopisa));
                    }
                }
            }
            rsRacun = "Select ID_RacunStavke,ID_ArtikliView,ID_LotView,ID_MagacinskaPolja,Kolicina,DatumPopisa " +
                      " from RacunStavke Where  ID_DokumentaView = @param0";
            dtrsracun = db.ParamsQueryDT(rsRacun, iddokument);
            foreach (DataRow row in dtrsracun.Rows)
            {
                datumPopisa = Convert.ToDateTime(row["DatumPopisa"]);
                sel =
                    "Select sum(Ulaz-Izlaz)as Kol,Sum(VrednostNab) as NabavnaVrednost, ID_ArtikliView,ID_LotView,ID_MagacinskaPolja " +
                    " from StanjeRobeNaSkl Where   datum <= @param0 and " +
                    " datum > @param1 and skl = @param2" +
                    " and id_dokumentaview not in (Select ID_DokumentaTotali  from DokumentaTotali where ((ID_DokumentaStablo=506 or ID_DokumentaStablo=505 ) " +
                    " and Datum= @param3) And Opis like'popis%') " +
                    " and ID_Vlasnika= @param4 and ID_LotView= @param5 and ID_ArtikliView= @param6 " +
                    " and ID_MagacinskaPolja=@param7 " +
                    " group by ID_ArtikliView,id_lotview, id_magacinskapolja  " +
                    " having  sum(ulaz-izlaz)<>0 ";
                DataTable dtrsstanje = db.ParamsQueryDT(sel, Convert.ToDateTime(datumStanja),
                    Convert.ToDateTime(datumPopisa), kojeSkladiste, Convert.ToDateTime(datumStanja),
                    Convert.ToInt32(vlasnik), Convert.ToInt32(row["ID_LotView"]),
                    Convert.ToInt32(row["ID_ArtikliView"]), Convert.ToInt32(row["ID_MagacinskaPolja"]));
                if (dtrsstanje.Rows.Count != 0)
                {
                    row.SetField("Kolicina",
                        Convert.ToDecimal(row["Kolicina"]) + Convert.ToDecimal(dtrsstanje.Rows[0]["kol"]));
                }
            }
            string rskol = "Select  SUM(VrednostNab) as vred,SUM(Ulaz - Izlaz)  AS kolicina, ID_ArtikliView" +
                           " from CeneArtikalaNaSkladistimaPred Where   datum <= @param0" +
                           " and datum >=@param1 and skl = @param2" +
                           " and ID_Vlasnika=@param3" +
                           " group by ID_ArtikliView  " +
                           " having  sum(ulaz-izlaz)<>0 " +
                           " order by ID_ArtikliView";
            DataTable dt2 = db.ParamsQueryDT(rskol, Convert.ToDateTime(datumStanja), Convert.ToDateTime(datumOd),
                kojeSkladiste, Convert.ToInt32(vlasnik));

            foreach (DataRow row in dt2.Rows)
            {
                string update = "update RacunStavke set NabavnaCena=@param0" +
                                " from  RacunStavke as rs " +
                                " where rs.ID_ArtikliView =@param1" +
                                " and rs.id_DokumentaView= @param2 ";

                DataSet ds = db.ParamsQueryDS(update, Convert.ToDouble(row["Vred"]) / Convert.ToDouble(row["Kolicina"]),
                    Convert.ToInt32(row["ID_ArtikliView"]), iddokument);

            }
            string update1 = "update RacunStavke set RacunStavke.Ulaz = valsum " +
                             " From RacunStavke " +
                             " Inner Join " +
                             " (select sum(Ulaz) as Valsum,ID_ArtikliView " +
                             " From CeneArtikalaNaSkladistimaPred " +
                             " where   datum <=  @param0  and datum >= @param1  and skl = @param2" +
                             " and id_Vlasnika=@param3" +
                             " group by  ID_ArtikliView) as ulaz " +
                             " on ulaz.ID_ArtikliView=RacunStavke.ID_ArtikliView " +
                             " and RacunStavke.id_DokumentaView =@param4";
            DataSet ds1 = db.ParamsQueryDS(update1, Convert.ToDateTime(datumStanja), Convert.ToDateTime(datumOd),
                kojeSkladiste, Convert.ToInt32(vlasnik), iddokument);
        }
        public static void PreuzimanjeRateKredita()
        {
            var db = new DataBaseBroker();
            if (UnosMesecaIGodineZaKojiPlacamoKredit(out var pdat)) return;

            const string pdokument = "PlacanjeKredita";
            const string pdokumentf = "ObracunKredita";
            int idDokView = 0;
            if (UnosDokumentaKredita(out var brDokKredita)) return;

            if (ProveraDaLiJeRegistrovanDokument(db, pdokumentf, brDokKredita, out var dtrsu)) return;

            var idDokViewf = Convert.ToInt32(dtrsu.Rows[0]["ID_DokumentaTotali"]);

            if (UnosDatumaPlacanjaKredita(out var pDatum)) return;

            var rsu =
                " Select DokumentaTotali.Brdok,DokumentaTotali.datum,ID_DokumentaStablo,ID_DokumentaTotali from DokumentaTotali,FinansijskiInterniNalogTotali ,OrganizacionaStrukturaStavkeView " +
                " where Dokument=@param0 and DokumentaTotali.Datum=@param1  and id_OrganizacionaStrukturaView =" +
                " ID_OrganizacionaStrukturaStavkeView and ID_DokumentaTotali=ID_FinansijskiInterniNalogTotali And ID_OrganizacionaStrukturaStablo=@param2" +
                " and PozivNaBrDok = @param3";
            dtrsu = db.ParamsQueryDT(rsu, pdokument, pDatum, Program.idFirme, brDokKredita);

            if (dtrsu.Rows.Count == 0)
            {
                if (PronadjiIdDokumentaStablo(db, pdokument, out var idDokumentaStablo)) return;                
                string brojDok = "";
                clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
                idDokView = os.UpisiDokument(ref brojDok, "Placanje rate kredita " + brDokKredita, idDokumentaStablo, pDatum);                
            }
            var mPoruka = "";
            if (PlacanjeKredita(pdat, pDatum, idDokViewf, idDokView, ref mPoruka) == false) MessageBox.Show(mPoruka);
        }
        private static bool UnosDatumaPlacanjaKredita(out string pDatum)
        {
            pDatum = Prompt.ShowDialog(DateTime.Now.ToShortDateString(), "Placanje rate kredita",
                "Unesite datum placanja kredita ");
            if (pDatum == "") return true;
            if (DateTime.TryParse(pDatum, out var temp)) return false;
            MessageBox.Show("Pogresno unesen datum ponovite!!");
            return true;
        }
        private static bool ProveraDaLiJeRegistrovanDokument(DataBaseBroker db, string pdokumentf, string brDokKredita,
            out DataTable dtrsu)
        {
            const string rsu =
                "Select Brdok,datum,ID_DokumentaStablo,ID_DokumentaTotali from DokumentaTotali,OrganizacionaStrukturaStavkeView " +
                " where Dokument=@param0 " + " AND BrDok=@param1 AND id_OrganizacionaStrukturaView=" +
                " ID_OrganizacionaStrukturaStavkeView  And ID_OrganizacionaStrukturaStablo=@param2 ";
            dtrsu = db.ParamsQueryDT(rsu, pdokumentf, brDokKredita, Program.idFirme);

            if (dtrsu.Rows.Count != 0) return false;
            MessageBox.Show("Nije registrovan dokument " + pdokumentf + " broj " + brDokKredita);
            return true;
        }
        private static bool UnosDokumentaKredita(out string brDokKredita)
        {
            brDokKredita = Prompt.ShowDialog(null, "Placanje rate kredita", "Unesite dokument kredita");
            if (brDokKredita != "") return false;
            MessageBox.Show("Niste unijeli broj dokumenta kredita!");
            return true;
        }
        private static bool UnosMesecaIGodineZaKojiPlacamoKredit(out string pdat)
        {
            pdat = Prompt.ShowDialog(
                DateTime.Now.ToShortDateString().Substring(3, 2) + DateTime.Now.ToShortDateString().Substring(6, 2),
                "Placanje rate kredita", "Unesite mesec i godinu za koju placamo kredit");
            if (pdat == "")
            {
                MessageBox.Show("Niste unijeli mesec i godinu za koji placamo ratu kredita!");
                return true;
            }

            if (pdat.Length == 4) return false;
            MessageBox.Show("Niste unijeli mjesec i godinu u trazenom formatu!");
            return true;
        }
        private static bool PlacanjeKredita(string mesgod, string datum, int idDokViewf, int idDokView,
            ref string mPoruka)
        {
            const string poruka = "";
            var db = new DataBaseBroker();
            var dtrsu = db.ParamsQueryDT(
                "SELECT Id_DokumentaView FROM FinansijskiInterniNalogStavke WHERE ID_DokumentaView =@param0",
                idDokView);
            if (dtrsu.Rows.Count != 0)
            {
                const MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                var result = MessageBox.Show("Da li brisete postojece promene?", "", buttons);
                if (result == DialogResult.Yes)
                {
                    db.ParamsQueryDS("Delete from FinansijskiInterniNalogStavke where ID_DokumentaView =@param0",
                        idDokView);
                    db.ParamsQueryDS("Delete from FinansijskiInterniNalog where ID_DokumentaView =@param0", idDokView);
                }
                else
                {
                    return true;
                }
            }
            if (!PopuniPlacanjeKredita(idDokView, mesgod, datum, idDokViewf)) goto PrinudniIzlaz;
            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:FinansijskiInterniNalog",
                "IdDokument: " + idDokView);
            MessageBox.Show("Formiran nalog za placanje kredita");
            return true;

            PrinudniIzlaz:
            mPoruka = poruka;
            return false;
        }
        private static bool PopuniPlacanjeKredita(int fidDok, string mesec, string datum, int pidDok)
        {
            var db = new DataBaseBroker();

            db.ParamsQueryDS(" Insert into FinansijskiInterniNalog ( ID_DokumentaView,ID_PozivNaBrDok,UUser)" +
                             " Select @param0 as ID_DokumentaView, @param1,@param2", fidDok, pidDok, Program.idkadar);
            const string rsu =
                "Select Id_KomitentiView, ID_Analitika, ID_SifrarnikValuta , DatumValute, Anuitet, Otplata, Kamata , OznVal, DomVal, KomitentiTotali.OdakleJeKomitent " +
                " from ObracunKreditaTotali,KomitentiTotali where ID_KomitentiView =ID_KomitentiTotali and ID_ObracunKreditaTotali=@param0" +
                " and month(datumvalute) = @param1 and year(datumvalute) =@param2 " +
                " and datumvalute = @param3";

            var dt = db.ParamsQueryDT(rsu, pidDok, mesec.Substring(0, 2), mesec.Substring(2, 4), datum);
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToDecimal(dt.Rows[0]["Otplata"]) != 0)
                {
                    var nazi =
                        " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,ID_Komitent,ID_SifrarnikValuta,Iznos,Iznos2,UUser)" +
                        " values (@param0,@param1,@param2,@param3,@param4,@param5,@param6)";
                    db.ParamsQueryDS(nazi, fidDok, Convert.ToInt32(dt.Rows[0]["ID_Analitika"]),
                        Convert.ToInt32(dt.Rows[0]["ID_KomitentiView"]),
                        Convert.ToInt32(dt.Rows[0]["ID_SifrarnikValuta"]), Convert.ToDecimal(dt.Rows[0]["Otplata"]), 0,
                        Program.idkadar);
                    nazi =
                        " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,ID_Komitent,ID_SifrarnikValuta,Iznos,Iznos2,UUser)" +
                        " values (@param0,@param1,@param2,@param3,@param4,@param5,@param6)";
                    db.ParamsQueryDS(nazi, fidDok, Convert.ToInt32(dt.Rows[0]["ID_Analitika"]),
                        Convert.ToInt32(dt.Rows[0]["ID_KomitentiView"]),
                        Convert.ToInt32(dt.Rows[0]["ID_SifrarnikValuta"]), 0, Convert.ToDecimal(dt.Rows[0]["Otplata"]),
                        Program.idkadar);
                }

                if (Convert.ToDecimal(dt.Rows[0]["Kamata"]) != 0)
                {
                    var nazi =
                        " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,ID_Komitent,ID_SifrarnikValuta,Iznos,Iznos2,UUser)" +
                        " values (@param0,@param1,@param2,@param3,@param4,@param5,@param6)";
                    db.ParamsQueryDS(nazi, fidDok, 2020, Convert.ToInt32(dt.Rows[0]["ID_KomitentiView"]),
                        Convert.ToInt32(dt.Rows[0]["ID_SifrarnikValuta"]), 0, Convert.ToDecimal(dt.Rows[0]["Kamata"]),
                        Program.idkadar);
                    nazi =
                        " Insert into FinansijskiInterniNalogStavke ( ID_DokumentaView,ID_ArtikliView,ID_Komitent,ID_SifrarnikValuta,Iznos,Iznos2,UUser)" +
                        " values (@param0,@param1,@param2,@param3,@param4,@param5,@param6)";
                    db.ParamsQueryDS(nazi, fidDok, 2334, Convert.ToInt32(dt.Rows[0]["ID_KomitentiView"]),
                        Convert.ToInt32(dt.Rows[0]["ID_SifrarnikValuta"]), Convert.ToDecimal(dt.Rows[0]["Kamata"]), 0,
                        Program.idkadar);
                }
            }
            return true;
        }
    }
}
