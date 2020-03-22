using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; }

        public bool State { get; set; }

        public virtual Client Client { get; set; }

        public int ClientId { get; set; }

        public List<Item> Items { get; set; }

        public int Discount { get; set; }

        public bool Command { get; set; }

        public virtual Worker Worker { get; set; }

        public int WorkerId { get; set; }
    }
}