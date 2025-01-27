using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class updateusersfk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tenants_TenantId",
                table: "AspNetUsers");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tenants_TenantId",
                table: "AspNetUsers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tenants_TenantId",
                table: "AspNetUsers");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tenants_TenantId",
                table: "AspNetUsers",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
