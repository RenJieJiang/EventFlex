using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class update_seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3cb683d0-e63e-4052-846d-dfb8d91eab37"), null, "User", "USER" },
                    { new Guid("d15b4362-ceb7-4e25-9899-8d0da9ee4afe"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("02e06226-a573-403e-ad08-8b8d7fb76f99"), "Description for Tenant 2", "Tenant 2" },
                    { new Guid("3eadc2db-a92a-479b-9dbc-681809f09f14"), "Description for Tenant 1", "Tenant 1" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("b51c1111-fd0a-45be-8a7c-440c24658c9f"), 0, "7052869c-8004-4598-8d63-7a4eba1dab8b", "jiangrj1@hotmail.com", true, false, null, "admin", "JIANGRJ1@HOTMAIL.COM", "JIANGRJ1@HOTMAIL.COM", "AQAAAAIAAYagAAAAEHRJ5KhmDJv4FF8+6h9ZVYrb+nh6cht3Btk/qyoobotpsEqF4E/nuX8pMVKJ84vBNQ==", null, false, "", new Guid("3eadc2db-a92a-479b-9dbc-681809f09f14"), false, "jiangrj1@hotmail.com" },
                    { new Guid("d4611abd-882c-4171-81e7-e6589f77034b"), 0, "9739c4eb-c1f8-48ff-9efc-019584594761", "user2@example.com", true, false, null, "user2", "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", "AQAAAAIAAYagAAAAECyXiLXQamEb6tcWX0ztsrKo1XC6vwYvm31zvIVfm1cpARzRm/v8XThyouXSPMsLfA==", null, false, "", new Guid("02e06226-a573-403e-ad08-8b8d7fb76f99"), false, "user2@example.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3cb683d0-e63e-4052-846d-dfb8d91eab37"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d15b4362-ceb7-4e25-9899-8d0da9ee4afe"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b51c1111-fd0a-45be-8a7c-440c24658c9f"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d4611abd-882c-4171-81e7-e6589f77034b"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("02e06226-a573-403e-ad08-8b8d7fb76f99"));

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "Id",
                keyValue: new Guid("3eadc2db-a92a-479b-9dbc-681809f09f14"));

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
    }
}
