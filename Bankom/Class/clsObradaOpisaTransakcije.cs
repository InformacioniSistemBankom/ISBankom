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
    class clsObradaOpisaTransakcije
    {
        private string iddokview;
        private string iid;
        private string PoljeGdeSeUpisujeIId;
        private string imedokumenta;
        private string dokje;
        private string idReda;
        private string UpitiIme;
        private string TTabela = "";
        private Form forma;
        string sql = "";
        string sql1 = "";
        string sql2 = "";
        string sqlInsert = "";
        DataBaseBroker db = new DataBaseBroker();

        public bool ObradiOpisTransakcije()
        {
            bool ObradiOpisTransakcije = false;
            //forma = new Form();
            forma = Program.Parent.ActiveMdiChild;
            iddokview = Convert.ToString(((Bankom.frmChield)forma).iddokumenta);
            idReda = Convert.ToString(((Bankom.frmChield)forma).idReda);
            imedokumenta = forma.Controls["limedok"].Text;
            dokje = forma.Controls["ldokje"].Text;

            //azuriramo tabelu transakcije za odabranu transakciju tako sto brisemo sve slogove koji imaju ID_dokumentaview=iddokview
            //i upisemo ih ponovo funkcijom UpisOpisaTransakcijeUTransakcije
            if (Convert.ToInt32(idReda) ==-1 && forma.Controls["OOperacija"].Text == "IZMENA")
            {
                UpisOpisaTransakcijeUTransakcije(forma, iddokview, idReda);
                ObradiOpisTransakcije = true;
                return ObradiOpisTransakcije;
            }

            //obradjujemo pojedinacnu stavku odabrane transakcije
            if (forma.Controls["OOperacija"].Text == "BRISANJE")
            {
                sql = "Select g.IDOpisTransa,g.brnal from glavnaknjiga as g,transakcije as t "
                    + "where t.id_opisTransakcijeStavke= @param0 and t.ID_Transakcije=g.IDOpisTransa AND Year(g.Datum)>=Year(getdate())-1";
                DataTable dt = db.ParamsQueryDT(sql, idReda);
                if (dt.Rows.Count != 0)
                {
                    MessageBox.Show("Transakcija je koristena ne moze se brisati!!!! ");
                    return ObradiOpisTransakcije;
                }
                else
                {
                    if(ProveraIspravnosti()==false) return ObradiOpisTransakcije;
                }
            }

            sql = "Select* from Upiti WHERE NazivDokumenta = @param0 AND ime like 'Uu%'";
            DataTable du = db.ParamsQueryDT(sql, "dokumenta");
            if (du.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoje upiti za transakciju! ");
                return ObradiOpisTransakcije;
            }
            else
            {
                foreach (DataRow row in du.Rows)
                {
                    int IndUpisan = 0;
                    UpitiIme = row["Ime"].ToString();
                    if (row["Ime"].ToString().Contains("Stavke") == false) //upis zaglavlja
                    {
                        PoljeGdeSeUpisujeIId = "ID_DokumentaView";
                        iid = iddokview;
                        //da li je vec upisano zaglavlje
                        sql = "select ID_DokumentaView from "+ row["Tabela"].ToString() + " where "
                            + "ID_DokumentaView = "+ iddokview ;
                        DataTable duz = db.ReturnDataTable(sql);
                        if (duz.Rows.Count != 0)
                            IndUpisan = 1;
                    }
                    else //upis u stavke
                    {
                        PoljeGdeSeUpisujeIId = "ID_"+ row["Tabela"].ToString();
                        iid = idReda;
                    }

                    if (forma.Controls["OOperacija"].Text == "UNOS")
                    {
                        if (IndUpisan == 1) //vec postoji upisan slog u zaglavlje
                            if (IzmeniSlog() == false)
                                return ObradiOpisTransakcije;
                            else
                                 if (UpisiSlog(row["Tabela"].ToString().Trim()) == false)
                                     return ObradiOpisTransakcije;

                    }

                    if (forma.Controls["OOperacija"].Text == "IZMENA")
                    {
                        if (IzmeniSlog() == false)
                            return ObradiOpisTransakcije;
                    }

                    if (forma.Controls["OOperacija"].Text == "BRISANJE")
                    {
                        if (BrisiStavku(row["Tabela"].ToString().Trim()) == false)
                            return ObradiOpisTransakcije;
                    }

                }

                if (DovrsiObradu() == false)
                    return ObradiOpisTransakcije;

                clsdokumentRefresh cdr = new clsdokumentRefresh();
                cdr.refreshDokumentBody(forma, imedokumenta, iddokview, dokje);
                cdr.refreshDokumentGrid(forma, imedokumenta, iddokview, "","1", "");

                ObradiOpisTransakcije = true;
            }
            
            return ObradiOpisTransakcije;
        }
        public bool UpisOpisaTransakcijeUTransakcije(Form forma,string iddokview, string idreda)
        {
            bool DaNe =false;
            SqlCommand cmd = new SqlCommand();

            if (idreda=="-1")
            {
                sql = "delete from transakcije where id_dokumentaview = @param0";
                DataTable dt = db.ParamsQueryDT(sql, iddokview);
                sql = "select opistransakcije.opistransakcije,konto,opistransakcijestavke.analitika,opistransakcijestavke.skladiste,opistransakcijestavke.banka,opistransakcijestavke.komitent, "
                    + " opistransakcijestavke.artikal,opistransakcijestavke.valuta,opistransakcijestavke.duguje,opistransakcijestavke.potrazuje,opistransakcijestavke.id_opistransakcijestavke "
                    + " from opistransakcije,opistransakcijestavke "
                    + " where opistransakcije.id_dokumentaview="+ iddokview + " and opistransakcije.id_dokumentaview= opistransakcijestavke.id_dokumentaview";
             }
            else
            {
                sql = "delete from transakcije where id_dokumentaview = @param0 and id_opistransakcijestavke = @param1";
                DataTable dt = db.ParamsQueryDT(sql, iddokview,idreda);
                sql = "select opistransakcije.opistransakcije,konto,opistransakcijestavke.analitika,opistransakcijestavke.skladiste,opistransakcijestavke.banka,opistransakcijestavke.komitent, "
                    + " opistransakcijestavke.artikal,opistransakcijestavke.valuta,opistransakcijestavke.duguje,opistransakcijestavke.potrazuje,opistransakcijestavke.id_opistransakcijestavke "
                    + " from opistransakcije,opistransakcijestavke "
                    + " where opistransakcije.id_dokumentaview=" + iddokview + " and opistransakcije.id_dokumentaview= opistransakcijestavke.id_dokumentaview and "
                    + " opistransakcijestavke.id_opistransakcijestavke ="+idreda;
            }
            DataTable dot = db.ReturnDataTable(sql);
           
            sql = " select ulazniizlazni from sifarnikdokumenta where naziv= @param0";
            DataTable dtt = db.ParamsQueryDT(sql, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Tabela").Vrednost);
            if (dtt.Rows.Count != 0)
                TTabela = dtt.Rows[0]["ulazniizlazni"].ToString();

            foreach (DataRow row in dot.Rows)
            {
                DaNe = true;
                sqlInsert = "insert into Transakcije (ID_OpisTransakcijeStavke,ID_DokumentaView,Analitika,Konto,Sif1,Sif2,OdakleJeKomitent,OpisRacuna,OpisSkladista"
            + " ,Koef,Duguje,Potrazuje,Tabela,OpisTarife,OBRADA,OpisTransakcije,Sif1Tabela,Sif2Tabela,DugujeDomVal,PotrazujeDomVal,Valuta"
            + " ,UUser,TTime,Upit,IInsert,KontoTabela)"
            + " values (@ID_OpisTransakcijeStavke, @ID_DokumentaView, @Analitika, @Konto, @Sif1, @Sif2, @OdakleJeKomitent, @OpisRacuna, @OpisSkladista"
            + " , @Koef, @Duguje, @Potrazuje, @Tabela, @OpisTarife, @OBRADA, @OpisTransakcije, @Sif1Tabela, @Sif2Tabela, @DugujeDomVal, @PotrazujeDomVal, @Valuta"
            + " , @UUser, @TTime, @Upit, @IInsert, @KontoTabela)";

                cmd = new SqlCommand(sqlInsert);

                SqlParameter analitika = new SqlParameter();
                analitika.ParameterName = "@Analitika";
                analitika.Value = "";
               
                SqlParameter konto = new SqlParameter();
                konto.ParameterName = "@Konto";
                konto.Value = "";

                SqlParameter sif1 = new SqlParameter();
                sif1.ParameterName = "@Sif1";
                sif1.Value = "";

                SqlParameter sif2 = new SqlParameter();
                sif2.ParameterName = "@Sif2";
                sif2.Value = "";

                SqlParameter OdakleJeKomitent = new SqlParameter();
                OdakleJeKomitent.ParameterName = "@OdakleJeKomitent";
                OdakleJeKomitent.Value = "";

                SqlParameter OpisRacuna = new SqlParameter();
                OpisRacuna.ParameterName = "@OpisRacuna";
                OpisRacuna.Value = "";

                SqlParameter OpisSkladista = new SqlParameter();
                OpisSkladista.ParameterName = "@OpisSkladista";
                OpisSkladista.Value = "";

                SqlParameter Duguje = new SqlParameter();
                Duguje.ParameterName = "@Duguje";
                Duguje.Value = "0";

                SqlParameter Potrazuje = new SqlParameter();
                Potrazuje.ParameterName = "@Potrazuje";
                Potrazuje.Value = "0";

                SqlParameter OpisTarife = new SqlParameter();
                OpisTarife.ParameterName = "@OpisTarife";
                OpisTarife.Value = "";

                SqlParameter Sif1Tabela = new SqlParameter();
                Sif1Tabela.ParameterName = "@Sif1Tabela";
                Sif1Tabela.Value = "";

                SqlParameter Sif2Tabela = new SqlParameter();
                Sif2Tabela.ParameterName = "@Sif2Tabela";
                Sif2Tabela.Value = "";

                SqlParameter DugujeDomVal = new SqlParameter();
                DugujeDomVal.ParameterName = "@DugujeDomVal";
                DugujeDomVal.Value = "0";

                SqlParameter PotrazujeDomVal = new SqlParameter();
                PotrazujeDomVal.ParameterName = "@PotrazujeDomVal";
                PotrazujeDomVal.Value = "0";

                SqlParameter Valuta = new SqlParameter();
                Valuta.ParameterName = "@Valuta";
                Valuta.Value = "";

                SqlParameter KontoTabela = new SqlParameter();
                KontoTabela.ParameterName = "@KontoTabela";
                KontoTabela.Value = "";

                for (int i = 1; i <= dot.Columns.Count - 2; i++)
                {
                    if (row[dot.Columns[i].ToString()].ToString().Trim() != "")
                    {
                        sql1 = "select * from Knj" + dot.Columns[i].ColumnName.ToString() + " where " + dot.Columns[i].ColumnName.ToString() + "='" + row[dot.Columns[i].ToString()].ToString()+"'";//dot.Columns[i].ToString();
                        DataTable dknj = db.ReturnDataTable(sql1);
                        if (dknj.Rows.Count == 0)
                        {
                            MessageBox.Show(" Postoji greska za " + dot.Columns[i].ColumnName.ToString() + " , " + dot.Columns[i].ToString() + " ispravite !!");
                            DaNe = false;
                        }


                        if (dot.Columns[i].ColumnName.ToString().ToUpper() == "KONTO")
                        {
                            if (dknj.Rows[0]["ALIJASPOLJA"].ToString().Trim() != "")
                            {
                                konto.Value = dknj.Rows[0]["ALIJASPOLJA"].ToString();
                                KontoTabela.Value = dknj.Rows[0]["TABELA"].ToString();
                            }
                            else
                                konto.Value = row["KONTO"].ToString();
                        }


                        if (dot.Columns[i].ColumnName.ToString().ToUpper() == "ANALITIKA")
                        {
                            analitika.Value = row["analitika"].ToString();
                            if (TTabela == "PreknjizavanjeKonta")
                                sif1.Value = row["analitika"].ToString();
                            if (TTabela == "TemeljnicaZaIsplatu" || TTabela == "PDVUlazniRacunZaUsluge" || TTabela == "LotInterniNalogZaRobu")
                                sif1.Value = dot.Columns["analitika"].ColumnName.ToString();
                        }

                        if (dot.Columns[i].ColumnName.ToString() == "skladiste")
                        {
                            if (dknj.Rows[0]["OPISSKLADISTA"].ToString() != "OpisSkladista")
                            {
                                sql2 = "select o.* from RecnikPodataka as o, RecnikPodataka as p where o.polje='OpisSkladista' and p.alijaspolja= '"
                                      + dknj.Rows[0]["alijaspolja"].ToString() + "' and o.alijastabele=p.alijastabele and o.dokument=p.dokument and p.dokument ='"
                                      + dknj.Rows[0]["dokument"].ToString()+"'";
                                DataTable drp = db.ReturnDataTable(sql2);
                                Console.WriteLine(sql2);
                                if (drp.Rows.Count == 0)
                                {
                                    MessageBox.Show("Nema Opisa skladista za polje: " + dknj.Rows[0]["OPISSKLADISTA"].ToString());
                                    DaNe = false;
                                }
                                else
                                    OpisSkladista.Value = drp.Rows[0]["alijaspolja"].ToString().Trim() + "=''" + dknj.Rows[0]["OPISSKLADISTA"].ToString() + "''";
                            }

                            if (dknj.Rows[0]["EvidentiraSe"].ToString() == "EvidentiraSe")
                            {
                                if (dknj.Rows[0]["Analitika"].ToString() == "Analitika")
                                {
                                    sif1.Value = dknj.Rows[0]["alijaspolja"].ToString();
                                    Sif1Tabela.Value = "Skladiste";
                                }
                                else
                                {
                                    sif2.Value = dknj.Rows[0]["alijaspolja"].ToString();
                                    Sif2Tabela.Value = "Skladiste";
                                }
                            }
                        }

                        if (dot.Columns[i].ColumnName.ToString() == "banka")
                        {
                            OpisRacuna.Value = row["OpisRacuna"].ToString();
                            if (dknj.Rows[0]["EvidentiraSe"].ToString() == "EvidentiraSe")
                            {
                                if (dknj.Rows[0]["Analitika"].ToString() == "Analitika")
                                {
                                    sif1.Value = dknj.Rows[0]["alijaspolja"].ToString();
                                    Sif1Tabela.Value = "Banka";
                                }
                                else
                                {
                                    sif2.Value = dknj.Rows[0]["alijaspolja"].ToString();
                                    Sif2Tabela.Value = "Banka";
                                }
                            }
                        }

                        if (dot.Columns[i].ColumnName.ToString() == "valuta")
                        {
                            Valuta.Value = dknj.Rows[0]["alijaspolja"].ToString();
                        }

                        if (dot.Columns[i].ColumnName.ToString() == "komitent")
                        {
                            if (dknj.Rows[0]["OdakleJeKomitent"].ToString() != "OdakleJeKomitent")
                            {
                                sql2 = "select o.* from RecnikPodataka as o, RecnikPodataka as p where o.polje='OdakleJeKomitent' and o.TabelavView=p.TabelavView and p.alijaspolja= '"
                                    + dknj.Rows[0]["alijaspolja"].ToString() + "' and o.alijastabele=p.alijastabele and o.dokument=p.dokument and p.dokument ='"
                                    + dknj.Rows[0]["dokument"].ToString()+"'";
                                DataTable drp = db.ReturnDataTable(sql2);
                                Console.WriteLine(sql2);
                                if (drp.Rows.Count == 0)
                                {
                                    MessageBox.Show("Nema Odakle je komitent za polje: " + dknj.Rows[0]["OdakleJeKomitent"].ToString().Trim());
                                    DaNe = false;
                                }
                                else
                                    OdakleJeKomitent.Value = drp.Rows[0]["alijaspolja"].ToString().Trim() + "=''" + dknj.Rows[0]["OdakleJeKomitent"].ToString() + "''";
                            }

                            if (dknj.Rows[0]["EvidentiraSe"].ToString() == "EvidentiraSe")
                            {
                                if (dknj.Rows[0]["Analitika"].ToString() == "Analitika")
                                {
                                    sif1.Value = dknj.Rows[0]["alijaspolja"].ToString();
                                    Sif1Tabela.Value = "KomitentiView";
                                }
                                else
                                {
                                    sif2.Value = dknj.Rows[0]["alijaspolja"].ToString();
                                    Sif2Tabela.Value = "KomitentiView";
                                }
                            }
                        }

                        if (dot.Columns[i].ColumnName.ToString() == "artikal")
                        {
                            if (dknj.Rows[0]["OpisTarife"].ToString() != "OpisTarife")
                                OpisTarife.Value = dknj.Rows[0]["OpisTarife"].ToString();

                            if (dknj.Rows[0]["EvidentiraSe"].ToString() == "EvidentiraSe")
                            {
                                if (dknj.Rows[0]["Analitika"].ToString() == "Analitika")
                                {
                                    sif1.Value = dknj.Rows[0]["alijaspolja"].ToString();
                                    Sif1Tabela.Value = "ArtikliView";
                                }
                                else
                                {
                                    sif2.Value = dknj.Rows[0]["alijaspolja"].ToString();
                                    Sif2Tabela.Value = "ArtikliView";
                                }
                            }
                        }

                        if (dot.Columns[i].ColumnName.ToString() == "OrganizacioniDeo")
                        {
                            if (dknj.Rows[0]["Analitika"].ToString() == "Analitika")
                            {
                                sif1.Value = dknj.Rows[0]["alijaspolja"].ToString();
                                Sif1Tabela.Value = "OrganizacionaStrukturaView";
                            }
                            else
                            {
                                sif2.Value = dknj.Rows[0]["alijaspolja"].ToString();
                                Sif2Tabela.Value = "OrganizacionaStrukturaView";
                            }
                        }

                        if (dot.Columns[i].ColumnName.ToString() == "duguje")
                        {
                            Duguje.Value = dknj.Rows[0]["alijaspolja"].ToString();
                            if (dknj.Rows[0]["alijaspolja"].ToString().Contains("DomVal") == true)
                                DugujeDomVal.Value = dknj.Rows[0]["alijaspolja"].ToString();
                            else
                                DugujeDomVal.Value = dknj.Rows[0]["alijaspolja"].ToString() + "DomVal";
                        }

                        if (dot.Columns[i].ColumnName.ToString() == "potrazuje")
                        {
                            Potrazuje.Value =  dknj.Rows[0]["alijaspolja"].ToString();
                            if (dknj.Rows[0]["alijaspolja"].ToString().Contains("DomVal") == true)
                                PotrazujeDomVal.Value = dknj.Rows[0]["alijaspolja"].ToString();
                            else
                                PotrazujeDomVal.Value = dknj.Rows[0]["alijaspolja"].ToString() + "DomVal";
                        }
                    }
              
                }

                cmd.Parameters.AddWithValue("@ID_OpisTransakcijeStavke", row["ID_OpisTransakcijeStavke"].ToString());
                cmd.Parameters.AddWithValue("@ID_DokumentaView", iddokview);
                cmd.Parameters.Add(analitika);
                cmd.Parameters.Add(konto);
                cmd.Parameters.Add(sif1);
                cmd.Parameters.Add(sif2);
                cmd.Parameters.Add(OdakleJeKomitent);
                cmd.Parameters.Add(OpisRacuna);
                cmd.Parameters.Add(OpisSkladista);
                cmd.Parameters.AddWithValue("@Koef", "0");
                cmd.Parameters.Add(Duguje);
                cmd.Parameters.Add(Potrazuje);
                cmd.Parameters.AddWithValue("@Tabela", TTabela + "Totali");
                cmd.Parameters.Add(OpisTarife);
                cmd.Parameters.AddWithValue("@OBRADA", DateTime.Now);
                cmd.Parameters.AddWithValue("@OpisTransakcije", forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "OpisTransakcije").Vrednost);
                cmd.Parameters.Add(Sif1Tabela);
                cmd.Parameters.Add(Sif2Tabela);
                cmd.Parameters.Add(DugujeDomVal);
                cmd.Parameters.Add(PotrazujeDomVal);
                cmd.Parameters.Add(Valuta);
                cmd.Parameters.AddWithValue("@UUser", Program.idkadar);
                cmd.Parameters.AddWithValue("@TTime", DateTime.Now);
                cmd.Parameters.AddWithValue("@Upit", "");
                cmd.Parameters.AddWithValue("@IInsert", "");
                cmd.Parameters.Add(KontoTabela);

                if (db.Comanda(cmd) == "") { }
                else
                {
                    MessageBox.Show("Greska u upitu!");
                     DaNe = false;
                }

                if (DaNe == false)
                {
                   sql2 = "delete from transakcije where id_dokumentaview = @param0 and id_opistransakcijestavke = @param1";
                   DataTable dt = db.ParamsQueryDT(sql2, iddokview, row["ID_OpisTRansakcijeStavke"].ToString());
                }

            }

            db.ExecuteStoreProcedure("TransakcijeZaKnjizenje", "IdReda:" + idreda, "IdDokView:" + iddokview);
            return DaNe;
        }
        public bool ProveraIspravnosti()
        {
            bool ProveraIspravnosti = false;

            ProveraIspravnosti = true;
            return ProveraIspravnosti;
        }

        public bool DovrsiObradu()
        {
            bool DovrsiObradu = false;

            if (forma.Controls["OOperacija"].Text == "BRISANJE")
            {
                sql = "delete from transakcije where id_dokumentaview=@param0 and id_opistransakcijestavke=@param1 ";
                DataTable du = db.ParamsQueryDT(sql, iddokview, idReda);
            }

            if (forma.Controls["OOperacija"].Text == "UNOS")
            {
                sql = "select max(ID_OpisTransakcijeStavke) as m from OpisTransakcijeStavke ";
                DataTable dm = db.ParamsQueryDT(sql);
                if (dm.Rows.Count != 0)
                    idReda = dm.Rows[0]["m"].ToString();
            }

            UpisOpisaTransakcijeUTransakcije(forma,iddokview, idReda);

            DovrsiObradu = true;
            return DovrsiObradu;
        }
        public bool UpisiSlog(string Tabela)
        {
            bool UpisiSlog = false;
            sql = "Select * From "+Tabela;
            DataTable dt = db.ReturnDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoji UU-Upit !!! ");
                return UpisiSlog;
            }

            if (Tabela.Contains("Stavke")==false)
            {
                sql = "insert into OpisTransakcije(ID_DokumentaView,SifraTransakcije,OpisTransakcije,Tabela,UUser,TTime,CCopy)"
                    + " values(@param0, @param1,@param2,@param3,@param4,@param5,@param6)";
                //DataTable dts = db.ParamsQueryDT(sql, iddokview, row["ID_ArtikliView"].ToString(), Program.idkadar, DateTime.Now, 0);
            }
            else
            {

            }
            //for (int i = 0; i <= dt.Columns.Count - 1; i++)
            //{

            //    forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == dt.Columns[i].ColumnName.ToString()).cEnDis = "true";
            //    foreach (var ctrls in forma.Controls.OfType<Field>())
            //    {
            //       if (ctrls.IME == dt.Columns[i].ColumnName.ToString())
            //            { }
            //            }
            //}
            


                        UpisiSlog = true;
            return UpisiSlog;
        }
        public bool IzmeniSlog()
        {
            bool IzmeniSlog = false;

            IzmeniSlog = true;
            return IzmeniSlog;
        }
        public bool BrisiStavku(string Tabela )
        {
            bool BrisiStavku = false;

            sql= "delete from " +Tabela + " where id_" + Tabela + " = " + idReda;
            // upis u log
            CRUD l = new CRUD();
            l.CreateLog(forma, iddokview,idReda, imedokumenta,forma.Controls["OOperacija"].Text, UpitiIme);
          
            BrisiStavku = true;
            return BrisiStavku;
        }
    }
}
