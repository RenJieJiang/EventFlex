using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class updateusertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("12d68c4d-bd05-4dee-8ba4-272179255239"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("bdbd369e-f0bf-4611-98b9-30b8806a7eb4"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("441a33b0-43f1-40f6-8203-216eecca36e5"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("ccb3ac6d-aead-4cbe-8208-9c5183bb57fb"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("4494ef03-2699-4b8a-a91b-23b4efc8b47d"), "Description for Tenant 1", "Tenant 1" },
                    { new Guid("6c409c58-ca1a-4c75-a320-c14330445ea6"), "Description for Tenant 2", "Tenant 2" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("1cb666b9-e2a8-43a4-a19d-0df0092e6999"), 0, "856f1cd7-4891-4e4d-b9de-6408bdc75ff6", "user2@example.com", true, false, null, "", "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAELUZJvfRC2PXDfBbZXBOARk/ut2uBvwgoJ3tfax/BgdgEhw4fo5qoHbwEtsgksw6lw==", null, false, "", new Guid("6c409c58-ca1a-4c75-a320-c14330445ea6"), false, "user2@example.com" },
                    { new Guid("29c3195f-7e02-44c9-8260-1e6ac3aa11dd"), 0, "a356e785-61c3-43f2-8e9a-f6b99ec44823", "jiangrj1@hotmail.com", true, false, null, "", "JIANGRJ1@HOTMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEIxx5yE1+yNWAKOD2Qcl1Nre8KZ0Z6haUu12UiSXwc/HXeMDQ59T+vjhmHyjad9xLg==", null, false, "", new Guid("4494ef03-2699-4b8a-a91b-23b4efc8b47d"), false, "admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1cb666b9-e2a8-43a4-a19d-0df0092e6999"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("29c3195f-7e02-44c9-8260-1e6ac3aa11dd"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("4494ef03-2699-4b8a-a91b-23b4efc8b47d"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("6c409c58-ca1a-4c75-a320-c14330445ea6"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("441a33b0-43f1-40f6-8203-216eecca36e5"), "Description for Tenant 1", "Tenant 1" },
                    { new Guid("ccb3ac6d-aead-4cbe-8208-9c5183bb57fb"), "Description for Tenant 2", "Tenant 2" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("12d68c4d-bd05-4dee-8ba4-272179255239"), 0, "1476a0f4-c058-47f0-bb6c-b978e93cbaf6", "jiangrj1@hotmail.com", true, false, null, "JIANGRJ1@HOTMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEHrgibs0M290wqGiBXMbN3N8/Uar7bhUiWYXfiDgdDdJuD8HzqL0gXUPd5e0hs5N5g==", null, false, "", new Guid("441a33b0-43f1-40f6-8203-216eecca36e5"), false, "admin" },
                    { new Guid("bdbd369e-f0bf-4611-98b9-30b8806a7eb4"), 0, "1d26c3bb-f1a2-4abf-ae32-c367c431ef1f", "user2@example.com", true, false, null, "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAEEbIzxG6o5X4R91PKptX2rpCqTQG43gUFoPfh5rrH67WVqXyjBzk8tJKT67c6NaFIw==", null, false, "", new Guid("ccb3ac6d-aead-4cbe-8208-9c5183bb57fb"), false, "user2@example.com" }
                });
        }
    }
}
