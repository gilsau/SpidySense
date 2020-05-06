using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace SpidySense.Controllers
{
    public class ExportController : Controller
    {
        [HttpGet]
        public JsonResult GetFiles()
        {
            string[] filepaths = Directory.GetFiles(Server.MapPath("~/files/"));
            string[] filenames = filepaths.Select(fp => "<a target='_blank' href='/files/" + fp.Split('\\')[fp.Split('\\').Length - 1] + "'>" + fp.Split('\\')[fp.Split('\\').Length - 1] + "</a>").ToArray();
            return Json(string.Join("<hr>", filenames), JsonRequestBehavior.AllowGet);
        }
    }
}