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

namespace MVC_Final_Final.Controllers
{

    public class DocsController : Controller
    {
        double fileSizeSend;
        string fileSizeType;
        string strLastModified;

        DocsClass MyDocs = new DocsClass();
        LogClass MyLog = new LogClass();

        [HttpPost]
        public async Task<IActionResult>UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

           string path = ("C:/Users/Tush/Desktop/FileUploads/" + file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);

                strLastModified = System.IO.File.GetLastWriteTime(path).ToString("dd/MM/yyyy HH:mm:ss");

                //Get and Calculate the fileSize
                double fileSize = file.Length;

                if (fileSize < 1024 && fileSize >= 0)
                {
                    fileSizeSend = Math.Round(fileSize , 2);
                    fileSizeType = "Bytes";
                }
                else if(fileSize >= 1024 && fileSize < 1048576)
                {
                    fileSizeSend = Math.Round(fileSize / 1024 , 2);
                    fileSizeType = "KB";
                }
                else
                {
                    fileSizeSend = Math.Round((fileSize / 1024) / 1024 , 2);
                    fileSizeType = "MB";
                }

                MyDocs.SelectDocs(file.FileName,path,strLastModified, fileSizeSend, fileSizeType);
                MyLog.LogHistoryInsert(file.FileName, path, strLastModified, fileSizeSend, fileSizeType);
            }

            return View("FileUpload");
        }

        public async Task<IActionResult>Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            string path = ("C:/Users/Tush/Desktop/FileUploads/" + filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

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

        //this.User.FindFirstValue(ClassTypes.NameIdentifier).toString()  //return view


    }
}
