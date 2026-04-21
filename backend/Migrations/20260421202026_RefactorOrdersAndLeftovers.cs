using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CosmeticEnterpriseBack.Migrations
{
    /// <inheritdoc />
    public partial class RefactorOrdersAndLeftovers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_customers_id_customer",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_order_statuses_id_order_status",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_sales_channels_id_sales_channel",
                table: "orders");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "order_statuses");

            migrationBuilder.DropIndex(
                name: "IX_orders_id_customer",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "id_customer",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_id_order_status",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "id_order_status",
                table: "orders");

            migrationBuilder.AddColumn<long>(
                name: "id_user",
                table: "orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_orders_id_user",
                table: "orders",
                column: "id_user");

            migrationBuilder.AddColumn<string>(
                name: "comment",
                table: "orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                table: "orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<decimal>(
                name: "delivery_price",
                table: "orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "delivery_status",
                table: "orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "order_status",
                table: "orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "payment_method",
                table: "orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "payment_status",
                table: "orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "payment_type",
                table: "orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "total_amount",
                table: "orders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at_utc",
                table: "orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()");

            migrationBuilder.AddColumn<decimal>(
                name: "line_total",
                table: "order_items",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "order_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "unit_price",
                table: "order_items",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "leftovers_in_warehouses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "reserved_quantity",
                table: "leftovers_in_warehouses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_sales_channels_id_sales_channel",
                table: "orders",
                column: "id_sales_channel",
                principalTable: "sales_channels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_id_user",
                table: "orders",
                column: "id_user",
                principalTable: "users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_sales_channels_id_sales_channel",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_id_user",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "comment",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "created_at_utc",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "delivery_price",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "delivery_status",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "order_status",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "payment_method",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "payment_status",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "payment_type",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "line_total",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "unit_price",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "leftovers_in_warehouses");

            migrationBuilder.DropColumn(
                name: "reserved_quantity",
                table: "leftovers_in_warehouses");

            migrationBuilder.DropIndex(
                name: "IX_orders_id_user",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "id_user",
                table: "orders");

            migrationBuilder.AddColumn<long>(
                name: "id_order_status",
                table: "orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_orders_id_order_status",
                table: "orders",
                column: "id_order_status");

            migrationBuilder.AddColumn<long>(
                name: "id_customer",
                table: "orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_statuses",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_statuses", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_id_customer",
                table: "orders",
                column: "id_customer");

            migrationBuilder.CreateIndex(
                name: "IX_customers_id",
                table: "customers",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_customers_name",
                table: "customers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_statuses_id",
                table: "order_statuses",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_order_statuses_name",
                table: "order_statuses",
                column: "name",
                unique: true);

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
        }
    }
}