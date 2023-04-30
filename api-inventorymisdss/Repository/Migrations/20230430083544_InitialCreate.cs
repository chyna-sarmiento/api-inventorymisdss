using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_inventorymisdss.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BarcodeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VariantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Measurement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockCount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incomings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTimeRestock = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IncomingStockQuantity = table.Column<int>(type: "int", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductListId = table.Column<long>(type: "bigint", nullable: false),
                    IncomingProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outgoings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateTimeOutgoing = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductListId = table.Column<long>(type: "bigint", nullable: false),
                    OutgoingProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outgoings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductList",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    IncomingId = table.Column<long>(type: "bigint", nullable: true),
                    OutgoingId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductList_Incomings_IncomingId",
                        column: x => x.IncomingId,
                        principalTable: "Incomings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductList_Outgoings_OutgoingId",
                        column: x => x.OutgoingId,
                        principalTable: "Outgoings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductList_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incomings_ProductListId",
                table: "Incomings",
                column: "ProductListId");

            migrationBuilder.CreateIndex(
                name: "IX_Outgoings_ProductListId",
                table: "Outgoings",
                column: "ProductListId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductList_IncomingId",
                table: "ProductList",
                column: "IncomingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductList_OutgoingId",
                table: "ProductList",
                column: "OutgoingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductList_ProductId",
                table: "ProductList",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Incomings_ProductList_ProductListId",
                table: "Incomings",
                column: "ProductListId",
                principalTable: "ProductList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Outgoings_ProductList_ProductListId",
                table: "Outgoings",
                column: "ProductListId",
                principalTable: "ProductList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incomings_ProductList_ProductListId",
                table: "Incomings");

            migrationBuilder.DropForeignKey(
                name: "FK_Outgoings_ProductList_ProductListId",
                table: "Outgoings");

            migrationBuilder.DropTable(
                name: "ProductList");

            migrationBuilder.DropTable(
                name: "Incomings");

            migrationBuilder.DropTable(
                name: "Outgoings");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
