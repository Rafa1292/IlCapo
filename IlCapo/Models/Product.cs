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

        public virtual ProductSubCategory ProductSubCategory { get; set; }

        public int ProductSubCategoryId { get; set; }


    }
}