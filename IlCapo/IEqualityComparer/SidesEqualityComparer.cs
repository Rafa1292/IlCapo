using IlCapo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IlCapo.IEqualityComparer
{
    public class SidesEqualityComparer : IEqualityComparer<ItemSide>
    {
        public bool Equals(ItemSide x, ItemSide y)
        {
            return (x.SidesId == y.SidesId && x.ProductQuantity == y.ProductQuantity);
        }

        public int GetHashCode(ItemSide itemSide)
        {
            return itemSide.GetHashCode();
        }
    }
}