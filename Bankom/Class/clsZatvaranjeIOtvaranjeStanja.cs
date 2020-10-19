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
    class clsZatvaranjeIOtvaranjeStanja
    {
        private string iddokument;
        private string Godina;
        private string datum;
        private string datumOd;
        private int ID_DokumentaStablo;
        string sql = "";
        private string NazivDokumenta;
        DataBaseBroker db = new DataBaseBroker();
        int ret = 0;
        int pidDok = 0;
        public bool ObradiZahtev(string PoLotuDaNe)
        {
            bool ObradiZahtev = false;
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;
            
            if (MessageBox.Show("Zatvaramo promet za " + Program.imeFirme, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Godina = Prompt.ShowDialog("", "Unesite Godinu za koju vrsimo zatvaranje ", "Zatvaranje prometa");
                if (Godina == "") { MessageBox.Show("Niste uneli godinu ponovite !!!"); return ObradiZahtev; }
                datum = "31.12." + Godina;
                datumOd = "01.01." + Godina;
                sql = "select dokument from  DokumentaTotali where dokument ='PocetnoStanjeZaRobu' and Datum=@param0 and nazivorg like @param1";
                DataTable t = db.ParamsQueryDT(sql, datum, Program.imeFirme + "%");
                if (t.Rows.Count == 0)
                {
                    // Zatvaranje i otvaranje stanja
                    Zatvaranje(PoLotuDaNe);
                    Otvaranje(PoLotuDaNe);
                }
                else
                {
                    MessageBox.Show("Vec je izvrseno zatvaranje");
                    return ObradiZahtev;
                }
            }
            ObradiZahtev = true;
            return ObradiZahtev;
        }

        private bool Zatvaranje(string PoLotuDaNe)
        {
            bool Zatvaranje = false;
            if (PoLotuDaNe=="NE")
                sql = "select c.skl,c.ID_vlasnika, c.NazivSkl,c.ID_ArtikliView ,c.Stanje,c.VrednostNab,c.ProsecnaNabavnaCena, k.NazivKomitenta as nazivVl "
                       + " from ceneartikalanaskladistimapred as c,Komitenti as k "
                       + " where datum <= @param0 and ID_Vlasnika=ID_Komitenti and datum >=@param1 and ID_OrganizacionaStrukturaView=@param2 "
                       + " order by Skl,ID_Vlasnika,ID_ArtikliView,datum,ID_CeneArtikalaNaSkladistimaPred ";
            else
                sql = "select c.skl,c.ID_vlasnika, c.NazivSkl,c.ID_ArtikliView ,c.Stanje,c.VrednostNab,c.ProsecnaNabavnaCena, c.id_lotview,c.id_magacinskapolja,k.NazivKomitenta as nazivVl "
                       + " from StanjeRobeNaSkl as c,Komitenti as k "
                       + " where datum <= @param0 and ID_Vlasnika=ID_Komitenti and datum >=@param1 and ID_OrganizacionaStrukturaView=@param2 "
                       + " order by  Skl,ID_Vlasnika,ID_ArtikliView,ID_LotView,ID_MagacinskaPolja,datum,ID_StanjeRobeNaSkl ";

            DataTable dt = db.ParamsQueryDT(sql, datum,datumOd,  Program.idFirme);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("");
                return Zatvaranje;
            }
            foreach (DataRow row in dt.Rows)
            {

                string kojivl = "";

                if (!row["NazivVl"].Equals("")) kojivl = "-" + row["NazivVl"].ToString().Substring(0, 10);

                //   upis  pocetnogstanjazarobu u dokumenta
                
                clsObradaOsnovnihSifarnika cls = new clsObradaOsnovnihSifarnika();
                string ParRb ="";

                if (PoLotuDaNe == "NE")
                {
                    ID_DokumentaStablo = 42;
                    NazivDokumenta = "PocetnoStanjeZaRobu";
                }
                else
                {
                    ID_DokumentaStablo = 500;
                    NazivDokumenta = "LotPocetnoStanjeMagacin";
                }
                
                if (!row["NazivVl"].Equals("")) kojivl = "-" + row["NazivVl"].ToString().Substring(0, 10);

                pidDok = cls.UpisiDokument(ref ParRb, row["NazivSkl"].ToString() + kojivl + "-zatvaranje", ID_DokumentaStablo, datum);

                if (pidDok == -1)
                {
                    MessageBox.Show("Greska prilikom inserta!");
                    return Zatvaranje;
                }

                sql = "Insert into racun(ID_DokumentaView, ID_Skladiste,ID_VlasnikRobe ) values(@param0, @param1,@param2)";
                DataTable dtr = db.ParamsQueryDT(sql, pidDok.ToString(), row["skl"].ToString(), row["ID_vlasnika"].ToString());

                // zapisi stavku u zatvaranju
             
                db.ExecuteStoreProcedure("ZatvaranjeIOtvaranjeStanja", "Firma:" + Program.idFirme, "PoLotuDaNe:" + PoLotuDaNe, "IdDokView:" + pidDok,
                                         "DoDatuma:" + datum, "NazivSkl:" + row["NazivSkl"].ToString(), "ID_vlasnika:" + row["ID_vlasnika"].ToString(),
                                         "IdPrethodni:1", "UUser:" + Program.idkadar);
               
                sql = "select * from racunstavke where ID_DokumentaView = @param0";
                DataTable rs = db.ParamsQueryDT(sql, pidDok);
                if (rs.Rows.Count == 0)
                {
                    sql = "delete from dokumenta where ID_Dokumenta = @param0";
                    ret = db.ParamsInsertScalar(sql, pidDok);
                    sql = "delete from racun where ID_DokumentaView = = @param0";
                    ret = db.ParamsInsertScalar(sql, pidDok);
                }
                else
                {
                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);
                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:"+ NazivDokumenta, "IdDokument:" + pidDok);
                }
            }

            if (PoLotuDaNe == "NE") db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:0");
            else db.ExecuteStoreProcedure("StanjeRobePoLotu", "IdDokView:0");

            Zatvaranje = true;
            return Zatvaranje;
        }
        private bool Otvaranje(string PoLotuDaNe)
        {
            bool Otvaranje = false;
            datumOd = "01.01." + Convert.ToDateTime(datum).AddYears(+1).Year;
            if (PoLotuDaNe == "NE")
                sql = "Select c.NazivSkl,c.id_skladiste as skl,c.ID_PocetnoStanjeZaRobuTotali as ID_DokumentaView,ID_ArtikliView,kolicina as ulaz,nabavnacena as ProsecnaNabavnaCena, "
                      + " k.NazivKomitenta as NazivVl,k.id_komitenti as id_vlasnika "
                      + " from PocetnoStanjeZaRobuTotali as c,Komitenti as k ,dokumenta,OrganizacionaStruktura "
                      + " Where c.datum = @param0  and  id_dokumenta=ID_PocetnoStanjeZaRobuTotali and ID_VlasnikRobe=ID_Komitenti and Dokumenta.ID_OrganizacionaStrukturaView = OrganizacionaStruktura.ID_OrganizacionaStruktura and"
                      + " c.brdok like'42-%'   And ID_OrganizacionaStrukturaStablo = @param1 "
                      + " order by Skl,ID_dokumentaview,ID_ArtikliView ";
            else
                sql = "Select c.NazivSkl,c.id_skladiste as skl,c.ID_LotPocetnoStanjeMagacinTotali as ID_DokumentaView,ID_ArtikliView,ID_LotView,ID_MagacinskaPolja,kolicina as ulaz,nabavnacena as ProsecnaNabavnaCena, "
                     + " k.NazivKomitenta as NazivVl,k.id_komitenti as id_vlasnika "
                     + " from LotPocetnoStanjeMagacinTotali as c,Komitenti as k ,dokumenta,OrganizacionaStruktura "
                     + " Where c.datum = @param0  and  id_dokumenta=ID_LotPocetnoStanjeMagacinTotali and ID_VlasnikRobe=ID_Komitenti and Dokumenta.ID_OrganizacionaStrukturaView = OrganizacionaStruktura.ID_OrganizacionaStruktura and"
                     + " c.brdok like'500-%'   And ID_OrganizacionaStrukturaStablo = @param1 "
                     + " order by Skl,ID_dokumentaview,ID_ArtikliView,ID_LotView,ID_MagacinskaPolja";

            DataTable dt = db.ParamsQueryDT(sql,  datum, Program.idFirme);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("");
                return Otvaranje;
            }
            foreach (DataRow row in dt.Rows)
            {

                string kojivl = "";

                if (!row["NazivVl"].Equals("")) kojivl = "-" + row["NazivVl"].ToString().Substring(0, 10);

                //   upis  pocetnogstanjazarobu u dokumenta
                clsObradaOsnovnihSifarnika cls = new clsObradaOsnovnihSifarnika();
                string ParRb = "";
                if (PoLotuDaNe == "NE")
                {
                    ID_DokumentaStablo = 42;
                    NazivDokumenta = "PocetnoStanjeZaRobu";
                }
                else
                {
                    ID_DokumentaStablo = 500;
                    NazivDokumenta = "LotPocetnoStanjeMagacin";
                }

                if (!row["NazivVl"].Equals("")) kojivl = "-" + row["NazivVl"].ToString().Substring(0, 10);

                pidDok = cls.UpisiDokument(ref ParRb, row["NazivSkl"].ToString() + kojivl + "-otvaranje", ID_DokumentaStablo, datumOd);

                if (pidDok == -1)
                {
                    MessageBox.Show("Greska prilikom inserta!");
                    return Otvaranje;
                }

                sql = "Insert into racun(ID_DokumentaView, ID_Skladiste,ID_VlasnikRobe,ID_KomitentiView  ) values(@param0, @param1,@param2,@param3)";
                DataTable dtr = db.ParamsQueryDT(sql, pidDok.ToString(), row["skl"].ToString(), row["ID_vlasnika"].ToString(), row["ID_vlasnika"].ToString());

                // zapisi stavku u otvaranju
                db.ExecuteStoreProcedure("ZatvaranjeIOtvaranjeStanja", "Firma:" + Program.idFirme, "PoLotuDaNe:" + PoLotuDaNe, "IdDokView:" + pidDok,
                                         "DoDatuma:" + datum, "NazivSkl:" + row["NazivSkl"].ToString(), "ID_vlasnika:" + row["ID_vlasnika"].ToString(),
                                         "IdPrethodni:" + row["ID_DokumentaView"], "UUser:" + Program.idkadar);

               
               db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + pidDok);
               db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:"+ NazivDokumenta, "IdDokument:" + pidDok);
 
            }

            if (PoLotuDaNe == "NE") db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:0");
            else db.ExecuteStoreProcedure("StanjeRobePoLotu", "IdDokView:0");

            Otvaranje = true;
            return Otvaranje;
        }
        public bool ObradiIspravku()
        {
            bool ObradiIspravku = false;
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;
            pidDok = Convert.ToInt32(((Bankom.frmChield)forma).iddokumenta);

            if (MessageBox.Show("Da li zelite ponistiti postojece stavke?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
              if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost.Substring(0, 2) == "31")
                {
                    sql = "delete from racunstavke where ID_DokumentaView = @param0";
                    ret = db.ParamsInsertScalar(sql, pidDok);
                    sql = "delete from CeneArtikalaNaSkladistimaPred where id_Dokumentaview = @param0";
                    ret = db.ParamsInsertScalar(sql, pidDok);
                    ZatvaranjeIspravka(pidDok, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").ID);
                    OtvaranjeIspravka(pidDok, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivSkl").ID);
                }
                else
                {
                    MessageBox.Show("Pronadjite odgovarajuce zatvaranje i ispravite njega!");
                    return ObradiIspravku;
                }
            }
            ObradiIspravku = true;
            return ObradiIspravku;
        }
        private bool ZatvaranjeIspravka(int ID_DokumentaView,string datum,string ID_Skladiste)
        {
            bool ZatvaranjeIspravka = false;
            string ID_Vlasnik = "1";
            string NazivSkl = "";
            datumOd = "01.01." + Convert.ToDateTime(datum).Year ;

            sql = "select ID_VlasnikRobe,NazivSkl from PocetnoStanjeZaRobuTotali where ID_PocetnoStanjeZaRobuTotali= @param0";
            DataTable dv = db.ParamsQueryDT(sql, ID_DokumentaView);
            if (dv.Rows.Count == 0)
            {
                MessageBox.Show("");
                return ZatvaranjeIspravka;
            }
            else
            {
               ID_Vlasnik = dv.Rows[0]["ID_VlasnikRobe"].ToString();
               NazivSkl = dv.Rows[0]["NazivSkl"].ToString();
            }
            sql = "select c.skl,c.ID_vlasnika, c.NazivSkl,c.ID_ArtikliView ,c.Stanje,c.VrednostNab,c.ProsecnaNabavnaCena, k.NazivKomitenta as nazivVl "
                       + " from ceneartikalanaskladistimapred as c,Komitenti as k "
                       + " where datum <= @param0 and ID_Vlasnika=ID_Komitenti and datum >=@param1 and ID_OrganizacionaStrukturaView=@param2 "
                       + "  and Skl=@param3 and ID_Vlasnika=@param4"
                       + " order by Skl,ID_Vlasnika,ID_ArtikliView,datum,ID_CeneArtikalaNaSkladistimaPred ";
            DataTable dt = db.ParamsQueryDT(sql, datum, datumOd, Program.idFirme, ID_Skladiste, ID_Vlasnik);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("");
                return ZatvaranjeIspravka;
            }
           else
            {
                // zapisi stavku u zatvaranju
                db.ExecuteStoreProcedure("ZatvaranjeIOtvaranjeStanja", "Firma:" + Program.idFirme, "PoLotuDaNe:" + "NE", "IdDokView:" + ID_DokumentaView,
                                         "DoDatuma:" + datum, "NazivSkl:" + NazivSkl, "ID_vlasnika:" + ID_Vlasnik,
                                         "IdPrethodni:1", "UUser:" + Program.idkadar);

               db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PocetnoStanjeZaRobu", "IdDokument:" + ID_DokumentaView);
               db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:" + ID_DokumentaView);
            }

            ZatvaranjeIspravka = true;
            return ZatvaranjeIspravka;
        }
        private bool OtvaranjeIspravka(int ID_DokumentaView, string datum, string ID_Skladiste)
        {
            bool OtvaranjeIspravka = false;
            string ID_Vlasnik = "1";
            string NazivSkl = "";
            string ID_Otvaranje = "1";
            datumOd = "01.01." + Convert.ToDateTime(datum).AddYears(+1).Year;

            sql = "select ID_VlasnikRobe,NazivSkl from PocetnoStanjeZaRobuTotali where ID_PocetnoStanjeZaRobuTotali= @param0";
            DataTable dv = db.ParamsQueryDT(sql, ID_DokumentaView);
            if (dv.Rows.Count == 0)
            {
                MessageBox.Show("");
                return OtvaranjeIspravka;
            }
            else
            {
                ID_Vlasnik = dv.Rows[0]["ID_VlasnikRobe"].ToString();
                NazivSkl = dv.Rows[0]["NazivSkl"].ToString();

                sql = "select ID_PocetnoStanjeZaRobuTotali "
                      + " from PocetnoStanjeZaRobuTotali "
                      + " Where brdok like'42-%' and datum = @param0  and "
                      + " ID_Skladiste = @param1 and ID_VlasnikRobe=@param2";

               DataTable dt = db.ParamsQueryDT(sql, datumOd, ID_Skladiste, ID_Vlasnik);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("");
                    return OtvaranjeIspravka;
                }
                else ID_Otvaranje = dt.Rows[0]["ID_PocetnoStanjeZaRobuTotali"].ToString();

                sql = "delete from racunstavke where ID_DokumentaView = @param0";
                ret = db.ParamsInsertScalar(sql, ID_Otvaranje);
                sql = "delete from CeneArtikalaNaSkladistimaPred where id_Dokumentaview = @param0";
                ret = db.ParamsInsertScalar(sql, ID_Otvaranje);

                // zapisi stavku u otvaranju
                db.ExecuteStoreProcedure("ZatvaranjeIOtvaranjeStanja", "Firma:" + Program.idFirme, "PoLotuDaNe:" + "NE", "IdDokView:" + ID_Otvaranje,
                                         "DoDatuma:" + datum, "NazivSkl:" + NazivSkl, "ID_vlasnika:" + ID_Vlasnik,
                                         "IdPrethodni:" + ID_DokumentaView, "UUser:" + Program.idkadar);

                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:PocetnoStanjeZaRobu", "IdDokument:" + ID_Otvaranje);
                db.ExecuteStoreProcedure("CeneArtikalaPoSkladistimaIStanje", "IdDokView:" + ID_Otvaranje);
            }

            OtvaranjeIspravka = true;
            return OtvaranjeIspravka;
        }
    }
}
