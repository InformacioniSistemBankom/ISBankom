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
    class clsFormInitialisation
    {
        private Form forma = new Form();
        private string operacija;

        public void ObrisiZaglavljeIStavkePoljaZaUnos()
        {
            DataBaseBroker db = new DataBaseBroker();
            forma = Program.Parent.ActiveMdiChild;
            //string dokje,string imestabla,string ime,string idstablo,string ident)
            string sql = "";
            string dokje = forma.Controls["ldokje"].Text;
            string imestabla = forma.Controls["limestabla"].Text;
            string ident = forma.Controls["liddok"].Text;
            string idstablo = forma.Controls["lidstablo"].Text;
            string imedokumenta = forma.Controls["limedok"].Text;
            string NazivKlona = "";
            dokje = forma.Controls["ldokje"].Text;
            operacija = forma.Controls["OOperacija"].Text;
            if (dokje == "S")
                imedokumenta = ((Bankom.frmChield)forma).imestabla;
            else
                imedokumenta = forma.Controls["limedok"].Text;
            sql = "SELECT UlazniIzlazni as NazivKlona from SifarnikDokumenta where Naziv=@param0";
            DataTable dt = db.ParamsQueryDT(sql, imedokumenta);
            if (dt.Rows.Count != 0) NazivKlona = dt.Rows[0]["NazivKlona"].ToString();

            sql = "SELECT alijaspolja as polje,izborno from RecnikPodataka where  dokument=@param0 and TabIndex>=0 and width>0 and Height > 0 order by tabindex";
            DataTable drp = db.ParamsQueryDT(sql, NazivKlona);

            foreach (DataRow row in drp.Rows)
            {                
                Field pb = (Field)Program.Parent.ActiveMdiChild.Controls[row["polje"].ToString()];
                if (pb != null)
                {
                    //pb.ID = "1";
                    pb.Vrednost = "";
                    pb.cEnDis = "";
                    switch (pb.VrstaKontrole)
                    {
                        case "tekst":
                            pb.textBox.Text = "";
                            pb.textBox.Enabled = true;
                            break;
                        case "datum":
                            pb.dtp.Text = "";
                            //pb.dtp.CustomFormat = " ";
                            pb.dtp.Format = DateTimePickerFormat.Custom;
                            pb.dtp.Enabled = true;
                            //pb.dtp.CustomFormat = "dd.MM.yy";
                            //pb.dtp.Format = DateTimePickerFormat.Custom;
                            break;
                        case "combo":                            
                            if (operacija == "PREGLED")
                            {
                                pb.comboBox.Text = "";
                                pb.ID = "1";
                                pb.cIzborno = row["izborno"].ToString();
                                pb.comboBox.Enabled = true;
                            }
                            
                            break;
                    }
                }
            }
        }
        public void InitValues()
           
        {
            forma = Program.Parent.ActiveMdiChild;
            operacija = forma.Controls["OOperacija"].Text;                        

            foreach (var ctrls in forma.Controls.OfType<Field>())
            {                        
                 

                switch (ctrls.VrstaKontrole)
                {
                    case "tekst":
                        if (ctrls.cTip != 10)
                        {
                            if (ctrls.cTip==3)
                            {
                                ctrls.textBox.Text = "1";

                            }
                            else
                            {
                                ctrls.textBox.Text = "0";
                            }
                            string vred = ctrls.textBox.Text;
                            ctrls.textBox.Text = FormatirajPolje(vred, ctrls.cTip);
                            Console.WriteLine(ctrls.textBox.Text);
                        }
                        else
                        {
                            ctrls.textBox.Text = "";
                            ctrls.Vrednost = ctrls.textBox.Text;
                        }
                        break;

                    case "datum":
                         if (ctrls.cTip == 8)
                            {

                                ctrls.dtp.Text = string.Format("{0:dd.MM.yy}", System.DateTime.Now.ToString());
                                ctrls.dtp.Value = System.DateTime.Now;
                            }
                            else
                            {
                                string mgodina = System.DateTime.Now.ToString().Substring(6, 2);
                                string mdatum = "01.01." + mgodina;
                                ctrls.dtp.Text = string.Format("{0:dd.MM.yy}", mdatum.ToString());
                                ctrls.dtp.Value = Convert.ToDateTime(ctrls.dtp.Text);
                                ctrls.Vrednost = ctrls.dtp.Text;
                            }
                        //}
                        break;
                    case "combo":
                        ctrls.comboBox.Text = "";
                        ctrls.Vrednost = ctrls.comboBox.Text;
                        ctrls.ID = "1";
                        break;
                    case "cek":
                       ctrls.cekboks.Checked = false;
                        break;
                     }
            } // end searching controls     
        }
        public string FormatirajPolje(string polje,int  Tip)
        {
            //polje = polje.Replace(".", "");
            Double mValue = 0;
            if (Tip == 3 || Tip == 4 || Tip == 5 || Tip == 6 || Tip == 7 || Tip == 11 || Tip == 13 || Tip == 17 || Tip == 19 || Tip == 20 || Tip == 21)
            {
                if (polje.Trim() == "")
                    polje = "0";
                mValue = Convert.ToDouble(polje);
                switch (Tip)
                {
                    case 5:
                    case 11:
                        polje = mValue.ToString("N2", CultureInfo.CurrentCulture);
                        break;
                    case 6:
                        polje = mValue.ToString("N3", CultureInfo.CurrentCulture);
                        break;
                    case 7:
                        polje = mValue.ToString("N4", CultureInfo.CurrentCulture);
                        break;
                    case 19:
                        polje = mValue.ToString("N5", CultureInfo.CurrentCulture);
                        break;
                    case 20:
                        polje = mValue.ToString("N7", CultureInfo.CurrentCulture);
                        break;
                    case 21:
                        polje = mValue.ToString("N9", CultureInfo.CurrentCulture);
                        break;
                    default:
                        break;
                }
            }
            //polje = polje.Replace(".", ",");
            return polje;
        }
    }
}
