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

namespace SpidySense
{
    public partial class Crawl : System.Web.UI.Page
    {
        protected string ExportUrl { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["WebUrlId"] != null)
            {
                int WebUrlId = int.Parse(Request.QueryString["WebUrlId"]);
                StringBuilder sbBrowser = new StringBuilder();
                StringBuilder sbFile = new StringBuilder();

                //get website info from db
                SSEntities db = new SSEntities();
                WebUrl webUrl = db.WebUrls.SingleOrDefault(w => w.Id == WebUrlId);
                
                //Get html for each url
                var web = new HtmlWeb();
                var document = web.Load(webUrl.Url);
                var page = document.DocumentNode;

                //get field names, and add them to the top row
                if (sbFile.Length == 0)
                {
                    int w = 0;
                    foreach (WebUrlField webUrlField in webUrl.WebUrlFields)
                    {
                        if (w > 0)
                        {
                            sbBrowser.Append(" , ");
                            sbFile.Append(",");
                        }
                        sbBrowser.Append("<b>" + webUrlField.Name + "</b>");
                        sbFile.Append(string.Format("\"{0}\"", webUrlField.Name));
                        w++;
                    }
                    sbBrowser.Append("<hr>");
                    sbFile.Append(Environment.NewLine);
                }

                //get rows
                foreach (var item in page.QuerySelectorAll(webUrl.RowSelector))
                {
                    //scrape fields
                    string fieldName = string.Empty;
                    string fieldVal = string.Empty;
                    int f = 0;
                    foreach (WebUrlField field in webUrl.WebUrlFields)
                    {
                        fieldVal = item.QuerySelector(field.Selector).InnerText;

                        if (f > 0)
                        {
                            sbBrowser.Append(" , ");
                            sbFile.Append(",");
                        }
                        sbBrowser.Append(fieldVal);
                        sbFile.Append(string.Format("\"{0}\"", fieldVal.Replace("\"", "'")));
                        f++;
                    }
                    sbBrowser.Append("<hr>");
                    sbFile.Append(Environment.NewLine);

                    //write to browser
                    Response.Write(sbBrowser.ToString());
                    Response.Flush();

                    sbBrowser.Clear();

                    
                }

                //write to file
                if (sbFile.Length > 0)
                {
                    DateTime dt = DateTime.Now;
                    string filename = string.Format("{0:yyyy_MM_dd_hh_mm_ss}.csv", dt);
                    string filePath = string.Format("{0}{1}", Server.MapPath("~/files/"), filename);
                    using (FileStream fs = File.OpenWrite(filePath))
                    {
                        Byte[] info;

                        //write title info
                        info = new UTF8Encoding(true).GetBytes(string.Format("\"WebUrl Name: {0}\"{1}\"Search Url: {2}\"{3}\"Run Time: {4:MM/dd/yyyy hh:mm:ss}\"{5}{6}", webUrl.Name, Environment.NewLine, webUrl.Url, Environment.NewLine, dt, Environment.NewLine, Environment.NewLine));
                        fs.Write(info, 0, info.Length);

                        //write fields
                        info = new UTF8Encoding(true).GetBytes(sbFile.ToString());
                        fs.Write(info, 0, info.Length);
                    }
                }
            }
        }
    }
}