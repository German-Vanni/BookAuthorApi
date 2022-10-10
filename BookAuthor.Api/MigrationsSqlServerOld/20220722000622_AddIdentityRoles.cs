//using System;
//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace BookAuthor.Api.Migrations
//{
//    public partial class AddIdentityRoles : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.InsertData(
//                table: "AspNetRoles",
//                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
//                values: new object[,]
//                {
//                    { "060eb90f-b430-47eb-bc7b-4453056fbcbd", "10d817f3-3590-4edf-887d-f04b2df6c90c", "User", "USER" },
//                    { "71be4037-0100-412b-a591-4c39fe5a78d1", "7322bfca-9bfa-4c48-97ff-2960b27790aa", "Admin", "ADMIN" }
//                });

//            migrationBuilder.UpdateData(
//                table: "Authors",
//                keyColumn: "Id",
//                keyValue: 1,
//                column: "BirthDay",
//                value: new DateTime(1972, 8, 2, 21, 6, 21, 891, DateTimeKind.Local).AddTicks(5887));

//            migrationBuilder.UpdateData(
//                table: "Authors",
//                keyColumn: "Id",
//                keyValue: 2,
//                column: "BirthDay",
//                value: new DateTime(1987, 7, 30, 21, 6, 21, 891, DateTimeKind.Local).AddTicks(5901));

//            migrationBuilder.UpdateData(
//                table: "Authors",
//                keyColumn: "Id",
//                keyValue: 3,
//                column: "BirthDay",
//                value: new DateTime(1992, 7, 28, 21, 6, 21, 891, DateTimeKind.Local).AddTicks(5902));
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DeleteData(
//                table: "AspNetRoles",
//                keyColumn: "Id",
//                keyValue: "060eb90f-b430-47eb-bc7b-4453056fbcbd");

//            migrationBuilder.DeleteData(
//                table: "AspNetRoles",
//                keyColumn: "Id",
//                keyValue: "71be4037-0100-412b-a591-4c39fe5a78d1");

//            migrationBuilder.UpdateData(
//                table: "Authors",
//                keyColumn: "Id",
//                keyValue: 1,
//                column: "BirthDay",
//                value: new DateTime(1972, 8, 1, 21, 20, 18, 546, DateTimeKind.Local).AddTicks(4895));

//            migrationBuilder.UpdateData(
//                table: "Authors",
//                keyColumn: "Id",
//                keyValue: 2,
//                column: "BirthDay",
//                value: new DateTime(1987, 7, 29, 21, 20, 18, 546, DateTimeKind.Local).AddTicks(4910));

//            migrationBuilder.UpdateData(
//                table: "Authors",
//                keyColumn: "Id",
//                keyValue: 3,
//                column: "BirthDay",
//                value: new DateTime(1992, 7, 27, 21, 20, 18, 546, DateTimeKind.Local).AddTicks(4911));
//        }
//    }
//}
