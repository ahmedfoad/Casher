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
    
    public partial class Invoice_Mowrid
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice_Mowrid()
        {
            this.Invoice_Mowrid_Product = new HashSet<Invoice_Mowrid_Product>();
            this.Invoice_Mowrid_Sadad = new HashSet<Invoice_Mowrid_Sadad>();
        }
    
        public int ID { get; set; }
        public int Mowrid_id { get; set; }
        public System.DateTime Date_Invoice { get; set; }
        public string Date_Invoice_Hijri { get; set; }
        public decimal Price { get; set; }
        public decimal Total_Sadad { get; set; }
        public int User_ID { get; set; }
        public string ComputerName { get; set; }
        public string ComputerUser { get; set; }
        public System.DateTime InDate { get; set; }
    
        public virtual Mowrid Mowrid { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice_Mowrid_Product> Invoice_Mowrid_Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice_Mowrid_Sadad> Invoice_Mowrid_Sadad { get; set; }
        public virtual User User { get; set; }
    }
}
