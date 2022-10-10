//using Microsoft.EntityFrameworkCore.Migrations;

//#nullable disable

//namespace BookAuthor.Api.Migrations
//{
//    public partial class AddAdminAccountOnSeed : Migration
//    {
//        protected override void Up(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DeleteData(
//                table: "AspNetRoles",
//                keyColumn: "Id",
//                keyValue: "6e8cf144-1565-4687-8dd6-8cc9d67da2ca");

//            migrationBuilder.DeleteData(
//                table: "AspNetRoles",
//                keyColumn: "Id",
//                keyValue: "a3fb20c7-8ebd-4a7c-b833-bed0b11ee550");

//            migrationBuilder.InsertData(
//                table: "AspNetRoles",
//                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
//                values: new object[] { "1594d6a0-d9fe-4deb-8f05-9c5ec66474d0", "bff39b19-1b93-4af1-a505-1acb6af4eb9a", "Admin", "ADMIN" });

//            migrationBuilder.InsertData(
//                table: "AspNetRoles",
//                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
//                values: new object[] { "72605b6b-1f30-42ff-a82c-be4733912ecf", "9a3c51f0-e36c-4465-8bb2-e861dd0e7eef", "User", "USER" });

//            migrationBuilder.InsertData(
//                table: "AspNetUsers",
//                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
//                values: new object[] { "6f7a887a-0fb9-40cb-bc04-b97cf2fcf36b", 0, "9db9ff89-5590-412c-94a0-852d757a98ac", "admin@admin.com", false, false, null, "ADMIN@ADMIN.COM", "ADMIN@ADMIN.COM", "AQAAAAEAACcQAAAAEJyWzlWb+Bpzf4M3Nu1XN2oTu2C/ly3h1Nhg0l6vtxus1SBt/pVcE+Ta9i6MH30YQw==", null, false, "718d5b1d-3ed3-4192-a8b4-c07c904bf268", false, "admin@admin.com" });

//            migrationBuilder.InsertData(
//                table: "AspNetUserRoles",
//                columns: new[] { "RoleId", "UserId" },
//                values: new object[] { "1594d6a0-d9fe-4deb-8f05-9c5ec66474d0", "6f7a887a-0fb9-40cb-bc04-b97cf2fcf36b" });
//        }

//        protected override void Down(MigrationBuilder migrationBuilder)
//        {
//            migrationBuilder.DeleteData(
//                table: "AspNetRoles",
//                keyColumn: "Id",
//                keyValue: "72605b6b-1f30-42ff-a82c-be4733912ecf");

//            migrationBuilder.DeleteData(
//                table: "AspNetUserRoles",
//                keyColumns: new[] { "RoleId", "UserId" },
//                keyValues: new object[] { "1594d6a0-d9fe-4deb-8f05-9c5ec66474d0", "6f7a887a-0fb9-40cb-bc04-b97cf2fcf36b" });

//            migrationBuilder.DeleteData(
//                table: "AspNetRoles",
//                keyColumn: "Id",
//                keyValue: "1594d6a0-d9fe-4deb-8f05-9c5ec66474d0");

//            migrationBuilder.DeleteData(
//                table: "AspNetUsers",
//                keyColumn: "Id",
//                keyValue: "6f7a887a-0fb9-40cb-bc04-b97cf2fcf36b");

//            migrationBuilder.InsertData(
//                table: "AspNetRoles",
//                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
//                values: new object[] { "6e8cf144-1565-4687-8dd6-8cc9d67da2ca", "446dcbf1-8b4a-498a-9e74-01f7b3055310", "User", "USER" });

//            migrationBuilder.InsertData(
//                table: "AspNetRoles",
//                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
//                values: new object[] { "a3fb20c7-8ebd-4a7c-b833-bed0b11ee550", "c05e1f92-fbfb-4e7c-9cc3-48d5bc29d192", "Admin", "ADMIN" });
//        }
//    }
//}
