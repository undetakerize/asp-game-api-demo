using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameService.Migrations
{
    /// <inheritdoc />
    public partial class AddGameReviewJoinTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_review_game_game_id",
                table: "review");

            migrationBuilder.DropIndex(
                name: "ix_review_game_id",
                table: "review");

            migrationBuilder.DropColumn(
                name: "game_id",
                table: "review");

            migrationBuilder.CreateTable(
                name: "game_review",
                columns: table => new
                {
                    game_id = table.Column<int>(type: "integer", nullable: false),
                    review_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_review", x => new { x.game_id, x.review_id });
                    table.ForeignKey(
                        name: "fk_game_review_game_game_id",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_game_review_review_review_id",
                        column: x => x.review_id,
                        principalTable: "review",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_game_review_review_id",
                table: "game_review",
                column: "review_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_review");

            migrationBuilder.AddColumn<int>(
                name: "game_id",
                table: "review",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_review_game_id",
                table: "review",
                column: "game_id");

            migrationBuilder.AddForeignKey(
                name: "fk_review_game_game_id",
                table: "review",
                column: "game_id",
                principalTable: "game",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
