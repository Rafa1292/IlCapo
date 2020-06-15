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

        public ActionResult Index()
        {
            BeginDay beginDay = new BeginDay();
            Worker worker = db.Workers.FirstOrDefault(w => w.Mail == User.Identity.Name);
            beginDay = beginDay.GetBeginDay(worker);
            var bills = db.Bills.ToList().Where(x => x.BeginDayId == beginDay.BeginDayId).ToList();
            ViewBag.totalBills = bills.Sum(x => x.Total);
            return View(bills);
        }

        public ActionResult Create(int tableId, bool toGo, int billId)
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
            var billContent = bill.tableContent(tableId, beginDay.BeginDayId, billId);

            if (tableId == 0 && billId == 0)
            {
                billContent = null;
            }

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
            ViewBag.Favorites = product.Get().OrderByDescending(p => p.TotalSales).ToList();
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
            bill.Total = 0;
            bill.SubTotal = 0;
            bill.DiscountAmount = 0;
            bill.ExtrasAmount = 0;
            bill.Taxes = 0;

            return bill;
        }

        public ActionResult CommandBill(string jsonData, int orderNumber)
        {
            Validation.Validation elValidador = new Validation.Validation(db, User.Identity.Name);
            Result elResultado = elValidador.ValidateUser();

            if (!elResultado.IsValid)
            {
                ViewBag.error = elResultado.Message;
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
                bill.Command = true;
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                bill = EditBill(bill, newBill);

            }
            else
            {
                bill = CreateBill(newBill);
            }


            bill.Command = true;
            db.Entry(bill).State = EntityState.Modified;
            db.SaveChanges();

            return Create(bill.TableId, bill.ToGo, bill.BillId);
        }

        private Bill ParseJsonBill(string jsonData, BeginDay beginDay)
        {
            var serializer = new JavaScriptSerializer();
            var billJson = serializer.Deserialize<BillJson>(jsonData);

            Client client = new Client();
            client = client.GetClient(billJson.Phone);
            client.SelectedAddressId = billJson.Address;
            db.Entry(client).State = EntityState.Modified;
            db.SaveChanges();

            Bill bill = new Bill()
            {
                BeginDayId = beginDay.BeginDayId,
                ClientId = client.ClientId,
                Command = false,
                Discount = billJson.Discount,
                ToGo = billJson.ToGo,
                Express = billJson.Express,
                State = true,
                TableId = billJson.TableId,
                SubTotal = billJson.SubTotal,
                ExtrasAmount = billJson.ExtrasAmount,
                DiscountAmount = billJson.DiscountAmount,
                Taxes = billJson.Taxes,
                Total = billJson.Total
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

            bill.ClientId = newBill.ClientId;
            bill.Discount = newBill.Discount;
            bill.Express = newBill.Express;
            bill.SubTotal = newBill.SubTotal;
            bill.ExtrasAmount = newBill.ExtrasAmount;
            bill.DiscountAmount = newBill.DiscountAmount;
            bill.Taxes = newBill.Taxes;
            bill.Total = newBill.Total;
            EditItems(bill, newBill);
            db.Entry(bill).State = EntityState.Modified;
            db.SaveChanges();
            SendCommand(bill);
            List<Item> items = GetBillItems(bill);
            items.ForEach(x => x.Commanded = true);
            items.ForEach(y => db.Entry(y).State = EntityState.Modified);
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
            var editCommand = false;
            if (item.Quantity != newItem.Quantity)
            {
                item.Quantity = newItem.Quantity;
                item.Price = newItem.Price;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                editCommand = true;

            }
            CompareExtras(newItem.ItemExtras, item.ItemExtras, item);
            CompareSides(newItem.ItemSides, item.ItemSides, item);

            if (editCommand)
            {
                EditCommand(item);
            }
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
            EditExtras(ItemExtraList, listaDeExtrasPorAjustar, item);

            if (listaDeExtrasPorAgregar.Count > 0 || listaDeExtrasPorBorrar.Count > 0)
            {
                EditCommand(item);
            }
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

        public void EditExtras(List<ItemExtra> itemExtrasList, List<ItemExtra> listaDeExtrasPorAjustar, Item item)
        {
            foreach (var extraPorAjustar in listaDeExtrasPorAjustar)
            {
                var itemExtra = itemExtrasList.SingleOrDefault(y => y.ExtraId == extraPorAjustar.ExtraId && y.ProductQuantity == extraPorAjustar.ProductQuantity);

                if (itemExtra.Quantity != extraPorAjustar.Quantity)
                {
                    itemExtra.Quantity = extraPorAjustar.Quantity;
                    db.Entry(itemExtra).State = EntityState.Modified;
                    EditCommand(item);


                }
            }
            db.SaveChanges();
        }

        public bool Billing(int billId, string payMethod, int payWith, int discount)
        {
            try
            {
                PayMethod payMethodEnum = GetPayMethod(payMethod);
                Bill bill = db.Bills.Include(x => x.Client.Addresses).Include("Items").FirstOrDefault(x => x.BillId == billId);
                bill.PayMethod = payMethodEnum;
                bill.Discount = discount;
                bill.DiscountAmount = discount * bill.Total / 100;
                bill.PayWith = payWith;
                bill.State = false;
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                SendTicket(bill);
                SendTicket(bill);

                foreach (var item in bill.Items)
                {
                    var product = db.Products.Find(item.ProductId);
                    product.TotalSales += item.Quantity;
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public PayMethod GetPayMethod(string name)
        {
            PayMethod payMethod = new PayMethod();

            switch (name)
            {
                case "card":
                    payMethod = PayMethod.Card;
                    break;
                case "cash":
                    payMethod = PayMethod.Cash;
                    break;
                case "dollar":
                    payMethod = PayMethod.Dollar;
                    break;
            }

            return payMethod;
        }

        public Bill CreateBill(Bill bill)
        {
            List<Item> currentItems = bill.Items;
            List<Item> Items = new List<Item>();
            bill.Items = Items;
            bill.Date = DateTime.Now.ToShortTimeString();
            db.Bills.Add(bill);
            db.SaveChanges();


            foreach (var item in currentItems)
            {
                var newItem = CreateItem(item, bill);
                newItem.Product = db.Products.Find(item.ProductId);
            }

            bill.Items = Items;
            SendCommand(bill);
            Items.ForEach(x => x.Commanded = true);
            Items.ForEach(y => db.Entry(y).State = EntityState.Modified);
            db.SaveChanges();
            return bill;
        }

        public Item CreateItem(Item item, Bill bill)
        {
            List<ItemExtra> currentItemExtra = new List<ItemExtra>();
            List<ItemSide> currentItemSide = new List<ItemSide>();
            currentItemExtra = item.ItemExtras;
            currentItemSide = item.ItemSides;
            item.BillId = bill.BillId;
            item.Commanded = false;
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

            foreach (var itemExtra in item.ItemExtras)
            {
                itemExtra.Extra = db.Extras.Find(itemExtra.ExtraId);
            }

            foreach (var itemSide in item.ItemSides)
            {
                itemSide.Sides = db.Sides.Find(itemSide.SidesId);
            }

            item.Product = db.Products.Find(item.ProductId);

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

        public bool SendTicket(Bill bill)
        {
            var address = bill.Client.Addresses.Find(x => x.AddressId == bill.Client.SelectedAddressId);
            ticket ticket = new ticket();
            var total = bill.Total - bill.DiscountAmount + bill.Taxes;
            ticket.TextoCentro("Pizza Il Capo");
            ticket.TextoCentro("Alajuela, Grecia, Centro");
            ticket.TextoCentro("Mall plaza Grecia");
            ticket.TextoCentro("Telefono: 2444-3001");
            ticket.TextoCentro("Ticket #:" + bill.BillId.ToString());
            ticket.TextoCentro("Fecha:" + DateTime.Now.ToShortDateString());
            ticket.TextoCentro("Hora:" + DateTime.Now.ToLongTimeString());

            ticket.LineasAsterisco();

            ticket.TextoIzquierda(" ");
            ticket.TextoIzquierda(" Atendio: : " + User.Identity.Name);
            ticket.TextoIzquierda(" Cliente: " + bill.Client.Name);
            ticket.TextoIzquierda(" Contacto: " + bill.Client.Phone);
            ticket.TextoIzquierda("Direccion: " + address.Description);

            ticket.TextoIzquierda(" ");
            ticket.LineasAsterisco();

            ticket.Encabezado();
            ticket.LineasIgual();

            foreach (var item in bill.Items)
            {
                ticket.AgregarArticulo($" {item.Product.Name} {item.Product.ProductSubCategory.Name}", item.Quantity, item.UnitPrice, item.Price);
            }
            ticket.LineasIgual();
            ticket.AgregarTotales("               SUBTOTAL......", decimal.Parse(bill.SubTotal.ToString()));
            ticket.AgregarTotales("               EXTRAS........", decimal.Parse(bill.ExtrasAmount.ToString()));
            ticket.AgregarTotales("               DESCUENTO.....", decimal.Parse(bill.DiscountAmount.ToString()));
            ticket.AgregarTotales("               IMPUESTOS.....", decimal.Parse(bill.Taxes.ToString()));
            ticket.AgregarTotales("               TOTAL.........", decimal.Parse(total.ToString()));
            ticket.AgregarTotales("               PAGA CON......", decimal.Parse(bill.PayWith.ToString()));
            ticket.AgregarTotales("               VUELTO........", decimal.Parse((bill.PayWith - total).ToString()));



            for (int i = 0; i < 7; i++)
            {
                ticket.TextoIzquierda(" ");

            }

            ticket.TextoCentro("Gracias por su compra");


            ticket.CortarTicket();
            ticket.ImprimirTicket("LR2000");

            return true;
        }

        public bool EditCommand(Item item)
        {
            Bill bill = db.Bills.Find(item.BillId);
            ticket ticket = new ticket();


            ticket.TextoCentro("Ticket #:" + bill.BillId.ToString());
            ticket.TextoCentro("Fecha:" + DateTime.Now.ToShortDateString());
            ticket.TextoCentro("Hora:" + DateTime.Now.ToLongTimeString());

            ticket.LineasAsterisco();

            ticket.TextoIzquierda(" ");
            ticket.TextoIzquierda(" Atendio: : " + User.Identity.Name);
            ticket.TextoIzquierda(" Cliente: " + bill.Client.Name);
            ticket.TextoIzquierda(" ");

            ticket.LineasAsterisco();
            ticket.TextoCentro($" ESTA ES UNA EDICION DEL TICKET #{bill.BillId} SOBRE EL ITEM #{item.ItemId}, HAGA CASO OMISO A DICHO ITEM.");

            ticket.LineasIgual();


            ticket.AgregarArticuloComanda(item.Product, item.Quantity, item.Description, item.ItemExtras, item.ItemSides, item.ItemId);

            ticket.LineasIgual();

            for (int i = 0; i < 7; i++)
            {
                ticket.TextoIzquierda(" ");

            }

            ticket.CortarTicket();
            ticket.ImprimirTicket("LR2000");

            return true;
        }


        public bool SendCommand(Bill bill)
        {
            var toGo = " Servido";

            if (bill.ToGo)
            {
                toGo = " LLevar";
            }
            ticket ticket = new ticket();


            ticket.TextoCentro("Ticket #:" + bill.BillId.ToString());
            ticket.TextoCentro("Fecha:" + DateTime.Now.ToShortDateString());
            ticket.TextoCentro("Hora:" + DateTime.Now.ToLongTimeString());

            ticket.LineasAsterisco();

            ticket.TextoIzquierda(" ");
            ticket.TextoIzquierda(" Atendio: : " + User.Identity.Name);
            ticket.TextoIzquierda(" Cliente: " + bill.Client.Name);
            ticket.TextoIzquierda(toGo);

            ticket.TextoIzquierda(" ");

            ticket.LineasAsterisco();


            var haveItems = false;

            foreach (var item in bill.Items)
            {
                if (!item.Commanded && item.Product.KitchenMessage)
                {
                    haveItems = true;
                    ticket.AgregarArticuloComanda(item.Product, item.Quantity, item.Description, item.ItemExtras, item.ItemSides, item.ItemId);
                    ticket.TextoIzquierda("");
                    ticket.LineasGuion();
                }
            }

            for (int i = 0; i < 7; i++)
            {
                ticket.TextoIzquierda(" ");

            }

            if (haveItems)
            {
                ticket.CortarTicket();
                ticket.ImprimirTicket("LR2000");
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
