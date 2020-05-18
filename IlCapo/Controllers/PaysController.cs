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
    public class PaysController : Controller
    {
        public IlCapoContext db = new IlCapoContext();

        // GET: Pays
        public ActionResult Index()
        {
            return PartialView("Index", GetValidatePaysList());
        }


        // GET: Pays/Create
        public ActionResult Create()
        {

            if (!ValidateUser())
            {
                return RedirectToAction("Index");
            }

            ViewBag.ProviderId = new SelectList(db.Providers, "ProviderId", "Name");
            ViewBag.PayTypes = new SelectList(Enum.GetValues(typeof(PayType)));
            return PartialView("Create");
        }


        public ActionResult AddPay( Pay pay, int payName)
        {
            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);

            if (!ValidateUser())
            {
                return RedirectToAction("Index");
            }

            BeginDay beginDay = new BeginDay();
            beginDay = beginDay.GetBeginDay(worker);
            Pay tempPay = SetPayType(pay.PayType, payName);
            pay.basicServices = tempPay.basicServices;
            pay.Provider = tempPay.Provider;
            pay.Worker = tempPay.Worker;
            pay.BeginDayId = beginDay.BeginDayId;

            if (ModelState.IsValid)
            {
                db.Pays.Add(pay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProviderId = new SelectList(db.Providers, "ProviderId", "Name", pay.ProviderId);
            return PartialView("Create", pay);
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

        public List<Pay> GetValidatePaysList()
        {
            List<Pay> pays = new List<Pay>();
            var paysdb = db.Pays.Include(p => p.Provider).ToList();

            if (ValidateUser())
            {
                Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
                BeginDay beginDay = new BeginDay();
                beginDay = beginDay.GetBeginDay(worker);
                Pay pay = new Pay();

                pays = pay.GetPays(beginDay.BeginDayId, paysdb);
            }

            ViewBag.Provider = db.Providers.ToList();
            return pays;
        }

        public string GetTypes(string payType)
        {
            var json = "";

            switch (payType)
            {
                case "Proveedor":
                    var Providers = new SelectList(db.Providers, "ProviderId", "Name");
                    json = JsonConvert.SerializeObject(Providers);
                    break;
                case "Servicios":
                    var BasicServices = new SelectList(db.BasicServices, "BasicServicesId", "Name");
                    json = JsonConvert.SerializeObject(BasicServices);
                    break;
                case "Salarios":
                    var Workers = new SelectList(db.Workers, "WorkerId", "Name");
                    json = JsonConvert.SerializeObject(Workers);
                    break;
                default:
                    break;
            }


            return json;
        }

        public Pay SetPayType(PayType payType, int payTypeId)
        {
            Pay pay = new Pay();

            try
            {
                switch (payType)
                {
                    case PayType.Proveedor:
                        var provider = db.Providers.FirstOrDefault(p => p.ProviderId == payTypeId);
                        pay.Provider = provider;
                        break;
                    case PayType.Servicios:
                        var basicService = db.BasicServices.FirstOrDefault(b => b.BasicServicesId == payTypeId);
                        pay.basicServices = basicService;
                        break;
                    case PayType.Salarios:
                        var worker = db.Workers.FirstOrDefault(w => w.WorkerId == payTypeId);
                        pay.Worker = worker;
                        break;
                    default:
                        break;
                }

                return pay;
            }
            catch (Exception)
            {
                return pay;

            }

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
