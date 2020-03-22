using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Provider
    {
        [Key]
        public int ProviderId { get; set; }

        [Display(Name = "Proveedor")]
        public string Name { get; set; }

        public int Phone { get; set; }

        public List<Provider> GetProviders()
        {
            List<Provider> providers = new List<Provider>();

            using (IlCapoContext db = new IlCapoContext())
            {
                providers = db.Providers.ToList();
            }

            return providers;
        }

    }
}