using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace MVC_Final_Final.Models.Docs
{
    

    public class DocsContext
    { 
       //get attributes from DocsClass
       public string DocNames { get; set; }
       public string DocUploadTime { get; set; }
       public string fileSize { get; set; }
       public string fileSizeType { get; set; }
       public string UserName { get; set; }
       public string status { get; set; }

        public string PrivateDocNames { get; set; }
       public string PrivateDocUploadTime { get; set; }
       public string PrivatefileSize { get; set; }
       public string PrivatefileSizeType { get; set; }
       public string PrivateUserName { get; set; }
        public string Pvtstatus { get; set; }

    }
}
