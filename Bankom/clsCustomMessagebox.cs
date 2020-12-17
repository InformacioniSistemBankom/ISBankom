using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace Bankom
{
    public class clsCustomMessagebox : System.Windows.Forms.Form
    {

        System.Windows.Forms.Label message = new System.Windows.Forms.Label();
            public Button b1 = new Button();
            public Button b2 = new Button();
            public Button b3 = new Button();


            public clsCustomMessagebox(string body, string button1, string button2, string button3)
            {


                this.ClientSize = new System.Drawing.Size(490, 150);

                b1.Location = new System.Drawing.Point(311, 112);
                b1.Size = new System.Drawing.Size(75, 23);
                b1.Text = button1;
                b1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
                b1.Font = new System.Drawing.Font("TimesRoman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                b1.ForeColor = System.Drawing.Color.Snow;
                b1.DialogResult = DialogResult.OK;

                b2.Location = new System.Drawing.Point(211, 112);
                b2.Size = new System.Drawing.Size(75, 23);
                b2.Text = button2;
                b2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
                b2.Font = new System.Drawing.Font("TimesRoman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                b2.ForeColor = System.Drawing.Color.Snow;
                b2.DialogResult = DialogResult.Cancel;


                b3.Location = new System.Drawing.Point(111, 112);
                b3.Size = new System.Drawing.Size(75, 23);
                b3.Text = button3;
                b3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
                b3.Font = new System.Drawing.Font("TimesRoman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                b3.ForeColor = System.Drawing.Color.Snow;
                b3.DialogResult = DialogResult.Retry;

                message.Location = new System.Drawing.Point(30, 30);
                message.Text = body;
                message.Font = new System.Drawing.Font("TimesRoman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                message.AutoSize = true;
                message.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));

                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.ShowIcon = false;
                this.Text = "";
                this.BackColor = System.Drawing.Color.White;
                this.ShowIcon = false;

                this.Controls.Add(b1);
                this.Controls.Add(b2);
                this.Controls.Add(b3);
                this.Controls.Add(message); 
            }
        }
    }

