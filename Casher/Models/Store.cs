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
    
    public partial class Store
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Store()
        {
            this.Invoice_Cancel_Product = new HashSet<Invoice_Cancel_Product>();
            this.Invoice_Mowrid_Product = new HashSet<Invoice_Mowrid_Product>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> User_ID { get; set; }
        public string ComputerName { get; set; }
        public string ComputerUser { get; set; }
        public Nullable<System.DateTime> InDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice_Cancel_Product> Invoice_Cancel_Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice_Mowrid_Product> Invoice_Mowrid_Product { get; set; }
        public virtual User User { get; set; }
    }
}
