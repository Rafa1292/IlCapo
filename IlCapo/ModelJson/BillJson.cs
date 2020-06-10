using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IlCapo.ModelJson
{
    public class BillJson
    {
        public int Phone { get; set; }

        public bool ToGo { get; set; }

        public bool Express { get; set; }

        public int Discount { get; set; }

        public int TableId { get; set; }

        public int Address { get; set; }

        public int SubTotal { get; set; }

        public int Total { get; set; }

        public int Taxes { get; set; }

        public int ExtrasAmount { get; set; }

        public int DiscountAmount { get; set; }

        public List<ItemJson> Items { get; set; }
    }
}