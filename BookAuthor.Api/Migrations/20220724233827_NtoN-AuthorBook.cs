using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookAuthor.Api.Migrations
{
    public partial class NtoNAuthorBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_AuthorId",
                table: "Books");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "060eb90f-b430-47eb-bc7b-4453056fbcbd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71be4037-0100-412b-a591-4c39fe5a78d1");

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

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "ISBN13",
                table: "Books",
                newName: "Isbn13");

            migrationBuilder.RenameColumn(
                name: "ISBN10",
                table: "Books",
                newName: "Isbn10");

            migrationBuilder.RenameColumn(
                name: "BirthDay",
                table: "Authors",
                newName: "DeathDate");

            migrationBuilder.AddColumn<int>(
                name: "PageCount",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Authors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomePlace",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuthorBooks",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBooks", x => new { x.BookId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_AuthorBooks_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "65beaf15-c642-49a4-ad3d-40e5a49645fc", "d3f05de0-8b84-4401-83dc-dcde59b8b70e", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c1f1a3b2-251c-45fa-8c06-5eaeb1f298eb", "7eeb23c3-8d6b-4ad5-b5dc-05a2798b3e19", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBooks_AuthorId",
                table: "AuthorBooks",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBooks");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "65beaf15-c642-49a4-ad3d-40e5a49645fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1f1a3b2-251c-45fa-8c06-5eaeb1f298eb");

            migrationBuilder.DropColumn(
                name: "PageCount",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "About",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "HomePlace",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "Isbn13",
                table: "Books",
                newName: "ISBN13");

            migrationBuilder.RenameColumn(
                name: "Isbn10",
                table: "Books",
                newName: "ISBN10");

            migrationBuilder.RenameColumn(
                name: "DeathDate",
                table: "Authors",
                newName: "BirthDay");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "060eb90f-b430-47eb-bc7b-4453056fbcbd", "10d817f3-3590-4edf-887d-f04b2df6c90c", "User", "USER" },
                    { "71be4037-0100-412b-a591-4c39fe5a78d1", "7322bfca-9bfa-4c48-97ff-2960b27790aa", "Admin", "ADMIN" }
                });

            //migrationBuilder.InsertData(
            //    table: "Authors",
            //    columns: new[] { "Id", "BirthDay", "Name" },
            //    values: new object[,]
            //    {
            //        { 1, new DateTime(1972, 8, 2, 21, 6, 21, 891, DateTimeKind.Local).AddTicks(5887), "Author One" },
            //        { 2, new DateTime(1987, 7, 30, 21, 6, 21, 891, DateTimeKind.Local).AddTicks(5901), "Author Two" },
            //        { 3, new DateTime(1992, 7, 28, 21, 6, 21, 891, DateTimeKind.Local).AddTicks(5902), "Author Three" }
            //    });

            //migrationBuilder.InsertData(
            //    table: "Books",
            //    columns: new[] { "Id", "AuthorId", "ISBN10", "ISBN13", "Title" },
            //    values: new object[,]
            //    {
            //        { 1, 1, "4242421421", "4242421421123", "Title 1" },
            //        { 2, 1, "4242421421", "4242421421123", "Title 2" },
            //        { 3, 2, "4242421421", "4242421421123", "Title 3" },
            //        { 4, 3, "4242421421", "4242421421123", "Title 4" }
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
