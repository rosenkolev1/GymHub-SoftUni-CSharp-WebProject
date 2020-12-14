using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GymHub.Data.Migrations
{
    public partial class AddIDeleteableEntitytoproductcommentslikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                nullable: true,
                defaultValue: new DateTime(2020, 11, 26, 16, 27, 59, 12, DateTimeKind.Utc).AddTicks(8055),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 11, 26, 13, 11, 7, 248, DateTimeKind.Utc).AddTicks(559));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 26, 16, 27, 59, 11, DateTimeKind.Utc).AddTicks(5284),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 26, 13, 11, 7, 246, DateTimeKind.Utc).AddTicks(7324));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ProductCommentLikes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductCommentLikes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ProductCommentLikes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductCommentLikes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 11, 26, 13, 11, 7, 248, DateTimeKind.Utc).AddTicks(559),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 11, 26, 16, 27, 59, 12, DateTimeKind.Utc).AddTicks(8055));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 26, 13, 11, 7, 246, DateTimeKind.Utc).AddTicks(7324),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 26, 16, 27, 59, 11, DateTimeKind.Utc).AddTicks(5284));
        }
    }
}
