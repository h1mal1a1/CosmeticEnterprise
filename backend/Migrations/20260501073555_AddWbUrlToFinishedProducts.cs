using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticEnterpriseBack.Migrations
{
    /// <inheritdoc />
    public partial class AddWbUrlToFinishedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "wb_url",
                table: "finished_products",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "wb_url",
                table: "finished_products");
        }
    }
}
