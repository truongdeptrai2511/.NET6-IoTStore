using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IotSupplyStore.Migrations
{
    /// <inheritdoc />
    public partial class DropTableDetailProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailsProducts");

            migrationBuilder.RenameColumn(
                name: "P_Status",
                table: "Products",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "P_Quantity",
                table: "Products",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "P_Code",
                table: "Products",
                newName: "Code");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Products",
                newName: "P_Status");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Products",
                newName: "P_Quantity");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Products",
                newName: "P_Code");

            migrationBuilder.CreateTable(
                name: "DetailsProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    P_Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    P_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    P_KeywordSeo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    P_Price = table.Column<float>(type: "real", nullable: false),
                    P_TitleSeo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    P_Warranty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailsProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailsProducts_ProductId",
                table: "DetailsProducts",
                column: "ProductId",
                unique: true);
        }
    }
}
