using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

public class IlCapoContext : DbContext
{

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
    }
    // You can add custom code to this file. Changes will not be overwritten.
    // 
    // If you want Entity Framework to drop and regenerate your database
    // automatically whenever you change your model schema, please use data migrations.
    // For more information refer to the documentation:
    // http://msdn.microsoft.com/en-us/data/jj591621.aspx

    public IlCapoContext() : base("name=IlCapoContext")
    {

    }

    public System.Data.Entity.DbSet<IlCapo.Models.ProductCategory> ProductCategories { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.ProductSubCategory> ProductSubCategories { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.Product> Products { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.BeginDay> BeginDays { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.Worker> Workers { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.Pay> Pays { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.Provider> Providers { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.BasicServices> BasicServices { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.EndDay> EndDays { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.Entry> Entries { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.Bill> Bills { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.Client> Clients { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.Tax> Taxes { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.ProductTax> ProductTaxes { get; set; }

    public DbSet<IlCapo.Models.Item> Items { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.Sides> Sides { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.Extra> Extras { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.Address> Addresses { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.ItemExtra> ItemExtras { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.ItemSide> ItemSides { get; set; }

    public System.Data.Entity.DbSet<IlCapo.Models.WorkDay> WorkDays { get; set; }


}
