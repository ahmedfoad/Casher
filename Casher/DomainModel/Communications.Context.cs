﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Casher.DomainModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CasherEntities : DbContext
    {
        public CasherEntities()
            : base("name=CasherEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<COM_TREATMENT_TYPE> COM_TREATMENT_TYPE { get; set; }
        public DbSet<COM_SUBJECTS> COM_SUBJECTS { get; set; }
        public DbSet<COM_DEPARTMENTS> COM_DEPARTMENTS { get; set; }
        public DbSet<BLADIAINFO> BLADIAINFO { get; set; }
        public DbSet<COM_LIST_EXPORT> COM_LIST_EXPORT { get; set; }
        public DbSet<COM_EMPLOYEES> COM_EMPLOYEES { get; set; }
        public DbSet<COM_EMPLOYEES_VIEWS> COM_EMPLOYEES_VIEWS { get; set; }
        public DbSet<COM_EMPLOYEES_AUTHENTICATION> COM_EMPLOYEES_AUTHENTICATION { get; set; }
        public DbSet<COM_TREATMENT_COPY> COM_TREATMENT_COPY { get; set; }
        public DbSet<COM_LIST_IMPORT_REFER> COM_LIST_IMPORT_REFER { get; set; }
        public DbSet<COM_LIST_EXPORT_REFER> COM_LIST_EXPORT_REFER { get; set; }
        public DbSet<COM_PAYMENT_REGISTRATION> COM_PAYMENT_REGISTRATION { get; set; }
        public DbSet<COM_LIST_IMPORT> COM_LIST_IMPORT { get; set; }
        public DbSet<ACTIONS> ACTIONS { get; set; }
    }
}