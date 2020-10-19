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
    class clsObradaKursnihRazlika
    {
        private string iddokument;
        string sql = "";
        DataBaseBroker db = new DataBaseBroker();
        int ret = 0;
        public bool ObradiRazlike()
        {
            bool ObradiRazlike = true;
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;
            iddokument = Convert.ToString(((Bankom.frmChield)forma).iddokumenta);
            

            if (ProveraIspravnosti(forma) == false)
                goto PrinudniIzlaz;

            //CRUD ccrud = new CRUD();
            //ccrud.DoIt(forma, iddokument, ((Bankom.frmChield)forma).imedokumenta);

            if (forma.Controls["OOperacija"].Text == "UNOS")
            {
                // obrada kursnih razlika 
                db.ExecuteStoreProcedure("ObradaKursnihRazlika","Firma:"+ Program.idFirme, "Analitika:" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Analitika").Vrednost, 
                                         "DoDatuma:" + forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "DatumZaObracun").Vrednost,
                                         "DomacaValuta:"+Program.ID_DomacaValuta ,"IdDokView:" + iddokument,"UUser:"+ Program.idkadar);
            }
        // izvodjenje totala
        db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:" + ((Bankom.frmChield)forma).imedokumenta, "IdDokument:" + iddokument);
          

            return ObradiRazlike;
      PrinudniIzlaz:
            ObradiRazlike = false;
            return ObradiRazlike;

        }

        public bool ProveraIspravnosti(Form forma)
        {
            bool ProveraIspravnosti = true;
            //forma = Program.Parent.ActiveMdiChild;
            Console.WriteLine(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Analitika").Vrednost);
            if (forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Analitika").Vrednost.Contains("reval") == false &&
                forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Analitika").Vrednost.Contains("ran") == false &&
                forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Analitika").Vrednost.Contains("vanbil") == false &&
                forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Analitika").Vrednost.Contains("88") == false &&
                forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Analitika").Vrednost.Contains("tek") == false)
            {
                string Poruka = "Pogresan izbor vrste obracuna";
                MessageBox.Show(Poruka);
                ProveraIspravnosti = false;
            }
            Console.WriteLine(forma.Controls["OOperacija"].Text);
            if (forma.Controls["OOperacija"].Text == "UNOS")
            {
                sql = "Select Id_DokumentaView from KursnerazlikeStavke where ID_DokumentaView =@param0"  ;
                DataTable t = db.ParamsQueryDT(sql, iddokument);
                if (t.Rows.Count > 0)
                {
                    if (MessageBox.Show("Da li brisete postojece promene?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        sql = "Delete from KursnerazlikeStavke where ID_DokumentaView =@param0";
                        ret = db.ParamsInsertScalar(sql, iddokument);
                    }
                }
            }
            return ProveraIspravnosti;
        }

       

    }
}