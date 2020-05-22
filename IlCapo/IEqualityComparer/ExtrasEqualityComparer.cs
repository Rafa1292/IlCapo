using IlCapo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IlCapo.IEqualityComparer
{
    public class ExtrasEqualityComparer : IEqualityComparer<ItemExtra>
    {
        public bool Equals(ItemExtra x, ItemExtra y)
        {
            return (x.ExtraId == y.ExtraId && x.ProductQuantity == y.ProductQuantity);
        }

        public int GetHashCode(ItemExtra itemExtra)
        {
            return itemExtra.GetHashCode();
        }
    }
}