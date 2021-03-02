using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Bankom.Class
{
    class clsObradaKalkulacije
    {
        DataBaseBroker db = new DataBaseBroker();
        string sql = "";
        string str = "";
        DataTable t = new DataTable();
        DataTable tt = new DataTable();
        public void RasporedTroskova(long IdDok, string OOd, string DDo, string skl, string Odakle)
        {
            if (Odakle == "STORNO")
                goto RasporediTRoskove;

            string BrojKalkulacije = "";
            string KalkulacijaStaraNova = "";
            sql = " SELECT DokumentaStablo.Naziv as BrDokKorisnik,Dokumenta.BrojDokumenta ";
            sql += " FROM Dokumenta, DokumentaStablo ";
            sql += " WHERE Dokumenta.ID_DokumentaStablo=DokumentaStablo.ID_DokumentaStablo ";
            sql += " AND Dokumenta.ID_Dokumenta=@param0";
            Console.WriteLine(sql);
            DataTable t = new DataTable();
            t = db.ParamsQueryDT(sql, IdDok);
            KalkulacijaStaraNova = t.Rows[0]["BrDokKorisnik"].ToString();
            BrojKalkulacije = t.Rows[0]["BrojDokumenta"].ToString();

            switch (KalkulacijaStaraNova)
            {
                case "KalkulacijaUlaza":
                    sql = " insert into kalk(ID_DokumentaView, Skl, ID_ArtikliView, StaraSifra, Kolicina, IznosDomVal)";
                    sql += " select distinct " + IdDok.ToString() + " as ID_DokumentaView,  NazivSkl as Skl, ID_ArtikliView,StaraSifra, Kolicina, IznosDomVal";
                    sql += " from UlazniRacunTotali ";
                    sql += " where ID_UlazniRacunTotali=(select ID_UlazniRacunCeo from KalkulacijaUf where id_dokumentaview=" + IdDok.ToString() + ")";
                    break;
                case "PDVKalkulacijaUlaza":                   
                    break;
            }

        RasporediTRoskove:
            if (Odakle != "Load")
            {
                long IdRacuna=0;
                sql = "select ID_UlazniRacunCeo from KalkulacijaUf where id_dokumentaview=@param0";
                t = db.ParamsQueryDT(sql, IdDok);
                if (t.Rows.Count > 0)
                {
                    IdRacuna = Convert.ToInt64(t.Rows[0]["ID_UlazniRacunCeo"].ToString());
                    string sel = "";
                    sel = "select kut.IznosDomVal/kut.Kolicina as Cena,sum(kufs.iznos)/kut.kolicina as dodatakcene,kut.ID_ArtikliView ,kut.IId ";
                    sel += " From KonacniUlazniRacunTotali as kut, KalkulacijaUfStavke as kufs, kalkulacijauf as kuf ";
                    sel += " Where kut.iid=kufs.ID_StavkeUlaznogRacuna and ID_KonacniUlazniRacunTotali= kuf.ID_UlazniRacunCeo And kuf.ID_DokumentaView = kufs.ID_DokumentaView ";
                    sel += " and kut.ID_ArtikliView=kufs.ID_ArtikliView and  ID_UlazniRacunCeo=@param0";
                    sel += " Group by kut.ID_ArtikliView ,kut.IId ,kut.IznosDomVal/kut.Kolicina ,kut.kolicina ";
                    Console.WriteLine(sel);
                    t = db.ParamsQueryDT(sel, IdRacuna);
                    for (int i = 0; i < t.Rows.Count; i++)
                    {
                        //JOvana 05.02.21
                        sel = "update RacunStavke set NabavnaCena=" + t.Rows[i]["Cena"].ToString().Replace(",",".") + t.Rows[i]["DodatakCene"].ToString().Replace(",", ".");
                        sel += " FROM racunstavke as r where r.ID_RacunStavke=@param0";
                        Console.WriteLine(sel);
                        int r = db.ParamsInsertScalar(sel, t.Rows[i]["iid"]);
                    }
                }
            }
        }
        public void  RasporediTRoskoveNaNekiOdNacina(string IdDokView, string IdUlaznogRacunaIId , string IdUlaznogRacunaIdUR , double SKurs ,string  UslovKalkulacije , string UslovKalkulacijeID , string UlzniRacunBrojID , double FakturnaVrednost, double IznosTroska)
        {
            string sql = "";
            //long IdDokView = 1;
            string Dokument = "";
            int r = 0;
            switch (UslovKalkulacije)
            {
                case "Kolicina":
                    sql = "delete from KalkulacijaUfStavke where  ID_Trosak=@param0";
                    sql += " and ID_UlazniRacunZaUslugeCeo=@param1";
                    sql += " and ID_DokumentaView=@param2";
                    r = db.ParamsInsertScalar(sql, IdUlaznogRacunaIdUR, IdUlaznogRacunaIId, IdDokView);
                    if (Dokument == "KalkulacijaUlaza")
                    {
                        str = " insert into KalkulacijaUfStavke(ID_DokumentaView, ID_ArtikliView, ID_Trosak, Iznos,";
                        str += " ID_UslovKalkulacije, ID_UlazniRacunZaUslugeCeo, FakturnaVrednost)";
                        str += " select  distinct " + IdDokView.ToString() + " as ID_DokumentaView, ID_ArtikliView, ";
                        str += IdUlaznogRacunaIdUR.ToString() + " as ID_Trosak, " + IznosTroska.ToString();
                        str += " * Kolicina/SumaKolicina, " + UslovKalkulacijeID.ToString() + ", ";
                        str += IdUlaznogRacunaIId.ToString() + ", " + FakturnaVrednost;
                        str += " from UlazniRacunTotali where ID_UlazniRacunTotali=" + UlzniRacunBrojID.ToString();
                    }
                    else
                    { str = " insert into KalkulacijaUfStavke(ID_DokumentaView, ID_ArtikliView, ID_Trosak, Iznos,";
                        str += " ID_UslovKalkulacije, ID_UlazniRacunZaUslugeCeo, FakturnaVrednost,ID_StavkeUlaZnogRacuna)";
                        str += " select  distinct " + IdDokView.ToString() + " as ID_DokumentaView, ID_ArtikliView, ";
                        str += IdUlaznogRacunaIdUR.ToString() + " as ID_Trosak, " + IznosTroska.ToString();
                        str += " * Kolicina/SumaKolicina, " + UslovKalkulacijeID.ToString() + ", ";
                        str += IdUlaznogRacunaIId.ToString() + ",(FakturnaVrednost* Kolicina/SumaKolicina),iid ";
                        str += " from KonacniUlazniRacunTotali where ID_KonacniUlazniRacunTotali=" + UlzniRacunBrojID.ToString();
                    }
                    break;
                case "Vrednost":
                    str = "delete from KalkulacijaUfStavke where  ID_Trosak=" + IdUlaznogRacunaIdUR.ToString();
                    str += " and ID_UlazniRacunZaUslugeCeo=" + IdUlaznogRacunaIId.ToString();
                    str += " and ID_DokumentaView=" + IdDokView.ToString();

                    if (Dokument == "KalkulacijaUlaza")
                    {
                        str = " insert into KalkulacijaUfStavke(ID_DokumentaView, ID_ArtikliView, ID_Trosak, Iznos,";
                        str += "ID_UslovKalkulacije, ID_UlazniRacunZaUslugeCeo, FakturnaVrednost,ID_StavkeUlaZnogRacuna)";
                        str += " select  distinct " + IdDokView.ToString() + " as ID_DokumentaView, ID_ArtikliView, ";
                        str += IdUlaznogRacunaIdUR.ToString() + " as ID_Trosak, " + FakturnaVrednost.ToString();
                        str += " * Iznos/SumaIznos, " + UslovKalkulacijeID.ToString() + ", " + IdUlaznogRacunaIId.ToString();
                        str += ", " + FakturnaVrednost.ToString() + " ,iid ";
                        str += " from UlazniRacunTotali where ID_UlazniRacunTotali=" + UlzniRacunBrojID.ToString();
                    }
                    else
                    {
                        str = " insert into KalkulacijaUfStavke(ID_DokumentaView, ID_ArtikliView, ID_Trosak, Iznos,";
                        str += "ID_UslovKalkulacije, ID_UlazniRacunZaUslugeCeo, FakturnaVrednost,ID_StavkeUlaZnogRacuna)";
                        str += " select  distinct " + IdDokView.ToString() + " as ID_DokumentaView, ID_ArtikliView, ";
                        str += IdUlaznogRacunaIdUR.ToString() + " as ID_Trosak, " + IznosTroska.ToString();
                        str += " * Iznos/SumaIznos, " + UslovKalkulacijeID.ToString() + ", ";
                        str += IdUlaznogRacunaIId.ToString() + ", " + FakturnaVrednost.ToString() + " * Iznos/SumaIznos,iid ";
                        str += " from KonacniUlazniRacunTotali where ID_KonacniUlazniRacunTotali=" + UlzniRacunBrojID.ToString();
                    }
                    break;
                case "Broj artikala":
                    str = "delete from KalkulacijaUfStavke where  ID_Trosak=@param0";
                    str += " and ID_UlazniRacunZaUslugeCeo=@param1";
                    str += " and ID_DokumentaView=@param2";
                    r = db.ParamsInsertScalar(str, IdUlaznogRacunaIdUR, IdUlaznogRacunaIId, IdDokView);

                    str +=  "select count(*) as b from RacunStavke where ID_DokumentaView=@param0";
                    t = db.ParamsQueryDT(str, UlzniRacunBrojID);
                    int broj = Convert.ToInt32(t.Rows[0]["b"]);

                    if (Dokument == "KalkulacijaUlaza")
                    {
                        str = " insert into KalkulacijaUfStavke(ID_DokumentaView, ID_ArtikliView, ID_Trosak, Iznos,";
                        str +="ID_UslovKalkulacije, ID_UlazniRacunZaUslugeCeo, FakturnaVrednost,ID_StavkeUlaZnogRacuna)";
                        str += " select  distinct " + IdDokView.ToString() + " as ID_DokumentaView, ID_ArtikliView, ";
                        str += IdUlaznogRacunaIdUR.ToString() + " as ID_Trosak," + Convert.ToString(FakturnaVrednost / broj);
                        str+=", " + UslovKalkulacijeID.ToString() + ", " + IdUlaznogRacunaIId.ToString() + ", ";
                        str += FakturnaVrednost.ToString()+" ,iid ";
                        str +=" from UlazniRacunTotali where ID_UlazniRacunTotali=@param0";
                    }
                    else
                    {
                        str = " insert into KalkulacijaUfStavke(ID_DokumentaView, ID_ArtikliView, ID_Trosak, Iznos,";
                        str += " ID_UslovKalkulacije, ID_UlazniRacunZaUslugeCeo, FakturnaVrednost,ID_StavkeUlaZnogRacuna)";
                        str += " select  distinct " + IdDokView.ToString() + " as ID_DokumentaView, ID_ArtikliView, ";
                        str += IdUlaznogRacunaIdUR.ToString() + " as ID_Trosak, " + Convert.ToString(IznosTroska / broj);
                        str += ", " + UslovKalkulacijeID.ToString() + ", " + IdUlaznogRacunaIId.ToString() + ", ";
                        str += Convert.ToString(FakturnaVrednost / broj) + " ,iid ";
                        str += " from KonacniUlazniRacunTotali where ID_KonacniUlazniRacunTotali=@param0";                        
                    }
                    Console.WriteLine(str);
                    r = db.ParamsInsertScalar(str, UlzniRacunBrojID);

                    break;
                default:
                    str = "Update KalkulacijaUfStavke set iznos= " + Convert.ToString(FakturnaVrednost * SKurs) + " where  ID_Trosak=@param0";
                    str += " and ID_UlazniRacunZaUslugeCeo=@param1";
                    str += " and ID_DokumentaView=@param2";
                    r = db.ParamsInsertScalar(str, IdUlaznogRacunaIdUR, IdUlaznogRacunaIId, IdDokView);
                    break;
            }
            SqlCommand cmd = new SqlCommand(sql);
            string message = db.Comanda(cmd);
            if (message != "")
                MessageBox.Show(message);

            str = "delete from KalkulacijaUfStavke where ID_ArtikliView=1";
            cmd = new SqlCommand(sql);
            message = db.Comanda(cmd);
            if (message != "")
                MessageBox.Show(message);

            str = "UPDATE KalkulacijaUfStavke SET UUser =" + Program.idkadar.ToString() + " WHERE ID_DokumentaView=@param0";
            r = db.ParamsInsertScalar(str, IdDokView);
        }
    }
}
    

