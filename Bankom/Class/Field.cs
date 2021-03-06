﻿using System;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Bankom.Class;
using System.Collections.Generic;
using System.IO;
//NOVI PROJEKAT

namespace Bankom.Class
{

    public struct MyStruct
    {
        public string query;
        public string ValueMember;
        public string DisplayMember;

    }
    // 23.10.2020. promena pozicije kontrola na formi se radi tako sto se selektuje kontrola na formi kada je program pokrenut 
    //i onda pokretima strelica (gore - dole) namestamo poziciju i cuvamo je klikom na F10. Za sirinu se koriste Ctrl +2 i Ctrl+3.
    class Field : FlowLayoutPanel
    {
        public Label label;
        public TextBox textBox;
        public ComboBox comboBox;
        public DateTimePicker dtp;
        public CheckBox cekboks;
        public DataBaseBroker db = new DataBaseBroker();
        public DataGridView dv = new DataGridView();
        public string ID { get; set; }

        public string TabelaVView { get; set; }

        public string IME { get; set; }
        public int TipKontrole { get; set; }
        public string VrstaKontrole { get; set; }
        public string AlijasTabele { get; set; }

        public string Vrednost { get; set; }
        public string FormatVrednosti { get; set; }
        //public double ofsetx = 2.2;

        //Djora 07.07.20
        //public double ofset = 3.2;
        //public double ofsety = 1.8;

        //Djora 26.09.20
        //public double ofset = Program.RacioWith * 1.3333333333333333;
        //public double ofsety = Program.RacioHeight * 1.3333333333333333;
        public double ofset = Program.RacioWith;
        public double ofsety = Program.RacioHeight;
        
        //Djora 30.11.20
        public string Stavke { get; set; }

        public string cPolje;
        public string cIzborno;
        public string cDokument;
        public string cAlijasTabele;
        public string cTabelaVView;
        public string cTabela;
        public string cSegment;
        public string cRestrikcije;
        public string cFormulaForme;
        public string cEnDis;
        public int cTip;
        public int cTabIndex;
        public int cImaNaslov;
        public string ctekst;
        //Djora 26.09.20
        public string cIdNaziviNaFormi;
        //Ivana 11.12.2020.
        public string cZavisiOd;
        Form forma = new Form();
        //private int izmena;
        private string aaa = "";
        private string sadrzaj = "";
        //public event EventHandler ValueChanged;
        //public Field(Form form1, string iddok, string dokument, string label_text, string polje, string Ime, Color boja, double levo, double vrh, double visina, double sirina,
        //string PozicijaLabele, int Tip, string izborno, string idNaziviNaFormi, string tud, string EnDis, string FormatStringa, string Tabela, string AlijasTabele, string TabelaVView, int TabIndex, string FormatPolja, string Segment, string Restrikcije, int ImaNaslov, string FormulaForme) : base()
        public Field(Form form1, string iddok, string dokument, string label_text, string polje, string Ime, Color boja, double levo, double vrh, double visina, double sirina,
                     string PozicijaLabele, int Tip, string izborno, string idNaziviNaFormi, string zavisiOd, string tud, string EnDis, string FormatStringa, string Tabela, string AlijasTabele, string TabelaVView, string FormatPolja, string Segment, string Restrikcije, int ImaNaslov, string FormulaForme) : base()
        {
            boja = Color.AliceBlue;
            forma = form1;
            FormatVrednosti = FormatStringa;
            IME = Ime;
            TipKontrole = Tip;
            cPolje = polje;
            cIzborno = izborno;
            cDokument = dokument;
            cTabela = Tabela;
            cAlijasTabele = AlijasTabele;
            cTabelaVView = TabelaVView;
            cSegment = Segment;
            cRestrikcije = Restrikcije;
            cImaNaslov = ImaNaslov;
            ctekst = label_text;
            cFormulaForme = FormulaForme;
            cEnDis = EnDis; //jovana
            cTip = Tip;
            //Djora 26.09.20
            cIdNaziviNaFormi = idNaziviNaFormi;
            //Ivana 11.12.2020.
            cZavisiOd = zavisiOd;
            //Djora 26.09.20
            //this.BackColor = Color.Red;
            this.BorderStyle = BorderStyle.None;
            //this.Margin= new Padding(0, 0, 0, 0);

            //Djora 30.11.20
            Stavke = "0";

            if (Ime == "Ugovor")
            {
                Console.WriteLine("ssss");
            }

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            Left = (int)(levo * ofset);
            Top = (int)Convert.ToDouble(vrh * ofsety);
            //
            //Djora 09.07.20
            //Height = (int)Convert.ToDouble(visina * ofset);
            Height = (int)Convert.ToDouble(visina * ofsety);
            //Djora 06.07.20
            ////if (idNaziviNaFormi != "20")
            ////{
            Console.WriteLine(IME);

            if (Tip != 24) //Djora 30.12.20
            {
                if (PozicijaLabele != "2")        //   2- labela ne postoji ; 1 = ispred, 0= iznad
                {                                // label postoji
                    label = new Label();
                    label.Text = ctekst;//label_text;
                    label.Anchor = AnchorStyles.Left;
                    label.TextAlign = ContentAlignment.MiddleCenter;    //MiddleLeft;
                    label.ForeColor = Color.Black;                                                   //Djora 26.09.20
                                                                                                     //label.Height = (int)(visina * 1.2);
                    label.Font = new Font("TimesRoman", 13, FontStyle.Regular);
                    //label.Font = new Font("TimesRoman", 10.8F, FontStyle.Bold);

                    //Djora 26.09.20
                    PromenaFonta(label);
                    //Djora 26.09.20
                    label.Margin = new Padding(0, 0, 0, 0);

                    Controls.Add(label);
                }
                if (PozicijaLabele == "0")// labela je iznad
                {
                    label.Width = (int)Convert.ToDouble(sirina * ofset);
                    FlowDirection = FlowDirection.TopDown;
                }

                if (PozicijaLabele == "1") // labela je ispred
                {
                    //Djora 09.07.20   
                    //label.Width = (int)sirina;
                    label.Width = (int)Convert.ToDouble((sirina * ofset) / 2);
                    label.TextAlign = ContentAlignment.MiddleLeft;
                    FlowDirection = FlowDirection.LeftToRight;
                    //label.Margin = new Padding(0, 0, 0, 0);
                }
            }
            //}
            switch (Tip)
            {
                case 8:
                case 9:
                    VrstaKontrole = "datum";
                    dtp = new DateTimePicker()
                    {
                        CustomFormat = "dd.MM.yy",
                        Format = DateTimePickerFormat.Custom,
                        BackColor = Color.AliceBlue,

                        Tag = Tip,
                        Name = Ime,
                        //AllowDrop = true
                    };

                    if (EnDis == "D")
                    {
                        dtp.Enabled = false;
                    }


                    //Djora 26.09.20
                    //dtp.Height = (int)visina;
                    //dtp.Height = (int)visina * ofsety;
                    dtp.Height = (int)(visina * ofsety);

                    if (PozicijaLabele == "1")
                    {
                        //Djora 09.07.20
                        //dtp.Width = (int)(sirina);
                        dtp.Width = (int)Convert.ToDouble((sirina * ofset) / 2);
                    }
                    if (PozicijaLabele == "0" || PozicijaLabele == "2")
                    {
                        //int sirina = (int)Convert.ToDouble(Convert.ToDouble(t2.Rows[i]["WidthKolone"].ToString()) * 2.12);
                        dtp.Width = (int)Convert.ToDouble(sirina * ofset);
                    }


                    //dtp.Font = new Font("TimesRoman", 10.8F, FontStyle.Bold);
                    //dtp.Text.a

                    //Djora 26.09.20
                    //dtp.Font = new Font("TimesRoman", 13, FontStyle.Regular);

                    PromenaFonta(dtp);

                    dtp.Format = DateTimePickerFormat.Custom;
                    dtp.CustomFormat = "dd.MM.yyyy";
                    //dtp.Format = DateTimePickerFormat.Short;

                    //Djora 26.09.20
                    dtp.Margin = new Padding(0, 0, 0, 0);

                    this.Controls.Add(dtp);

                    //Djora 08.07.20
                    dtp.Parent.Name = Ime;


                    if (Tip == 8)
                    {
                        //dtp.Text = string.Format("{0:dd.MM.yy}", System.DateTime.Now.ToString());
                        dtp.Value = Convert.ToDateTime(System.DateTime.Now);
                    }
                    else // tip 9
                    {
                        string mgodina = System.DateTime.Now.ToString().Substring(6, 2);
                        //string mgodina = System.DateTime.Now.ToString().Substring(6, 4);
                        string mdatum = "01.01." + mgodina;
                        dtp.Value = Convert.ToDateTime(mdatum);
                    }
                    Vrednost = dtp.Value.ToShortDateString();

                    // dogadjaj kod izmene datuma
                    dtp.ValueChanged += new EventHandler(dtp_ValueChanged);
                    dtp.TextChanged += new EventHandler(dtp_TextChanged);

                    break;
                case 24:
                    VrstaKontrole = "cek";

                    cekboks = new CheckBox();

                    cekboks.Name = Ime;
                    cekboks.Text = ctekst; //    label_text;
                    //Djora 30.12.20
                    //cekboks.Height = (int)visina;
                    cekboks.Height = (int)(visina * ofsety);

                    //Djora 11.01.21
                    cekboks.Width = (int)(sirina * ofset);

                    if (EnDis == "D")
                    {
                        cekboks.Enabled = false;
                    }
                    if (cekboks.Checked == true)
                        Vrednost = "1";
                    else
                        Vrednost = "0";

                    //Djora 26.09.20
                    cekboks.Margin = new Padding(0, 0, 0, 0);

                    Controls.Add(cekboks);

                    //Djora 08.07.20
                    cekboks.Parent.Name = Ime;

                    //Ivana 14.12.2020.
                    cekboks.CheckedChanged += new EventHandler(checkBox_CheckedChanged);

                    //Djora 30.12.20
                    PromenaFonta(cekboks);

                    break;
                default:
                    if (izborno != null && izborno.Trim() != "") // ima izborno
                    {
                        VrstaKontrole = "combo";
                        comboBox = new ComboBox()
                        {
                            DropDownWidth = 300,
                            //Height = (int)visina,
                            Name = Ime,
                            Tag = Tip,
                            BackColor = Color.AliceBlue,
                            ForeColor = Color.Black,
                            FlatStyle = FlatStyle.Standard
                            //FlatStyle = FlatStyle.Flat

                        };
                        comboBox.SelectedIndex = -1;

                        comboBox.Text = "";
                        Vrednost = comboBox.Text;
                        ID = "1";

                        if (EnDis == "D")
                            comboBox.Enabled = false;
                        else
                            comboBox.Enabled = true;

                        if (PozicijaLabele == "1")
                        {
                            //Djora 09.07.20
                            //comboBox.Width = (int)(sirina);
                            comboBox.Width = (int)Convert.ToDouble((sirina * ofset) / 2);
                        }
                        if (PozicijaLabele == "0" || PozicijaLabele == "2")
                        {
                            comboBox.Width = (int)Convert.ToDouble(sirina * ofset);
                            comboBox.Left = Left;
                        }

                        //Djora 26.09.20
                        //comboBox.Font = new Font("TimesRoman", 13, FontStyle.Regular);
                        PromenaFonta(comboBox);

                        comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
                        comboBox.MaxDropDownItems = 10;

                        //Djora 13.07.20
                        comboBox.Height = (int)visina;

                        //Djora 26.09.20
                        comboBox.Margin = new Padding(0, 0, 0, 0);

                        Controls.Add(comboBox);

                        //Djora 08.07.20
                        comboBox.Parent.Name = Ime;

                        // obrada dogadjaja za comboBox             
                        //comboBox.GotFocus += new EventHandler(comboBox_GotFocus);
                        comboBox.MouseClick += new MouseEventHandler(comboBox_MouseClick);
                        //comboBox.TextUpdate += new EventHandler(comboBox_TextUpdate);
                        comboBox.KeyPress += new KeyPressEventHandler(comboBox_KeyPress);
                        comboBox.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
                        comboBox.DropDown += new EventHandler(comboBox_DropDown);
                        comboBox.DropDownClosed += new EventHandler(comboBox_DropDownClosed);
                        //zajedno 24.12.2020.
                        string upit = "select count(*) from RecnikPodataka where AlijasPolja like 'NazivPolja%' and Dokument=@param0 and ID_TipoviPodataka=10 and TabIndex>=0";
                        int broj = db.ParamsInsertScalar(upit, cDokument);

                        string upit1 = "select AlijasPolja from RecnikPodataka where AlijasPolja like 'NazivSkl%' and Dokument=@param0 and ID_TipoviPodataka=10 and TabIndex>=0";
                        DataTable dt = db.ParamsQueryDT(upit1, cDokument);
                        if (dt.Rows.Count == 1)
                            ((Bankom.frmChield)forma).nastavakSkladista1 = dt.Rows[0][0].ToString().Substring(8);
                        else if (dt.Rows.Count == 2)
                        {
                            ((Bankom.frmChield)forma).nastavakSkladista1 = dt.Rows[0][0].ToString().Substring(8);
                            ((Bankom.frmChield)forma).nastavakSkladista2 = dt.Rows[1][0].ToString().Substring(8);
                        }
                        //for (int i=0;i<dt.Rows.Count;i++)
                        //{
                        //    if (dt.Rows[i][0].ToString().Length > 8)
                        //        nastavakSkladista1 = dt.Rows[i][0].ToString().Substring(8);
                        //}
                        if (IME.Contains("NazivSkl") && broj > 0) //uci ce samo kada ima mag polje, a desavace se samo pri stvaranju comboBox-a NazivSkl
                        {
                            ((Bankom.frmChield)forma).NazivSkladista = comboBox.Text;
                            comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
                        }
                        //zajedno 9.3.2021.
                        upit = "Select barkod from Lot where Lot.ID_Artikli = @param0";
                       // dt = db.ParamsQueryDT(upit, );
                        if (IME.Contains("Artik") && broj > 0)
                        {


                        }
                        comboBox.Leave += new EventHandler(Leave);
                        comboBox.Validating += comboBox_Validating;
                    }
                    else // nema izborno 
                    {
                        VrstaKontrole = "tekst";
                        textBox = new TextBox()
                        {
                            BackColor = Color.AliceBlue,
                            ForeColor = Color.Black,
                            Tag = Tip,  //Tip polja: broj, datum, combobox, ....                            
                            Name = Ime,
                            Text = "",
                            Enabled = true
                        };

                        if (visina > 0)
                        {
                            textBox.Visible = true;
                        }
                        else
                        {
                            textBox.Visible = false;
                        }
                        Vrednost = textBox.Text;

                        if (PozicijaLabele == "1")
                        {
                            //Djora 09.07.20
                            //textBox.Width = (int)sirina;
                            textBox.Width = (int)Convert.ToDouble((sirina * ofset) / 2);

                        }
                        if (PozicijaLabele == "0" || PozicijaLabele == "2")
                        {
                            textBox.Width = (int)Convert.ToDouble(sirina * ofset);
                        }

                        //Djora 26.09.20
                        //textBox.Height = (int)visina;
                        textBox.Height = (int)(visina * ofsety);


                        if (Tip == 3 || Tip == 4 || Tip == 5 || Tip == 6 || Tip == 7 || Tip == 11 || Tip == 13 || Tip == 17 || Tip == 19 || Tip == 20 || Tip == 21)
                        {
                            textBox.TextAlign = HorizontalAlignment.Right;

                            //Djora 31.08.20
                            textBox.Text = "0";

                            clsFormInitialisation fi = new clsFormInitialisation();
                            textBox.Text = fi.FormatirajPolje(textBox.Text, Tip);
                        }

                        else
                        {
                            textBox.TextAlign = HorizontalAlignment.Left;
                        }


                        if (EnDis == "D")
                        {
                            textBox.ReadOnly = true;
                        }
                        textBox.MouseDown += new MouseEventHandler(textBox_MouseDown);
                        textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
                        textBox.TextChanged += new EventHandler(textBox_TextChange);
                        textBox.LostFocus += new EventHandler(textBox_LostFocus);

                        //Djora 28.08.20                       
                        textBox.Enter += new EventHandler(textBox_Enter);

                        Vrednost = textBox.Text;
                        ID = "1";

                        //Djora 26.09.20
                        //textBox.Font = new Font("TimesRoman", 13, FontStyle.Regular);
                        PromenaFonta(textBox);

                        if (textBox.Tag == null)
                        {
                            //MessageBox.Show(Ime);
                        }

                        //Djora 26.09.20
                        textBox.Margin = new Padding(0, 0, 0, 0);

                        Controls.Add(textBox);

                        //Djora 08.07.20
                        textBox.Parent.Name = Ime;
                    }
                    break;


            }
            //}// kraj za idNaziviNaFormi<>20

            //Uzima iz upita upite za datagrigove  TUD <> 0 i dogradjuje ih sa npr. where  id_KonacniRacunTotali =
            if (idNaziviNaFormi == "20" && tud != "0")
            {
                string tIme = Ime.Replace("ID_", "GgRr");
                //26.09.20
                //string sel = " SELECT TUD, MaxHeight, Levo, Vrh, Width, height, Upit, Ime "
                //                                + " FROM dbo.Upiti "
                //                                + " WHERE(NazivDokumenta = N'" + dokument + "') "
                //                                + " AND(Ime = N'" + tIme + "') "
                //                                + " AND(TUD <> 0) ORDER By TUD";
                //26.09.20
                string sel = " SELECT TUD, MaxHeight, Levo, CVrh as Vrh, cWidth as Width, height, Upit, Ime "
                                                + " FROM dbo.Upiti "
                                                + " WHERE(NazivDokumenta = N'" + dokument + "') "
                                                + " AND(Ime = N'" + tIme + "') "
                                                + " AND(TUD <> 0) ORDER By TUD";

                //Console.WriteLine(sel);
                DataTable t = db.ReturnDataTable(sel);
                if (t.Rows.Count > 0)
                {
                    var tLevo = Convert.ToDouble(t.Rows[0]["Levo"].ToString()) * ofset;
                    var tVrh = Convert.ToDouble(t.Rows[0]["Vrh"].ToString()) * ofsety;
                    var tWidth = Convert.ToDouble(t.Rows[0]["Width"].ToString()) * ofset + 13.5;

                    string tUpit = t.Rows[0]["Upit"].ToString();
                    int ttud = Convert.ToInt32(t.Rows[0]["TUD"].ToString()); // borka 07.08.0
                    //Djora 10.07.20
                    //var cheight = (Convert.ToDouble(t.Rows[0]["height"].ToString()) + 0.8) * ofset;
                    //var cheight = (Convert.ToDouble(t.Rows[0]["height"].ToString()) + 1.4);
                    //var cheight = (Convert.ToDouble(t.Rows[0]["height"].ToString())) * ofsety;

                    //Djora 09.07.20
                    //var brredova = Convert.ToDouble(t.Rows[0]["MaxHeight"].ToString());
                    var brredova = Convert.ToDouble(t.Rows[0]["MaxHeight"].ToString()); //* ofsety;

                    //Djora 26.09.20 14.10.20
                    //var theight = (Convert.ToDouble(t.Rows[0]["height"].ToString())) * ofsety * brredova;
                    var theight = (Convert.ToDouble(t.Rows[0]["height"].ToString())) * ofsety * 1.33333333 * 1.8;



                    dv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    dv.AllowUserToResizeRows = false;
                    dv.BorderStyle = BorderStyle.Fixed3D;
                    dv.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
                    dv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.AliceBlue;

                    //dv.DefaultCellStyle.SelectionForeColor = Color.Snow;
                    dv.BackgroundColor = Color.Snow;

                    dv.EnableHeadersVisualStyles = false;
                    dv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;


                    //Djora 26.09.20
                    PromenaFonta(dv);
                    //Djora 26.09.20
                    //dv.DefaultCellStyle.Font = new Font("TimesRoman", 13);

                    dv.Width = (int)(tWidth);
                    dv.Top = (int)tVrh;

                    //Djora 26.09.20
                    //dv.Height = (int)(theight);
                    dv.Height = (int)(theight * brredova);

                    //dv.Tag = ttud;
                    dv.Tag = "-1";
                    dv.Name = tIme;

                    //Da  moze da se edituju celije
                    dv.ReadOnly = false;
                    //Djora 14.05.20
                    dv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    //dv.RowHeadersWidth = 4;
                    //Da bude selektovan ceo red     

                    dv.RowHeadersVisible = false;
                    //dv.SelectionMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

                    dv.BackgroundColor = Color.Snow;


                    //Da duge reci vide cele u koloni (WRAP)
                    //Djora 26.09.20
                    //dv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                    //Da se poveca visina reda ako imamo wrap reci. Da bi moglo sve da se vidi 
                    //Djora 26.09.20
                    //dv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    //Djora 26.09.20
                    dv.RowTemplate.Height = Convert.ToInt32(theight); // 80;

                    switch (cImaNaslov)
                    {
                        case 0:
                            dv.ColumnHeadersVisible = false;
                            //dv.ColumnHeadersHeight = 18; //Convert.ToInt32(cheight);
                            dv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 5, FontStyle.Regular);
                            dv.ColumnHeadersDefaultCellStyle.BackColor = Color.AliceBlue;
                            dv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Snow;
                            break;
                        case 1:
                            dv.ColumnHeadersVisible = true;

                            //dv.Columnheaderscaption=
                            break;
                    }
                    //dv.Tag = -1; /// ttud;
                    //dv.RowHeadersWidth = 0;
                    dv.AllowUserToAddRows = false;
                    dv.AllowUserToResizeColumns = true;
                    dv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                    dv.Visible = true;
                    //dv.ColumnHeader.Font = new Font("TimesRoman", 5, FontStyle.Regular);

                    //Djora 26.09.20
                    //dv.Font = new Font("TimesRoman", 13, FontStyle.Regular);

                    //Djora 26.09.20
                    dv.Margin = new Padding(0, 0, 0, 0);
                    VrstaKontrole = "grid";
                    Controls.Add(dv);

                    //Djora 08.07.20
                    //dv.Parent.Name = Ime;

                    //Djora 10.07.20
                    dv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(dv_CellContenClick);
                    //Djora 14.05.20
                    //dv.CellContentDoubleClick += dv_CellContentDoubleClick;
                    dv.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dv_CellMouseDoubleClick);
                    //dv.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dv_ColumnHeaderMouseClick);
                }
            } // kraj za idNaziviNaFormi == "20" && tud != "0"



        }
        private Control activeControl;

        private Point previousLocation;

        //Ivana 11.12.2020.

        public DataTable dt = new DataTable();
        public void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            string upit = "Select distinct AlijasPolja FROM RecnikPodataka where TabIndex> -1 and ZavisiOd=@param0";
            dt = db.ParamsQueryDT(upit, this.Name);
            if (cekboks.Checked)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Field kontrola = (Field)Program.Parent.ActiveMdiChild.Controls[dt.Rows[i][0].ToString()];
                    kontrola.Visible = true;
                }
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Field kontrola = (Field)Program.Parent.ActiveMdiChild.Controls[dt.Rows[i][0].ToString()];
                    kontrola.Visible = false;
                }
            }
        }
        //zajedno 22.12.2020.
        public void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox.Name.Length == 8)
                ((Bankom.frmChield)forma).NazivSkladista = comboBox.Text;
            else if (((Bankom.frmChield)forma).nastavakSkladista1 == comboBox.Name.Substring(8))
            {
                ((Bankom.frmChield)forma).NazivSkladista1 = comboBox.Text;
                ((Bankom.frmChield)forma).nastavakSkladista1 = comboBox.Name.Substring(8);
            }
            else
            {
                ((Bankom.frmChield)forma).NazivSkladista2 = comboBox.Text;
                ((Bankom.frmChield)forma).nastavakSkladista2 = comboBox.Name.Substring(8);
            }
        }
        void textBox_MouseDown(object sender, MouseEventArgs e)
        {
            activeControl = sender as Control;
            previousLocation = e.Location;
            Cursor = Cursors.Hand;
        }
        void label_MouseClick(object sender, MouseEventArgs e)
        {
            activeControl = sender as Control;
            previousLocation = e.Location;
            Cursor = Cursors.Hand;
            //MessageBox.Show(activeControl.Left.ToString() + "     " + activeControl.Top.ToString() + "     " + activeControl.Width.ToString() + "        " + activeControl.Height.ToString());
        }
        private int GetIndexFocusedControl()
        {
            int ind = -1;
            foreach (Control ctr in this.Controls)
            {
                if (ctr.Focused)
                {
                    ind = (int)this.Controls.IndexOf(ctr);
                }
            }
            return ind;
        }

        void textBox_TextChange(object sender, EventArgs e)
        {
            ID = "1";
            Vrednost = textBox.Text;
        }

        //Djora 28.08.20
        void textBox_Enter(object sender, EventArgs e)
        {

            BeginInvoke((Action)delegate
            {
                this.textBox.SelectAll();
            });
        }
        void textBox_LostFocus(object sender, EventArgs e)
        {
            clsFormInitialisation FI = new clsFormInitialisation();
            int Tip = Convert.ToInt32(textBox.Tag.ToString());

            string sadrzaj = textBox.Text;
            textBox.Text = FI.FormatirajPolje(sadrzaj, Tip);
            Vrednost = textBox.Text;
            ID = "1";
        }
        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            int indexFocused = GetIndexFocusedControl();
            string ctip = this.Controls[indexFocused].Tag.ToString();


            //Djora 31.08.20
            switch (ctip)
            {
                case "4":
                case "11":
                case "26":
                    if (!char.IsControl(e.KeyChar)
                       && !char.IsDigit(e.KeyChar)
                       && e.KeyChar != '-'
                     )
                    {
                        e.Handled = true;
                    }

                    break;
                case "5":
                case "6":
                case "7":
                case "13":
                    if (e.KeyChar == '.')
                    {
                        e.KeyChar = ',';
                    }

                    if (!char.IsControl(e.KeyChar)
                        && !char.IsDigit(e.KeyChar)
                        && e.KeyChar != ','
                        && e.KeyChar != '-')
                    {
                        e.Handled = true;
                    }

                    // only allow one decimal point
                    if (e.KeyChar == ','
                        && (sender as TextBox).Text.IndexOf(',') > -1)
                    {
                        e.Handled = true;
                    }
                    break;

                case "8":
                    break;

                case "10":
                    break;

                case "14":
                    break;
            }
            //zajedno 22.1.2021.
            if (e.KeyChar == (char)Keys.Enter)
            {
                string tipFajla = "Slike";
                string nazivFoldera = "";
                if (IME.Contains("NazivSirovin") || IME.Contains("NazivArt") || IME.Contains("NazivProizv"))
                {
                    string upit = "Select ID_Artikli from Artikli where NazivArtikla=@param0";
                    dt = db.ParamsQueryDT(upit, Vrednost);
                    if (dt.Rows.Count > 0)
                    {
                        string imeSlike = dt.Rows[0][0].ToString();
                        nazivFoldera = "Artikli";
                        if (File.Exists(@"\\SQL2016\\ISDokumenta\\" + Program.imeFirme + "\\" + tipFajla + "\\" + nazivFoldera + "\\" + imeSlike + ".jpg"))
                        {
                            frmSlika slika = new frmSlika();
                            slika.groupBox1.Visible = false;
                            slika.cmbNazivSlike.Visible = false;
                            slika.button1.Visible = false;
                            slika.label1.Visible = false;
                            slika.button2.Visible = false;
                            slika.label2.Visible = false;
                            slika.pictureBox1.Image = Image.FromFile(@"\\SQL2016\\ISDokumenta\\" + Program.imeFirme + "\\" + tipFajla + "\\" + nazivFoldera + "\\" + imeSlike + ".jpg");
                            slika.Show();
                        }
                    }
                }
            }
        }
        string comboText = "";
        public void ComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ComboBox control = (ComboBox)sender;
            comboText = control.Text;
        }
        //private void comboBox_TextUpdate(Object sender, EventArgs e)
        public new void Leave(object sender, EventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            //MessageBox.Show("You are in the ComboBox.TextUpdate event.");
            if (!FoundText(combo))
            {
                //MessageBox.Show("This is not a valid ");              
                combo.ForeColor = Color.Red;
            }
            else
            {
                combo.ForeColor = Color.Black;
            }
        }
        //private void comboBox_GotFocus(object sender, EventArgs e)
        //{

        //}
        private void comboBox_MouseClick(object sender, EventArgs e)
        {
            //  comboBox.DroppedDown = true;
        }
        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            SendKeys.Send("{tab}");
        }
        private void comboBox_DropDown(object sender, EventArgs e)
        {
            ComboBox control = (ComboBox)sender;
            string sql = FillList(control, cTip);
            DataTable dt = new DataTable();
            bool crveno = false;
            //zajedno 24.12.2020.
            if (sql == "")
            {
                if (rez.Rows.Count != 0)
                {
                    for (int i = 0; i < rez.Rows.Count; i++)
                        if (rez.Rows[i][0].ToString().ToLower().Contains(control.Text.ToLower()))
                        {
                            control.Items.Add(rez.Rows[i][0]);
                            crveno = true;
                        }
                    if (crveno)
                    {
                        control.Text = "";
                        control.ValueMember = "iid";
                        control.DisplayMember = "polje";
                        ID = Convert.ToString(control.SelectedValue);
                        if (control.SelectedIndex > -1)
                            if (forma.Controls["OOperacija"].Text.Trim() != "PREGLED")
                                FillOtherControls(control, ID);
                        control.SelectedIndex = -1;
                    }
                    else
                    {
                        control.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                Console.WriteLine(sql);
                control.SelectedIndex = -1;
                dt = db.ReturnDataTable(sql);

                if (dt.Rows.Count > 0)
                {
                    // podesavanje sirine dropdown liste na sirinu sadrzaja                
                    int maxWidth = 0;
                    foreach (var row in dt.Rows)
                    {
                        var temp = TextRenderer.MeasureText(Convert.ToString(((System.Data.DataRow)row).ItemArray[1]), Font).Width;

                        if (temp > maxWidth)
                        {
                            maxWidth = temp;
                        }
                    }
                    control.DropDownWidth = maxWidth + 10;

                    control.DataSource = dt;
                    control.ValueMember = "iid";
                    control.DisplayMember = "polje";

                    ID = Convert.ToString(control.SelectedValue);
                    Vrednost = dt.Rows[0]["polje"].ToString().Trim();

                    //control.SelectedIndex = -1;
                    Console.WriteLine(forma.Controls["OOperacija"].Text.Trim());
                    if (control.SelectedIndex > -1)
                        if (forma.Controls["OOperacija"].Text.Trim() != "PREGLED") // BORKA da se ne bi u pregledu punila ostala polja za izabrano                        
                            FillOtherControls(control, ID);

                    control.SelectedIndex = -1;
                }
                else //.Rows.Count = 0
                {
                    control.DataSource = null;

                    ID = "1";
                    Vrednost = "";
                }
            }
        }

        //public new void Leave(object sender, EventArgs e)
        private void comboBox_Validating(object sender, CancelEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;

            if (!FoundText(combo))
            {
                //MessageBox.Show("This is not a valid ");
                //this.ClearDisplay();
                combo.ForeColor = Color.Red;
                e.Cancel = true;
            }
            else
            {
                combo.ForeColor = Color.Black;
                e.Cancel = false;
            }
        }

        //public new void Leave(object sender, EventArgs e)
        public Boolean FoundText(ComboBox control)
        {
            Boolean isfound = true;
            //ComboBox control = (ComboBox)sender;
            Form Me = Program.Parent.ActiveMdiChild;
            string uuPIT = "";
            string Restrikcija = "";
            // zajedno 28.12.2020. dopuna, jer je pucao pri zatvaranju, kada je Me bilo null
            if (control.Text.Trim() != "" && Me != null)
            {
                Field pb = (Field)Me.Controls[control.Name]; //  uzimamo kontrolu na formi  
                if (pb != null)
                {
                    if (pb.cRestrikcije.Trim() != "")
                    {
                        Restrikcija = " AND " + pb.cRestrikcije;
                    }

                    if ((control.Text).Trim() == "")
                    {
                        ID = "1";
                        Vrednost = "";
                    }
                    else
                    {
                        if (pb.cIzborno.Contains("KKNJ"))
                        {
                            ID = "1";
                        }
                        else  // nisu kknjupiti
                        {
                            if (pb.cTip == 25)
                                uuPIT = "SELECT ID_" + pb.cIzborno + " as idt," + pb.cPolje + " as polje from " + pb.cIzborno + " WHERE " + pb.cPolje + " Like N'%" + control.Text + "%'" + Restrikcija;
                            else
                                uuPIT = "SELECT ID_" + pb.cIzborno + " as idt," + pb.cPolje + " as polje from " + pb.cIzborno + " WHERE " + pb.cPolje + "='" + control.Text + "'" + Restrikcija;
                            Console.WriteLine(uuPIT);
                            DataTable tt = db.ReturnDataTable(uuPIT);
                            if (tt.Rows.Count > 0)
                            {
                                ID = tt.Rows[0]["idt"].ToString();
                                Vrednost = tt.Rows[0]["polje"].ToString();
                                if (pb.cTip == 25)
                                    aaa = tt.Rows[0]["polje"].ToString().Substring(0, tt.Rows[0]["polje"].ToString().IndexOf(","));
                                else
                                    aaa = tt.Rows[0]["polje"].ToString();

                                pb.ID = ID;
                                control.Text = aaa;
                                control.Refresh();

                                control.ForeColor = Color.Black;
                                if (forma.Controls["OOperacija"].Text.Trim() != "PREGLED") // BORKA da se ne bi u pregledu punila ostala polja za izabrano                        
                                    FillOtherControls(control, ID);
                            }
                            else
                            {
                                ID = "1";
                                Vrednost = "";
                                isfound = false;
                            }
                        }
                    }
                }
            }
            else
            {
                ID = "1";
                Vrednost = "";
            }
            return isfound;
        }
        public void dtp_TextChanged(Object sender, EventArgs e)
        {
            //Sluzi da se automatski pomeri na sledece polje u datetimepickeru da dana, na mesec, pa na godinu, dok kucam rucno.
            if (dtp.Focused == true)
                SendKeys.Send(".");
        }
        public void dtp_ValueChanged(Object sender, EventArgs e)
        {
            dtp.CustomFormat = "dd.MM.yyyy"; //jovana
            dtp.Format = DateTimePickerFormat.Custom;
            Vrednost = dtp.Value.ToShortDateString();
            //Vrednost = Vrednost.Substring(0, 6) + Vrednost.Substring(8, 2);
            ID = "1";
        }
        public void dtpTextChanged(Object sender, EventArgs e)
        {
            if (dtp.Focused)
                SendKeys.Send(".");
        }
        private void FillOtherControls(ComboBox control, string IdSloga)
        {
            string cQuery = "";

            cQuery = "Select * from " + cIzborno + " WHERE ID_" + cIzborno.Trim() + "=" + IdSloga;
            Console.WriteLine(cQuery);
            DataTable dt2 = db.ReturnDataTable(cQuery);
            foreach (var pb in this.Parent.Controls.OfType<Field>().Where(g => String.Equals(g.cAlijasTabele, cAlijasTabele)))
            {
                if (pb.IME != control.Name) // ovde ulaza samo kontrole razlicite od kontrole koju smo upravo napustili a imaju isti alijastabele
                {
                    pb.ID = "1";
                    if (dt2.Rows.Count == 0)
                    {
                        pb.Vrednost = "";
                    }
                    else
                    {
                        if (pb.cIzborno == cIzborno)
                        {
                            if (pb.cTip != 25)
                            {
                                try
                                {
                                    pb.ID = IdSloga;
                                    pb.Vrednost = dt2.Rows[0][pb.cPolje].ToString();
                                }
                                catch (Exception ex)
                                {
                                    pb.Vrednost = "";
                                }
                            }
                            else ///pb.cTip=25
                            {
                                pb.Vrednost = "";
                            }
                        }
                        else //nije pb.cizbormo=cizborno
                        {
                            if (pb.cIzborno.Trim() != "")
                            {
                                if (pb.cTip != 25)//nije lot
                                {
                                    try
                                    {
                                        pb.Vrednost = dt2.Rows[0][pb.cPolje].ToString();
                                    }
                                    catch (Exception ex)
                                    {
                                        pb.Vrednost = "";
                                    }
                                }
                                else // jeste lot 
                                {
                                    pb.Vrednost = "";
                                }
                            }
                            else //izborno jeste prazno
                            {
                                try
                                {
                                    pb.Vrednost = dt2.Rows[0][pb.cPolje].ToString();
                                }
                                catch (Exception ex)
                                {
                                    pb.Vrednost = "";
                                }
                            }
                        }   // NIJE cIzborno=izborno KRAJ                     

                        switch (pb.VrstaKontrole)
                        {
                            case "tekst":
                                pb.textBox.Text = pb.Vrednost;
                                break;
                            case "combo":
                                pb.comboBox.Text = pb.Vrednost;
                                break;
                            case "datum":
                                pb.dtp.Value = Convert.ToDateTime(pb.Vrednost);
                                break;
                        }
                    }
                } // NIJE POLJE KOJE SMO UPRAVO NAPUSTILI KRAJ
            } //polja koja pripadaju istom Alijasu Tabele KRAJ
        }
        /// <summary>


        /// <summary>
        /// 
        public DataTable rez = new DataTable();
        private string FillList(ComboBox control, int Tip)
        {
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;
            string sql = "";
            string Restrikcija = "";
            string Dokument = forma.Controls["limedok"].Text;
            string ssel = "";

            if (Dokument == "OpisTransakcije" && forma.Controls["ldokje"].Text == "D")
            {
                string tabela = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "Tabela").Vrednost;

                if (control.Name != "Tabela" && control.Name != "Konto" && control.Name != "Analitika")
                {
                    if (control.Name == "Valuta")
                        Restrikcija = "(" + control.Name + " like '%-" + tabela + "%'  OR " + control.Name + " = 'DomacaValuta')";
                    else
                        Restrikcija = "(" + control.Name + " like '%-" + tabela + "%' )";
                }
            }

            Restrikcija += cRestrikcije.Trim();
            //zajedno 24.12.2020.
            if (IME.Contains("NazivPolja") && Tip == 10)
            {
                comboBox.Items.Clear();
                string upit = "select NazivPolja from MagacinskaPoljaStavkeView where NazivSkl=@param0";
                // jovana 13.01.21
                if (((Bankom.frmChield)forma).nastavakSkladista2 != "" && ((Bankom.frmChield)forma).NazivSkladista2 != null && IME.Substring(10).Contains(((Bankom.frmChield)forma).nastavakSkladista2))
                    rez = db.ParamsQueryDT(upit, ((Bankom.frmChield)forma).NazivSkladista2);
                else if (((Bankom.frmChield)forma).nastavakSkladista1 != "" && ((Bankom.frmChield)forma).NazivSkladista1 != null && IME.Substring(10).Contains(((Bankom.frmChield)forma).nastavakSkladista1))
                    rez = db.ParamsQueryDT(upit, ((Bankom.frmChield)forma).NazivSkladista1);
                else
                    rez = db.ParamsQueryDT(upit, ((Bankom.frmChield)forma).NazivSkladista);
            }
            else if (Tip != 3)
            {
                if (Restrikcija.Trim() != "") // postji restrikcija
                {
                    if (cIzborno == "Dokumenta" && control.Name == "Predhodni")
                    {
                        ssel = " SELECT DISTINCT TOP 50 ID_" + cIzborno + " as iid," + cPolje + " as polje FROM " + cIzborno;
                        ssel += " WHERE " + cPolje + " like '%" + control.Text + "%' AND (" + Restrikcija + ")";
                    }
                    else
                    {
                        if (cRestrikcije.ToUpper().Contains("BRDOK") == true)  ///' ako jeste izbor po broju dokumenta
                        {
                            ssel = " SELECT DISTINCT ID_" + cIzborno + " as iid," + cPolje + " as polje FROM " + cIzborno;
                            ssel += " WHERE " + cPolje + " like '%" + control.Text + "%' AND (" + Restrikcija + ")";
                            Console.WriteLine(ssel);
                        }
                        else// control.text ne sadrzi brdok 
                        {
                            ssel = "SELECT DISTINCT ID_" + cIzborno + " as iid, " + cPolje + " as polje FROM " + cIzborno;
                            ssel += " WHERE " + cPolje + " like'%" + control.Text.Trim() + "%'  AND  (" + Restrikcija.Trim() + ")  ORDER BY polje";
                        }
                    }
                }
                else // nema restrikciju
                {
                    if (cIzborno == "Dokumenta" && control.Name == "Predhodni")
                    {
                        ssel = "SELECT DISTINCT Top 50 ID_" + cIzborno + " as iid, " + cPolje + " as polje FROM " + cIzborno;
                        ssel += " WHERE " + cPolje + " like'%" + control.Text.Trim() + "%'  ORDER BY polje ";
                    }
                    else
                    {
                        ssel = "SELECT DISTINCT  ID_" + cIzborno + " as iid, " + cPolje + " as polje FROM " + cIzborno;
                        ssel += " WHERE " + cPolje + " like'%" + control.Text.Trim() + "%'  ORDER BY polje ";
                    }
                }
            }
            else   // jeste tip=3
            {
                clsOperacije co = new clsOperacije();
                if (co.IsNumeric(control.Text) == false) { }
                if (cRestrikcije.Trim() != "") // ima restrikciju
                {
                    if (co.IsNumeric(control.Text) == true)
                        ssel = "SELECT DISTINCT TOP 50 ID_" + cIzborno + " as iid," + cPolje + " as polje FROM " + cIzborno + " WHERE " + cPolje + " >= " + control.Text + " And " + Restrikcija.Trim() + " ORDER BY polje ";
                    else
                        ssel = "SELECT DISTINCT TOP 50 ID_" + cIzborno + " as iid," + cPolje + " as polje FROM " + cIzborno + " WHERE " + Restrikcija.Trim() + " Order by polje";
                }
                else // nema restrikciju
                {
                    if (co.IsNumeric(control.Text) == true)
                        ssel = "SELECT DISTINCT TOP 50 ID_" + cIzborno + " as iid," + cPolje + " as polje FROM " + cIzborno + " WHERE " + cPolje + " >= " + control.Text + " ORDER BY polje ";
                    else
                        ssel = "SELECT DISTINCT Top 50  ID_" + cIzborno + " as iid," + cPolje + " as polje FROM " + cIzborno + " Order by polje";

                }
            }
            sql = ssel;
            return sql;
        }
        private void dv_CellContenClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            Program.colname = "";
            DataGridView control = (DataGridView)sender;
            DataTable myt = (DataTable)control.DataSource;
            Program.imegrida = control.Name;
            Program.colname = dv.Columns[e.ColumnIndex].Name;
            Program.activecontrol = control;
            dv.BackgroundColor = Color.AliceBlue;
        }
        private void FillControls(DataGridView control, ref int iid, ref string brdok, ref DateTime datum, DataGridViewCellMouseEventArgs e)
        {
            dv.BackgroundColor = Color.AliceBlue;
            string mimegrida = control.Name;
            mimegrida = mimegrida.Substring(4);
            string sel = "";
            DataBaseBroker db = new DataBaseBroker();
            DataTable t = new DataTable();
            int i = 0;
            foreach (DataGridViewColumn column in control.Columns)
            {
                Field pb = (Field)Program.Parent.ActiveMdiChild.Controls[column.Name];
                {
                    if (pb != null)
                    {
                        if (pb.cTip == 25)
                        {
                            if (control.Rows[e.RowIndex].Cells[i].FormattedValue.ToString().Trim() != "")
                            {
                                sel = " Select barkod,ID_LotVieW from LotView where barkod='" + control.Rows[e.RowIndex].Cells[i].FormattedValue.ToString() + "'";
                                Console.WriteLine(sel);
                                t = db.ReturnDataTable(sel);
                                if (t.Rows.Count > 0)
                                {
                                    pb.Vrednost = t.Rows[0]["barkod"].ToString();
                                    pb.ID = t.Rows[0]["ID_LotView"].ToString();
                                }
                            }
                        }
                        else
                            pb.Vrednost = control.Rows[e.RowIndex].Cells[i].FormattedValue.ToString();

                        switch (pb.VrstaKontrole)
                        {
                            case "tekst":
                                pb.textBox.Text = control.Rows[e.RowIndex].Cells[i].FormattedValue.ToString();
                                pb.Vrednost = pb.textBox.Text;///  pb.Vrednost.Replace(".", "").Replace(",", "."); ///borka 01.09.20
                                if (pb.IME == "BrDok")
                                {
                                    brdok = pb.textBox.Text;
                                }
                                break;
                            case "combo":
                                pb.comboBox.Text = control.Rows[e.RowIndex].Cells[i].Value.ToString();
                                DataTable myt = (DataTable)control.DataSource;
                                if (pb.cIzborno == pb.cTabela)
                                {
                                    if (myt.Columns.Contains("ID_" + pb.cAlijasTabele))
                                        pb.ID = myt.Rows[e.RowIndex]["ID_" + pb.cAlijasTabele].ToString();
                                }
                                else
                                {
                                    if (pb.cIzborno.Trim() != "" && myt.Columns.Contains("ID_" + pb.cIzborno))
                                        pb.ID = myt.Rows[e.RowIndex]["ID_" + pb.cIzborno].ToString();
                                }


                                break;
                            case "datum":
                                pb.dtp.Value = Convert.ToDateTime(control.Rows[e.RowIndex].Cells[i].FormattedValue.ToString());
                                if (pb.IME == "Datum")
                                {
                                    datum = pb.dtp.Value;
                                }
                                break;
                            case "cek":
                                if (control.Rows[e.RowIndex].Cells[i].Value.ToString() == "1")
                                {
                                    pb.cekboks.Checked = true;
                                    Vrednost = "1";
                                }
                                else
                                {
                                    pb.cekboks.Checked = false;
                                    Vrednost = "0";
                                }
                                break;
                        }
                    }
                    if (column.Name.ToUpper().Contains("IID"))
                    {
                        iid = Convert.ToInt32(control.Rows[e.RowIndex].Cells[i].Value.ToString());
                        // jovana 04.11. grid tag napuni sa idreda
                        control.Tag = control.Rows[e.RowIndex].Cells[i].Value.ToString();
                        ((Bankom.frmChield)forma).idReda = iid;
                    }
                }
                i++;
            }
        }

        //Djora 14.05.20
        private void dv_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Program.brtabova < 10)
            {
                DataGridView control = (DataGridView)sender;

                Form Me = Program.Parent.ActiveMdiChild;
                int middok = 0; /// Convert.ToInt32(Me.Controls["liddok"].Text);
                string mbrdok = "";
                DateTime mdatum = Convert.ToDateTime(System.DateTime.Now);

                control.ReadOnly = true;
                string mimedok = Me.Controls["limedok"].Text.Trim();

                //  mimedok = Program.AktivnaSifraIzvestaja.ToString();
                //  mimedok = Program.AktivnaSifraIzvestaja.ToString();

                FillControls(control, ref middok, ref mbrdok, ref mdatum, e);    // punjenje kontrola koje se odnose na stavku iz reda grida na koji smo kliknuli

                string mojestablo = Me.Controls["limestabla"].Text.Trim();
                int midstablo = Convert.ToInt32(Me.Controls["lidstablo"].Text);

                ((Bankom.frmChield)Me).imegrida = control.Name;
                ((Bankom.frmChield)Me).idReda = middok;
                ((Bankom.frmChield)Me).brdok = mbrdok;

                string ddatum = mdatum.ToString("dd.MM.yy");
                string DokumentJe = ((Bankom.frmChield)Me).DokumentJe;
                if (mojestablo == "Dokumenta" && DokumentJe == "S")
                {
                    ((Bankom.frmChield)Me).iddokumenta = middok;
                    ((Bankom.frmChield)Me).imedokumenta = ((Bankom.frmChield)Me).imestabla;
                }


                if (Me.Controls["OOperacija"].Text.Trim() == "") // nije odabrana operacija
                {
                    if (mojestablo == "Dokumenta" && DokumentJe == "S") // kliknuli smo na spisak dokumenata i nismo odabrali operaciju
                    {
                        // OTVARAMO NOVU FORMU NA KOJOJ CEMO PRIKAZATI DOKUMENT NA KOJI SMO KLIKNULI U SPISKU DOKUMENATA (RED U GRIDU)
                        // NOVOJ FORMI PREDAJEMO SLEDECE PODATKE: imedokumenta,idstablo,iddokumenta,brojdokumenta,vrstudokumenta,zadatuoperaciju,vrstuprikaza 
                        //jovana
                        clsDokumentaStablo ds = new clsDokumentaStablo();
                        if (ds.Obradi(middok, ref midstablo, ref mimedok, ref mbrdok) == false) return;
                        Program.Parent.ShowNewForm(mojestablo, midstablo, mimedok, middok, mbrdok, ddatum, "D", "", "");
                    }
                    else
                        this.ObradiDupliKlik(control, mimedok, DokumentJe, "", e);
                }
                else
                {
                    //((Bankom.frmChield)Me).idReda = middok;
                }

                control.ReadOnly = false;
            }
            else
                MessageBox.Show("Imate dosta otvorenih formi, zatvorite neku od njih.");
        }
        private void ObradiDupliKlik(DataGridView control, string Dokument, string DokumentJe, string OperacijaDokumenta, DataGridViewCellMouseEventArgs e)
        {
            dv.BackgroundColor = Color.AliceBlue;
            Form Me = Program.Parent.ActiveMdiChild;

            string[] separators = new[] { "," };
            Form novaforma = new Form();

            string vl, ll, sts, ss;
            int lid;
            long ai;
            string brojdok = "";

            DataTable t = new DataTable();
            string parametri = "";
            string vrednostiparametara = "";
            string KojiDokument = "";
            int k = e.RowIndex;
            t = (DataTable)control.DataSource;
            clsIzvestaji Izv = new clsIzvestaji();
            switch (DokumentJe)
            {
                case "I":             // mogucnost da se  klikom na grid izvestaja prikaze dokument
                    string ssi = "SELECT distinct Polje,uslov,uslovoperacija,izborno,PoljeSaDok,NazivDokumenta ";
                    ssi += "FROM Izvestaji WHERE (Uslov=1 or Uslov=2 or Uslov=4) AND Izvestaj =@param0 ";
                    ssi += "order by NazivDokumenta,uslov ";
                    Console.WriteLine(ssi);
                    t = db.ParamsQueryDT(ssi, Dokument);

                    int i = 0;
                    brojdok = "";
                    string konto = "";
                    string opiskonta = "";
                    switch (Dokument)

                    {
                        case "SpisakDokumenata":
                            foreach (DataGridViewColumn column in control.Columns)
                            {
                                if (column.Name == ("BrojDokumenta"))
                                {
                                    brojdok = control.Rows[k].Cells[i].Value.ToString();
                                    //brojdok = kolona.Substring(kolona.IndexOf(":") + 1, kolona.IndexOf(",") - kolona.IndexOf(":") - 1);
                                    break;     // izlaz iz petlje
                                }
                                i++;
                            }
                            break;
                        case "ZurnalKnjizenja":
                            foreach (DataGridViewColumn column in control.Columns)
                            {
                                if (column.Name == ("OpisKnj"))
                                {
                                    string kolona = control.Rows[k].Cells[i].Value.ToString();
                                    brojdok = kolona.Substring(kolona.IndexOf(":") + 1, kolona.IndexOf(",") - kolona.IndexOf(":") - 1);
                                    break;     // izlaz iz petlje
                                }
                                i++;
                            }
                            break;
                        case "SpisakNalogaZaPrijem":
                            brojdok = t.Rows[k]["BrDok"].ToString();
                            break;
                        case "PortFolioKomitenta":
                            for (int ii = 0; ii < t.Rows.Count; ii++) // citamo tabelu izvestaji za uslov>0
                            {
                                if (t.Rows[ii]["Uslov"].ToString() == "4")
                                {
                                    parametri += "Firma" + ",";
                                    vrednostiparametara += Program.imeFirme + ",";
                                }
                                else
                                {
                                    Console.WriteLine(t.Rows[ii]["Polje"].ToString());
                                    Field kontrola = (Field)Me.Controls[t.Rows[ii]["Polje"].ToString()]; //  uzimamo kontrolu na formi                                  
                                    if (kontrola == null)
                                    {
                                        i = 0;
                                        foreach (DataGridViewColumn column in control.Columns)
                                        {
                                            if (column.Name == t.Rows[ii]["Polje"].ToString())
                                            {
                                                if (column.Name == "KontoSaOpisom")
                                                {
                                                    konto = control.Rows[k].Cells[i].Value.ToString().Substring(0, control.Rows[k].Cells[i].Value.ToString().IndexOf("-"));
                                                    opiskonta = control.Rows[k].Cells[i].Value.ToString().Substring(control.Rows[k].Cells[i].Value.ToString().IndexOf("-") + 1);
                                                    parametri += "Konto" + ",";
                                                    vrednostiparametara += konto + ",";
                                                    parametri += "OpisKonta" + ",";
                                                    vrednostiparametara += opiskonta + ",";
                                                }
                                                else
                                                {
                                                    parametri += column.Name + ",";
                                                    vrednostiparametara += control.Rows[k].Cells[i].Value.ToString() + ",";
                                                }
                                            }
                                            i++;
                                        }
                                    }
                                    else
                                    {
                                        parametri += kontrola.IME + ",";
                                        vrednostiparametara += kontrola.Vrednost + ",";
                                    }
                                }

                            }
                            KojiDokument = "KarticaFinansijska";
                            break;
                        case "StanjeRobe":
                            parametri = "";
                            vrednostiparametara = "";
                            i = 0;
                            foreach (DataGridViewColumn column in control.Columns)
                            {
                                if (column.Name == "NazivArt" || column.Name == "StaraSifra")
                                {
                                    parametri += column.Name + ",";
                                    vrednostiparametara += control.Rows[k].Cells[i].Value.ToString() + ",";
                                }
                                i++;
                            }
                            for (int ii = 0; ii < t.Rows.Count; ii++)
                            {
                                if (t.Rows[ii]["Uslov"].ToString() == "4")
                                {
                                    parametri += "Firma" + ",";
                                    vrednostiparametara += Program.imeFirme + ",";
                                }
                                else
                                {
                                    Field kontrola = (Field)Me.Controls[t.Rows[ii]["Polje"].ToString()];
                                    if (kontrola.IME != "NazivArt")
                                    {
                                        parametri += kontrola.IME + ",";
                                        vrednostiparametara += kontrola.Vrednost.ToString() + ",";
                                    }
                                }
                            }

                            KojiDokument = "RobnaKartica";
                            break;
                        case "RobnaKartica":
                            foreach (DataGridViewColumn column in control.Columns)
                            {
                                if (control.Rows[k].Cells[i].Value.ToString().Contains(":") == true && column.Name.Contains("Datum") == false)
                                {
                                    string kolona = control.Rows[k].Cells[i].Value.ToString();
                                    brojdok = kolona.Substring(kolona.IndexOf(":") + 1, kolona.IndexOf(",") - kolona.IndexOf(":") - 1);
                                    break;     // izlaz iz petlje
                                }
                                i++;
                            }
                            break;
                        case "KarticaFinansijska":
                            foreach (DataGridViewColumn column in control.Columns)
                            {
                                if (control.Rows[k].Cells[i].Value.ToString().Contains(":") == true && column.Name.Contains("Datum") == false && column.Name.Contains("ValutaPl") == false)
                                {
                                    string kolona = control.Rows[k].Cells[i].Value.ToString();
                                    brojdok = kolona.Substring(kolona.IndexOf(":") + 1);
                                    break;     // izlaz iz petlje
                                }
                                i++;
                            }
                            break;
                        case "KarticaFinansijskaDinarska":
                            foreach (DataGridViewColumn column in control.Columns)
                            {
                                if (control.Rows[k].Cells[i].Value.ToString().Contains(":") == true && column.Name.Contains("Datum") == false && column.Name.Contains("ValutaPl") == false)
                                {
                                    string kolona = control.Rows[k].Cells[i].Value.ToString();
                                    brojdok = kolona.Substring(kolona.IndexOf(":") + 1);
                                    break;     // izlaz iz petlje
                                }
                                i++;
                            }
                            break;
                        case "LotStanjeRobeM":
                            parametri = "";
                            vrednostiparametara = "";
                            foreach (DataGridViewColumn column in control.Columns)
                            {
                                if (column.Name == "NazivArt" || column.Name == "StaraSifra" || column.Name == "Lot")
                                {
                                    parametri += column.Name + ",";
                                    vrednostiparametara += control.Rows[k].Cells[i].Value.ToString() + ",";
                                }
                                i++;
                            }
                            for (int ii = 0; ii < t.Rows.Count; ii++)
                            {
                                if (t.Rows[ii]["Uslov"].ToString() == "4")
                                {
                                    parametri += "Firma" + ",";
                                    vrednostiparametara += Program.imeFirme + ",";
                                }
                                else
                                {
                                    Field kontrola = (Field)Me.Controls[t.Rows[ii]["Polje"].ToString()];
                                    if (kontrola.IME != "NazivArt")
                                    {
                                        parametri += kontrola.IME + ",";
                                        vrednostiparametara += kontrola.Vrednost.ToString() + ",";
                                    }
                                }
                            }
                            KojiDokument = "LotRobnaKarticaM";
                            break;
                    }
                    // Prikaz Sledeceg izvestaja iz otvorenog izvestaja na formi
                    if (KojiDokument.Trim() != "")
                    {
                        parametri = parametri.Substring(0, parametri.Length - 1);
                        vrednostiparametara = vrednostiparametara.Substring(0, vrednostiparametara.Length - 1);
                        string[] mparam = parametri.Split(separators, StringSplitOptions.None);
                        string[] mvred = vrednostiparametara.Split(separators, StringSplitOptions.None);

                        string mojestablo = Program.Parent.ActiveMdiChild.Controls["limestabla"].Text.Trim();
                        string mst = mojestablo + "Stablo";
                        DataTable tt = new DataTable();
                        string sql = "Select ID_" + mst + " as IdSt From " + mst + " Where Naziv =@param0";
                        t = db.ParamsQueryDT(sql, KojiDokument);
                        int midstablo = Convert.ToInt32(t.Rows[0]["IdSt"].ToString());
                        Program.Parent.ShowNewForm(mojestablo, midstablo, KojiDokument, -1, "", "", DokumentJe, "", "");

                        novaforma = Program.Parent.ActiveMdiChild;
                        if (novaforma.Text != KojiDokument.Trim()) { break; }


                        for (i = 0; i < mvred.Length; i++)
                        {
                            if (mvred[i] == Program.imeFirme) { }
                            else
                            {
                                Field kontrola = (Field)novaforma.Controls[mparam[i].ToString()];
                                if (kontrola != null)
                                {
                                    Console.WriteLine(mparam[i]);
                                    kontrola.Vrednost = mvred[i];
                                    if (kontrola.cIzborno.Trim() != "")
                                        kontrola.comboBox.Text = kontrola.Vrednost;
                                    if (kontrola.cTip == 8 || kontrola.cTip == 9)
                                        kontrola.dtp.Value = Convert.ToDateTime(kontrola.Vrednost);
                                }
                            }
                        }
                        Izv.PrikazIzvestaja(KojiDokument);
                    }
                    else // Zahtev za prikaz Dokumenta klikom na red grida izvestaja
                    {
                        if (brojdok.Trim() != "")
                        {
                            if (brojdok.Contains(",") == true)
                                brojdok = brojdok.Substring(0, brojdok.IndexOf(",") - 1);
                            this.PrikaziDokument(brojdok);
                        }
                    }
                    break;     //kraj dokumentje="I"                   
                case "D":
                    if (Dokument == "Izvod")
                    {
                        if (OperacijaDokumenta == "Izmena" || OperacijaDokumenta == "Brisanje" || OperacijaDokumenta == "Unos") { }
                        else
                            brojdok = t.Rows[k]["PozivNaBroj"].ToString();
                        if (brojdok.Trim() != "")
                        {
                            this.PrikaziDokument(brojdok);
                        }
                    }
                    //jovana 09.09.20
                    if (Dokument == "Nalog1450")
                    {
                        if (OperacijaDokumenta == "Izmena" || OperacijaDokumenta == "Brisanje" || OperacijaDokumenta == "Unos") { }
                        else
                            brojdok = t.Rows[k]["BrRacVPred"].ToString();
                        if (brojdok.Trim() != "")
                        {
                            string sel = "Select BrojDokumenta from Dokumenta,RacunVPredracunView where id_Dokumenta=id_RacunVPredracunView and BrDok=@param0";
                            DataTable tt = db.ParamsQueryDT(sel, brojdok);
                            if (tt.Rows.Count != 0)
                            {
                                brojdok = tt.Rows[k]["BrojDokumenta"].ToString();
                                if (brojdok.Trim() != "")
                                {
                                    this.PrikaziDokument(brojdok);
                                }
                            }
                        }
                    }
                    if (Dokument == "Kompenzacija")
                    {
                        if (OperacijaDokumenta == "Izmena" || OperacijaDokumenta == "Brisanje" || OperacijaDokumenta == "Unos") { }
                        else
                        {
                            if (control.Name.Contains("1") == true)
                                brojdok = t.Rows[k]["OpisKnj1"].ToString().Substring(t.Rows[k]["OpisKnj1"].ToString().IndexOf(":") + 1);
                            else
                                brojdok = t.Rows[k]["OpisKnj2"].ToString().Substring(t.Rows[k]["OpisKnj2"].ToString().IndexOf(":") + 1);
                        }

                        if (brojdok.Trim() != "")
                        {
                            if (brojdok.Contains(",") == true)
                                brojdok = brojdok.Substring(0, brojdok.IndexOf(","));
                            this.PrikaziDokument(brojdok);
                        }
                    }
                    break;
            }
        }
        public void PrikaziDokument(string Broj)
        {
            if (Broj.Trim() != "")
            {
                string sel = "Select id_dokumentatotali as ID_dok,ID_DokumentaStablo,dokument,Datum from dokumentatotali where brdok=@param0";
                DataTable tt = db.ParamsQueryDT(sel, Broj);
                long iddokview = Convert.ToInt64(tt.Rows[0]["ID_dok"].ToString());
                int idstablo = Convert.ToInt32(tt.Rows[0]["ID_DokumentaStablo"].ToString());
                string dokument = tt.Rows[0]["dokument"].ToString();
                string mdatum = tt.Rows[0]["Datum"].ToString();
                Program.Parent.ShowNewForm("Dokumenta", idstablo, dokument, iddokview, Broj, mdatum, "D", "", "");
            }
        }
        public void PozivIzvestaja(string mdokument, string mNaslov)
        {
            //            Ccaption = ""
            //  '------------------------------------------
            switch (mdokument.Trim())
            {
                case "DospecePoKontima":
                    break;
                case "DospecaObaveza":
                    break;
                case "DospecaPotrazivanja":
                    break;
                case "PregledPrometaKomercijalistaPoMesecima":

                    if (Program.NazivBaze != "dbbbTestNew2003Bankom")
                        db.ExecuteStoreProcedure("AzurirajDospeca");
                    break;
                case "PrometKomercijalistaPoMesecima":
                    db.ExecuteStoreProcedure("AzurirajDospecaPoMesecima", Program.idFirme);
                    //      cnn1.Execute "AzurirajDospecaPoMesecima '" + Firma + "'", adCmdStoredProc
                    break;
                case "StanjeRobeUporedno":
                    if (Program.imeFirme == "Bankom" || Program.imeFirme == "Bioprotein")
                    {
                        //                  Dim DatumDo As String
                        //2:
                        //                  DatumDo = Trim$(InputBox("Unesite datum sa kojim zelite stanje ", "Uporedno stanje", Date))
                        //                  If DatumDo = "" Then GoTo KRAJ
                        //                     If Not IsDate(DatumDo) Then
                        //                        MsgBox "Pogresno unesen datum ponovite !!"
                        //                        GoTo 2:
                        //                     End If
                        //                     cnn1.Execute "UporednoStanjeRobe '" + Trim(DatumDo) + "'", adCmdStoredProc
                        //                  End If
                    }
                    break;
                case "DnevniPromet":
                    //'                RunAJob ImeServera, "Dnevni" + Firma
                    break;
                case "SpecifikacijaPoreklaRobe":
                    string sselect = "";
                    sselect = " alter view SpecPorekla  as ";
                    //sselect += "SELECT * from SpecPorekla1 WHERE Brdok= '" + BrojOtpremnice + "'";
                    //sselect += " UNION ";
                    //sselect += "SELECT * from SpecPorekla2 WHERE Brdok= '" + BrojOtpremnice + "'";
                    //sselect += " UNION ";
                    //sselect += "SELECT * from SpecPorekla3 WHERE Brdok= '" + BrojOtpremnice + "'";
                    //sselect += " UNION ";
                    //sselect += "SELECT * from SpecPorekla4 WHERE Brdok= '" + BrojOtpremnice + "'";
                    //sselect += " UNION ";
                    //sselect += "SELECT * from SpecPorekla5 WHERE Brdok= '" + BrojOtpremnice + "'";
                    //sselect += " UNION ";
                    //sselect += "SELECT * from SpecPorekla6 WHERE Brdok= '" + BrojOtpremnice + "'";

                    //           cnn1.Execute sselect
                    break;
            }
            // '------------------------
            string ssel = "";
            ssel = " Select * from sifarnikDokumenta where naziv=@param0";
            DataTable ts = new DataTable();
            ts = db.ParamsQueryDT(ssel, mdokument);

            if (ts.Rows.Count > 0)
            {
                string OperacijaDokumenta = "";
                string Dokument = mdokument;
                string Naslov = mNaslov;
                string PPrikaz = ts.Rows[0]["prikaz"].ToString();
                string NacinRegistracije = ts.Rows[0]["NacinRegistracije"].ToString();
                string NazivKlona = ts.Rows[0]["UlazniIzlazni"].ToString();
                string DokumentJe = ts.Rows[0]["Vrsta"].ToString();
                long IdDokView = 0;
                if (DokumentJe == "I")
                    IdDokView = -1;
                else
                    IdDokView = 0;
            }
        }

        //Djora 26.09.20
        public void PromenaFonta(Control kontrola)
        {

            if (kontrola == label)
            {
                label.ForeColor = Color.Black;
            }

            if (Screen.PrimaryScreen.Bounds.Width < 1440)
            {
                //if (Screen.PrimaryScreen.Bounds.Width <= 800)
                //{
                //    kontrola.Font = new Font("TimesRoman", 7F);
                //}
                //else
                //{
                kontrola.Font = new Font("TimesRoman", 8F);
                //}
            }
            else
            {
                kontrola.Font = new Font("TimesRoman", 13F);
            }
        }
        //zajedno 22.1.2021.
        private void comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //ivana 25.1.2021. zabrana otvaranja vise prozora za prikazivanje slika
            Form f = Application.OpenForms["frmSlika"];
            if (f != null)
                f.Close();
            if (e.KeyChar == (char)Keys.Enter)
            {
                string tipFajla = "Slike";
                string nazivFoldera = "";
                string imeSlike = "";
                string upit = "";
                frmSlika slika = new frmSlika();
                if (IME.Contains("NazivSkl"))
                {
                    upit = "Select ID_Skladiste from Skladiste where NazivSkl=@param0";
                    nazivFoldera = "Skladiste";
                }
                else if (IME.Contains("NazivPolja"))
                {
                    upit = "Select ID_MagacinskaPolja from MagacinskaPolja where NazivPolja=@param0";
                    nazivFoldera = "MagacinskaPolja";
                }
                else if (IME.Contains("NazivSirovin") || IME.Contains("NazivArt") || IME.Contains("NazivProizv"))
                {
                    upit = "Select ID_Artikli from Artikli where NazivArtikla=@param0";
                    nazivFoldera = "Artikli";
                }
                else if (IME.Contains("Sifr"))
                {
                    upit = "Select ID_Artikli from Artikli where ID_Artikli=@param0";
                    nazivFoldera = "Artikli";
                }
                if (upit != "")
                {
                    dt = db.ParamsQueryDT(upit, Vrednost);
                    if (dt.Rows.Count > 0)
                    {
                        imeSlike = dt.Rows[0][0].ToString();
                        if (slika.Visible)
                            slika.Close();
                        if (File.Exists(@"\\SQL2016\\ISDokumenta\\" + Program.imeFirme + "\\" + tipFajla + "\\" + nazivFoldera + "\\" + imeSlike + ".jpg"))
                        {
                            slika.groupBox1.Visible = false;
                            slika.cmbNazivSlike.Visible = false;
                            slika.button1.Visible = false;
                            slika.label1.Visible = false;
                            slika.button2.Visible = false;
                            slika.label2.Visible = false;
                            slika.pictureBox1.Image = Image.FromFile(@"\\SQL2016\\ISDokumenta\\" + Program.imeFirme + "\\" + tipFajla + "\\" + nazivFoldera + "\\" + imeSlike + ".jpg");
                            slika.Show();
                        }
                    }
                }
            }
        }
    }
}




