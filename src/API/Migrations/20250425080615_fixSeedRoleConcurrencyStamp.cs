using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameService.Migrations
{
    /// <inheritdoc />
    public partial class fixSeedRoleConcurrencyStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "1",
                column: "concurrency_stamp",
                value: "a54bf09c-00c0-44f1-80d8-ad1ea061baaf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "2",
                column: "concurrency_stamp",
                value: "fb913210-693b-43e0-af4d-e67ea8a688f8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "3",
                column: "concurrency_stamp",
                value: "5927da9b-a345-4c2f-9b0c-cc5f12cbcd41");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "1",
                column: "concurrency_stamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "2",
                column: "concurrency_stamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "id",
                keyValue: "3",
                column: "concurrency_stamp",
                value: null);
        }
    }
}
