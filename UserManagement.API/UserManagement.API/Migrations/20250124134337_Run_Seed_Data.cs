using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class Run_Seed_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("63f6b994-8246-4951-a649-35c1161b0b3b"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("f4a1f634-afd1-4e6d-b903-40d41b72824b"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("cd53c611-3114-4eb6-adea-ecbe29ebfa4b"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("d755131d-2562-47f1-8a3b-d4d942daa64a"));

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("53a35e25-3a39-4f61-9601-f76bd9678b86"), "Description for Tenant 1", "Tenant 1" },
                    { new Guid("b947d0dc-9e31-47a4-8f3e-85b26614094c"), "Description for Tenant 2", "Tenant 2" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("559d89b4-0c0a-4dc4-91e9-3f6f0559d8f6"), 0, "02cc842f-d6ac-4df9-8dbb-1eda25396b3a", "jiangrj1@hotmail.com", true, false, null, "JIANGRJ1@HOTMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEGKfXxiTRT6/5DwXBYltM4rBwosSkLC2HmAkW1UldC4lVzalrYptZghkmfWqHubBMA==", null, false, "", new Guid("53a35e25-3a39-4f61-9601-f76bd9678b86"), false, "admin" },
                    { new Guid("a154f1d4-fd3e-45b7-94df-237e7a856517"), 0, "58c6ec26-23f2-4a90-9354-2b1d287e846f", "user2@example.com", true, false, null, "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAEEZs28GalUbJfM9N2Do6jPhIQ7wkX5USYzgE0QjL87q9DAhfDa4wdaTv6qTE7a/0dg==", null, false, "", new Guid("b947d0dc-9e31-47a4-8f3e-85b26614094c"), false, "user2@example.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("559d89b4-0c0a-4dc4-91e9-3f6f0559d8f6"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("a154f1d4-fd3e-45b7-94df-237e7a856517"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("53a35e25-3a39-4f61-9601-f76bd9678b86"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("b947d0dc-9e31-47a4-8f3e-85b26614094c"));

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("cd53c611-3114-4eb6-adea-ecbe29ebfa4b"), "Description for Tenant 2", "Tenant 2" },
                    { new Guid("d755131d-2562-47f1-8a3b-d4d942daa64a"), "Description for Tenant 1", "Tenant 1" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("63f6b994-8246-4951-a649-35c1161b0b3b"), 0, "5c05ac06-dece-4932-b712-fbb003cf9764", "user2@example.com", true, false, null, "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAEKjk760FRcoUPSEmx8rtFO+shhKFNq8I5YhDPfIx1yL46uiFGGtXgvz8yPF8fW1vFA==", null, false, "", new Guid("cd53c611-3114-4eb6-adea-ecbe29ebfa4b"), false, "user2@example.com" },
                    { new Guid("f4a1f634-afd1-4e6d-b903-40d41b72824b"), 0, "d0eac1c4-8120-4826-a30f-f33492bd8bd9", "user1@example.com", true, false, null, "USER1@EXAMPLE.COM", "USER1@EXAMPLE.COM", "AQAAAAIAAYagAAAAEEAXigeYLuXCmdPbD54TqqJlR8bfycAcds+pkZpMu4lDlCkd7VMTKPt2NMy8DvDtow==", null, false, "", new Guid("d755131d-2562-47f1-8a3b-d4d942daa64a"), false, "user1@example.com" }
                });
        }
    }
}
