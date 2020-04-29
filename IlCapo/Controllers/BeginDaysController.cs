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
    public class BeginDaysController : Controller
    {
        private IlCapoContext db = new IlCapoContext();

        // GET: BeginDays
        public ActionResult Index()
        {
            var beginDays = db.BeginDays.Include(b => b.Worker).OrderByDescending(x => x.Date).ToList();
            return PartialView("Index", beginDays.ToList());
        }

        // GET: BeginDays/Create
        public ActionResult Create()
        {
            if (!ValidateUser())
            {
                return PartialView("Index", db.BeginDays.ToList());
            }

            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
            BeginDay beginDay = new BeginDay();
            beginDay.Worker = worker;
            return PartialView("Create", beginDay);
        }

        // POST: BeginDays/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        public ActionResult AddBeginDay(BeginDay beginDay)
        {
            if (!ValidateUser())
            {
                return PartialView("Index", db.BeginDays.ToList());
            }

            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
            beginDay.Worker = worker;
            beginDay.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.BeginDays.Add(beginDay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return PartialView("Create", beginDay);
        }

        public bool ValidateUser()
        {
            if (!ValidateWorker())
            {
                return false;
            }

            if (!ValidateBeginDay())
            {
                return false;
            }

            return true;
        }

        public bool ValidateBeginDay()
        {
            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
            BeginDay beginDay = new BeginDay();

            if (beginDay.IsInBeginDay(worker))
            {
                ViewBag.error = "Ya has iniciado una jornada hoy!!!";
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
