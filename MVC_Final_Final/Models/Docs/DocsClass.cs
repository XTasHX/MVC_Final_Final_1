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
        string InsertCmd,UpdateCmd,Selectquery, DeleteQuery;

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

        //method to update database tabels
        public bool UpdateDocs(string DocName, string DocPath, string DocUploadTime, double fileSize, string fileSizeType, string user)
        {
            UpdateCmd = "UPDATE documents SET DocName = @DocName,DocPath = @DocPath, DocUploadTime = @DocUploadTime, DocSize = @fileSize , DocSizeType = @fileSizeType, UserName = @user WHERE  DocName = @DocName";

            bool Succsess = false;

            try
            {
                using (MySqlConnection Myconn = GetConnection())
                {
                    Myconn.Open();

                    MySqlCommand cmd = new MySqlCommand(UpdateCmd, Myconn);
                    cmd.Parameters.AddWithValue("@DocName", DocName);
                    cmd.Parameters.AddWithValue("@DocPath", DocPath);
                    cmd.Parameters.AddWithValue("@DocUploadTime", DocUploadTime);
                    cmd.Parameters.AddWithValue("@fileSize", fileSize);
                    cmd.Parameters.AddWithValue("@fileSizeType", fileSizeType);
                    cmd.Parameters.AddWithValue("@user", user);

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

        //method to select data from  database
        public bool SelectDocs(string DocNames, string DocPath, string DocUploadTime, double fileSize, string fileSizeType, string user,string Pvtstatus)
        {

            bool Succsess = false;

            try
            {
                using (MySqlConnection Myconn = GetConnection())
                {
                    Myconn.Open();

                    Selectquery = ("SELECT * FROM documents WHERE DocName = @DocNames;");

                    MySqlCommand Selectcmd = new MySqlCommand(Selectquery, Myconn);
                    Selectcmd.Parameters.AddWithValue("@DocNames", DocNames);
          

                    MySqlDataReader selectdr = Selectcmd.ExecuteReader();
                    //If Docs exists
                    if ((selectdr.Read() == true))

                    {
                        UpdateDocs(DocNames, DocPath, DocUploadTime, fileSize, fileSizeType, user);
                    }
                    //If Doc does not exist
                    else

                    {
                        InsertDocs(DocNames, DocPath, DocUploadTime, fileSize, fileSizeType, user, Pvtstatus);
                    }
                }
            }

            catch (Exception)
            {
                return false;
            }

            return Succsess;
        }

        public bool InsertDocs(string DocNames, string DocPath, string DocUploadTime, double fileSize, string fileSizeType, string user,string Pvtstatus)
        {
            InsertCmd = "INSERT into mvc_data1.documents(DocName,DocPath,DocUploadTime,DocSize,DocSizeType,UserName,status) values(@DocNames,@DocPath,@DocUploadTime,@fileSize,@fileSizeType,@user,@Pvtstatus)";


            bool Succsess = false;

            try
            {
                using (MySqlConnection Myconn = GetConnection())
                {
                    Myconn.Open();

                    MySqlCommand cmd = new MySqlCommand(InsertCmd, Myconn);
                    cmd.Parameters.AddWithValue("@DocNames",DocNames);
                    cmd.Parameters.AddWithValue("@DocPath", DocPath);
                    cmd.Parameters.AddWithValue("@DocUploadTime", DocUploadTime);
                    cmd.Parameters.AddWithValue("@fileSize", fileSize);
                    cmd.Parameters.AddWithValue("@fileSizeType", fileSizeType);
                    cmd.Parameters.AddWithValue("@Pvtstatus", Pvtstatus);
                    cmd.Parameters.AddWithValue("@user", user);


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

        public bool Delete(string FileName)
        {
            DeleteQuery = "DELETE FROM documents WHERE DocName = @DocName";

            bool Succsess = false;

            try
            {
                using (MySqlConnection Myconn = GetConnection())
                {
                    Myconn.Open();

                    MySqlCommand cmd = new MySqlCommand(DeleteQuery, Myconn);
                    cmd.Parameters.AddWithValue("@DocName",FileName);

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

        public bool InsertPrivateDocs(string PrivateDocNames, string PrivateDocPath, string PrivateDocUploadTime, double PrivatefileSize, string PrivatefileSizeType, string user,string status)
        {
            InsertCmd = "INSERT into mvc_data1.privatedocs(PrivateDocName,PrivateDocPath,PrivateDocUploadTime,PrivateDocSize,PrivateDocSizeType,UserName,status) values(@PrivateDocNames,@PrivateDocPath,@PrivateDocUploadTime,@PrivatefileSize,@PrivatefileSizeType,@user,@status)";


            bool Succsess = false;

            try
            {
                using (MySqlConnection Myconn = GetConnection())
                {
                    Myconn.Open();

                    MySqlCommand cmd = new MySqlCommand(InsertCmd, Myconn);
                    cmd.Parameters.AddWithValue("@PrivateDocNames", PrivateDocNames);
                    cmd.Parameters.AddWithValue("@PrivateDocPath", PrivateDocPath);
                    cmd.Parameters.AddWithValue("@PrivateDocUploadTime", PrivateDocUploadTime);
                    cmd.Parameters.AddWithValue("@PrivatefileSize", PrivatefileSize);
                    cmd.Parameters.AddWithValue("@PrivatefileSizeType", PrivatefileSizeType);
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@status", status);


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

        public bool SelectPrivateDocs(string PrivateDocNames, string PrivateDocPath, string PrivateDocUploadTime, double PrivatefileSize, string PrivatefileSizeType, string user, string status)
        {

            bool Succsess = false;

            try
            {
                using (MySqlConnection Myconn = GetConnection())
                {
                    Myconn.Open();

                    Selectquery = ("SELECT * FROM privatedocs WHERE PrivateDocName = @PrivateDocNames;");

                    MySqlCommand Selectcmd = new MySqlCommand(Selectquery, Myconn);
                    Selectcmd.Parameters.AddWithValue("@PrivateDocNames", PrivateDocNames);


                    MySqlDataReader selectdr = Selectcmd.ExecuteReader();
                    //If Docs exists
                    if ((selectdr.Read() == true))

                    {
                        UpdateDocs(PrivateDocNames, PrivateDocPath, PrivateDocUploadTime, PrivatefileSize, PrivatefileSizeType, user);
                    }
                    //If Doc does not exist
                    else

                    {
                        InsertPrivateDocs(PrivateDocNames, PrivateDocPath, PrivateDocUploadTime, PrivatefileSize, PrivatefileSizeType, user,status);
                    }
                }
            }

            catch (Exception)
            {
                return false;
            }

            return Succsess;
        }
        //ArrayList to store data for model used in views
        public List<DocsContext> GetDataList()
        {
            List<DocsContext> DocList = new List<DocsContext>();

            using (MySqlConnection Myconn = GetConnection())
            {
                Myconn.Open();
                MySqlCommand cmd = new MySqlCommand("select DocName , DocUploadTime , DocSize , DocSizeType,UserName,status from documents", Myconn);

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
                                fileSizeType = dr["DocSizeType"].ToString(),
                                UserName = dr["UserName"].ToString(),
                                status = dr["status"].ToString()
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


        //ArrayList to store data for model used in views
        public List<DocsContext> GetPrivateDataList()
        {
            List<DocsContext> DocList = new List<DocsContext>();

            using (MySqlConnection Myconn = GetConnection())
            {
                Myconn.Open();
                MySqlCommand cmd = new MySqlCommand("select PrivateDocName , PrivateDocUploadTime , PrivateDocSize , PrivateDocSizeType,UserName,status from privatedocs", Myconn);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (dr.Read())
                        {
                            DocList.Add(new DocsContext()
                            {
                                PrivateDocNames = dr["PrivateDocName"].ToString(),
                                PrivateDocUploadTime = dr["PrivateDocUploadTime"].ToString(),
                                PrivatefileSize = dr["PrivateDocSize"].ToString(),
                                PrivatefileSizeType = dr["PrivateDocSizeType"].ToString(),
                                PrivateUserName = dr["UserName"].ToString(),
                                Pvtstatus = dr["status"].ToString()
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

        public bool PrivateDelete(string FileName)
        {
            DeleteQuery = "DELETE FROM privatedocs WHERE PrivateDocName = @PrivateDocName";

            bool Succsess = false;

            try
            {
                using (MySqlConnection Myconn = GetConnection())
                {
                    Myconn.Open();

                    MySqlCommand cmd = new MySqlCommand(DeleteQuery, Myconn);
                    cmd.Parameters.AddWithValue("@PrivateDocName", FileName);

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

    }    
}
