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
    class clsZatvaranjeIOtvaranjeBilansa
    {
        private string iddokument;
        private string IznosZaOtpis;
        private string KontoPrihoda;
        private string KontoTroskova;
        string sql = "";
        DataBaseBroker db = new DataBaseBroker();
        int ret = 0;
        public bool ObradiZahtev()
        {
            bool ObradiZahtev = true;
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;
            iddokument = Convert.ToString(((Bankom.frmChield)forma).iddokumenta);


            if (ProveraIspravnosti(forma) == false)
                goto PrinudniIzlaz;



            if (forma.Controls["OOperacija"].Text == "UNOS" || forma.Controls["OOperacija"].Text == "IZMENA")
            {
                // Zatvaranje i otvaranje bilansa
                db.ExecuteStoreProcedure("ZatvaranjeIOtvaranjeBilansa", "Firma:" + Program.idFirme, "Status:" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Proknjizeno").Vrednost,
                                         "BrNal:" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrDok").Vrednost,
                                         "DoDatuma:" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Datum").Vrednost,
                                         "DomacaValuta:" + Program.ID_DomacaValuta, "IdDokView:" + iddokument,
                                         "IdPrethodni:" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Predhodni").Vrednost, "UUser:" + Program.idkadar,
                                         "IznosZaOtpis:"+ IznosZaOtpis, "KontoPrihoda:"+ KontoPrihoda, "KontoTroskova:" + KontoTroskova);
            }


            return ObradiZahtev;
            PrinudniIzlaz:
            ObradiZahtev = false;
            return ObradiZahtev;

        }

        public bool ProveraIspravnosti(Form forma)
        {
            bool ProveraIspravnosti = false;
            //forma = Program.Parent.ActiveMdiChild;

            sql = "Select Id_DokumentaView from GlavnaKnjiga where ID_DokumentaView =@param0";
            DataTable t = db.ParamsQueryDT(sql, iddokument);
            if (t.Rows.Count > 0)
            {
                if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Proknjizeno").Vrednost.Contains("Zatvaranje") == true)
                {
                    if (MessageBox.Show("Vec postoji glana knjiga zelite li je brisati ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        sql = "Delete from GlavnaKnjiga where ID_DokumentaView =@param0";
                        ret = db.ParamsInsertScalar(sql, iddokument);
                    }
                }
            }

            if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Proknjizeno").Vrednost.Contains("MalaSalda") == true)
            {
                IznosZaOtpis = Prompt.ShowDialog("", "Unesite iznos za otpis ", " Otpis malih salda ");
                clsOperacije co = new clsOperacije();
                bool r = co.IsNumeric(IznosZaOtpis);
                if (r == false) { MessageBox.Show("Pogresno unesen iznos ponovite !!!"); return ProveraIspravnosti; }

                KontoPrihoda = Prompt.ShowDialog("", "Unesite konto za prihod ", " Otpis malih salda ").Trim();
                if (KontoPrihoda.Substring(1, 1) != "6") { MessageBox.Show("Pogresno unesen konto prihoda !!!"); return ProveraIspravnosti; }

                KontoTroskova = Prompt.ShowDialog("", "Unesite konto za troskove ", " Otpis malih salda ").Trim();
                if (KontoPrihoda.Substring(1, 1) != "5") { MessageBox.Show("Pogresno unesen konto troskova !!!"); return ProveraIspravnosti; }

                sql = "Delete from GlavnaKnjiga where ID_DokumentaView =@param0";
                ret = db.ParamsInsertScalar(sql, iddokument);
            }


            if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Proknjizeno").Vrednost.Contains("Otvaranje") == true)
            {
                if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Predhodni").Vrednost == "1")
                {
                    MessageBox.Show("Niste odabrali  predhodnika !!!");
                    return ProveraIspravnosti;
                }

                sql = "Select Proknjizeno from Dokumenta where ID_Dokumenta =@param0";
                t = db.ParamsQueryDT(sql, forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Predhodni").ID);
                if (t.Rows.Count > 0)
                {
                    if (t.Rows[0]["Proknjizeno"].ToString().Contains("Zatvaranje") == false)
                    {
                        MessageBox.Show("Niste odabrali zatvaranje kao predhodnik otvaranju!!!");
                        return ProveraIspravnosti;
                    }
                }
             }

            ProveraIspravnosti = true;
            return ProveraIspravnosti;
        }
    }
}