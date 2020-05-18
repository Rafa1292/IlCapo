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
    public class ProductsController : Controller
    {
        private IlCapoContext db = new IlCapoContext();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ProductSubCategory);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductSubCategoryId = new SelectList(db.ProductSubCategories, "ProductSubCategoryId", "Name");
            ViewBag.Taxes = new SelectList(db.Taxes, "TaxId", "Name");
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,Name,Cost,Price,KitchenMessage,ProductSubCategoryId")] Product product, int[] Tax)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                db.Products.Add(product);
                db.SaveChanges();
                bool TaxState = Tax != null ? AddTaxes(Tax, product) : true;

                if (TaxState)
                {
                    transaction.Commit();
                    return RedirectToAction("Index");
                }

                transaction.Rollback();
                ViewBag.Taxes = new SelectList(db.Taxes, "TaxId", "Name");
                ViewBag.ProductSubCategoryId = new SelectList(db.ProductSubCategories, "ProductSubCategoryId", "Name", product.ProductSubCategoryId);
                return View(product);
            }
        }

        public bool AddTaxes(int[] taxesId, Product product)
        {
            ProductTax productTax = new ProductTax();
            List<ProductTax> productTaxes = new List<ProductTax>();

            if (!(taxesId.Count() > 0))
                return true;

            try
            {
                foreach (var tax in taxesId)
                {
                    productTax = new ProductTax();
                    productTax.ProductId = product.ProductId;
                    productTax.TaxId = tax;
                    productTaxes.Add(productTax);
                }

                db.ProductTaxes.AddRange(productTaxes);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductSubCategoryId = new SelectList(db.ProductSubCategories, "ProductSubCategoryId", "Name", product.ProductSubCategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,Name,Cost,Price,KitchenMessage,ProductSubCategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductSubCategoryId = new SelectList(db.ProductSubCategories, "ProductSubCategoryId", "Name", product.ProductSubCategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public string GetProducts()
        {
            var products = db.Products.Include("ProductTaxes").ToList();
            List<object> productsJson = new List<object>();
            foreach (var product in products)
            {
                Object productJson = new
                {
                    Name = product.Name,
                    Id = product.ProductId,
                    Price = product.Price,
                    Message = product.KitchenMessage,
                    Sides = product.Sides,
                    SidesQuantity = 5,
                    Category = product.ProductSubCategory.ProductCategory.Name,
                    SubCategory = product.ProductSubCategory.Name,
                    Taxes = GetTaxes(product)
                };

                productsJson.Add(productJson);
            }

            var json = JsonConvert.SerializeObject(productsJson);
            return json;
        }

        public List<Tax> GetTaxes(Product product)
        {
            List<Tax> taxesList = new List<Tax>();
            var taxes = from t in db.ProductTaxes
                        where t.ProductId == product.ProductId
                        select t.Tax;
            taxesList = taxes.ToList();

            return taxesList;
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
