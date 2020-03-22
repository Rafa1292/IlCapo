using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        public virtual Client Client { get; set; }

        public int ClientId { get; set; }

        public string Description { get; set; }
    }
}