using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bankom
{
    class clsOperacije
    {      
        public bool IsNumeric(string input)
        {
            return float.TryParse(input, NumberStyles.Number, CultureInfo.CreateSpecificCulture("sr-Lat"), out float test);
        }
        public bool IsDateTime(string text)
        {
            DateTime dateTime;
            bool isDateTime = true;

            // Check for empty string.
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            isDateTime = DateTime.TryParse(text, out dateTime);

            return isDateTime;
        }

        public bool Prestupna(int godina)
        {
            if (godina % 100 == 0)
            {
                if (godina % 400 == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (godina % 4 == 0)
            {
                return true;
            }
            return false;
        }
        public string AsciiKarakteri(string input)
        {
            input = input.Replace("[", "&a&");
            input = input.Replace("]", "&b&");
            input = input.Replace("{", "&c&");
            input = input.Replace("|", "&d&");
            input = input.Replace("`", "&e&");
            input = input.Replace("~", "&f&");
            input = input.Replace("@", "&g&");
            //input = input.Replace("\", " & h & ");
            input = input.Replace("|", "&i&");
            input = input.Replace("^", "&j&");


            input = input.Replace("&a&", "[[]");
            input = input.Replace("&b&", "[]]");
            input = input.Replace("&c&", "[{]");
            input = input.Replace("&d&", "[|]");
            input = input.Replace("&e&", "[`]");
            input = input.Replace("&f&", "[~]");
            input = input.Replace("&g&", "[@]");
            //WWhere = Replace(WWhere, "&h&", "[\]")
            input = input.Replace("&i&", "[|]");
            input = input.Replace("&j&", "[^]");

            return input;
        }

        public string CitajIniFajl(string kluc, string trazi)
        {
            string vrati = "";
            string trazenarec = "";
            string fileReader;
            fileReader = File.ReadAllText(Application.StartupPath + @"\XmlLat\xxxx.ini");

            string[] separators11 = new[] { "[", "]" };
            string[] words = fileReader.Split(separators11, StringSplitOptions.RemoveEmptyEntries);
            for (int n = 0; n < words.Length; n++)
            {
                string cc = words[n];


                if (trazenarec != "")
                {
                    string[] lines = cc.Split(new[] { "\r\n", "=" }, StringSplitOptions.None);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].ToLower() == trazi.ToLower())
                        {
                            vrati = lines[i + 1];
                            break;
                        }
                    }

                }

                if (vrati != "") { break; }

                if (cc.ToLower() == kluc.ToLower())
                {
                    trazenarec = cc;


                }


            }

            return vrati;

        }

    }
}
