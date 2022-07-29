using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookAuthor.Api.Migrations
{
    public partial class AddSoftDeleteAndApproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Books",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Authors",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Authors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "Authors",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1594d6a0-d9fe-4deb-8f05-9c5ec66474d0",
                column: "ConcurrencyStamp",
                value: "81b7d7d5-9213-4ac5-aeca-cc009bbbc4e5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72605b6b-1f30-42ff-a82c-be4733912ecf",
                column: "ConcurrencyStamp",
                value: "b604033b-05c1-40f9-afca-d9f7de8ef3bf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6f7a887a-0fb9-40cb-bc04-b97cf2fcf36b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "18b375c0-138b-4432-bf4f-93ebd7d58061", "AQAAAAEAACcQAAAAEFVPbtdv+Df4Q1d/H6g9Awn69jTuYN+Iwb+VBkkvqcRAq9wlc+O/iJXkQYkFsGY9rg==", "1f93b25a-a537-4f60-a026-21193e98a2e9" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Authors");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1594d6a0-d9fe-4deb-8f05-9c5ec66474d0",
                column: "ConcurrencyStamp",
                value: "bff39b19-1b93-4af1-a505-1acb6af4eb9a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72605b6b-1f30-42ff-a82c-be4733912ecf",
                column: "ConcurrencyStamp",
                value: "9a3c51f0-e36c-4465-8bb2-e861dd0e7eef");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6f7a887a-0fb9-40cb-bc04-b97cf2fcf36b",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9db9ff89-5590-412c-94a0-852d757a98ac", "AQAAAAEAACcQAAAAEJyWzlWb+Bpzf4M3Nu1XN2oTu2C/ly3h1Nhg0l6vtxus1SBt/pVcE+Ta9i6MH30YQw==", "718d5b1d-3ed3-4192-a8b4-c07c904bf268" });
        }
    }
}
