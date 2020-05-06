using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpidySense.Data;
using SpidySense.Models;

namespace SpidySense.Controllers
{
    public class WebsitesController : Controller
    {
        private SSEntities db = new SSEntities();

        // GET: Websites
        public ActionResult Index()
        {
            return View(db.Websites.ToList());
        }

        // GET: Websites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Website website = db.Websites.Find(id);
            if (website == null)
            {
                return HttpNotFound();
            }
            return View(website);
        }

        // GET: Websites/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Websites/Create
        [HttpPost]
        public JsonResult Create(WebsiteModel model) {

            Result result = new Result();

            //add site info to db
            Website web = new Website();
            web.Name = model.Name;
            web.Description = model.Description;
            web.Url = model.Url;
            db.Websites.Add(web);

            //save website info to db
            try
            {
                db.SaveChanges();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.UserMsg = ex.Message;
            }

            //add webUrls and fields to website
            if (result.Success)
            {
                //search urls
                foreach (WebUrlModel webUrl in model.WebUrls)
                {

                    //url info
                    WebUrl wUrl = new WebUrl()
                    {
                        WebsiteId = web.Id,
                        Name = webUrl.Name,
                        Url = webUrl.Url,
                        RowSelector = webUrl.RowSelector,
                        WebUrlFields = new List<WebUrlField>()
                    };

                    //url fields
                    foreach (WebUrlFieldModel webUrlField in webUrl.WebUrlFields)
                    {
                        wUrl.WebUrlFields.Add(new WebUrlField()
                        {
                            Name = webUrlField.Name,
                            Selector = webUrlField.Selector
                        });
                    }

                    //add url to site
                    web.WebUrls.Add(wUrl);
                }

                //save webUrls and fields for this website to db
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.UserMsg = ex.Message;
                }
            }

            return Json(result);
        }

        // GET: Websites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Website website = db.Websites.Find(id);
            if (website == null)
            {
                return HttpNotFound();
            }
            return View(website);
        }

        //POST: Websites/Edit
        [HttpPost]
        public JsonResult Edit(WebsiteModel model)
        {
            Result result = new Result();

            //add site info to db
            Website web = db.Websites.Find(model.Id);
            web.Name = model.Name;
            web.Description = model.Description;
            web.Url = model.Url;
            
            //save website info to db
            try
            {
                db.SaveChanges();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.UserMsg = ex.Message;
            }

            if (result.Success)
            {
                result = new Result();

                //loop thru web urls for website, and REMOVE THEM ALL
                for (int w = web.WebUrls.Count - 1; w > -1; w--)
                {
                    WebUrl wu = web.WebUrls.ElementAt(w);

                    //loop thru fields for url
                    for (int f = wu.WebUrlFields.Count - 1; f > -1; f--)
                    {
                        WebUrlField wuf = wu.WebUrlFields.ElementAt(f);
                        db.WebUrlFields.Remove(wuf);
                    }
                    db.WebUrls.Remove(wu);
                }
                
                try
                {
                    db.SaveChanges();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.UserMsg = ex.Message;
                }
            }

            //add webUrls and fields to website
            if (result.Success)
            {
                result = new Result();

                //search urls
                foreach (WebUrlModel webUrl in model.WebUrls)
                {
                    //url info
                    WebUrl wUrl = new WebUrl()
                    {
                        WebsiteId = web.Id,
                        Name = webUrl.Name,
                        Url = webUrl.Url,
                        RowSelector = webUrl.RowSelector
                    };

                    //add url to site
                    web.WebUrls.Add(wUrl);

                    try
                    {
                        db.SaveChanges();
                        result.Success = true;
                    }
                    catch (Exception ex)
                    {
                        result.UserMsg = ex.Message;
                    }

                    if (result.Success)
                    {
                        result = new Result();

                        //url fields
                        foreach (WebUrlFieldModel webUrlField in webUrl.WebUrlFields)
                        {
                            wUrl.WebUrlFields.Add(new WebUrlField()
                            {
                                WebUrlId = wUrl.Id,
                                Name = webUrlField.Name,
                                Selector = webUrlField.Selector
                            });
                        }

                        try
                        {
                            db.SaveChanges();
                            result.Success = true;
                        }
                        catch (Exception ex)
                        {
                            result.UserMsg = ex.Message;
                        }
                    }
                }
            }

            return Json(result);
        }

        // GET: Websites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Website website = db.Websites.Find(id);
            if (website == null)
            {
                return HttpNotFound();
            }
            return View(website);
        }

        // POST: Websites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //get website
            Website website = db.Websites.Find(id);

            //loop thru web urls for website
            for(int w = website.WebUrls.Count-1; w > -1; w--) {
                WebUrl wu = website.WebUrls.ElementAt(w);

                //loop thru fields for url
                for (int f = wu.WebUrlFields.Count - 1; f > -1; f--)
                {
                    WebUrlField wuf = wu.WebUrlFields.ElementAt(f);
                    db.WebUrlFields.Remove(wuf);
                }
                db.WebUrls.Remove(wu);
            }
            db.Websites.Remove(website);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
