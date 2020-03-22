using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Billing
    {
        [Key]
        public int BillingId { get; set; }

        public List<Bill> Bills { get; set; }

        public virtual Worker Worker { get; set; }

        public int WorkerId { get; set; }
    }
}