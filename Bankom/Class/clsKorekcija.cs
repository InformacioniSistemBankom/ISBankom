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
    class clsKorekcija
    {
        private int ID_DokumentaStablo;
        private string datumOd;
        private string datum;
        private string NazivSkl = "";
        private string ID_Skladiste = "1";
        private string ID_DokumentaView = "1";
        string sql = "";
        private string NazivDokumenta;
        int ret = 0;
        DataBaseBroker db = new DataBaseBroker();

        public bool ObradiZahtev()
        {
            bool ObradiZahtev = false;
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;

            if (MessageBox.Show("Korigujemo promet za " + Program.imeFirme, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                datumOd = Prompt.ShowDialog("", "Unesite datum korekcije: ", "Korekcija  prometa");

                if (datumOd == "") { MessageBox.Show("Niste uneli datum ponovite !!!"); return ObradiZahtev; }

                if (CheckDate(datumOd) == false) { MessageBox.Show("Pogresan datum !!!"); return ObradiZahtev; }

                NazivSkl = Prompt.ShowDialog("", "Unesite skladiste za koje vrsite korekciju ", "Korekcija  prometa");

                if (NazivSkl == "") ID_Skladiste = "1";
                else
                {
                    sql = "select ID_Skladiste from skladiste where NazivSkl= @param0";
                    DataTable t = db.ParamsQueryDT(sql, NazivSkl);
                    if (t.Rows.Count == 0)
                    {
                        MessageBox.Show("Greska kod unosa skladista !!!");
                        return ObradiZahtev;
                    }
                    else
                    {
                        ID_Skladiste = t.Rows[0]["ID_Skladiste"].ToString();
                    }
                }
                KorekcijaNaDan(datumOd, NazivSkl, ID_Skladiste);
                ObradiZahtev = true;
            }
            return ObradiZahtev;
        }
        protected bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool KorekcijaNaDan(string datumOd, string NazivSkl, string ID_Skladiste)
        {
            bool KorekcijaNaDan = false;
            string pomskl="";
            string pomidvlasnik="";
            if (ID_Skladiste == "1")
            {
                sql = "Select  Firma,godina,id_skladiste,id_artikliView,Sum(finSaldo - robsaldo) as razlika,ID_KomitentiView,NazivKom "
                    + " from VrednostRobeiFinansija, artiklitotali,komitentitotali "
                    + " where artiklitotali.id_artiklitotali=id_artikliView and komitentitotali.id_komitentitotali=ID_KomitentiView " 
                    + " and godina=@param0 and firma=@param1 "
                    + " group by Firma,godina,id_skladiste,id_artikliView,ID_KomitentiView  "
                    + " having Sum(finSaldo - robsaldo)<>0 "
                    + " Order by id_skladiste,ID_KomitentiView,id_artikliView";
            }
            else
            {
                sql = "Select  Firma,godina,id_skladiste,id_artikliView,Sum(finSaldo - robsaldo) as razlika,ID_KomitentiView,NazivKom "
                   + " from VrednostRobeiFinansija, artiklitotali,komitentitotali "
                   + " where artiklitotali.id_artiklitotali=id_artikliView and komitentitotali.id_komitentitotali=ID_KomitentiView "
                   + " and godina=@param0 and firma=@param1 and ID_Skladiste=@param2 "
                   + " group by Firma,godina,id_skladiste,id_artikliView,ID_KomitentiView  "
                   + " having Sum(finSaldo - robsaldo)<>0 "
                   + " Order by id_skladiste,ID_KomitentiView,id_artikliView";
            }
            // cela razrada za vlasnika robe a u view-u VrednostRobeiFinansija pise id_vlasnik =1 , 
            // pri tom su tu i pobrojana konta sto nije dobro kod promene kontnog plana
            DataTable dt = db.ParamsQueryDT(sql, DateTime.Parse(datumOd).Year, Program.idFirme, ID_Skladiste);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Nema korekcije !!!");
                return KorekcijaNaDan;
            }
            else
            {
                int prolaz = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (prolaz == 0)
                    {
                        pomskl = row["id_skladiste"].ToString();
                        pomidvlasnik = row["id_KomitentiView"].ToString();
                    }

                    if (pomskl != row["id_skladiste"].ToString() || pomidvlasnik != row["id_KomitentiView"].ToString())
                    {
                        db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + ID_DokumentaView);
                        db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + NazivDokumenta, "IdDokument:" + ID_DokumentaView);
                    }

                    if (pomskl != row["id_skladiste"].ToString() || pomidvlasnik != row["id_KomitentiView"].ToString() || prolaz == 0)
                    {
                        sql = "Select ID_Dokumenta as ID_DokumentaView from Dokumenta"
                        + " where ID_DokumentaStablo=44 and datum= @param0 and Opis ='Usaglasavanje roba-finansije-'+@param1+'-'+@param2";
                        DataTable t = db.ParamsQueryDT(sql, datumOd, NazivSkl, row["NazivKom"].ToString());
                        if (t.Rows.Count != 0)
                        {
                            ID_DokumentaView = t.Rows[0]["ID_DokumentaView"].ToString();
                            sql = "delete from InterniNalogZaRobuStavke where id_Dokumentaview= @param0";
                            ret = db.ParamsInsertScalar(sql, ID_DokumentaView);
                            sql = "delete from InterniNalogZaRobu where id_Dokumentaview= @param0";
                            ret = db.ParamsInsertScalar(sql, ID_DokumentaView);
                            sql = "delete from CeneArtikalaNaSkladistimaPred where id_Dokumentaview= @param0";
                            ret = db.ParamsInsertScalar(sql, ID_DokumentaView);
                        }
                        else
                        {
                            //   upis  korekcije u dokumenta
                            clsObradaOsnovnihSifarnika cls = new clsObradaOsnovnihSifarnika();
                            string ParRb = "";
                            ID_DokumentaStablo = 44;
                            NazivDokumenta = "ObracunVrednostiZaliha";

                            ret = cls.UpisiDokument(ref ParRb, "Usaglasavanje roba - finansije -" + NazivSkl, ID_DokumentaStablo, datumOd);

                            if (ret == -1)
                            {
                                MessageBox.Show("Greska prilikom inserta!");
                                return KorekcijaNaDan;
                            }
                            ID_DokumentaView = ret.ToString();
                        }

                        sql = "Insert into interninalogZaRobu(ID_DokumentaView, ID_Skladiste ,ID_Analitika,id_komitentiview)"
                             + " values(@param0, @param1,@param2,@param3)";
                        DataTable dtr = db.ParamsQueryDT(sql, ID_DokumentaView, row["id_skladiste"].ToString(), 1, row["id_KomitentiView"].ToString());

                    }
                    if (row["razlika"].ToString() != "0")
                    {
                        sql = "Insert into InterniNalogZaRobuStavke(ID_DokumentaView, ID_ArtikliView, KolicinaIzlaz, JedinicnaCena )"
                            + " values(@param0, @param1,@param2,@param3)";
                        DataTable dts = db.ParamsQueryDT(sql, ID_DokumentaView, row["ID_ArtikliView"].ToString(), row["razlika"].ToString(), 1);
                    }

                    pomskl = row["id_skladiste"].ToString();
                    pomidvlasnik = row["id_KomitentiView"].ToString();

                    prolaz += 1;
                }

                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + ID_DokumentaView);
                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + NazivDokumenta, "IdDokument:" + ID_DokumentaView);

                KorekcijaNaDan =true;
                return KorekcijaNaDan;
            }
           
        }

        public bool VrednostNaDan()
        {
            bool VrednostNaDan = false;
            string ID_Artikli;
            double SaldoIzPrometa = 0;
            double Stanje = 0;
            double Cena;  double Odstupanje;
            double PoslednjaCena = 0;
            int ID_Vlasnika = 1;
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;

            if (MessageBox.Show("Da li zelite ponistiti postojece stavke?", "Vrednost zaliha", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "delete from InterniNalogZaRobuStavke where id_Dokumentaview= @param0";
                ret = db.ParamsInsertScalar(sql, ((Bankom.frmChield)forma).iddokumenta);
                sql = "delete from InterniNalogZaRobu where id_Dokumentaview= @param0";
                ret = db.ParamsInsertScalar(sql, ((Bankom.frmChield)forma).iddokumenta);
                sql = "delete from CeneArtikalaNaSkladistimaPred where id_Dokumentaview= @param0";
                ret = db.ParamsInsertScalar(sql, ((Bankom.frmChield)forma).iddokumenta);

                string datum = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost;
                string datumOd = "01.01." + DateTime.Parse(datum).Year;
                int ID_Skladiste = Convert.ToInt32(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").ID);
                //ID_Vlasnika = Convert.ToInt32(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivKom").ID);

                sql = "Select ID_ArtikliView,Datum,Brdok,Ulaz,Izlaz,VrednostNab as Vrednost,"
                    + " ProsecnaNabavnaCena AS ProsCena,PoslednjaNabavnaCena,ID_Vlasnika "
                    + " from ceneartikalanaskladistimapred "
                    + " where datum <= @param0 and datum>=@param1 and skl=@param2 AND ID_Vlasnika=@param3 "
                    + " order by ID_ArtikliView,datum,Ulaz desc,ID_DokumentaView";

                DataTable t = db.ParamsQueryDT(sql, datum, datumOd, ID_Skladiste, ID_Vlasnika);
                if (t.Rows.Count == 0)
                {
                    MessageBox.Show("Greska!");
                    return VrednostNaDan;
                }
                else
                {
                    ID_Artikli ="1";
                    int prolaz = 0;
                    foreach (DataRow row in t.Rows)
                    {
                        if (prolaz==0) ID_Artikli = row["ID_ArtikliView"].ToString();

                        //if (row["ID_ArtikliView"].ToString() == "14674")
                        //{
                        //    Console.WriteLine("jovana");
                        //}
                        if (ID_Artikli != row["ID_ArtikliView"].ToString())
                        {

                            if (Stanje != 0)
                            {
                                Cena = Math.Round((SaldoIzPrometa / Stanje), 4);
                                Odstupanje = Stanje * PoslednjaCena - Stanje * Cena;
                            }
                            else
                            {
                                Cena = PoslednjaCena;
                                Odstupanje = -SaldoIzPrometa;
                            }

                            if (Odstupanje != 0)
                            {
                                if (Odstupanje > 0)
                                {
                                    sql = "Insert into InterniNalogZaRobuStavke(ID_DokumentaView, ID_ArtikliView, KolicinaUlaz, JedinicnaCena )"
                                   + " values(@param0, @param1,@param2,@param3)";
                                    DataTable dts = db.ParamsQueryDT(sql, ((Bankom.frmChield)forma).iddokumenta, ID_Artikli, Odstupanje, Cena);
                                }
                                else
                                {
                                    sql = "Insert into InterniNalogZaRobuStavke(ID_DokumentaView, ID_ArtikliView, KolicinaIzlaz, JedinicnaCena )"
                                                                  + " values(@param0, @param1,@param2,@param3)";
                                    DataTable dts = db.ParamsQueryDT(sql, ((Bankom.frmChield)forma).iddokumenta, ID_Artikli, -1 * Odstupanje, Cena);
                                }
                            }

                            SaldoIzPrometa = 0;
                            Stanje = 0;

                        }

                        string aa;
                        if (row["Ulaz"].ToString() == "0" && row["Izlaz"].ToString() == "0")
                        {
                            if (row["BrDok"].ToString().Contains("44-") || row["BrDok"].ToString().Contains("46-") || row["BrDok"].ToString().Contains("42-"))
                            {
                                SaldoIzPrometa += Convert.ToDouble(row["Vrednost"]);
                            }
                        }
                        else
                        {
                            Stanje = Stanje + (Convert.ToDouble(row["Ulaz"]) - Convert.ToDouble(row["Izlaz"]));
                            SaldoIzPrometa += Convert.ToDouble(row["Vrednost"]);
                            aa = row["BrDok"].ToString().Substring(0, row["BrDok"].ToString().IndexOf("-"));
                            // dodala sam dokumenta 507 i 510
                            if ((aa == "503" || aa == "504" || aa == "505" || aa == "506" || aa == "507" || aa == "510") && Convert.ToDouble(row["Ulaz"]) > 0 || Convert.ToDouble(row["Ulaz"]) < 0 || row["ProsCena"].ToString() == "0")
                            {
                                if (aa == "503" || aa == "504" || aa == "505" || aa == "506" || aa == "507" || aa == "510")
                                {
                                    if (Stanje != 0)
                                        PoslednjaCena = Math.Round((SaldoIzPrometa / Stanje), 4);
                                    // ne postoji else dal bi ovo bilo ispravno???
                                    //else PoslednjaCena = Convert.ToDouble(row["PoslednjaNabavnaCena"]);
                                }
                            }
                            else PoslednjaCena = Convert.ToDouble(row["PoslednjaNabavnaCena"]);
                        }
                    
                        
                     ID_Artikli = row["ID_ArtikliView"].ToString();
                     prolaz += 1;
                        
                    }

                    if (Stanje != 0)
                    {
                        Cena = Math.Round((SaldoIzPrometa / Stanje), 4);
                        Odstupanje = Stanje * PoslednjaCena - Stanje * Cena;
                    }
                    else
                    {
                        Cena = PoslednjaCena;
                        Odstupanje = -SaldoIzPrometa;
                    }

                    if (Odstupanje != 0)
                    {
                        if (Odstupanje > 0)
                        {
                            sql = "Insert into InterniNalogZaRobuStavke(ID_DokumentaView, ID_ArtikliView, KolicinaUlaz, JedinicnaCena )"
                           + " values(@param0, @param1,@param2,@param3)";
                            DataTable dts = db.ParamsQueryDT(sql, ((Bankom.frmChield)forma).iddokumenta, ID_Artikli, Odstupanje, Cena);
                        }
                        else
                        {
                            sql = "Insert into InterniNalogZaRobuStavke(ID_DokumentaView, ID_ArtikliView, KolicinaIzlaz, JedinicnaCena )"
                                                          + " values(@param0, @param1,@param2,@param3)";
                            DataTable dts = db.ParamsQueryDT(sql, ((Bankom.frmChield)forma).iddokumenta, ID_Artikli, -1 * Odstupanje, Cena);
                        }
                    }

                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:ObracunVrednostiZaliha", "IdDokument:" + ((Bankom.frmChield)forma).iddokumenta);
                    VrednostNaDan = true;
                    return VrednostNaDan;
                }
            }
            return VrednostNaDan;
        }
    }
}