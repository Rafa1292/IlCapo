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
    public class EntriesController : Controller
    {
        private IlCapoContext db = new IlCapoContext();

        // GET: Entries
        public ActionResult Index()
        {
            return PartialView("Index", GetValidateEntriesList());
        }


        // GET: Entries/Create
        public ActionResult Create()
        {
            if (!ValidateUser())
            {
                return RedirectToAction("Index");
            }

            return PartialView("Create");
        }


        public ActionResult AddEntry(Entry entry)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser())
                {
                    try
                    {
                        Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
                        BeginDay beginDay = new BeginDay();
                        beginDay = beginDay.GetBeginDay(worker);
                        entry.BeginDayId = beginDay.BeginDayId;
                        db.Entries.Add(entry);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }
            }

            return PartialView("Create", entry);
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

        public List<Entry> GetValidateEntriesList()
        {
            List<Entry> entries = new List<Entry>();
            var entriesList = db.Entries.ToList();

            if (ValidateUser())
            {
                Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
                BeginDay beginDay = new BeginDay();
                beginDay = beginDay.GetBeginDay(worker);
                Entry entry = new Entry();

                entries = entry.GetEntries(beginDay.BeginDayId, entriesList);
            }

            return entries;
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
