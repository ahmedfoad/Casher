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
    
    public partial class Invoice_Moshtary_Sadad
    {
        public int ID { get; set; }
        public int Invoice_Id { get; set; }
        public System.DateTime Date_Added { get; set; }
        public decimal Money { get; set; }
        public int Sadad_Type_Id { get; set; }
        public int User_ID { get; set; }
        public string ComputerName { get; set; }
        public string ComputerUser { get; set; }
        public System.DateTime InDate { get; set; }
    
        public virtual Invoice_Moshtary Invoice_Moshtary { get; set; }
        public virtual Sadad_Type Sadad_Type { get; set; }
        public virtual User User { get; set; }
    }
}