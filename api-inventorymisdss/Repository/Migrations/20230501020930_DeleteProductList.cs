using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_inventorymisdss.Repository.Migrations
{
    /// <inheritdoc />
    public partial class DeleteProductList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incomings_ProductList_ProductListId",
                table: "Incomings");

            migrationBuilder.DropForeignKey(
                name: "FK_Outgoings_ProductList_ProductListId",
                table: "Outgoings");

            migrationBuilder.DropTable(
                name: "ProductList");

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

            migrationBuilder.RenameIndex(
                name: "IX_Incomings_ProductListId",
                table: "Incomings",
                newName: "IX_Incomings_ProductId");

            migrationBuilder.AlterColumn<long>(
                name: "OutgoingProductId",
                table: "Outgoings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "IncomingProductId",
                table: "Incomings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "ProductId",
                table: "Incomings",
                newName: "ProductListId");

            migrationBuilder.RenameIndex(
                name: "IX_Incomings_ProductId",
                table: "Incomings",
                newName: "IX_Incomings_ProductListId");

            migrationBuilder.AlterColumn<int>(
                name: "OutgoingProductId",
                table: "Outgoings",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "IncomingProductId",
                table: "Incomings",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "ProductList",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
    }
}
