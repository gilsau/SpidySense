using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using SpidySense.Data;

using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;

namespace SpidySense.Controllers
{
    public class CrawlerController : Controller
    {
        SSEntities db = new SSEntities();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.WebUrls = db.WebUrls.OrderBy(wu => wu.Website.Name).OrderBy(wu => wu.Name).ToList();
            return View();
        }
    }
}