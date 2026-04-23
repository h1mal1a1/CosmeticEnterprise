using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CosmeticEnterpriseBack.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_categories",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "recipes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sales_channels",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sales_channels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suppliers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "units_of_measurement",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_units_of_measurement", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    password_hash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    role_name = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "warehouses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "supplies_from_suppliers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_supplier = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplies_from_suppliers", x => x.id);
                    table.ForeignKey(
                        name: "FK_supplies_from_suppliers_suppliers_id_supplier",
                        column: x => x.id_supplier,
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "finished_products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    id_recipe = table.Column<long>(type: "bigint", nullable: false),
                    id_product_category = table.Column<long>(type: "bigint", nullable: false),
                    id_units_of_measurement = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_finished_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_finished_products_product_categories_id_product_category",
                        column: x => x.id_product_category,
                        principalTable: "product_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_finished_products_recipes_id_recipe",
                        column: x => x.id_recipe,
                        principalTable: "recipes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_finished_products_units_of_measurement_id_units_of_measurem~",
                        column: x => x.id_units_of_measurement,
                        principalTable: "units_of_measurement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_units_of_measurement = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materials", x => x.id);
                    table.ForeignKey(
                        name: "FK_materials_units_of_measurement_id_units_of_measurement",
                        column: x => x.id_units_of_measurement,
                        principalTable: "units_of_measurement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shopping_carts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_user = table.Column<long>(type: "bigint", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopping_carts", x => x.id);
                    table.ForeignKey(
                        name: "FK_shopping_carts_users_id_user",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_addresses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_user = table.Column<long>(type: "bigint", nullable: false),
                    recipient_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    city = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    house = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    apartment = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    postal_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    is_default = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_addresses_users_id_user",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_refresh_tokens",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_user = table.Column<long>(type: "bigint", nullable: false),
                    refresh_token_hash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    expires_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revoked_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false),
                    created_by_ip = table.Column<string>(type: "text", nullable: true),
                    revoked_by_ip = table.Column<string>(type: "text", nullable: true),
                    device_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_refresh_tokens_users_id_user",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "leftovers_in_warehouses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_finished_product = table.Column<long>(type: "bigint", nullable: false),
                    id_warehouse = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    reserved_quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leftovers_in_warehouses", x => x.id);
                    table.ForeignKey(
                        name: "FK_leftovers_in_warehouses_finished_products_id_finished_produ~",
                        column: x => x.id_finished_product,
                        principalTable: "finished_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_leftovers_in_warehouses_warehouses_id_warehouse",
                        column: x => x.id_warehouse,
                        principalTable: "warehouses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_finished_product = table.Column<long>(type: "bigint", nullable: false),
                    object_key = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_main = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_images_finished_products_id_finished_product",
                        column: x => x.id_finished_product,
                        principalTable: "finished_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_parties",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_finished_product = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_parties", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_parties_finished_products_id_finished_product",
                        column: x => x.id_finished_product,
                        principalTable: "finished_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "supply_positions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_supplies_from_supplier = table.Column<long>(type: "bigint", nullable: false),
                    id_material = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supply_positions", x => x.id);
                    table.ForeignKey(
                        name: "FK_supply_positions_materials_id_material",
                        column: x => x.id_material,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_supply_positions_supplies_from_suppliers_id_supplies_from_s~",
                        column: x => x.id_supplies_from_supplier,
                        principalTable: "supplies_from_suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shopping_cart_items",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_shopping_cart = table.Column<long>(type: "bigint", nullable: false),
                    id_finished_product = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopping_cart_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_shopping_cart_items_finished_products_id_finished_product",
                        column: x => x.id_finished_product,
                        principalTable: "finished_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_shopping_cart_items_shopping_carts_id_shopping_cart",
                        column: x => x.id_shopping_cart,
                        principalTable: "shopping_carts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_user = table.Column<long>(type: "bigint", nullable: false),
                    id_user_address = table.Column<long>(type: "bigint", nullable: false),
                    id_sales_channel = table.Column<long>(type: "bigint", nullable: false),
                    order_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    delivery_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    payment_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    payment_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    payment_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    delivery_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    comment = table.Column<string>(type: "text", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_sales_channels_id_sales_channel",
                        column: x => x.id_sales_channel,
                        principalTable: "sales_channels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_user_addresses_id_user_address",
                        column: x => x.id_user_address,
                        principalTable: "user_addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_users_id_user",
                        column: x => x.id_user,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_order = table.Column<long>(type: "bigint", nullable: false),
                    id_finished_product = table.Column<long>(type: "bigint", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    line_total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_items_finished_products_id_finished_product",
                        column: x => x.id_finished_product,
                        principalTable: "finished_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_order_items_orders_id_order",
                        column: x => x.id_order,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_finished_products_id_product_category",
                table: "finished_products",
                column: "id_product_category");

            migrationBuilder.CreateIndex(
                name: "IX_finished_products_id_recipe",
                table: "finished_products",
                column: "id_recipe");

            migrationBuilder.CreateIndex(
                name: "IX_finished_products_id_units_of_measurement",
                table: "finished_products",
                column: "id_units_of_measurement");

            migrationBuilder.CreateIndex(
                name: "IX_leftovers_in_warehouses_id_finished_product",
                table: "leftovers_in_warehouses",
                column: "id_finished_product");

            migrationBuilder.CreateIndex(
                name: "IX_leftovers_in_warehouses_id_warehouse",
                table: "leftovers_in_warehouses",
                column: "id_warehouse");

            migrationBuilder.CreateIndex(
                name: "IX_materials_id_units_of_measurement",
                table: "materials",
                column: "id_units_of_measurement");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_id_finished_product",
                table: "order_items",
                column: "id_finished_product");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_id_order",
                table: "order_items",
                column: "id_order");

            migrationBuilder.CreateIndex(
                name: "IX_orders_id_sales_channel",
                table: "orders",
                column: "id_sales_channel");

            migrationBuilder.CreateIndex(
                name: "IX_orders_id_user",
                table: "orders",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_orders_id_user_address",
                table: "orders",
                column: "id_user_address");

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_name",
                table: "product_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_images_id_finished_product",
                table: "product_images",
                column: "id_finished_product");

            migrationBuilder.CreateIndex(
                name: "IX_product_images_id_finished_product_is_main",
                table: "product_images",
                columns: new[] { "id_finished_product", "is_main" },
                unique: true,
                filter: "\"is_main\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_product_parties_id_finished_product",
                table: "product_parties",
                column: "id_finished_product");

            migrationBuilder.CreateIndex(
                name: "IX_recipes_name",
                table: "recipes",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sales_channels_name",
                table: "sales_channels",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shopping_cart_items_id_finished_product",
                table: "shopping_cart_items",
                column: "id_finished_product");

            migrationBuilder.CreateIndex(
                name: "IX_shopping_cart_items_id_shopping_cart_id_finished_product",
                table: "shopping_cart_items",
                columns: new[] { "id_shopping_cart", "id_finished_product" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shopping_carts_id_user",
                table: "shopping_carts",
                column: "id_user",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_supplies_from_suppliers_id_supplier",
                table: "supplies_from_suppliers",
                column: "id_supplier");

            migrationBuilder.CreateIndex(
                name: "IX_supply_positions_id_material",
                table: "supply_positions",
                column: "id_material");

            migrationBuilder.CreateIndex(
                name: "IX_supply_positions_id_supplies_from_supplier",
                table: "supply_positions",
                column: "id_supplies_from_supplier");

            migrationBuilder.CreateIndex(
                name: "IX_units_of_measurement_name",
                table: "units_of_measurement",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_addresses_id_user",
                table: "user_addresses",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_user_refresh_tokens_id_user",
                table: "user_refresh_tokens",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "leftovers_in_warehouses");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "product_images");

            migrationBuilder.DropTable(
                name: "product_parties");

            migrationBuilder.DropTable(
                name: "shopping_cart_items");

            migrationBuilder.DropTable(
                name: "supply_positions");

            migrationBuilder.DropTable(
                name: "user_refresh_tokens");

            migrationBuilder.DropTable(
                name: "warehouses");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "finished_products");

            migrationBuilder.DropTable(
                name: "shopping_carts");

            migrationBuilder.DropTable(
                name: "materials");

            migrationBuilder.DropTable(
                name: "supplies_from_suppliers");

            migrationBuilder.DropTable(
                name: "sales_channels");

            migrationBuilder.DropTable(
                name: "user_addresses");

            migrationBuilder.DropTable(
                name: "product_categories");

            migrationBuilder.DropTable(
                name: "recipes");

            migrationBuilder.DropTable(
                name: "units_of_measurement");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
