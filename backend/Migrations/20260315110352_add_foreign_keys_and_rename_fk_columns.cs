using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmeticEnterpriseBack.Migrations
{
    /// <inheritdoc />
    public partial class add_foreign_keys_and_rename_fk_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_finished_products_product_categories_ProductCategoriesId",
                table: "finished_products");

            migrationBuilder.DropForeignKey(
                name: "FK_finished_products_recipes_RecipeId",
                table: "finished_products");

            migrationBuilder.DropForeignKey(
                name: "FK_finished_products_units_of_measurement_UnitsOfMeasurementId",
                table: "finished_products");

            migrationBuilder.DropForeignKey(
                name: "FK_leftovers_in_warehouses_finished_products_FinishedProductsId",
                table: "leftovers_in_warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_leftovers_in_warehouses_warehouses_WarehousesId",
                table: "leftovers_in_warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_materials_units_of_measurement_UnitsOfMeasurementId",
                table: "materials");

            migrationBuilder.DropForeignKey(
                name: "FK_order_items_finished_products_FinishedProductsId",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK_order_items_orders_OrderId",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_customers_IdCustomers",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_order_statuses_IdOrderStatuses",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_sales_channels_IdSalesChannels",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_product_parties_finished_products_FinishedProductsId",
                table: "product_parties");

            migrationBuilder.DropForeignKey(
                name: "FK_supplies_from_suppliers_suppliers_SupplierId",
                table: "supplies_from_suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_supply_positions_materials_MaterialId",
                table: "supply_positions");

            migrationBuilder.DropForeignKey(
                name: "FK_supply_positions_supplies_from_suppliers_SuppliesFromSuppli~",
                table: "supply_positions");

            migrationBuilder.RenameColumn(
                name: "SuppliesFromSuppliersId",
                table: "supply_positions",
                newName: "id_supplies_from_supplier");

            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "supply_positions",
                newName: "id_material");

            migrationBuilder.RenameIndex(
                name: "IX_supply_positions_SuppliesFromSuppliersId",
                table: "supply_positions",
                newName: "IX_supply_positions_id_supplies_from_supplier");

            migrationBuilder.RenameIndex(
                name: "IX_supply_positions_MaterialId",
                table: "supply_positions",
                newName: "IX_supply_positions_id_material");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "supplies_from_suppliers",
                newName: "id_supplier");

            migrationBuilder.RenameIndex(
                name: "IX_supplies_from_suppliers_SupplierId",
                table: "supplies_from_suppliers",
                newName: "IX_supplies_from_suppliers_id_supplier");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "sales_channels",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sales_channels",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_sales_channels_Name",
                table: "sales_channels",
                newName: "IX_sales_channels_name");

            migrationBuilder.RenameIndex(
                name: "IX_sales_channels_Id",
                table: "sales_channels",
                newName: "IX_sales_channels_id");

            migrationBuilder.RenameColumn(
                name: "FinishedProductsId",
                table: "product_parties",
                newName: "id_finished_product");

            migrationBuilder.RenameIndex(
                name: "IX_product_parties_FinishedProductsId",
                table: "product_parties",
                newName: "IX_product_parties_id_finished_product");

            migrationBuilder.RenameColumn(
                name: "IdSalesChannels",
                table: "orders",
                newName: "id_sales_channel");

            migrationBuilder.RenameColumn(
                name: "IdOrderStatuses",
                table: "orders",
                newName: "id_order_status");

            migrationBuilder.RenameColumn(
                name: "IdCustomers",
                table: "orders",
                newName: "id_customer");

            migrationBuilder.RenameIndex(
                name: "IX_orders_IdSalesChannels",
                table: "orders",
                newName: "IX_orders_id_sales_channel");

            migrationBuilder.RenameIndex(
                name: "IX_orders_IdOrderStatuses",
                table: "orders",
                newName: "IX_orders_id_order_status");

            migrationBuilder.RenameIndex(
                name: "IX_orders_IdCustomers",
                table: "orders",
                newName: "IX_orders_id_customer");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "order_statuses",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "order_statuses",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_order_statuses_Name",
                table: "order_statuses",
                newName: "IX_order_statuses_name");

            migrationBuilder.RenameIndex(
                name: "IX_order_statuses_Id",
                table: "order_statuses",
                newName: "IX_order_statuses_id");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "order_items",
                newName: "id_order");

            migrationBuilder.RenameColumn(
                name: "FinishedProductsId",
                table: "order_items",
                newName: "id_finished_product");

            migrationBuilder.RenameIndex(
                name: "IX_order_items_OrderId",
                table: "order_items",
                newName: "IX_order_items_id_order");

            migrationBuilder.RenameIndex(
                name: "IX_order_items_FinishedProductsId",
                table: "order_items",
                newName: "IX_order_items_id_finished_product");

            migrationBuilder.RenameColumn(
                name: "UnitsOfMeasurementId",
                table: "materials",
                newName: "id_units_of_measurement");

            migrationBuilder.RenameIndex(
                name: "IX_materials_UnitsOfMeasurementId",
                table: "materials",
                newName: "IX_materials_id_units_of_measurement");

            migrationBuilder.RenameColumn(
                name: "WarehousesId",
                table: "leftovers_in_warehouses",
                newName: "id_warehouse");

            migrationBuilder.RenameColumn(
                name: "FinishedProductsId",
                table: "leftovers_in_warehouses",
                newName: "id_finished_product");

            migrationBuilder.RenameIndex(
                name: "IX_leftovers_in_warehouses_WarehousesId",
                table: "leftovers_in_warehouses",
                newName: "IX_leftovers_in_warehouses_id_warehouse");

            migrationBuilder.RenameIndex(
                name: "IX_leftovers_in_warehouses_FinishedProductsId",
                table: "leftovers_in_warehouses",
                newName: "IX_leftovers_in_warehouses_id_finished_product");

            migrationBuilder.RenameColumn(
                name: "UnitsOfMeasurementId",
                table: "finished_products",
                newName: "id_units_of_measurement");

            migrationBuilder.RenameColumn(
                name: "RecipeId",
                table: "finished_products",
                newName: "id_recipe");

            migrationBuilder.RenameColumn(
                name: "ProductCategoriesId",
                table: "finished_products",
                newName: "id_product_category");

            migrationBuilder.RenameIndex(
                name: "IX_finished_products_UnitsOfMeasurementId",
                table: "finished_products",
                newName: "IX_finished_products_id_units_of_measurement");

            migrationBuilder.RenameIndex(
                name: "IX_finished_products_RecipeId",
                table: "finished_products",
                newName: "IX_finished_products_id_recipe");

            migrationBuilder.RenameIndex(
                name: "IX_finished_products_ProductCategoriesId",
                table: "finished_products",
                newName: "IX_finished_products_id_product_category");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "customers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "customers",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_customers_Name",
                table: "customers",
                newName: "IX_customers_name");

            migrationBuilder.RenameIndex(
                name: "IX_customers_Id",
                table: "customers",
                newName: "IX_customers_id");

            migrationBuilder.AddForeignKey(
                name: "FK_finished_products_product_categories_id_product_category",
                table: "finished_products",
                column: "id_product_category",
                principalTable: "product_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_finished_products_recipes_id_recipe",
                table: "finished_products",
                column: "id_recipe",
                principalTable: "recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_finished_products_units_of_measurement_id_units_of_measurem~",
                table: "finished_products",
                column: "id_units_of_measurement",
                principalTable: "units_of_measurement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_leftovers_in_warehouses_finished_products_id_finished_produ~",
                table: "leftovers_in_warehouses",
                column: "id_finished_product",
                principalTable: "finished_products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_leftovers_in_warehouses_warehouses_id_warehouse",
                table: "leftovers_in_warehouses",
                column: "id_warehouse",
                principalTable: "warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_materials_units_of_measurement_id_units_of_measurement",
                table: "materials",
                column: "id_units_of_measurement",
                principalTable: "units_of_measurement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_finished_products_id_finished_product",
                table: "order_items",
                column: "id_finished_product",
                principalTable: "finished_products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_orders_id_order",
                table: "order_items",
                column: "id_order",
                principalTable: "orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_customers_id_customer",
                table: "orders",
                column: "id_customer",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_order_statuses_id_order_status",
                table: "orders",
                column: "id_order_status",
                principalTable: "order_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_sales_channels_id_sales_channel",
                table: "orders",
                column: "id_sales_channel",
                principalTable: "sales_channels",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_parties_finished_products_id_finished_product",
                table: "product_parties",
                column: "id_finished_product",
                principalTable: "finished_products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_supplies_from_suppliers_suppliers_id_supplier",
                table: "supplies_from_suppliers",
                column: "id_supplier",
                principalTable: "suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_supply_positions_materials_id_material",
                table: "supply_positions",
                column: "id_material",
                principalTable: "materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_supply_positions_supplies_from_suppliers_id_supplies_from_s~",
                table: "supply_positions",
                column: "id_supplies_from_supplier",
                principalTable: "supplies_from_suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_finished_products_product_categories_id_product_category",
                table: "finished_products");

            migrationBuilder.DropForeignKey(
                name: "FK_finished_products_recipes_id_recipe",
                table: "finished_products");

            migrationBuilder.DropForeignKey(
                name: "FK_finished_products_units_of_measurement_id_units_of_measurem~",
                table: "finished_products");

            migrationBuilder.DropForeignKey(
                name: "FK_leftovers_in_warehouses_finished_products_id_finished_produ~",
                table: "leftovers_in_warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_leftovers_in_warehouses_warehouses_id_warehouse",
                table: "leftovers_in_warehouses");

            migrationBuilder.DropForeignKey(
                name: "FK_materials_units_of_measurement_id_units_of_measurement",
                table: "materials");

            migrationBuilder.DropForeignKey(
                name: "FK_order_items_finished_products_id_finished_product",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK_order_items_orders_id_order",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_customers_id_customer",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_order_statuses_id_order_status",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_sales_channels_id_sales_channel",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_product_parties_finished_products_id_finished_product",
                table: "product_parties");

            migrationBuilder.DropForeignKey(
                name: "FK_supplies_from_suppliers_suppliers_id_supplier",
                table: "supplies_from_suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_supply_positions_materials_id_material",
                table: "supply_positions");

            migrationBuilder.DropForeignKey(
                name: "FK_supply_positions_supplies_from_suppliers_id_supplies_from_s~",
                table: "supply_positions");

            migrationBuilder.RenameColumn(
                name: "id_supplies_from_supplier",
                table: "supply_positions",
                newName: "SuppliesFromSuppliersId");

            migrationBuilder.RenameColumn(
                name: "id_material",
                table: "supply_positions",
                newName: "MaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_supply_positions_id_supplies_from_supplier",
                table: "supply_positions",
                newName: "IX_supply_positions_SuppliesFromSuppliersId");

            migrationBuilder.RenameIndex(
                name: "IX_supply_positions_id_material",
                table: "supply_positions",
                newName: "IX_supply_positions_MaterialId");

            migrationBuilder.RenameColumn(
                name: "id_supplier",
                table: "supplies_from_suppliers",
                newName: "SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_supplies_from_suppliers_id_supplier",
                table: "supplies_from_suppliers",
                newName: "IX_supplies_from_suppliers_SupplierId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "sales_channels",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "sales_channels",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_sales_channels_name",
                table: "sales_channels",
                newName: "IX_sales_channels_Name");

            migrationBuilder.RenameIndex(
                name: "IX_sales_channels_id",
                table: "sales_channels",
                newName: "IX_sales_channels_Id");

            migrationBuilder.RenameColumn(
                name: "id_finished_product",
                table: "product_parties",
                newName: "FinishedProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_product_parties_id_finished_product",
                table: "product_parties",
                newName: "IX_product_parties_FinishedProductsId");

            migrationBuilder.RenameColumn(
                name: "id_sales_channel",
                table: "orders",
                newName: "IdSalesChannels");

            migrationBuilder.RenameColumn(
                name: "id_order_status",
                table: "orders",
                newName: "IdOrderStatuses");

            migrationBuilder.RenameColumn(
                name: "id_customer",
                table: "orders",
                newName: "IdCustomers");

            migrationBuilder.RenameIndex(
                name: "IX_orders_id_sales_channel",
                table: "orders",
                newName: "IX_orders_IdSalesChannels");

            migrationBuilder.RenameIndex(
                name: "IX_orders_id_order_status",
                table: "orders",
                newName: "IX_orders_IdOrderStatuses");

            migrationBuilder.RenameIndex(
                name: "IX_orders_id_customer",
                table: "orders",
                newName: "IX_orders_IdCustomers");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "order_statuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "order_statuses",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_order_statuses_name",
                table: "order_statuses",
                newName: "IX_order_statuses_Name");

            migrationBuilder.RenameIndex(
                name: "IX_order_statuses_id",
                table: "order_statuses",
                newName: "IX_order_statuses_Id");

            migrationBuilder.RenameColumn(
                name: "id_order",
                table: "order_items",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "id_finished_product",
                table: "order_items",
                newName: "FinishedProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_order_items_id_order",
                table: "order_items",
                newName: "IX_order_items_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_order_items_id_finished_product",
                table: "order_items",
                newName: "IX_order_items_FinishedProductsId");

            migrationBuilder.RenameColumn(
                name: "id_units_of_measurement",
                table: "materials",
                newName: "UnitsOfMeasurementId");

            migrationBuilder.RenameIndex(
                name: "IX_materials_id_units_of_measurement",
                table: "materials",
                newName: "IX_materials_UnitsOfMeasurementId");

            migrationBuilder.RenameColumn(
                name: "id_warehouse",
                table: "leftovers_in_warehouses",
                newName: "WarehousesId");

            migrationBuilder.RenameColumn(
                name: "id_finished_product",
                table: "leftovers_in_warehouses",
                newName: "FinishedProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_leftovers_in_warehouses_id_warehouse",
                table: "leftovers_in_warehouses",
                newName: "IX_leftovers_in_warehouses_WarehousesId");

            migrationBuilder.RenameIndex(
                name: "IX_leftovers_in_warehouses_id_finished_product",
                table: "leftovers_in_warehouses",
                newName: "IX_leftovers_in_warehouses_FinishedProductsId");

            migrationBuilder.RenameColumn(
                name: "id_units_of_measurement",
                table: "finished_products",
                newName: "UnitsOfMeasurementId");

            migrationBuilder.RenameColumn(
                name: "id_recipe",
                table: "finished_products",
                newName: "RecipeId");

            migrationBuilder.RenameColumn(
                name: "id_product_category",
                table: "finished_products",
                newName: "ProductCategoriesId");

            migrationBuilder.RenameIndex(
                name: "IX_finished_products_id_units_of_measurement",
                table: "finished_products",
                newName: "IX_finished_products_UnitsOfMeasurementId");

            migrationBuilder.RenameIndex(
                name: "IX_finished_products_id_recipe",
                table: "finished_products",
                newName: "IX_finished_products_RecipeId");

            migrationBuilder.RenameIndex(
                name: "IX_finished_products_id_product_category",
                table: "finished_products",
                newName: "IX_finished_products_ProductCategoriesId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "customers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "customers",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_customers_name",
                table: "customers",
                newName: "IX_customers_Name");

            migrationBuilder.RenameIndex(
                name: "IX_customers_id",
                table: "customers",
                newName: "IX_customers_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_finished_products_product_categories_ProductCategoriesId",
                table: "finished_products",
                column: "ProductCategoriesId",
                principalTable: "product_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_finished_products_recipes_RecipeId",
                table: "finished_products",
                column: "RecipeId",
                principalTable: "recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_finished_products_units_of_measurement_UnitsOfMeasurementId",
                table: "finished_products",
                column: "UnitsOfMeasurementId",
                principalTable: "units_of_measurement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_leftovers_in_warehouses_finished_products_FinishedProductsId",
                table: "leftovers_in_warehouses",
                column: "FinishedProductsId",
                principalTable: "finished_products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_leftovers_in_warehouses_warehouses_WarehousesId",
                table: "leftovers_in_warehouses",
                column: "WarehousesId",
                principalTable: "warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_materials_units_of_measurement_UnitsOfMeasurementId",
                table: "materials",
                column: "UnitsOfMeasurementId",
                principalTable: "units_of_measurement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_finished_products_FinishedProductsId",
                table: "order_items",
                column: "FinishedProductsId",
                principalTable: "finished_products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_orders_OrderId",
                table: "order_items",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_customers_IdCustomers",
                table: "orders",
                column: "IdCustomers",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_order_statuses_IdOrderStatuses",
                table: "orders",
                column: "IdOrderStatuses",
                principalTable: "order_statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_sales_channels_IdSalesChannels",
                table: "orders",
                column: "IdSalesChannels",
                principalTable: "sales_channels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_parties_finished_products_FinishedProductsId",
                table: "product_parties",
                column: "FinishedProductsId",
                principalTable: "finished_products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_supplies_from_suppliers_suppliers_SupplierId",
                table: "supplies_from_suppliers",
                column: "SupplierId",
                principalTable: "suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_supply_positions_materials_MaterialId",
                table: "supply_positions",
                column: "MaterialId",
                principalTable: "materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_supply_positions_supplies_from_suppliers_SuppliesFromSuppli~",
                table: "supply_positions",
                column: "SuppliesFromSuppliersId",
                principalTable: "supplies_from_suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
