using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Bankom.Class
{
    class clsIzvestaji
    {
        Boolean vrati = true;
        int idke = Program.idkadar;
        string Poruka = "";
        int Koliko = 0;
        string Dokument = "";
        string KojiView = "";
        string WWhere = "";
        string res = "";
        string OrderBy = "";
        // parametri za stampu
        public string parametri = "";
        public string vrednostiparametara = "";
        //
        string[] separators = new[] { "," };

        DataBaseBroker db = new DataBaseBroker();
        DataTable t = new DataTable();
        DataTable tt = new DataTable();
        Form forma = new Form();
        clsdokumentRefresh dokr = new clsdokumentRefresh();
        public void PrikazIzvestaja(string dokument)
        {
            forma = Program.Parent.ActiveMdiChild;
            Dokument = dokument;

            vrati = PripremiParametre();
            if (vrati == true)
            {
                PopuniFormuIzvestaja();
                dokr.CalculatedField(forma, Dokument, "0");
            }
            else
                MessageBox.Show(Poruka);
        }

        private void DodajOrder(string TUD)
        {
            string ssel = "SELECT distinct PoljeSaDok,Polje,OOrderBy ";
            ssel += "FROM Izvestaji WHERE OOrderBy>0 AND Izvestaj =@param0 and TabelaVView=@param1 And TUD=@param2  and len(PoljeSaDok)>2";
            Console.WriteLine(ssel);
            ssel += " order by OOrderBy ";
            t = db.ParamsQueryDT(ssel, Dokument, KojiView, TUD);
            if (t.Rows.Count > 0)
                OrderBy = " ORDER BY ";
            for (int j = 0; j < t.Rows.Count; j++)
            {
                OrderBy += t.Rows[j]["Polje"].ToString() + ",";
            }
            if (OrderBy.Trim() != "")
                OrderBy = OrderBy.Substring(0, OrderBy.Length - 1);
        }
        private void PrikaziZbirove(int mTUD)
        {
            clsFormInitialisation fi = new clsFormInitialisation();
            string ssel = " SElect AlijasPolja from RecnikPodataka Where TotalVSubtotal='S'  and Dokument =@param0" + " AND Width>0";
            t = db.ParamsQueryDT(ssel, Dokument);
            for (int k = 0; k < t.Rows.Count; k++)
            {
                var pb = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == t.Rows[k]["AlijasPolja"].ToString());
                string ssel1 = "Select sum(" + t.Rows[k]["AlijasPolja"].ToString() + ") as zbir from Unija" + Dokument + " " + WWhere;
                Console.WriteLine(ssel1);
                tt = db.ReturnDataTable(ssel1);
                if (tt.Rows.Count > 0)
                {
                    Console.WriteLine(tt.Rows[0]["zbir"].ToString());
                    if (string.IsNullOrEmpty(tt.Rows[0]["zbir"].ToString()))
                        pb.Vrednost = "0";
                    else
                        pb.Vrednost = tt.Rows[0]["zbir"].ToString();
                    pb.textBox.Text = fi.FormatirajPolje(pb.Vrednost, pb.cTip);
                    //pb.textBox.Text=string.Format("{0:###,##0.00}", tt.Rows[0]["zbir"]);

                }
            }
        }
        private Boolean PripremiParametre()
        {
            // BORKA U SSI PISALO FROM IZVESTAJ UMESTO IZVESTAJI 29.11.20
            vrati = true;
            string Delovi = "";
            string mVred = "";
            string ssi = "SELECT distinct Polje,uslov,uslovoperacija,izborno,PoljeSaDok ";
            ssi += "FROM Izvestaji WHERE TabIndex >= 0 and (Uslov=1 or Uslov=2 or Uslov=4) AND Izvestaj =@param0 ";
            ssi += "order by uslov ";
            Console.WriteLine(ssi);
            t = db.ParamsQueryDT(ssi, Dokument);//formiramo delove zadatih uslova za izvestaj       
            if (t.Rows.Count > 0)
            {
                //string pNazivDokumenta = t.Rows[j]["NazivDokumenta"].ToString().Trim();   
                for (int j = 0; j < t.Rows.Count; j++)
                {
                    //var pb = forma.Controls.OfType<Field>().FirstOrDefault(n => n.IME == t.Rows[j]["Polje"].ToString().Trim());
                    Field pb = (Field)forma.Controls[t.Rows[j]["Polje"].ToString()]; //  uzimamo kontrolu na formi                                  
                    if (pb == null) { break; }

                    if (t.Rows[j]["PoljeSaDok"].ToString() != "NazivOrg")
                    {
                        parametri += t.Rows[j]["Polje"].ToString() + ",";
                        vrednostiparametara += pb.Vrednost + ",";
                    }
                    switch (Dokument)
                    {
                        case "KontrolnikIzvestaj":
                        case "KontrolnikIzvestaj2":
                            if (t.Rows[j]["uslov"].ToString() == "1" || t.Rows[j]["uslov"].ToString() == "2")
                            {
                                if (pb.Vrednost == "")
                                {
                                }
                                else
                                {
                                    if (t.Rows[j]["UslovOperacija"].ToString().Trim().ToUpper() != "LIKE")///InStr(mRS!UslovOperacija, "Like") = 0 Then
                                    {
                                        if (string.IsNullOrEmpty(t.Rows[j]["Izborno"].ToString().Trim()))  //Not IsNull(mRS!Izborno) And mRS!Izborno <> " " Then
                                        {
                                        }
                                        else // nije ni null ni prazno    //fform.Controls("ct" + mRS!Polje).ID <> 1 Or 
                                        {
                                            if (pb.ID != "1" || pb.cPolje == "OznVal" || pb.cPolje == "NazivVlasnika")
                                                Delovi += " id_" + pb.cIzborno + t.Rows[j]["UslovOperacija"].ToString() + pb.ID + " AND ";
                                        }
                                    }
                                    else //Jeste uslov=LIKE
                                        Delovi += t.Rows[j]["PoljeSaDokumenta"].ToString() + " like '" + pb.Vrednost + "%' and ";
                                    if (t.Rows[j]["UslovOperacija"].ToString().Trim().ToUpper() == "LIKE_%")
                                        Delovi += pb.cPolje + " like '%" + pb.Vrednost + "' and ";
                                    if (t.Rows[j]["UslovOperacija"].ToString().Trim().ToUpper() == "LIKE%_%")
                                        Delovi += t.Rows[j]["Polje"] + " like '%" + pb.Vrednost + "%' and ";
                                }
                            }
                            else
                            {
                                if (t.Rows[j]["uslov"].ToString() == "4")
                                {
                                    if (t.Rows[j]["UslovOperacija"].ToString().ToUpper() == "LIKE")
                                    {
                                        if (string.IsNullOrEmpty(t.Rows[j]["Izborno"].ToString())) { }
                                        else
                                            Delovi += t.Rows[j]["PoljeSaDok"] + " like '%" + pb.Vrednost + "%' and ";
                                    }
                                    else
                                        Delovi += t.Rows[j]["PoljeSaDok"].ToString() + " " + t.Rows[j]["UslovOperacija"].ToString() + " '" + pb.Vrednost + "%' and ";
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(t.Rows[j]["Izborno"].ToString())) { }
                                    else
                                    {
                                        if (t.Rows[j]["Izborno"].ToString() == "Firma")
                                            Delovi += t.Rows[j]["PoljeSaDok"] + " " + t.Rows[j]["UslovOperacija"].ToString() + " '" + Program.imeFirme + "' and ";
                                        else
                                            Delovi += t.Rows[j]["PoljeSaDok"] + " " + t.Rows[j]["UslovOperacija"].ToString() + " '" + t.Rows[j]["Izborno"] + "' and ";
                                    }
                                }
                            }
                            break;
                        default:
                            if (pb.Vrednost.Trim() != "" || pb.IME == "NazivVlasnika") // vrednost ima sadrzaj
                            {
                                if (t.Rows[j]["uslov"].ToString() == "1" || t.Rows[j]["uslov"].ToString() == "2")
                                {
                                    if (t.Rows[j]["UslovOperacija"].ToString().ToUpper().Contains("LIKE") == false)// ne sadrzi LIKE
                                    {

                                        if (string.IsNullOrEmpty(t.Rows[j]["Izborno"].ToString())) // ne postoji izborno
                                            Delovi += t.Rows[j]["PoljeSaDok"].ToString().Trim() + " " + t.Rows[j]["UslovOperacija"].ToString().Trim() + " '" + pb.Vrednost + "' and ";
                                        else
                                        {
                                            if (pb.ID != "1" || pb.IME == "OznVal" || pb.IME == "NazivVlasnika")
                                                Delovi += " id_" + t.Rows[j]["Izborno"].ToString().Trim() + t.Rows[j]["UslovOperacija"].ToString().Trim() + pb.ID + " and ";
                                            else
                                                Delovi += t.Rows[j]["PoljeSaDok"].ToString().Trim() + " " + t.Rows[j]["UslovOperacija"].ToString().Trim() + " '" + pb.Vrednost + "' and ";
                                        }
                                    }
                                    else // sadrzi LIKE
                                    {
                                        // ovde sam htela da uzmem pb.comboBox.Text odnosi se na magacinsku karticu
                                        Console.WriteLine(pb.comboBox.Text);
                                        if (pb.cTip == 25 && pb.Vrednost.Trim() != "" && pb.Vrednost.Contains(",") == true)
                                            mVred = pb.Vrednost.Substring(0, pb.Vrednost.IndexOf(","));
                                        else
                                            mVred = pb.Vrednost;

                                        if (t.Rows[j]["UslovOperacija"].ToString().Trim() == "LIKE")
                                            Delovi = t.Rows[j]["PoljeSaDok"].ToString() + " like '" + mVred + "' and ";
                                        if (t.Rows[j]["UslovOperacija"].ToString() == "LIKE '_'")
                                            Delovi = t.Rows[j]["PoljeSaDok"].ToString() + " like '" + mVred + "' and ";
                                        if (t.Rows[j]["UslovOperacija"].ToString() == "LIKE '_%'")
                                            Delovi += t.Rows[j]["PoljeSaDok"].ToString() + " like '" + mVred + "%' and ";
                                        if (t.Rows[j]["UslovOperacija"].ToString() == "LIKE '%_'")
                                            Delovi += t.Rows[j]["PoljeSaDok"] + " like '%" + mVred + "' and ";
                                        if (t.Rows[j]["UslovOperacija"].ToString() == "LIKE '%_%'")
                                            Delovi += t.Rows[j]["PoljeSaDok"].ToString() + " like '%" + mVred + "%' and ";
                                    }
                                }
                                else // uslov nije 1 ili 2
                                {
                                    if (t.Rows[j]["uslov"].ToString() == "4")
                                    {
                                        if (t.Rows[j]["UslovOperacija"].ToString().ToUpper() == "LIKE")
                                        {
                                            if (string.IsNullOrEmpty(t.Rows[j]["Izborno"].ToString())) { }
                                            else
                                                Delovi += t.Rows[j]["PoljeSaDok"].ToString() + " " + t.Rows[j]["UslovOperacija"].ToString().Trim() + " '" + t.Rows[j]["Izborno"].ToString() + "%' and ";
                                        }
                                        else
                                            if (string.IsNullOrEmpty(t.Rows[j]["Izborno"].ToString())) { }
                                        else
                                            Delovi += t.Rows[j]["PoljeSaDok"].ToString() + " " + t.Rows[j]["UslovOperacija"].ToString().Trim() + " '" + Program.imeFirme + "' and ";
                                    }
                                }

                            } // KRAJ vrednost ima sadrzaj
                            break;
                    }
                }
                if (Delovi.Length > 0)
                {
                    Delovi = Delovi.Substring(0, Delovi.Length - 4);
                    parametri = parametri.Substring(0, parametri.Length - 1);
                    Console.WriteLine(Delovi);
                    vrednostiparametara = vrednostiparametara.Substring(0, vrednostiparametara.Length - 1);
                    // sacuvamo podatke za stampu izvestaja
                    Program.param = parametri;
                    Program.vred = vrednostiparametara;
                }
            }
            if (forma.Text == "Finansijska kartica bez zatvaranja - online")
            {
                if (Delovi.Trim() != "")
                    WWhere = " Where " + Delovi + " AND Opisknj Not Like 'Zatvaranje%'";
                else
                      if (Delovi.Trim() != "")
                    WWhere = " Where " + Delovi;
            }
            else
            {
                if (Delovi.Trim() != "")
                    WWhere = " Where " + Delovi;
            }
            Program.WWhere = WWhere;
            Console.WriteLine(WWhere);
            return vrati;
        }
        private Boolean PopuniFormuIzvestaja()
        {
            clsObradaOsnovnihSifarnika os = new clsObradaOsnovnihSifarnika();
            vrati = true;
            string mUpit = "";
            string uUpit = "";
            string mGroup = "";
            string mTUD = "";
            string mIme = "";
            string iddok = "0";
            string rss = "";
            clsdokumentRefresh dokr = new clsdokumentRefresh();

            string rsu = "Select * From Upiti Where  nazivdokumenta=@param0 And ime  like '%ggrr%' ORDER BY TUD";
            Console.WriteLine(rsu);
            DataTable tt = db.ParamsQueryDT(rsu, Dokument);
            for (int j = 0; j < tt.Rows.Count; j++)
            {
                res = "";
                mGroup = "";
                OrderBy = "";
                mTUD = tt.Rows[j]["TUD"].ToString();
                uUpit = tt.Rows[j]["Upit"].ToString();
                mIme = tt.Rows[j]["Ime"].ToString();
                KojiView = mIme.Replace("GgRr", "");
                if (uUpit.ToUpper().Contains("GROUP") == true)
                {
                    mGroup = uUpit.Substring(uUpit.ToUpper().IndexOf("GROUP"), uUpit.Length - uUpit.ToUpper().IndexOf("GROUP"));
                    mUpit = uUpit.Substring(0, uUpit.ToUpper().IndexOf("GROUP") - 1);
                }
                else
                {
                    mGroup = "";
                    mUpit = uUpit;
                }
                if (Convert.ToInt32(mTUD) > 0)
                {
                    res = os.DodajRestrikcije(Dokument, mTUD);
                    if (WWhere.Trim() != "")
                    {
                        if (res.Trim() != "")
                            res = " AND " + res;
                    }
                    else
                    {
                        if (res.Trim() != "")
                            res = " WHERE " + res;
                    }
                    DodajOrder(mTUD);
                    mUpit += WWhere + res + mGroup + OrderBy;
                    Console.WriteLine(mUpit);
                    DataTable t = new DataTable();
                    t = db.ReturnDataTable(mUpit);
                    if (t.Rows.Count == 0)
                    {
                        MessageBox.Show("Ne postoje podaci za zadate parametre!!!");
                        vrati = false;
                        return vrati;
                    }
                    dokr.refreshDokumentGrid(forma, Dokument, "1", mUpit, mTUD, "I");

                }
                else
                {
                    Console.WriteLine(mUpit);
                    dokr.refreshDokumentBody(forma,Dokument, iddok, "I");//BORKA DA LI NAM OVO TREBA 29.11.20

                }
            }
            PrikaziZbirove(0);
            return vrati;
        }
    }
}


