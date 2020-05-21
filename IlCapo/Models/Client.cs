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

        public int SelectedAddressId { get; set; }

        public Client GetClient(int phone)
        {
            Client client = new Client();
            using (IlCapoContext db = new IlCapoContext())
            {
                var ClientEF = from c in db.Clients.Include("Addresses")
                               where c.Phone == phone
                               select c;
                client = ClientEF.ToList().FirstOrDefault();
            }

            return client;
        }

        public Client AddClient(int phone, string name)
        {
            Client client = new Client();

            using (IlCapoContext db = new IlCapoContext())
            {
                var ClientEF = from c in db.Clients
                               where c.Phone == phone
                               select c;
                client = ClientEF.ToList().FirstOrDefault();

                if (client == null)
                {
                    client = new Client();
                    client.Name = name;
                    client.Phone = phone;
                    db.Clients.Add(client);
                    db.SaveChanges();
                }
            }

            return client;
        }
    }
}