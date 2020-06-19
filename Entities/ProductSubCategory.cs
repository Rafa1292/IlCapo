using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class ProductSubCategory
    {
        [Key]
        public int ProductSubCategoryId { get; set; }

        public string Name { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }

        public int ProductCategoryId { get; set; }

        public List<Product> Products { get; set; }

        public IEnumerable<ProductSubCategory> Get()
        {
            using (IlCapoContext db = new IlCapoContext())
            {
                IEnumerable<ProductSubCategory> productSubCategory = db.ProductSubCategories.Include("ProductCategory").ToList();
                return productSubCategory;
            }
        }
    }
}