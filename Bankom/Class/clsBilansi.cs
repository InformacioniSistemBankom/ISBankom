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
    class clsBilansi
    {
        Form forma = new Form();
        DataBaseBroker db = new DataBaseBroker();
        public void ObradiBilans()
        {
            string iddokument = "";
            Boolean vrati = true;
            forma = Program.Parent.ActiveMdiChild;
            int idke = Program.idkadar;
            int NadjiPredh = 1;
            iddokument = Convert.ToString(((Bankom.frmChield)forma).iddokumenta);
            string imedokumenta = ((Bankom.frmChield)forma).imedokumenta.Trim();
            string DatumOd = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "DatumOd").Vrednost;
            string DatumDo = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "DatumDo").Vrednost;
            string Dokument = forma.Controls["limedok"].Text;
            long IdentSeme = Convert.ToInt32(forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "KontoAopBilansView").ID);

            db.ExecuteStoreProcedure("repDovrsiObradu", "Dokument:" + imedokumenta, "NazivKlona:" + imedokumenta, "Firma:" + Program.imeFirme, "IdentSeme:" + IdentSeme.ToString(),
                    "IdDokView:" + iddokument, "DatumOd:'" + DatumOd, "DatumDo:" + DatumDo);
        }
    }
}


