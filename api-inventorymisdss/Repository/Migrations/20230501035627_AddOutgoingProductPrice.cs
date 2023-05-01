using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_inventorymisdss.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddOutgoingProductPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ProductPrice",
                table: "Outgoings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductPrice",
                table: "Outgoings");
        }
    }
}
