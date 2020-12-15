using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GymHub.Data.Migrations
{
    public partial class AddmunicipalitytoSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 15, 21, 0, 17, 392, DateTimeKind.Utc).AddTicks(1566),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 15, 19, 21, 2, 861, DateTimeKind.Utc).AddTicks(1022));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 15, 21, 0, 17, 391, DateTimeKind.Utc).AddTicks(166),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 15, 19, 21, 2, 859, DateTimeKind.Utc).AddTicks(9685));

            migrationBuilder.AddColumn<string>(
                name: "Municipality",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Municipality",
                table: "Sales");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 15, 19, 21, 2, 861, DateTimeKind.Utc).AddTicks(1022),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 15, 21, 0, 17, 392, DateTimeKind.Utc).AddTicks(1566));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 15, 19, 21, 2, 859, DateTimeKind.Utc).AddTicks(9685),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 15, 21, 0, 17, 391, DateTimeKind.Utc).AddTicks(166));
        }
    }
}
