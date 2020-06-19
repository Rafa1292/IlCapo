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

        public Address AddAddress(Client client, string address)
        {
            Address newAddress = new Address();
            using (IlCapoContext db = new IlCapoContext())
            {
                if (client != null)
                {
                    newAddress.ClientId = client.ClientId;
                    newAddress.Description = address;
                    db.Addresses.Add(newAddress);
                    db.SaveChanges();
                }

            }

            return newAddress;
        }

        public List<Address> GetAddresses(Client client)
        {
            List<Address> addresses= new List<Address>();

            using (IlCapoContext db = new IlCapoContext())
            {
                if (client != null)
                {
                    var addressesEF = from a in db.Addresses
                                      where a.ClientId == client.ClientId
                                      select a;
                    addresses = addressesEF.ToList();
                }
            }

            return addresses;
        }


        public Address GetAddress(int id)
        {
            Address address = new Address();

            using (IlCapoContext db = new IlCapoContext())
            {
                address = db.Addresses.Find(id);
            }

            if (address == null)
            {
                address = new Address();
            }

            return address;

        }
    }
}