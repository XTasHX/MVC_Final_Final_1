using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace MVC_Final_Final.Models.Docs
{
    public class DocsClass
    {
       // public string DBConn { get; set; }

       string DBConn = "Server=localhost;port=3306;Database=mvc_Data1;User=root;Password=Natassja12;";
       string InsertCmd;

      public MySqlConnection GetConnection()
        {
            return new MySqlConnection(DBConn);
        } 

        public bool InsertDocs(string DocName, string DocPath)
        {
            InsertCmd = "insert into mvc_data1.documents(DocName,DocPath) values('" + DocName + "','" + DocPath + "');";

            bool Succsess = false;

            try
            {
                using (MySqlConnection Myconn = GetConnection())
                {
                    Myconn.Open();

                    MySqlCommand cmd = new MySqlCommand(InsertCmd , Myconn);
                

                    if (cmd.ExecuteNonQuery() == 1)
                        Succsess = true;
                        
                }
            }

            catch(Exception)
            {
                return false;
            }

            
            return Succsess;
        }

        public List<string> GetDatabaseList()
        {
            List<string> list = new List<string>();

            using (MySqlConnection Myconn = GetConnection())
            {
                Myconn.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT UserID from aspnetusers", Myconn))
                {
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(dr[0].ToString());
                        }
                    }
                }
            }
            return list;

        }


    }
}
