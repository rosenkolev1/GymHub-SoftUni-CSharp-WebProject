using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GymHub.Data.Migrations
{
    public partial class AddModifiedOncolumntoproductcomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                nullable: true,
                defaultValue: new DateTime(2020, 11, 26, 13, 11, 7, 248, DateTimeKind.Utc).AddTicks(559),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 11, 23, 20, 8, 28, 593, DateTimeKind.Utc).AddTicks(9679));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 26, 13, 11, 7, 246, DateTimeKind.Utc).AddTicks(7324),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 23, 20, 8, 28, 592, DateTimeKind.Utc).AddTicks(6699));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "ProductsComments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "ProductsComments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 11, 23, 20, 8, 28, 593, DateTimeKind.Utc).AddTicks(9679),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 11, 26, 13, 11, 7, 248, DateTimeKind.Utc).AddTicks(559));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 23, 20, 8, 28, 592, DateTimeKind.Utc).AddTicks(6699),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 26, 13, 11, 7, 246, DateTimeKind.Utc).AddTicks(7324));
        }
    }
}
