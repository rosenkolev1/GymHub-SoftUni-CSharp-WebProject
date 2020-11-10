using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GymHub.Data.Migrations
{
    public partial class AddPurchasedOncolumntotableSales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                nullable: true,
                defaultValue: new DateTime(2020, 11, 7, 14, 56, 53, 149, DateTimeKind.Utc).AddTicks(9445),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 11, 7, 14, 46, 39, 470, DateTimeKind.Utc).AddTicks(4851));

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 7, 14, 56, 53, 148, DateTimeKind.Utc).AddTicks(6040));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasedOn",
                table: "Sales");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 11, 7, 14, 46, 39, 470, DateTimeKind.Utc).AddTicks(4851),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 11, 7, 14, 56, 53, 149, DateTimeKind.Utc).AddTicks(9445));
        }
    }
}
