using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IlCapo.Models;
using Newtonsoft.Json;

namespace IlCapo.Controllers
{
    public class SidesController : Controller
    {
        private IlCapoContext db = new IlCapoContext();

        // GET: Sides
        public ActionResult Index()
        {
            return View(db.Sides.ToList());
        }

        // GET: Sides/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sides sides = db.Sides.Find(id);
            if (sides == null)
            {
                return HttpNotFound();
            }
            return View(sides);
        }

        // GET: Sides/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sides/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SidesId,Name")] Sides sides)
        {
            if (ModelState.IsValid)
            {
                db.Sides.Add(sides);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sides);
        }

        // GET: Sides/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sides sides = db.Sides.Find(id);
            if (sides == null)
            {
                return HttpNotFound();
            }
            return View(sides);
        }

        // POST: Sides/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SidesId,Name")] Sides sides)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sides).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sides);
        }

        // GET: Sides/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sides sides = db.Sides.Find(id);
            if (sides == null)
            {
                return HttpNotFound();
            }
            return View(sides);
        }

        // POST: Sides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sides sides = db.Sides.Find(id);
            db.Sides.Remove(sides);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public string GetSides()
        {
            var sides = db.Sides.ToList();
            List<object> productsJson = new List<object>();
            foreach (var side in sides)
            {
                Object productJson = new
                {
                    Name = side.Name,
                    Id = side.SidesId
                };

                productsJson.Add(productJson);
            }

            var json = JsonConvert.SerializeObject(productsJson);
            return json;
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
