﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web_Api_EPamphalet.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Epamphalet_dbEntities4 : DbContext
    {
        public Epamphalet_dbEntities4()
            : base("name=Epamphalet_dbEntities4")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Fence> Fences { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
    }
}
