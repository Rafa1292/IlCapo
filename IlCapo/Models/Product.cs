using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string Name { get; set; }

        public decimal Cost { get; set; }

        public decimal Price { get; set; }

        public bool KitchenMessage { get; set; }

        public bool Sides { get; set; }

        public int SidesQuantity { get; set; }

        public int TotalSales { get; set; }

        public ICollection<Tax> Taxes { get; set; }

        public virtual ProductSubCategory ProductSubCategory { get; set; }

        public int ProductSubCategoryId { get; set; }

        public IEnumerable<Product> Get()
        {
            using (IlCapoContext db = new IlCapoContext())
            {
                IEnumerable<Product> products = db.Products.Include("ProductSubCategory").ToList();
                return products;
            }
        }


    }
}