using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class seed_role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("4eac9065-7d05-4378-b2ca-d5874bbb07e6"), null, "Admin", "ADMIN" },
                    { new Guid("6fbafe58-95bb-42e0-a5dd-395f6a26cb63"), null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("c456f316-f9c9-4d67-ac6b-2a0e232b4568"), "Description for Tenant 2", "Tenant 2" },
                    { new Guid("cb46b933-f260-4b9a-b47a-32828c42b04d"), "Description for Tenant 1", "Tenant 1" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("72127d35-3d9e-417a-afb6-30e30f1c524b"), 0, "405a2ca7-8cbf-4157-9560-b8a67252283a", "user2@example.com", true, false, null, "", "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAEIsx7dtBkwiXLiHi1UXo9vJs4tT/MLDxniPR7RDOT4pZWlMkBDHUXw4JuW+EdRIPwQ==", null, false, "", new Guid("c456f316-f9c9-4d67-ac6b-2a0e232b4568"), false, "user2@example.com" },
                    { new Guid("88068a70-bb31-45e6-af87-1fd55362d3bc"), 0, "9754b9c6-4ab7-4c1c-9e5d-7630e444caed", "jiangrj1@hotmail.com", true, false, null, "", "JIANGRJ1@HOTMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEJ8wN2hW6TQsN/g5//fFPzx/M666I/91D73Z6hjj7pSgjKZ67dpeJbEUCFd8HPcOiw==", null, false, "", new Guid("cb46b933-f260-4b9a-b47a-32828c42b04d"), false, "admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4eac9065-7d05-4378-b2ca-d5874bbb07e6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6fbafe58-95bb-42e0-a5dd-395f6a26cb63"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("72127d35-3d9e-417a-afb6-30e30f1c524b"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("88068a70-bb31-45e6-af87-1fd55362d3bc"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("c456f316-f9c9-4d67-ac6b-2a0e232b4568"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("cb46b933-f260-4b9a-b47a-32828c42b04d"));

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
    }
}
