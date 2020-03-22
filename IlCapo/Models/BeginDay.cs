using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class BeginDay
    {
        [Key]
        public int BeginDayId { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }

        [Display(Name = "Caja inicial")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public decimal Cash { get; set; }

        public virtual Worker Worker { get; set; }

        public int WorkerId { get; set; }

        public bool IsInBeginDay(Worker worker)
        {
            bool state = false;

            if (worker == null)
            {
                return state;
            }

            using (IlCapoContext db = new IlCapoContext())
            {
                List<BeginDay> beginDays = db.BeginDays.ToList();

                foreach (var bD in beginDays)
                {
                    if (bD.Date.Date == DateTime.Now.Date && bD.Worker.WorkerId == worker.WorkerId)
                    {
                        state = true;
                    }
                }
            }

            return state;
        }

        public int GetBeginDayId(Worker worker)
        {
            int id = 0;

            if (worker == null)
            {
                return id;
            }

            using (IlCapoContext db = new IlCapoContext())
            {
                List<BeginDay> beginDays = db.BeginDays.ToList();

                foreach (var bD in beginDays)
                {
                    if (bD.Date.Date == DateTime.Now.Date && bD.Worker.WorkerId == worker.WorkerId)
                    {
                        return bD.BeginDayId;
                    }
                }
            }

            return id;
        }

    }
}