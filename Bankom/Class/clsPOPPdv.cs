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
    class clsPOPPdv
    {
        private string Godina;
        private string Mesec;
        private string Firma;
        private string imedokumenta;
        private long IdentSeme;
        string sql = "";
        DataBaseBroker db = new DataBaseBroker();
        int ret = 0;
        public bool ObradiPOPPdv()
        {
            bool ObradiPOPPdv = false;
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;

            imedokumenta = ((Bankom.frmChield)forma).imedokumenta.Trim();
            Godina = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Godina").Vrednost;
            Mesec = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Mesec").Vrednost;
            IdentSeme = Convert.ToInt32(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "BrDokKorisnik").ID);

            Obrada();

            //clsdokumentRefresh docref1 = new clsdokumentRefresh();
            //string str = "SELECT ID_POPPdvStavke1View,PoBroj, Opis,Naknada FROM POPPdvStavke1View WHERE AOP>=2000 and AOP <=2015 AND godina="
            //            + Godina + " and mesec=" + Mesec +" and Firma="+ Firma +" ORDER BY AOP ";
            //docref1.refreshDokumentGrid(forma, imedokumenta, "1", str, "S");
            ObradiPOPPdv = true;
            return ObradiPOPPdv;
        }
        public bool Obrada()
        {
            bool Obrada = false;

            if (Program.imeFirme == "Hotel Nevski" || Program.imeFirme == "Bankom")
                Firma = "Bankom";
            else
                Firma = Program.imeFirme;

            if (MessageBox.Show("Da li zelite ponistiti postojece stavke?", "POPPdv", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "Delete from Poppdv WHERE firma=@param0 and mesec=@param1 and godina=@param2";
                ret = db.ParamsInsertScalar(sql, Firma,Mesec,Godina);

                sql = "Insert into POPPdv ( id_SemaBilansa,firma,godina,mesec,PoBroj,AOP,FORMULA,Opis,zavisnost)"
                    + " select id_DokumentaView,@param0,@param1,@param2,Konto,AOP,Formula,Opis,Zavisnost "
                    + " from kontoaopstavke where id_dokumentaView=@param3";
                DataTable dtr = db.ParamsQueryDT(sql, Firma, Godina,Mesec, IdentSeme);

                db.ExecuteStoreProcedure("PopuniRedoveSemePOPPdv", "Dokument:" + imedokumenta, "Firma:" + Firma, "Godina:"+Godina,"Mesec:"+Mesec);

            }

            return Obrada;
        }
    }
}
