using Bankom.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using static System.Windows.Forms.AxHost;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using System.Data.SqlTypes;
using Microsoft.VisualBasic.Compatibility;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Threading;

namespace Bankom
{
    public partial class BankomMDI : Form
    {
        DataBaseBroker db = new DataBaseBroker();

        public string connectionString = Program.connectionString;

        public static string filter = "";
        private AutoCompleteStringCollection DataCollectionSpisak = new AutoCompleteStringCollection();

        public static string imeDokumenta = "";
        public BankomMDI()
        {
            InitializeComponent();
        }
        public void ShowNewForm(string imestabla, int idstablo, string imedokumenta, long iddokument, string brojdokumenta, string datum, string dokumentje, string operacija, string vrstaprikaza)
        {        
            
            string ss;
            if (dokumentje == "D")
            {
                ss = brojdokumenta; 
            }
            else
            {
                ss = imedokumenta;
            }
            Boolean odgovor;
            odgovor = DalijevecOtvoren(dokumentje, brojdokumenta, imedokumenta);            //string ss;    
            if (odgovor == false) ///nije vec otvoren
            {
               
                frmChield childForm = new frmChield();
                childForm.MdiParent = this;
                int sirina;
                if (IzborJezika.Text == "Српски-Ћирилица") { childForm.Name = VratiCirlilicu(imedokumenta);
            }
                sirina = (Width / 100) * 10;

                //tamara 01.02.2020.
                
        
                childForm.imedokumenta = imedokumenta;
                childForm.iddokumenta = iddokument;
                childForm.idstablo = idstablo;
                childForm.imestabla = imestabla;
                childForm.VrstaPrikaza = vrstaprikaza;
                childForm.brdok = brojdokumenta;
                childForm.DokumentJe = dokumentje;
                childForm.datum = datum;

                childForm.toolStripTextBroj.Text = Convert.ToString(idstablo);
                //tamara 01.02.2020. Text je prebacen u prazno da se ne bi prikazivao u liniji, a zamenjen je sa Name i to u itemn, itemb, itemb1 click
                childForm.Text = "";
                childForm.Name = ss;
                addFormTotoolstrip1(childForm, imedokumenta);

                if (imedokumenta == "LOT")
                {

                    foreach (var pb in childForm.Controls.OfType<Field>())
                    {
                        string s = pb.IME;
                        //if (pb.cTip == 10 || pb.cTip == 8)
                        //    pb.Enabled = false;
                        if (pb.cTip == 10)
                        {
                            if (pb.VrstaKontrole == "combo") pb.comboBox.Enabled = false;
                            else pb.textBox.Enabled = false;
                        }
                        if (pb.cTip == 8) pb.dtp.Enabled = false;
                    }
                }

                childForm.Show();
                SrediFormu(); // BORKA OVO MORA OSTATI!!!!!!!!!!!!!!!!!
            }
            updateToolStrip(ss);
        }
        //ivana 26.1.2021.
        int sirina = 0;
        public void updateToolStrip(string imedokumenta)
        {
            int a = toolStrip1.Items.Count;
            for (int i = 0; i < a; i++)
            {
                toolStrip1.Items[i].Font = new System.Drawing.Font("TimesRoman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                if (toolStrip1.Items[i].Text == imedokumenta)
                {
                    toolStrip1.Items[i].Font = new System.Drawing.Font("TimesRoman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                //ivana 26.1.2021.
                sirina += toolStrip1.Items[i].Width;
            }
        }
        public bool DalijevecOtvoren(string dokumentje, string brojdokumenta, string imedokumenta)
        {
            string ss;
            bool vrednost = true;
            foreach (Form f in Application.OpenForms)
            {
                if (dokumentje == "D")
                {
                    ss = brojdokumenta;
                    if (f.Text == ss)
                    {
                        MessageBox.Show("Ova forma je već otvorena.");
                        f.Focus();
                        vrednost = true;
                        break;
                    }
                    else
                    {
                        vrednost = false;
                    }
                }
                else
                {
                   
                    ss = imedokumenta;
                    if (f.Text == ss)
                    {
                        MessageBox.Show("Ova forma je već otvorena.");
                        f.Focus();
                        vrednost = true;
                        break;
                    }
                    else
                    {
                        vrednost = false;
                    }
                }
            }
            return vrednost;
        }
        public void addFormTotoolstrip1(Form forma, string imedokumenta)
        {
            //tamara 14.12.2020.
            toolStrip1.Visible = true;
            this.Width = Width - 20;
            ToolStripLabel itemn = new ToolStripLabel();
            ToolStripButton itemB = new ToolStripButton();
            ToolStripSeparator itemnsep = new ToolStripSeparator();
            itemn.Text = forma.Name;
            //zajedno 14.1.2021.
            //DataTable dt = new DataTable();
            //string upit = "Select NazivJavni from DokumentaStablo where Naziv=@param0";
            //dt = db.ParamsQueryDT(upit, forma.Text);
            //if(dt.Rows.Count>0)
            //    itemn.Text = dt.Rows[0][0].ToString();
            //itemn.Text = Program.AktivnaSifraIzvestaja;
            itemn.ToolTipText = imedokumenta;
            itemn.Name = forma.Name;
            itemB.Image = global::Bankom.Properties.Resources.del12;

            // ivana 26.1.2021.
            itemn.TextAlign = ContentAlignment.MiddleLeft;
            itemB.ImageAlign = ContentAlignment.MiddleRight;

            itemB.Size = new Size(3, itemn.Height);
            itemnsep.Size = new Size(2, itemn.Height);

            itemn.Click += new EventHandler(itemn_click);
            itemB.Click += new EventHandler(itemB_click);
            itemB.Name = forma.Name;
            itemnsep.Name = forma.Name;

            toolStrip1.Items.Add(itemn);
            toolStrip1.Items.Add(itemB);
            toolStrip1.Items.Add(itemnsep);
        }
        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }
        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }
        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }
        private void BankomMDI_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
          this.Text = "ISBankom - " +Program.NazivOrg + " - " + Program.imekorisnika;
            addKombo();
            clsSettingsButtons sb = new clsSettingsButtons();
            sb.ToolBarItemsEnDis();

            //tamara 22.10.2020.
            ToolBar.Enabled = true;
            //zajedno 18.1.2021.
            DodajSliku.Enabled = true;
            clsObradaMenija obradaMenija = new clsObradaMenija(this);
            obradaMenija.CreateMenu();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        ContextMenu cm = new ContextMenu();

        private void addKombo()
        {
            DataBaseBroker db2 = new DataBaseBroker();
            DataSet ds = new DataSet();
            DataView dv = new DataView();

            string stra = " WITH RekurzivnoStablo (ID_DokumentaStablo,Naziv, NazivJavni,BrDok,Vezan,RedniBroj,ccopy, Level,slave,pd,pp) AS  ( SELECT e.ID_DokumentaStablo, e.Naziv, e.NazivJavni, e.BrDok, e.Vezan, e.RedniBroj, e.ccopy, 0 AS Level, CASE e.vrstacvora  WHEN 'f' THEN 0 ELSE 1 END as slave, PrikazDetaljaDaNe as pd, PrikazPo as pp FROM DokumentaStablo AS e where Naziv in  (select g.naziv from Grupa as g, KadroviIOrganizacionaStrukturaStavkeView as ko Where(KO.ID_OrganizacionaStruktura = G.ID_OrganizacionaStruktura Or KO.id_kadrovskaevidencija = G.id_kadrovskaevidencija)  And KO.ID_OrganizacionaStruktura = 11 and ko.id_kadrovskaevidencija = 23) UNION ALL  SELECT e.ID_DokumentaStablo,e.Naziv,e.NazivJavni,e.BrDok, e.Vezan,e.RedniBroj, e.ccopy,Level + 1 ,  CASE e.vrstacvora WHEN 'f' THEN 0 ELSE 1 END as slave,PrikazDetaljaDaNe As pd, PrikazPo As pp FROM DokumentaStablo AS e INNER JOIN RekurzivnoStablo AS d  ON e.ID_DokumentaStablo = d.Vezan )  SELECT distinct NazivJavni FROM RekurzivnoStablo where ccopy = 0 order by NazivJavni";
            if (Directory.Exists(Application.StartupPath + @"\XmlLat") == false)
            {
                Directory.CreateDirectory(Application.StartupPath + @"\XmlLat");
            }

            if ((System.IO.File.Exists(Application.StartupPath + @"\XmlLat\lista.txt")) == true)

            {
                //    ds.ReadXml(Application.StartupPath + @"\XmlLat\addmenuKombo.xml");

                string[] novo1 = System.IO.File.ReadAllLines(Application.StartupPath + @"\XmlLat\" + "lista.txt");
                //string[] novo = alphabet.ToArray();
                return;
            }
            else

                ds = db2.ParamsQueryDS(stra);
            ds.WriteXml(Application.StartupPath + @"\XmlLat\" + "addmenuKombo.xml");
            ////////////////////////////////////////////////////////////////////////////         
            dv = ds.Tables[0].DefaultView;
            List<string> alphabet = new List<string>();
            int x1 = 0;
            do
            {
                alphabet.Add(item: dv[x1][0].ToString());
                //  alphabet.Add(item:  dv[x1][0].ToString());

                //tstbPretraga.AutoCompleteCustomSource.Add(" " + Convert.ToString(dv[x1][0]));

                x1++;
            } while (x1 < dv.Count);

            string[] novo = alphabet.ToArray();


            System.IO.File.WriteAllLines(Application.StartupPath + @"\XmlLat\" + "lista.txt", alphabet);

        }

       public string VratiCirlilicu(string inputLatinica)
        {
            string str;
            str = inputLatinica;
            if (str != "")
            {


                string sCir = "";
                string sCirZ = "";
                string[] separators = new[] { " < ", ">" };
                string[] words = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                foreach (string word in words)
                {
                    int zz = str.IndexOf("<" + word + ">");
                    if (zz > -1)
                    {
                        sCir = sCir + "<" + word + ">";
                    }
                    else
                    {
                        sCir = word;
                        sCir = sCir.Replace("nj", "њ");
                        sCir = sCir.Replace("lj", "љ");
                        sCir = sCir.Replace("dž", "џ");
                        sCir = sCir.Replace("dj", "ђ");
                        sCir = sCir.Replace("đ", "ђ");
                        sCir = sCir.Replace("Nj", "Њ");
                        sCir = sCir.Replace("Lj", "Љ");
                        sCir = sCir.Replace("Dž", "Џ");
                        sCir = sCir.Replace("Dj", "Ђ");
                        sCir = sCir.Replace("DJ", "Ђ");
                        sCir = sCir.Replace("Đ", "Ђ");
                        sCir = sCir.Replace("Lj", "Љ");
                        sCir = sCir.Replace("DŽ", "Џ");
                        sCir = sCir.Replace("NJ", "Њ");
                        sCir = sCir.Replace("LJ", "Љ");
                        sCir = sCir.Replace("d", "д");
                        sCir = sCir.Replace("ž", "ж");
                        sCir = sCir.Replace("z", "з");
                        sCir = sCir.Replace("l", "л");
                        sCir = sCir.Replace("n", "н");
                        sCir = sCir.Replace("h", "х");
                        sCir = sCir.Replace("p", "п");
                        sCir = sCir.Replace("r", "р");
                        sCir = sCir.Replace("s", "с");
                        sCir = sCir.Replace("ć", "ћ");
                        sCir = sCir.Replace("u", "у");
                        sCir = sCir.Replace("f", "ф");
                        sCir = sCir.Replace("c", "ц");
                        sCir = sCir.Replace("š", "ш");
                        sCir = sCir.Replace("b", "б");
                        sCir = sCir.Replace("v", "в");
                        sCir = sCir.Replace("g", "г");
                        sCir = sCir.Replace("a", "а");
                        sCir = sCir.Replace("e", "е");
                        sCir = sCir.Replace("j", "ј");
                        sCir = sCir.Replace("k", "к");
                        sCir = sCir.Replace("m", "м");
                        sCir = sCir.Replace("o", "о");
                        sCir = sCir.Replace("i", "и");
                        sCir = sCir.Replace("č", "ч");
                        sCir = sCir.Replace("t", "т");
                        sCir = sCir.Replace("D", "Д");
                        sCir = sCir.Replace("Ž", "Ж");
                        sCir = sCir.Replace("Z", "З");
                        sCir = sCir.Replace("L", "Л");
                        sCir = sCir.Replace("N", "Н");
                        sCir = sCir.Replace("H", "Х");
                        sCir = sCir.Replace("P", "П");
                        sCir = sCir.Replace("R", "Р");
                        sCir = sCir.Replace("S", "С");
                        sCir = sCir.Replace("Ć", "Ћ");
                        sCir = sCir.Replace("U", "У");
                        sCir = sCir.Replace("F", "ф");
                        sCir = sCir.Replace("C", "Ц");
                        sCir = sCir.Replace("Š", "Ш");
                        sCir = sCir.Replace("B", "Б");
                        sCir = sCir.Replace("V", "В");
                        sCir = sCir.Replace("G", "Г");
                        sCir = sCir.Replace("P", "П");
                        sCir = sCir.Replace("I", "И");
                        sCir = sCir.Replace("Č", "Ч");
                        sCir = sCir.Replace("Dž", "Џ");
                        sCirZ = sCirZ + sCir;
                    }
                    str = sCirZ;
                    return str;
                }
            }
            return str;
        }
        private string VratiLatinicu(string inputLat)

        {
            string str;
            str = inputLat;
            string word;
            word = inputLat;
            word = inputLat.Replace("њ", "nj");
            word = inputLat.Replace("љ", "lj");
            word = inputLat.Replace("џ", "dž");
            word = inputLat.Replace("ђ", "dj");
            word = inputLat.Replace("ђ", "đ");
            word = inputLat.Replace("Њ", "Nj");
            word = inputLat.Replace("Љ", "Lj");
            word = inputLat.Replace("Џ", "Dž");
            word = inputLat.Replace("Ђ", "Dj");
            word = inputLat.Replace("Ђ", "DJ");
            word = inputLat.Replace("Ђ", "Đ");
            word = inputLat.Replace("Љ", "Lj");
            word = inputLat.Replace("Џ", "DŽ");
            word = inputLat.Replace("Љ", "LJ");
            word = inputLat.Replace("д", "d");
            word = inputLat.Replace("ž", "ж");
            word = inputLat.Replace("з", "z");
            word = inputLat.Replace("л", "l");
            word = inputLat.Replace("н", "n");
            word = inputLat.Replace("х", "h");
            word = inputLat.Replace("п", "p");
            word = inputLat.Replace("р", "r");
            word = inputLat.Replace("с", "s");
            word = inputLat.Replace("ћ", "ć");
            word = inputLat.Replace("у", "u");
            word = inputLat.Replace("ф", "f");
            word = inputLat.Replace("ц", "c");
            word = inputLat.Replace("ш", "š");
            word = inputLat.Replace("б", "b");
            word = inputLat.Replace("в", "v");
            word = inputLat.Replace("г", "g");
            word = inputLat.Replace("а", "a");
            word = inputLat.Replace("е", "e");
            word = inputLat.Replace("к", "k");
            word = inputLat.Replace("м", "m");
            word = inputLat.Replace("и", "i");
            word = inputLat.Replace("ч", "č");
            word = inputLat.Replace("т", "t");


            word = inputLat.Replace("Д", "D");
            word = inputLat.Replace("Ж", "Ž");
            word = inputLat.Replace("З", "Z");
            word = inputLat.Replace("Л", "L");
            word = inputLat.Replace("Н", "N");
            word = inputLat.Replace("Х", "H");
            word = inputLat.Replace("П", "P");
            word = inputLat.Replace("Р", "R");
            word = inputLat.Replace("С", "S");
            word = inputLat.Replace("Ћ", "Ć");
            word = inputLat.Replace("У", "U");
            word = inputLat.Replace("ф", "F");
            word = inputLat.Replace("C", "Ц");
            word = inputLat.Replace("Ш", "Š");
            word = inputLat.Replace("Б", "B");
            word = inputLat.Replace("В", "V");
            word = inputLat.Replace("Г", "G");
            word = inputLat.Replace("П", "P");
            word = inputLat.Replace("И", "I");
            word = inputLat.Replace("Ч", "Č");
            word = inputLat.Replace("Џ", "Dž");
            return word;
        }
        private void Menuitem_Click(object sender, EventArgs e)
        {

            ToolStripMenuItem item = sender as ToolStripMenuItem;
            string ss = sender.ToString();
            int imgindex = item.ImageIndex;
            if (imgindex == 0) { return; }

            string nn = item.OwnerItem.OwnerItem.Name;// item.OwnerItem.Name;

            if (imgindex > 0)
            {

                if (windowsMenu.DropDownItems.Count >= 7)
                {
                    string aaa = "";
                    for (int n = 6; n < windowsMenu.DropDownItems.Count; n++)
                    {
                        int x = windowsMenu.DropDownItems[n].ToString().IndexOf(" ");
                        if (x > -1) { aaa = windowsMenu.DropDownItems[n].ToString().Substring(x).Trim(); };
                        if (ss == aaa)
                        {
                            //MessageBox.Show("Vec postoji");
                            //return;
                        }
                    }
                }

                toolStrip1.Visible = true;

                ToolStripLabel itemn = new ToolStripLabel();
                ToolStripButton itemB = new ToolStripButton();
                ToolStripSeparator itemnsep = new ToolStripSeparator();
             
                itemn.Text = item.Name;
                itemn.Name = item.Name;
                itemB.Image = global::Bankom.Properties.Resources.del12;

                itemnsep.Name = "sep" + item.Name;
                itemn.Click += new EventHandler(itemn_click);

                itemB.Click += new EventHandler(itemB_click);
                itemB.Name = item.Name;

                toolStrip1.Items.Add(itemn);
                toolStrip1.Items.Add(itemB);
                toolStrip1.Items.Add(itemnsep);
                //ivana 26.1.2021.
                sirina += toolStrip1.Items["itemn"].Width;
            }
        }

        public void itemn_click(object sender, EventArgs e) // aktiviranje forme klikom na tab
        {
            toolStripTextBox1.Text = "";
            string b = sender.ToString();
          
            frmChield active = new frmChield();
            active.AutoScroll = true;
            active.FormBorderStyle = FormBorderStyle.None;
            int a = toolStrip1.Items.Count;
            int c = 0;
            for (int i = 0; i < a; i++)
            {
                toolStrip1.Items[i].Font = new System.Drawing.Font("TimesRoman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                if (toolStrip1.Items[i].Name == b)
                {
                    toolStrip1.Items[i].Font = new System.Drawing.Font("TimesRoman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                if (toolStrip1.Items[i] == toolStrip1.Items["itemn"])
                    c++;
            }
            //ivana 26.1.2021.
            //int sirinaforme = this.Width * 2;
            //if (sirina > sirinaforme)
            //    for(int i = 0; i < a; i++)
            //        toolStrip1.Items[i].Size = new Size(sirinaforme / a, 0);
            //for (int x = 0; x < toolStrip1.Items.Count; x++)
            //{   
            int n = MdiChildren.Count<Form>();
            foreach (Form childForm in MdiChildren)
            {
                if (childForm.Name.ToUpper() == b.ToUpper())
                {
                    childForm.FormBorderStyle = FormBorderStyle.None;
                    childForm.BackColor = System.Drawing.Color.Snow;
                    //ivana 27.1.2021.
                    ActivateMdiChild(childForm);
                    childForm.Focus();

                    childForm.LayoutMdi(MdiLayout.TileVertical);
                    childForm.WindowState = FormWindowState.Maximized;
                    break;
                }
            }
            //}
            SrediFormu();
            //10.01.21 BORKA UMRTVILA  JER ZELIM DA SE KOD AKTIVACIJE FORME DOBIJEM IZGLED KAKAV JE BIO 
            //KAD SAM FORMU NAPUSTILA NPR AKO SAM  SORTIRALA DOKUMENTA  DA OSTANU SORTIRANA
            //zajedno 30.12.2020.
            //if (!b.Contains("print")) 
            //{
            //    //jovana 24.12.2020.
            //    clsRefreshForm rf = new clsRefreshForm();
            //    rf.refreshform();
            //}
        }

        public void itemB1_click(string imetula)  // zahtev za zatvaranje  forme klikom na tipku izlaz
        {
           toolStripTextBox1.Text = "";
            for (int j = 0; j < toolStrip1.Items.Count; j++)
            {
                if (this.MdiChildren[j].Name == imetula)
                {
                    this.ActivateMdiChild(this.MdiChildren[j]);
                    break;
                }
            }

            Form childForm1 = ActiveMdiChild;
            childForm1.Visible = false;
            childForm1.Dispose();

            for (int x = 0; x < toolStrip1.Items.Count; x++)
            {
                if (imetula == toolStrip1.Items[x].Name)
                {
                    toolStrip1.Items.Remove(toolStrip1.Items[x]); // button
                    toolStrip1.Items.Remove(toolStrip1.Items[x]); // label
                    toolStrip1.Items.Remove(toolStrip1.Items[x]); //image                    
                    break;
                }
            }
            
            
            if (toolStrip1.Items.Count == 0)
            {
                toolStrip1.Visible = false;
            }            

            SrediFormu();
        }
        public void itemB_click(object sender, EventArgs e)  // zahtev za zatvaranje forme klikom na tab
        {
            toolStripTextBox1.Text = "";
            ToolStripButton tb = sender as ToolStripButton;
            string b = tb.Name;
            if (b == "Imenik")
            {
                Iimenik.Enabled = true;
            }
            toolStripTextBox1.Text = "";
            for (int j = 0; j < toolStrip1.Items.Count; j++)
            {
                if (this.MdiChildren[j].Name == b)
                {
                    this.ActivateMdiChild(this.MdiChildren[j]);
                    break;
                }
            }

            Form childForm1 = ActiveMdiChild;
            childForm1.Visible = false;
            childForm1.Dispose();

            for (int x = 0; x < toolStrip1.Items.Count; x++)
            {
                if (b == toolStrip1.Items[x].Name)
                {
                    toolStrip1.Items.Remove(toolStrip1.Items[x]); // button
                    toolStrip1.Items.Remove(toolStrip1.Items[x]); // label
                    toolStrip1.Items.Remove(toolStrip1.Items[x]); //image                    
                    break;
                }
            }
            if (toolStrip1.Items.Count == 0)
            {
                toolStrip1.Visible = false;
            }
            SrediFormu();
        }

        private void toolStripLogin_Click(object sender, EventArgs e)
        {
            toolStrip1.Items.Clear();
            toolStrip1.Visible = false;
            Program.IntLogovanje = -1;
            foreach (Form ChildForm in this.MdiChildren)
            {
                ChildForm.Close();
            }

            LoginForm f = new LoginForm();
            f.MdiParent = this;
            f.Text = "Prijava";
            f.Show();
        }

        private void KrajRada_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Izlaz iz programa ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }

        }

        private void SrpskiCirilica_Click(object sender, EventArgs e)
        {
            
        }

        public BankomMDI(string connectionString,
            IContainer components,
            MenuStrip menuStrip,
            ToolStrip toolStrip,
            StatusStrip statusStrip,
  
            ToolStripMenuItem printSetupToolStripMenuItem,
         
            ToolStripStatusLabel toolStripStatusLabel,
            ToolStripMenuItem tileHorizontalToolStripMenuItem,
            ToolStripMenuItem fileMenu,
            ToolStripMenuItem newToolStripMenuItem,
            ToolStripMenuItem openToolStripMenuItem,
            ToolStripMenuItem saveToolStripMenuItem,
            ToolStripMenuItem saveAsToolStripMenuItem,
            ToolStripMenuItem printToolStripMenuItem,
            ToolStripMenuItem printPreviewToolStripMenuItem,
            ToolStripMenuItem exitToolStripMenuItem,
            ToolStripMenuItem editMenu,
            ToolStripMenuItem undoToolStripMenuItem,
            ToolStripMenuItem redoToolStripMenuItem,
            ToolStripMenuItem cutToolStripMenuItem,
            ToolStripMenuItem copyToolStripMenuItem,
            ToolStripMenuItem pasteToolStripMenuItem,
            ToolStripMenuItem selectAllToolStripMenuItem,
            ToolStripMenuItem viewMenu,
            ToolStripMenuItem toolBarToolStripMenuItem,
            ToolStripMenuItem statusBarToolStripMenuItem,
            ToolStripMenuItem toolsMenu,
            ToolStripMenuItem optionsToolStripMenuItem,
            ToolStripMenuItem windowsMenu,
            ToolStripMenuItem cascadeToolStripMenuItem,
            ToolStripMenuItem tileVerticalToolStripMenuItem,
            ToolStripMenuItem closeAllToolStripMenuItem,
            ToolStripMenuItem arrangeIconsToolStripMenuItem,
            ToolTip toolTip, MenuStrip menuStrip1,
            ToolStripMenuItem dokumenta,
            ToolStripMenuItem osnovniSifarnici,
            ToolStripMenuItem artikli,
            ToolStripMenuItem komitenti,
            ToolStripMenuItem organizacionaStruktura,
            ToolStripMenuItem izvestaji, ToolStripMenuItem menadzment,
            ToolStripMenuItem gotovinaISredstva,
            ToolStripMenuItem brutoBilans,
            ToolStripMenuItem saldoPoDospecima,
            ToolStripMenuItem dospecaObavezaIPotrazivanja,
            ToolStripMenuItem finansijskaKartica,
            ToolStripMenuItem dnevniBilansUspeha,
            ToolStripMenuItem troskoviZaposlenih,
       
            ToolStripMenuItem maloprodaja,
            ToolStripMenuItem stampaCenovnikaRecepcije,
            ToolStripMenuItem stampaCenovnika,
            ToolStripMenuItem dnevnikRada,
            ToolStripMenuItem prometMaloprodajePoNacinuPlacanja,
            ToolStripMenuItem prometRecepcijePoNacinuPlacanja,
            ToolStripMenuItem prometrecepcijepovrstamausluga,
            ToolStripMenuItem nabavkaZaKuhinjuNevski,
      
            ToolStripMenuItem pregledRazduzenjaPoVrednosti,
            ToolStripMenuItem pregledRazduzenjaPoProizvodima,
          
            ToolStripMenuItem zaRobuIUsluge,
            ToolStripMenuItem realizacijaUgovoraOKupoprodajiToolStripMenuItem,
            ToolStripMenuItem realizacijaOtkupaToolStripMenuItem,
            ToolStripMenuItem pregledInternihNalogaToolStripMenuItem,
            ToolStripMenuItem zavisniTroskoviNabavkeToolStripMenuItem,
     
            ToolStripMenuItem razlikaUCeniPoArtiklimaToolStripMenuItem,
            ToolStripMenuItem razlikaUCeniPoArtiklimaanalitikaToolStripMenuItem,
            ToolStripMenuItem razlikaUCeniPoArtiklimaIKupcimaToolStripMenuItem,
          
            ToolStripMenuItem zaKupceIDobavljace,
            ToolStripMenuItem spisakNakogaZaPrijemIUtovar,
            ToolStripMenuItem rekapitulacijaNarudzbenicaKupaca,
            ToolStripMenuItem rekapitulacijaNalogaZaSkladiste,
            ToolStripMenuItem prihvacenostPonuda,
            ToolStripMenuItem izgubljeniKupci,
            ToolStripMenuItem planVoznje,
            ToolStripMenuItem mesecniPregledRealizacije,
            ToolStripMenuItem prometKomercijalistaPoMesecima,
            ToolStripMenuItem uporedniPrometKomercijalista,
            ToolStripMenuItem razlikaUCeniPoKomercijalistima,
            ToolStripMenuItem naplataPoKomercijalistima,
            ToolStripMenuItem razlikaUCeniPoKupcima,
            ToolStripMenuItem razlikaUCeniPoKupcimaanalitika,
            ToolStripMenuItem analizaRazlikeUCeniPoKupcima,
            ToolStripMenuItem mesecniPotencijalKupaca,
  
            ToolStripMenuItem ukupnaNabavkaPoDobavljacima,
            ToolStripMenuItem analizaPrometaPoDobavljacu,
            ToolStripMenuItem ucesceUUkupnojNabavci,
            ToolStripMenuItem analitikaNabavkePoDobavljacima,
            ToolStripMenuItem razlikeUCeniSaAnalitikama,
        
            ToolStripMenuItem knjigovodstvo,
            ToolStripMenuItem obracunPoreza,
            ToolStripMenuItem stanjeSkladista,
            ToolStripMenuItem uporednoStanjeRobamagacin,
            ToolStripMenuItem magacinskoStanjeSkladista,
            ToolStripMenuItem robnaKartica,
            ToolStripMenuItem robnaKarticaMagacinska,
            ToolStripMenuItem trgovackaKnjiga,
            ToolStripMenuItem prometTranzita,
            ToolStripMenuItem kEPU,
            ToolStripMenuItem kalkulacijaIzlazaUzPredracunPonudu,
            ToolStripMenuItem kalkulacijaIzlaza,
            ToolStripMenuItem iOS,
            ToolStripMenuItem spisakIOSa,
            ToolStripMenuItem dnevniBilansUspehaMenuItem1,
            ToolStripMenuItem portfolioKomitenta,
            ToolStripMenuItem brutoBilansMenuItem1,
            ToolStripMenuItem brutoBilansWeb,
      
            ToolStripMenuItem nestimajuciNalozi,
            ToolStripMenuItem nestimajiciNaloziAnaliticki,
            ToolStripMenuItem finansijskaKarticaToolStripMenuItem1,
            ToolStripMenuItem finansijskaKarticaUDinarimaToolStripMenuItem,
            ToolStripMenuItem razlikaUCeniSaAnalitikamaToolStripMenuItem,

            ToolStripMenuItem popisnaListaToolStripMenuItem,

            ToolStripMenuItem finansije, ToolStripMenuItem zurnalKnjizenja,
            ToolStripMenuItem zurnalKnjizenjaTroskova,
            ToolStripMenuItem spisakPromenaZaKomitentaIOdabraniIntervalVremena,
            ToolStripMenuItem dnevniIzvestaj,
            ToolStripMenuItem stanjeGotovineIVisokoLikvidnihSredstava,
    
            ToolStripMenuItem dospecaObavezaIPotrazivanjaMenuItem1,
            ToolStripMenuItem dugovanjaIAvansi,
            ToolStripMenuItem potrazivanjaIAvansi,
            ToolStripMenuItem kompenzacija,
            ToolStripMenuItem saldoPoDospecimaMenuItem1,
            ToolStripMenuItem dospeceMenica,
            ToolStripMenuItem dnevniBilansUspehaMenuItem2,
            ToolStripMenuItem zastarelaPotrazivanja,
            ToolStripMenuItem prometGKKursiranUEure,
      
            ToolStripMenuItem spoljnaTrgovina,
            ToolStripMenuItem kontrolnikUvoza,
            ToolStripMenuItem kontrolnikIzvoza,
            ToolStripMenuItem izjavaUzRaspored,
            ToolStripMenuItem izjavaUzNalog,
            ToolStripMenuItem prodajaNaInoTrzistu,
            ToolStripMenuItem nabavkaNaInoTrzistu,
            ToolStripMenuItem komercijala,
            ToolStripMenuItem kartonKupca,
            ToolStripMenuItem planProdaje,
            ToolStripMenuItem realizacijaProdaje,
            ToolStripMenuItem proizvodnja,
            ToolStripMenuItem prometProizvodnje,
            ToolStripMenuItem prodajaFSHProizvodaPoKupcima,
            ToolStripMenuItem rUCOdProdajeVlastitihProizvoda,
            ToolStripMenuItem proizvodi,
            ToolStripMenuItem sirovineZaProizvodnju,
     
            ToolStripMenuItem osnovnaSredstva,
            ToolStripMenuItem karticaOsnovnihSredstavaIOpreme,
            ToolStripMenuItem stanjeOsnovnihSredstavaIOpreme,
            ToolStripMenuItem pregledUlaganja,
            ToolStripMenuItem pomocniSifarnici,
            ToolStripMenuItem dozvoleToolStripMenuItem,
            ToolStripMenuItem raznoMemuItem,
            ToolStripMenuItem peeisiToolStripMenuItem,
            ToolStripMenuItem preuzimanjeRateKredita,
            ToolStripMenuItem preuzimanjeManjkovaIViskova,
            ToolStripMenuItem preuzimanjeUplataKupacaIzBanaka,
            ToolStripMenuItem prenosNalogaNaPlacanje,
            ToolStripMenuItem preuzimanjeIzvodaIzBanaka,
            ToolStripMenuItem prepisNaplataIPlacanjaUIzvod,
            ToolStripMenuItem formiranjePPPPDZaPlate,
            ToolStripMenuItem uvozPlataUPlacanja,
            ToolStripMenuItem formiranjePPPPDZaPrevoz,
            ToolStripMenuItem uvozPrevozaUPlacanja,
           
            ToolStripMenuItem prenosiZaProdajnaMjesta,
            ToolStripMenuItem faktureRecepcijeZaOdabraneDatume,
            ToolStripMenuItem faktureRestoranaZaOdabraneDatume,
         
            ToolStripMenuItem razduzenjesirovinaminibar,
            ToolStripMenuItem razduzenjesirovinazaodabraniintervaldatuma,
            ToolStripMenuItem godisnjeObradeToolStripMenuItem,
            ToolStripMenuItem zatvaranjeStanjaPoLotu,
            ToolStripMenuItem pocetakGodine,
            ToolStripMenuItem usaglasavanjeRobeIFinansija,
            ToolStripMenuItem kursnaListaZaCeluGodinu,
            ToolStripMenuItem popunjavanjeTabeleDatuma,
            ToolStripMenuItem pocesiranjeToolStripMenuItem,
            ToolStripMenuItem procesirajeDnevnogiIzvestaja,
            ToolStripMenuItem procesiranjeBrutoBilansa,
            ToolStripMenuItem izvestajiIzStabla,
        
            ToolStripLabel toolStripLogin,
            ToolStripLabel KrajRada,
            ToolStripMenuItem izborJezika,
            ToolStripMenuItem srpskiCirilica,
            ToolStripMenuItem srpskiLatinica,
            ToolStripMenuItem english,
            ToolStripMenuItem ruski,
            ToolStripMenuItem oK,
            ToolStripMenuItem dokument,
            ToolStripMenuItem imenik,
            ToolStripMenuItem grupisi,
            ToolStripMenuItem brisi,
            ToolStripMenuItem storno,
            ToolStripMenuItem izmena,
            ToolStripMenuItem pregled,
            ToolStripMenuItem unos,
            ToolStripMenuItem prekid,
     
            ToolStripMenuItem closeActive,
            ToolStripComboBox toolStripComboBox1,
            ToolStripTextBox toolStripTextBox2,
            ToolStripTextBox toolStripTextBox3,
            ToolStripMenuItem kalkulator,
      
            ToolStripButton printToolStripButton,
            ToolStripMenuItem toolStripMenuItem1,
            ToolStripMenuItem f1ToolStripMenuItem,
            ToolStripMenuItem spaceToolStripMenuItem,
       
            ToolStrip toolStrip1,
            ToolStrip toolStrip2,
            ToolStripTextBox toolStripPrazno,
            ToolStripTextBox toolStripBrDok,
            ToolStripTextBox toolStripDatum,
            ToolStripTextBox toolStripOpis,
            ToolStripTextBox toolStripRef,
            ToolStripTextBox toolStripLikvidatura,
            ToolStripTextBox toolStripStatus,
            ToolStripTextBox toolStripporez,
            ToolStripTextBox toolStripDatumRada
            )
        {
            this.connectionString = connectionString;
            //this.childFormNumber = childFormNumber;
            this.components = components;
            //this.toolStrip = toolStrip;

         
            this.tileHorizontalToolStripMenuItem = tileHorizontalToolStripMenuItem;
            this.fileMenu = fileMenu;
            this.newToolStripMenuItem = newToolStripMenuItem;
            this.openToolStripMenuItem = openToolStripMenuItem;
            this.saveToolStripMenuItem = saveToolStripMenuItem;
            this.saveAsToolStripMenuItem = saveAsToolStripMenuItem;
            this.printToolStripMenuItem = printToolStripMenuItem;
            this.printPreviewToolStripMenuItem = printPreviewToolStripMenuItem;
            this.exitToolStripMenuItem = exitToolStripMenuItem;
            this.viewMenu = viewMenu;

            this.statusBarToolStripMenuItem = statusBarToolStripMenuItem;
            this.toolsMenu = toolsMenu;
            this.windowsMenu = windowsMenu;
            this.cascadeToolStripMenuItem = cascadeToolStripMenuItem;
            this.tileVerticalToolStripMenuItem = tileVerticalToolStripMenuItem;
            this.closeAllToolStripMenuItem = closeAllToolStripMenuItem;
            this.arrangeIconsToolStripMenuItem = arrangeIconsToolStripMenuItem;
            this.toolTip = toolTip;
            //this.menuStrip1 = menuStrip1;
            Dokumenta = dokumenta;
            OsnovniSifarnici = osnovniSifarnici;

            PomocniSifarnici = pomocniSifarnici;
            DozvoleToolStripMenuItem = dozvoleToolStripMenuItem;
            Sort = raznoMemuItem;
            PeeisiToolStripMenuItem = peeisiToolStripMenuItem;
            PreuzimanjeRateKredita = preuzimanjeRateKredita;
            PreuzimanjeManjkovaIViskova = preuzimanjeManjkovaIViskova;
            PreuzimanjeUplataKupacaIzBanaka = preuzimanjeUplataKupacaIzBanaka;
            PrenosNalogaNaPlacanje = prenosNalogaNaPlacanje;
  
            PreuzimanjeIzvodaIzBanaka = preuzimanjeIzvodaIzBanaka;
            PrepisNaplataIPlacanjaUIzvod = prepisNaplataIPlacanjaUIzvod;
            FormiranjePPPPDZaPlate = formiranjePPPPDZaPlate;
            UvozPlataUPlacanja = uvozPlataUPlacanja;
            FormiranjePPPPDZaPrevoz = formiranjePPPPDZaPrevoz;
            UvozPrevozaUPlacanja = uvozPrevozaUPlacanja;
           
            PrenosiZaProdajnaMjesta = prenosiZaProdajnaMjesta;
            FaktureRecepcijeZaOdabraneDatume = faktureRecepcijeZaOdabraneDatume;
            FaktureRestoranaZaOdabraneDatume = faktureRestoranaZaOdabraneDatume;
        
            Razduzenjesirovinaminibar = razduzenjesirovinaminibar;
            Razduzenjesirovinazaodabraniintervaldatuma = razduzenjesirovinazaodabraniintervaldatuma;
            GodisnjeObradeToolStripMenuItem = godisnjeObradeToolStripMenuItem;
            ZatvaranjeStanjaPoLotu = zatvaranjeStanjaPoLotu;
            PocetakGodine = pocetakGodine;
            UsaglasavanjeRobeIFinansija = usaglasavanjeRobeIFinansija;
            KursnaListaZaCeluGodinu = kursnaListaZaCeluGodinu;
            PopunjavanjeTabeleDatuma = popunjavanjeTabeleDatuma;
            PocesiranjeToolStripMenuItem = pocesiranjeToolStripMenuItem;
            ProcesirajeDnevnogiIzvestaja = procesirajeDnevnogiIzvestaja;
            ProcesiranjeBrutoBilansa = procesiranjeBrutoBilansa;
            IzvestajiIzStabla = izvestajiIzStabla;
            // this.toolStripSeparator2 = toolStripSeparator2;
            //this.toolStripLogin = toolStripLogin;
            this.KrajRada = KrajRada;
            IzborJezika = izborJezika;
            SrpskiCirilica = srpskiCirilica;
            SrpskiLatinica = srpskiLatinica;
            English = english;
            Ruski = ruski;
  
            CloseActive = closeActive;
            // this.tstbPretraga = tstbPretraga;

            //this.toolStripTextBox3 = toolStripTextBox3;


      
            this.toolStripMenuItem1 = toolStripMenuItem1;
            this.f1ToolStripMenuItem = f1ToolStripMenuItem;
            this.spaceToolStripMenuItem = spaceToolStripMenuItem;
        
            this.toolStrip1 = toolStrip1;

        }

        public BankomMDI(BankomMDI mdiParent)
        {
            MdiParent = mdiParent;
        }


        public void BrziPristup(object sender)
        {
            DataBaseBroker db2 = new DataBaseBroker();
            ToolStripTextBox item = sender as ToolStripTextBox;


            //foreach (Form childForm in MdiChildren)
            //{
            //    string ii = childForm.Text;
            //    if (ii == "Dokumenta")
            //    {
            //        if (childForm.Controls["tv"] != null)
            //        {


            //        }

            //    }
            //}

            if (Program.intAkcija > 0)
            {
                MessageBox.Show("Nije izabran 'Pregled'");
                return;
            }


            string ss = item.Text.Trim();
            if (ss == "") { return; }
            if (ss == "System.Windows.Forms.ToolStripTextBox") { return; }

            string aaa = "";
            for (int n = 6; n < windowsMenu.DropDownItems.Count; n++)
            {
                int x = windowsMenu.DropDownItems[n].ToString().IndexOf(" ");
                if (x > -1) { aaa = windowsMenu.DropDownItems[n].ToString().Substring(x).Trim(); };
                if (ss == aaa)
                {
                    //MessageBox.Show("Vec postoji");
                    //toolStripTextBox1.Text = "";

                    //return;
                }
            }


            string strSender = "";
            string str = ss;
            int idstablo = 0;
            string naziv = "";

            str = "select ID_DokumentaStablo,NazivJavni,Naziv from DokumentaStablo where NazivJavni = '" + item + "'";
            DataTable tt = new DataTable();
            tt = db2.ReturnDataTable(str);

            if (tt.Rows.Count > 0)
            {
                idstablo = Convert.ToInt32(tt.Rows[0]["ID_DokumentaStablo"]);
                naziv = tt.Rows[0]["Naziv"].ToString();
            }
            else
            {
                MessageBox.Show("Dokumenat nije pronadjen");
                // toolStripTextBox1.Text = "";
                return;
            }


            if (windowsMenu.DropDownItems.Count >= 8)
            {
                string wa = "";
                for (int n = 7; n < windowsMenu.DropDownItems.Count; n++)
                {
                    int x = windowsMenu.DropDownItems[n].ToString().IndexOf(" ");
                    if (x > -1) { wa = windowsMenu.DropDownItems[n].ToString().Substring(x).Trim(); };
                    if (strSender == wa.Replace(" ", ""))
                    {
                        MessageBox.Show("Dokumenat je vec otvoren");
                        this.Text = "";
                        // toolStripTextBox1.Text = " ";

                        return;
                    }
                }
            }

            ShowNewForm("Dokumenta", idstablo, naziv, 1, "", "", "S", "", ""); // na dogadjaju form load otvara se nova forma  sa predatim parametrima 
        }

        //tamara 23.10.2020.




        private void CloseActive_Click(object sender, EventArgs e)
        {
            Form childForm = ActiveMdiChild;
            string b = childForm.Name;


            if (childForm != null) { childForm.Close(); }
            for (int x = 0; x < toolStrip1.Items.Count; x++)
            {

                if (b == toolStrip1.Items[x].Name)
                {
                    toolStrip1.Items.Remove(toolStrip1.Items[x]);
                    toolStrip1.Items.Remove(toolStrip1.Items[x]);
                    toolStrip1.Items.Remove(toolStrip1.Items[x]);
                    break;
                }
            }
            if (toolStrip1.Items.Count == 0) { toolStrip1.Visible = false; }

        }

        private void SrpskiLatinica_Click(object sender, EventArgs e)
        {
            Program.ID_Jezik = 3;
            IzborJezika.Text = "Srpski-Latinica";
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void windowsMenu_Click(object sender, EventArgs e)
        {

        }

        private void pretragaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmChield activeChild = (frmChield)this.ActiveMdiChild;
            activeChild.FormBorderStyle= FormBorderStyle.None;
            activeChild.BackColor = System.Drawing.Color.Snow;
            DialogResult res = MsgBox.ShowDialog("Tekst pretrage:", "Pretraga", ((Bankom.frmChield)activeChild).toolStripTextFind.Text,
            MsgBox.Icon.Question,
            MsgBox.Buttons.OkCancel,
            MsgBox.Type.TextBox,
            MsgBox.Type.ComboBox, new string[] { "", "BrDok", "Datum", "Opis", "Ref.", "Likvidatura", "Status", "Porez", "DatumRada" }, true,
            new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold));

            if (res == System.Windows.Forms.DialogResult.OK || res == System.Windows.Forms.DialogResult.Yes)
            {
                string strKolona = MsgBox.ResultValue;
                string strfind = MsgBox.ResultFind;

                if (strfind == "Ponisti")
                {
                    strfind = "";
                    ((Bankom.frmChield)activeChild).toolStripTextFind.Text = strfind;
                    clsdokumentRefresh docref1 = new clsdokumentRefresh();
                    string strstablo = ((Bankom.frmChield)activeChild).toolStripTextBroj.Text;
                    ((Bankom.frmChield)activeChild).ToolStripTextPos.Text = "1";


                    string str = "SELECT DISTINCT ID_Proknjizeno,ID_MesecPoreza,ID_Predhodni,ID_DokumentaStablo,IId,ID_KadrovskaEvidencija,ID_OrganizacionaStrukturaView,ID_LikvidacijaDokumenta, ID_DokumentaTotali  AS ID_GgRrDokumentaStavkeView,BrDok,Datum,Opis,Predhodni,LikvidacijaDokumenta,Proknjizeno,MesecPoreza,TTime,SifRadnika,NazivOrg,RB FROM DokumentaTotali  as s  WITH(NOLOCK)  WHERE s.ID_DokumentaStablo= " + strstablo + " and nazivorg in (Select nazivorg  from OrganizacionaStrukturaStavkeView where ID_OrganizacionaStrukturaStablo= 6)  order by  s.id_DokumentaTotali desc";
                    docref1.refreshDokumentGrid(this,"Dokumenta", "1", str, "1", "S");
                    DataBaseBroker db = new DataBaseBroker();
                    DataTable tbb = db.ReturnDataTable("select  Count(*)  from Dokumentatotali where ID_DokumentaStablo=" + strstablo);

                    if (tbb.Rows.Count > 0) { ((Bankom.frmChield)activeChild).ToolStripLblPos.Text = Convert.ToString(Convert.ToInt32(tbb.Rows[0][0]) / 25); }

                    return;
                }
                if (strfind == "" || strKolona == "") { return; }
                strfind = strfind.Replace("Likvidatura", "LikvidacijaDokumenta");
                strfind = strfind.Replace("Ref.", "Predhodni");
                strfind = strfind.Replace("Porez", "MesecPoreza");
                strfind = strfind.Replace("Status", "Proknjizeno");


                ((Bankom.frmChield)activeChild).toolStripTextFind.Text = strfind;
                string c = ((Bankom.frmChield)activeChild).toolStripTexIme.Text;
                string d = ((Bankom.frmChield)activeChild).toolStripTextBroj.Text;
                long f = ((Bankom.frmChield)activeChild).iddokumenta;
                string sprikaz = ((Bankom.frmChield)activeChild).VrstaPrikaza;
                string dokumentje = ((Bankom.frmChield)activeChild).DokumentJe;
                ((Bankom.frmChield)activeChild).ToolStripLblPos.Text = "1";
                ((Bankom.frmChield)activeChild).ToolStripTextPos.Text = "1";



                clsdokumentRefresh docref = new clsdokumentRefresh();
                docref.refreshDokumentGrid(this,"Dokumenta", "1", "SELECT DISTINCT ID_Proknjizeno, ID_MesecPoreza, ID_Predhodni, ID_DokumentaStablo, IId, ID_KadrovskaEvidencija, ID_OrganizacionaStrukturaView, ID_LikvidacijaDokumenta, ID_DokumentaTotali  AS ID_GgRrDokumentaStavkeView, BrDok, Datum, Opis, Predhodni, LikvidacijaDokumenta, Proknjizeno, MesecPoreza, TTime, SifRadnika, NazivOrg, RB FROM DokumentaTotali as s  WITH(NOLOCK)  WHERE s.ID_DokumentaStablo = " + Convert.ToString(d) + " and nazivorg in (Select nazivorg  from OrganizacionaStrukturaStavkeView where ID_OrganizacionaStrukturaStablo = 6)  order by  s.id_DokumentaTotali desc", "1", "S");


            }

        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // tamara 26.10.2020.

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripOpis_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripBrDok_Click(object sender, EventArgs e)
        {

        }

        private void PomocniSifarnici_Click(object sender, EventArgs e)

        {
            string b = "PomocniSifarnici";

            for (int x = 0; x < toolStrip1.Items.Count; x++)
            {
                if (b == toolStrip1.Items[x].Name)
                { 
                        MessageBox.Show("Vec postoji");
                        return;                }
            }
            ShowNewForm("PomocniSifarnici", 1, "PomocniSifarnici", 1, "", "", "P", "", "TreeView");

        }


        private void Artikli_Click(object sender, EventArgs e)
        {
            string b = "Artikli";

            for (int x = 0; x < toolStrip1.Items.Count; x++)
            {
                if (b == toolStrip1.Items[x].Name)
                {
                        MessageBox.Show("Vec postoji");
                        return;
                }
            }
            ShowNewForm("Artikli", 1, "Artikli", 1, "", "", "S", "", "TreeView");
        }

        private void IzvestajiIzStabla_Click(object sender, EventArgs e)
        {
            string b = "Izvestaji";

            for (int x = 0; x < toolStrip1.Items.Count; x++)
            {
                if (b == toolStrip1.Items[x].Name)
                {
                        MessageBox.Show("Vec postoji");
                        return;
                }
            }
            ShowNewForm("Izvestaj", 1, "Izvestaj", 1, "", "", "I", "", "TreeView");
        }

        private void OrgStr_Click(object sender, EventArgs e)
        {
            string b = "Organizacionastruktura";

            for (int x = 0; x < toolStrip1.Items.Count; x++)
            {
                if (b == toolStrip1.Items[x].Name)
                {
                        MessageBox.Show("Vec postoji");
                        return;
                }
            }

            ShowNewForm("Organizacionastruktura", 1, "Organizacionastruktura", 1, "", "", "S", "", "TreeView");
        }

        private void Komitenti_Click(object sender, EventArgs e)
        {
            string b = "Komitenti";

            for (int x = 0; x < toolStrip1.Items.Count; x++)
            {
                if (b == toolStrip1.Items[x].Name)
                {
                        MessageBox.Show("Vec postoji");
                        return;
                }
            }
            ShowNewForm("Komitenti", 1, "Komitenti", 1, "", "", "S", "", "TreeView");
        }

        private void editMenu_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            frmChield activeChild = (frmChield)this.ActiveMdiChild;
            activeChild.FormBorderStyle= FormBorderStyle.None;
            activeChild.BackColor = System.Drawing.Color.Snow;
            DajVrednostPropertija(activeChild);
        }

        //Djora 20.09.19
        private static void DajVrednostPropertija(Object obj)
        {
            long aa = ((Bankom.frmChield)obj).iddokumenta;
            int bb = ((Bankom.frmChield)obj).idstablo;
            string cc = ((Bankom.frmChield)obj).imedokumenta;
            MessageBox.Show(aa + "," + bb + "," + cc);



            Type t = obj.GetType();
            //Console.WriteLine("Type is: {0}", t.Name);
            PropertyInfo[] props = t.GetProperties();
            //Console.WriteLine("Properties (N = {0}):", props.Length);
            //    foreach (var prop in props)
            //        if (prop.GetIndexParameters().Length == 0)
            //            Console.WriteLine("   {0} ({1}): {2}", prop.Name,
            //                              prop.PropertyType.Name,
            //                              prop.GetValue(obj));
            //        else
            //            Console.WriteLine("   {0} ({1}): <Indexed>", prop.Name,
            //                              prop.PropertyType.Name);
            //}
            //MessageBox.Show(t.GetProperty("intdok").GetValue(obj).ToString());
        }

        private void toolStripMenuPRN_Click(object sender, EventArgs e)
        {
            string ime = "";
            long t = 0;
            foreach (Form childForm in MdiChildren)
            {
                if (childForm != this.ActiveMdiChild) childForm.WindowState = FormWindowState.Minimized;
            }

            foreach (Form childForm in MdiChildren)
            {
                if (childForm != this.ActiveMdiChild) childForm.WindowState = FormWindowState.Minimized;
            }

            Form activeChild = (frmChield)this.ActiveMdiChild;
            activeChild.FormBorderStyle= FormBorderStyle.None;
            activeChild.BackColor = System.Drawing.Color.Snow;
            if (activeChild != null)
            {

                t = ((Bankom.frmChield)activeChild).iddokumenta;

                activeChild.Select();
                ime = activeChild.Text;
            }

            int pozicija = ime.IndexOf("(");
            if (pozicija > -1) { ime = ime.Substring(0, pozicija).Trim(); }

            pozicija = ime.IndexOf(")");
            if (pozicija > -1) { ime = ime.Substring(0, pozicija).Trim(); }

            DataTable dt = new DataTable();

            string sql = "SELECT [Name]  FROM [ReportServer].[dbo].[Catalog]   where Name='prn" + ime + "' and Type=2";
            string connectionString = "Data Source=server;Initial Catalog=ReportServer;User ID=sa;password=password;";
            SqlConnection conn = new SqlConnection(connectionString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoji izveštaj!");
                return;
            }

            frmPrint fs = new frmPrint();
            fs.BackColor = System.Drawing.Color.Snow;
            fs.FormBorderStyle= FormBorderStyle.None;
            fs.MdiParent = this;
            fs.Text = "print - " + ime;
            fs.intCurrentdok = Convert.ToInt32(t);
            fs.LayoutMdi(MdiLayout.TileVertical);
            fs.imefajla = ime;
            fs.Show();

            toolStrip1.Visible = true;

            ToolStripLabel itemn = new ToolStripLabel();
            ToolStripButton itemB = new ToolStripButton();
            ToolStripSeparator itemnsep = new ToolStripSeparator();
            itemn.Text = "print - " + ime;
            itemn.Name = "print - " + ime;
            itemB.Image = global::Bankom.Properties.Resources.del12;
            itemnsep.Name = "sepprint - " + ime;
            itemn.Click += new EventHandler(itemn_click);

            itemB.Click += new EventHandler(itemB_click);
            itemB.Name = "print - " + ime;

            toolStrip1.Items.Add(itemn);
            toolStrip1.Items.Add(itemB);
            toolStrip1.Items.Add(itemnsep);
            LayoutMdi(MdiLayout.TileVertical);

        }

        private void toolStripMenuRPT_Click(object sender, EventArgs e)
        {
            string ime = "";
            string imeDokumenta = "";
            foreach (Form childForm in MdiChildren)
            {
                if (childForm != this.ActiveMdiChild) childForm.WindowState = FormWindowState.Minimized;
            }

            foreach (Form childForm in MdiChildren)
            {
                if (childForm != this.ActiveMdiChild) childForm.WindowState = FormWindowState.Minimized;
            }


            Form activeChild = (frmChield)this.ActiveMdiChild;
            activeChild.FormBorderStyle= FormBorderStyle.None;
            activeChild.BackColor = System.Drawing.Color.Snow;
            if (activeChild != null)
            {
                imeDokumenta = ((Bankom.frmChield)activeChild).imedokumenta;
                activeChild.Select();
                ime = activeChild.Text;
            }
            ime = imeDokumenta;
            int pozicija = ime.IndexOf("(");
            if (pozicija > -1) { ime = ime.Substring(0, pozicija).Trim(); }



            if (ime.Substring(0, 3) != "rpt")
            {
                ime = "rpt" + ime;

            }


            DataTable dt = new DataTable();

            string sql = "SELECT [Name]  FROM [ReportServer].[dbo].[Catalog]   where Name='" + ime + "' and Type=2";
            string connectionString = "Data Source=server;Initial Catalog=ReportServer;User ID=sa;password=password;";
            SqlConnection conn = new SqlConnection(connectionString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Ne postoji izveštaj!");
                return;
            }

            frmPrint fs = new frmPrint();
            fs.BackColor = System.Drawing.Color.Snow;
            fs.FormBorderStyle= FormBorderStyle.None;
            fs.MdiParent = this;
            fs.Text = "print - " + ime;
            fs.LayoutMdi(MdiLayout.TileVertical);
            fs.imefajla = ime;
            fs.Show();
            toolStrip1.Visible = true;

            ToolStripLabel itemn = new ToolStripLabel();
            ToolStripButton itemB = new ToolStripButton();
            ToolStripSeparator itemnsep = new ToolStripSeparator();
            itemn.Text = "print - " + ime;

            itemn.Name = "print - " + ime;
            itemB.Image = global::Bankom.Properties.Resources.del12;
            itemnsep.Name = "sepprint - " + ime;
            itemn.Click += new EventHandler(itemn_click);

            itemB.Click += new EventHandler(itemB_click);
            itemB.Name = "print - " + ime;

            toolStrip1.Items.Add(itemn);
            toolStrip1.Items.Add(itemB);
            toolStrip1.Items.Add(itemnsep);
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void toolStripMenuRefresh_Click(object sender, EventArgs e)
        {

            frmChield activeChild = (frmChield)this.ActiveMdiChild;
            activeChild.FormBorderStyle= FormBorderStyle.None;
            activeChild.BackColor = System.Drawing.Color.Snow;
            string c = ((Bankom.frmChield)activeChild).toolStripTexIme.Text;
            string d = ((Bankom.frmChield)activeChild).toolStripTextBroj.Text;
            long f = ((Bankom.frmChield)activeChild).iddokumenta;
            string sprikaz = ((Bankom.frmChield)activeChild).VrstaPrikaza;
            string dokumentje = ((Bankom.frmChield)activeChild).DokumentJe;
            ((Bankom.frmChield)activeChild).ToolStripLblPos.Text = "1";
            ((Bankom.frmChield)activeChild).ToolStripTextPos.Text = "1";
            activeChild.Close();
            ((Bankom.BankomMDI)this).ShowNewForm(c, Convert.ToInt32(d), c, f, "", "", dokumentje, "", sprikaz);

        }

        private void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolsMenu_Click(object sender, EventArgs e)
        {

        }

        private void DozvoleToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Program.Parent.ShowNewForm("", 1, "Dozvole", 1, "", "", "P", "", "");

        }

        private void toolStripSplitStampa_ButtonClick(object sender, EventArgs e)
        {

        }

        private void Grupisi_Click(object sender, EventArgs e)
        {

        }

        private void Imenik_Click(object sender, EventArgs e)
        {

        }

        private void Dokument_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // tamara 26.10.2020.


        }

     

        private void Uunos_Click(object sender, EventArgs e)
        {


            Form activeChild = this.ActiveMdiChild;
            activeChild.FormBorderStyle= FormBorderStyle.None;
            //13.01.2021. tamara lotovi
            if (activeChild.Text == "LOT")
            {
                //activeChild.Controls["OOperacija"].Text = "";
                NoviLot unosNovog = new NoviLot(Uunos);
                Uunos.Enabled = false;
                unosNovog.FormBorderStyle = FormBorderStyle.FixedSingle;
                unosNovog.Show();
                clsRefreshForm rf = new clsRefreshForm();
                rf.refreshform();
                activeChild.Controls["OOperacija"].Text = "";
            }
            else if (activeChild != null)
            {
                activeChild.Controls["OOperacija"].Text = "UNOS";
            }

        }
        
        private void PrenosiZaProdajnaMjesta_Click(object sender, EventArgs e)
        {
            Prenosi childForm = new Prenosi();

            childForm.MdiParent = this;

            // childForm.WindowState = FormWindowState.Maximized;
            childForm.Show();

        }

        private void PeeisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;
            if (activeChild != null)
            {
                // activeChild.Controls["OOperacija"].Text = "PRENOSI";
            }
        }

        private void Ppregled_Click(object sender, EventArgs e)
        {
            //Daj mi aktivnu child formu
            Form activeChild = this.ActiveMdiChild;
            
            //Popuni text u kontroli OOperacija sa "PREGLED" na aktivnoj child formi
            if (activeChild != null)
            {
                activeChild.FormBorderStyle = FormBorderStyle.None;
                if (((Bankom.frmChield)activeChild).panel1.Visible == true) ((Bankom.frmChield)activeChild).panel1.Visible = false;
                activeChild.Controls["OOperacija"].Text = "PREGLED";
                filter = "S";
                clsFormInitialisation fi = new clsFormInitialisation();
                fi.ObrisiZaglavljeIStavkePoljaZaUnos();
                fi.InitValues();

            }
            else
            {
               MessageBox.Show("Nemate aktivnu formu");
            }
        }

        private void Pprekid_Click(object sender, EventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;
            //13.01.2021. tamara
            if (activeChild.Text == "LOT")
            {

                foreach (var pb in activeChild.Controls.OfType<Field>())
                {
                    //if (pb.cTip == 10 || pb.cTip == 8)
                    //    pb.Enabled = false;
                    if (pb.cTip == 10 )
                    {
                        if (pb.VrstaKontrole == "combo") pb.comboBox.Enabled = false;
                        else pb.textBox.Enabled = false;
                    }
                    if (pb.cTip == 8) pb.dtp.Enabled = false;
                }
            }
            activeChild.Controls["OOperacija"].Text = "PREKID";
            if (activeChild != null)
            {
                
                activeChild.FormBorderStyle = FormBorderStyle.None;
                
                if (((Bankom.frmChield)activeChild).panel1.Visible == true) ((Bankom.frmChield)activeChild).panel1.Visible = false;
                clsFormInitialisation fi = new clsFormInitialisation();
                fi.ObrisiZaglavljeIStavkePoljaZaUnos();
                clsRefreshForm rf = new clsRefreshForm();
                rf.refreshform();
                activeChild.Controls["OOperacija"].Text = "";

            }
            else
            {
                MessageBox.Show("Nemate aktivnu formu");
            }

        }

        private void Iizmena_Click(object sender, EventArgs e)
        {
            //Daj mi aktivnu child formu
            Form activeChild = this.ActiveMdiChild;
            activeChild.FormBorderStyle = FormBorderStyle.None;
            //13.01.2021. tamara
            if (activeChild.Text == "LOT")
            {

                foreach (var pb in activeChild.Controls.OfType<Field>())
                {
                    //if (pb.cTip == 10 || pb.cTip == 8)
                    //    pb.Enabled = true;
                    if (pb.cTip == 10)
                    {
                        if (pb.VrstaKontrole == "combo") pb.comboBox.Enabled = true;
                        else pb.textBox.Enabled = true;
                    }
                    if (pb.cTip == 8) pb.dtp.Enabled = true;
                }
            }
            //Popuni text u kontroli OOperacija sa "IZMENA" na aktivnoj child formi
            if (activeChild != null)
            {
                activeChild.Controls["OOperacija"].Text = "IZMENA";
            }
            else
            {
                MessageBox.Show("Nemate aktivnu formu");
            }
        }

        private void Sstorno_Click(object sender, EventArgs e)
        {
            //Daj mi aktivnu child formu
            Form activeChild = this.ActiveMdiChild;
           
            //Popuni text u kontroli OOperacija sa "STORNO" na aktivnoj child formi
            if (activeChild == null)
            {
                MessageBox.Show("Nemate aktivnu formu");
            }
            else
            {
                activeChild.FormBorderStyle = FormBorderStyle.None;
                activeChild.Controls["OOperacija"].Text = "STORNO";
            }

        }

        private void toolStripSplitStampa_ButtonClick_1(object sender, EventArgs e)
        {

        }
        DataTable rezProvere = new DataTable();
        private void Sstampa_Click(object sender, EventArgs e)
        {
            Form Me = this.ActiveMdiChild;
            Me.FormBorderStyle = FormBorderStyle.None;
            Me.Focus();
            if (Me == null)
            {
                MessageBox.Show("Nemate aktivnu formu");

            }
            else
            {
                string ime = Me.Text;
                //string iddok = Me.Tag.ToString();


                ime = Me.Controls["limedok"].Text;
                string iddok = Me.Controls["liddok"].Text;

                //((Bankom.frmChield)forma).pparametri

                string naslov = "print - " + ime;
                Boolean odgovor = false;
                odgovor = DalijevecOtvoren("D", naslov, ime);
                if (odgovor == false) //nije otvoren
                {
                    frmPrint fs = new frmPrint();
                    fs.FormBorderStyle = FormBorderStyle.None;
                    fs.BackColor = System.Drawing.Color.Snow;
                    fs.MdiParent = this;
                    fs.Text = naslov;
                    fs.intCurrentdok = Convert.ToInt32(iddok); //id
                    fs.LayoutMdi(MdiLayout.TileVertical);
                    fs.imefajla = ime;
                    switch (Me.Controls["ldokje"].Text)
                    {
                        case "I":
                            fs.kojiprint = "rpt";
                            break;
                        case "D":
                            fs.kojiprint = "prn";
                            break;
                        case "S":
                        case "P":
                            fs.kojiprint = "sif";
                            break;
                        default:
                            fs.kojiprint = "prn";
                            break;
                    }
                    //zajedno 14.1.2021. provera da li dokument ima stampu
                    string upit1 = "Select Name from Catalog where Name=@param0";
                    rezProvere = db.ParamsQueryDT(upit1, fs.kojiprint + fs.imefajla);
                    if (rezProvere.Rows.Count == 0)
                        MessageBox.Show("Dokument nema štampu!");
                    else
                    {
                        fs.Show();
                        addFormTotoolstrip1(fs, ime);
                    }
                }
            }
        }



        private void Bbrisanje_Click(object sender, EventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;
            activeChild.Focus();
            //Popuni text u kontroli OOperacija sa "BRISANJE" na aktivnoj child formi
            if (activeChild == null)
            {
                MessageBox.Show("Nemate aktivnu formu");

            }
            else
            {
                activeChild.Controls["OOperacija"].Text = "BRISANJE";
            }
        }

        private void Iimenik_Click(object sender, EventArgs e)
        {

            frmImenik frmi = new frmImenik(Iimenik);
            Iimenik.Enabled = false;
            frmi.FormBorderStyle = FormBorderStyle.None;
            frmi.Text = "Imenik";
            frmi.MdiParent = this;
            frmi.Dock = DockStyle.Fill;
            if (this.IzborJezika.Text == "Српски-Ћирилица") { frmi.Text = this.VratiCirlilicu("Imenik"); }
            int sirina = (this.Width / 100) * 10;
            this.addFormTotoolstrip1(frmi, "Imenik");
            this.updateToolStrip("Imenik");
            frmi.StartPosition = FormStartPosition.CenterScreen;
            frmi.Show();
        }

        public static string cf;
        private void Iizlaz_Click(object sender, EventArgs e)
        {
            // ivana 24.11.2020.
            if (this.ActiveMdiChild != null)
            {
                Form childForm = this.ActiveMdiChild;
                itemB1_click(childForm.Name);
            }
            if (toolStrip1.Items.Count == 0)
            {
                toolStrip1.Visible = false;
                if (MessageBox.Show("Izlaz iz programa?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //toolStrip1.Visible = false;
                    Application.ExitThread();
                }

            }
        }
        private void unospb_Click(object sender, EventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;
            if (activeChild != null)
            {
                activeChild.Controls["OOperacija"].Text = "UNOS PODBROJA";
            }
        }

        private void windowsMenu_Click_1(object sender, EventArgs e)
        {

        }

        private void cascadeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void tileVerticalToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void tileHorizontalToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void arrangeIconsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseActive_Click_1(object sender, EventArgs e)
        {

            string b = "";
            if (this.MdiChildren.Count() > 1)
            {
                foreach (Form childForm in this.MdiChildren)
                {
                    if (childForm == this.ActiveMdiChild)
                    {
                        b = childForm.Name;
                        childForm.Close();
                        childForm.Dispose();
                        return;
                    }
                }
            }
            for (int x = 0; x < toolStrip1.Items.Count; x++)
            {

                if (b == toolStrip1.Items[x].Name)
                {
                    toolStrip1.Items.Remove(toolStrip1.Items[x]);
                    toolStrip1.Items.Remove(toolStrip1.Items[x]);
                    toolStrip1.Items.Remove(toolStrip1.Items[x]);
                }
            }
            if (toolStrip1.Items.Count == 0) { toolStrip1.Visible = false; }

        }


        private void CloseAllToolStripMenuItem_Click_1(object sender, EventArgs e)

        {


            for (int jj = 6; jj < windowsMenu.DropDownItems.Count; jj++)
            {

                windowsMenu.DropDownItems.RemoveAt(jj);

            }

            toolStrip1.Items.Clear();
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            toolStripTextBox1.Text = "";
            if (toolStrip1.Items.Count == 0) { toolStrip1.Visible = false; }
        }
        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)

        {


            foreach (Form frm in this.MdiChildren)
            {
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.BackColor = System.Drawing.Color.Snow;

                if (!frm.Focused)
                {
                    frm.Visible = false;
                    frm.Dispose();
                }

            }
            toolStrip1.Items.Clear();

            toolStripTextBox1.Text = "";
            if (toolStrip1.Items.Count == 0) { toolStrip1.Visible = false; }

        }
        private void SrpskiCirilica_Click_1(object sender, EventArgs e)
        {
            Program.ID_Jezik = 4;
            IzborJezika.Text = "Српски-Ћирилица";            
        }

        private void statusBarToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strFileName = "";


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Excel Files (*.xls)|*.xls;*.xlsx|Word Files (*.doc;*.docx;*.dot)|*.doc;*.docx;*.dot|Pdf Files (*.pdf)|*.pdf|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {

                strFileName = openFileDialog.FileName;
                string folderPath = openFileDialog.InitialDirectory;
                this.Text = openFileDialog.SafeFileName;
                switch (openFileDialog.FilterIndex)
                {
                    case 1:

                        var excelApp = new Excel.Application();
                        excelApp.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlNormal;
                        //excelApp.Top = Top + 108; ;
                        // excelApp.Width = Width-70;
                        string ime = excelApp.Name;
                        //excelApp.Height = Height-70;
                        excelApp.Visible = true;
                        Excel.Workbooks books = excelApp.Workbooks;
                        //excelApp.Width = Width;
                        //excelApp.Height = Height;

                        excelApp.Workbooks.Open(strFileName);

                        break;
                    case 2:
                        Microsoft.Office.Interop.Word.Application wdApp = new Microsoft.Office.Interop.Word.Application();
                        wdApp.Visible = true;
                        wdApp.WindowState = Word.WdWindowState.wdWindowStateNormal;
                        Word.Document aDoc = wdApp.Documents.Open(strFileName);


                        break;
                    case 3:
                        WebBrowser wb = new WebBrowser();
                        wb.Top = Top - 40;
                        wb.Navigate(strFileName);

                        break;
                    case 4:
                        //System.Diagnostics.Process.Start(strFileName);
                        ProcessStartInfo startInfo = new ProcessStartInfo("notepad.exe");
                        startInfo.WindowStyle = ProcessWindowStyle.Maximized;
                        Process.Start(strFileName);
                        break;
                }
            }
        }
        //4.12.2020.
        //private void toolStripKrajRada_Click(object sender, EventArgs e)
        //{
        //    if (MessageBox.Show("Izlaz iz programa ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //    {
        //        Application.Exit();
        //    }
        //}

        //private void toolStripLogin_Click_1(object sender, EventArgs e)
        //{

        //}

        private void Ddokum_Click(object sender, EventArgs e)
        {
            ShowNewForm(" ", 1, "SpisakDokumenata", 1, "", "", "I", "", "");
        }

        private void Kalkulator_Click_1(object sender, EventArgs e)
        {
            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = "calc.exe";
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.Start();
        }

        private void toolsMenu_Click_1(object sender, EventArgs e)
        {

        }

        private void pretragaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            frmChield activeChild = (frmChield)this.ActiveMdiChild;
            activeChild.FormBorderStyle = FormBorderStyle.None;
            activeChild.BackColor = System.Drawing.Color.Snow;

            DialogResult res = MsgBox.ShowDialog("Tekst pretrage:", "Pretraga", ((Bankom.frmChield)activeChild).toolStripTextFind.Text,
            MsgBox.Icon.Question,
            MsgBox.Buttons.OkCancel,
            MsgBox.Type.TextBox,
            MsgBox.Type.ComboBox, new string[] { "", "BrDok", "Datum", "Opis", "Ref.", "Likvidatura", "Status", "Porez", "DatumRada" }, true,
            new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold));

            if (res == System.Windows.Forms.DialogResult.OK || res == System.Windows.Forms.DialogResult.Yes)
            {
                string strKolona = MsgBox.ResultValue;
                string strfind = MsgBox.ResultFind;

                if (strfind == "Ponisti")
                {
                    strfind = "";
                    ((Bankom.frmChield)activeChild).toolStripTextFind.Text = strfind;
                    clsdokumentRefresh docref1 = new clsdokumentRefresh();
                    string strstablo = ((Bankom.frmChield)activeChild).toolStripTextBroj.Text;
                    ((Bankom.frmChield)activeChild).ToolStripTextPos.Text = "1";


                    string str = "SELECT DISTINCT ID_Proknjizeno,ID_MesecPoreza,ID_Predhodni,ID_DokumentaStablo,IId,ID_KadrovskaEvidencija,ID_OrganizacionaStrukturaView,ID_LikvidacijaDokumenta, ID_DokumentaTotali  AS ID_GgRrDokumentaStavkeView,BrDok,Datum,Opis,Predhodni,LikvidacijaDokumenta,Proknjizeno,MesecPoreza,TTime,SifRadnika,NazivOrg,RB FROM DokumentaTotali  as s  WITH(NOLOCK)  WHERE s.ID_DokumentaStablo= " + strstablo + " and nazivorg in (Select nazivorg  from OrganizacionaStrukturaStavkeView where ID_OrganizacionaStrukturaStablo= 6)  order by  s.id_DokumentaTotali desc";
                    docref1.refreshDokumentGrid(activeChild,"Dokumenta", "1", str, "1", "S");
                    DataBaseBroker db = new DataBaseBroker();
                    DataTable tbb = db.ReturnDataTable("select  Count(*)  from Dokumentatotali where ID_DokumentaStablo=" + strstablo);

                    if (tbb.Rows.Count > 0) { ((Bankom.frmChield)activeChild).ToolStripLblPos.Text = Convert.ToString(Convert.ToInt32(tbb.Rows[0][0]) / 25); }

                    return;
                }
                if (strfind == "" || strKolona == "") { return; }
                strfind = strfind.Replace("Likvidatura", "LikvidacijaDokumenta");
                strfind = strfind.Replace("Ref.", "Predhodni");
                strfind = strfind.Replace("Porez", "MesecPoreza");
                strfind = strfind.Replace("Status", "Proknjizeno");






                ((Bankom.frmChield)activeChild).toolStripTextFind.Text = strfind;
                string c = ((Bankom.frmChield)activeChild).toolStripTexIme.Text;
                string d = ((Bankom.frmChield)activeChild).toolStripTextBroj.Text;
                long f = ((Bankom.frmChield)activeChild).iddokumenta;
                string sprikaz = ((Bankom.frmChield)activeChild).VrstaPrikaza;
                string dokumentje = ((Bankom.frmChield)activeChild).DokumentJe;
                //((Bankom.frmChield)activeChild).ToolStripLblPos.Text = "1";
                ((Bankom.frmChield)activeChild).ToolStripTextPos.Text = "1";

                //    clscontrolsOnForm cononf = new clscontrolsOnForm();
                clsdokumentRefresh docref = new clsdokumentRefresh();
                //cononf.addFormControls(activeChild, c, d.ToString(), "");
                clsObradaStablaStipa procss = new clsObradaStablaStipa();
                string supit = procss.Proces("Dokumenta", c, Convert.ToInt32(d));
                docref.refreshDokumentGrid(activeChild, "Dokumenta", d.ToString(), supit, "1", "S");

                //docref.refreshDokumentGrid(this, "Dokumenta", "1", "SELECT DISTINCT ID_Proknjizeno, ID_MesecPoreza, ID_Predhodni, ID_DokumentaStablo, IId, ID_KadrovskaEvidencija, ID_OrganizacionaStrukturaView, ID_LikvidacijaDokumenta, ID_DokumentaTotali  AS ID_GgRrDokumentaStavkeView, BrDok, Datum, Opis, Predhodni, LikvidacijaDokumenta, Proknjizeno, MesecPoreza, TTime, SifRadnika, NazivOrg, RB FROM DokumentaTotali as s  WITH(NOLOCK)  WHERE s.ID_DokumentaStablo = " + Convert.ToString(d) + " and nazivorg in (Select nazivorg  from OrganizacionaStrukturaStavkeView where ID_OrganizacionaStrukturaStablo = 6)  order by  s.id_DokumentaTotali desc", "S");


            }
        }

        //private void toolStripMenuRefresh_Click_1(object sender, EventArgs e)
        //{
        //    clsRefreshForm rf = new clsRefreshForm();
        //    rf.refreshform();
        //}

        private void IzborJezika_Click(object sender, EventArgs e)
        {

        }

        private void Ccalc_Click(object sender, EventArgs e)
        {
            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = true;
            myProcess.StartInfo.FileName = "calc.exe";
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.Start();
        }

        private void newToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void Kknjzi_Click(object sender, EventArgs e)
        {
            Form forma = new Form();
            forma = Program.Parent.ActiveMdiChild;

            clsKnjizenje knjizi = new clsKnjizenje();
            knjizi.ObradiNalogAutomatski();
        }


        private void Mmagacin_Click(object sender, EventArgs e)
        {

        }

        private void Pposta_Click(object sender, EventArgs e)
        {
            ////            Dim godina As Integer
            ////        Dim KnjigaPoste As New ADODB.Recordset
            ////        Dim Maxbroj As Integer
            ////        Dim PidDok As Long
            ////        KnjigaPoste.CursorLocation = adUseClient
            ////        KnjigaPoste.Open "Select * from IzlaznaPosta where  PrenesenoUknjigu= 0 and month(datum)= 12 and year(datum)=year(getdate())-1 and ID_organizacionaStrukturaStablo= " + Str(idFirme), cnn1
            ////        If Not KnjigaPoste.EOF Then
            ////           godina = Year(Now) - 1
            ////        Else
            ////             godina = Year(Now)
            ////        End If
            ////        KnjigaPoste.Close

            ////        If Trim(NazivKlona) = "KnjigaIzlaznePoste" Then
            ////           KnjigaPoste.Open "Select max(rbr) as maximum from IzlaznaPosta where  Vrsta='I' and year(datum)=" + Trim(Str(godina)) + " and ID_organizacionaStrukturaStablo= " + Str(idFirme), cnn1
            ////        Else
            ////            KnjigaPoste.Open "Select max(rbr) as maximum from IzlaznaPosta where Vrsta='I' and year(datum)=" + Trim(Str(godina)) + " and ID_organizacionaStrukturaStablo= " + Str(idFirme), cnn1
            ////        End If

            ////        If Not KnjigaPoste.EOF Then
            ////           If Not IsNull(KnjigaPoste!Maximum) Then
            ////              Maxbroj = KnjigaPoste!Maximum
            ////           Else
            ////                Maxbroj = 0
            ////           End If
            ////        End If


            ////        KnjigaPoste.Close
            ////        If godina = Year(Now) - 1 Then
            ////           KnjigaPoste.Open "Select id_IzlaznaPosta,ID_DokumentaView from IzlaznaPosta where PrenesenoUknjigu= 0 and year(datum)=" + Trim(Str(godina)) _
            ////                          & " and month(datum)= 12 and ID_organizacionaStrukturaStablo= " + Str(idFirme) + " order by 1 asc", cnn1
            ////        Else
            ////            KnjigaPoste.Open "Select id_IzlaznaPosta,ID_DokumentaView from IzlaznaPosta where PrenesenoUknjigu= 0 and  ID_DokumentaView=" + Str(IdDokView) _
            ////                       & " order by id_IzlaznaPosta asc", cnn1
            ////        End If

            ////        Do While Not KnjigaPoste.EOF
            ////           PidDok = KnjigaPoste!ID_DokumentaView
            ////           Do While PidDok = KnjigaPoste!ID_DokumentaView
            ////              Maxbroj = Maxbroj + 1
            ////              cnn1.Execute "Update  IzlaznaPosta set PrenesenoUKnjigu=1 ,rbr =" + Str(Maxbroj) _
            ////                         & " Where id_IzlaznaPosta=" + Str(KnjigaPoste!id_IzlaznaPosta)
            ////              KnjigaPoste.MoveNext
            ////              If KnjigaPoste.EOF Then Exit Do
            ////           Loop
            ////           If Trim(NazivKlona) = "KnjigaIzlaznePoste" Then
            ////              cnn1.Execute "Update  IzlaznaPosta set Vrsta='I'" _
            ////                      & " Where ID_DokumentaView=" + Str(PidDok)
            ////           Else
            ////                cnn1.Execute "Update  IzlaznaPosta set Vrsta='U'" _
            ////                      & " Where id_DokumentaView=" + Str(PidDok)
            ////           End If
            ////           cnn1.Execute "TotaliZaDokument 'KnjigaIzlaznePoste'," + Str(PidDok), adCmdStoredProc
            ////        Loop


            ////        KnjigaPoste.Close
            ////        Set KnjigaPoste = Nothing

            ////'        If Trim(NazivKlona) = "KnjigaIzlaznePoste" Then
            ////'           cnn1.Execute "Update  IzlaznaPosta set Vrsta='I'  Where   ID_DokumentaView=" + Str(IdDokView)
            ////'        Else
            ////'           cnn1.Execute "Update  IzlaznaPosta set Vrsta='U'  Where   ID_DokumentaView=" + Str(IdDokView)
            ////'        End If

            ////'        cnn1.Execute "TotaliZaDokument 'KnjigaIzlaznePoste'," + Str(IdDokView), adCmdStoredProc


            ////        OSD.OsveziDokument fform, cnn1, "DA"
        }

        private void FormiranjePPPPDZaPlate_Click(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            string pDatum = d.ToString("dd.MM.yy");
            string mg = Prompt.ShowDialog(pDatum.Substring(3, 2) + pDatum.Substring(6, 2), "Formiranje PPPPD za plate", "Unesite mesec i godinu za koji formiramo " + Environment.NewLine + " PPPPD za plate ");
            if (string.IsNullOrEmpty(mg)) { return; }
            clsOperacije co = new clsOperacije();
            bool r = co.IsNumeric(mg);
            if (r == false) { MessageBox.Show("Nekorektan unos"); return; }
            if (mg.Length != 4) { MessageBox.Show("Nekorektan unos"); return; }
            string svrsta = Prompt.ShowDialog("", "Formiranje PPPPD za plate", "Unesite vrstu obracuna: A za akontaciju ili K za platu ");
            svrsta = svrsta.ToUpper();
            if (svrsta != "a".ToUpper() && svrsta != "K".ToUpper())
            {
                MessageBox.Show("Pogresno unesena vrsta obracuna moze samo A ili K PONOVITE!!!!!");
                return;
            }
            clsXmlPlacanja cxml = new clsXmlPlacanja();
            cxml.izborPlacanja(2, mg + svrsta);
            frmPrint fs = new frmPrint();
            fs.BackColor = System.Drawing.Color.Snow;

            fs.MdiParent = this;
            fs.Text = "plate-" + mg;
            fs.LayoutMdi(MdiLayout.TileVertical);
            fs.imefajla = "plate" + mg;
            fs.kojiprint = "pla";
            fs.Show();

            toolStrip1.Visible = true;

            ToolStripLabel itemn = new ToolStripLabel();
            ToolStripButton itemB = new ToolStripButton();
            ToolStripSeparator itemnsep = new ToolStripSeparator();
            itemn.Text = "plate-" + mg;
            itemn.Name = "plate-" + mg;
            itemB.Image = global::Bankom.Properties.Resources.del12;
            itemnsep.Name = "plate-" + mg;
            itemn.Click += new EventHandler(itemn_click);

            itemB.Click += new EventHandler(itemB_click);
            itemB.Name = "plate-" + mg;

            toolStrip1.Items.Add(itemn);
            toolStrip1.Items.Add(itemB);
            toolStrip1.Items.Add(itemnsep);
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void Prevoz_Click(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            string pDatum = d.ToString("dd.MM.yy");

            string mg = Prompt.ShowDialog(pDatum.Substring(3, 2) + pDatum.Substring(6, 2), "Formiranje naloga za knjiženje prevoza : ", "Unesite mesec i godinu za koji isplaćujemo prevoz");
            if (string.IsNullOrEmpty(mg)) { return; }
            clsOperacije co = new clsOperacije();
            bool r = co.IsNumeric(mg);
            if (r == false) { MessageBox.Show("Nekorektan unos"); return; }
            if (mg.Length != 4) { MessageBox.Show("Nekorektan unos"); return; }



            clsXmlPlacanja cxml = new clsXmlPlacanja();
            cxml.izborPlacanja(0, mg);
            frmPrint fs = new frmPrint();
            fs.BackColor = System.Drawing.Color.Snow;

            fs.MdiParent = this;
            fs.Text = "prevoz-" + mg;
            fs.LayoutMdi(MdiLayout.TileVertical);
            fs.imefajla = "prevoz" + mg;
            fs.Show();

            toolStrip1.Visible = true;

            ToolStripLabel itemn = new ToolStripLabel();
            ToolStripButton itemB = new ToolStripButton();
            ToolStripSeparator itemnsep = new ToolStripSeparator();
            itemn.Text = "prevoz-" + mg;
            itemn.Name = "prevoz-" + mg;
            itemB.Image = global::Bankom.Properties.Resources.del12;
            itemnsep.Name = "prevoz-" + mg;
            itemn.Click += new EventHandler(itemn_click);

            itemB.Click += new EventHandler(itemB_click);
            itemB.Name = "prevoz-" + mg;

            toolStrip1.Items.Add(itemn);
            toolStrip1.Items.Add(itemB);
            toolStrip1.Items.Add(itemnsep);
            LayoutMdi(MdiLayout.TileVertical);

        }

        private void Nagrade_Click(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            string pDatum = d.ToString("dd.MM.yy");

            string mg = Prompt.ShowDialog(pDatum.Substring(3, 2) + pDatum.Substring(6, 2), "Formiranje naloga za knjiženje nagrada :  ", "Unesite mesec i godinu za koji isplaćujemo nagrade");
            if (string.IsNullOrEmpty(mg)) { return; }
            clsOperacije co = new clsOperacije();
            bool r = co.IsNumeric(mg);
            if (r == false) { MessageBox.Show("Nekorektan unos"); return; }
            if (mg.Length != 4) { MessageBox.Show("Nekorektan unos"); return; }



            clsXmlPlacanja cxml = new clsXmlPlacanja();
            cxml.izborPlacanja(1, mg);
            frmPrint fs = new frmPrint();
            fs.BackColor = System.Drawing.Color.Snow;
            fs.kojiprint = "nag";
            fs.MdiParent = this;
            fs.Text = "nagrade-" + mg;
            fs.LayoutMdi(MdiLayout.TileVertical);
            fs.imefajla = "nagrade" + mg;
            fs.Show();

            toolStrip1.Visible = true;

            ToolStripLabel itemn = new ToolStripLabel();
            ToolStripButton itemB = new ToolStripButton();
            ToolStripSeparator itemnsep = new ToolStripSeparator();
            itemn.Text = "nagrade-" + mg;
            itemn.Name = "nagrade-" + mg;
            itemB.Image = global::Bankom.Properties.Resources.del12;
            itemnsep.Name = "nagrade-" + mg;
            itemn.Click += new EventHandler(itemn_click);

            itemB.Click += new EventHandler(itemB_click);
            itemB.Name = "nagrade-" + mg;

            toolStrip1.Items.Add(itemn);
            toolStrip1.Items.Add(itemB);
            toolStrip1.Items.Add(itemnsep);
            LayoutMdi(MdiLayout.TileVertical);
        }



        private void FaktureRecepcijeZaOdabraneDatume_Click(object sender, EventArgs e)
        {
            Preuzimanja.FaktureRecepcijeZaOdabraneDatume();
        }


        private void FaktureRestoranaZaOdabraneDatume_Click(object sender, EventArgs e)

        {
            Preuzimanja.FaktureRestoranaZaOdabraneDatume();
        }

        private void Razduzenjesirovinaminibar_Click(object sender, EventArgs e)
        {
            Preuzimanja.RazduzenjeSirovinaMiniBar();
        }


        private void Razduzenjesirovinazaodabraniintervaldatuma_Click(object sender, EventArgs e)

        {
            Preuzimanja.RazduzenjeSirovinaZaOdabraniIntervalDatuma();
        }





        private void PreuzimanjeManjkovaIViskova_Click(object sender, EventArgs e)
        {
            Preuzimanja.PreuzimanjeManjkovaIViskova();
        }


        private void PreuzimanjeUplataKupacaIzBanaka_Click(object sender, EventArgs e)
        {
            Preuzimanja.PreuzimanjeUplataKupacaIzBanaka();
        }



        private void PreuzimanjeRateKredita_Click(object sender, EventArgs e)
        {
            Preuzimanja.PreuzimanjeRateKredita();

        }


        private void RaznoMemuItem_Click(object sender, EventArgs e)
        {
            //    KursnaLista kl = new KursnaLista();
            //    kl.Show();
        }

        private void PocesiranjeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void PocetakGodine_Click(object sender, EventArgs e)
        {

        }

        private void KursnaListaZaCeluGodinu_Click(object sender, EventArgs e)
        {
            string GodinaKursa = "";
            string PocetniDatumKursa = "";
            int KojiIDDokstablo = 1;
            string sql = "";
            long granica = 0;
            int ret = 1;
            string ID_DokumentaView = "1";
            DateTime DatumKursa;
            DataBaseBroker db = new DataBaseBroker();

            if (MessageBox.Show("Upisujemo kursnu listu za " + (System.DateTime.Now).Year.ToString(), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                GodinaKursa = Prompt.ShowDialog("", "Unesite Godinu za kursnu listu ", "Kursna lista");
                PocetniDatumKursa = "01.01." + GodinaKursa.Trim();
                sql = "select ID_DokumentaTotali from  DokumentaTotali where dokument ='KursnaLista' and Datum>@param0";
                DataTable t = db.ParamsQueryDT(sql, PocetniDatumKursa);

                if (t.Rows.Count == 0)
                {
                    sql = "select ID_DokumentaStablo from DokumentaStablo where Naziv='KursnaLista'";
                    DataTable dt = db.ParamsQueryDT(sql);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Nije definisana kursna lista !!!");
                    }
                    else
                    {
                        KojiIDDokstablo = Convert.ToInt32(dt.Rows[0]["ID_DokumentaStablo"]);

                        clsOperacije cOp = new clsOperacije();
                        if (cOp.Prestupna(Convert.ToInt32(GodinaKursa)) == true)
                            granica = 366;
                        else
                            granica = 365;

                        int i = 1;

                        for (; i <= granica; i++)
                        {
                            DatumKursa = Convert.ToDateTime(PocetniDatumKursa).AddDays(i);

                            clsObradaOsnovnihSifarnika cls = new clsObradaOsnovnihSifarnika();
                            string ParRb = "";

                            ret = cls.UpisiDokument(ref ParRb, "Kursna lista " + DatumKursa.Date, KojiIDDokstablo, DatumKursa.ToString());

                            if (ret == -1)
                            {
                                MessageBox.Show("Greska prilikom inserta!");
                                return;
                            }
                            ID_DokumentaView = ret.ToString();

                            //stavka za domacu valutu
                            sql = " Insert into KursnaLista(ID_SifrarnikValuta,ID_Zemlja,ID_DokumentaView,datum,paritet,"
                                + " Kupovni,Srednji,Prodajni,Dogovorni,verzija,KupovniZaDevize,ProdajniZaDevize,OznVal,UUser,TTIME )"
                                + " Values(@param0,@param1,@param2,@param3,@param4, "
                                + " @param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14)";

                            DataTable dkl = db.ParamsQueryDT(sql, 1, Program.ID_MojaZemlja, ID_DokumentaView, DatumKursa.ToString(), 001,
                                1, 1, 1, 1, "", 1, 1, Program.DomacaValuta, Program.idkadar.ToString(), (System.DateTime.Now).ToString());

                            // Druga stavka za eur ako je zemlja bosna

                            if (Program.ID_MojaZemlja == 38)
                            {
                                sql = " Insert into KursnaLista(ID_SifrarnikValuta,ID_Zemlja,ID_DokumentaView,datum,paritet,"
                                    + " Kupovni,Srednji,Prodajni,Dogovorni,verzija,KupovniZaDevize,ProdajniZaDevize,OznVal,UUser,TTIME )"
                                    + " Values(@param0,@param1,@param2,@param3,@param4, "
                                    + " @param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12,@param13,@param14)";

                                DataTable dkb = db.ParamsQueryDT(sql, 1, Program.ID_MojaZemlja, ID_DokumentaView, DatumKursa.ToString(), 001,
                                    1.95583, 1.95583, 1.95583, 1.95583, "", 1.95583, 1.95583, "EUR", Program.idkadar.ToString(), (System.DateTime.Now).ToString());
                            }
                            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:Dokumenta", "IdDokument:" + ID_DokumentaView);
                            db.ExecuteStoreProcedure("TotaliZaDokument", "NazivDokumenta:KursnaLista", "IdDokument:" + ID_DokumentaView);
                        }
                        MessageBox.Show("Zavrseno !!!");
                    }
                }
                else MessageBox.Show("Vec je unesena kursna lista za datume tekuce godine !!!");


            }
            return;
        }

        private void PopunjavanjeTabeleDatuma_Click(object sender, EventArgs e)
        {
            string GodinaDatuma = "";
            string sql = "";
            DataBaseBroker db = new DataBaseBroker();

            if (MessageBox.Show("Upisujemo tabelu datuma za " + (System.DateTime.Now).Year.ToString(), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                GodinaDatuma = Prompt.ShowDialog("", "Unesite Godinu za tabelu datuma ", "Tabela datuma");
                if (GodinaDatuma == "") { MessageBox.Show("Niste uneli godinu ponovite !!!"); return; }

                if (GodinaDatuma != (System.DateTime.Now).Year.ToString() && GodinaDatuma != (System.DateTime.Now).AddYears(+1).Year.ToString())
                {
                    MessageBox.Show("Pogresno unesena godina ponovite");
                    return;
                }
                else
                {
                    sql = "select time_id from  Time_by_Day where the_year =@param0 ";
                    DataTable t = db.ParamsQueryDT(sql, GodinaDatuma);
                    if (t.Rows.Count == 0)
                    {
                        db.ExecuteStoreProcedure("PopuniTimeByDay", "Godina:" + GodinaDatuma);
                        MessageBox.Show("Zavrseno !!!");
                    }
                    else MessageBox.Show("Vec je unesena godina!!!");
                }
            }
            return;
        }

        private void PrenosNalogaNaPlacanje_Click(object sender, EventArgs e)
        {
            ///  URADJENO
            //clsPreuzimanja cp = new clsPreuzimanja();//BORKA
            DateTime DatOd = DateTime.Parse(DateTime.Now.ToString("dd.MM.yy"));///, "dd/MM/yyyy", null);// , CultureInfo.InvariantCulture);
            string TekuciRacun = "";



            TekuciRacun = Prompt.ShowDialog("", "Unesite tekuci racun za koji prenosimo naloge ", "Prepisivanje naloga iz PripremeZaPlacanje");
            if (TekuciRacun == "") { return; }

            //string vrati = cp.PrepisiNaloge(DatOd.ToShortDateString(), TekuciRacun); //BORKA
            MessageBox.Show("Zavrseno!!");



        }

        private void UvozPrevozaUPlacanja_Click(object sender, EventArgs e)
        {
            clsXmlPlacanja cls = new clsXmlPlacanja();
            cls.izborPlacanja(4, "");
        }

        private void UvozPlataUPlacanja_Click(object sender, EventArgs e)
        {
            clsXmlPlacanja cls = new clsXmlPlacanja();
            cls.izborPlacanja(3, "");
        }

        private void PreuzimanjeIzvodaIzBanaka_Click(object sender, EventArgs e)
        {
            clsPreuzimanja cp = new clsPreuzimanja();
            string strPreuzimanjePlacanja = cp.preuzimanjeIzvodaizBanaka();
            if (strPreuzimanjePlacanja == "") { return; }


            char[] separators = { '#' };
            frmIzvod childForm = new frmIzvod();
            childForm.BackColor = System.Drawing.Color.Snow;
            childForm.MdiParent = this;
            childForm.strPutanjaPlacanja = strPreuzimanjePlacanja.Split(separators)[0];
            childForm.mesecgodina = strPreuzimanjePlacanja.Split(separators)[1];
            childForm.IdDokView = Convert.ToInt32(strPreuzimanjePlacanja.Split(separators)[2]);
            childForm.KojiPrepis = strPreuzimanjePlacanja.Split(separators)[3];
            childForm.Show();
        }

        private void PrepisNaplataIPlacanjaUIzvod_Click(object sender, EventArgs e)
        {
            clsOperacije co = new clsOperacije();
            clsPreuzimanja cp = new clsPreuzimanja();
            DataBaseBroker db = new DataBaseBroker();
            string DatOd = "";
            string TekuciRacun = "";
            DataTable rsp = new DataTable();
            DatOd = Prompt.ShowDialog("", "Unesite datum za koji prepisujemo izvod ", "Prepisivanje izvoda iz Placanja i naplata");
            if (DatOd == "") { return; }

            if (co.IsDateTime(DatOd) == false)
            { MessageBox.Show("pogresno unesen datum ponovite !!"); return; };

            TekuciRacun = Prompt.ShowDialog("", "Unesite tekuci racun za koji prepisujete promet ", "Prepisivanje izvoda iz Placanja i naplata");
            if (TekuciRacun == "") { return; }
            rsp = db.ReturnDataTable("if not exists (select datum from  IzvodTotali  where Datum='" + DatOd + "' And Blagajna='" + TekuciRacun.ToString() + "') select 0 else select 1 ");
            if (rsp.Rows.Count == 0) { MessageBox.Show("Ne postoje podaci za datum i tekuci racun !!"); return; }

            if (Convert.ToInt16(rsp.Rows[0][0]) == 1)
            { MessageBox.Show("Vec je izvrsen prepis izvoda za datum " + DatOd); }
            else
            {
                cp.izborPReuzimanja(1, DatOd + "#" + TekuciRacun);
            }
            MessageBox.Show("Zavrseno!!");
        }

        private void FormiranjePPPPDZaPrevoz_Click(object sender, EventArgs e)
        {

        }

        private void PreuzimanjeManjkovaIViskova_Click_1(object sender, EventArgs e)
        {
            Preuzimanja.PreuzimanjeManjkovaIViskova();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            KursnaLista kl = new KursnaLista();
            kl.FormBorderStyle = FormBorderStyle.None;
            
            kl.Show();
        }

        private void rastuci_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Program.colname))
            {
                MessageBox.Show("Odaberite kolonu za sortiranje");
            }
            else
            {
                frmChield activeChild = (frmChield)this.ActiveMdiChild;
                activeChild.BackColor = System.Drawing.Color.Snow;
                string dokumentje = ((Bankom.frmChield)activeChild).DokumentJe;
                string nazivklona = ((Bankom.frmChield)activeChild).imedokumenta;
                DataGridView dg = activeChild.Controls.Find(Program.imegrida, true).FirstOrDefault() as DataGridView;
                if (dg != null)
                {
                    Program.smer = " ASC ";
                    clsObradaOsnovnihSifarnika obs = new clsObradaOsnovnihSifarnika();
                    obs.SortirajGrid(ref dg, nazivklona, dokumentje);
                }
                Program.colname = "";
            }
        }

        private void opadajuci_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Program.colname))
            {
                MessageBox.Show("Odaberite kolonu za sortiranje");
            }
            else
            {
                frmChield activeChild = (frmChield)this.ActiveMdiChild;
                activeChild.BackColor = System.Drawing.Color.Snow;
                string dokumentje = ((Bankom.frmChield)activeChild).DokumentJe;
                string nazivklona = ((Bankom.frmChield)activeChild).imedokumenta;
                DataGridView dg = activeChild.Controls.Find(Program.imegrida, true).FirstOrDefault() as DataGridView;
                if (dg != null)
                {
                    Program.smer = " DESC ";
                    clsObradaOsnovnihSifarnika obs = new clsObradaOsnovnihSifarnika();
                    obs.SortirajGrid(ref dg, nazivklona, dokumentje);
                }
            }
            Program.colname = "";
        }

        private void Kkalki_Click(object sender, EventArgs e)
        {
            clsIzvestaji IZV = new clsIzvestaji();
            frmChield activeChild = (frmChield)this.ActiveMdiChild;
            activeChild.BackColor = System.Drawing.Color.Snow;
            string NazivKlona = ((Bankom.frmChield)activeChild).imedokumenta;
            string BrDok = ((Bankom.frmChield)activeChild).brdok;
            string KojiIzvestaj = "";
            Form novaforma = new Form();

            if (((Bankom.frmChield)activeChild).DokumentJe == "D" && (NazivKlona.Trim() == "PDVPredracun" || NazivKlona.Trim() == "InoPredracun" || NazivKlona.Trim() == "PDVPonuda"))
            {
                KojiIzvestaj = "KalkulacijaIzlazaUzPredracun";
            }
            else
            {
                if (Program.imeFirme == "Bankom" || Program.imeFirme == "Feedmix")
                    KojiIzvestaj = "KalkulacijaIzlazaSaTroskovima";
                else
                    KojiIzvestaj = "KalkulacijaIzlaza";
            }

            Program.Parent.ShowNewForm("Izvestaj", 1, KojiIzvestaj, -1, "", "", "I", "", "");
            novaforma = Program.Parent.ActiveMdiChild;
            Field kontrola = (Field)novaforma.Controls["BrDok"];
            if (kontrola != null)
                kontrola.Vrednost = BrDok;
            IZV.PrikazIzvestaja(KojiIzvestaj);
        }

        private void Kkalku_Click(object sender, EventArgs e)
        {
            clsIzvestaji IZV = new clsIzvestaji();
            DataBaseBroker db = new DataBaseBroker();
            frmChield activeChild = (frmChield)this.ActiveMdiChild;
            activeChild.BackColor = System.Drawing.Color.Snow;
            string NazivKlona = ((Bankom.frmChield)activeChild).imedokumenta;
            string BrDok = ((Bankom.frmChield)activeChild).brdok;
            string KojiIzvestaj = "";
            Form novaforma = new Form();
            Field pb = null;
            if (((Bankom.frmChield)activeChild).DokumentJe == "D" && NazivKlona.Trim() == "KonacniUlazniRacun")
            {
                string sel = "select distinct BrDok,ID_PDVKalkulacijaUlazaTotali as IDKalk from PDVKalkulacijaUlazaTotali where UlzniRacunBroj=@param0";
                DataTable t = db.ParamsQueryDT(sel, BrDok);
                if (t.Rows.Count > 0)
                {
                    if (Program.imeFirme == "Bankom" || Program.imeFirme == "Feedmix")
                    {
                        KojiIzvestaj = "KalkulacijaUlazaSaTroskovima";
                        Program.Parent.ShowNewForm("Izvestaj", 1, KojiIzvestaj, -1, "", "", "I", "", "");
                        novaforma = Program.Parent.ActiveMdiChild;
                        pb = (Field)novaforma.Controls["BrDok"];
                        if (pb != null)
                        {
                            pb.Vrednost = BrDok;
                            IZV.PrikazIzvestaja(KojiIzvestaj);
                        }
                    }
                    else
                    {
                        KojiIzvestaj = "PDVKalkulacijaUlaza";
                        Program.Parent.ShowNewForm("Dokumenta", 1476, KojiIzvestaj, Convert.ToInt64(t.Rows[0]["IDKalk"].ToString()), t.Rows[0]["BrDok"].ToString(), "", "D", "", "");
                    }

                }
                else
                    MessageBox.Show("Ne postoji kalkulacija ulaza za odabrani dokument");
            }
        }
        private void English_Click(object sender, EventArgs e)
        {
            Program.ID_Jezik = 5;
        }

        private void toolStrip_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void fileMenu_Click(object sender, EventArgs e)
        {

        }

        private void Ruski_Click_1(object sender, EventArgs e)
        {
            Program.ID_Jezik = 6;
        }

        private void PreuzimanjeRateKredita_Click_1(object sender, EventArgs e)
        {
            Preuzimanja.PreuzimanjeRateKredita();
        }

        private void PreuzimanjeUplataKupacaIzBanaka_Click_1(object sender, EventArgs e)
        {
            Preuzimanja.PreuzimanjeUplataKupacaIzBanaka();
        }

        private void Oodjava_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void Oodjava_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

   


        public void SrediFormu()
        {
           // flowLayoutPanel1.Width = 161;


            flowLayoutPanel1.Width = 162;
            flowLayoutPanel1.Width = 0;
           
            button1.Location = new Point(0, 73);

        }


        //// tamara 21.10.2020.
        private void button1_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Width == 162)
            {
                flowLayoutPanel1.Width = 0;

                button1.Location = new Point(0, 73);
            }
            else
            {
                flowLayoutPanel1.Width = 162;
           
                button1.Location = new Point(159, 73);
            }
        }




        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

            if (Program.KlasifikacijaSlovo == "K" || Program.KlasifikacijaSlovo == "k")
            {

                toolStripTextBox1.AutoCompleteCustomSource = null;
            }
            else
            {
                var param0 = toolStripTextBox1.Text;
                DataBaseBroker db = new DataBaseBroker();
                AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();


                string sselect;
                string idke = Program.idkadar.ToString();
                string idfirme = Program.idFirme.ToString();
                //tamara 08.01.2021.s
                sselect = "select distinct NazivJavni from DokumentaStablo where ccopy= 0 and VrstaCvora='d' and Naziv in "
                +"(select g.naziv from Grupa as g,KadroviIOrganizacionaStrukturaStavkeView as ko Where (KO.ID_OrganizacionaStruktura = G.ID_OrganizacionaStruktura Or KO.id_kadrovskaevidencija = G.id_kadrovskaevidencija) "
                +" And KO.ID_OrganizacionaStrukturaStablo = " + idfirme + " and ko.id_kadrovskaevidencija = " + idke + ") and NazivJavni like '%" + param0 +"%'";

                var dr = db.ReturnDataReader(sselect);



                if (dr.HasRows == true)
                {
                    while (dr.Read())
                        namesCollection.Add(dr["NazivJavni"].ToString());
                }
// BORKA 10.12.20 CEMU OVO SLUZI ???????????????
// Tamara: autocomplete popunjava predloge u pretrazi tj kada ukucate kona, predlaze i konacni racun i konacni ulazni racun...
                toolStripTextBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                toolStripTextBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                toolStripTextBox1.AutoCompleteCustomSource = namesCollection;
                //SrediFormu();
                ToolStripTextBox item = sender as ToolStripTextBox;
                BrziPristup(item);
                //30.12.2020.
                toolStripTextBox1.Text = "";
            }
        }

        public void BankomMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            //tamara 16.12.2020.

            clsCustomMessagebox customMessage = new clsCustomMessagebox(
            "Da li ste sigurni da želite da izađete iz aplikacije?",
            "Ne",
            "Da",
            "Odjava"
            );
            customMessage.StartPosition = FormStartPosition.CenterParent;
     

            customMessage.ShowDialog();

            if (customMessage.DialogResult == DialogResult.Cancel)
                e.Cancel = false;
           else if (customMessage.DialogResult == DialogResult.OK)
                e.Cancel = true;
           else
            {         
                Thread t = new Thread(new ThreadStart(Program.Main));
#pragma warning disable CS0618 // Type or member is obsolete
                t.ApartmentState = ApartmentState.STA;
#pragma warning restore CS0618 // Type or member is obsolete
                t.Start();
                Application.ExitThread();
              
            } 
        }

        

        private void unosNovogČvoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //zajedno 28.10.2020.

            //clsTreeProcessing tr = new clsTreeProcessing();
            //MessageBox.Show(tr.ime2.ToString());
            MessageBox.Show(Program.AktivnaSifraIzvestaja);
        }


        private void Uunos_DropDownClosed(object sender, EventArgs e)
        {
            Uunos.ForeColor = System.Drawing.Color.Snow;
        }

        private void Uunos_DropDownOpened(object sender, EventArgs e)
        {
            Uunos.ForeColor = System.Drawing.Color.Black;
        }
        private void Ggrupisi_DropDownClosed(object sender, EventArgs e)
        {
            Ggrupisi.ForeColor = System.Drawing.Color.Snow;
        }
        private void Ggrupisi_DropDownOpened(object sender, EventArgs e)
        {
            Ggrupisi.ForeColor = System.Drawing.Color.Black;
        }

        private void Ppotvrda_Click_1(object sender, EventArgs e)
        {           
            Form forma = this.ActiveMdiChild;
            //13.01.2021. tamara
            if (forma.Text == "LOT")
            {

                foreach (var pb in forma.Controls.OfType<Field>())
                {
                    //if (pb.cTip == 10 || pb.cTip == 8)
                    //    pb.Enabled = false;
                    if (pb.cTip == 10)
                    {
                        if (pb.VrstaKontrole == "combo") pb.comboBox.Enabled = false;
                        else pb.textBox.Enabled = false;
                    }
                    if (pb.cTip == 8) pb.dtp.Enabled = false;
                }
            }
            Boolean vrati = new Boolean();
            if (forma == null)
            {
                MessageBox.Show("Nemate aktivnu formu!");
                vrati = false;
                return;
            }
            // Jovana 08.12.20
            if (forma.Controls["OOperacija"].Text.Trim() != "UNOS" && forma.Controls["OOperacija"].Text.Trim() != "" && forma.Controls["OOperacija"].Text.Trim() != "PREGLED")
            {
                if (((Bankom.frmChield)forma).idReda<2 && ((Bankom.frmChield)forma).DokumentJe != "K")
                {
                    if (((Bankom.frmChield)forma).idReda == -1) { }
                    else
                    {
                        MessageBox.Show("Niste oznacili slog za odabranu operaciju!");
                        vrati = false;
                        return;
                    }
                }
            }

            if (forma.Controls["OOperacija"].Text.Trim() == "" && ((Bankom.frmChield)forma).DokumentJe != "I")
            {
                MessageBox.Show("Niste odabrali operaciju!");
            }
            else
            {
                if (forma.Controls["OOperacija"].Text.Trim() == "PREGLED")
                {
                    clsPregled clp = new clsPregled();
                    long Koliko = clp.Pregledaj("S");
                    if (Koliko < 1)
                    {
                        MessageBox.Show("Ne postoje podaci za zadate uslove!");
                    }
                }
                else
                {
                    // POZIV KLASE CRUD ZA IZVRSENJE ZADATE OPERACIJE
                    CRUD ccrud = new CRUD();

                    switch (((Bankom.frmChield)forma).DokumentJe)
                    {
                        case "I":
                            switch (((Bankom.frmChield)forma).imedokumenta)
                            {
                                case "AnalizaNabavke":
                                    //RasporedTroskova 0, fform.Controls("ctDatumOd").Vrednost, fform.Controls("ctDatumDo").Vrednost, fform.Controls("ctNazivSkl").Vrednost
                                    break;
                                //case "SpisakDokumenata":
                                //    //frmMain.Toolbar2.Buttons("cmdodabrani").Visible = True
                                //    //frmMain.Toolbar2.Buttons("cmdodabrani").Enabled = True
                                //    break;
                                case "DnevniBilansUspeha":
                                    clsBilansi OBB = new clsBilansi();
                                    OBB.ObradiBilans();
                                    break;
                                case "POPPdv":
                                    clsPOPPdv POP = new clsPOPPdv();
                                    POP.ObradiPOPPdv();
                                    break; ///'''Exit Function
                                case "IOS":
                                    clsObradaOsnovnihSifarnika obos = new clsObradaOsnovnihSifarnika();
                                    //fform("DatumDo").Vrednost, fform("konto").Vrednost, fform("NazivKom").ID
                                    var pb = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "DatumDo".ToString().Trim());
                                    string DatumDo = pb.Vrednost;
                                    pb = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "konto".ToString().Trim());
                                    string konto = pb.Vrednost;
                                    pb = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == "NazivKom".ToString().Trim());
                                    int nazivkom = Convert.ToInt32(pb.ID);
                                    if (obos.Iosi(DatumDo, konto, nazivkom) == true)
                                    { break; }
                                    else
                                        break;
                                default:
                                    clsIzvestaji IZV = new clsIzvestaji();
                                    IZV.PrikazIzvestaja(((Bankom.frmChield)forma).imedokumenta);
                                    break;
                            }
                            break;
                        case "S":
                            vrati = ccrud.DoIt(forma, Convert.ToString(((Bankom.frmChield)forma).idReda), ((Bankom.frmChield)forma).imestabla);
                            break;
                        case "K":
                            if (forma.Controls["OOperacija"].Text.Trim() == "UNOS")
                            {
                                clsObradaKlasifikacija o = new clsObradaKlasifikacija();
                                string d = toolStripTextBox1.Text;
                                o.Klasifikacija_Click(d, Program.pomIzv, Program.pomStablo);
                            }
                            else if (forma.Controls["OOperacija"].Text.Trim() == "BRISANJE")
                            {
                                clsObradaKlasifikacija o = new clsObradaKlasifikacija();
                                o.KlasifikacijaBrisanje(Program.pomIzv, Program.pomStablo);
                            }
                            else if (forma.Controls["OOperacija"].Text.Trim() == "IZMENA")
                            {
                                clsObradaKlasifikacija o = new clsObradaKlasifikacija();
                                string d = toolStripTextBox1.Text;
                                o.KlasifikacijaIzmena(d, Program.pomIzv, Program.pomStablo);
                            }
                            else if (forma.Controls["OOperacija"].Text.Trim() == "KOPIRAJ")
                            {
                                clsObradaKlasifikacija o = new clsObradaKlasifikacija();
                                o.KlasifikacijaPremestiGrupu(Program.pomIzv, Program.pomStablo);


                                //ovde smo stigle
                            }
                            else if (forma.Controls["OOperacija"].Text.Trim() == "NALEPI")
                            {
                                clsObradaKlasifikacija o = new clsObradaKlasifikacija();
                                o.KlasifikacijaNovaPozicija(Program.pomIzv, Program.pomStablo);

                            }
                       
                            break;
                        case "D":
                            vrati = ccrud.DoIt(forma, Convert.ToString(((Bankom.frmChield)forma).iddokumenta), ((Bankom.frmChield)forma).imedokumenta);
                            if (forma.Controls["OOperacija"].Text.Trim() == "BRISANJE") break;
                            if (vrati == false) break;// ovde se vraca tok 

                            switch (forma.Controls["limedok"].Text)
                            {
                                case "NalogGlavneKnjige":
                                    clsKnjizenje knj = new clsKnjizenje();
                                    knj.ObradiNalogGlavneKnjige();
                                    break;
                                case "NalogGlavneKnjigeSimulacija":
                                    clsKnjizenje knjs = new clsKnjizenje();
                                    knjs.ObradiSimulaciju();
                                    break;
                                case "KursneRazlike":
                                    clsObradaKursnihRazlika kr = new clsObradaKursnihRazlika();
                                    vrati = kr.ObradiRazlike();
                                    break;
                                case "BilansStanja":
                                case "BilansUspeha":
                                case "BilansPrihodiIRashodi":
                                case "Pokazateljlikvidnosti":
                                    clsBilansi OBBilans = new clsBilansi();
                                    OBBilans.ObradiBilans();
                                    break;
                                case "PocetnoStanjeZaRobu":
                                    clsZatvaranjeIOtvaranjeStanja ZiO = new clsZatvaranjeIOtvaranjeStanja();
                                    vrati = ZiO.ObradiIspravku();
                                    break;
                                case "ObracunVrednostiZaliha":
                                    clsKorekcija Vrednost = new clsKorekcija();
                                    vrati = Vrednost.VrednostNaDan();
                                    break;
                                case "ZatvaranjeAvansa":
                                    clsAvansi Avans = new clsAvansi();
                                    vrati = Avans.ZatvaranjeAvansa(forma, forma.Controls["limedok"].Text, Convert.ToString(((Bankom.frmChield)forma).iddokumenta));
                                    break;
                                case "Kompenzacija":
                                    clsKompenzacija rez = new clsKompenzacija();
                                    rez.ObradiKomenzaciju();
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                           vrati = ccrud.DoIt(forma, Convert.ToString(((Bankom.frmChield)forma).iddokumenta), ((Bankom.frmChield)forma).imedokumenta);

                            break;
                    }
                    // OSVEZAVANJE FORME NAKON IZVRSENE OPERACIJE
                    if (vrati == true) //jovana
                    {
                        clsFormInitialisation fi = new clsFormInitialisation();                
                        fi.ObrisiZaglavljeIStavkePoljaZaUnos();
                        clsRefreshForm fr = new clsRefreshForm();
                        fr.refreshform();

                    }
                }
                forma.Controls["OOperacija"].Text = "";
            }

            // KRAJ else 

        }

        private void Ggrupisinp_Click(object sender, EventArgs e)
        {
            Form forma = this.ActiveMdiChild;
            forma.Controls["OOperacija"].Text = "NALEPI";
            forma.Controls["OOperacija"].Visible = false;
            Program.Parent.premestiGrupuToolStripMenuItem.Enabled = true;
            Program.Parent.premestiGrupuToolStripMenuItem.Visible = true;
            Program.Parent.Ggrupisinp.Enabled = false;
            Program.Parent.Ggrupisinp.Visible = false;
        }

        private void premestiGrupuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form forma = this.ActiveMdiChild;
            forma.Controls["OOperacija"].Text = "KOPIRAJ";
            forma.Controls["OOperacija"].Visible = false;
            Program.Parent.Ggrupisinp.Enabled = true;
            Program.Parent.Ggrupisinp.Visible = true;
            Program.Parent.premestiGrupuToolStripMenuItem.Enabled = false;
            Program.Parent.premestiGrupuToolStripMenuItem.Visible = false;
        }
        public static int i = 1;

        private void Ssort_Click(object sender, EventArgs e)
        {
            i*=-1;
            frmChield activeChild = (frmChield)this.ActiveMdiChild;
            string n = activeChild.Name.ToString();
            if (string.IsNullOrEmpty(Program.colname))
            {
                MessageBox.Show("Odaberite kolonu za sortiranje");
            }
            else if (activeChild == this.ActiveMdiChild)
                {

                string dokumentje = ((Bankom.frmChield)activeChild).DokumentJe;
                string nazivklona = ((Bankom.frmChield)activeChild).imedokumenta;
                DataGridView dg = activeChild.Controls.Find(Program.imegrida, true).FirstOrDefault() as DataGridView;
                if (dg != null)
                {
                    if (i > 0)
                    {
                        Program.smer = " ASC ";
                        clsObradaOsnovnihSifarnika obs = new clsObradaOsnovnihSifarnika();
                        obs.SortirajGrid(ref dg, nazivklona, dokumentje);
                    }
                    else
                    {
                        Program.smer = " DESC ";
                        clsObradaOsnovnihSifarnika obs = new clsObradaOsnovnihSifarnika();
                        obs.SortirajGrid(ref dg, nazivklona, dokumentje);
                    }

                }
                Program.colname = "";
            }
        }
        //zajedno 18.1.2021.
        private void DodajSliku_Click(object sender, EventArgs e)
        {
            frmSlika slika = new frmSlika(DodajSliku);
            slika.Show();
            DodajSliku.Enabled = false;
        }
    }
}

