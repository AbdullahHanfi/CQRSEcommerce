using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "DiscountRate", "ImagePath", "MinimumQuantity", "Name", "Price", "ProductCode" },
                values: new object[,]
                {
                    { new Guid("2cf8249e-ffe7-4e0a-bc12-075a693f606d"), "phone", 20m, "products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg", 1, "phone", 10m, "p4" },
                    { new Guid("56060871-8ad5-4a2d-9ef5-97285a71a84d"), "phone", 20m, "products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg", 1, "phone", 10m, "p1" },
                    { new Guid("612b8625-a0a5-4337-afc9-d85b096c0c1d"), "phone", 20m, "products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg", 1, "phone", 10m, "p2" },
                    { new Guid("7ec3ba39-a895-4186-8b97-b212f1b9d02f"), "phone", 20m, "products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg", 1, "phone", 10m, "p7" },
                    { new Guid("8fcee124-4bf3-494a-93cf-52bc06336513"), "phone", 20m, "products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg", 1, "phone", 10m, "p6" },
                    { new Guid("9a1da725-3dbc-4701-9ac2-9109c04124ab"), "phone", 20m, "products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg", 1, "phone", 10m, "p5" },
                    { new Guid("9d41ce90-3410-4285-9051-6a94aca291d0"), "phone", 20m, "products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg", 1, "phone", 10m, "p8" },
                    { new Guid("c0ff3b27-2432-45ed-8fcc-969fe4a8fccb"), "phone", 20m, "products\\5019a1de-599b-4e3f-ad48-07d88eb42605.jpg", 1, "phone", 10m, "p3" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCode",
                table: "Products",
                column: "ProductCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductCode",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("2cf8249e-ffe7-4e0a-bc12-075a693f606d"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("56060871-8ad5-4a2d-9ef5-97285a71a84d"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("612b8625-a0a5-4337-afc9-d85b096c0c1d"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("7ec3ba39-a895-4186-8b97-b212f1b9d02f"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("8fcee124-4bf3-494a-93cf-52bc06336513"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9a1da725-3dbc-4701-9ac2-9109c04124ab"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9d41ce90-3410-4285-9051-6a94aca291d0"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c0ff3b27-2432-45ed-8fcc-969fe4a8fccb"));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);
        }
    }
}
