using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticEnterpriseBack.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToRecipesAndUnitsOfMeasurement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "units_of_measurement",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "recipes",
                newName: "id");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "units_of_measurement",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "recipes",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_units_of_measurement_name",
                table: "units_of_measurement",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recipes_name",
                table: "recipes",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_units_of_measurement_name",
                table: "units_of_measurement");

            migrationBuilder.DropIndex(
                name: "IX_recipes_name",
                table: "recipes");

            migrationBuilder.DropColumn(
                name: "name",
                table: "units_of_measurement");

            migrationBuilder.DropColumn(
                name: "name",
                table: "recipes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "units_of_measurement",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "recipes",
                newName: "Id");
        }
    }
}
