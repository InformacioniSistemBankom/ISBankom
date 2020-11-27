using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;


namespace Bankom.Class
{



    class DataBaseBroker
    {
        //Djora 14.06.17
      public string connstring = Program.connectionString;///@"Server = ""bankomw""; Database =""Bankom""; User Id = ""sa""; Password = ""password"";";
        //private string connstring = @"Server = ""Sql2016""; Database =""dbbbTestNew2003Bankom""; User Id = ""sa""; Password = ""password"";";

        // public static string connstring = "Data Source=DESKTOP-540V69B; Initial Catalog = dbbbTestNew2003Bankom; User Id=sa;password=tanjug; ";
        //zika
        //kikivrati = db.ExecuteSqlTransaction(lista);
        public DataTable ReturnDataTableWithParam(List<string[]> lista)
        {
            //listu cine dva inputa
            // queru , parametri 
            string input = "";
            char[] delimit = { '=' };
            string param = "";
            string pattern = @"\[([^\[\]]+)\]";
            string sql = "";
            //if (lista.Count == 0) { return "Greska"; }
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(connstring);
            if (con.State == ConnectionState.Closed) { con.Open(); }
            input = lista[0][0]; //query
            string query = input;

            try
            {
                SqlCommand cmd = new SqlCommand(sql, Program.con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception e)
            {
                //conn.Close();
                //   conn.Dispose();
            }
            finally
            {
                con.Close();
                // conn.Dispose();
            }
            return dt;
        }
        public DataTable ReturnDataTable(string sql)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connstring);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }
            Console.WriteLine(sql);
            try
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        public DataSet ReturnDS(string str)
        {
            // vraca dataset
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(connstring);

            try
            {

                if (con.State == ConnectionState.Closed) { con.Open(); }
                using (SqlDataAdapter da = new SqlDataAdapter(str, con)) { da.Fill(ds); }
                con.Close();
                return ds;
            }

            catch (Exception ex)
            {


                MessageBox.Show(ex.Message);
                if (con != null) { ((IDisposable)con).Dispose(); }

                return null;
            }

        }
        public SqlDataReader ReturnDataReader(string sql)
        {
            Console.WriteLine(sql);
            SqlConnection con = new SqlConnection(connstring);
            SqlCommand Cmd = new SqlCommand();
            try
            {

                if (con.State == ConnectionState.Closed) { con.Open(); }
                Cmd.Connection = con;
                Cmd = con.CreateCommand();
                Cmd.CommandText = sql;
                SqlDataReader Reader = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return Reader;

            }
            catch (Exception ex)
            {
                con.Close();
                con.Dispose();
                throw ex;
            }
            finally
            {
                Cmd = null;
            }
        }
        public string Comanda(SqlCommand cmd)
        {

            SqlConnection conn = new SqlConnection(connstring);
            cmd.Connection = conn;
            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();
            }
            //Djora 17.01.17
            catch (SqlException odbcEx)
            {
                return odbcEx.Number.ToString();
            }
            catch (Exception e)
            {
                conn.Close();
                conn.Dispose();

                return e.Message;

            }
            finally
            {

                conn.Close();
                conn.Dispose();

            }
            return "";

        }

        
        

        public int ReturnInsertUpdate(string str, int izbor)
        {
            // 0-insert;
            //1 -update
            SqlConnection conn = new SqlConnection(connstring);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = str;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            int retVal = -1;  // vraca 1 ako nema greske!
            try
            {
                retVal = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return retVal;


        }

        public int ReturnInt(string str, int intkolona)
        {
            Console.WriteLine(str);
            int intstr = -1;
            SqlConnection conn = new SqlConnection(connstring);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = str;
                Console.WriteLine(str);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        try

                        {
                            intstr = Convert.ToInt32(reader[intkolona]);
                        }
                        catch
                        {
                            return -1;
                        }

                        return intstr;
                    }
                    return -1;
                }
            }
        }

        public string ReturnString(string str, int intkolona) // vraca string
        {

            SqlConnection conn = new SqlConnection(Program.connectionString);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }


            using (var cmd = conn.CreateCommand())
            {

                cmd.CommandText = str;
                cmd.CommandType = CommandType.Text;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        str = Convert.ToString(reader[intkolona]);
                        return str;
                    }
                    return null;
                }
            }

        }

        public string VratiTipKolone(string strTabela, string imekolone)
            {
            
            SqlConnection conn = new SqlConnection(Program.connectionString);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            string kolona="";
            string tabela = strTabela;
            string input = imekolone;
            string SqlCmd = "select COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE, ";
            SqlCmd += " DATETIME_PRECISION, IS_NULLABLE from INFORMATION_SCHEMA.COLUMNS";
            SqlCmd += " where TABLE_NAME = '" + tabela + "' and COLUMN_NAME = '" + imekolone + "'";
            
            SqlCommand Cmd = new SqlCommand();
            Cmd.Connection = conn;
            Cmd.CommandType = CommandType.Text;
            Cmd.CommandText = SqlCmd;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                DataSet ds = new DataSet();
                DataView dv = new DataView();
                sda.SelectCommand = Cmd;
                sda.Fill(ds);
                dv = ds.Tables[0].DefaultView;
             
                dv.RowFilter = "";
                dv.RowFilter = "COLUMN_NAME='" + imekolone + "'";
                kolona = dv[0][1].ToString();
            }
            return kolona;
        }

        public string ReturnSqlTransactionParamsFull(List<string[]> lista)
        {
            //listu cini pet inputa
            // queru , parametri , ime tabele,doktype i IDDokumenta
            // sve kolone  moraju da budu uokvirene karakterima []  npr: [Datum]
            // doktype moze imati sledece vrednosti S sledbenik;P prazan dokument; D vrace se ID insertovanog sloga
            if (lista.Count == 0) { return "Greska"; }

            SqlConnection conn = new SqlConnection(connstring);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            int ctr = 0;
            string Query = "";
            char[] delimit = { '`' };
            char[] delimit1 = { '=' };
            string kljuc = "";
            string vrednost = "";
            string rezultat = "";
            string name = "";
            string strTabela = lista[0][2].ToString();
            string spisakKolonaUluz = "";
            string spisakKolonaDB = "";
           string[] spisakKolona = null;
            int idPrazan = 1;
            int idSled = 1;
            int newID = 0;
            int rowsAffected = 0;
            string pattern = @"\[([^\[\]]+)\]";
            string input;
            string SqlCmd = "";
            bool isUpdated = false;
            string IdDokument = "";
            string doctype = "";
            using (SqlCommand commandSql = new SqlCommand())
            {
                SqlTransaction transaction;
                transaction = conn.BeginTransaction("trTransaction");
                commandSql.Connection = conn;
                commandSql.Transaction = transaction;
              

                for (int kk = 0; kk < lista.Count; kk++)
                {
                    input = lista[kk][0]; //query
                    Console.WriteLine(input);
                    strTabela = lista[kk][2].ToString(); //tabela
                    doctype = lista[kk][3].ToString(); //tip dokumenta 
                    IdDokument = lista[kk][4].ToString();
                    if (input.ToUpper().Contains("EXECUTE") == true)
                    {
                        commandSql.CommandType = CommandType.Text;
                        if (input.Contains("ssss") == true) input = input.Replace("'ssss'", idSled.ToString());
                        if (input.Contains("pppp") == true) input = input.Replace("'pppp'", idPrazan.ToString());
                        if (input.Contains("tttt") == true) input = input.Replace("'tttt'", newID.ToString());
                        if (input.Contains("stanje") == true)
                        {
                            if(IdDokument.Trim()=="") IdDokument= idSled.ToString();
                            clsProveraStanja ps = new clsProveraStanja();
                            rezultat = ps.ProveriStanje(strTabela, IdDokument, ref conn, ref transaction);

                            if (rezultat.Trim() == "") isUpdated = true;
                            else isUpdated = false;
                            if (kk == lista.Count - 1) goto KrajTransakcije;
                        }

                        commandSql.CommandText = input;
                        try
                        {
                            rowsAffected = commandSql.ExecuteNonQuery();
                             if (rowsAffected > 0)
                            {
                                isUpdated = true;
                            }
                        }
                        catch (SqlException ex)
                        {
                            rezultat = input + Environment.NewLine + ((System.Data.SqlClient.SqlException)ex).Number.ToString() + "-" + ex.Message.ToString();
                            transaction.Rollback();
                            return rezultat;
                        }
                    }
                    else//input ne sadrzi execute
                    {
                        spisakKolonaUluz = "";
                        spisakKolonaDB = "";

                        foreach (System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(input, pattern))
                        {
                            string polje = m.Groups[1].Value;
                            spisakKolonaUluz += (polje + "#");
                            spisakKolonaDB += "'" + polje + "',";

                            ++ctr;
                        }
                        spisakKolona = spisakKolonaUluz.Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                        spisakKolonaDB = spisakKolonaDB.Trim().Substring(0, spisakKolonaDB.Length - 1);
                        DataBaseBroker db = new DataBaseBroker();
                        SqlCmd = "select COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE, ";
                        SqlCmd += " DATETIME_PRECISION, IS_NULLABLE from INFORMATION_SCHEMA.COLUMNS";
                        SqlCmd += " where TABLE_NAME = '" + strTabela + "' and COLUMN_NAME in  (" + spisakKolonaDB + ")";

                        Console.WriteLine(SqlCmd);

                        DataTable dt = db.ReturnDataTable(SqlCmd);                    
                        DataView dv = dt.DefaultView;

                        string[] delovi = lista[kk][1].Split(delimit);
                        commandSql.Parameters.Clear();
                        int k = 0;
                        foreach (string polje in spisakKolona)
                        {
                            dv.RowFilter = "";
                            dv.RowFilter = "COLUMN_NAME='" + polje + "'";
                            kljuc = delovi[k].Split(delimit1)[0].ToString();
                            name = dv[0][1].ToString();
                            SqlDbType sqlType = (SqlDbType)Enum.Parse(typeof(SqlDbType), name, true);
                            vrednost = delovi[k].Split(delimit1)[1].ToString();
                            commandSql.Parameters.Add(kljuc, sqlType).Value = vrednost;
                            k++;
                        }
                        Query = lista[kk][0];
                        commandSql.CommandType = CommandType.Text; //CommandType.StoredProcedure
                        commandSql.CommandText = Query;

                        if (lista[kk][0].ToUpper().Contains("INSERT") == true )
                        {
                            commandSql.CommandText += " SELECT CAST(scope_identity() as int) ";
                            try
                            {                               
                                    newID = (int)commandSql.ExecuteScalar();
                                    if (lista[kk][3] == "S") idSled = newID;
                                    if (lista[kk][3] == "P") idPrazan = newID;
                              
                                if (lista[kk][3] == "D")
                                {
                                    IdDokument= Convert.ToString(newID);
                                    rezultat = Convert.ToString(newID); 
                                    isUpdated = true;
                                    //transaction.Commit();
                                    //return rezultat;
                                }
                            }
                            catch (SqlException ex)
                            {
                                rezultat =  Environment.NewLine + ((System.Data.SqlClient.SqlException)ex).Number.ToString() + "-" + ex.Message.ToString();
                                transaction.Rollback();
                                return rezultat;
                            }

                        }
                        else // input ne sadrzi INSERT
                        {
                            commandSql.CommandText = Query;

                            try
                            {
                                rowsAffected = commandSql.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    isUpdated = true;
                                }
                            }
                            catch (SqlException ex)
                            {
                                rezultat = ex.Message.ToString();
                                transaction.Rollback();
                                return rezultat;
                            }
                        }

                    }

                }
KrajTransakcije:
                if (isUpdated == true)
                    transaction.Commit();
                else
                    transaction.Rollback();
                //return rezultat;                
            }
            return rezultat;
        }
        public string ReturnSqlTransactionParams(List<string[]> lista)
        {
          
            //primer poziva  query sa parametrima ,strparams niz parametri sa vrednoscu
            //  lista.Add(new string[] { str, strParams });              
            ///...
            //lista.ToArray();
            //rezultat = db.ReturnSqlTransactionParams(lista);

            if (lista.Count == 0) { return "Greska"; }
            
            SqlConnection conn = new SqlConnection(connstring);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }


            string Query = "";
            char[] delimit = { '`' };
            char[] delimit1 = { '=' };
            string kljuc = "";
            string vrednost = "";
            string rezultat = "";
            int rowsAffected = 0;
            bool isUpdated = false;

            using (SqlCommand commandSql = new SqlCommand())
            {
                SqlTransaction transaction;
                transaction = conn.BeginTransaction("trTransaction");
                commandSql.Connection = conn;
                commandSql.Transaction = transaction;
                try
                {
                    for (int i = 0; i < lista.Count; i++)
                    {

                        string[] delovi = lista[i][1].Split(delimit);
                        commandSql.Parameters.Clear();
                        for (int k = 0; k < delovi.Length; k++)
                        {
                            kljuc = delovi[k].Split(delimit1)[0].ToString();

                            
                                kljuc = delovi[k].Split(delimit1)[0].ToString();
                                vrednost = delovi[k].Split(delimit1)[1].ToString();
                                commandSql.Parameters.AddWithValue(kljuc, vrednost);
                            
                        }
                        Query = lista[i][0];
                        commandSql.CommandType = CommandType.Text;
                        commandSql.CommandText = Query;
                        rowsAffected = commandSql.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            isUpdated = true;
                        }
                    }
                    transaction.Commit();
                }

                catch (SqlException ex)
                {
                    rezultat =Query + Environment.NewLine +  ((System.Data.SqlClient.SqlException)ex).Number.ToString() + "-" + ex.Message.ToString();
                    transaction.Rollback();

                    return rezultat;

                }

                //}

                return rezultat;
            }

        }
        


        public DataSet ReturnStoreProcedure(string imeprocedure, string[] kolone, string tipovi)
        {

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(connstring);
            if (conn.State == ConnectionState.Closed) { conn.Open(); }

            switch (imeprocedure)
            {
                case "":
                    break;
            }

            return ds;
        }

        //Djora 26.09.19
        //Primer koriscenja: 
        //DataBaseBroker db22 = new DataBaseBroker();
        //DataSet ss = db22.ParamsQueryDS("INSERT INTO CUSTOMERS(ID,NAME,AGE,ADRESS,COUNTRY) VALUES(@param0,@param1,@param2,@param3,@param4)",5,"Ali",27,"my City","England");
        //DataSet ss = db22.ParamsQueryDS("select nazivArtikla from artikli where id_artikli=@param0", 5);
        //Primer sa Like klauzulom: DataSet ss = db22.ParamsQueryDS("SELECT NazivArtikla FROM dbo.Artikli WHERE(NazivArtikla like @param0) AND(ID_ArtikliStablo = @param1)", "%ban%", 1440);
        //Primer bez parametara: DataSet ss = db22.ParamsQueryDT("SELECT NazivArtikla FROM dbo.Artikli");
        public DataSet ParamsQueryDS(string query, params object[] args)
        {
            try
            {
                var con = new SqlConnection(Program.connectionString);
                using (con)
                {
                    con.Open();
                    var cmd = new SqlCommand(query, con);
                    //if (sqlParams != null)
                    //{
                    for (int i = 0; i < args.Length; i++)
                        cmd.Parameters.AddWithValue("@param" + i, args[i]);
                    //}
                    using (var sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);
                            con.Close();
                            return ds;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //Djora 26.09.19
        //Primer koriscenja: 
        //DataBaseBroker db22 = new DataBaseBroker();
        //DataTable ss = db22.ParamsQueryDT("INSERT INTO CUSTOMERS(ID,NAME,AGE,ADRESS,COUNTRY) VALUES(@param0,@param1,@param2,@param3,@param4)",5,"Ali",27,"my City","England");
        //DataTable ss = db22.ParamsQueryDT("select nazivArtikla from artikli where id_artikli=@param0", 5);
        //Primer sa Like klauzulom: DataTable ss = db22.ParamsQueryDT("SELECT NazivArtikla FROM dbo.Artikli WHERE(NazivArtikla like @param0) AND(ID_ArtikliStablo = @param1)", "%ban%", 1440);
        //Primer bez parametara: DataTable ss = db22.ParamsQueryDT("SELECT NazivArtikla FROM dbo.Artikli");
        public DataTable ParamsQueryDT(string query, params object[] args)
        {
            try
            {
                var con = new SqlConnection(connstring);
                using (con)
                {
                    con.Open();
                    var cmd = new SqlCommand(query, con);

                    //Ako ima parametara
                    if (args.Length != 0)
                    {
                        for (int i = 0; i < args.Length; i++)
                            cmd.Parameters.AddWithValue("@param" + i, args[i]);
                    }
                    using (var sda = new SqlDataAdapter())
                    {                        
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {                            
                            sda.Fill(dt);
                            //sda.FillSchema(dt,Shema);
                            con.Close();
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        //Djora 28.11.19
        public Dictionary<string, string> ExucuteStoreProcedureDva(string NazivSP,ref SqlConnection con1, ref SqlTransaction transaction, params object[] args)
        {
            int j = 0;
            char[] delimit = { ',' };
            string Rezultati = "";
            string[] Rez;
            Dictionary<string, string> R = new Dictionary<string, string>();
            //SqlConnection con1;
            //con1 = new SqlConnection(Program.connectionString);
            //if (con1.State == ConnectionState.Closed) { con1.Open() ; }

            SqlCommand cmd = new SqlCommand();
            if(con1==null)
            {
               con1 = new SqlConnection(Program.connectionString);
               if (con1.State == ConnectionState.Closed) { con1.Open(); }
            }
            cmd.Connection = con1;
            cmd.Transaction = transaction;
            cmd.CommandText = NazivSP;
            cmd.CommandType = CommandType.StoredProcedure;
            j = 0;
            //Ako ima parametara
            if (args.Length != 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string vred = args[i].ToString();
                    int indeks = vred.IndexOf(":");
                    string parametar = "@" + vred.Substring(0, indeks);
                    string vrednost = vred.Substring(indeks + 1);

                    Console.WriteLine(parametar);
                    Console.WriteLine(vrednost);
                    if (vrednost == "") // izlazni
                    {
                        cmd.Parameters.Add(parametar, SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        if (j == 0)
                            Rezultati = parametar;
                        else
                            Rezultati = ',' + parametar;
                    }
                    else  //ulazni
                        cmd.Parameters.AddWithValue(parametar, vrednost);

                }
            }

            try
            {
                cmd.ExecuteNonQuery();
                 cmd.ExecuteNonQuery();
                Rez = Rezultati.Split(delimit);
                if (Rez[0] != "")
                    for (int i = 0; i < Rez.Length; i++)
                    {
                        //        Console.WriteLine(Convert.ToString(cmd.Parameters[Rez[i]].Value));
                        R.Add(Rez[i], Convert.ToString(cmd.Parameters[Rez[i]].Value));
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            return R;
        }

        public Dictionary<string, string> ExecuteStoreProcedure(string NazivSP, params object[] args)
        {
            int j = 0;
            char[] delimit = { ',' };
            string Rezultati = "";
            string[] Rez;
            Dictionary<string, string> R = new Dictionary<string, string>();
            SqlConnection con1;
            con1 = new SqlConnection(Program.connectionString);
            if (con1.State == ConnectionState.Closed) { con1.Open(); }

            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con1;
            cmd.CommandText = NazivSP;
            cmd.CommandType = CommandType.StoredProcedure;
            j = 0;
            //Ako ima parametara
            if (args.Length != 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string vred = args[i].ToString();
                    int indeks = vred.IndexOf(":");
                    string parametar = "@" + vred.Substring(0, indeks);
                    string vrednost = vred.Substring(indeks + 1);

                    Console.WriteLine(parametar);
                    Console.WriteLine(vrednost);
                    if (vrednost == "") // izlazni
                    {
                        cmd.Parameters.Add(parametar, SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                        if (j == 0)
                            Rezultati = parametar;
                        else
                            Rezultati = ',' + parametar;
                    }
                    else  //ulazni
                        cmd.Parameters.AddWithValue(parametar, vrednost);

                }
            }

            try
            {
                cmd.ExecuteNonQuery();
                Rez = Rezultati.Split(delimit);
                if (Rez[0] != "")
                    for (int i = 0; i < Rez.Length; i++)
                    {
                        //        Console.WriteLine(Convert.ToString(cmd.Parameters[Rez[i]].Value));
                        R.Add(Rez[i], Convert.ToString(cmd.Parameters[Rez[i]].Value));
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            return R;
        }



        // Djora 09.12.19 Insert i vracanjeID-a novo stvorenog rekorda
        // Djora 09.12.19 Upotreba:
        // sql = " insert into Dokumenta(RedniBroj, ID_KadrovskaEvidencija, ID_Predhodni, ID_DokumentaStablo, BrojDokumenta, Datum, Opis, ID_OrganizacionaStrukturaView, Proknjizeno) "
        //     + " values(@param0, @param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8); SELECT SCOPE_IDENTITY();";
        // DataBaseBroker db = new DataBaseBroker();
        // int newID = db.ParamsInsertScalar(sql, RedniBroj, Program.idkadar.ToString(), 1, 29, BrojDok, dtm.ToString(), IdStabloZaKnjizenje.ToString() + "-" + RedniBrZaKnjizenje, RedniBrZaKnjizenje, Program.idOrgDeo.ToString(), "NeKnjiziSe");
        public int ParamsInsertScalar(string query, params object[] args)
        {
            // definise return vrednost - novo insertovanog ID-a
            int returnValue = -1;
          Console.WriteLine(query);
            try
            {
                var con = new SqlConnection(connstring);
                if (con.State == ConnectionState.Closed) { con.Open(); }
                using (con)
                {
                    //con.Open();
                    var cmd = new SqlCommand(query, con);

                    //Ako ima parametara
                    if (args.Length != 0)
                    {
                        for (int i = 0; i < args.Length; i++)
                            cmd.Parameters.AddWithValue("@param" + i, args[i]);
                    }

                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        int.TryParse(returnObj.ToString(), out returnValue);
                    }

                    //Vraca Novo insertovani ID

                    con.Close();
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);

            }
        }

        public string ExecuteSqlTransaction(List<string> cmd)
        {
            using (SqlConnection connection = new SqlConnection(Program.connectionString))

            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }

                using (SqlCommand command = connection.CreateCommand())
                {

                    SqlTransaction transaction;
                    string result = "";
                    int i = 0;
                    // Start a local transaction.
                    transaction = connection.BeginTransaction("SampleTransaction");

                    // Must assign both transaction object and connection
                    // to Command object for a pending local transaction
                    command.Connection = connection;
                    command.Transaction = transaction;
                    
                    try
                    {
                        if (cmd.Count != 0)
                        {
                            for (i = 0; i < cmd.Count; i++)
                            {
                                if (cmd[i] != "")
                                {
                                    command.CommandText = cmd[i];
                                    Console.WriteLine(cmd[i]);
                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                        Console.WriteLine("Both records are written to database.");
                        Console.WriteLine("Both records are written to database.");
                    }


                    catch (SqlException ex)
                    {
                        Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                        Console.WriteLine("  Message: {0}", ex.Message);


                        transaction.Rollback();

                        Console.WriteLine("Rollback Exception Type: {0}", ex.GetType());
                        Console.WriteLine("  Message: {0}", ((System.Data.SqlClient.SqlException)ex).Number.ToString() + ex.Message);
                        return ((System.Data.SqlClient.SqlException)ex).Number.ToString() + "-" + ex.Message.ToString();


                    }

                    return result;
                }
            }

        }
    }
}