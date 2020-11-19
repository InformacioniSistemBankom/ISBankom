using System;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bankom.Class
{
    class clsDokumentaStablo
    {
        //private int ID_DokumentaStablo;
        //private string BrDok;
        private string datum;
        private string Prethodni = "";
        private string ID_Prethodni = "1";
        //private int IdDokView = 1;
        string sql = "";
        private string Dokument;
        private string NazivKlona;
        private string NacinRegistracije;
        private string prikaz;
        //private string vrsta;
        private string OdakleSePreuzima;
        private string TrebaProvera = "0";
        private string DokumentJe = "";
        private string Naslov = "";
        private string PutanjaDokumenta = "";
        //int ret = 0;
        DataBaseBroker db = new DataBaseBroker();

        public bool Obradi(int IdDokView, ref int ID_DokumentaStablo, ref string Dokument, ref string BrDok)
        {
            bool Obradi = false;

            sql = "select distinct d.Dokument,d.ID_DokumentaStablo,BrDok,datum,Predhodni,ID_Predhodni from dokumentatotali as d WITH(NOLOCK) where  d.id_dokumentatotali=@param0 ";
            DataTable dt = db.ParamsQueryDT(sql, IdDokView);
            if (dt.Rows.Count != 0)
            {
                Dokument = dt.Rows[0]["Dokument"].ToString();
                ID_DokumentaStablo = Convert.ToInt32(dt.Rows[0]["ID_DokumentaStablo"]);
                BrDok = dt.Rows[0]["BrDok"].ToString();
                datum = dt.Rows[0]["datum"].ToString();
                Prethodni = dt.Rows[0]["Predhodni"].ToString();
                ID_Prethodni = dt.Rows[0]["ID_Predhodni"].ToString();
            }

            sql = "select UlazniIzlazni  as NazivKlona,NacinRegistracije,PutanjaZaCuvanje,prikaz,vrsta,OdakleSePreuzima from SifarnikDokumenta where Naziv=@param0";
            DataTable dt1 = db.ParamsQueryDT(sql, Dokument);
            if (dt1.Rows.Count != 0)
            {
                NazivKlona = dt1.Rows[0]["NazivKlona"].ToString();
                NacinRegistracije = dt1.Rows[0]["NacinRegistracije"].ToString();
                prikaz = dt1.Rows[0]["prikaz"].ToString().Trim();
                DokumentJe = dt1.Rows[0]["vrsta"].ToString().Trim();
                OdakleSePreuzima = dt1.Rows[0]["OdakleSePreuzima"].ToString().Trim();
                PutanjaDokumenta = dt1.Rows[0]["PutanjaZaCuvanje"].ToString().Trim();
            }

            sql = "select oorderby as TrebaProvera from recnikpodataka where dokument=@param0 and oorderby>0 and oorderby<5";
            DataTable dt2 = db.ParamsQueryDT(sql, Dokument);
            if (dt2.Rows.Count != 0) TrebaProvera = dt2.Rows[0]["TrebaProvera"].ToString();

            if (Dokument.Trim() == "") return Obradi;
            // provera dozvola se zove kod otvaranja forme
            clsProveraDozvola cPd = new clsProveraDozvola();
            Obradi = cPd.ProveriDozvole(Dokument, ID_DokumentaStablo.ToString(), IdDokView.ToString(), DokumentJe);

            if (Obradi == false)
            {
                MessageBox.Show("Nije dozvoljen pristup");
                return Obradi;
            }

            sql = "select NazivJavni from dokumentastablo where id_dokumentastablo=@param0";
            DataTable dt3 = db.ParamsQueryDT(sql, ID_DokumentaStablo);
            if (dt3.Rows.Count != 0) Naslov = dt3.Rows[0]["NazivJavni"].ToString();

            if (NacinRegistracije == "B")
            {
                if (Dokument.Contains("OpisTransakcije") == true) return true;
                sql = "select ID_" + NazivKlona.Trim() + "Totali from " + NazivKlona.Trim() + "Totali where ID_" + NazivKlona.Trim() + "Totali=" + IdDokView;
                DataTable dt4 = db.ReturnDataTable(sql);
                if (dt4.Rows.Count != 0)
                {
                    if (NazivKlona != "PrometUplata")
                        return true;
                }
            }
            if (NacinRegistracije == "W" || NacinRegistracije == "E" || NacinRegistracije == "P")
            {
              
                ObradaWordExcelPdf.OtvoriDokument(NacinRegistracije,PutanjaDokumenta,BrDok);
                return false;
            }

            if (NacinRegistracije == "B" && OdakleSePreuzima.Trim() != "")
            {
                PovlacenjeSaPrethodnika(IdDokView);
            }


            Obradi = true;
            return Obradi;

        }

        public Boolean PovlacenjeSaPrethodnika(int IdDokView)
        {
            Boolean PovlacenjeSaPrethodnika = false;

            string str = "";
            string strParams = "";
            List<string[]> lista = new List<string[]>();
            string strTabela = "Dokumenta";
            string DDDokument = "";
            string DokumentP = "";
            string rezultat = "";
            string dokType = "";
            string MagPolje = "";
            string Id_MagPolje = "1";
            string Id_Skladiste = "1";
            string ID_ArtikliView = "1";
            string ID_JedinicaMere = "1";
            string NazivArtikla = "";
            int i = 0;
            string[] lot2 = new string[1];
            string[] kol = new string[1];
            string[] IdMagPolje = new string[1];
            decimal pom = 0;
            decimal pom2 = 0;
            decimal pom3 = 0;

            DataTable ts = new DataTable();
            DataTable tt = new DataTable();
            switch (OdakleSePreuzima)
            {
                case "P":
                    {
                        if (Prethodni.Trim() == "") return PovlacenjeSaPrethodnika;
                        if (MessageBox.Show("Da li preuzimate podatke sa predhodnog dokumenta ?", "Povlacenje sa predhodnika", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            sql = "select  ds.naziv as Dokument , d.* from dokumenta as d, dokumentastablo as ds "
                               + " where ds.id_dokumentaStablo=d.id_DokumentaStablo and d.id_dokumenta=@param0";

                            DataTable dt = db.ParamsQueryDT(sql, ID_Prethodni);
                            if (dt.Rows.Count != 0)
                            {
                                DokumentP = dt.Rows[0]["Dokument"].ToString();
                                sql = "select UlazniIzlazni  as NazivKlona,NacinRegistracije,prikaz,vrsta,OdakleSePreuzima from SifarnikDokumenta where Naziv=@param0";
                                DataTable dt1 = db.ParamsQueryDT(sql, DokumentP);
                                if (dt1.Rows.Count != 0)
                                {
                                    DDDokument = dt1.Rows[0]["NazivKlona"].ToString();
                                }
                            }

                            if (NazivKlona == "LotOtpremnica")
                            {
                                MagPolje = Prompt.ShowDialog("", "Preuzimanje otpremnice ", "Unesite magacinsko polje: ");
                                if (MagPolje == "") { MessageBox.Show("Niste uneli magacinsko polje ponovite !!!"); return PovlacenjeSaPrethodnika; }
                                sql = "Select ID_MagacinskaPoljaStavkeView as ID_MagacinskaPolja From MagacinskaPoljaStavkeView where NazivPolja=@param0";
                                DataTable dt2 = db.ParamsQueryDT(sql, MagPolje);
                                if (dt2.Rows.Count != 0)
                                {
                                    Id_MagPolje = dt2.Rows[0]["ID_MagacinskaPolja"].ToString();
                                }
                                else
                                {
                                    MessageBox.Show("Pogresna vrednost za  polje: " + MagPolje);
                                    return PovlacenjeSaPrethodnika;
                                }
                            }

                            str = "Execute PrethSled '" + DDDokument + "', '" + NazivKlona.Trim() + "', " + ID_Prethodni + ", " + IdDokView;
                            lista.Add(new string[] { str, strParams, strTabela, dokType, IdDokView.ToString() });
                            lista.ToArray();

                            if (NazivKlona == "LotOtpremnica")
                            {
                                sql = "Select ID_KomitentiView  From " + DDDokument.Trim() + "Totali where ID_" + DDDokument.Trim() + "Totali=" + ID_Prethodni;
                                DataTable dtt = db.ReturnDataTable(sql);
                                if (dtt.Rows.Count != 0)
                                {
                                    strParams = "@param1=" + dtt.Rows[0]["ID_KomitentiView"].ToString() + "`";
                                    strParams += "@param2=" + IdDokView.ToString() + "`";
                                    str = "update otpremnica set [ID_MestoIsporuke]=@param1 Where [ID_DokumentaView] = @param2";
                                    lista.Add(new string[] { str, strParams, "otpremnica", dokType, IdDokView.ToString() });
                                    lista.ToArray();
                                }

                                strParams = "@param1=" + DokumentP + "`";
                                strParams += "@param2=" + IdDokView.ToString() + "`";
                                str = "update otpremnica set [BrojOtp]=@param1 Where [ID_DokumentaView] = @param2";
                                lista.Add(new string[] { str, strParams, "otpremnica", dokType, IdDokView.ToString() });
                                lista.ToArray();

                                strParams = "@param1=Obrenovac`";
                                strParams += "@param2=" + IdDokView.ToString() + "`";
                                str = "update otpremnica set [MestoUtovara]=@param1 Where [ID_DokumentaView] = @param2";
                                lista.Add(new string[] { str, strParams, "Otpremnica", dokType, IdDokView.ToString() });
                                lista.ToArray();

                                strParams = "@param1=" + Program.idkadar.ToString() + "`";
                                strParams += "@param2=" + IdDokView.ToString() + "`";
                                str = "update otpremnica set [ID_Magacioner]=@param1 Where [ID_DokumentaView] = @param2";
                                lista.Add(new string[] { str, strParams, "Otpremnica", dokType, IdDokView.ToString() });
                                lista.ToArray();

                                sql = " select  ID_ArtikliView ,Kolicina , ID_Skladiste,NazivArtikla,otpremnicastavke.ID_JedinicaMere "
                                    + " from Otpremnica, Otpremnicastavke,artikli "
                                    + " where otpremnica.id_dokumentaview = Otpremnicastavke.id_dokumentaview and ID_ArtikliView =id_Artikli "
                                    + " and otpremnica.id_dokumentaview =@param0";
                                DataTable dt3 = db.ParamsQueryDT(sql, ID_Prethodni);
                                if (dt3.Rows.Count != 0)
                                {
                                    Id_Skladiste = dt3.Rows[0]["ID_Skladiste"].ToString();

                                    sql = " select ID_MagacinskaPoljaStavkeView  From MagacinskaPoljaStavkeView where NazivPolja=@param0 "
                                    + " and ID_Skladiste =@param1 ";
                                    DataTable dt4 = db.ParamsQueryDT(sql, MagPolje, Id_Skladiste);
                                    if (dt4.Rows.Count == 0)
                                        return PovlacenjeSaPrethodnika;

                                    foreach (DataRow row in dt3.Rows)
                                    {
                                        ID_ArtikliView = row["ID_ArtikliView"].ToString();
                                        NazivArtikla = row["NazivArtikla"].ToString();
                                        ID_JedinicaMere = row["ID_JedinicaMere"].ToString();
                                        pom = 0;
                                        sql = " select  ID_LotView, SUM(Stanje) AS saldo, ID_MagacinskaPolja  from StanjeRobeNaSkl "
                                            + " where ID_ArtikliView=@param0 and ID_MagacinskaPolja =@param1"
                                            + " and ID_Skladiste =@param2 "
                                            + " group by ID_LotView, DatumIsteka, ID_MagacinskaPolja "
                                            + " having (Sum(Stanje) > 0) "
                                            + " order by DatumIsteka ";
                                        DataTable dt5 = db.ParamsQueryDT(sql, ID_ArtikliView, Id_MagPolje, Id_Skladiste);

                                        if (dt5.Rows.Count == 0)
                                        {
                                            MessageBox.Show("Nema dovoljno " + NazivArtikla + " na polju: " + MagPolje);

                                            strParams = "@param1=" + IdDokView.ToString() + "`";
                                            strParams += "@param2=" + ID_ArtikliView + "`";
                                            str = "delete from OtpremnicaStavke where [ID_DokumentaView] = @param1 and [ID_ArtikliView]=@param2";
                                            lista.Add(new string[] { str, strParams, "OtpremnicaStavke", dokType, IdDokView.ToString() });
                                            lista.ToArray();

                                            goto Sledeci;
                                        }
                                        i = 0;
                                        pom2 = 0;
                                        foreach (DataRow row1 in dt5.Rows)
                                        {
                                            lot2[i] = row1["ID_LotView"].ToString();
                                            pom += Convert.ToDecimal(row1["saldo"].ToString());
                                            IdMagPolje[i] = row1["ID_MagacinskaPolja"].ToString();

                                            if (pom >= Convert.ToDecimal(row["Kolicina"]))
                                            {
                                                if (i == 0) kol[i] = row["Kolicina"].ToString();
                                                else
                                                {
                                                    pom3 = (Convert.ToDecimal(row["Kolicina"]) - pom2);
                                                    kol[i] = pom3.ToString();
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                pom2 += Convert.ToDecimal(row1["saldo"].ToString());
                                                kol[i] = row1["saldo"].ToString();
                                                i += 1;
                                                Array.Resize(ref lot2, i + 1);
                                                Array.Resize(ref kol, i + 1);
                                                Array.Resize(ref IdMagPolje, i + 1);
                                            }

                                        }

                                        if (pom < Convert.ToDecimal(row["Kolicina"]))
                                        {
                                            MessageBox.Show("Nema dovoljno " + NazivArtikla + " na polju: " + MagPolje);

                                            strParams = "@param1=" + IdDokView.ToString() + "`";
                                            strParams += "@param2=" + ID_ArtikliView + "`";
                                            str = "delete from OtpremnicaStavke where [ID_DokumentaView] = @param1 and [ID_ArtikliView]=@param2";
                                            lista.Add(new string[] { str, strParams, "OtpremnicaStavke", dokType, IdDokView.ToString() });
                                            lista.ToArray();

                                            goto Sledeci;
                                        }


                                        for (int c = 0; c <= i; c++)
                                        {
                                            if (c == 0)
                                            {
                                                strParams = "@param1=" + kol[c] + "`";
                                                strParams += "@param2=" + lot2[c] + "`";
                                                strParams += "@param3=" + IdMagPolje[c] + "`";
                                                strParams += "@param4=" + IdDokView.ToString() + "`";
                                                strParams += "@param5=" + ID_ArtikliView + "`";
                                                str = "update OtpremnicaStavke set [kolicina]=@param1,[ID_LotView]=@param2, [ID_MagacinskaPolja]=@param3"
                                                    + " where [ID_DokumentaView] = @param4 and  [ID_ArtikliView]=@param5";
                                                lista.Add(new string[] { str, strParams, "OtpremnicaStavke", dokType, IdDokView.ToString() });
                                                lista.ToArray();
                                            }
                                            else
                                            {
                                                strParams = "@param1=" + IdDokView.ToString() + "`";
                                                strParams += "@param2=" + ID_JedinicaMere + "`";
                                                strParams += "@param3=" + ID_ArtikliView + "`";
                                                strParams += "@param4=" + kol[c] + "`";
                                                strParams += "@param5=0`";
                                                strParams += "@param6=0`";
                                                strParams += "@param7= `";
                                                strParams += "@param8=" + Program.idkadar.ToString() + "`";
                                                strParams += "@param9=" + DateTime.Now + "`";
                                                strParams += "@param10=" + lot2[c] + "`";
                                                strParams += "@param11=" + IdMagPolje[c] + "`";
                                                strParams += "@param12= `";

                                                str = "Insert Into OtpremnicaStavke ( [ID_DokumentaView], [ID_JedinicaMere],";
                                                str += " [ID_ArtikliView], [Kolicina], [ProdajnaCena], [KolicinaPoDokumentu],";
                                                str += " [Primedba],[UUser],[TTime],[ID_LotView],[ID_MagacinskaPolja],[Paleta])";
                                                str += " values (@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12)";

                                                lista.Add(new string[] { str, strParams, "OtpremnicaStavke", dokType, IdDokView.ToString() });
                                                lista.ToArray();
                                            }
                                        }

                                    Sledeci:;
                                    }
                                }
                            }// Kraj LOtOtpremnica
                            if (NazivKlona == "KonacniRacun" || NazivKlona == "PDVIzlazniRacunZaUsluge")
                            {
                                strParams = "@param1=1`";
                                strParams += "@param2=" + IdDokView.ToString() + "`";
                                str = "update Racun set [ID_Avansi]=@param1 Where [ID_DokumentaView] = @param2";
                                lista.Add(new string[] { str, strParams, "Racun", dokType, IdDokView.ToString() });
                                lista.ToArray();

                                strParams = "@param1=1`";
                                strParams += "@param2=" + IdDokView.ToString() + "`";
                                str = "update RacunZaUsluge set [ID_AvansiIzvodi]=@param1 Where [ID_DokumentaView] = @param2";
                                lista.Add(new string[] { str, strParams, "RacunZaUsluge", dokType, IdDokView.ToString() });
                                lista.ToArray();
                            }
                            //MENJAMO POLJA KOJA NE TREBAJU POSTOJATI U NOVOM DOKUMENTU A ZAPISANA SU U TOKU POVLACENJA SA PREDHODNIKA
                            if (NazivKlona == "KonacniUlazniRacun" || Dokument == "UlazniAvansniRacun")
                            {
                                strParams = "@param1= `";
                                strParams += "@param2=" + IdDokView.ToString() + "`";
                                str = "update Racun set [BrUr]=@param1 Where [ID_DokumentaView] = @param2";
                                lista.Add(new string[] { str, strParams, "Racun", dokType, IdDokView.ToString() });
                                lista.ToArray();


                                strParams = "@param1= `";
                                strParams += "@param2=" + IdDokView.ToString() + "`";
                                str = "update Racun set [Otpremnica]=@param1 Where [ID_DokumentaView] = @param2";
                                lista.Add(new string[] { str, strParams, "Racun", dokType, IdDokView.ToString() });
                                lista.ToArray();
                            }

                            if (NazivKlona == "PDVUlazniRacunZaUsluge")
                            {
                                strParams = "@param1= `";
                                strParams += "@param2=" + IdDokView.ToString() + "`";
                                str = "update RacunZaUsluge set [BrUr]=@param1 Where [ID_DokumentaView] = @param2";
                                lista.Add(new string[] { str, strParams, "RacunZaUsluge", dokType, IdDokView.ToString() });
                                lista.ToArray();
                            }

                            if (NazivKlona.Contains("lazniJCI") == true)
                            {
                                //strParams = "@param1=1`"; ///+ ID_Prethodni + "`"; //?????????????????????? borka 070920
                                //strParams += "@param2=" + IdDokView.ToString() + "`";
                                //str = "update JCI set [ID_RacunTotali]=@param1 Where [ID_DokumentaView] = @param2";
                                //lista.Add(new string[] { str, strParams, "JCI", dokType, IdDokView.ToString() });
                                //lista.ToArray();
                            }

                            str = "Execute TotaliZaDokument '" + NazivKlona.Trim() + "'," + IdDokView.ToString();
                            lista.Add(new string[] { str, strParams, strTabela, dokType, "" });
                            lista.ToArray();

                            sql = "select polje from recnikpodataka where dokument=@param0 and polje=@param1";
                            tt = db.ParamsQueryDT(sql, NazivKlona, "NazivSkl");
                            if (tt.Rows.Count > 0)
                            {
                                // poziv potrebnih storedprocedura
                                str = "Execute CeneArtikalaPoSkladistimaIStanje " + IdDokView.ToString();
                                lista.Add(new string[] { str, strParams, strTabela, dokType, "" });
                                lista.ToArray();

                                str = "Execute StanjeRobePoLotu " + IdDokView.ToString();
                                lista.Add(new string[] { str, strParams, strTabela, dokType, "" });
                                lista.ToArray();

                                // provera stanja nakon povlacenja slogova sa predhodnika
                                if (TrebaProvera != "0")
                                {
                                    dokType = "";
                                    strParams = "";
                                    str = "Execute stanje 'ssss'";
                                    strTabela = NazivKlona;
                                    lista.Add(new string[] { str, strParams, strTabela, dokType, "" });
                                    lista.ToArray();
                                }
                            }
                            rezultat = db.ReturnSqlTransactionParamsFull(lista);
                            if (rezultat.Trim() != "") { lista.Clear(); MessageBox.Show(rezultat); return false; }
                            lista.Clear();

                        }
                    }
                    break;
                case "F":
                case "M":

                    if (NazivKlona == "KursnaLista")
                    {
                        string format = "dd.MM.yy";
                        CultureInfo provider = CultureInfo.InvariantCulture;




                        string Datum = Prompt.ShowDialog("", "Prepis kursne liste", "Izaberite datum iz kog prepisujete u formatu dd.mm.yy");
                        if (Datum.Trim()=="")
                        {
                            PovlacenjeSaPrethodnika = false;
                            break;
                        }
                        DateTime DatumKursneListe = DateTime.ParseExact(Datum, format, provider);
                        sql = "select ID_DokumentaView from KursnaLista where Datum = @param0 ";
                        DataTable dkl = db.ParamsQueryDT(sql, DatumKursneListe);
                        if (dkl.Rows.Count == 0)
                        {
                            MessageBox.Show("Nema kursne liste za taj datum u bazi");
                        }
                        else
                        {
                            sql = "select datum from Dokumenta where Id_Dokumenta = @param0 ";
                            DataTable dkl1 = db.ParamsQueryDT(sql, IdDokView);
                            if (dkl1.Rows.Count == 0)
                            {
                                MessageBox.Show("Nije registrovan dokument kursna lista!");
                            }
                            else
                            {
                                sql = "select datum from KursnaLista where Datum = @param0 ";
                                DataTable dkl2 = db.ParamsQueryDT(sql, dkl1.Rows[0]["datum"].ToString());
                                if (dkl2.Rows.Count == 0)
                                {
                                    db.ExecuteStoreProcedure("[PrepisKursneListe]", "Datum:" + dkl1.Rows[0]["datum"].ToString(),
                                                             "ID_DokumentaViewPreth:" + dkl.Rows[0]["ID_DokumentaView"].ToString(),
                                                              "ID_DokumentaViewSledb:" + IdDokView.ToString());
                                    db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + NazivKlona, "IdDokument:" + IdDokView);
                                }
                                else
                                {
                                    MessageBox.Show("Kursna Lista za taj datum vec postoji!");
                                }
                            }
                        }
                    }
                    break;
            }
            PovlacenjeSaPrethodnika = true;
            return PovlacenjeSaPrethodnika;

        }
    }
}
