using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Djora 30.11.20
namespace Bankom.Model
{
    public class mForma
    {
        public string OOperacija { get; set; }
        public int lidstablo { get; set; }
        public string ldokje { get; set; }
        public string limegrida { get; set; }
        public string iddokumenta { get; set; }
        public int idreda { get; set; }
        public string imedokumenta { get; set; }

        public List<mKontrola> kontrole { get; set; } = new List<mKontrola>();

    }
}