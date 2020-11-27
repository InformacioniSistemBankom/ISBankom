using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Bankom.Class
{
    class clsSettingsButtons
    {
        public void ToolBarItemsEnDis()
        {          // BORKA 23.11.20 TREBA JOS DORADITI
           Form form1 = Program.Parent.ActiveMdiChild;
            {
                for (int h = 0; h < Program.Parent.ToolBar.Items.Count; h++)
                {
                    Program.Parent.ToolBar.Items[h].Enabled = false;
                
                switch (Program.Parent.ToolBar.Items[h].Name)
                {
                    case "Ddokum":
                    case "Iimenik":
                    //case "Iizlaz":
                    case "Ccalc":
                    case "Mmagacin":
                        Program.Parent.ToolBar.Items[h].Enabled = true;
                        break;
                    case "Ppotvrda":
                    case "Pprekid":               
                          if (form1 != null)
                          {
                                Program.Parent.ToolBar.Items[h].Enabled = true;
                          }
                          break;
                        case "Sstorno":
                            if (form1 != null)
                            {
                                //BORKA 24.11.20
                                if (((Bankom.frmChield)form1).DokumentJe == "S" && ((Bankom.frmChield)form1).imestabla == "Dokumenta" && ((Bankom.frmChield)form1).VrstaPrikaza != "TreeView")
                                {
                                    Program.Parent.ToolBar.Items[h].Enabled = true;
                                }
                            }
                            break;
                        case "Ppregled":
                            if (form1 != null)
                            {
                                if (((Bankom.frmChield)form1).VrstaPrikaza != "TreeView") //BORKA 24.11.20
                                {
                                    Program.Parent.ToolBar.Items[h].Enabled = true;
                                }
                                if (form1.Controls["ldokje"].Text != "I")
                                {
                                    Program.Parent.ToolBar.Items[h].Enabled = true;
                                }
                            }
                            break;
                        case "Sstampa":
                        if (form1 != null)
                        {
                            if (form1.Controls["OOperacija"].Text == "Unos" || form1.Controls["OOperacija"].Text == "Izmena")
                            //if (form1.Controls["OOperacija"].Text.Trim() == "")
                            {
                                Program.Parent.ToolBar.Items[h].Enabled = true;
                            }
                        }
                        break;
                    case "web":
                        if (Program.imeFirme == "Bankom" || Program.imeFirme == "Bioprotein" || Program.imeFirme == "Hotel Nevski")
                        {
                            Program.Parent.ToolBar.Items[h].Enabled = true;
                            Program.Parent.ToolBar.Items[h].Visible = true;
                        }
                        break;

                    //case "Pregled":
                    //    Program.Parent.ToolBar.Items[h].Enabled = true;
                    //    if (form1 != null)
                    //    {
                    //        if (form1.Controls["ldokje"].Text == "I")
                    //        {
                    //            Program.Parent.ToolBar.Items[h].Enabled = false;
                    //        }
                    //    }
                    //    break;
                }
            }
            }
        }
    }
}
