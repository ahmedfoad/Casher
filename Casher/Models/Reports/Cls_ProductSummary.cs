using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Casher.Models.Reports
{
    public class Cls_ProductSummary
    {
        public string Product_Name { get; set; }
        public string Company_Name { get; set; }
        public string Department_Name { get; set; }
        public int Prev_Amount { get; set; }
        public int Sell_Amount { get; set; }
        public int Current_Amount { get; set; }

    }

   
}