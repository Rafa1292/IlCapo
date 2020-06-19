using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class EndDay
    {
        [Key]
        public int EndDayId { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }

        [Display(Name = "Dinero en caja")]
        [Required]
        public decimal Cash { get; set; }

        [Display(Name = "Ventas con tarjeta")]
        [Required]
        public decimal CreditCard { get; set; }

        public decimal Dollar { get; set; }

        public virtual Worker Worker { get; set; }

        public int WorkerId { get; set; }

        public bool IsInEndDay(Worker worker)
        {
            bool state = false;

            if (worker == null)
            {
                return state;
            }

            using (IlCapoContext db = new IlCapoContext())
            {
                List<EndDay> endDays = db.EndDays.ToList();

                foreach (var eD in endDays)
                {
                    if (eD.Date.Date == DateTime.Now.Date && eD.Worker.WorkerId == worker.WorkerId)
                    {
                        state = true;
                    }
                }
            }
            return state;
        }

        public List<EndDay> GetEndDays(Worker worker)
        {
            List<EndDay> InitialEnDaysList = new List<EndDay>();

            using (IlCapoContext db = new IlCapoContext())
            {

                var entries = from e in db.EndDays.Include("Worker")
                           where e.WorkerId == worker.WorkerId
                           select e;

                InitialEnDaysList = entries.ToList();
            }

            return InitialEnDaysList;
        }
    }
}