using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using IlCapo.IEqualityComparer;
using IlCapo.ModelJson;
using IlCapo.Models;
using IlCapo.Validation;

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

            Validation.Validation elValidador = new Validation.Validation(db, User.Identity.Name);
            Result elResultado = elValidador.ValidateUser();

            if (!elResultado.IsValid)
            {
                ViewBag.error = elResultado.Message;
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

            ViewBag.Billable = "billable";
            ViewBag.Display = "";
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
            Validation.Validation elValidador = new Validation.Validation(db, User.Identity.Name);
            Result elResultado = elValidador.ValidateUser();

            if (!elResultado.IsValid)
            {
                ViewBag.error = elResultado.Message;
                return elResultado.IsValid;
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

            Bill bill = new Bill()
            {
                BeginDayId = beginDay.BeginDayId,
                Client = client,
                Command = false,
                Discount = billJson.Discount,
                ToGo = billJson.ToGo,
                Express = billJson.Express,
                State = true,
                TableId = billJson.TableId
            };
            List<Item> items = ParseItems(billJson.Items, bill);
            bill.Items = items;

            return bill;
        }

        public List<Item> ParseItems(List<ItemJson> itemsJson, Bill bill)
        {
            List<Item> items = new List<Item>();

            itemsJson.ForEach(i => items.Add(new Item()
            {
                Description = i.Observation,
                ProductId = i.ProductId,
                Quantity = i.ProductQuantity,
                Price = i.Price,
                BillId = bill.BillId,
                ItemExtras = ParseExtras(i.Extras),
                ItemSides = ParseSides(i.Sides)
            }));

            return items;
        }

        public List<ItemExtra> ParseExtras(List<ExtraJson> extrasJson)
        {
            List<ItemExtra> itemExtras = new List<ItemExtra>();
            extrasJson.ForEach(e => itemExtras.Add(new ItemExtra()
            {
                ExtraId = e.Id,
                ProductQuantity = e.Quantity,
                Quantity = e.ExtraQuantity
            }));


            return itemExtras;
        }

        public List<ItemSide> ParseSides(List<SideJson> sidesJson)
        {
            List<ItemSide> itemSides = new List<ItemSide>();
            sidesJson.ForEach(s => itemSides.Add(new ItemSide()
            {
                SidesId = s.SideId,
                ProductQuantity = s.Quantity
            }));


            return itemSides;
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
                item.Price = newItem.Price;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
            CompareExtras(newItem.ItemExtras, item.ItemExtras, item);
            CompareSides(newItem.ItemSides, item.ItemSides, item);
        }

        public bool DeleteItem(int id)
        {
            if (DeleteItemExtra(id) && DeleteItemSides(id))
            {
                try
                {
                   
                    var items = from i in db.Items
                                     where i.ItemId == id
                                     select i;
                    items.ToList().ForEach(x => db.Items.Remove(x));
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }

            return false;

        }

        public bool DeleteItemExtra(int id)
        {
            try
            {
                var itemExtras = from i in db.ItemExtras
                                 where i.ItemId == id
                                 select i;
                itemExtras.ToList().ForEach(x => db.ItemExtras.Remove(x));
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteItemSides(int id)
        {
            try
            {
                var itemSides = from i in db.ItemSides
                                 where i.ItemId == id
                                 select i;
                itemSides.ToList().ForEach(x => db.ItemSides.Remove(x));
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void CompareExtras(List<ItemExtra> newItemExtraList, List<ItemExtra> ItemExtraList, Item item)
        {
            var listaDeExtrasPorAgregar = newItemExtraList.Where(x => !ItemExtraList.Contains(x, new ExtrasEqualityComparer())).ToList();
            var listaDeExtrasPorAjustar = newItemExtraList.Where(x => ItemExtraList.Contains(x, new ExtrasEqualityComparer())).ToList();
            var listaDeExtrasPorBorrar = ItemExtraList.Where(x => !newItemExtraList.Contains(x, new ExtrasEqualityComparer())).ToList();

            AddExtras(listaDeExtrasPorAgregar, item);
            DeleteExtras(listaDeExtrasPorBorrar);
            EditExtras(ItemExtraList, listaDeExtrasPorAjustar);

        }

        public void CompareSides(List<ItemSide> newItemSideList, List<ItemSide> itemSideList, Item item)
        {
            var listaDeSidesPorAgregar = newItemSideList.Where(x => !itemSideList.Contains(x, new SidesEqualityComparer())).ToList();
            var listaDeSidesPorBorrar = itemSideList.Where(x => !newItemSideList.Contains(x, new SidesEqualityComparer())).ToList();
            listaDeSidesPorAgregar.ForEach(x => AddSides(x, item));
            listaDeSidesPorBorrar.ForEach(x => DeleteSides(x));

        }

        public void AddSides(ItemSide itemSide, Item item)
        {
            itemSide.ItemId = item.ItemId;
            db.ItemSides.Add(itemSide);
            db.SaveChanges();
        }

        public void DeleteSides(ItemSide itemSide)
        {
            db.ItemSides.Remove(itemSide);
            db.SaveChanges();
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
                var itemExtra = itemExtrasList.SingleOrDefault(y => y.ExtraId == extraPorAjustar.ExtraId && y.ProductQuantity == extraPorAjustar.ProductQuantity);

                if (itemExtra.Quantity != extraPorAjustar.Quantity)
                {
                    itemExtra.Quantity = extraPorAjustar.Quantity;
                    db.Entry(itemExtra).State = EntityState.Modified;
                }
            }
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
            item.BillId = bill.BillId;
            item.UnitPrice = item.Price / item.Quantity;
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

        public ActionResult SliceAccount(int quantity, int price)
        {
            SliceAccount sliceAccount = new SliceAccount();
            sliceAccount.Price = price / quantity;
            sliceAccount.Quantity = quantity;

            return PartialView("BillParts/SliceAccount", sliceAccount);
        }

        public ActionResult SeparateAccount(int billId)
        {
            List<Item> items = new List<Item>();

            items = db.Items.Include("Product").Include("ItemExtras").Include("ItemSides").Include(x => x.Product.ProductTaxes).Where(x => x.BillId == billId).ToList();
            Bill bill = db.Bills.Find(billId);
            bill.Items = items;
            bill.Client = bill.Client.GetClient(bill.Client.Phone);
            ViewBag.Display = "none";
            ViewBag.Billable = "notBillable";
            ViewBag.Extras = db.Extras.ToList();
            return PartialView("BillParts/SeparateAccount", bill);
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
