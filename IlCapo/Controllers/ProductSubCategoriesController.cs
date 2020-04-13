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
    public class ProductSubCategoriesController : Controller
    {
        private IlCapoContext db = new IlCapoContext();

        // GET: ProductSubCategories
        public ActionResult Index()
        {
            var productSubCategories = db.ProductSubCategories.Include(p => p.ProductCategory);
            return View(productSubCategories.ToList());
        }

        // GET: ProductSubCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSubCategory productSubCategory = db.ProductSubCategories.Find(id);
            if (productSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(productSubCategory);
        }

        // GET: ProductSubCategories/Create
        public ActionResult Create()
        {
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "Name");
            return View();
        }

        // POST: ProductSubCategories/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductSubCategoryId,Name,ProductCategoryId")] ProductSubCategory productSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.ProductSubCategories.Add(productSubCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "Name", productSubCategory.ProductCategoryId);
            return View(productSubCategory);
        }

        // GET: ProductSubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSubCategory productSubCategory = db.ProductSubCategories.Find(id);
            if (productSubCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "Name", productSubCategory.ProductCategoryId);
            return View(productSubCategory);
        }

        // POST: ProductSubCategories/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductSubCategoryId,Name,ProductCategoryId")] ProductSubCategory productSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productSubCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "ProductCategoryId", "Name", productSubCategory.ProductCategoryId);
            return View(productSubCategory);
        }

        // GET: ProductSubCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductSubCategory productSubCategory = db.ProductSubCategories.Find(id);
            if (productSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(productSubCategory);
        }

        // POST: ProductSubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductSubCategory productSubCategory = db.ProductSubCategories.Find(id);
            db.ProductSubCategories.Remove(productSubCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public string GetProducts(int productSubCategoryId)
        {
            var json = "";
            Product product = new Product();
            var products = from p in product.Get()
                           where p.ProductSubCategoryId == productSubCategoryId
                           select p;
            List<Object> productsList = new List<object>();

            foreach (var item in products.ToList())
            {
                var o = new { Id = item.ProductId, Name = item.Name, SubCategory = item.ProductSubCategory.Name };
                productsList.Add(o);
            }

            json = JsonConvert.SerializeObject(productsList);

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
