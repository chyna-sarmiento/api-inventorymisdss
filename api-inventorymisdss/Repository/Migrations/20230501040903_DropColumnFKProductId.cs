using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_inventorymisdss.Repository.Migrations
{
    /// <inheritdoc />
    public partial class DropColumnFKProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outgoings_Products_ProductId",
                table: "Outgoings");

            migrationBuilder.DropIndex(
                name: "IX_Outgoings_ProductId",
                table: "Outgoings");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Outgoings");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomings_Products_ProductId",
                table: "Incomings");

            migrationBuilder.DropIndex(
                name: "IX_Incomings_ProductId",
                table: "Incomings");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Incomings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
            name: "ProductId",
            table: "Outgoings",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
            name: "ProductId",
            table: "Incomings",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);
        }
    }
}
