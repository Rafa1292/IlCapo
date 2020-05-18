using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IlCapo.ModelJson
{
    public class ItemJson
    {
        public int ProductId { get; set; }

        public int ProductQuantity { get; set; }

        public string Observation { get; set; }

        public List<ExtraJson> Extras { get; set; }

        public List<SideJson> Sides { get; set; }

    }
}