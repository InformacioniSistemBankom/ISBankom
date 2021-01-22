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
        {
            Form form1 = Program.Parent.ActiveMdiChild;
            //form1 = Program.Parent.ActiveMdiChild;
            {
                for (int h = 0; h < Program.Parent.ToolBar.Items.Count; h++)
                {
                    Program.Parent.ToolBar.Items[h].Enabled = false;
                    switch (Program.Parent.ToolBar.Items[h].Name)
                    {
                        case "Ddokum":
                        case "Iimenik":
                            //zajedno 19.01.2021.
                        case "DodajSliku":
                        case "Iizlaz":
                        case "Ccalc":
                        case "Ppotvrda":
                        case "Pprekid":
                        case "Mmagacin":
                        case "toolStripTextBox1":         
                            Program.Parent.ToolBar.Items[h].Enabled = true;
                            break;
                        case "Ssort":
                            if (form1 != null)
                            {
                               
                             if (Convert.ToString(((Bankom.frmChield)form1).VrstaPrikaza) != "TreeView" )
                                 Program.Parent.ToolBar.Items[h].Enabled = true;
                            }
                            break;
                        case "Sstampa":
                            if (form1 != null)
                            {
                                //if (form1.Controls["OOperacija"].Text == "Unos" || form1.Controls["OOperacija"].Text == "Izmena")
                                if (form1.Controls["OOperacija"].Text.Trim() == "")
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

                        case "Ppregled":
                            Program.Parent.ToolBar.Items[h].Enabled = true;
                            if (form1 != null)
                            {
                                if (form1.Controls["ldokje"].Text == "I")                                
                                    Program.Parent.ToolBar.Items[h].Enabled = false;
                                else
                                    Program.Parent.ToolBar.Items[h].Enabled = true;
                            }

                            break;
                    }
                }
            }
        }
    }
}
