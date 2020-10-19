using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using Bankom.Class;

namespace Bankom.Class
{
    class clsAvansi
    {
        DataBaseBroker db = new DataBaseBroker();
        string str = "";


        public void PovlacenjeAvansa(Form forma1, string KojiDok, int ParIdDok)

        {


            int IdDokAvans = 0;
            string KojiRacun = "";
            List<string[]> lista = new List<string[]>();
            string strParams = "";

            SqlCommand cmd = new SqlCommand();

            if (KojiDok == "PDVIzlazniRacunZaUsluge" || KojiDok == "PDVUlazniRacunZaUsluge")
            { KojiRacun = "RacunZaUsluge"; }
            else
            { KojiRacun = "Racun"; }

            //IdDokAvans = Convert.ToInt32(db.ReturnElementField(forma1, "Avansi")[0]);
            IEnumerable<string> m_oEnum = null;
            m_oEnum = forma1.Controls.OfType<Field>().Where(k => k.IME == "Avansi").Select(n => n.ID).ToArray();
            IdDokAvans = Convert.ToInt32(((string[])m_oEnum)[0]);

            if (IdDokAvans == 1)
            {
                strParams = "";                
                strParams += "@param1=" + ParIdDok;

                cmd.CommandType = CommandType.Text;
                str = "Update " + KojiRacun  + "Stavke set IznosAvansa = 0 WHERE ID_dokumentaView = @param1";
                lista.Add(new string[] { str, strParams });            /////...
                lista.ToArray();
                string rezultat = db.ReturnSqlTransactionParams(lista);
                // cmd.CommandText = str;
                //  db.Comanda(cmd);
                //  cmd.Dispose();
                return;
            }
            str = "delete from " + KojiRacun + "Stavke Where id_ArtikliView= 1 AND ";
            str += "ID_dokumentaView =" + ParIdDok.ToString();

            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = str;
            db.Comanda(cmd);
            cmd.Dispose();

            str = "Select Sum(IznosAvansa) as UpisaniAvansi from ";
            str += KojiRacun + "Stavke where ID_dokumentaView =" + ParIdDok.ToString();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = str;
            db.Comanda(cmd);
            cmd.Dispose();

            str = "Select Sum(IznosAvansa) as UpisaniAvansi from ";
            str += KojiRacun + "Stavke where ID_dokumentaView = " + ParIdDok.ToString();
            DataTable dt = db.ReturnDataTable(str);


            switch (dt.Rows.Count)
            {
                case 0:
                    break;
                default:
                    UpisiAvans(forma1, KojiDok, KojiRacun, ParIdDok, IdDokAvans, "Update", 0);
                    break;
            }


        }



        private bool UpisiAvans(Form frm,string ZaDokument,string VrstaRacuna, int RacunID, int AvansID , string PKojaOperacija,int TUD)
         {
            
            
            bool UpisiAvans = false;
            DataTable rssupisi = new DataTable();
            DataTable rsavansi = new DataTable();
            DataTable rsracun = new DataTable();
            DataTable rsStavke = new DataTable();
            DataTable rsUkupnoOstatak = new DataTable();
            string BrojAvansnogDokumenta = "";
            string NazivAvansa = "";
            string Prefiks = "";
            int KojiRed = 0;
            string KojaRoba = "";
            string KojaUsluga = "";
            string str = "";
            string sbroj = "";
            string NazivArt = "";
            double Ostatak=0;
            
            str = "select Dokument, BrDok as BrojDokumenta ,RB from DokumentaTotali WITH(NOLOCK) where id_dokumentaTotali = " + AvansID.ToString();
                rssupisi = db.ReturnDataTable(str);
            if (rssupisi.Rows.Count == 0)
            {
                UpisiAvans = false;
                str = "Update " + VrstaRacuna + "Stavke set IznosAvansa = 0 WHERE ID_dokumentaView =" + RacunID.ToString();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = str;
                db.Comanda(cmd);
                cmd.Dispose();


                if (VrstaRacuna.Contains("Usluge") == true)
                {
                    str = "Update " + VrstaRacuna + " set ID_Avansi = 1 WHERE ID_dokumentaView =" + RacunID.ToString();
                }
                else
                {
                    str = "Update " + VrstaRacuna + " set ID_AvansiIzvodi = 1 WHERE ID_dokumentaView =" + RacunID.ToString();
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = str;
                db.Comanda(cmd);
                cmd.Dispose();
                return true;
            }
            UpisiAvans = true;
            NazivAvansa = rssupisi.Rows[0]["Dokument"].ToString();
            BrojAvansnogDokumenta = rssupisi.Rows[0]["BrojDokumenta"].ToString();

            if (ZaDokument.Contains("Ulazni") == false)
            {
                KojaRoba = ZaDokument + "Totali";
                KojaUsluga = "PDVIzlazniRacunzaUslugeTotali";
            }
            else
            {
                KojaRoba = "KonacniUlazniRacunTotali";
                KojaUsluga = "PDVUlazniRacunzaUslugeTotali";
            }
            str = "";
            str = "SELECT isnull(DERIVEDTBL.NazivPoreza,' ')as NazivPoreza,isnull(SUM(DERIVEDTBL.IznosUplate - ISNULL(DERIVEDTBL_1.IskoristenaRoba,0) -ISNULL(DERIVEDTBL_2.IskoristeneUsluge,0)),0) AS Ostaje ";
            str += " FROM (SELECT DISTINCT  NazivPoreza,";
            str += "SUM(IznosUplatePlacanja) AS IznosUplate ";
            str += " FROM " + NazivAvansa.Trim() + "Totali ";
            str += " WHERE BrDok LIKE '" + BrojAvansnogDokumenta + "%'";
            str += " GROUP BY NazivPoreza) DERIVEDTBL FULL JOIN ";
            str += "(SELECT DISTINCT  NazivPoreza,";
            str += "SUM(IznosAvansa) AS IskoristenaRoba ";
            str += " FROM " + KojaRoba.Trim();
            str += " WHERE Avansi LIKE '" + BrojAvansnogDokumenta + "%' and id_" + KojaRoba.Trim() + " <> " + RacunID.ToString();
            str += " GROUP BY NazivPoreza )DERIVEDTBL_1 ON DERIVEDTBL.NazivPoreza = DERIVEDTBL_1.NazivPoreza FULL JOIN";
            str += "(SELECT DISTINCT  NazivPoreza,";
            str += "SUM(IznosAvansa) AS IskoristeneUsluge ";
            str += " FROM " + KojaUsluga.Trim();
            str += " WHERE Avansi LIKE '" + BrojAvansnogDokumenta + "%' and id_" + KojaUsluga.Trim() + " <> " + RacunID.ToString();
            str += " GROUP BY NazivPoreza ) DERIVEDTBL_2 ON DERIVEDTBL.NazivPoreza = DERIVEDTBL_2.NazivPoreza "; 
            str += " GROUP BY DERIVEDTBL.NazivPoreza  ";

            rssupisi = null;
            rssupisi = db.ReturnDataTable(str);

            foreach (DataRow row in rssupisi.Rows)
            {
                if (PKojaOperacija == "Update")
                {

                    
                    Ostatak = Convert.ToDouble(row["Ostaje"]);
                    int IdUpdateReda = -1;
                    IEnumerable<string> m_oEnum = null;
                    m_oEnum = frm.Controls.OfType<Field>().Where(k => k.IME == "idreda").Select(n => n.IME).ToArray();

                    foreach (string value in m_oEnum)
                    {

                        IdUpdateReda = Convert.ToInt32(value);
                    }




                    if (VrstaRacuna == "Racun")
                    {
                        str = " select sum(RacunStavke.IznosAvansa) as SumaIznosAvansa ";
                        str += " from  RacunStavke, " + KojaRoba.Trim().Substring(0, KojaRoba.Trim().Length - 6) + "StavkeView ";
                        str += " where " + KojaRoba.Trim().Substring(0, KojaRoba.Trim().Length - 6) + "StavkeView.id_artikliview  = RacunStavke.id_artikliview";
                        str += " and id_" + KojaRoba.Trim().Substring(0, KojaRoba.Trim().Length - 6) + "StavkeView = RacunStavke.id_dokumentaview";
                        str += " and iid=id_RacunStavke and RacunStavke.id_dokumentaview=" + RacunID.ToString();
                        str += " and RacunStavke.ID_RacunStavke <> " + IdUpdateReda.ToString();
                        str += " and " + KojaRoba.Trim().Substring(0, KojaRoba.Trim().Length - 6) + "StavkeView.PorezIme = '" + Convert.ToString(rssupisi.Rows[0]["NazivPoreza"]).Trim() + "'";

                        rsUkupnoOstatak = db.ReturnDataTable(str);
                        if (rsUkupnoOstatak.Rows.Count > 0)
                        {
                            switch (rsUkupnoOstatak.Rows[0]["SumaiznosAvansa"])
                            {
                                case null:
                                    break;
                                default:
                                    Ostatak = Ostatak - Convert.ToDouble(rsUkupnoOstatak.Rows[0]["SumaiznosAvansa"]);
                                    break;
                            }

                        }

                         m_oEnum = null;
                        m_oEnum = frm.Controls.OfType<Field>().Where(k => k.IME == "IznosAvansa").Select(q => q.Vrednost).ToArray();
                        
                        foreach (string value in m_oEnum)
                        {

                            sbroj = value;
                            break;
                        }
                        if (sbroj == "") { sbroj = "0"; };
                        if (Ostatak > Convert.ToDouble(sbroj))
                        {
                            Ostatak = Convert.ToDouble(sbroj);
                        }

                        m_oEnum = null;
                        m_oEnum = frm.Controls.OfType<Field>().Where(k => k.IME == "NazivArt").Select(q => q.Vrednost).ToArray();
                        foreach (string value in m_oEnum)
                        {

                            NazivArt = value;
                        }
                        //        ' upisujem novu stavku
                        //'NOVO1


                        if (TUD == 0 && NazivArt != "")
                        {
                            rsracun = db.ReturnDataTable("select max(ID_RacunStavke) as max from RacunStavke where id_dokumentaview=" + RacunID.ToString());
                            if (rsracun.Rows[0]["max"] != null)
                            {
                                KojiRed = Convert.ToInt32(rsracun.Rows[0]["max"]);
                            }
                            else
                            {
                                KojiRed = IdUpdateReda;
                            }

                        }
                            if (IdUpdateReda > 0)
                          {
                            str = "";
                            str = "UPDATE RacunStavke set IznosAvansa = " + Ostatak.ToString();
                            str += " from  RacunStavke, " + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView ";
                            str += " where " + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView.id_artikliview  = RacunStavke.id_artikliview";
                            str += " and id_" + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView = RacunStavke.id_dokumentaview";
                            str += " and RacunStavke.id_dokumentaview=" + RacunID.ToString();
                            str += " and RacunStavke.ID_RacunStavke = " + KojiRed.ToString();
                            str += " and (RacunStavke.Kolicina*RacunStavke.ProdajnaCena*(100-RacunStavke.ProcenatRabata)/100)+RacunStavke.PDV >= " + Ostatak.ToString();
                            str += " and " + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView.PorezIme = '" + rssupisi.Rows[0]["NazivPoreza"].ToString().Trim() + "'";
                            string rezultet = db.ReturnString(str, 0);
                            if (rezultet != "") { MessageBox.Show("Greska -UpisiAvansa"); return false; }

                            str = "UPDATE RacunStavke set IznosAvansa =(RacunStavke.Kolicina*RacunStavke.ProdajnaCena*(100-RacunStavke.ProcenatRabata)/100)+RacunStavke.PDV";
                            str += " from  RacunStavke, " + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView ";
                            str += " where " + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView.id_artikliview  = RacunStavke.id_artikliview";
                            str += " and id_" + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView = RacunStavke.id_dokumentaview";
                            str += " and RacunStavke.id_dokumentaview=" + RacunID.ToString();
                            str += " and RacunStavke.ID_RacunStavke = " + KojiRed.ToString();
                            str += " and (RacunStavke.Kolicina*RacunStavke.ProdajnaCena*(100-RacunStavke.ProcenatRabata)/100)+RacunStavke.PDV < " + Ostatak.ToString();
                            str += " and " + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView.PorezIme = '" + rssupisi.Rows[0]["NazivPoreza"].ToString() + "'";
                            rezultet = db.ReturnString(str, 0);
                            if (rezultet != "") { MessageBox.Show("Greska -UpisiAvansa"); return false; }
                        }
                        else
                         {
                            //' kod izmene ide po svim stavkama
                            
                            str = "Select * from RacunStavke, " + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView ";
                            str += " where " + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView.id_artikliview  = RacunStavke.id_artikliview";
                            str += " and id_" + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView = RacunStavke.id_dokumentaview";
                            str += " and Id_DokumentaView = " + RacunID.ToString() + " and RacunStavke.IznosAvansa=0 ";
                            str += " and " + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView.PorezIme = '" + rssupisi.Rows[0]["NazivPoreza"] + "'";
                            rsStavke = db.ReturnDataTable(str);
                            
                            foreach (DataRow red in rsStavke.Rows)
                            {
                                
                            str = "UPDATE RacunStavke set IznosAvansa = " + Ostatak.ToString();
                            str += " from RacunStavke where id_RacunStavke= " + red["id_RacunStavke"];
                            str += " and (RacunStavke.Kolicina*RacunStavke.ProdajnaCena*(100-RacunStavke.ProcenatRabata)/100)+RacunStavke.PDV >= " + Ostatak.ToString();
                            string rezultet = db.ReturnString(str, 0);

                                str = "UPDATE RacunStavke set IznosAvansa =(RacunStavke.Kolicina*RacunStavke.ProdajnaCena*(100-RacunStavke.ProcenatRabata)/100)+RacunStavke.PDV";
                                str += " where id_RacunStavke= " + red["id_RacunStavke"];
                                str += " and (RacunStavke.Kolicina*RacunStavke.ProdajnaCena*(100-RacunStavke.ProcenatRabata)/100)+RacunStavke.PDV < " + Ostatak.ToString();
                                 rezultet = db.ReturnString(str, 0);

                                
                                if (Convert.ToInt32(red["Kolicina"]) * Convert.ToInt32(red["ProdajnaCena"]) * ( 100 - Convert.ToInt32(red["ProcenatRabata"]) / 100) + Convert.ToInt32(red["PDV"]) >= Ostatak ) 
                                    { Ostatak = 0; }
                                else
                                {
                                    Ostatak = Ostatak - (Convert.ToInt32(red["Kolicina"]) * Convert.ToInt32(red["ProdajnaCena"]) * (100 - Convert.ToInt32(red["ProcenatRabata"]) + Convert.ToInt32(red["PDV"]) ));
                                }
            
                            }

                        }
                       

                        }
                        else
                        {
                        str = "select sum(RacunZaUslugeStavke.IznosAvansa) as SumaIznosAvansa";
                        str += " from RacunZaUslugeStavke, " + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView ";
                        str += " where " + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView.id_artikliview  = RacunZaUslugeStavke.id_artikliview";
                        str += " and id_" + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView = RacunZaUslugeStavke.id_dokumentaview";
                        str += " and RacunZaUslugeStavke.id_dokumentaview=" + RacunID.ToString();
                        str += " and RacunZaUslugeStavke.ID_RacunZaUslugeStavke <> " + IdUpdateReda.ToString();
                        str += " and " + KojaRoba.Substring(0, KojaRoba.Length - 6) + "StavkeView.PorezIme = '" + rssupisi.Rows[0]["NazivPoreza"] + "'";

                        rsUkupnoOstatak = db.ReturnDataTable(str);
                        if (rsUkupnoOstatak.Rows.Count > 0 && rsUkupnoOstatak.Rows[0]["SumaiznosAvansa"] != null) { Ostatak = Ostatak - Convert.ToInt32(rsUkupnoOstatak.Rows[0]["SumaiznosAvansa"]); }
                        if (Ostatak >= Convert.ToInt32(sbroj))
                        {
                            Ostatak = Convert.ToInt32(sbroj);
                        }
              
       


                    }
                }
            }
            return UpisiAvans;
         } 

        private bool UpisiAvansPoArtiklu(string ZaDokument, string VrstaRacuna, long RacunID, long AvansID,  string PKojaOperacija)
        {

            bool upisiAvansPoArtiklu = true;
            string KojaOperacija = "";
            long RedniBrojavansDokumenta = 0;
            DataTable rssupisi = new DataTable();
            DataTable rsavansi = new DataTable();
            DataTable rsracun = new DataTable();
            string BrojAvansnogDokumenta = "";
            string NazivAvansa = "";
            string KojaRoba = "";
            string KojaUsluga = "";
            string Prefiks = "";
            string str = "";
            string rezultat = "";


            str = "select Dokument, BrDok as BrojDokumenta ,RB from DokumentaTotali  where id_dokumentaTotali = " + AvansID.ToString();     
            rssupisi = db.ReturnDataTable(str);
            if (rssupisi.Rows.Count == 0)
            {
                rssupisi.Dispose();
                upisiAvansPoArtiklu = false;
                str = "Update " + VrstaRacuna + "Stavke set IznosAvansa = 0 ";
                str += "WHERE ID_dokumentaView =" + RacunID.ToString();
                rezultat = db.ReturnString(str, 0);
                if (VrstaRacuna.Contains("Usluge") == true)
                {
                    str = "Update " + VrstaRacuna + " set ID_Avansi = 1";
                    str = "WHERE ID_dokumentaView =" + RacunID.ToString();
                    rezultat = db.ReturnString(str, 0);

                }
                    
     
        
            }
        else
            {
                str = "Update " + VrstaRacuna + " set ID_AvansiIzvodi = 1";
                str += "WHERE ID_dokumentaView =" + RacunID.ToString();
                rezultat = db.ReturnString(str, 0);
            }
            NazivAvansa = Convert.ToString(rssupisi.Rows[0]["Dokument"]);
            BrojAvansnogDokumenta = Convert.ToString(rssupisi.Rows[0]["BrojDokumenta"]);




            if (ZaDokument.Contains("Ulazni") == false)
            {
                KojaRoba = "KonacniRacunTotali";
                KojaUsluga = "PDVIzlazniRacunzaUslugeTotali";
            }
            else
            {
                KojaRoba = "KonacniUlazniRacunTotali";
                KojaUsluga = "PDVUlazniRacunzaUslugeTotali";
            }



            str = "SELECT isnull(DERIVEDTBL.ID_ArtikliView,0)as ID_ArtikliView,isnull(SUM(DERIVEDTBL.IznosUplate - ISNULL(DERIVEDTBL_1.IskoristenaRoba,0) -ISNULL(DERIVEDTBL_2.IskoristeneUsluge,0)),0) AS Ostaje ";
            str += " FROM (SELECT DISTINCT  ID_ArtikliView,";
            str += "SUM(IznosUplatePlacanja) AS IznosUplate ";
            str += " FROM " + NazivAvansa.Trim() + "Totali ";
            str += " WHERE BrDok LIKE '" + BrojAvansnogDokumenta + "%'";
            str += " GROUP BY ID_ArtikliView ) DERIVEDTBL FULL JOIN ";
            str += "(SELECT DISTINCT  ID_ArtikliView,";
            str += "SUM(IznosAvansa) AS IskoristenaRoba ";
            str += " FROM " + KojaRoba.Trim();
            str += " WHERE Avansi LIKE '" + BrojAvansnogDokumenta + "%' and id_" + KojaRoba.Trim() + " <> " + RacunID.ToString();
            str += " GROUP BY ID_ArtikliView )DERIVEDTBL_1 ON DERIVEDTBL.ID_ArtikliView = DERIVEDTBL_1.ID_ArtikliView FULL JOIN";
            str += "(SELECT DISTINCT  ID_ArtikliView,";
            str += "SUM(IznosAvansa) AS IskoristeneUsluge ";
            str += " FROM " + KojaUsluga.Trim();
            str += " WHERE Avansi LIKE '" + BrojAvansnogDokumenta + "%' and id_" + KojaUsluga.Trim() + " <> " + RacunID.ToString();
            str += " GROUP BY ID_ArtikliView ) DERIVEDTBL_2 ON DERIVEDTBL.ID_ArtikliView = DERIVEDTBL_2.ID_ArtikliView ";
            str += " GROUP BY DERIVEDTBL.ID_ArtikliView  ";
            rssupisi = db.ReturnDataTable(str);

            foreach (DataRow dr in rssupisi.Rows)
            {
                if (PKojaOperacija == "Update")
                {
                    if( VrstaRacuna == "Racun")
                    {
                        str = "UPDATE RacunStavke set IznosAvansa = " + Convert.ToString(dr["Ostaje"]);
                        str += " where id_artikliview=" + Convert.ToString(dr["ID_ArtikliView"]);
                        str += " and id_dokumentaview=" + RacunID.ToString();
                        str += " and (Kolicina*ProdajnaCena*(100-ProcenatRabata)/100)+PDV >= " + Convert.ToString(dr["Ostaje"]);
                        rezultat = db.ReturnString(str,0);

                        str = "UPDATE RacunStavke set IznosAvansa =(Kolicina*ProdajnaCena*(100-ProcenatRabata)/100)+PDV";
                        str += " where id_artikliview=" + Convert.ToString(dr["ID_ArtikliView"]);
                        str += " and id_dokumentaview=" + RacunID.ToString();
                        str += " and (Kolicina*ProdajnaCena*(100-ProcenatRabata)/100)+PDV < " + Convert.ToString(dr["Ostaje"]);
                        rezultat = db.ReturnString(str, 0);

                    }
                    else
                    {
                        str = "UPDATE RacunZaUslugeStavke set IznosAvansa = " + Convert.ToString(dr["Ostaje"]);
                        str += " where id_artikliview=" + Convert.ToString(dr["ID_ArtikliView"]);
                        str += " and id_dokumentaview=" + RacunID.ToString();
                        str += " and (FakturnaVrednost+PDV )>= " + Convert.ToString(dr["Ostaje"]);
                        rezultat = db.ReturnString(str, 0);

                        str = "UPDATE RacunZaUslugeStavke set IznosAvansa =  (FakturnaVrednost+PDV )";
                        str += " where id_artikliview=" + Convert.ToString(dr["ID_ArtikliView"]);
                        str += " and id_dokumentaview=" + Convert.ToString(RacunID);
                        str += " and (FakturnaVrednost+PDV )< " + Convert.ToString(dr["Ostaje"]);
                        rezultat = db.ReturnString(str, 0);
                    }

                }
            }

        return true;
        }
    }
}