using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GymHub.Data.Migrations
{
    public partial class Addsalestatustabletodatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleStatus",
                table: "Sales");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 17, 14, 26, 27, 928, DateTimeKind.Utc).AddTicks(3751),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 15, 21, 0, 17, 392, DateTimeKind.Utc).AddTicks(1566));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 17, 14, 26, 27, 927, DateTimeKind.Utc).AddTicks(2972),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 15, 21, 0, 17, 391, DateTimeKind.Utc).AddTicks(166));

            migrationBuilder.AddColumn<string>(
                name: "SaleStatusId",
                table: "Sales",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SaleStatuses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SaleStatusId",
                table: "Sales",
                column: "SaleStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_SaleStatuses_SaleStatusId",
                table: "Sales",
                column: "SaleStatusId",
                principalTable: "SaleStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_SaleStatuses_SaleStatusId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "SaleStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Sales_SaleStatusId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SaleStatusId",
                table: "Sales");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 15, 21, 0, 17, 392, DateTimeKind.Utc).AddTicks(1566),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 17, 14, 26, 27, 928, DateTimeKind.Utc).AddTicks(3751));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 15, 21, 0, 17, 391, DateTimeKind.Utc).AddTicks(166),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 17, 14, 26, 27, 927, DateTimeKind.Utc).AddTicks(2972));

            migrationBuilder.AddColumn<string>(
                name: "SaleStatus",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
