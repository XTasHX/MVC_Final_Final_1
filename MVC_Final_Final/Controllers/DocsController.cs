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

    public class DocsController : Controller
    {
        double fileSizeSend, fileSize;
        string fileSizeType, user, path, strLastModified,privatePath,status,PrivateOrPublic;
        
        //Objects of model classes

        DocsClass MyDocs = new DocsClass();
        LogClass MyLog = new LogClass();

        //Upload Files Method

        [HttpPost]
        public async Task<IActionResult>UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            //get the user name that uploads the file (current logged in user)
            user = User.Identity.Name;
            status = "New";
            PrivateOrPublic = "Public";

            //set path where upload files are sent to

            path = ("C:/Users/Tush/Desktop/FileUploads/" + file.FileName);
       
            // get the file from path and store them in path given above
            using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                    strLastModified = System.IO.File.GetLastWriteTime(path).ToString("dd/MM/yyyy HH:mm:ss");

                    //Get and Calculate the fileSize
                    fileSize = file.Length;

                    if (fileSize < 1024 && fileSize >= 0)
                    {
                        fileSizeSend = Math.Round(fileSize, 2);
                        fileSizeType = "Bytes";
                    }
                    else if (fileSize >= 1024 && fileSize < 1048576)
                    {
                        fileSizeSend = Math.Round(fileSize / 1024, 2);
                        fileSizeType = "KB";
                    }
                    else
                    {
                        fileSizeSend = Math.Round((fileSize / 1024) / 1024, 2);
                        fileSizeType = "MB";
                    }
                    //calling two methods in the DocsController to store data in database
                    MyDocs.SelectDocs(file.FileName, path, strLastModified, fileSizeSend, fileSizeType, user,status,PrivateOrPublic);
                    MyLog.LogHistoryInsert(file.FileName, path, strLastModified, fileSizeSend, fileSizeType, user,PrivateOrPublic);
                }

            return View("FileUpload");
        }

        //Method to download selected file
        public async Task<IActionResult>Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            //path where the local files are stored 
            path = ("C:/Users/Tush/Desktop/FileUploads/" + filename);

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

      
        public IActionResult Delete(string filename)
        {
            Models.Docs.DocsClass mycontext = HttpContext.RequestServices.GetService(typeof(Models.Docs.DocsClass)) as Models.Docs.DocsClass;

            if (MyDocs.Delete(filename))
                return View("FileDownload", mycontext.GetDataList());
            else
                return View("FileDownload", mycontext.GetDataList());
        }

        public IActionResult FileUpload()
        {
            return View();

        }

        public IActionResult FileDownload()
        {
            Models.Docs.DocsClass mycontext = HttpContext.RequestServices.GetService(typeof(Models.Docs.DocsClass)) as Models.Docs.DocsClass;

            return View(mycontext.GetDataList());
        }

        public IActionResult LogHistory()
        {
            return View();

        }
       
    }
}
