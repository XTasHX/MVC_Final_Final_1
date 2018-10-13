using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_Final_Final.Models.Docs;
using MVC_Final_Final.Models.Log;

namespace MVC_Final_Final.Controllers
{
    public class LogController : Controller
    {
        LogClass dbclass;

        //geting history data from LogClass and adding it to the view
        public IActionResult LogHistory()
        {
            dbclass = HttpContext.RequestServices.GetService(typeof(Models.Log.LogClass)) as Models.Log.LogClass;

            return View(dbclass.GetLogHistory());
        }
    }
}