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
    public class BillsController : Controller
    {
        private IlCapoContext db = new IlCapoContext();

        // GET: Bills
        public ActionResult Index()
        {
            return View();
        }

        // GET: Bills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // GET: Bills/Create
        public ActionResult Create(int tableId, bool toGo)
        {
            if (!ValidateUser())
            {
                return PartialView("advertisement");
            }

            Bill bill = new Bill();
            BeginDay beginDay = new BeginDay();
            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name); 

            var billContent = bill.tableContent(tableId, beginDay.GetBeginDayId(worker));

            if (billContent != null)
            {
                bill = billContent;
            }
            else
            {
                bill = GetEmptyBill();
            }

            ViewBag.ToGo = toGo.ToString();
            Product product = new Product();
            ViewBag.Favorites = product.Get().OrderBy(p => p.TotalSales).ToList();
            ViewBag.Category = db.ProductCategories.ToList();            
            return PartialView("TableMenu/TableMenu", bill);
        }

        // POST: Bills/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BillId,State,TableId,ClientId,Discount,Command,WorkerId")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Bills.Add(bill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "Name", bill.ClientId);
            return View(bill);
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

        public Bill GetEmptyBill()
        {
            Bill bill = new Bill();
            Client emptyClient = new Client { Name = "", Phone = 0 };
            Address emptyAddress = new Address { Description = "", Client = emptyClient };
            List<Item> emptyItems = new List<Item>();
            bill.Client = emptyClient;
            bill.Address = emptyAddress;
            bill.Items = emptyItems;

            return bill;
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

        public ActionResult SliceAccount(int quantity, int price)
        {
            SliceAccount sliceAccount = new SliceAccount();
            sliceAccount.Price = price / quantity;
            sliceAccount.Quantity = quantity;

            return PartialView("BillParts/SliceAccount", sliceAccount);
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
