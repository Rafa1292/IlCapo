using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasicServices",
                columns: table => new
                {
                    BasicServicesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Expiration = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicServices", x => x.BasicServicesId);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<int>(nullable: false),
                    SelectedAddressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Extras",
                columns: table => new
                {
                    ExtraId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    cost = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extras", x => x.ExtraId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    ProductCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.ProductCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    ProviderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Phone = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.ProviderId);
                });

            migrationBuilder.CreateTable(
                name: "Sides",
                columns: table => new
                {
                    SidesId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sides", x => x.SidesId);
                });

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    TaxId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Percentage = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.TaxId);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    WorkerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Mail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.WorkerId);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Addresses_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSubCategories",
                columns: table => new
                {
                    ProductSubCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ProductCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubCategories", x => x.ProductSubCategoryId);
                    table.ForeignKey(
                        name: "FK_ProductSubCategories_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "ProductCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BeginDays",
                columns: table => new
                {
                    BeginDayId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Cash = table.Column<decimal>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeginDays", x => x.BeginDayId);
                    table.ForeignKey(
                        name: "FK_BeginDays_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EndDays",
                columns: table => new
                {
                    EndDayId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Cash = table.Column<decimal>(nullable: false),
                    CreditCard = table.Column<decimal>(nullable: false),
                    Dollar = table.Column<decimal>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndDays", x => x.EndDayId);
                    table.ForeignKey(
                        name: "FK_EndDays_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Cost = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    KitchenMessage = table.Column<bool>(nullable: false),
                    Sides = table.Column<bool>(nullable: false),
                    SidesQuantity = table.Column<int>(nullable: false),
                    TotalSales = table.Column<int>(nullable: false),
                    ProductSubCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_ProductSubCategories_ProductSubCategoryId",
                        column: x => x.ProductSubCategoryId,
                        principalTable: "ProductSubCategories",
                        principalColumn: "ProductSubCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkDays",
                columns: table => new
                {
                    WorkDayId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<bool>(nullable: false),
                    InitialCash = table.Column<decimal>(nullable: false),
                    TotalSales = table.Column<decimal>(nullable: false),
                    TotalEntries = table.Column<decimal>(nullable: false),
                    TotalTaxes = table.Column<decimal>(nullable: false),
                    TotalPays = table.Column<decimal>(nullable: false),
                    TotalDollar = table.Column<decimal>(nullable: false),
                    TotalCard = table.Column<decimal>(nullable: false),
                    TotalCash = table.Column<decimal>(nullable: false),
                    TotalDiscount = table.Column<decimal>(nullable: false),
                    Diference = table.Column<decimal>(nullable: false),
                    BeginDayId = table.Column<int>(nullable: false),
                    EndDayId = table.Column<int>(nullable: false),
                    WorkerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDays", x => x.WorkDayId);
                    table.ForeignKey(
                        name: "FK_WorkDays_BeginDays_BeginDayId",
                        column: x => x.BeginDayId,
                        principalTable: "BeginDays",
                        principalColumn: "BeginDayId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkDays_EndDays_EndDayId",
                        column: x => x.EndDayId,
                        principalTable: "EndDays",
                        principalColumn: "EndDayId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkDays_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTaxes",
                columns: table => new
                {
                    ProductTaxId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    TaxId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTaxes", x => x.ProductTaxId);
                    table.ForeignKey(
                        name: "FK_ProductTaxes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTaxes_Taxes_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Taxes",
                        principalColumn: "TaxId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BillId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<bool>(nullable: false),
                    SubTotal = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    Taxes = table.Column<int>(nullable: false),
                    ExtrasAmount = table.Column<int>(nullable: false),
                    DiscountAmount = table.Column<int>(nullable: false),
                    TableId = table.Column<int>(nullable: false),
                    Date = table.Column<string>(nullable: true),
                    ClientId = table.Column<int>(nullable: false),
                    Discount = table.Column<int>(nullable: false),
                    Command = table.Column<bool>(nullable: false),
                    Express = table.Column<bool>(nullable: false),
                    ToGo = table.Column<bool>(nullable: false),
                    PayMethod = table.Column<int>(nullable: false),
                    PayWith = table.Column<int>(nullable: false),
                    BeginDayId = table.Column<int>(nullable: false),
                    WorkDayId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BillId);
                    table.ForeignKey(
                        name: "FK_Bills_BeginDays_BeginDayId",
                        column: x => x.BeginDayId,
                        principalTable: "BeginDays",
                        principalColumn: "BeginDayId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bills_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bills_WorkDays_WorkDayId",
                        column: x => x.WorkDayId,
                        principalTable: "WorkDays",
                        principalColumn: "WorkDayId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    EntryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    BeginDayId = table.Column<int>(nullable: false),
                    WorkDayId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_Entries_BeginDays_BeginDayId",
                        column: x => x.BeginDayId,
                        principalTable: "BeginDays",
                        principalColumn: "BeginDayId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entries_WorkDays_WorkDayId",
                        column: x => x.WorkDayId,
                        principalTable: "WorkDays",
                        principalColumn: "WorkDayId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pays",
                columns: table => new
                {
                    PayId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true),
                    PayType = table.Column<int>(nullable: false),
                    ProviderId = table.Column<int>(nullable: true),
                    WorkerId = table.Column<int>(nullable: true),
                    BasicServicesId = table.Column<int>(nullable: true),
                    BeginDayId = table.Column<int>(nullable: false),
                    WorkDayId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pays", x => x.PayId);
                    table.ForeignKey(
                        name: "FK_Pays_BasicServices_BasicServicesId",
                        column: x => x.BasicServicesId,
                        principalTable: "BasicServices",
                        principalColumn: "BasicServicesId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pays_BeginDays_BeginDayId",
                        column: x => x.BeginDayId,
                        principalTable: "BeginDays",
                        principalColumn: "BeginDayId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pays_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pays_WorkDays_WorkDayId",
                        column: x => x.WorkDayId,
                        principalTable: "WorkDays",
                        principalColumn: "WorkDayId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pays_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<int>(nullable: false),
                    Commanded = table.Column<bool>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    BillId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Items_Bills_BillId",
                        column: x => x.BillId,
                        principalTable: "Bills",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemExtras",
                columns: table => new
                {
                    ItemExtraId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(nullable: false),
                    ExtraId = table.Column<int>(nullable: false),
                    ProductQuantity = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemExtras", x => x.ItemExtraId);
                    table.ForeignKey(
                        name: "FK_ItemExtras_Extras_ExtraId",
                        column: x => x.ExtraId,
                        principalTable: "Extras",
                        principalColumn: "ExtraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemExtras_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemSides",
                columns: table => new
                {
                    ItemSideId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(nullable: false),
                    SidesId = table.Column<int>(nullable: false),
                    ProductQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSides", x => x.ItemSideId);
                    table.ForeignKey(
                        name: "FK_ItemSides_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemSides_Sides_SidesId",
                        column: x => x.SidesId,
                        principalTable: "Sides",
                        principalColumn: "SidesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ClientId",
                table: "Addresses",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_BeginDays_WorkerId",
                table: "BeginDays",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_BeginDayId",
                table: "Bills",
                column: "BeginDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_ClientId",
                table: "Bills",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_WorkDayId",
                table: "Bills",
                column: "WorkDayId");

            migrationBuilder.CreateIndex(
                name: "IX_EndDays_WorkerId",
                table: "EndDays",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BeginDayId",
                table: "Entries",
                column: "BeginDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_WorkDayId",
                table: "Entries",
                column: "WorkDayId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemExtras_ExtraId",
                table: "ItemExtras",
                column: "ExtraId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemExtras_ItemId",
                table: "ItemExtras",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_BillId",
                table: "Items",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ProductId",
                table: "Items",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSides_ItemId",
                table: "ItemSides",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSides_SidesId",
                table: "ItemSides",
                column: "SidesId");

            migrationBuilder.CreateIndex(
                name: "IX_Pays_BasicServicesId",
                table: "Pays",
                column: "BasicServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_Pays_BeginDayId",
                table: "Pays",
                column: "BeginDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Pays_ProviderId",
                table: "Pays",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Pays_WorkDayId",
                table: "Pays",
                column: "WorkDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Pays_WorkerId",
                table: "Pays",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductSubCategoryId",
                table: "Products",
                column: "ProductSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubCategories_ProductCategoryId",
                table: "ProductSubCategories",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTaxes_ProductId",
                table: "ProductTaxes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTaxes_TaxId",
                table: "ProductTaxes",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDays_BeginDayId",
                table: "WorkDays",
                column: "BeginDayId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDays_EndDayId",
                table: "WorkDays",
                column: "EndDayId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDays_WorkerId",
                table: "WorkDays",
                column: "WorkerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "ItemExtras");

            migrationBuilder.DropTable(
                name: "ItemSides");

            migrationBuilder.DropTable(
                name: "Pays");

            migrationBuilder.DropTable(
                name: "ProductTaxes");

            migrationBuilder.DropTable(
                name: "Extras");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Sides");

            migrationBuilder.DropTable(
                name: "BasicServices");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "WorkDays");

            migrationBuilder.DropTable(
                name: "ProductSubCategories");

            migrationBuilder.DropTable(
                name: "BeginDays");

            migrationBuilder.DropTable(
                name: "EndDays");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "Workers");
        }
    }
}
