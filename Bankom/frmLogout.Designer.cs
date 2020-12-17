namespace Bankom
{
    partial class frmLogout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnOdjava = new System.Windows.Forms.Button();
            this.btnNe = new System.Windows.Forms.Button();
            this.btnDa = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("TimesRoman", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.label1.Location = new System.Drawing.Point(12, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(366, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Da li ste sigurni da želite da ugasite program?";
            // 
            // btnOdjava
            // 
            this.btnOdjava.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.btnOdjava.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOdjava.Font = new System.Drawing.Font("TimesRoman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOdjava.ForeColor = System.Drawing.Color.Snow;
            this.btnOdjava.Location = new System.Drawing.Point(12, 131);
            this.btnOdjava.Name = "btnOdjava";
            this.btnOdjava.Size = new System.Drawing.Size(75, 23);
            this.btnOdjava.TabIndex = 1;
            this.btnOdjava.Text = "Odjava";
            this.btnOdjava.UseVisualStyleBackColor = false;
            this.btnOdjava.Click += new System.EventHandler(this.btnOdjava_Click);
            // 
            // btnNe
            // 
            this.btnNe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.btnNe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNe.Font = new System.Drawing.Font("TimesRoman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNe.ForeColor = System.Drawing.Color.Snow;
            this.btnNe.Location = new System.Drawing.Point(149, 131);
            this.btnNe.Name = "btnNe";
            this.btnNe.Size = new System.Drawing.Size(75, 23);
            this.btnNe.TabIndex = 2;
            this.btnNe.Text = "Ne";
            this.btnNe.UseVisualStyleBackColor = false;
            this.btnNe.Click += new System.EventHandler(this.btnNe_Click);
            // 
            // btnDa
            // 
            this.btnDa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.btnDa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDa.Font = new System.Drawing.Font("TimesRoman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDa.ForeColor = System.Drawing.Color.Snow;
            this.btnDa.Location = new System.Drawing.Point(282, 131);
            this.btnDa.Name = "btnDa";
            this.btnDa.Size = new System.Drawing.Size(75, 23);
            this.btnDa.TabIndex = 3;
            this.btnDa.Text = "Da";
            this.btnDa.UseVisualStyleBackColor = false;
            this.btnDa.Click += new System.EventHandler(this.btnDa_Click);
            // 
            // frmLogout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(384, 178);
            this.Controls.Add(this.btnDa);
            this.Controls.Add(this.btnNe);
            this.Controls.Add(this.btnOdjava);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLogout";
            this.Text = "frmLogout";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOdjava;
        private System.Windows.Forms.Button btnNe;
        private System.Windows.Forms.Button btnDa;
    }
}