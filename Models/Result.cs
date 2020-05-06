using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpidySense.Models
{
    public class Result
    {
        public Result() {
            Success = false;
        }

        public bool Success { get; set; }
        public Exception Ex { get; set; }
        public string UserMsg { get; set; }
        public string LogMsg { get; set; }
    }
}