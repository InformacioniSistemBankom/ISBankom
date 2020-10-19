using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace Bankom.Class
{
    public static class MsgBox
    {

        private static System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        public static string ResultValue="";
        public static string ResultFind="";
        private static DialogResult DialogRes;
        private static string[] buttonTextArray = new string[4];


        public enum Icon
        {
            Error,
            Exclamation,
            Information,
            Question,
            Nothing
        }

        public enum Type
        {
            ComboBox,
            TextBox,            
            Nothing
        }

        public enum Buttons
        {
            Ok,
            OkCancel,
            YesNo,
            YesNoCancel
        }

        public enum Language
        {
            Latinica,
            Cirilica,
            English
        }


        public static DialogResult ShowDialog(string Message, string Title = "", string strukupno = "", Icon icon = Icon.Information, Buttons buttons = Buttons.Ok, Type txt = Type.TextBox, Type type = Type.Nothing, string[] ListItems = null, bool ShowInTaskBar = false, Font FormFont = null)
        {

            frm.Controls.Clear();
            ResultValue = "";

            //Form definition
            frm.MaximizeBox = false;
            frm.MinimizeBox = false;
            frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            frm.Size = new System.Drawing.Size(350, 220);
            frm.Text = Title;
            frm.ShowIcon = false;
            frm.ShowInTaskbar = ShowInTaskBar;
            frm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frm_FormClosing);
            //frm.LostFocus += new frm_FormClosing;
            frm.StartPosition = FormStartPosition.CenterParent;

            //Panel definiton
            Panel panel = new Panel
            {
                Name = "panels",
                Location = new System.Drawing.Point(0, 20),
                Size = new System.Drawing.Size(340, 105),
                BackColor = System.Drawing.Color.White,
                Font = new Font("Arial", 11, FontStyle.Regular)
            };
            frm.Controls.Add(panel);

            Label lblUkupno = new Label
            {
                Name = "lblUkupno",
                Text = strukupno,
                TextAlign = ContentAlignment.TopCenter,
                Size = new System.Drawing.Size(245, 20),
                Location = new System.Drawing.Point(20, 0),
                //lblUkupno.TextAlign = ContentAlignment.MiddleLeft;
                BorderStyle = BorderStyle.None,
                Font = new Font("Arial", 11, FontStyle.Regular)
            };

            frm.Controls.Add(lblUkupno);


            TextBox txt1 = new TextBox
            {
                Name = "text1",
                TextAlign = HorizontalAlignment.Center,
                Size = new System.Drawing.Size(180, 60),
                Location = new System.Drawing.Point(90, 40),
                Font = new Font("Arial", 11, FontStyle.Regular)
            };

            panel.Controls.Add(txt1);
            //Add icon in to panel
            //panel.Controls.Add(Picture(icon));

            //Label definition (message)
            System.Windows.Forms.Label label = new System.Windows.Forms.Label
            {
                Text = Message,
                Name = "lblpretraga",
                Size = new System.Drawing.Size(245, 60),
                Location = new System.Drawing.Point(110, 0),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Arial", 11, FontStyle.Bold)
            };
            panel.Controls.Add(label);

            Button btnAdd = new Button
            {
                Location = new Point(10, 150),
                Height = 23,
                Width = 75,
                Text = "Ponisti"
            };

            btnAdd.Click += new System.EventHandler(btnAdd_Click);
               frm.Controls.Add(btnAdd);


            //Add buttons to the form
            foreach (Button btn in Btns(buttons))
                frm.Controls.Add(btn);

            //Add ComboBox or TextBox to the form
            Control ctrl = Cntrl(type, ListItems);
            ctrl.Font = new Font("Arial", 11, FontStyle.Bold);
            ctrl.BackColor = System.Drawing.Color.White;
            
            // ctrl.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            panel.Controls.Add(ctrl);

            //Get automaticly cursor to the TextBox
            if (ctrl.Name == "textBox")
                frm.ActiveControl = ctrl;

            //Set label font
            if (FormFont != null)
                label.Font = FormFont;

            frm.ShowDialog();

            //Return text value
            if (ResultFind == "ponisiti")
            {
                return DialogRes;
            }
            switch (type)
            {
                case Type.Nothing:
                    break;
                default:
                    if (DialogRes == DialogResult.OK || DialogRes == DialogResult.Yes)
                    {
                        ResultValue = ctrl.Text;
                        if (ResultFind == "") { ResultFind = ctrl.Text.Trim() + ":" + txt1.Text.Trim(); }
                    }

                    else ResultValue = "";
                    break;

               

            }
            //if (ctrl.Text.Trim() == "") { MessageBox.Show("Nije izabrana kolona"); }
            return DialogRes;
        }
        private static void btnAdd_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(frm.Controls["panels"].Controls["text1"].Text);
            frm.Controls["lblUkupno"].Text = "";
            ResultFind = "Ponisti";
            ResultValue = "";
            DialogRes = DialogResult.OK;
            frm.Close();



        }
        private static void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            ResultValue = cmb.SelectedItem.ToString();

            if (frm.Controls["lblUkupno"].Text.Contains(ResultValue) == true)
            {
                          
                MessageBox.Show("Vec je odabrana kolona: "  + ResultValue);                
                ResultValue = "";
                return;
            }
            if (frm.Controls["panels"].Controls["text1"].Text.Trim() == "")
            {
                MessageBox.Show("Ne postoji tekst za pretragu");
                return;
            }

            int count = frm.Controls["lblUkupno"].Text.Split(':').Length - 1;
            if (count < 4)

            {
                ResultFind = frm.Controls["lblUkupno"].Text + " <" + cmb.SelectedItem.ToString() + ":" + frm.Controls["panels"].Controls["text1"].Text + ">"; }
            else
            {
                ResultFind = frm.Controls["lblUkupno"].Text ;
            }
            
    //        MessageBox.Show(ResultFind);

        }
        private static void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            switch (button.Name)
            {
                case "Yes":
                    DialogRes = DialogResult.Yes;
                    break;
                case "No":
                    DialogRes = DialogResult.No;
                    break;
                case "Cancel":
                    DialogRes = DialogResult.Cancel;
                    break;
                default:
                    DialogRes = DialogResult.OK;
                    break;
            }

            frm.Close();
        }

        private static void textBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DialogRes = DialogResult.OK;
                frm.Close();
            }
        }

        private static void frm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {


        }

    
        private static Button[] Btns(Buttons button, Language lang = Language.English)
        {
            //Buttons field for return
            System.Windows.Forms.Button[] returnButtons = new Button[3];

            //Buttons instances
            System.Windows.Forms.Button OkButton = new System.Windows.Forms.Button();
            System.Windows.Forms.Button StornoButton = new System.Windows.Forms.Button();
            System.Windows.Forms.Button AnoButton = new System.Windows.Forms.Button();
            System.Windows.Forms.Button NeButton = new System.Windows.Forms.Button();

            //Set buttons names and text
            OkButton.Text = "OK"; //buttonTextArray[0];
            OkButton.Name = "OK";
            

            AnoButton.Text = "Yes";// buttonTextArray[1];
            AnoButton.Name = "Yes";

            NeButton.Text = NeButton.Name = "No"; //buttonTextArray[2];
            NeButton.Name = "No";

            StornoButton.Text = "Cancel"; //buttonTextArray[3];
            StornoButton.Name = "Cancel";

            //Set buttons position
            switch (button)
            {
                case Buttons.Ok:
                    OkButton.Location = new System.Drawing.Point(250, 150);
                    returnButtons[0] = OkButton;
                    break;

                case Buttons.OkCancel:
                    OkButton.Location = new System.Drawing.Point(170, 150);
                    returnButtons[0] = OkButton;

                    StornoButton.Location = new System.Drawing.Point(250, 150);
                    returnButtons[1] = StornoButton;
                    break;

                case Buttons.YesNo:
                    AnoButton.Location = new System.Drawing.Point(170, 101);
                    returnButtons[0] = AnoButton;

                    NeButton.Location = new System.Drawing.Point(250, 101);
                    returnButtons[1] = NeButton;
                    break;

                case Buttons.YesNoCancel:
                    AnoButton.Location = new System.Drawing.Point(90, 101);
                    returnButtons[0] = AnoButton;

                    NeButton.Location = new System.Drawing.Point(170, 101);
                    returnButtons[1] = NeButton;

                    StornoButton.Location = new System.Drawing.Point(250, 101);
                    returnButtons[2] = StornoButton;
                    break;
            }

            //Set size and event for all used buttons
            foreach (Button btn in returnButtons)
            {
                if (btn != null)
                {
                    btn.Size = new System.Drawing.Size(75, 23);
                    btn.Click += new System.EventHandler(Button_Click);
                }
            }

            return returnButtons;
        }

        private static Control Cntrl(Type type, string[] ListItems)
        {
            //ComboBox
            System.Windows.Forms.ComboBox comboBox = new System.Windows.Forms.ComboBox
            {
                Size = new System.Drawing.Size(180, 22),
                Location = new System.Drawing.Point(90, 70),
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList,                
                Name = "comboBox"
            };
            if (ListItems != null)
            {
                foreach (string item in ListItems)
                    comboBox.Items.Add(item);
                   comboBox.SelectedIndex = 0;
                comboBox.SelectedIndexChanged += new System.EventHandler(ComboBox_SelectedIndexChanged);
            }

            //Textbox
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox
            {
                Size = new System.Drawing.Size(180, 23),
                Location = new System.Drawing.Point(90, 70)
            };
            textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(textBox_KeyDown);
            textBox.Name = "textBox";

            //Set returned Control
            Control returnControl = new Control();

            switch (type)
            {
                case Type.ComboBox:
                    returnControl = comboBox;
                    break;
                case Type.TextBox:
                    returnControl = textBox;
                    break;
            }

            return returnControl;
        }

        
        public static void SetLanguage(Language lang)
        {
            switch (lang)
            {
                case Language.Cirilica:
                    buttonTextArray = "ОК,Да,Не,Поништи".Split(',');
                    break;
                case Language.Latinica:
                    buttonTextArray = "OK,Yes,No,Cancel".Split(',');
                    break;
                case Language.English:
                    buttonTextArray = "OK,Yes,No,Cancel".Split(',');
                    break;
                
                default:
                    buttonTextArray = "OK,Yes,No,Cancel".Split(',');
                    break;
            }
        }
    }
}
