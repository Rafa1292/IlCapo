using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class WorkDay
    {
        [Key]
        public int WorkDayId { get; set; }

        public bool State { get; set; }

        public virtual BeginDay BeginDay { get; set; }

        public virtual EndDay EndDay { get; set; }

        public int BeginDayId { get; set; }

        public List<Billing> Billings { get; set; }

        public int EndDayId { get; set; }

        public List<Pay> Pays { get; set; }

        public List<Entry> Entries { get; set; }

        public virtual Worker Worker { get; set; }

        public int WorkerId { get; set; }

        public bool IsInWorkingDay(Worker worker)
        {
            bool state = false;

            using (IlCapoContext db = new IlCapoContext())
            {

                if (worker == null )
                {
                    return state;
                }

                BeginDay beginDay = new BeginDay();
                EndDay endDay = new EndDay();

                if (beginDay.IsInBeginDay(worker) && !endDay.IsInEndDay(worker))
                {
                    state = true;
                }

                return state;
            }
        }
    }
}