using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticEnterpriseBack.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToProductCategoriesName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_product_categories_Name",
                table: "product_categories",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_product_categories_Name",
                table: "product_categories");
        }
    }
}
