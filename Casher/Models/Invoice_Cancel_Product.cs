//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Casher.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Invoice_Cancel_Product
    {
        public int ID { get; set; }
        public int Invoice_Cancel_Id { get; set; }
        public int Product_Id { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalPrice { get; set; }
        public int Store_id { get; set; }
        public System.DateTime Date_Poduction { get; set; }
        public string Date_Poduction_Hijri { get; set; }
        public System.DateTime Date_Expiration { get; set; }
        public string Date_Expiration_Hijri { get; set; }
    
        public virtual Invoice_Cancel Invoice_Cancel { get; set; }
        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
    }
}
