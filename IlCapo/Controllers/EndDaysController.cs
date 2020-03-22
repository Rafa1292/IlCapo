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
            var endDays = db.EndDays.Include(e => e.Worker);
            return View(endDays.ToList());
        }

        // GET: EndDays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndDay endDay = db.EndDays.Find(id);
            if (endDay == null)
            {
                return HttpNotFound();
            }
            return View(endDay);
        }

        // GET: EndDays/Create
        public ActionResult Create()
        {
            ViewBag.WorkerId = new SelectList(db.Workers, "WorkerId", "Name");
            return View();
        }

        // POST: EndDays/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EndDayId,CloseHour,Date,Cash,WorkerId")] EndDay endDay)
        {
            if (ModelState.IsValid)
            {
                db.EndDays.Add(endDay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.WorkerId = new SelectList(db.Workers, "WorkerId", "Name", endDay.WorkerId);
            return View(endDay);
        }

        // GET: EndDays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndDay endDay = db.EndDays.Find(id);
            if (endDay == null)
            {
                return HttpNotFound();
            }
            ViewBag.WorkerId = new SelectList(db.Workers, "WorkerId", "Name", endDay.WorkerId);
            return View(endDay);
        }

        // POST: EndDays/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EndDayId,CloseHour,Date,Cash,WorkerId")] EndDay endDay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(endDay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WorkerId = new SelectList(db.Workers, "WorkerId", "Name", endDay.WorkerId);
            return View(endDay);
        }

        // GET: EndDays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndDay endDay = db.EndDays.Find(id);
            if (endDay == null)
            {
                return HttpNotFound();
            }
            return View(endDay);
        }

        // POST: EndDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EndDay endDay = db.EndDays.Find(id);
            db.EndDays.Remove(endDay);
            db.SaveChanges();
            return RedirectToAction("Index");
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
