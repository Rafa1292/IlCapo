using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; }

        public bool State { get; set; }

        public int TableId { get; set; }

        public virtual Client Client { get; set; }

        public int ClientId { get; set; }

        public List<Item> Items { get; set; }

        public int Discount { get; set; }

        public bool Command { get; set; }

        public virtual BeginDay BeginDay { get; set; }

        public int BeginDayId { get; set; }

        public Bill tableContent(int tableId, int beginDayId)
        {
            Bill bill = new Bill();


            using (IlCapoContext db = new IlCapoContext())
            {
                var bills = from b in db.Bills
                            where b.BeginDayId == beginDayId && b.TableId == tableId && b.State
                            select b;
                bill = bills.FirstOrDefault();
            }
            return bill;
        }
    }
}