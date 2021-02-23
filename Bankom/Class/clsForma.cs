using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bankom.Model;
using Bankom.Class;
using System.Data.SqlClient;

//Djora 30.11.20
namespace Bankom.Class
{
    class clsForma
    {
        mForma pforma = new mForma();
        List<mKontrola> pkontrole = new List<mKontrola>();

        private Form forma;

        //Kreira Virtualnu formu u radu sa programom na osnovu prave forme
        public mForma VirtualnaForma()
        {
            //Instanca Virtualne Forme
            //mForma pforma = new mForma();
            //List<mKontrola> pkontrole = new List<mKontrola>();

            //Tekuca forma
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;

            //Promenjive na tekucoj frmChield formi
            string iddok = Convert.ToString(((Bankom.frmChield)forma).iddokumenta);
            string idreda = Convert.ToString(((Bankom.frmChield)forma).idReda);
            string imegrida = Convert.ToString(((Bankom.frmChield)forma).imegrida);

            pforma.OOperacija = Convert.ToString(((Bankom.frmChield)forma).OOperacija.Text);
            pforma.lidstablo = ((Bankom.frmChield)forma).idstablo;
            pforma.ldokje = Convert.ToString(((Bankom.frmChield)forma).DokumentJe);
            pforma.limegrida = Convert.ToString(((Bankom.frmChield)forma).imegrida);
            pforma.iddokumenta = Convert.ToString(((Bankom.frmChield)forma).iddokumenta);
            pforma.idreda = ((Bankom.frmChield)forma).idReda;

            pforma.imedokumenta = ((Bankom.frmChield)forma).imedokumenta;
            //pforma.kontrole.AddRange(pkontrole);


            //pforma.imedokumenta = ((Bankom.frmChield)forma).imedokumenta;
            foreach (Control control in forma.Controls)
            {
                //if (control.GetType() )
                //Console.WriteLine(control.GetType());

                if (control.GetType() == typeof(Field))
                {

                    Field kontrola = (Field)forma.Controls[control.Name];

                    //Console.WriteLine(kontrola.Stavke);
                    //Console.WriteLine(kontrola.ctekst);

                    Console.WriteLine(kontrola.IME);
                    Console.WriteLine(kontrola.ID);
                    Console.WriteLine("-------------------------");

                    pkontrole.Add(new mKontrola()
                    {
                        IME = kontrola.IME,
                        cIzborno = kontrola.cIzborno,
                        cAlijasTabele = kontrola.cAlijasTabele,
                        cTabela = kontrola.cTabela,
                        cDokument = kontrola.cDokument,
                        ID = kontrola.ID,
                        cSegment = kontrola.cSegment,
                        TipKontrole = kontrola.TipKontrole,
                        Vrednost = kontrola.Vrednost,
                        cPolje = kontrola.cPolje,
                        cTabelaVView = kontrola.cTabelaVView,
                        Stavke = kontrola.Stavke,
                        cCaption = kontrola.ctekst,
                        cIdNaziviNaFormi = kontrola.cIdNaziviNaFormi,
                        cEnDis = kontrola.cEnDis,
                        cRestrikcije = kontrola.cRestrikcije,
                        cFormulaForme = kontrola.cFormulaForme
                    });

                    //Console.WriteLine(kontrola.Vrednost);
                }
                else
                {
                    //Console.WriteLine(control.Text);
                }

            }

            pforma.kontrole.AddRange(pkontrole);

            //Console.WriteLine(pforma.kontrole.Count);

            //OBRISATI OVO ISPOD
            foreach (var ww in pforma.kontrole)
            {
                Console.WriteLine("cPolje --> " + ww.cPolje);
                Console.WriteLine("cRestrikcije --> " + ww.cRestrikcije);
                Console.WriteLine("cSegment --> " + ww.cSegment);
                Console.WriteLine("cTabela --> " + ww.cTabela);
                Console.WriteLine("cTabelaVView --> " + ww.cTabelaVView);
                Console.WriteLine("cCaption --> " + ww.cCaption);
                Console.WriteLine("ID --> " + ww.ID);
                Console.WriteLine("IME --> " + ww.IME);
                Console.WriteLine("Stavke --> " + ww.Stavke);
                Console.WriteLine("Vrednost --> " + ww.Vrednost);
                Console.WriteLine("cAlijasTabele --> " + ww.cAlijasTabele);
                Console.WriteLine("cDokument --> " + ww.cDokument);
                Console.WriteLine("TipKontrole --> " + ww.TipKontrole);
                Console.WriteLine("cEnDis --> " + ww.cEnDis);
                Console.WriteLine("cFormulaForme --> " + ww.cFormulaForme);
                Console.WriteLine("cIdNaziviNaFormi --> " + ww.cIdNaziviNaFormi);
                Console.WriteLine("cIzborno --> " + ww.cIzborno);
                Console.WriteLine("---------------");

            }

            return pforma;
        }

        //public string ProbaForma(Form forma1, string field)
        public string ProbaForma(List<mForma> polja)
        {

            string IInsert = "";
            string SSelectZaInsert = "";
            string UVView;


            foreach (var pforma in polja)
            {
                //Console.WriteLine(pforma.Tip);
                //Console.WriteLine(pforma.Naziv);
                //Console.WriteLine(pforma.Svojstvo);
                //Console.WriteLine(pforma.Vrednost);
                //Console.WriteLine("------------------------");
            }

            UVView = "DokumentaStavkeView";

            return "";
        }

        //Djora 30.11.20
        //Kreira Virtualnu formu na osnovu podataka iz baze, a u vezi je sa testiranjem
        public mForma VirtualnaFormaIzBaze(string NazivDokumenta, int iddokumenta, int idstablo, string idokje)
        {
            //Za registraciju dokumenta
            pforma.OOperacija = "UNOS";
            pforma.lidstablo = idstablo;
            pforma.ldokje = "S";
            pforma.limegrida = "GgRrDokumentaStavke";
            pforma.iddokumenta = "1";
            pforma.idreda = -1;

            NazivDokumenta = "Dokumenta";

            //npr. "KonacniUlazniRacun"
            pforma.imedokumenta = NazivDokumenta;

            DataBaseBroker db = new DataBaseBroker();
            DataTable t = new DataTable();

            string query = " SELECT RecnikPodatakaDjora.ID_RecnikPodataka AS ID, RecnikPodatakaDjora.Clevo as levo, RecnikPodatakaDjora.Cvrh as vrh, RecnikPodatakaDjora.Cwidth as width, RecnikPodatakaDjora.height, "
                 + " RecnikPodatakaDjora.AlijasPolja, RecnikPodatakaDjora.AlijasTabele, dbo.TipoviPodataka.tip, dbo.TipoviPodataka.velicina, dbo.TipoviPodataka.naziv, dbo.TipoviPodataka.DifoltZaIzvestaje, "
                 + " dbo.TipoviPodataka.Format, dbo.TipoviPodataka.Alajment, RecnikPodatakaDjora.Izborno, RecnikPodatakaDjora.Polje, RecnikPodatakaDjora.PostojiLabela, RecnikPodatakaDjora.ID_NaziviNaFormi, "
                 + " RecnikPodatakaDjora.TUD, RecnikPodatakaDjora.TabelaVView, RecnikPodatakaDjora.StornoIUpdate, RecnikPodatakaDjora.Tabela, dbo.Prevodi.Prevod as Srpski, "
                 + " dbo.TipoviPodataka.CSharp as FormatPolja,RecnikPodatakaDjora.Restrikcije,RecnikPodatakaDjora.JJoinTvV as ImaNaslov,RecnikPodatakaDjora.FormulaForme"
                 + " FROM dbo.RecnikPodataka AS RecnikPodatakaDjora INNER JOIN "
                 + " dbo.TipoviPodataka ON RecnikPodatakaDjora.ID_TipoviPodataka = dbo.TipoviPodataka.ID_TipoviPodataka INNER JOIN "
                 + " dbo.Prevodi ON RecnikPodatakaDjora.ID_NaziviNaFormi = dbo.Prevodi.ID_NaziviNaFormi "
                 + " WHERE(RecnikPodatakaDjora.Dokument = N'" + NazivDokumenta + "') AND(RecnikPodatakaDjora.TabIndex >= 0) AND(RecnikPodatakaDjora.width <> 0) AND(dbo.Prevodi.ID_Jezik = " + Program.ID_Jezik.ToString() + ") OR "
                 + " (RecnikPodatakaDjora.Dokument = N'" + NazivDokumenta + "') AND(RecnikPodatakaDjora.ID_NaziviNaFormi = 20)  AND(dbo.Prevodi.ID_Jezik = " + Program.ID_Jezik.ToString() + ") "
                 + " ORDER BY RecnikPodatakaDjora.TabelaVView DESC, RecnikPodatakaDjora.TabIndex";

            t = db.ReturnDataTable(query);

            string cAlijasPolja = "";
            string cizborno = "";
            string cAlijasTabele = "";
            string cTabela = "";
            string cSegment = "";
            int cTip = 0;
            string cPolje = "";
            string cTabelaVView = "";
            string cCaption = "";

            string ID = "1";
            string cDokument = "";

            if (t.Rows.Count > 0)
            {
                foreach (DataRow row in t.Rows)
                {
                    var csirina = Convert.ToDouble(row["width"].ToString());
                    if (csirina == 0 || csirina > 9 || row["polje"].ToString() == "RedniBroj") // ne prikazuju se kontrole za polja cija je sirina<9 
                    {
                        cAlijasPolja = row["AlijasPolja"].ToString();
                        cizborno = row["izborno"].ToString();
                        cAlijasTabele = row["AlijasTabele"].ToString();
                        cTabela = row["Tabela"].ToString();
                        cSegment = row["TabelavView"].ToString();
                        cTip = Int32.Parse(row["tip"].ToString());
                        cPolje = row["Polje"].ToString();
                        cTabelaVView = row["TabelaVView"].ToString();
                        cCaption = row["srpski"].ToString();

                        ID = "1";
                        cDokument = NazivDokumenta;

                        //Bez popunjenih vrednosti za parametre: Vrednost i stavke
                        pkontrole.Add(new mKontrola() { IME = cAlijasPolja, cIzborno = cizborno, cAlijasTabele = cAlijasTabele, cTabela = cTabela, cDokument = cDokument, ID = ID, cSegment = cSegment, TipKontrole = cTip, cPolje = cPolje, cTabelaVView = cTabelaVView, cCaption = cCaption });
                        //pkontrole.Add(new mKontrola() { IME = cAlijasPolja, cIzborno = cizborno, cAlijasTabele = cAlijasTabele, cTabela = cTabela, cDokument = cDokument, ID = ID, cSegment = cSegment, TipKontrole = cTip, Vrednost = Vrednost, cPolje = cPolje, cTabelaVView = cTabelaVView, Stavke = Stavke, cCaption = cCaption });

                    }
                }

                pforma.kontrole.AddRange(pkontrole);
            }


            ///////////////////////////////////////////////////


            //foreach (Control control in forma.Controls)
            //{
            //    //if (control.GetType() )
            //    //Console.WriteLine(control.GetType());

            //    if (control.GetType() == typeof(Field))
            //    {

            //        Field kontrola = (Field)forma.Controls[control.Name];

            //        //Console.WriteLine(kontrola.Stavke);
            //        //Console.WriteLine(kontrola.ctekst);

            //        Console.WriteLine(kontrola.IME);
            //        Console.WriteLine(kontrola.ID);
            //        Console.WriteLine("-------------------------");

            //        pkontrole.Add(new mKontrola() { IME = kontrola.IME, cIzborno = kontrola.cIzborno, cAlijasTabele = kontrola.cAlijasTabele, cTabela = kontrola.cTabela, cDokument = kontrola.cDokument, ID = kontrola.ID, cSegment = kontrola.cSegment, TipKontrole = kontrola.TipKontrole, Vrednost = kontrola.Vrednost, cPolje = kontrola.cPolje, cTabelaVView = kontrola.cTabelaVView, Stavke = kontrola.Stavke, cCaption = kontrola.ctekst });

            //        //Console.WriteLine(kontrola.Vrednost);
            //    }
            //    else
            //    {
            //        //Console.WriteLine(control.Text);
            //    }

            //}

            //pforma.kontrole.AddRange(pkontrole);

            //Console.WriteLine(pforma.kontrole.Count);

            ////OBRISATI OVO ISPOD
            //foreach (var ww in pforma.kontrole)
            //{
            //    Console.WriteLine(ww.IME);
            //    Console.WriteLine(ww.TipKontrole);
            //    Console.WriteLine("---------------");

            // }

            return pforma;
        }


        //Djora 30.11.20
        public void SkupiPromene()
        {
            //Daj zadnji uradjeni dokument u testnoj bazi
            int rez = ZadnjiUradjeniDokument();

            DataBaseBroker db = new DataBaseBroker("Data Source=Bankomw; Initial Catalog = BankomVeza; User Id=sa;password=password;");
            DataTable t = new DataTable();

            //Moram prvo da uradim za Kursne liste inace ce da mi puca jer nema kurs za taj dan
            string sql = "SELECT id_dokumenta FROM Dokumenta WHERE id_Dokumenta > " + rez.ToString() + " AND ID_DokumentaStablo=61";

            //Otvori dokumente u produkcionoj bazi i kreni od id-ja veceg od zadnjeg uradjenog i obradjuj jedan po jedan
            t = db.ReturnDataTable(sql);

            if (t.Rows.Count > 0)
            {

                foreach (DataRow row in t.Rows)
                {

                    ObradaJednogDokumenta(row["ID_Dokumenta"].ToString());
                }
            }

        }


        private void ObradaJednogDokumenta(string iddok)
        {
            DataBaseBroker db = new DataBaseBroker("Data Source=Bankomw; Initial Catalog = BankomVeza; User Id=sa;password=password;");
            DataTable t = new DataTable();

            //Ovde je zakucan jedan dokumenta sa ID_Dokumenta=1989315 iz baze BankooNovi koji glumi Produkcionu bazu koja se puni sa starim programom
            string sql = "SELECT dbo.Dokumenta.*, dbo.SifarnikDokumenta.Vrsta, dbo.DokumentaStablo.Naziv "
                       + "FROM dbo.Dokumenta INNER JOIN "
                       + "     dbo.DokumentaStablo ON dbo.Dokumenta.ID_DokumentaStablo = dbo.DokumentaStablo.ID_DokumentaStablo INNER JOIN "
                       + "     dbo.SifarnikDokumenta ON dbo.DokumentaStablo.Naziv = dbo.SifarnikDokumenta.Naziv "
                       + "WHERE dbo.Dokumenta.ID_Dokumenta = " + iddok;  //1498695;

            //string sql = " SELECT ID_DokumentaStavkeView, BrDok, Datum, Opis, Predhodni, LikvidacijaDokumenta, Proknjizeno, MesecPoreza, TTime, NazivOrg, SifRadnika, RB, Godina, Kvartal "
            //          + " FROM DokumentaTotali "
            //          + " WHERE(ID_DokumentaTotali = 1498695)";


            t = db.ReturnDataTable(sql);

            if (t.Rows.Count > 0)
            {
                foreach (DataRow row in t.Rows)
                {
                    clsForma aa = new clsForma();

                    //Console.WriteLine(row["ID_Dokumenta"].ToString());
                    //Console.WriteLine(Int32.Parse(row["ID_DokumentaStablo"].ToString()));
                    //Console.WriteLine(row["Vrsta"].ToString());
                    //Console.WriteLine(row["Naziv"].ToString());

                    // 1498695, 78, "S", "KonacniUlazniRacun"
                    mForma lis = aa.VirtualnaFormaIzBaze(row["Naziv"].ToString(), Int32.Parse(row["ID_Dokumenta"].ToString()), Int32.Parse(row["ID_DokumentaStablo"].ToString()), row["Vrsta"].ToString());

                    //Prolazim kroz sve kontrole i upisujem vrednosti
                    foreach (var ww in lis.kontrole)
                    {

                        VratiVrednostZaPoljeVirtualneForme(row["Naziv"].ToString(), ww, Int32.Parse(row["ID_Dokumenta"].ToString()));

                        //Ova if petlja sluzi da se popune vrednonsti polja virtualne forme
                        //Kod polja "Proknjizeno" i "MesecPoreza" u tabeli dokumenta smo odstupili od seme  
                        //if (ww.cIzborno.Trim() == "" || ww.IME == "Proknjizeno" || ww.IME == "MesecPoreza")
                        //{
                        //    ww.Vrednost = row[ww.cPolje].ToString();
                        //} else
                        //{
                        //    ww.Vrednost = row["ID_" + ww.IME].ToString();
                        //}

                        Console.WriteLine(ww.IME);
                        Console.WriteLine(ww.Vrednost);
                        Console.WriteLine("==================");

                    }
                    //Djora 19.02.21
                    Boolean vrati = new Boolean();
                    CRUD ccrud = new CRUD();
                    //vrati = ccrud.DoIt(forma, Convert.ToString(((Bankom.frmChield)forma).iddokumenta), ((Bankom.frmChield)forma).imedokumenta, lis);
                    //vrati = ccrud.DoIt(forma, row["ID_Dokumenta"].ToString(), row["Naziv"].ToString(), lis);
                    vrati = ccrud.DoIt(forma, row["ID_Dokumenta"].ToString(), "Dokumenta", lis);

                }
            }
        }




        public void VratiVrednostZaPoljeVirtualneForme(string NazivDok, mKontrola polje, int iddok)
        {
            DataBaseBroker db = new DataBaseBroker("Data Source=Bankomw; Initial Catalog = BankomVeza; User Id=sa;password=password;");
            string selu = " SELECT DISTINCT ID_Proknjizeno,ID_MesecPoreza,ID_Predhodni, ID_DokumentaTotali  AS ID_GgRrDokumentaStavkeView,ID_DokumentaStablo,IId,ID_KadrovskaEvidencija, "
                        + " ID_OrganizacionaStrukturaView,ID_LikvidacijaDokumenta,BrDok,Datum,Opis,Predhodni,LikvidacijaDokumenta,Proknjizeno,MesecPoreza,TTime,NazivOrg,SifRadnika,RB,Godina,Kvartal "
                        + " FROM DokumentaTotali WHERE ID_DokumentaTotali=" + iddok.ToString();

            //DataTable tt = db.ParamsQueryDT(selu, NazivDok);
            DataTable tt = db.ParamsQueryDT(selu, "Dokumenta");

            for (int i = 0; i < tt.Rows.Count; i++)
            {
                if ((polje.IME == "Proknjizeno" || polje.IME == "MesecPoreza") && (polje.cPolje != "ID_Dokumenta") && polje.cIzborno.Trim() == "")
                {
                    polje.Vrednost = tt.Rows[i][polje.cPolje].ToString();
                }
                else if (polje.cPolje == "ID_Dokumenta")
                {
                    polje.Vrednost = tt.Rows[i]["ID_GgRrDokumentaStavkeView"].ToString();
                }
                else
                {
                    polje.Vrednost = tt.Rows[i][polje.IME].ToString();
                }
            }
        }

        //Vraca id_dokumenta zadnjeg prenesenog dokumenta u testnu bazu. od tog broja pa navise radim prenos
        public int ZadnjiUradjeniDokument()
        {
            int rez = 0;
            //Link ka testnoj bazi, a NE produkcionoj
            //string connstr = "Data Source=sql2019; Initial Catalog = dbbbTestNew2003Bankom; User Id=sa;password=#Wa61+hK@34; ";
            string connstr = "Data Source=bankomw; Initial Catalog = BankomVezaTest; User Id=sa;password=password; ";
            string ssql = " SELECT TOP 1 id_dokumenta FROM dokumenta ORDER BY id_dokumenta DESC";

            SqlConnection conn = new SqlConnection(connstr);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = ssql;
                Console.WriteLine(ssql);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try
                        {
                            rez = Convert.ToInt32(reader[0]);
                        }
                        catch
                        {
                            rez = 0;
                        }
                    }
                }
            }

            return rez;
        }

    }
}
