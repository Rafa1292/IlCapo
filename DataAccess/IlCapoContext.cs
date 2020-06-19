using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class IlCapoContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(
                    "Server=DESKTOP-T74OE0I; Database=IlCapo; User Id=" +
                    "DESKTOP-T74OE0I\rvill; Password=");
            }
        }

        public DbSet<IlCapo.Models.ProductCategory> ProductCategories { get; set; }

        public DbSet<IlCapo.Models.ProductSubCategory> ProductSubCategories { get; set; }

        public DbSet<IlCapo.Models.Product> Products { get; set; }

        public DbSet<IlCapo.Models.BeginDay> BeginDays { get; set; }

        public DbSet<IlCapo.Models.Worker> Workers { get; set; }

        public DbSet<IlCapo.Models.Pay> Pays { get; set; }

        public DbSet<IlCapo.Models.Provider> Providers { get; set; }

        public DbSet<IlCapo.Models.BasicServices> BasicServices { get; set; }

        public DbSet<IlCapo.Models.EndDay> EndDays { get; set; }

        public DbSet<IlCapo.Models.Entry> Entries { get; set; }

        public DbSet<IlCapo.Models.Bill> Bills { get; set; }

        public DbSet<IlCapo.Models.Client> Clients { get; set; }

        public DbSet<IlCapo.Models.Tax> Taxes { get; set; }

        public DbSet<IlCapo.Models.ProductTax> ProductTaxes { get; set; }

        public DbSet<IlCapo.Models.Item> Items { get; set; }

        public DbSet<IlCapo.Models.Sides> Sides { get; set; }

        public DbSet<IlCapo.Models.Extra> Extras { get; set; }

        public DbSet<IlCapo.Models.Address> Addresses { get; set; }

        public DbSet<IlCapo.Models.ItemExtra> ItemExtras { get; set; }

        public DbSet<IlCapo.Models.ItemSide> ItemSides { get; set; }

        public DbSet<IlCapo.Models.WorkDay> WorkDays { get; set; }
    }
}
