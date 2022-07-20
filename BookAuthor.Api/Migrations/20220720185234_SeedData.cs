using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookAuthor.Api.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "BirthDay", "Name" },
                values: new object[] { 1, new DateTime(1972, 8, 1, 15, 52, 34, 258, DateTimeKind.Local).AddTicks(6101), "Author One" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "BirthDay", "Name" },
                values: new object[] { 2, new DateTime(1987, 7, 29, 15, 52, 34, 258, DateTimeKind.Local).AddTicks(6118), "Author Two" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "BirthDay", "Name" },
                values: new object[] { 3, new DateTime(1992, 7, 27, 15, 52, 34, 258, DateTimeKind.Local).AddTicks(6119), "Author Three" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "ISBN10", "ISBN13", "Title" },
                values: new object[,]
                {
                    { 1, 1, "4242421421", "4242421421123", "Title 1" },
                    { 2, 1, "4242421421", "4242421421123", "Title 2" },
                    { 3, 2, "4242421421", "4242421421123", "Title 3" },
                    { 4, 3, "4242421421", "4242421421123", "Title 4" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
