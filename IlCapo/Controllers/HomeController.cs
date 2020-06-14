using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IlCapo.Controllers
{
    public class HomeController : Controller
    {
        IlCapoContext db = new IlCapoContext();
        public ActionResult Index()
        {
            var bills = db.Bills.ToList().Where(x => x.State && x.ToGo).ToList();

            return View(bills);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}