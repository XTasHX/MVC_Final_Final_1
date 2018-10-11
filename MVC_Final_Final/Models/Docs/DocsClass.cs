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
        string DBConn = "Server=localhost;port=3306;Database=mvc_Data1;User=root;Password=Natassja12;";
        string InsertCmd;
        string UpdateCmd;
        string InsertHistoryCmd;


        public string ConnectionString { get; set; }

        public DocsClass(string connection)
        {
            this.ConnectionString = connection;
        }

        public DocsClass() { }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(DBConn);
        }

        public bool UpdateDocs(string DocName, string DocPath, string DocUploadTime, double fileSize, string fileSizeType)
        {
            UpdateCmd = "UPDATE documents SET DocName = '" + DocName + "', DocPath = '" + DocPath + "', DocUploadTime = '" + DocUploadTime + "',  DocSize = '" + fileSize + "', DocSizeType = '" + fileSizeType + "' where  DocName = '" + DocName + "'";


            bool Succsess = false;

            try
            {
                using (MySqlConnection Myconn = GetConnection())
                {
                    Myconn.Open();

                    MySqlCommand cmd = new MySqlCommand(UpdateCmd, Myconn);


                    if (cmd.ExecuteNonQuery() == 1)
                        Succsess = true;

                }
            }

            catch (Exception)
            {
                return false;
            }


            return Succsess;
        }

        public bool SelectDocs(string DocNames, string DocPath, string DocUploadTime, double fileSize, string fileSizeType)
        {

            bool Succsess = false;

            try
            {
                using (MySqlConnection Myconn = GetConnection())
                {
                    Myconn.Open();

                    MySqlCommand Selectcmd = new MySqlCommand("SELECT * FROM documents WHERE (DocName = '" + DocNames + "')", Myconn);

                    MySqlDataReader selectdr = Selectcmd.ExecuteReader();


                    if ((selectdr.Read() == true))

                    {

                        UpdateDocs(DocNames, DocPath, DocUploadTime, fileSize, fileSizeType);

                    }

                    else

                    {

                        InsertDocs(DocNames, DocPath, DocUploadTime, fileSize, fileSizeType);

                    }

                }

            }

            catch (Exception)
            {
                return false;
            }


            return Succsess;
        }

        public bool InsertDocs(string DocNames, string DocPath, string DocUploadTime, double fileSize, string fileSizeType)
        {
            InsertCmd = "INSERT into mvc_data1.documents(DocName,DocPath,DocUploadTime,DocSize,DocSizeType) values('" + DocNames + "','" + DocPath + "','" + DocUploadTime + "','" + fileSize + "','" + fileSizeType + "');";


            bool Succsess = false;

            try
            {
                using (MySqlConnection Myconn = GetConnection())
                {
                    Myconn.Open();

                    MySqlCommand cmd = new MySqlCommand(InsertCmd, Myconn);


                    if (cmd.ExecuteNonQuery() == 1)
                        Succsess = true;

                }
            }

            catch (Exception)
            {
                return false;
            }


            return Succsess;
        }



        public List<DocsContext> GetDataList()
        {
            List<DocsContext> DocList = new List<DocsContext>();

            using (MySqlConnection Myconn = GetConnection())
            {
                Myconn.Open();
                MySqlCommand cmd = new MySqlCommand("select DocName , DocUploadTime , DocSize , DocSizeType from documents", Myconn);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            DocList.Add(new DocsContext()
                            {
                                DocNames = dr["DocName"].ToString(),
                                DocUploadTime = dr["DocUploadTime"].ToString(),
                                fileSize = dr["DocSize"].ToString(),
                                fileSizeType = dr["DocSizeType"].ToString()
                            });
                        }
                        dr.Close();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        Myconn.Close();
                    }
                }
            }
            return DocList;
        }


    }

       
}
