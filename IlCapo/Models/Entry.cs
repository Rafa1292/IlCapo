using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IlCapo.Models
{
    public class Entry
    {
        [Key]
        public int EntryId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public int BeginDayId { get; set; }

        public virtual BeginDay BeginDay { get; set; }

        public List<Entry> GetEntries(int beginDayId, List<Entry> entriesList)
        {
            List<Entry> InitialpaysList = new List<Entry>();

            using (IlCapoContext db = new IlCapoContext())
            {

                var entries = from e in entriesList
                              where e.BeginDayId == beginDayId
                              select e;

                InitialpaysList = entries.ToList();
            }

            return InitialpaysList;
        }

    }
}