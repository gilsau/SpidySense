using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpidySense.Models
{
    public class WebsiteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public List<WebUrlModel> WebUrls { get; set; }
    }
        
    public class WebUrlModel {
        public string Name { get; set; }
        public string Url { get; set; }
        public string RowSelector { get; set; }
        public List<WebUrlFieldModel> WebUrlFields { get; set; }
    }

    public class WebUrlFieldModel
    {
        public string Name { get; set; }
        public string Selector { get; set; }
    }
}