using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IotSupplyStore.Migrations
{
    /// <inheritdoc />
    public partial class fixOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Or_Quantity",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Or_PriceSale",
                table: "Orders",
                newName: "PriceSale");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceSale",
                table: "Orders",
                newName: "Or_PriceSale");

            migrationBuilder.AddColumn<int>(
                name: "Or_Quantity",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
