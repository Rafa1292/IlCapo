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
            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
            var endDay = GetValidateEndDaysList();
            return PartialView("Index", endDay);

        }


        public ActionResult Create()
        {
            if (!ValidateUser())
            {
                return RedirectToAction("Index");
            }

            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
            BeginDay beginDay = new BeginDay();
            beginDay = beginDay.GetBeginDay(worker);
            Pay pay = new Pay();
            List<Pay> pays = pay.GetPays(beginDay.BeginDayId, db.Pays.Include( p => p.Provider).ToList());
            ViewBag.pays = pays;
            Entry entry = new Entry();
            List<Entry> entries = entry.GetEntries(beginDay.BeginDayId, db.Entries.ToList());
            ViewBag.entries = entries;            
            EndDay endDay = new EndDay();
            endDay.Worker = worker;
            int creditCardAmount = db.Bills.ToList().Where(x => x.PayMethod == PayMethod.Card && x.BeginDayId == beginDay.BeginDayId).ToList().Sum(x => x.Total - x.DiscountAmount);
            endDay.CreditCard = creditCardAmount;

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
                endDay.Date = DateTime.Now;
                db.EndDays.Add(endDay);
                db.SaveChanges();
                EndWorkDay(endDay.EndDayId);
                var endDayList = GetValidateEndDaysList();
                return PartialView("Index", endDayList);
            }


            ViewBag.WorkerId = new SelectList(db.Workers, "WorkerId", "Name", endDay.WorkerId);
            return PartialView("Create", endDay);
        }

        public void EndWorkDay(int endDayId)
        {
            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
            BeginDay beginDay = new BeginDay();
            beginDay = beginDay.GetBeginDay(worker);
            EndDay endDay = db.EndDays.Find(endDayId);
            List<Bill> bills = db.Bills.ToList().Where(x => x.BeginDayId == beginDay.BeginDayId).ToList();
            List<Pay> pays = db.Pays.ToList().Where(x => x.BeginDayId == beginDay.BeginDayId).ToList();
            List<Entry> entries = db.Entries.ToList().Where(x => x.BeginDayId == beginDay.BeginDayId).ToList();
            int billsAmount = bills.Sum(x => x.SubTotal);
            int taxesAmount = bills.Sum(x => x.Taxes);
            int discountAmount = bills.Sum(x => x.DiscountAmount);
            decimal entriesAmount = (entries.Sum(x => x.Amount));
            decimal paysAmount = pays.Sum(x => x.Amount);
            decimal diference = endDay.Cash - (beginDay.Cash + decimal.Parse(billsAmount.ToString()) + entriesAmount + decimal.Parse(taxesAmount.ToString()) - decimal.Parse(discountAmount.ToString()) - paysAmount - endDay.CreditCard);
            WorkDay workDay = new WorkDay()
            {
                BeginDayId = beginDay.BeginDayId,
                EndDayId = endDay.EndDayId,
                WorkerId = worker.WorkerId,
                InitialCash = beginDay.Cash,
                TotalSales = decimal.Parse(billsAmount.ToString()),
                TotalEntries = entriesAmount,
                TotalTaxes = decimal.Parse(taxesAmount.ToString()),
                TotalPays = paysAmount,
                TotalDiscount = decimal.Parse(discountAmount.ToString()),
                TotalCard = endDay.CreditCard,
                TotalCash = decimal.Parse(billsAmount.ToString()) - endDay.CreditCard,
                TotalDollar = endDay.Dollar,
                State = true,
                Diference = diference
            };
            db.WorkDays.Add(workDay);
            db.SaveChanges();
            SendCloseTicket(workDay);

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

        public bool SendCloseTicket( WorkDay workDay)
        {
            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
            var endDay = db.EndDays.Find(workDay.EndDayId);
            ticket ticket = new ticket();
            ticket.TextoCentro("Pizza Il Capo");
            ticket.TextoCentro("Alajuela, Grecia, Centro");
            ticket.TextoCentro("Mall plaza Grecia");
            ticket.TextoCentro("Telefono: 2444-3001");
            ticket.TextoCentro("Fecha:" + DateTime.Now.ToShortDateString());
            ticket.TextoCentro("Hora:" + DateTime.Now.ToLongTimeString());
            ticket.TextoCentro(" Cajero: : " + worker.Name);

            ticket.LineasAsterisco();
            ticket.TextoIzquierda(" ");
            ticket.TextoCentro("Cierre de jornada");
            ticket.TextoIzquierda(" ");
            ticket.LineasAsterisco();

            var total = workDay.InitialCash + workDay.TotalSales + workDay.TotalTaxes 
                + workDay.TotalEntries - workDay.TotalPays - workDay.TotalDiscount - workDay.TotalCard;
            ticket.TextoIzquierda($" Caja inicial: {workDay.InitialCash}");
            ticket.TextoIzquierda($" Ventas: {workDay.TotalSales}");
            ticket.TextoIzquierda($" Impuestos: {workDay.TotalTaxes}");
            ticket.TextoIzquierda($" Ingresos: {workDay.TotalEntries}");
            ticket.TextoIzquierda($" Gastos: {workDay.TotalPays}");
            ticket.TextoIzquierda($" Descuentos: {workDay.TotalDiscount}");
            ticket.TextoIzquierda($" Venta tarjeta: {workDay.TotalCard}");
            ticket.TextoIzquierda($" Venta efectivo: {workDay.TotalCash}");
            ticket.TextoIzquierda($" Total: {total}");
            ticket.TextoIzquierda($" Efectivo en caja: {endDay.Cash}");
            ticket.TextoIzquierda($" Diferencia: {workDay.Diference}");




            for (int i = 0; i < 7; i++)
            {
                ticket.TextoIzquierda(" ");

            }

            ticket.CortarTicket();
            ticket.ImprimirTicket("LR2000");

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
