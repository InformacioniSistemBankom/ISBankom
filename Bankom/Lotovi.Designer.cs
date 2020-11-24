namespace Bankom
{
    partial class Lotovi
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
            this.Insert = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pretraga = new System.Windows.Forms.TextBox();
            this.dataGridViewPaging1 = new In.Sontx.SimpleDataGridViewPaging.DataGridViewPaging();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Insert
            // 
            this.Insert.FlatAppearance.BorderSize = 3;
            this.Insert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Insert.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Insert.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.Insert.Location = new System.Drawing.Point(76, 27);
            this.Insert.Margin = new System.Windows.Forms.Padding(2);
            this.Insert.Name = "Insert";
            this.Insert.Size = new System.Drawing.Size(90, 33);
            this.Insert.TabIndex = 1;
            this.Insert.Text = "Unos";
            this.Insert.UseVisualStyleBackColor = true;
            this.Insert.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderSize = 3;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.button2.Location = new System.Drawing.Point(170, 27);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 33);
            this.button2.TabIndex = 2;
            this.button2.Text = "Izmena";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.FlatAppearance.BorderSize = 3;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.button3.Location = new System.Drawing.Point(264, 27);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(102, 33);
            this.button3.TabIndex = 3;
            this.button3.Text = "Brisanje";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 3;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.button1.Location = new System.Drawing.Point(370, 27);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 33);
            this.button1.TabIndex = 4;
            this.button1.Text = "Pregled";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // pretraga
            // 
            this.pretraga.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pretraga.ForeColor = System.Drawing.Color.Black;
            this.pretraga.Location = new System.Drawing.Point(763, 31);
            this.pretraga.Margin = new System.Windows.Forms.Padding(2);
            this.pretraga.Name = "pretraga";
            this.pretraga.Size = new System.Drawing.Size(251, 24);
            this.pretraga.TabIndex = 5;
            this.pretraga.TextChanged += new System.EventHandler(this.pretraga_TextChanged);
            this.pretraga.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.pretraga_KeyPress);
            // 
            // dataGridViewPaging1
            // 
            this.dataGridViewPaging1.AutoHideNavigator = false;
            this.dataGridViewPaging1.AutoSize = true;
            this.dataGridViewPaging1.BackColor = System.Drawing.Color.Snow;
            this.dataGridViewPaging1.DataSource = null;
            this.dataGridViewPaging1.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewPaging1.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewPaging1.Location = new System.Drawing.Point(74, 74);
            this.dataGridViewPaging1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.dataGridViewPaging1.MaxRecords = 27;
            this.dataGridViewPaging1.Name = "dataGridViewPaging1";
            this.dataGridViewPaging1.Size = new System.Drawing.Size(940, 502);
            this.dataGridViewPaging1.TabIndex = 6;
            this.dataGridViewPaging1.Load += new System.EventHandler(this.dataGridViewPaging1_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("TimesRoman", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(107)))), ((int)(((byte)(167)))));
            this.label1.Location = new System.Drawing.Point(680, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "Pretraga:";
            // 
            // Lotovi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Snow;
            this.ClientSize = new System.Drawing.Size(1096, 657);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewPaging1);
            this.Controls.Add(this.pretraga);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Insert);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Lotovi";
            this.Text = "Lotovi";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Lotovi_FormClosed);
            this.Load += new System.EventHandler(this.Lotovi_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Button Insert;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.TextBox pretraga;
    public In.Sontx.SimpleDataGridViewPaging.DataGridViewPaging dataGridViewPaging1;
    private System.Windows.Forms.Label label1;
}
}