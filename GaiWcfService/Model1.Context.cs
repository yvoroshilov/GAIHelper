﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GaiWcfService
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
    
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Violation> Violations { get; set; }
        public virtual DbSet<ViolationType> ViolationTypes { get; set; }
    }
}
