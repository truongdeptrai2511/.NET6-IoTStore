using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IotSupplyStore.Migrations
{
    /// <inheritdoc />
    public partial class FixFKProductDetailProductAndFixCategoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_DetailsProducts_DetailProductId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_DetailProductId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DetailProductId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "C_Home",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "C_Icon",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "C_Name",
                table: "Categories",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "CitizenIdentification",
                table: "AspNetUsers",
                newName: "CitizenIdentification");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "DetailsProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DetailsProducts_ProductId",
                table: "DetailsProducts",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailsProducts_Products_ProductId",
                table: "DetailsProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailsProducts_Products_ProductId",
                table: "DetailsProducts");

            migrationBuilder.DropIndex(
                name: "IX_DetailsProducts_ProductId",
                table: "DetailsProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "DetailsProducts");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Categories",
                newName: "C_Name");

            migrationBuilder.RenameColumn(
                name: "CitizenIdentification",
                table: "AspNetUsers",
                newName: "CitizenIdentification");

            migrationBuilder.AddColumn<int>(
                name: "DetailProductId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "C_Home",
                table: "Categories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "C_Icon",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_DetailProductId",
                table: "Products",
                column: "DetailProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_DetailsProducts_DetailProductId",
                table: "Products",
                column: "DetailProductId",
                principalTable: "DetailsProducts",
                principalColumn: "Id");
        }
    }
}
