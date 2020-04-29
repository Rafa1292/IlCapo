using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Extra
    {
        [Key]
        public int ExtraId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal cost { get; set; }
    }
}