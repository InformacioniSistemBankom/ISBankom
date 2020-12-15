﻿using System;
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

        public void InitValues()
        {
            forma = Program.Parent.ActiveMdiChild;
            operacija = forma.Controls["OOperacija"].Text;                        

            foreach (var ctrls in forma.Controls.OfType<Field>())
            {                        
                 ctrls.ID = "1";

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
                            ctrls.dtp.Value =  System.DateTime.Now;
                        }
                        else
                        {
                            string mgodina = System.DateTime.Now.ToString().Substring(6, 2);
                            string mdatum = "01.01."+mgodina;
                            ctrls.dtp.Text = string.Format("{0:dd.MM.yy}", mdatum.ToString());
                            ctrls.dtp.Value = Convert.ToDateTime(ctrls.dtp.Text);
                        }
                        ctrls.Vrednost = ctrls.dtp.Text;
                        break;
                    case "combo":
                        ctrls.comboBox.Text = "";
                        ctrls.Vrednost = ctrls.comboBox.Text;
                        break;
                    case "cek":

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
