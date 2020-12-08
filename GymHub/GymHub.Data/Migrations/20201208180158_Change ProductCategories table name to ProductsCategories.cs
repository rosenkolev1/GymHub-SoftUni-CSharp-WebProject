using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GymHub.Data.Migrations
{
    public partial class ChangeProductCategoriestablenametoProductsCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Category_CategoryId",
                table: "ProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Products_ProductId",
                table: "ProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "ProductCategory",
                newName: "ProductsCategories");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategory_ProductId",
                table: "ProductsCategories",
                newName: "IX_ProductsCategories_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategory_CategoryId",
                table: "ProductsCategories",
                newName: "IX_ProductsCategories_CategoryId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 8, 18, 1, 57, 781, DateTimeKind.Utc).AddTicks(3307),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 8, 17, 29, 32, 795, DateTimeKind.Utc).AddTicks(9226));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 8, 18, 1, 57, 780, DateTimeKind.Utc).AddTicks(1975),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 8, 17, 29, 32, 794, DateTimeKind.Utc).AddTicks(8265));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsCategories",
                table: "ProductsCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsCategories_Categories_CategoryId",
                table: "ProductsCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsCategories_Products_ProductId",
                table: "ProductsCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsCategories_Categories_CategoryId",
                table: "ProductsCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsCategories_Products_ProductId",
                table: "ProductsCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsCategories",
                table: "ProductsCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "ProductsCategories",
                newName: "ProductCategory");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsCategories_ProductId",
                table: "ProductCategory",
                newName: "IX_ProductCategory_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsCategories_CategoryId",
                table: "ProductCategory",
                newName: "IX_ProductCategory_CategoryId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 8, 17, 29, 32, 795, DateTimeKind.Utc).AddTicks(9226),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 8, 18, 1, 57, 781, DateTimeKind.Utc).AddTicks(3307));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 8, 17, 29, 32, 794, DateTimeKind.Utc).AddTicks(8265),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 8, 18, 1, 57, 780, DateTimeKind.Utc).AddTicks(1975));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Category_CategoryId",
                table: "ProductCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Products_ProductId",
                table: "ProductCategory",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
