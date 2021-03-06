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
    class CcalculatedVal
    {
        DataBaseBroker db = new DataBaseBroker();
        clsOperacije co = new clsOperacije();
        
        public string CalculateValue(Form form1, string formula)
        {
            
            string fformula = "";
            fformula = formula;  
            //string[] vrednosti = new string[100];
            //char[] delimiterChars = { '-', '+' ,'/','*'};
            char[] delimit = { ',' };
            //string[] delovi = formula.Split(delimiterChars);
            fformula = fformula.Replace("-", ",-,");
            fformula = fformula.Replace("+", ",+,");
            fformula = fformula.Replace("/", ",/,");
            fformula = fformula.Replace("*", ",*,");
            fformula = fformula.Replace("(", ",(,");
            fformula = fformula.Replace(")", ",),");
            Console.WriteLine(fformula);
            string[] op = fformula.Split(delimit, StringSplitOptions.RemoveEmptyEntries);

            string gotov = "";
            string valstr = "";           

            for (int k = 0; k < op.Length; k++)
            {
                if (op[k].Trim() != "")
                {
                    Console.WriteLine(op[k]);
                    if (op[k].Contains("+") == true || op[k].Contains("-") == true || op[k].Contains("*") == true || op[k].Contains("/") == true || op[k].Contains("(") == true || op[k].Contains(")") == true)
                    {
                        gotov = gotov + op[k];
                    }
                    else
                    {
                        if (co.IsNumeric(op[k]) == true)
                        {
                            gotov = gotov + op[k];
                            //break;
                        }
                        else
                        {
                            Field kontrola = (Field)form1.Controls[op[k].Trim()]; //  uzimamo kontrolu na formi                                  
                            if (kontrola == null)
                            {
                                valstr = "0.00";
                                //break;
                            }
                            else
                            {
                                valstr = Convert.ToString(kontrola.Vrednost);
                                if (valstr.Trim() == "")
                                {
                                    valstr = "0.00";
                                }
                                else
                                {
                                    valstr = valstr.Replace(".", "");
                                    valstr = valstr.Replace(",", ".");
                                }
                                gotov = gotov + valstr;

                                ////break;                                
                            }
                        }
                    }
                }
            }
            Console.WriteLine(gotov);
            return (gotov);
        }    

        public string Slovima(double KojiBroj, string KojaValuta)
        {
            //Jovana 05.01.21
            double U = Math.Abs(KojiBroj);
           // double U =  KojiBroj;
            double Broj;
            double Ost;
            double CeoDeo;
            double Ceo;
            double Ostatak;
            int k;
            double OstOst;
            double CeoOst;
            double pom;
            string sSlovima = "";

            CeoDeo = (U - (Mod2(U, 100))) / 100;
            if (CeoDeo < 0)
            {
                Broj = -1 * CeoDeo;
            }
            else
            {
                Broj = CeoDeo;
            }
            Ost = Mod2(U, 100);
            Ceo = (Broj - (Mod2(Broj, 1000))) / 1000;       
            k = 1;
            Ostatak = Mod2(Broj, 1000);

            do
            {
                OstOst = Mod2(Ostatak, 100);
                CeoOst = (Ostatak - (Mod2(Ostatak, 100))) / 100;
                if (OstOst < 10)
                {
                    DataTable Pretraga = db.ReturnDataTable("Select * from Brojevi Where BR=" + OstOst.ToString());
                    if (k == 1)
                    {
                        if (OstOst == 1)
                        {
                            sSlovima = Pretraga.Rows[0][k].ToString();
                        }
                        else
                        {
                            if (OstOst != 0)
                            {
                                sSlovima = Pretraga.Rows[0][k].ToString();
                            }
                        }
                    }
                    else
                    {
                        //Jovana 05.01.21
                        if (sSlovima.IndexOf("hiljada") == 0)
                            sSlovima = sSlovima.Substring(7, sSlovima.Length - 7);
                        sSlovima = string.Concat(Pretraga.Rows[0][k].ToString(), sSlovima);
                    }
                }
                else
                {
                    if (OstOst < 20 && OstOst > 10)
                    {
                        DataTable Pretraga = db.ReturnDataTable("Select * from Brojevi Where BR=" + OstOst.ToString());
                        if (k == 1)
                        {
                            sSlovima = Pretraga.Rows[0]["JEDINICA"].ToString();
                        }
                        else
                        {
                            sSlovima = string.Concat(Pretraga.Rows[0][k].ToString(), sSlovima);
                        }
                    }
                    else
                    {
                        if (k == 1)
                        {
                            pom = Mod2(OstOst, 10);
                            if (pom != 0)
                            {
                                DataTable Pretraga1 = db.ReturnDataTable("Select * from Brojevi Where BR=" + pom.ToString());
                                //if (pom == 1)
                                //{
                                //    sSlovima = Pretraga1.Rows[0][k].ToString();
                                //}
                                //else
                                //{
                                    sSlovima = (Pretraga1.Rows[0][k].ToString());
                                //}
                            }
                            pom = (OstOst - (Mod2(OstOst, 10))) / 10;
                            DataTable Pretraga2 = db.ReturnDataTable("Select * from Brojevi Where BR=" + pom.ToString());
                            sSlovima = string.Concat(Pretraga2.Rows[0]["DESETICA"].ToString(), sSlovima);/// Pretraga!desetica + sSlovima
                        }
                        else
                        {
                            pom = Mod2(OstOst, 10);
                            if (pom != 0)
                            {
                                DataTable Pretraga3 = db.ReturnDataTable("Select * from Brojevi Where BR=" + pom.ToString());
                                sSlovima = string.Concat(Pretraga3.Rows[0][k].ToString(), sSlovima);
                            }
                            pom = ((OstOst - (Mod2(OstOst, 10))) / 10);
                            //DataTable Pretraga5 = db.ReturnDataTable("Select * from Brojevi Where BR=" + pom.ToString());
                            if (k != 1 && (Mod2(OstOst, 10)) == 0)
                            {
                                DataTable Pretraga6 = db.ReturnDataTable("Select * from Brojevi Where BR=0");
                                sSlovima = string.Concat(Pretraga6.Rows[0][k].ToString(), sSlovima);
                            }
                            DataTable Pretraga = db.ReturnDataTable("Select * from Brojevi Where BR=" + pom.ToString());
                            sSlovima = string.Concat(Pretraga.Rows[0]["DESETICA"].ToString(), sSlovima);
                        }
                    }
                }
                if (CeoOst == -1)
                {
                    CeoOst = 0;
                }
                if (CeoOst != 0)
                {
                    DataTable Pretraga = db.ReturnDataTable("Select * from Brojevi Where BR=" + CeoOst.ToString());
                    sSlovima = string.Concat(Pretraga.Rows[0]["STOTINA"].ToString(), sSlovima);
                }
                if (k == 1)
                {
                    k = k + 3;
                }
                else
                {
                    if (k == 4)
                    {
                        k = k + 1;
                    }
                    else
                    {
                        k = 1;
                    }
                }
                Broj = Ceo;
                Ostatak = Mod2(Ceo, 1000);
                Ceo = (Ceo - (Mod2(Ceo, 1000))) / 1000;
            } while (Broj != 0);  // kraj do while

            double aaaa = Mod2(U, 100);
            //if (Mod2(U, 100) != 0)
            //{                
            sSlovima = string.Concat(sSlovima, " ", KojaValuta, " ", Convert.ToString(aaaa), "/100 ");
            //Jovana 05.01.21
            //
            if (KojiBroj < 0)
                sSlovima = "minus " + sSlovima;

            //}
            //else
            //{
            //sSlovima = string.Concat(sSlovima, " ", "RSD", " 0/100");
            //}

            return sSlovima;
        } 
        private double Mod2(double aa, double bb)
        {
            double mMod;
            if ((Math.Round(aa, 0) - bb  *(int)((double)( (Math.Round(aa, 0) / bb)))) < 0)   //(int)((double)(myObj.Value))
            {
                mMod = 0;
            }
            else
            {
                //mMod = Math.Round(aa, 0);
                mMod = (Math.Round(aa, 0) - bb * (int)((double)((Math.Round(aa, 0) / bb))));
                //mMod = bb * Convert.ToInt32((Math.Round(aa, 0) / bb));
            }
            return mMod;
            ///(Round(aa, 0) - bb * (Val(Round(aa, 0) / bb)) < 0, 0, Round(aa, 0) - bb * (Val(Round(aa, 0) / bb)))
        }       
    }
}





