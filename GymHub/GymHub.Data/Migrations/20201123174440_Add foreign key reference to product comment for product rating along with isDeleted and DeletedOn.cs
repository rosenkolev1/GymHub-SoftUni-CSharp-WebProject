using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GymHub.Data.Migrations
{
    public partial class AddforeignkeyreferencetoproductcommentforproductratingalongwithisDeletedandDeletedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                nullable: true,
                defaultValue: new DateTime(2020, 11, 23, 17, 44, 40, 689, DateTimeKind.Utc).AddTicks(6435),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 11, 17, 10, 30, 57, 513, DateTimeKind.Utc).AddTicks(703));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 23, 17, 44, 40, 688, DateTimeKind.Utc).AddTicks(3674),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 17, 10, 30, 57, 511, DateTimeKind.Utc).AddTicks(7769));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "ProductsRatings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductsRatings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProductCommentId",
                table: "ProductsRatings",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductRatingId",
                table: "ProductsComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsRatings_ProductCommentId",
                table: "ProductsRatings",
                column: "ProductCommentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsRatings_ProductsComments_ProductCommentId",
                table: "ProductsRatings",
                column: "ProductCommentId",
                principalTable: "ProductsComments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsRatings_ProductsComments_ProductCommentId",
                table: "ProductsRatings");

            migrationBuilder.DropIndex(
                name: "IX_ProductsRatings_ProductCommentId",
                table: "ProductsRatings");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "ProductsRatings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductsRatings");

            migrationBuilder.DropColumn(
                name: "ProductCommentId",
                table: "ProductsRatings");

            migrationBuilder.DropColumn(
                name: "ProductRatingId",
                table: "ProductsComments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 11, 17, 10, 30, 57, 513, DateTimeKind.Utc).AddTicks(703),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 11, 23, 17, 44, 40, 689, DateTimeKind.Utc).AddTicks(6435));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 17, 10, 30, 57, 511, DateTimeKind.Utc).AddTicks(7769),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 23, 17, 44, 40, 688, DateTimeKind.Utc).AddTicks(3674));
        }
    }
}
