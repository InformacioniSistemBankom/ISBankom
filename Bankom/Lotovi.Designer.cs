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
            this.Insert.Location = new System.Drawing.Point(98, 35);
            this.Insert.Name = "Insert";
            this.Insert.Size = new System.Drawing.Size(100, 34);
            this.Insert.TabIndex = 1;
            this.Insert.Text = "Unos";
            this.Insert.UseVisualStyleBackColor = true;
            this.Insert.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(224, 35);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(108, 34);
            this.button2.TabIndex = 2;
            this.button2.Text = "Izmena";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(350, 35);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(87, 34);
            this.button3.TabIndex = 3;
            this.button3.Text = "Brisanje";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(453, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 34);
            this.button1.TabIndex = 4;
            this.button1.Text = "Pregled";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // pretraga
            // 
            this.pretraga.Location = new System.Drawing.Point(1126, 49);
            this.pretraga.Name = "pretraga";
            this.pretraga.Size = new System.Drawing.Size(226, 22);
            this.pretraga.TabIndex = 5;
            this.pretraga.TextChanged += new System.EventHandler(this.pretraga_TextChanged);
            this.pretraga.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.pretraga_KeyPress);
            // 
            // dataGridViewPaging1
            // 
            this.dataGridViewPaging1.AutoHideNavigator = false;
            this.dataGridViewPaging1.AutoSize = true;
            this.dataGridViewPaging1.DataSource = null;
            this.dataGridViewPaging1.Location = new System.Drawing.Point(98, 91);
            this.dataGridViewPaging1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridViewPaging1.MaxRecords = 27;
            this.dataGridViewPaging1.Name = "dataGridViewPaging1";
            this.dataGridViewPaging1.Size = new System.Drawing.Size(1254, 610);
            this.dataGridViewPaging1.TabIndex = 6;
            this.dataGridViewPaging1.Load += new System.EventHandler(this.dataGridViewPaging1_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1033, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Pretraga:";
            // 
            // Lotovi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1346, 800);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewPaging1);
            this.Controls.Add(this.pretraga);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Insert);
            this.Name = "Lotovi";
            this.Text = "Lotovi";
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