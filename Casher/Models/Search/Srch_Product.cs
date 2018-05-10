using System;
using System.Collections.Generic;
using System.Linq; using System.Threading.Tasks;
using System.Web;

namespace Casher.Models.Search
{
    public class Srch_Product
    {
        public string Product_Name { get; set; }
        public int Department_id { get; set; }
        public int Company_id { get; set; }
        public int Price_From { get; set; }
        public int Price_To { get; set; }
        public int Barcode_From { get; set; }
        public int Barcode_To { get; set; }
        public int Taxes { get; set; }

    }
}