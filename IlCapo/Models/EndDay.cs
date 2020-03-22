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

        public DateTime CloseHour { get; set; }

        public DateTime Date { get; set; }

        public decimal Cash { get; set; }

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
    }
}