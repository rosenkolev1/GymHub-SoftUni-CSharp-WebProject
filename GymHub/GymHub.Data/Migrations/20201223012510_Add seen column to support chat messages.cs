using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GymHub.Data.Migrations
{
    public partial class Addseencolumntosupportchatmessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 23, 1, 25, 9, 929, DateTimeKind.Utc).AddTicks(2917),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 22, 20, 5, 33, 649, DateTimeKind.Utc).AddTicks(8992));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 23, 1, 25, 9, 928, DateTimeKind.Utc).AddTicks(2157),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 22, 20, 5, 33, 648, DateTimeKind.Utc).AddTicks(8243));

            migrationBuilder.AddColumn<bool>(
                name: "HasBeenSeen",
                table: "ContactsChatMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBeenSeen",
                table: "ContactsChatMessages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 22, 20, 5, 33, 649, DateTimeKind.Utc).AddTicks(8992),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 23, 1, 25, 9, 929, DateTimeKind.Utc).AddTicks(2917));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 22, 20, 5, 33, 648, DateTimeKind.Utc).AddTicks(8243),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 23, 1, 25, 9, 928, DateTimeKind.Utc).AddTicks(2157));
        }
    }
}
