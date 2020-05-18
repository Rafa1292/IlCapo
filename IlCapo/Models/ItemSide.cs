using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class ItemSide
    {
        public int ItemSideId { get; set; }

        public virtual Item Item { get; set; }

        public int ItemId { get; set; }

        public virtual Sides Sides { get; set; }

        public int SidesId { get; set; }

        public int ProductQuantity { get; set; }
    }
}