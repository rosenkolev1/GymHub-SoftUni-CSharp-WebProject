using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GymHub.Data.Migrations
{
    public partial class AddsaleStatustoSales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 15, 19, 21, 2, 861, DateTimeKind.Utc).AddTicks(1022),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 14, 20, 44, 2, 371, DateTimeKind.Utc).AddTicks(1070));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 15, 19, 21, 2, 859, DateTimeKind.Utc).AddTicks(9685),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 14, 20, 44, 2, 370, DateTimeKind.Utc).AddTicks(118));

            migrationBuilder.AddColumn<string>(
                name: "SaleStatus",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleStatus",
                table: "Sales");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 14, 20, 44, 2, 371, DateTimeKind.Utc).AddTicks(1070),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 15, 19, 21, 2, 861, DateTimeKind.Utc).AddTicks(1022));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 14, 20, 44, 2, 370, DateTimeKind.Utc).AddTicks(118),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 15, 19, 21, 2, 859, DateTimeKind.Utc).AddTicks(9685));
        }
    }
}
