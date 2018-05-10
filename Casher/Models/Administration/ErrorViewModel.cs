using System;
using System.Collections.Generic;
using System.Linq; using System.Threading.Tasks;
using System.Web;

namespace Casher.Models.Administration
{
    public class ErrorViewModel
    {
        public string Url { get; set; }
        public string ErrorName { get; set; }
        public string ErrorNumber { get; set; }
        public string ErrorFullNumber { get; set; }

        public string fileName { get; set; }
        public int ID { get; set; }
    }
}