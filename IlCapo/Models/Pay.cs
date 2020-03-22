using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Pay
    {
        [Key]
        public int PayId { get; set; }

        [Display(Name = "Monto")]
        public decimal Amount { get; set; }

        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Display(Name = "Tipo de pago")]
        public PayType PayType { get; set; }

        public int? ProviderId { get; set; }

        public virtual Provider Provider { get; set; }

        public int? WorkerId { get; set; }

        public virtual Worker Worker { get; set; }

        public int? BasicServicesId { get; set; }

        public virtual BasicServices basicServices { get; set; }

        public int BeginDayId { get; set; }

        public virtual BeginDay BeginDay { get; set; }

        public List<Pay> GetPays(int beginDayId, List<Pay> paysList)
        {
            List<Pay> InitialpaysList = new List<Pay>();

            using (IlCapoContext db = new IlCapoContext())
            {

                var pays = from p in paysList
                           where p.BeginDayId == beginDayId
                           select p;

                InitialpaysList = pays.ToList();
            }

            return InitialpaysList;
        }
    }
}