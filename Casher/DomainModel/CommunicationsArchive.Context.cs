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
    
    public partial class CasherArchiveEntities : DbContext
    {
        public CasherArchiveEntities()
            : base("name=CasherArchiveEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<IMAGES_EXPORT> IMAGES_EXPORT { get; set; }
        public DbSet<IMAGES_IMPORT> IMAGES_IMPORT { get; set; }
    }
}
