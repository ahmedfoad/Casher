using System;
using System.Collections.Generic;
using System.Linq; using System.Threading.Tasks;
using System.Web;

namespace Casher.Models
{
    public class ClsProduct
    {
        public int ID { get; set; }
        public string Name { get; set; }
       
         
        public string Department_Name { get; set; }
        public decimal Price_Unit { get; set; }
        public decimal Taxes_Price { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Barcode { get; set; }

       
        public int Company_Id { get; set; }
        public string Company_Name { get; set; }

        public decimal Price_Jumla { get; set; }
        public int Jumla_picesCount { get; set; }
       
        public string  User_Name { get; set; }
        public string ComputerName { get; set; }
        public string ComputerUser { get; set; }
        public string Carton_Barcode { get; set; }
        public string Carton_ProCount { get; set; }
        public string Date_Expiration { get; set; }
        public decimal Price_Mowrid { get; set; }
        public Nullable<System.DateTime> InDate { get; set; }

        


    }
}