using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Item
    {
        [Key]
        public int KeyId { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public virtual Product Product { get; set; }

        public int ProductId { get; set; }

        public virtual Bill Bill { get; set; }

        public int BillId { get; set; }
    }
}