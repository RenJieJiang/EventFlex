using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class remove_seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
