using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Final_Final.Models.Docs;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MVC_Final_Final.Models;
using MVC_Final_Final.Models.Log;
using System;
using System.Security.Claims;

namespace MVC_Final_Final.Controllers
{

    public class PrivateDocsController : Controller
    {
        double PrivatefileSizeSend, PrivatefileSize;
        string PrivatefileSizeType, user, PrivatePath, strLastModified, privatePath,path,Pvtstatus, PvtPrivateOrPublic;

        //Objects of model classes

        DocsClass MyDocs = new DocsClass();
        LogClass MyLog = new LogClass();

        //Upload Files Method

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            //get the user name that uploads the file (current logged in user)
            user = User.Identity.Name;
            Pvtstatus = "New";
            PvtPrivateOrPublic = "Private";

           //set path where upload files are sent to

           PrivatePath = ("C:/Users/Tush/Desktop/FileUploads/" + user + "/");
            path = ("C:/Users/Tush/Desktop/FileUploads/" + user + "/" + file.FileName);


            if (!System.IO.Directory.Exists(PrivatePath))
            {
               System.IO.Directory.CreateDirectory(PrivatePath);
            }

            // get the file from path and store them in path given above
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);

                strLastModified = System.IO.File.GetLastWriteTime(path).ToString("dd/MM/yyyy HH:mm:ss");

                //Get and Calculate the fileSize
                PrivatefileSize = file.Length;

                if (PrivatefileSize < 1024 && PrivatefileSize >= 0)
                {
                    PrivatefileSizeSend = Math.Round(PrivatefileSize, 2);
                    PrivatefileSizeType = "Bytes";
                }
                else if (PrivatefileSize >= 1024 && PrivatefileSize < 1048576)
                {
                    PrivatefileSizeSend = Math.Round(PrivatefileSize / 1024, 2);
                    PrivatefileSizeType = "KB";
                }
                else
                {
                    PrivatefileSizeSend = Math.Round((PrivatefileSize / 1024) / 1024, 2);
                    PrivatefileSizeType = "MB";
                }
                //calling two methods in the DocsController to store data in database
                MyDocs.SelectPrivateDocs(file.FileName, path, strLastModified, PrivatefileSizeSend, PrivatefileSizeType, user, Pvtstatus, PvtPrivateOrPublic);
                MyLog.LogHistoryInsert(file.FileName, path, strLastModified, PrivatefileSizeSend, PrivatefileSizeType, user,PvtPrivateOrPublic);
            }
            //  }

            return View("PrivateDoc");
        }

        //Method to download selected file
        public async Task<IActionResult> Download(string filename)
        {
            user = User.Identity.Name;

            if (filename == null)
                return Content("filename not present");

            //path where the local files are stored 
            path = ("C:/Users/Tush/Desktop/FileUploads/" + user + "/" + filename);

            //get the file from the path and download the file through browser
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }


        //gets the file extention type
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        //set the diffrent types of extentions the program is able to download
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".csv", "text/csv"}
            };
        }


        public IActionResult PrivateDelete(string filename)
        {
            Models.Docs.DocsClass mycontext = HttpContext.RequestServices.GetService(typeof(Models.Docs.DocsClass)) as Models.Docs.DocsClass;

            if (MyDocs.PrivateDelete(filename))
                return View("DownloadPrivateDoc", mycontext.GetPrivateDataList());
            else
                return View("DownloadPrivateDoc", mycontext.GetPrivateDataList());
        }


        public IActionResult PrivateDoc()
        {
            return View();

        }

        public IActionResult DownloadPrivateDoc()
        {
            Models.Docs.DocsClass mycontext = HttpContext.RequestServices.GetService(typeof(Models.Docs.DocsClass)) as Models.Docs.DocsClass;

            return View(mycontext.GetPrivateDataList());
        }

        public IActionResult LogHistory()
        {
            return View();

        }


    }
}
