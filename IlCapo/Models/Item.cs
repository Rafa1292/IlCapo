using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public virtual Product Product { get; set; }

        public int ProductId { get; set; }

        public virtual Bill Bill { get; set; }

        public int BillId { get; set; }

        public List<ItemExtra> ItemExtras { get; set; }

        public List<ItemSide> ItemSides { get; set; }

        public List<Extra> GetExtras(Item item)
        {

            using (IlCapoContext db = new IlCapoContext())
            {
                var extras = from e in db.ItemExtras
                             where e.ItemId == item.ItemId
                             select e.Extra;

                return extras.ToList();
            }

        }

    }
}