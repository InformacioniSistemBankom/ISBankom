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


namespace Bankom.Class
{
    class clscontrolsOnForm
    {
        public void addFormControls(Form forma, string dokument, string iddok,string operacija)
        {
            DataBaseBroker db = new DataBaseBroker();
            DataTable tt = new DataTable();
            string sql = "Select Naziv,UlazniIzlazni as NazivDokumenta from SifarnikDokumenta  where Naziv=@param0";
            tt = db.ParamsQueryDT(sql, dokument);
            if (tt.Rows.Count > 0) { dokument = tt.Rows[0]["NazivDokumenta"].ToString(); }
            Form form1 = new Form();
            form1 = forma;
            if (Program.ID_Jezik == 0)
                Program.ID_Jezik = 3;
            //string query = " SELECT RecnikPodatakaDjora.ID_RecnikPodataka AS ID, RecnikPodatakaDjora.levo, RecnikPodatakaDjora.vrh, RecnikPodatakaDjora.width, RecnikPodatakaDjora.height, "
            //           + " RecnikPodatakaDjora.AlijasPolja, RecnikPodatakaDjora.AlijasTabele, dbo.TipoviPodataka.tip, dbo.TipoviPodataka.velicina, dbo.TipoviPodataka.naziv, dbo.TipoviPodataka.DifoltZaIzvestaje, "
            //           + " dbo.TipoviPodataka.Format, dbo.TipoviPodataka.Alajment, RecnikPodatakaDjora.Izborno, RecnikPodatakaDjora.Polje, RecnikPodatakaDjora.PostojiLabela, RecnikPodatakaDjora.ID_NaziviNaFormi, "
            //           + " RecnikPodatakaDjora.TUD, RecnikPodatakaDjora.TabelaVView,RecnikPodatakaDjora.TabIndex, RecnikPodatakaDjora.StornoIUpdate, RecnikPodatakaDjora.Tabela, dbo.Prevodi.Prevod as Srpski, "
            //           + " dbo.TipoviPodataka.CSharp as FormatPolja,RecnikPodatakaDjora.Restrikcije,RecnikPodatakaDjora.JJoinTvV as ImaNaslov,RecnikPodatakaDjora.FormulaForme"
            //           + " FROM dbo.RecnikPodataka AS RecnikPodatakaDjora INNER JOIN "
            //           + " dbo.TipoviPodataka ON RecnikPodatakaDjora.ID_TipoviPodataka = dbo.TipoviPodataka.ID_TipoviPodataka INNER JOIN "
            //           + " dbo.Prevodi ON RecnikPodatakaDjora.ID_NaziviNaFormi = dbo.Prevodi.ID_NaziviNaFormi "
            //           + " WHERE(RecnikPodatakaDjora.Dokument = N'" + dokument + "')AND( RecnikPodatakaDjora.width <> 0  OR  RecnikPodatakaDjora.ID_NaziviNaFormi = 20 "
            //           + " OR RecnikPodatakaDjora.ID_NaziviNaFormi = 25 ) "
            //           + "  AND(dbo.Prevodi.ID_Jezik = "+Program.ID_Jezik.ToString()+") "
            //           + " ORDER BY RecnikPodatakaDjora.TabelaVView DESC, RecnikPodatakaDjora.TabIndex

            //Djora 26.09.20
            //string query = " SELECT RecnikPodatakaDjora.ID_RecnikPodataka AS ID, RecnikPodatakaDjora.levo, RecnikPodatakaDjora.vrh, RecnikPodatakaDjora.width, RecnikPodatakaDjora.height, "
            //       + " RecnikPodatakaDjora.AlijasPolja, RecnikPodatakaDjora.AlijasTabele, dbo.TipoviPodataka.tip, dbo.TipoviPodataka.velicina, dbo.TipoviPodataka.naziv, dbo.TipoviPodataka.DifoltZaIzvestaje, "
            //       + " dbo.TipoviPodataka.Format, dbo.TipoviPodataka.Alajment, RecnikPodatakaDjora.Izborno, RecnikPodatakaDjora.Polje, RecnikPodatakaDjora.PostojiLabela, RecnikPodatakaDjora.ID_NaziviNaFormi, "
            //       + " RecnikPodatakaDjora.TUD, RecnikPodatakaDjora.TabelaVView, RecnikPodatakaDjora.StornoIUpdate, RecnikPodatakaDjora.Tabela, dbo.Prevodi.Prevod as Srpski, "
            //       + " dbo.TipoviPodataka.CSharp as FormatPolja,RecnikPodatakaDjora.Restrikcije,RecnikPodatakaDjora.JJoinTvV as ImaNaslov,RecnikPodatakaDjora.FormulaForme"
            //       + " FROM dbo.RecnikPodataka AS RecnikPodatakaDjora INNER JOIN "
            //       + " dbo.TipoviPodataka ON RecnikPodatakaDjora.ID_TipoviPodataka = dbo.TipoviPodataka.ID_TipoviPodataka INNER JOIN "
            //       + " dbo.Prevodi ON RecnikPodatakaDjora.ID_NaziviNaFormi = dbo.Prevodi.ID_NaziviNaFormi "
            //       + " WHERE(RecnikPodatakaDjora.Dokument = N'" + dokument + "') AND(RecnikPodatakaDjora.TabIndex >= 0) AND(RecnikPodatakaDjora.width <> 0) AND(dbo.Prevodi.ID_Jezik = " + Program.ID_Jezik.ToString() + ") OR "
            //       + " (RecnikPodatakaDjora.Dokument = N'" + dokument + "') AND(RecnikPodatakaDjora.ID_NaziviNaFormi = 20)  AND(dbo.Prevodi.ID_Jezik = " + Program.ID_Jezik.ToString() + ") "
            //       + " ORDER BY RecnikPodatakaDjora.TabelaVView DESC, RecnikPodatakaDjora.TabIndex";

            //Djora 26.09.20
            string query = " SELECT RecnikPodatakaDjora.ID_RecnikPodataka AS ID, RecnikPodatakaDjora.Clevo as levo, RecnikPodatakaDjora.Cvrh as vrh, RecnikPodatakaDjora.Cwidth as width, RecnikPodatakaDjora.height, "
                  + " RecnikPodatakaDjora.AlijasPolja, RecnikPodatakaDjora.AlijasTabele, dbo.TipoviPodataka.tip, dbo.TipoviPodataka.velicina, dbo.TipoviPodataka.naziv, dbo.TipoviPodataka.DifoltZaIzvestaje, "
                  + " dbo.TipoviPodataka.Format, dbo.TipoviPodataka.Alajment, RecnikPodatakaDjora.Izborno, RecnikPodatakaDjora.Polje, RecnikPodatakaDjora.PostojiLabela, RecnikPodatakaDjora.ID_NaziviNaFormi, "
                  + " RecnikPodatakaDjora.TUD, RecnikPodatakaDjora.TabelaVView, RecnikPodatakaDjora.StornoIUpdate, RecnikPodatakaDjora.Tabela, dbo.Prevodi.Prevod as Srpski, "
                  + " dbo.TipoviPodataka.CSharp as FormatPolja,RecnikPodatakaDjora.Restrikcije,RecnikPodatakaDjora.JJoinTvV as ImaNaslov,RecnikPodatakaDjora.FormulaForme"
                  + " FROM dbo.RecnikPodataka AS RecnikPodatakaDjora INNER JOIN "
                  + " dbo.TipoviPodataka ON RecnikPodatakaDjora.ID_TipoviPodataka = dbo.TipoviPodataka.ID_TipoviPodataka INNER JOIN "
                  + " dbo.Prevodi ON RecnikPodatakaDjora.ID_NaziviNaFormi = dbo.Prevodi.ID_NaziviNaFormi "
                  + " WHERE(RecnikPodatakaDjora.Dokument = N'" + dokument + "') AND(RecnikPodatakaDjora.TabIndex >= 0) AND(RecnikPodatakaDjora.width <> 0) AND(dbo.Prevodi.ID_Jezik = " + Program.ID_Jezik.ToString() + ") OR "
                  + " (RecnikPodatakaDjora.Dokument = N'" + dokument + "') AND(RecnikPodatakaDjora.ID_NaziviNaFormi = 20)  AND(dbo.Prevodi.ID_Jezik = " + Program.ID_Jezik.ToString() + ") "
                  + " ORDER BY RecnikPodatakaDjora.TabelaVView DESC, RecnikPodatakaDjora.TabIndex";

            Console.WriteLine(query);
            DataTable t = db.ReturnDataTable(query);
            if (t.Rows.Count > 0)
            {
                foreach (DataRow row in t.Rows)
                {                    
                    var csirina = Convert.ToDouble(row["width"].ToString());
                    if (csirina == 0 || csirina > 9 || row["polje"].ToString()=="RedniBroj") // BORKA ne prikazuju se kontrole za polja cija je sirina<9 primeceno u pdvkalkulacijaulaza ????????
                    {
                            var clevo = Convert.ToDouble(row["levo"].ToString());
                            var cvrh = Convert.ToDouble(row["vrh"].ToString());
                            var cvisina = Convert.ToDouble(row["height"].ToString());
                            var ctekst = row["Srpski"].ToString();
                            var cPostojiLabela = row["PostojiLabela"].ToString();
                            var cTip = Int32.Parse(row["tip"].ToString());
                            var cAlijasPolja = row["AlijasPolja"].ToString();
                            var cizborno = row["izborno"].ToString();
                            var cidNaziviNaFormi = row["ID_NaziviNaFormi"].ToString();
                            var cTUD = row["tud"].ToString();
                            var cPolje = row["polje"].ToString();
                            string cEnDis = row["StornoIUpdate"].ToString();
                            string cFormat = row["Format"].ToString();
                            var cAlijasTabele = row["AlijasTabele"].ToString();
                            string cTabelaVView = row["TabelaVView"].ToString();
                            string cFormatPolja = row["FormatPolja"].ToString();
                            var cTabela = row["Tabela"].ToString();
                            var cSegment = row["TabelavView"].ToString();
                            //var cTabIndex = Convert.ToInt32(row["TabIndex"].ToString());
                            var cRestrikcije = row["Restrikcije"].ToString();
                            var cImaNaslov = 0;
                            //Console.WriteLine(row["ImaNaslov"].ToString());
                            if (row["ImaNaslov"].ToString() == "False")
                                cImaNaslov = 0;//row["ImaNaslov"].ToString();
                            else
                                cImaNaslov = 1;
                            var cFormulaForme = row["FormulaForme"].ToString();
                            var mfield = new Field(form1, iddok, dokument, ctekst, cPolje, cAlijasPolja, Color.Lavender, clevo, cvrh, cvisina, csirina, cPostojiLabela, cTip, cizborno, cidNaziviNaFormi, cTUD, cEnDis, cFormat, cTabela, cAlijasTabele, cTabelaVView,  cFormatPolja, cSegment, cRestrikcije, cImaNaslov, cFormulaForme);

                            form1.Controls.Add(mfield);                    
                    }    
                }

                foreach (var ctrls in forma.Controls.OfType<Field>())
                {
                    Console.WriteLine(ctrls.IME);
                }
            }
        }
    }
}
