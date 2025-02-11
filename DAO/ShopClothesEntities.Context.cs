﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAO
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class ManagementShopClothesEntities1 : DbContext
    {
        public ManagementShopClothesEntities1()
            : base("name=ManagementShopClothesEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EntrySlip> EntrySlips { get; set; }
        public DbSet<EntrySlipDetail> EntrySlipDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<TypeOfCustomer> TypeOfCustomers { get; set; }
    
        [EdmFunction("ManagementShopClothesEntities1", "TopCustomerBuy")]
        public virtual IQueryable<TopCustomerBuy_Result> TopCustomerBuy(Nullable<bool> checkType, Nullable<System.DateTime> date)
        {
            var checkTypeParameter = checkType.HasValue ?
                new ObjectParameter("checkType", checkType) :
                new ObjectParameter("checkType", typeof(bool));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<TopCustomerBuy_Result>("[ManagementShopClothesEntities1].[TopCustomerBuy](@checkType, @date)", checkTypeParameter, dateParameter);
        }
    
        [EdmFunction("ManagementShopClothesEntities1", "TopSelling")]
        public virtual IQueryable<TopSelling_Result> TopSelling(Nullable<int> quantity)
        {
            var quantityParameter = quantity.HasValue ?
                new ObjectParameter("quantity", quantity) :
                new ObjectParameter("quantity", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<TopSelling_Result>("[ManagementShopClothesEntities1].[TopSelling](@quantity)", quantityParameter);
        }
    
        [EdmFunction("ManagementShopClothesEntities1", "TopStaffSell")]
        public virtual IQueryable<TopStaffSell_Result> TopStaffSell(Nullable<bool> checkType, Nullable<System.DateTime> date)
        {
            var checkTypeParameter = checkType.HasValue ?
                new ObjectParameter("checkType", checkType) :
                new ObjectParameter("checkType", typeof(bool));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<TopStaffSell_Result>("[ManagementShopClothesEntities1].[TopStaffSell](@checkType, @date)", checkTypeParameter, dateParameter);
        }
    }
}
