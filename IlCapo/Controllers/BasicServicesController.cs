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
    public class BasicServicesController : Controller
    {
        private IlCapoContext db = new IlCapoContext();

        // GET: BasicServices
        public ActionResult Index()
        {
            return View(db.BasicServices.ToList());
        }

        // GET: BasicServices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BasicServices basicServices = db.BasicServices.Find(id);
            if (basicServices == null)
            {
                return HttpNotFound();
            }
            return View(basicServices);
        }

        // GET: BasicServices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BasicServices/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BasicServicesId,Name,Expiration,Amount")] BasicServices basicServices)
        {
            if (ModelState.IsValid)
            {
                db.BasicServices.Add(basicServices);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(basicServices);
        }

        // GET: BasicServices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BasicServices basicServices = db.BasicServices.Find(id);
            if (basicServices == null)
            {
                return HttpNotFound();
            }
            return View(basicServices);
        }

        // POST: BasicServices/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BasicServicesId,Name,Expiration,Amount")] BasicServices basicServices)
        {
            if (ModelState.IsValid)
            {
                db.Entry(basicServices).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(basicServices);
        }

        // GET: BasicServices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BasicServices basicServices = db.BasicServices.Find(id);
            if (basicServices == null)
            {
                return HttpNotFound();
            }
            return View(basicServices);
        }

        // POST: BasicServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BasicServices basicServices = db.BasicServices.Find(id);
            db.BasicServices.Remove(basicServices);
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
