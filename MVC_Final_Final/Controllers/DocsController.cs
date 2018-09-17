using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Final_Final.Models.Docs;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace MVC_Final_Final.Controllers
{

    public class DocsController : Controller
    {

        string Hello;
        string World;

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            //var path = Path.Combine( Directory.GetCurrentDirectory(), "wwwroot",file.FileName);

           string path = ("C:/Users/Tush/Desktop/New folder/MVC_Final_Final/" + file.FileName);

            DocsClass MyDocs = new DocsClass();
            Hello = "Hello";

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                
                MyDocs.InsertDocs(file.FileName,path);
            }

            return View("FileUpload");
        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", filename);

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
            return View();
        }

       
    }
}
