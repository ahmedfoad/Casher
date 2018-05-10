using System;
using System.Collections.Generic;
using System.Linq; using System.Threading.Tasks;
using System.Web;

namespace Casher.Models.Search
{
    public class Srch_Invoice_Mowarid
    {
        
        public int Invoice_From { get; set; }
        public int Invoice_To { get; set; }
        public int Moshtary_id { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Price_From { get; set; }
        public int Price_To { get; set; }

    }
}