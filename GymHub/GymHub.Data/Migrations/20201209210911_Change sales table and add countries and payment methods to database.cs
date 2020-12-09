using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GymHub.Data.Migrations
{
    public partial class Changesalestableandaddcountriesandpaymentmethodstodatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 9, 21, 9, 10, 990, DateTimeKind.Utc).AddTicks(6604),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 8, 18, 1, 57, 781, DateTimeKind.Utc).AddTicks(3307));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 9, 21, 9, 10, 989, DateTimeKind.Utc).AddTicks(5584),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 8, 18, 1, 57, 780, DateTimeKind.Utc).AddTicks(1975));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "Sales",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Sales",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethodId",
                table: "Sales",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Postcode",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "ProductsSales",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CountryId",
                table: "Sales",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_PaymentMethodId",
                table: "Sales",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Countries_CountryId",
                table: "Sales",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_PaymentMethods_PaymentMethodId",
                table: "Sales",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Countries_CountryId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_PaymentMethods_PaymentMethodId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_Sales_CountryId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_PaymentMethodId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Postcode",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductsSales");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RegisteredOn",
                table: "Users",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2020, 12, 8, 18, 1, 57, 781, DateTimeKind.Utc).AddTicks(3307),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2020, 12, 9, 21, 9, 10, 990, DateTimeKind.Utc).AddTicks(6604));

            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedOn",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 8, 18, 1, 57, 780, DateTimeKind.Utc).AddTicks(1975),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 9, 21, 9, 10, 989, DateTimeKind.Utc).AddTicks(5584));
        }
    }
}
