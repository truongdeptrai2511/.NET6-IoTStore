using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IotSupplyStore.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusPaymentField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "StatusPayment",
                table: "OrderStatus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusPayment",
                table: "OrderStatus");
        }
    }
}
