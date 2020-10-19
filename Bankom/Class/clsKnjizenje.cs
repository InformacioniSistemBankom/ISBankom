using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Djora 27.09.19 28.11.19
namespace Bankom.Class
{
    public class clsKnjizenje
    {
        private long RedniBroj = 0;
        public string BrojDok;
        private DateTime dtm;
        private int IdStabloZaKnjizenje;
        private long RedniBrZaKnjizenje;
        private string NazivDokumenta;
        private string IdDokZaKnj;
        public string IdTrans;
        public string BrDokZaKnj;
        public int idDokStablo;
        public string NazivKlona = "";
        public string opis = "";
        public int newID = 0;
        DataBaseBroker db = new DataBaseBroker();
        string sql="";
        int ret = 0;
        string Poruka = "";
        public void ObradiNalogAutomatski()
        {
          // UPISIVANJE NALOGA GLAVNE KNJGE U DOKUMENTA  
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;
            BrDokZaKnj = forma.Controls["lBrDok"].Text.ToString();
            idDokStablo = Convert.ToInt32(forma.Controls["lidstablo"].Text);
            
            //ODREDJIVANJE Id DOKUMENTA ZA KNJIZENJE i ID_DokumenStablo
            string IdDokZaKnj = forma.Controls["liddok"].Text.ToString();  /////ef.DajVrednostPropertija(forma, "iddokumenta");

            //long IdStabloZaKnjizenje = Convert.ToInt64(ef.DajVrednostPropertija(activeChild, "IDDokStablo"));

            sql = "select id_dokumentaTotali,dokument,ID_DokumentaStablo,Rb,datum from dokumentaTotali WITH(NOLOCK) where id_dokumentaTotali =" + IdDokZaKnj;
            DataTable t = db.ReturnDataTable(sql);

            IdStabloZaKnjizenje = Convert.ToInt32(t.Rows[0]["ID_DokumentaStablo"]);
            RedniBrZaKnjizenje = Convert.ToInt64(t.Rows[0]["RB"]);

            opis = IdStabloZaKnjizenje.ToString() + "-" + RedniBrZaKnjizenje.ToString();

            IdStabloZaKnjizenje = 29;
            NazivDokumenta = forma.Controls["limedok"].Text;    // t.Rows[0]["Dokument"].ToString();
            //strParams =  "@param1='" +NazivDokumenta  + "`";
            sql = "SELECT UlazniIzlazni as NazivKlona from SifarnikDokumenta where Naziv=@param0";

            t = db.ParamsQueryDT(sql,NazivDokumenta);
            
            NazivKlona = t.Rows[0]["NazivKlona"].ToString();
            dtm = DateTime.Today; // danasnji dan
           
            string  Datum = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost;
            //datum naloga da je datum dokumenta! ISKORISTITI PODATAK Program.datumk
            if (dtm < Convert.ToDateTime("01.03." + dtm.Year.ToString().Substring(2)) & dtm.Year != Convert.ToDateTime(Datum).Year)
            {
                dtm = Convert.ToDateTime(Datum);
            }

            //PROVERA DA LI JE VEC PROKNJIZEN NEKIM DRUGIM NALOGOM
            sql = " select ID_DokumentaView,ID_OpisTransakcijeView,BrojDokumenta "
                       + " from NalogGlavneKnjigeStavke WITH(NOLOCK),Dokumenta WITH(NOLOCK) "
                       + " where ID_Dokumenta= ID_DokumentaView and ID_DokumentZaKnj =" + IdDokZaKnj;
            t = db.ReturnDataTable(sql);
            if (t.Rows.Count > 0)
            {
                MessageBox.Show("Dokument je vec proknjizen nalogom -> " + t.Rows[0]["BrojDokumenta"]);
                return;
            }
            //PROVERA ZA IZVOD DA LI SE SLAZU UNESENI I DNEVNI PROMET
            if (NazivDokumenta == "Izvod")
            {
                sql = "Select Distinct SumaIsplate,DPDuguje,SumaUplate,DPPotrazuje from IzvodTotali WHERE ID_IzvodTotali=@param0";
                t=db.ParamsQueryDT(sql, IdDokZaKnj); 

                if (t.Rows[0]["SumaIsplate"] != t.Rows[0]["DPDuguje"] || t.Rows[0]["SumaUplate"] != t.Rows[0]["DPPotrazuje"]) {
                    MessageBox.Show("Ne slazu se dnevni i uneseni promet Izvod se ne moze knjiziti ISPAVITE!!!! ");
                    return; }
            }
            //PROVERA ZA NALOGZADORADU DA LI SE SLAZU KOLICINA ZA PAKOVANJE SA ZAPAKOVANIM KOLICINAMA
            if (NazivDokumenta == "LotNalogZaDoradu")
            {
                sql = " Select Distinct SumaKolicina as sk,SumaKolicinaSirovine as sks "
                    + " from LotNalogZaPoluProizvodTotali "
                    + " WHERE ID_LotNalogZaPoluProizvodTotali=@param0";

                t = db.ParamsQueryDT(sql,IdDokZaKnj);

                if (t.Rows[0]["sk"] != t.Rows[0]["sks"])
                {
                    MessageBox.Show("Ne slazu se sume kolicina dokument se ne moze knjiziti ISPAVITE!!!! ");
                    return;
                }

            }           

            //DODELJIVANJE Id TRANSAKCIJE(biranje seme knjizenja)
            if (NazivDokumenta == "FinansijskiInterniNalog" || NazivDokumenta == "PDVIzvestajOKnjizenju")
            {
                sql = "select ID_DokumentaView from OpisTransakcije WITH(NOLOCK) where OpisTransakcije not like '%ispravka%' and Tabela = '" + Program.PunoImeDokumenta + "' order by ID_DokumentaView  ";
            }
            else
            {
                if (NazivKlona == "PreknjizavanjeKonta")
                {
                    sql = " select ID_DokumentaView from OpisTransakcije  WITH(NOLOCK) where OpisTransakcije not like '%ispravka%' "
                        + " and Tabela ='" + NazivKlona + "' and opistransakcije like'%" + NazivDokumenta.Substring(1, 3) + "%' order by ID_DokumentaView desc ";
                }
                else
                {
                    sql = "select ID_DokumentaView from OpisTransakcije  WITH(NOLOCK) where OpisTransakcije not like '%ispravka%' and Tabela ='" + NazivDokumenta + "' order by ID_DokumentaView desc ";
                }
            }
            t = db.ReturnDataTable(sql);
            if (t.Rows.Count > 0)
            {
                //ID transakcije
                 IdTrans = t.Rows[0]["ID_DokumentaView"].ToString();
            }
            else
            {
                sql = " select ID_DokumentaVievKlona w from OpisTransakcije  WITH(NOLOCK) where OpisTransakcije not like '%ispravka%' "
                    + " and Tabela =@param0 order by ID_DokumentaView desc ";
                t = db.ParamsQueryDT(sql,NazivKlona);
                if (t.Rows.Count > 0)
                {
                    //ID transakcije
                     IdTrans = t.Rows[0]["ID_DokumentaView"].ToString();
                }
                else
                {
                    MessageBox.Show("Nije definisana transakcija za " + NazivDokumenta);
                    return;
                }
            }

            int IdDokView;
            clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
            IdDokView = os.UpisiDokument(ref BrojDok,opis, 29, Datum.ToString());
            //Totali za Dokumenta sa ID_Dokumenta = idDokView   izvrsava se u  FUNKCIJI UpisiDokument                

            UpisiUNalog(IdDokView);
            UpisiUStavke(IdDokView, IdDokZaKnj);
            ObradiBB(BrDokZaKnj, IdDokView);
            string Poruka = "";                     
            //provera ispravnostinaloga 
            Poruka = DovrsiObradu(IdDokZaKnj);

//Izvrsenje totala za Dokument koji knjizimo 
            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + IdDokZaKnj);
           
            if (Poruka.Trim() != "") /// razmisliti
            {
                MessageBox.Show(Poruka);
                deleteGlavnaKnjiga(IdDokZaKnj);
            }
            else
            {
                MessageBox.Show("Za dokument: " + BrDokZaKnj.Trim() + " Broj naloga je: " + (BrojDok).Trim());
            }
                
        } // kraj obradi nalog automatski
  
        public void ObradiSimulaciju()
        {
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;

            BrDokZaKnj = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrDokZaKnj").Vrednost;
            IdDokZaKnj = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrDokZaKnj").ID;
            int IdDokView = Convert.ToInt32(forma.Controls["liddok"].Text);
            IdTrans = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "OpisTransakcije").ID;                                                                //OpisTransakcije

            ObradiBBS(BrDokZaKnj, IdDokView);

            string Poruka = "";
            int ret = 0;
            //provera ispravnostinaloga 
            Poruka = DovrsiObraduS(IdDokZaKnj);
            //Izvrsenje totala za Dokument koji knjizimo  
            //db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + IdDokZaKnj);

            if (Poruka.Trim() != "")
            {
                MessageBox.Show(Poruka);
                //deleteGlavnaKnjiga(IdDokZaKnj);
            }
        }

        public void  ObradiNalogGlavneKnjige()
        {
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;
            
            BrDokZaKnj = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrDokZaKnj").Vrednost;
            IdDokZaKnj= forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrDokZaKnj").ID;
            int IdDokView = Convert.ToInt32(forma.Controls["liddok"].Text);
            IdTrans = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "OpisTransakcije").ID;                                                                //OpisTransakcije


            ObradiBB(BrDokZaKnj, IdDokView);

            string Poruka = "";
            int ret = 0;
//provera ispravnostinaloga 
            Poruka = DovrsiObradu(IdDokZaKnj);
//Izvrsenje totala za Dokument koji knjizimo  
            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + IdDokZaKnj);

            if (Poruka.Trim() != "") 
            {
                MessageBox.Show(Poruka);
                deleteGlavnaKnjiga(IdDokZaKnj);
            }
        }

        private void ObradiBBS(string BrDokZaKnj, int IdDokView)
        {
            //store procedura brutobilans
            db.ExecuteStoreProcedure("BrutoBilansSimulacija", "DDokument:NalogGlavneKnjigeSimulacija", "BrDok:" + BrDokZaKnj, "IdDokView:" + IdDokView);
        }
        private string DovrsiObraduS(string IdDokZaKnjizenje)
        {
            // provera ispravnosti knjizenja da li se slazu dugovna i potrazna strana
            sql = "select dbo.ProveraNalogaGlavneKnjigeSimulacijaF(@param0) as Provera";
            DataTable dt = db.ParamsQueryDT(sql, IdDokZaKnjizenje);
            string Provera = dt.Rows[0]["Provera"].ToString();

            ///// poruka da su i duguje i potrazuje = 0                 

            if (Provera.Trim() == "")
            {
                //sql = "Update Dokumenta set Proknjizeno='Proknjizen' from Dokumenta where ID_Dokumenta=@param0";
                //ret = db.ParamsInsertScalar(sql, IdDokZaKnjizenje);
            }
            else                                                        // neispravan
               if (Provera.Substring(0, 1) == "1")
            {
                Poruka = Provera.Substring(1);
                //sql = "Update Dokumenta set Proknjizeno='Proknjizen' from Dokumenta where ID_Dokumenta=@param0";
                //ret = db.ParamsInsertScalar(sql, IdDokZaKnjizenje);
            }
            else
            {
                /////kod naloga za knjizenje bilansa uspeha ne slazu se dugovna i potrazna strana za iznos dobiti ili gubitka
                if (NazivDokumenta == "BilansUspeha")
                {
                    sql = "Update Dokumenta set Proknjizeno='Proknjizen' from Dokumenta where ID_Dokumenta=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokZaKnjizenje);
                }
                else
                {
                    Poruka = Provera.Substring(1);
                    //sql = "Update Dokumenta set Proknjizeno='NijeProknjizeno' from Dokumenta where ID_Dokumenta=@param0";
                    //ret = db.ParamsInsertScalar(sql, IdDokZaKnjizenje);
                    return Poruka;
                }
            }
            return "";
        }
        private void ObradiBB(string BrDokZaKnj, int IdDokView) 
        {        
//store procedura brutobilans
            db.ExecuteStoreProcedure("BrutoBilans", "DDokument:NalogGlavneKnjige", "BrDok:" + BrDokZaKnj, "IdDokView:" + IdDokView);
        }        
        private void UpisiUNalog(long IdDok) // samo za automatsko knjizenje
        {
            sql = "Insert into NalogGlavneKnjige(Id_DokumentaView, UUser) values(@param0, @param1)";
            DataTable dt = db.ParamsQueryDT(sql, IdDok.ToString(), Program.idkadar.ToString());

        }

        private void UpisiUStavke(long IdDok, string IdDokZaK) // samo za automatsko knjizenje
        {
            sql = "Insert into NalogGlavneKnjigeStavke(Id_DokumentaView, ID_OpisTransakcijeView, ID_DokumentZaKnj , UUser) "
                    + " values(@param0, @param1, @param2, @param3)";         
            DataTable dt = db.ParamsQueryDT(sql, IdDok.ToString(), IdTrans.ToString(), IdDokZaK, Program.idkadar.ToString());
        }
        private string DovrsiObradu(string IdDokZaKnjizenje)
        {                     
            // provera ispravnosti knjizenja da li se slazu dugovna i potrazna strana
            sql = "select dbo.ProveraNalogaGlavneKnjigeF(@param0) as Provera";
            DataTable dt = db.ParamsQueryDT(sql, IdDokZaKnjizenje);
            string Provera = dt.Rows[0]["Provera"].ToString();

            ///// poruka da su i duguje i potrazuje = 0                 
 
            if (Provera.Trim() == "")
            {
                sql = "Update Dokumenta set Proknjizeno='Proknjizen' from Dokumenta where ID_Dokumenta=@param0";
                ret = db.ParamsInsertScalar(sql, IdDokZaKnjizenje);
            }
            else                                                        // neispravan
               if (Provera.Substring(0, 1) == "1"  )
               {
                   Poruka = Provera.Substring(1);
                   sql = "Update Dokumenta set Proknjizeno='Proknjizen' from Dokumenta where ID_Dokumenta=@param0";
                   ret = db.ParamsInsertScalar(sql, IdDokZaKnjizenje);
               }
               else 
               {
                /////kod naloga za knjizenje bilansa uspeha ne slazu se dugovna i potrazna strana za iznos dobiti ili gubitka
                   if (NazivDokumenta == "BilansUspeha")
                   {
                    sql = "Update Dokumenta set Proknjizeno='Proknjizen' from Dokumenta where ID_Dokumenta=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokZaKnjizenje);
                   }
                   else
                   {
                    Poruka = Provera.Substring(1);
                    sql = "Update Dokumenta set Proknjizeno='NijeProknjizeno' from Dokumenta where ID_Dokumenta=@param0";
                    ret = db.ParamsInsertScalar(sql, IdDokZaKnjizenje);                   
                    return Poruka;
                    }
            }           
             return "";
        }
        private void deleteGlavnaKnjiga(string IdDokZaKnjizenje)
        {
            sql = "Delete from glavnaknjiga  where ID_DokumentaView=@param0";
            ret = db.ParamsInsertScalar(sql, IdDokZaKnjizenje);
            sql = "Delete from nalogglavneknjigestavke where ID_DokumentZaKnj=@param0";
            ret = db.ParamsInsertScalar(sql, IdDokZaKnjizenje);
        }
    }
}
