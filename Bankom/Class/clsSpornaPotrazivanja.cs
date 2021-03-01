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
    class clsSpornaPotrazivanja
    {
        string sql = "";
        int ret = 0;
        DataBaseBroker db = new DataBaseBroker();

        public bool Obradi()
        {
            bool Obradi = false;
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;

            if (MessageBox.Show("Da li zelite ponistiti postojece stavke?", "Sporna potrazivanja", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "delete from SaldaPrometaStavke where id_Dokumentaview= @param0";
                ret = db.ParamsInsertScalar(sql, ((Bankom.frmChield)forma).iddokumenta);
                sql = "delete from SaldaPrometa where id_Dokumentaview= @param0";
                ret = db.ParamsInsertScalar(sql, ((Bankom.frmChield)forma).iddokumenta);


                string konto = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Konto").Vrednost;
                string DatumZaObracun = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "DatumZaObracun").Vrednost;
                Int32 StarostSalda = Convert.ToInt32(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "StarostSalda").Vrednost);

                db.ExecuteStoreProcedure("StarostSalda", "ZaFirmu:" + Program.imeFirme, "ZaKonto:" + konto.Trim(), "ZakljucnoSaDatumom:" + DatumZaObracun, "KojaStarost:" +  StarostSalda);
                sql = "insert into SaldaPrometaStavke(ID_DokumentaView,ID_KomitentiView,.ID_SifrarnikValuta,SaldoZaPrenos ) "
                    + " select distinct @param0, IdKom as ID_KomitentiView,IDval,Saldo "
                    + " from SaldaPoStarosti ";

                ret = db.ParamsInsertScalar(sql, ((Bankom.frmChield)forma).iddokumenta);

                db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + ((Bankom.frmChield)forma).imedokumenta, "IdDokument:" + ((Bankom.frmChield)forma).iddokumenta);
                Obradi = true;

            }
            return Obradi;
        }

    
//        Public Function PopuniStavkeZaSpornaPotrazivanja()
//Dim ssel As String
//ssel = "StarostSalda '" + Firma + "','" + fform("ctKonto").Vrednost + "','" + fform("ctDatumZaObracun").Vrednost + "'," + Str(fform("ctStarostSalda").Vrednost)
//cnn1.Execute ssel
//ssel = "Insert Into SaldaPrometaStavke (ID_DokumentaView,ID_KomitentiView,.ID_SifrarnikValuta,SaldoZaPrenos) " _
//     & "Select Distinct " + Str(IdDokView) + " as ID_DokumentaView ,IdKom as ID_KomitentiView,IDval,Saldo" _
//     & " From SaldaPoStarosti"
//      cnn1.Execute ssel

        //cnn1.Execute "TotaliZaDokument '" + Trim(Dokument) + "', " + Str(IdDokView), adCmdStoredProc
        //End Function


    }
}
