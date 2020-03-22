using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        public string Name { get; set; }

        public int Phone { get; set; }

        public List<Address> Addresses { get; set; }
    }
}