﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_cashierEntities : DbContext
    {
        public DB_cashierEntities()
            : base("name=DB_cashierEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Invoice_Cancel> Invoice_Cancel { get; set; }
        public virtual DbSet<Invoice_Cancel_Product> Invoice_Cancel_Product { get; set; }
        public virtual DbSet<Invoice_Moshtary> Invoice_Moshtary { get; set; }
        public virtual DbSet<Invoice_Moshtary_Product> Invoice_Moshtary_Product { get; set; }
        public virtual DbSet<Invoice_Moshtary_Sadad> Invoice_Moshtary_Sadad { get; set; }
        public virtual DbSet<Invoice_Mowrid> Invoice_Mowrid { get; set; }
        public virtual DbSet<Invoice_Mowrid_Product> Invoice_Mowrid_Product { get; set; }
        public virtual DbSet<Invoice_Mowrid_Sadad> Invoice_Mowrid_Sadad { get; set; }
        public virtual DbSet<Moshtary> Moshtaries { get; set; }
        public virtual DbSet<Mowrid> Mowrids { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Sadad_Type> Sadad_Type { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAction> UserActions { get; set; }
        public virtual DbSet<UserView> UserViews { get; set; }
        public virtual DbSet<View> Views { get; set; }
    }
}
