using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_inventorymisdss.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddProductList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incomings_Products_ProductId",
                table: "Incomings");

            migrationBuilder.DropForeignKey(
                name: "FK_Outgoings_Products_ProductId",
                table: "Outgoings");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Outgoings",
                newName: "ProductListId");

            migrationBuilder.RenameIndex(
                name: "IX_Outgoings_ProductId",
                table: "Outgoings",
                newName: "IX_Outgoings_ProductListId");

            migrationBuilder.RenameColumn(
                name: "StockCount",
                table: "Incomings",
                newName: "IncomingStockQuantity");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Incomings",
                newName: "ProductListId");

            migrationBuilder.RenameIndex(
                name: "IX_Incomings_ProductId",
                table: "Incomings",
                newName: "IX_Incomings_ProductListId");

            migrationBuilder.AddColumn<int>(
                name: "OutgoingProductId",
                table: "Outgoings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IncomingProductId",
                table: "Incomings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Incomings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "IncomingProductVM",
                columns: table => new
                {
                    IncomingStockQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "OutgoingProductVM",
                columns: table => new
                {
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ProductLists",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IncomingId = table.Column<int>(type: "int", nullable: true),
                    OutgoingId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductLists_Incomings_IncomingId",
                        column: x => x.IncomingId,
                        principalTable: "Incomings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductLists_Outgoings_OutgoingId",
                        column: x => x.OutgoingId,
                        principalTable: "Outgoings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductVM",
                columns: table => new
                {
                    BarcodeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VariantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Measurement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductLists_IncomingId",
                table: "ProductLists",
                column: "IncomingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLists_OutgoingId",
                table: "ProductLists",
                column: "OutgoingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incomings_ProductLists_ProductListId",
                table: "Incomings",
                column: "ProductListId",
                principalTable: "ProductLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Outgoings_ProductLists_ProductListId",
                table: "Outgoings",
                column: "ProductListId",
                principalTable: "ProductLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incomings_ProductLists_ProductListId",
                table: "Incomings");

            migrationBuilder.DropForeignKey(
                name: "FK_Outgoings_ProductLists_ProductListId",
                table: "Outgoings");

            migrationBuilder.DropTable(
                name: "IncomingProductVM");

            migrationBuilder.DropTable(
                name: "OutgoingProductVM");

            migrationBuilder.DropTable(
                name: "ProductLists");

            migrationBuilder.DropTable(
                name: "ProductVM");

            migrationBuilder.DropColumn(
                name: "OutgoingProductId",
                table: "Outgoings");

            migrationBuilder.DropColumn(
                name: "IncomingProductId",
                table: "Incomings");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Incomings");

            migrationBuilder.RenameColumn(
                name: "ProductListId",
                table: "Outgoings",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Outgoings_ProductListId",
                table: "Outgoings",
                newName: "IX_Outgoings_ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductListId",
                table: "Incomings",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "IncomingStockQuantity",
                table: "Incomings",
                newName: "StockCount");

            migrationBuilder.RenameIndex(
                name: "IX_Incomings_ProductListId",
                table: "Incomings",
                newName: "IX_Incomings_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incomings_Products_ProductId",
                table: "Incomings",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Outgoings_Products_ProductId",
                table: "Outgoings",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
