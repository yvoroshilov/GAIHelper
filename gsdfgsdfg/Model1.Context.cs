﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace gsdfgsdfg
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GAIDBEntities : DbContext
    {
        public GAIDBEntities()
            : base("name=GAIDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Shifts> Shifts { get; set; }
        public virtual DbSet<Violations> Violations { get; set; }
        public virtual DbSet<ViolationTypes> ViolationTypes { get; set; }
        public virtual DbSet<Violators> Violators { get; set; }
    }
}
