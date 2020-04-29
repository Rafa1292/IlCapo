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

        public bool AddAddress(Client client, string address)
        {
            bool result = false;
            using (IlCapoContext db = new IlCapoContext())
            {
                if (client != null)
                {
                    Address newAddress = new Address();
                    newAddress.ClientId = client.ClientId;
                    newAddress.Description = address;
                    db.Addresses.Add(newAddress);
                    db.SaveChanges();
                    result = true;
                }
                
            }

            return result;
        }
    }
}