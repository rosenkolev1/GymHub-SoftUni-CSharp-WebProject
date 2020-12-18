using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GymHub.Data.Migrations
{
    public partial class Fixrelationshipbetweenproductmainimageandproductsimages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsImages_Products_ProductId",
                table: "ProductsImages");

            migrationBuilder.DropColumn(
                name: "MainImage",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 18, 16, 41, 37, 448, DateTimeKind.Utc).AddTicks(2243),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 17, 20, 25, 36, 831, DateTimeKind.Utc).AddTicks(1767));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 18, 16, 41, 37, 447, DateTimeKind.Utc).AddTicks(1426),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 17, 20, 25, 36, 830, DateTimeKind.Utc).AddTicks(298));

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "ProductsImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsImages_Products_ProductId",
                table: "ProductsImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsImages_Products_ProductId",
                table: "ProductsImages");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "ProductsImages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 17, 20, 25, 36, 831, DateTimeKind.Utc).AddTicks(1767),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 18, 16, 41, 37, 448, DateTimeKind.Utc).AddTicks(2243));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 17, 20, 25, 36, 830, DateTimeKind.Utc).AddTicks(298),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 18, 16, 41, 37, 447, DateTimeKind.Utc).AddTicks(1426));

            migrationBuilder.AddColumn<string>(
                name: "MainImage",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsImages_Products_ProductId",
                table: "ProductsImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
