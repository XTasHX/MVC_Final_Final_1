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
       public string DocNames { get; set; }
       public string DocUploadTime { get; set; }
       public string fileSize { get; set; }
       public string fileSizeType { get; set; }

    }
}
