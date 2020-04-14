using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class ProductTax
    {
        public int ProductTaxId { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int TaxId { get; set; }

        public virtual Tax Tax { get; set; }


    }
}