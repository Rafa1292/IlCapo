using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class BasicServices
    {
        [Key]
        public int BasicServicesId { get; set; }

        public string Name { get; set; }

        public DateTime Expiration { get; set; }

        public decimal Amount { get; set; }

        public List<BasicServices> GetBasicServices()
        {
            List<BasicServices> basicServices = new List<BasicServices>();

            using (IlCapoContext db = new IlCapoContext())
            {
                basicServices = db.BasicServices.ToList();
            }

            return basicServices;
        }
    }
}