using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class ProductCategory
    {
        [Key]
        public int ProductCategoryId { get; set; }

        public string Name { get; set; }

        public List<ProductSubCategory> ProductSubCategories { get; set; }

    }
}