using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Djora 30.11.20
namespace Bankom.Model
{
    public class mKontrola
    {
        public string IME { get; set; }
        public string cIzborno { get; set; }
        public string cAlijasTabele { get; set; }
        public string cTabela { get; set; }
        public string cDokument { get; set; }
        public string ID { get; set; }
        public string cSegment { get; set; }
        public int TipKontrole { get; set; }
        public string Vrednost { get; set; }
        public string cPolje { get; set; }
        public string cTabelaVView { get; set; }
        public string Stavke { get; set; }
        public string cCaption { get; set; }
        //Djora 25.12.20
        public string cIdNaziviNaFormi { get; set; }
        public string cEnDis { get; set; }
        public string cRestrikcije { get; set; }
        public string cFormulaForme { get; set; }

    }
}
