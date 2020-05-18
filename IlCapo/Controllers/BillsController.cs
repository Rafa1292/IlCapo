using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using IlCapo.ModelJson;
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
            beginDay = beginDay.GetBeginDay(worker);
            var billContent = bill.tableContent(tableId, beginDay.BeginDayId);

            if (billContent != null)
            {
                bill = billContent;
                bill.Client = bill.Client.GetClient(bill.Client.Phone);
                bill.Items = GetBillItems(bill);
            }
            else
            {
                bill = GetEmptyBill();
            }

            var productTaxes = db.ProductTaxes.ToList();
            ViewBag.Extras = db.Extras.ToList();
            ViewBag.Sides = db.Sides.ToList();
            ViewBag.ProductTaxes = productTaxes;
            ViewBag.TableId = tableId;
            ViewBag.ToGo = toGo.ToString();
            Product product = new Product();
            ViewBag.Favorites = product.Get().OrderBy(p => p.TotalSales).ToList();
            ViewBag.Category = db.ProductCategories.ToList();
            return PartialView("TableMenu/TableMenu", bill);
        }

        public List<Item> GetBillItems(Bill bill)
        {
            List<Item> items = new List<Item>();
            var itemsEF = from i in db.Items.Include("ItemExtras").Include(x => x.Product.ProductTaxes).Include("ItemSides")
                    where i.BillId == bill.BillId
                    select i;

            items = itemsEF.ToList();

            return items;
        }


        public bool CommandBill(string jsonData, int orderNumber)
        {
            if (!ValidateUser())
            {
                return false;
            }

            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
            BeginDay beginDay = new BeginDay();
            beginDay = beginDay.GetBeginDay(worker);

            Bill bill = new Bill();
            Bill newBill = ParseJsonBill(jsonData, beginDay);

            if (orderNumber > 0)
            {
                bill = db.Bills.Find(orderNumber);
                bill = EditBill(bill, newBill);
            }
            else
            {
                bill = CreateBill(newBill);
            }

            return true;
        }


        public Bill EditBill(Bill bill, Bill newBill)
        {
            bill.Address = newBill.Address;
            bill.Client = newBill.Client;
            bill.Discount = newBill.Discount;
            bill.Express = newBill.Express;
            bill.Items = EditItems(bill, newBill);


            return bill;
        }

        private List<Item> EditItems(Bill bill, Bill newBill)
        {
            List<Item> items = new List<Item>();

            foreach (var newItem in newBill.Items)
            {
                var itemExists = false;

                foreach (var item in bill.Items)
                {
                    if (newItem.ProductId == item.ProductId)
                    {
                        itemExists = true;
                    }
                }

                if (!itemExists)
                {
                    items.Add(CreateItem(newItem, bill));
                }
            }

            return items;
        }

        public Bill CreateBill(Bill bill)
        {
            List<Item> currentItems = bill.Items;
            List<Item> Items = new List<Item>();
            bill.Items = Items;
            db.Bills.Add(bill);
            db.SaveChanges();


            foreach (var item in currentItems)
            {
                Items.Add(CreateItem(item, bill));
            }

            bill.Items = Items;

            return bill;
        }

        public Item CreateItem(Item item, Bill bill)
        {
            List<ItemExtra> currentItemExtra = new List<ItemExtra>();
            currentItemExtra = item.ItemExtras;
            List<ItemSide> currentItemSide = new List<ItemSide>();
            currentItemSide = item.ItemSides;
            item.Bill = bill;
            item.ItemExtras = new List<ItemExtra>();
            item.ItemSides = new List<ItemSide>();
            db.Items.Add(item);
            db.SaveChanges();
            item.ItemExtras = currentItemExtra;
            //item.ItemExtras = CreateExtras(item);
            item.ItemSides = currentItemSide;

            db.ItemExtras.AddRange(item.ItemExtras);
            db.ItemSides.AddRange(item.ItemSides);
            db.SaveChanges();

            return item;
        }



        private List<ItemExtra> CreateExtras(Item item)
        {
            ItemExtra itemExtra = new ItemExtra();
            List<ItemExtra> CurrentItemExtras = item.ItemExtras;
            List<ItemExtra> itemExtras = new List<ItemExtra>();

            foreach (var extra in CurrentItemExtras)
            {
                itemExtra.ExtraId = extra.ExtraId;
                itemExtra.Item = item;
                itemExtra.Quantity = extra.Quantity;
                itemExtra.ProductQuantity = extra.ProductQuantity;

                itemExtras.Add(itemExtra);
            }

            db.ItemExtras.AddRange(itemExtras);
            db.SaveChanges();

            return itemExtras;
        }

        private Bill ParseJsonBill(string jsonData, BeginDay beginDay)
        {
            var serializer = new JavaScriptSerializer();
            var billJson = serializer.Deserialize<BillJson>(jsonData);
            Address address = new Address();
            address = address.GetAddress(billJson.Address);

            Client client = new Client();
            client = client.GetClient(billJson.Phone);

            Bill bill = new Bill();
            bill.BeginDayId = beginDay.BeginDayId;
            bill.Address = address;
            bill.Client = client;
            bill.Command = false;
            bill.Discount = billJson.Discount;
            bill.ToGo = billJson.ToGo;
            bill.Express = billJson.Express;
            bill.State = true;
            bill.TableId = billJson.TableId;
            List<Item> items = ParseItems(billJson.Items, bill);
            bill.Items = items;

            return bill;
        }

        public List<Item> ParseItems(List<ItemJson> itemsJson, Bill bill)
        {
            List<Item> items = new List<Item>();

            foreach (var Json in itemsJson)
            {
                List<ItemExtra> itemExtras = new List<ItemExtra>();
                List<ItemSide> itemSides = new List<ItemSide>();
                Item item = new Item();

                item.Description = Json.Observation;
                item.ProductId = Json.ProductId;
                item.Quantity = Json.ProductQuantity;
                item.Bill = bill;

                foreach (var extra in Json.Extras)
                {
                    ItemExtra itemExtra = new ItemExtra();
                    itemExtra.ExtraId = extra.Id;
                    itemExtra.ProductQuantity = extra.Quantity;
                    itemExtra.Quantity = extra.ExtraQuantity;
                    itemExtras.Add(itemExtra);
                }

                foreach (var sideJson in Json.Sides)
                {
                    ItemSide itemSide = new ItemSide();
                    itemSide.SidesId = sideJson.SideId;
                    itemSide.ProductQuantity = sideJson.Quantity;
                    itemSides.Add(itemSide);
                }

                item.ItemExtras = itemExtras;
                item.ItemSides = itemSides;
                items.Add(item);
            }

            return items;
        }

        public void CompareItems(Item newItem, Item item)
        {

            if (newItem.Quantity == item.Quantity)
            {
                CompareExtras(newItem, item);
            }

        }

        public bool CompareExtras(Item newItem, Item item)
        {
            List<Extra> extras = item.GetExtras(item);
            
            
  

            return false;
        }

        public bool DeleteExtras(List<Extra> newExtras, List<Extra> extras)
        {

            foreach (var extra in extras)
            {
                foreach (var newExtra in newExtras)
                {
                    
                }
            }


            return false;
        }

        public bool AddExtras(List<Extra> newExtras, List<Extra> Extras)
        {




            return false;
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
            List<Address> emptyAddresses = new List<Address >();
            emptyClient.Addresses = emptyAddresses;
            List<Item> emptyItems = new List<Item>();
            bill.Client = emptyClient;
            bill.Items = emptyItems;
            bill.BillId = 0;

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
