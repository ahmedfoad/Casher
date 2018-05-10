using System;
using System.Collections.Generic;
using System.Linq; using System.Threading.Tasks;
using System.Web;

namespace Casher.Models
{
    public class Cls_Invoice_Moshtary
    {
        public int ID { get; set; }
        public Nullable<int> Moshtary_id { get; set; }
        public string Moshtary_Name { get; set; }
        public string Date_Invoice { get; set; }
        public DateTime _Date_Invoice { get; set; }
        public string Date_Invoice_Hijri { get; set; }
        public decimal Price { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Total_Sadad { get; set; }
        public int User_ID { get; set; }
        public string User_EmpNAME { get; set; }
        public string ComputerName { get; set; }
        public string ComputerUser { get; set; }
        public string InDate { get; set; }
      
        public List<ClsInvoiceMoshtary_Product> ClsInvoiceMoshtary_Product { get; set; }
    }

    public partial class ClsInvoiceMoshtary_Product
    {
        public int ID { get; set; }
        public int Invoice_Moshtary_Id { get; set; }
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }

    }
}