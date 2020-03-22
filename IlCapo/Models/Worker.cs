using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Worker
    {
        public int WorkerId { get; set; }

        [Display(Name = "Colaborador")]
        public string Name { get; set; }

        public string Mail { get; set; }

        public List<Worker> GetWorkers()
        {
            List<Worker> workers = new List<Worker>();

            using (IlCapoContext db = new IlCapoContext())
            {
                workers = db.Workers.ToList();
            }

            return workers;
        }

    }
}