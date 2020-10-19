using System.Drawing;
using System.Windows.Forms;

public static class Prompt
{
    public static string ShowDialog(string text, string caption,string lbltext)
    {
        Form prompt = new Form()
        {
            MaximizeBox = false,
            MinimizeBox = false,        
            Width = 350,
            Height = 200,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = caption,            

        StartPosition = FormStartPosition.CenterScreen};
        Label textLabel = new Label() { Left = 30, Top = 10, Text = text, Width = 270,Height=50 };
        textLabel.Font = new Font("Tahoma", 10.0f, FontStyle.Regular);
        TextBox textBox = new TextBox() { Left = 30, Top = 60, Width = 270 };
        textBox.Font = new Font("Tahoma", 10.0f, FontStyle.Bold);
        textBox.Text = text;
        Button confirmation = new Button() { Text = "Ok", Left = 30, Width = 100, Top = 100, DialogResult = DialogResult.OK };
        Button confirmationc = new Button() { Text = "Cancel", Left = 200, Width = 100, Top = 100, DialogResult = DialogResult.Cancel };
        textLabel.Text = lbltext;
        confirmation.Click += (sender, e) => { prompt.Close(); };
        prompt.Controls.Add(textBox);
        prompt.Controls.Add(confirmation);
        prompt.Controls.Add(confirmationc);
        prompt.Controls.Add(textLabel);
        prompt.AcceptButton = confirmation;

        return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
    }
}