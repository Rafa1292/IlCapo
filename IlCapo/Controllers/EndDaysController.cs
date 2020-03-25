using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IlCapo.Models;

namespace IlCapo.Controllers
{
    public class EndDaysController : Controller
    {
        private IlCapoContext db = new IlCapoContext();

        // GET: EndDays
        public ActionResult Index()
        {

            return PartialView("Index", GetValidateEndDaysList());

        }


        public ActionResult Create()
        {
            if (!ValidateUser())
            {
                return RedirectToAction("Index");
            }

            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
            BeginDay beginDay = new BeginDay();
            Pay pay = new Pay();
            List<Pay> pays = pay.GetPays(beginDay.GetBeginDayId(worker), db.Pays.Include( p => p.Provider).ToList());
            ViewBag.pays = pays;
            Entry entry = new Entry();
            List<Entry> entries = entry.GetEntries(beginDay.GetBeginDayId(worker), db.Entries.ToList());
            ViewBag.entries = entries;            
            EndDay endDay = new EndDay();
            endDay.Worker = worker;


            return PartialView("Create", endDay);
        }

        public ActionResult AddEndDay(EndDay endDay)
        {
            if (!ValidateUser())
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
                endDay.WorkerId = worker.WorkerId;
                db.EndDays.Add(endDay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WorkerId = new SelectList(db.Workers, "WorkerId", "Name", endDay.WorkerId);
            return PartialView("Create", endDay);
        }

        public bool ValidateUser()
        {
            if (!ValidateWorker())
            {
                return false;
            }

            if (!ValidateWorkerDay())
            {
                return false;
            }

            return true;
        }

        public bool ValidateWorkerDay()
        {
            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
            WorkDay workDay = new WorkDay();

            if (!workDay.IsInWorkingDay(worker))
            {
                ViewBag.error = "Debes abrir una jornada antes!!!";
                return false;
            }

            return true;
        }

        public bool ValidateWorker()
        {
            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);

            if (worker == null)
            {
                ViewBag.error = "Debes iniciar sesion antes!!!";
                return false;
            }

            return true;
        }

        public List<EndDay> GetValidateEndDaysList()
        {
            List<EndDay> endDays = new List<EndDay>();

            if (ValidateUser())
            {
                Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
                EndDay endDay = new EndDay();
                endDays = endDay.GetEndDays(worker);
            }

            return endDays;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
