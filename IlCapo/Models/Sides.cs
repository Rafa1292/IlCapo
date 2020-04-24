using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Sides
    {
        [Key]
        public int SidesId { get; set; }

        public string Name { get; set; }


    }
}