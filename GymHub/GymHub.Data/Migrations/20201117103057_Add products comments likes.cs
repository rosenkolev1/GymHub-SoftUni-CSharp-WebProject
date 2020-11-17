using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GymHub.Data.Migrations
{
    public partial class Addproductscommentslikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                nullable: true,
                defaultValue: new DateTime(2020, 11, 17, 10, 30, 57, 513, DateTimeKind.Utc).AddTicks(703),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 11, 7, 14, 56, 53, 149, DateTimeKind.Utc).AddTicks(9445));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 17, 10, 30, 57, 511, DateTimeKind.Utc).AddTicks(7769),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 7, 14, 56, 53, 148, DateTimeKind.Utc).AddTicks(6040));

            migrationBuilder.CreateTable(
                name: "ProductCommentLikes",
                columns: table => new
                {
                    ProductCommentId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCommentLikes", x => new { x.ProductCommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProductCommentLikes_ProductsComments_ProductCommentId",
                        column: x => x.ProductCommentId,
                        principalTable: "ProductsComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCommentLikes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCommentLikes_UserId",
                table: "ProductCommentLikes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCommentLikes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 11, 7, 14, 56, 53, 149, DateTimeKind.Utc).AddTicks(9445),
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 11, 17, 10, 30, 57, 513, DateTimeKind.Utc).AddTicks(703));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 7, 14, 56, 53, 148, DateTimeKind.Utc).AddTicks(6040),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 17, 10, 30, 57, 511, DateTimeKind.Utc).AddTicks(7769));
        }
    }
}
