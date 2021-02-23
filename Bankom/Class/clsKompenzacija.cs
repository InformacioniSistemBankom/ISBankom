using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//Djora 30.11.20
using Bankom.Model;

//Djora 14.05.20
namespace Bankom.Class
{
    public class clsKompenzacija
    {
        public void ObradiKomenzaciju()
        {
            //Djora 30.11.20 Poziv 
            clsForma aa = new clsForma();
            mForma lis = aa.VirtualnaForma();

            DataBaseBroker db = new DataBaseBroker();

            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;

            int IdDokView = Convert.ToInt32(forma.Controls["liddok"].Text);

            // if (forma.Controls["OOperacija"].Text == "UNOS")
            // {
            //     if (MessageBox.Show("Da li brisete predhodne stavke ? ", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //     {

            string Datum = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "DatumDo").Vrednost;
            string KontoDug = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "KontoDug").Vrednost;
            string KontoPot = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "KontoPot").Vrednost;
            int ID_Komitent = Convert.ToInt32(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivKom").ID);

            Boolean vrati = new Boolean();
            CRUD ccrud = new CRUD();
            
            //Djora 30.11.20
            //vrati = ccrud.DoIt(forma, Convert.ToString(((Bankom.frmChield)forma).iddokumenta), ((Bankom.frmChield)forma).imedokumenta);
            vrati = ccrud.DoIt(forma, Convert.ToString(((Bankom.frmChield)forma).iddokumenta), ((Bankom.frmChield)forma).imedokumenta, lis);

            //EXEC ObradiKompenzaciju 1422996, '01.01.2020', '21.04.2020', 204, 435, 3281, 'Bankom', 6
            db.ExecuteStoreProcedure("ObradiKompenzaciju", "FidDok:" + IdDokView.ToString(), "DoDatuma:" + Datum, "KontoDug:" + KontoDug, "KontoPot:" + KontoPot, "ID_Komitent:" + ID_Komitent.ToString(), "Firma:Bankom", "idFirme:" + Program.idFirme.ToString());
            //    } else {
            //        string sql = " Update kompdug set duguje = saldo where id_DokumentaView = @param1 ";
            //        int newID = db.ParamsInsertScalar(sql, @IdDokView.ToString());
            //     }
            // } else {
            //     string sql = " Update kompdug set duguje=saldo where id_Kompdug = @param1 ";
            //     //int newID = db.ParamsInsertScalar(sql, fform.GridK(1).IdUpdateReda.ToString());
            // }

            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Kompenzacija", "IdDokument:" + IdDokView.ToString());
        }
    }
}

