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

            ViewBag.SelectedId = bill.Client.SelectedAddressId;
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

        public Bill GetEmptyBill()
        {
            Bill bill = new Bill();
            Client emptyClient = new Client { Name = "", Phone = 0 };
            List<Address> emptyAddresses = new List<Address>();
            emptyClient.Addresses = emptyAddresses;
            List<Item> emptyItems = new List<Item>();
            bill.Client = emptyClient;
            bill.Items = emptyItems;
            bill.BillId = 0;

            return bill;
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
            db.SaveChanges();
            Bill bill = new Bill();
            Bill newBill = ParseJsonBill(jsonData, beginDay);


            if (orderNumber > 0)
            {
                bill = db.Bills.Find(orderNumber);
                bill.Items = GetBillItems(bill);
                bill = EditBill(bill, newBill);
            }
            else
            {
                bill = CreateBill(newBill);
            }

            return true;
        }

        private Bill ParseJsonBill(string jsonData, BeginDay beginDay)
        {
            var serializer = new JavaScriptSerializer();
            var billJson = serializer.Deserialize<BillJson>(jsonData);

            Client client = new Client();
            client = client.GetClient(billJson.Phone);
            client.SelectedAddressId = billJson.Address;

            Bill bill = new Bill();
            bill.BeginDayId = beginDay.BeginDayId;
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

        public Bill EditBill(Bill bill, Bill newBill)
        {

            Client client = new Client();
            client = client.GetClient(newBill.Client.Phone);
            client.SelectedAddressId = newBill.Client.SelectedAddressId;
            db.Entry(client).State = EntityState.Modified;

            bill.ClientId = client.ClientId;
            bill.Discount = newBill.Discount;
            bill.Express = newBill.Express;
            EditItems(bill, newBill);

            db.Entry(bill).State = EntityState.Modified;

            db.SaveChanges();
            return bill;
        }

        private List<Item> EditItems(Bill bill, Bill newBill)
        {
            List<Item> items = new List<Item>();

            var productosDeListaVieja = bill.Items.Select(y => y.ProductId).ToList();
            var productosDeListaNueva = newBill.Items.Select(y => y.ProductId).ToList();

            var listaDeItemsPorAgregar = newBill.Items.Where(x => !productosDeListaVieja.Contains(x.ProductId)).ToList();
            var listaDeItemsPorAjustar = newBill.Items.Where(x => productosDeListaVieja.Contains(x.ProductId)).ToList();

            foreach (var itemPorAgregar in listaDeItemsPorAgregar)
            {
                items.Add(CreateItem(itemPorAgregar, bill));
            }

            foreach (var itemPorAjustar in listaDeItemsPorAjustar)
            {
                var itemInicial = bill.Items.SingleOrDefault(y => y.ProductId == itemPorAjustar.ProductId);
                EditItem(itemPorAjustar, itemInicial);
            }

            return items;
        }


        public void EditItem(Item newItem, Item item)
        {
            if (item.Quantity != newItem.Quantity)
            {
                item.Quantity = newItem.Quantity;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            CompareExtras(newItem.ItemExtras, item.ItemExtras, item);
            //CompareSides(newItem.ItemSides, item.ItemSides, item.Quantity);
        }

        public void CompareExtras(List<ItemExtra> newItemExtraList, List<ItemExtra> ItemExtraList, Item item)
        {
            var extrasDeListaVieja = ItemExtraList.Select(y => y.ExtraId).ToList();
            var extrasDeListaNueva = newItemExtraList.Select(y => y.ExtraId).ToList();


            var listaDeExtrasPorAgregar = newItemExtraList.Where(x => !extrasDeListaVieja.Contains(x.ExtraId)).ToList();
            var listaDeExtrasPorAjustar = newItemExtraList.Where(x => extrasDeListaVieja.Contains(x.ExtraId)).ToList();
            var listaDeExtrasPorBorrar = ItemExtraList.Where(x => !extrasDeListaNueva.Contains(x.ExtraId)).ToList();

            AddExtras(listaDeExtrasPorAgregar, item);
            DeleteExtras(listaDeExtrasPorBorrar);
            EditExtras(ItemExtraList, listaDeExtrasPorAjustar);

        }

        public void CompareSides(List<ItemSide> newItemSideList, List<ItemSide> itemSideList, int currentQuantity)
        {
            foreach (var newItem in newItemSideList)
            {
                var itemExists = false;
                foreach (var item in itemSideList)
                {
                    if (newItem.SidesId == item.SidesId && newItem.ProductQuantity <= currentQuantity && item.ProductQuantity <= currentQuantity)
                    {
                        itemExists = true;
                    }
                }

                if (!itemExists)
                {
                    db.ItemSides.Add(newItem);
                }
            }

            foreach (var item in itemSideList)
            {
                var itemExists = false;
                foreach (var newItem in newItemSideList)
                {
                    if (newItem.ItemSideId == item.ItemSideId && newItem.ProductQuantity <= currentQuantity && item.ProductQuantity <= currentQuantity)
                    {
                        itemExists = true;
                    }
                }

                if (!itemExists)
                {
                    db.ItemSides.Remove(item);
                }
            }
        }

        public void AddExtras(List<ItemExtra> itemExtras, Item item)
        {
            foreach (var itemExtra in itemExtras)
            {
                itemExtra.ItemId = item.ItemId;
                db.ItemExtras.Add(itemExtra);
            }
            db.SaveChanges();
        }

        public void DeleteExtras(List<ItemExtra> itemExtras)
        {
            foreach (var item in itemExtras)
            {
                db.ItemExtras.Remove(item);
            }
            db.SaveChanges();
        }

        public void EditExtras(List<ItemExtra> itemExtrasList, List<ItemExtra> listaDeExtrasPorAjustar)
        {
            foreach (var extraPorAjustar in listaDeExtrasPorAjustar)
            {
                var itemExtra = itemExtrasList.SingleOrDefault(y => y.ExtraId == extraPorAjustar.ExtraId);

                if (itemExtra.Quantity != extraPorAjustar.Quantity)
                {
                    itemExtra.Quantity = extraPorAjustar.Quantity;
                    db.Entry(itemExtra).State = EntityState.Modified;
                }
            }
            db.SaveChanges();
        }

        public void DeleteItem(int ItemId)
        {
            Item item = db.Items.Find(ItemId);
            db.Items.Remove(item);
            db.SaveChanges();
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
            List<ItemSide> currentItemSide = new List<ItemSide>();
            currentItemExtra = item.ItemExtras;
            currentItemSide = item.ItemSides;
            item.Bill = bill;
            item.ItemExtras = new List<ItemExtra>();
            item.ItemSides = new List<ItemSide>();
            db.Items.Add(item);
            db.SaveChanges();
            item.ItemExtras = currentItemExtra;
            item.ItemSides = currentItemSide;

            db.ItemExtras.AddRange(item.ItemExtras);
            db.ItemSides.AddRange(item.ItemSides);
            db.SaveChanges();

            return item;
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
