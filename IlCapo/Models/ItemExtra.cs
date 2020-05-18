using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class ItemExtra
    {
        [Key]
        public int ItemExtraId { get; set; }

        public virtual Item Item { get; set; }

        public int ItemId { get; set; }

        public virtual Extra Extra { get; set; }

        public int ExtraId { get; set; }

        public int ProductQuantity { get; set; }

        public int Quantity { get; set; }

        public ItemExtra CreateItemExtra(int itemId, int extraId, int quantity)
        {
            ItemExtra itemExtra = new ItemExtra();

            using (IlCapoContext db = new IlCapoContext())
            {
                itemExtra.ItemId = itemId;
                itemExtra.ExtraId = extraId;
                itemExtra.Quantity = quantity;
                db.ItemExtras.Add(itemExtra);
                db.SaveChanges();
            }

            return itemExtra;
        }
    }
}