using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GymHub.Data.Migrations
{
    public partial class RemovePurchasedOnfromSalestable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasedOn",
                table: "Sales");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                nullable: true,
                defaultValue: new DateTime(2020, 11, 7, 14, 46, 39, 470, DateTimeKind.Utc).AddTicks(4851),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 11, 7, 14, 43, 4, 769, DateTimeKind.Utc).AddTicks(4539));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 11, 7, 14, 43, 4, 769, DateTimeKind.Utc).AddTicks(4539),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 11, 7, 14, 46, 39, 470, DateTimeKind.Utc).AddTicks(4851));

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 7, 14, 43, 4, 768, DateTimeKind.Utc).AddTicks(1520));
        }
    }
}
