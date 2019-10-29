using System;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Database_server
{
    class ProgramM
    {
        static void Main(string[] args)
        {
            /*
            SQLHandler sqlHandler = new SQLHandler();
            MySqlConnection mySqlConnect = sqlHandler.GetSqlConn();
            sqlHandler.OpenSql();

            //sqlHandler.setTable();
            sqlHandler.addTsk("kill the moking bird", "do my sdd homework", 32, "nibab");
            sqlHandler.addTsk("nb!", "do my PL review", 32, "shinibaba");

            Dictionary<String, String> lst = sqlHandler.getTsk();
            */
            sqltest();
            
        }

        static void sqltest()
        {
            checkMessage tmp = new checkMessage();
            tmp.success = false;
            tmp.lst = new Dictionary<string, string[]>();
            tmp.lst["baba"] = new string[3];
            tmp.lst["baba"][0] = "nihao";
            tmp.lst["baba"][1] = "shi";
            tmp.lst["baba"][2] = "baba";

            tmp.lst["niyeye"] = new string[3];
            tmp.lst["niyeye"][0] = "wo";
            tmp.lst["niyeye"][1] = "shi";
            tmp.lst["niyeye"][2] = "ni yeye";

            string serializedPerson = JsonConvert.SerializeObject(tmp);
            Console.WriteLine("\n\n>>>>>>>" + serializedPerson + "<<<<<\n\n\n");
            checkMessage newPerson = JsonConvert.DeserializeObject<checkMessage>(serializedPerson);
            Console.WriteLine("\n\n" + newPerson.success + newPerson.lst);

            foreach(KeyValuePair<string, string[]> pair in newPerson.lst)
            {
                Console.WriteLine(pair.Key + ">>>>");
                foreach(string itr in pair.Value)
                {
                    Console.Write(itr + " ");
                }
                Console.WriteLine();
            }
        }
    }

    public class checkMessage
    {
        public Boolean success;
        public string ErrorMessage;
        public Dictionary<string, string[]> lst;
    }




    public class SQLHandler 
    {
        /// <summary>
        /// Setting up the dataase 
        /// </summary>
        public MySqlConnection GetSqlConn()
        {
            // 数据库
            MySqlConnection sqlConn;
            string connStr = "Database=test;Data Source=127.0.0.1;User Id=root;Password=3358;port=3306";
            sqlConn = new MySqlConnection(connStr);
            return sqlConn;
        }

        /// <summary>
        /// Opens the sql.
        /// </summary>
        public void OpenSql()
        {
            // 数据库
            MySqlConnection sqlConn = GetSqlConn();
            try
            {
                sqlConn.Open();
                Console.WriteLine("NO ERROR!!!!!!!Connection success!");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.WriteLine("ERROR!!!!!!");
                return;
            }
            /*
            try
            {
                Dictionary<string, string> result = getTsk();

                List<string> test = new List<string>(result.Keys);
                Console.WriteLine(test.Count);
                for (int i = 0; i < test.Count; i++)
                {
                    Console.WriteLine(result[test[i]]);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Unable to get Dict");
                Console.WriteLine(ex.Message);
            }
            */


        }

        /// <summary>
        ///  Initialize:
        ///  (X) Two databse: (do the check first and then set up the database structure)
        ///  user
        ///  task
        ///  (X) addUsr(String name, String pwd)
        ///  (X) searchUsr(String name, String pwd)
        ///  () addCoin(String name, int coin)
        ///  () addExp(Strig name, int exp)
        ///  () losCoin(String name, int coin)
        /// 
        ///  (X) getTsk()
        ///  () addTsk(String title, String content, int coin)
        ///  () deleteTask(String title)
        ///  
        /// </summary>
        /// 

        public void setTable()
        {
            MySqlConnection sqlConn = GetSqlConn();
            try
            {
                sqlConn.Open();
                Console.WriteLine("suppose to set up the table");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return;
            }

            try
            {

                
                String SetUsr = "CREATE TABLE usr ( name CHAR(32) PRIMARY KEY , pwd CHAR(32) NOT NULL , coin int DEFAULT 100, exp int DEFAULT 0 );";
                String SetTsk = "CREATE TABLE tsk ( id int AUTO_INCREMENT PRIMARY KEY, title CHAR(32) , content CHAR(255), coin int, exp int, owner CHAR(32) NOT NULL, taker CHAR(32) );";
                MySqlCommand setBaseUsr = new MySqlCommand(SetUsr);
                setBaseUsr.Connection = sqlConn;
                MySqlCommand setBaseTsk = new MySqlCommand(SetTsk);
                setBaseTsk.Connection = sqlConn;
               
                setBaseUsr.ExecuteNonQuery();
                setBaseTsk.ExecuteNonQuery();
                
                sqlConn.Close();

            }
            catch (Exception ex)
            {
                sqlConn.Close();
                Console.Write("Create TABLE (maybe it has been created)");
                Console.Write(ex.ToString());
            }

        }

        public Boolean addUsr(String name, String pwd)
        {
            MySqlConnection sqlConn = GetSqlConn();
            try
            {
                sqlConn.Open();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            try
            {
                String strUsr = "INSERT usr(name , pwd) VALUES (@name, @pwd);";
                MySqlCommand instUsr = new MySqlCommand(strUsr, sqlConn);
                instUsr.Parameters.AddWithValue("@name", name);
                instUsr.Parameters.AddWithValue("@pwd", pwd);
                instUsr.ExecuteNonQuery();
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                sqlConn.Close();
                Console.WriteLine("INSERT INTO Usr may can not work");
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }


        public Dictionary<string, string> searchUsr(String name, String pwd)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();


            MySqlConnection sqlConn = GetSqlConn();
            try
            {
                sqlConn.Open();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                result["Error"] = "Unable to Connect to DB";
                return result;
            }

            try
            {
                String strUsr = "SELECT * FROM usr where name=@name;";
                MySqlCommand findUsr = new MySqlCommand(strUsr, sqlConn);
                findUsr.Parameters.AddWithValue("@name", name);
                MySqlDataReader resUsr = findUsr.ExecuteReader();
                resUsr.Read();
                if (resUsr.HasRows)
                {
                    if (pwd.Equals(resUsr["pwd"]))
                    {
                        result["coin"] = resUsr["coin"].ToString();
                        result["exp"] = resUsr["exp"].ToString();
                    }
                    else
                    {
                        result["Error"] = "Wrong pwd";
                    }

                    return result;
                }
                else
                {
                    sqlConn.Close();
                    result["Error"] = "Cannot find the shit";
                    return result;
                }
            }
            catch (Exception ex)
            {
                sqlConn.Close();

                Console.Write("searchUsr Usr may can not work");
                Console.Write(ex.Message);

                result["Error"] = "have problem in finding Usr";
            }
            return result;
        }


        public Dictionary<string, string> getTsk()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            MySqlConnection sqlConn = GetSqlConn();
            try
            {
                sqlConn.Open();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                result["Error"] = "Unable to Connect to DB";
                return result;
            }

            try
            {
                String strsql = "SELECT * FROM tsk;";
                MySqlCommand sqlComm = new MySqlCommand(strsql, sqlConn);
                MySqlDataReader sqlRes = sqlComm.ExecuteReader();

                while (sqlRes.Read())
                {
                    int id = (int)sqlRes["id"];
                    string content = (string)sqlRes["content"];
                    string owner = (string)sqlRes["owner"];
                    string row = content + "|" + owner;
                    result.Add(id.ToString(), row);
                }
            }
            catch (Exception ex)
            {
                sqlConn.Close();

                Console.Write("getTsk  may can not work");
                Console.Write(ex.Message);

                result["Error"] = "have problem in finding Tsk" + ex.ToString();
            }
            return result;
        }


        public Boolean addTsk(String title, String content, int coin, String owner)
        {
            MySqlConnection sqlConn = GetSqlConn();
            try
            {
                sqlConn.Open();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return true;
            }

            try
            {
                String strUsr = "INSERT tsk(title , content, coin, owner) VALUES (@title, @content, @coin, @owner);";
                MySqlCommand instUsr = new MySqlCommand(strUsr, sqlConn);
                instUsr.Parameters.AddWithValue("@title", title);
                instUsr.Parameters.AddWithValue("@content", content);
                instUsr.Parameters.AddWithValue("@coin", coin);
                instUsr.Parameters.AddWithValue("@owner", owner);
                instUsr.ExecuteNonQuery();
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                sqlConn.Close();
                Console.WriteLine("INSERT INTO Tsk may can not work");
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        
    }

}
