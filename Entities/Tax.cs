using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Tax
    {
        [Key]
        public int TaxId { get; set; }

        public string Name { get; set; }

        public int Percentage { get; set; }

    }
}