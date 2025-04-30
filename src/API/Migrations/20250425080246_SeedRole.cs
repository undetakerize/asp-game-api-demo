using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameService.Migrations
{
    /// <inheritdoc />
    public partial class SeedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "id", "concurrency_stamp", "name", "normalized_name" },
                values: new object[,]
                {
                    { "1e54f035-8f24-457a-89b2-276a032f2a15", null, "Guest", "GUEST" },
                    { "22106dcb-5e7d-4891-9215-ba3ed8703e43", null, "Admin", "ADMIN" },
                    { "6198a44c-41a7-4f2a-a55f-567e92382114", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "1e54f035-8f24-457a-89b2-276a032f2a15");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "22106dcb-5e7d-4891-9215-ba3ed8703e43");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "6198a44c-41a7-4f2a-a55f-567e92382114");
        }
    }
}
