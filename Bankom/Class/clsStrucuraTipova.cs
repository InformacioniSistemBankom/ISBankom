using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankom.Class
{

    public class IZVHALKOMS
    {
        public string TekuciRacunPartnera { get; set; }
        public string OznakaKnjizenja { get; set; }
        public string DatumObrade { get; set; }
        public string Storno { get; set; }
        public string NazivKomitenta { get; set; }
        public string Prazno { get; set; }
        public string DatumUplate { get; set; }
        public string TekuciRacunKomitenta { get; set; }
        public string iznos { get; set; }
        public string Prazno1 { get; set; }
        public string OblikPlacanja { get; set; }
        public string SifraPlacanja { get; set; }
        public string Prazno2 { get; set; }
        public string ModelPozivaNaBrojZaduzenja { get; set; }
        public string PozivNaBrojZaduzenja { get; set; }
        public string ModelPozivaNaBrojOdobrenja { get; set; }
        public string PozivNaBrojOdobrenja { get; set; }
        public string SvrhaPlacanja { get; set; }
        public string MestoPrimaoca { get; set; }
        public string NazivPrimaoca { get; set; }
        public string BrojZaReklamaciju { get; set; }
        public string TekuciRacunPrimaoca { get; set; }
        public string KrajReda { get; set; }

        // string spaces = new String(' ', 20);
        /// <summary>
        /// 

        /// </summary>
        /// 
    public IZVHALKOMS()
        {
            TekuciRacunPartnera = new String(' ', 18);
            OznakaKnjizenja = new String(' ', 2);
            DatumObrade = new String(' ', 8);
            Storno = new String(' ', 2);
            NazivKomitenta = new String(' ', 35);
            Prazno = new String(' ', 1);
            DatumUplate = new String(' ', 6);
            TekuciRacunKomitenta = new String(' ', 18);
            iznos = new String(' ', 15);
            Prazno1 = new String(' ', 1);
            OblikPlacanja = new String(' ', 1);
            SifraPlacanja = new String(' ', 2); //109
            Prazno2 = new String(' ', 2); ;
            ModelPozivaNaBrojZaduzenja = new String(' ', 2);
            PozivNaBrojZaduzenja = new String(' ', 22);
            ModelPozivaNaBrojOdobrenja = new String(' ', 2);
            PozivNaBrojOdobrenja = new String(' ', 22);
            SvrhaPlacanja = new String(' ', 36);
            MestoPrimaoca = new String(' ', 10);
            NazivPrimaoca = new String(' ', 35);
            BrojZaReklamaciju = new String(' ', 22);
            TekuciRacunPrimaoca = new String(' ', 18);
            KrajReda = new String(' ', 2);


        }
    }
    public class IZVHALKOMZ
    {
        public string VrstaStavke { get; set; }
        public string BrojRacuna { get; set; }
        public string DatumObrade { get; set; }
        public string DatumPredhodnogIzvoda { get; set; }
        public string StariSaldo { get; set; }
        public string BrojTransakcijaZaduzenja { get; set; }
        public string SumaZaduzenja { get; set; }
        public string BrojTransakcijaOdobrenja { get; set; }
        public string SumaOdobrenja { get; set; }
        public string NoviSaldo { get; set; }
        public string BrojTransakcijaNaCekanju { get; set; }
        public string SumaIznosNaCekanju { get; set; }
        public string RedniBrojIzvoda { get; set; }
        public string KrajReda { get; set; }
        public IZVHALKOMZ()
        {
            VrstaStavke = new String(' ', 2);
            BrojRacuna = new String(' ', 18); // prva stavka
            DatumObrade = new String(' ', 8); // druga stavka
            DatumPredhodnogIzvoda = new String(' ', 8);
            StariSaldo = new String(' ', 18);
            BrojTransakcijaZaduzenja = new String(' ', 6);
            SumaZaduzenja = new String(' ', 18);
            BrojTransakcijaOdobrenja = new String(' ', 6);
            SumaOdobrenja = new String(' ', 18);
            NoviSaldo = new String(' ', 18);
            BrojTransakcijaNaCekanju = new String(' ', 6);
            SumaIznosNaCekanju = new String(' ', 18);
            RedniBrojIzvoda = new String(' ', 3);
            KrajReda = new String(' ', 2);
        }

    }
  
    public class PNHALKOMZ
    {
        public string TekuciRacunPartnera { get; set; } //  ' tekuci racun kome placamo ili ko nam je uplatio
        public string NazivPrimaoca { get; set; }
        public string MestoPrimaoca { get; set; }
        public string PopunjenoSa0 { get; set; }
        public string ModelPozivaNaBrojZaduzenja { get; set; }
        public string PozivNaBrojZaduzenja { get; set; }
        public string SvrhaPlacanja { get; set; }
        public string PopunjenoSa5x0 { get; set; }
        public string Prazno { get; set; }
        public string OblikPlacanja { get; set; }     //   ' 2=prenos;3=kompenzacija(zaduzenje i odobrenje racuna za isti iznos
        public string SifraPlacanja { get; set; } //    ' po sifarniku ili vrednosti koju salje banka
        public string OblikPlacanja1 { get; set; }   // ' prazno blanko = prenos odnosno kompenzacija ili 9=povracaj sredstava
        public string Prazno1 { get; set; }
        public string iznos { get; set; }        // ' desno poravnan sa dve decimale bez zareza
        public string ModelPozivaNaBrojOdobrenja { get; set; }
        public string PozivNaBrojOdobrenja { get; set; }
        public string DatumValute { get; set; }
        public string TipDokumenta { get; set; }
        public string TipStavke  { get; set; }
        public string KrajReda { get; set; }
    public PNHALKOMZ()
        {
        TekuciRacunPartnera = new String(' ', 18);
        NazivPrimaoca = new String(' ', 35);
        MestoPrimaoca  = new String(' ', 10);
        PopunjenoSa0 = new String(' ', 1);
        ModelPozivaNaBrojZaduzenja = new String(' ', 2);
        PozivNaBrojZaduzenja = new String(' ', 23);
        SvrhaPlacanja = new String(' ', 36);
        PopunjenoSa5x0 = new String(' ', 5);
        Prazno = new String(' ', 1);
        OblikPlacanja = new String(' ', 1);
        SifraPlacanja = new String(' ', 2);
        OblikPlacanja1 = new String(' ', 1);
        Prazno1 = new String(' ', 1);
        iznos = new String(' ', 13);
        ModelPozivaNaBrojOdobrenja = new String(' ', 2);
        PozivNaBrojOdobrenja = new String(' ', 23);
        DatumValute = new String(' ', 6);
        TipDokumenta = new String(' ', 1);
        TipStavke = new String(' ', 1);
        KrajReda = new String(' ', 2);
        }
    }
    public class PNHALKOMZ1
    {
        public string TekuciRacunKomitenta { get; set; }
        public string Naziv { get; set; }
        public string Mesto { get; set; }
        public string DatumValute { get; set; }
        public string Prazno { get; set; }
        public string text { get; set; }
        public string TipStavke { get; set; }
        public string KrajReda { get; set; }
       

        public PNHALKOMZ1()  //'ZAGLAVLJE PLATNOG NALOGA HALKOM
        {
            TekuciRacunKomitenta = new String(' ', 18);
            Naziv = new String(' ', 35);
            Mesto = new String(' ', 10);
            DatumValute = new String(' ', 6);
            Prazno = new String(' ', 98);
            text = new string(' ', 12);
            TipStavke = new String(' ', 1);
            KrajReda = new String(' ', 2);
        }



    }
    public class PNHALKOMZ2
    {
    public string TekuciRacunKomitenta { get; set; } //   ' nas tekuci racun
    public string Naziv  { get; set; }
    public string Mesto { get; set; }
    public string ZbirIznosaSvihNaloga { get; set; }
    public string BrojPlatnihNalogaUDatoteci { get; set; }
    public string Prazno { get; set; } //                 ' upisano ''
    public string TipStavke { get; set; } //             ' 9=sabirna stavka
    public string KrajReda { get; set; }

public PNHALKOMZ2()
        {
        TekuciRacunKomitenta = new String(' ', 18);
        Naziv = new String(' ', 35);
        Mesto = new String(' ', 10);
        ZbirIznosaSvihNaloga = new String(' ', 15);
        BrojPlatnihNalogaUDatoteci = new String(' ', 5);
        Prazno = new String(' ', 96);
        TipStavke = new String(' ', 1);
        KrajReda = new String(' ', 2);
        }
    }
    public class Zaposleni
    {
        public char[] ID_Zaposleni { get; set; }
        public char[] Ime { get; set; }
        public char[] Prezime { get; set; }
        public char[] Suser { get; set; }
        public char[] Pass { get; set; }



        public Zaposleni()
        {
            ID_Zaposleni = new char[20];
            Ime = new char[50];
            Prezime = new char[50];
            Suser = new char[50];
            Pass = new char[20];


        }


    }

    public class Sobe
    {

        public char[] ID_Soba { get; set; }
        public char[] BrojSobe { get; set; }
        public char[] Sprat { get; set; }
        public char[] ID_KategorijaSobe { get; set; }
        public char[] ID_Objekat { get; set; }
        public char[] ID_Status { get; set; }
        public char[] DodatniLezaj { get; set; }
        public char[] DodatniPodaci { get; set; }





        public Sobe()
        {
            ID_Soba = new char[20];
            BrojSobe = new char[20];
            Sprat = new char[10];
            ID_KategorijaSobe = new char[10];
            ID_Objekat = new char[10];
            ID_Status = new char[2];
            DodatniLezaj = new char[1];
            DodatniPodaci = new char[50];
        }



    }
    public class PrometSankova
    {


        public DateTime Datum { get; set; }
        public int ID_Skladiste { get; set; }
        public int ID_Artikal { get; set; }
        public int ID_Komitent { get; set; }
        public double Kolicina { get; set; }
        public double Cena { get; set; }
        public double Popust { get; set; }
        public int ID_Soba { get; set; }
        public int ID_NacinPlacanja { get; set; }
        public int ID_Kelner { get; set; }
        public string NazivSkladista { get; set; }
        public int Racun { get; set; }
        public int ID_MestoProdaje { get; set; }
        public int ID_MestoProizvodnje { get; set; }
        public int ID_Kartica { get; set; }
        public string OznakaValute { get; set; }
        public char VrstaSloga { get; set; }
        public char Oznaka { get; set; }
        public int Neoporezivo { get; set; }



    }
    public class PrometRecepcije
    {

        public DateTime datum { get; set; }
        public int ID_Skladiste { get; set; }
        public int ID_Artikal { get; set; }
        public double kolicina { get; set; }
        public double cena { get; set; }
        public int ID_NacinPlacanja { get; set; }
        public int ID_Komitent { get; set; }
        public int ID_Kartica { get; set; }
        public int ID_Soba { get; set; }
        public int ID_Kelner { get; set; }
        public string NazivSkladista { get; set; }
        public int ID_SkladistaProizvodnje { get; set; }
        public int Racun { get; set; }
        public double Popust { get; set; }
        public string OznakaValute { get; set; }
        public char VrstaSloga { get; set; }
        public char Oznaka { get; set; }
        public int ID_MestoPredaje { get; set; }
        public int Neoporezivo { get; set; }
        public int ID_PosTerminal { get; set; }

      
    }
    class ProdajnaMesta
    {


        public char[] ID_Skladiste { get; set; }
        public char[] ID_OpisSkladista { get; set; }
        public char[] NazivSkl { get; set; }
        public char[] Kapacitet { get; set; }
        public char[] OpisSkladista { get; set; }
        public char[] ID_Status { get; set; }


        public ProdajnaMesta()
        {
            ID_Skladiste = new char[20];
            ID_OpisSkladista = new char[20];
            NazivSkl = new char[50];
            Kapacitet = new char[5];
            OpisSkladista = new char[20];
            ID_Status = new char[5];

        }



    }
    public class OpcijeCenovnika
    {

        public char[] ID_OpcijeCenovnika { get; set; }
        public char[] ID_ArtikliView { get; set; }
        public char[] ID_Opcije { get; set; }
        public char[] ID_OperacijaOpcije { get; set; }
        public char[] Procenat { get; set; }
        public char[] GranicaOd { get; set; }
        public char[] GranicaDo { get; set; }
        public char[] KodOpcije { get; set; }

        public OpcijeCenovnika()
        {
            ID_OpcijeCenovnika = new char[20];
            ID_ArtikliView = new char[20];
            ID_Opcije = new char[20];
            ID_OperacijaOpcije = new char[20];
            Procenat = new char[5];
            GranicaOd = new char[10];
            GranicaDo = new char[10];
            KodOpcije = new char[5];

        }


    }
    public class NaciniPlacanja
    {
        public char[] ID_NacinPlacanja { get; set; }
        public char[] NacinPlacanja { get; set; }

        public NaciniPlacanja()
        {
            ID_NacinPlacanja = new char[20];
            NacinPlacanja = new char[50];


        }

    }
    class KompletiUsluga
    {

        public char[] ID_ProizvodView { get; set; }
        public char[] ID_SirovinaView { get; set; }
        public char[] KolicinaKompleta { get; set; }
        public char[] KolicinaUsluge { get; set; }
        public char[] ProcenatUcesca { get; set; }


        public KompletiUsluga()
        {
            ID_ProizvodView = new char[20];
            ID_SirovinaView = new char[20];
            KolicinaKompleta = new char[20];
            KolicinaUsluge = new char[20];
            ProcenatUcesca = new char[5];

        }



      
    }
    class Komitenti
    {

        public char[] ID_Komitenti { get; set; }
        public char[] NazivKomitenta { get; set; }
        public char[] Mesto { get; set; }
        public char[] Adresa { get; set; }
        public char[] Ptt { get; set; }
        public char[] PIB { get; set; }
        public char[] ID_Zemlja { get; set; }

      
    }
    class CenovnikRecepcije
    {


        public char[] NazivUsluge { get; set; }
        public char[] JedinicaMere { get; set; }
        public char[] NazivObjekta { get; set; }
        public char[] KategorijaSobe { get; set; }
        public char[] ProdajnaCena { get; set; }
        public char[] VrstaGosta { get; set; }
        public char[] ID_Usluga { get; set; }
        public char[] ID_JedinicaMere { get; set; }
        public char[] ID_Objekat { get; set; }
        public char[] ID_KategorijaSobe { get; set; }
        public char[] ID_VrstaGosta { get; set; }
        public char[] ID_SkladisteProizvodnje { get; set; }
        public char[] NazivSkladistaProizvodnje { get; set; }
        public char[] ID_MestoProdaje { get; set; }
        public char[] ID_ProizvodView { get; set; }
        public char[] ID_Poreza { get; set; }
        public char[] NazivPoreza { get; set; }
        public char[] StopaPoreza { get; set; }
        public char[] Grupa { get; set; }
        public char[] kod { get; set; }             // 1= prosta usluga,2=slozena usluga,3=prosta usluga ,a cena se obracunava iz cene kompleta
        public char[] UkljucenDaNe { get; set; }    //0 nije ukljjucena cena u kompletu,1 ukljucena cena u kompletu





        public CenovnikRecepcije()
        {
            NazivUsluge = new char[60];
            JedinicaMere = new char[10];
            NazivObjekta = new char[20];
            KategorijaSobe = new char[40];
            ProdajnaCena = new char[16];
            VrstaGosta = new char[20];
            ID_Usluga = new char[10];
            ID_JedinicaMere = new char[10];
            ID_Objekat = new char[10];
            ID_KategorijaSobe = new char[10];
            ID_VrstaGosta = new char[10];
            ID_SkladisteProizvodnje = new char[10];
            NazivSkladistaProizvodnje = new char[20];
            ID_MestoProdaje = new char[10];
            ID_ProizvodView = new char[10];
            ID_Poreza = new char[2];
            NazivPoreza = new char[50];
            StopaPoreza = new char[10];
            Grupa = new char[50];
            kod = new char[1];
            UkljucenDaNe = new char[1];



        }


       
    }
    class Cenovnik
    {
        public char[] ID_ArtikliView { get; set; }
        public char[] ID_Skladiste { get; set; }
        public char[] ProdajnaCena { get; set; }
        public char[] NazivArtikla { get; set; }
        public char[] NazivSkladista { get; set; }
        public char[] JedinicaMere { get; set; }
        public char[] StaraSifra { get; set; }
        public char[] ID_SkladisteProizvodnje { get; set; }
        public char[] NazivSkladistaProizvodnje { get; set; }
        public char[] ID_Poreza { get; set; }
        public char[] NazivPoreza { get; set; }
        public char[] StopaPoreza { get; set; }
        public char[] Grupa { get; set; }






        public Cenovnik()
        {
            ID_ArtikliView = new char[10];
            ID_Skladiste = new char[10];
            ProdajnaCena = new char[16];
            NazivArtikla = new char[100];
            NazivSkladista = new char[20];
            JedinicaMere = new char[10];
            StaraSifra = new char[20];
            ID_SkladisteProizvodnje = new char[10];
            NazivSkladistaProizvodnje = new char[20];
            ID_Poreza = new char[2];
            NazivPoreza = new char[40];
            StopaPoreza = new char[10];
            Grupa = new char[50];




        }
       
    }

}
