using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RaptorProject.Migrations
{
    /// <inheritdoc />
    public partial class RmGallery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_Gallery_GalleryID",
                table: "Card");

            migrationBuilder.DropTable(
                name: "Gallery");

            migrationBuilder.RenameColumn(
                name: "GalleryID",
                table: "Card",
                newName: "PlayerID");

            migrationBuilder.RenameIndex(
                name: "IX_Card_GalleryID",
                table: "Card",
                newName: "IX_Card_PlayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Player_PlayerID",
                table: "Card",
                column: "PlayerID",
                principalTable: "Player",
                principalColumn: "PlayerID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Card_Player_PlayerID",
                table: "Card");

            migrationBuilder.RenameColumn(
                name: "PlayerID",
                table: "Card",
                newName: "GalleryID");

            migrationBuilder.RenameIndex(
                name: "IX_Card_PlayerID",
                table: "Card",
                newName: "IX_Card_GalleryID");

            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    GalleryID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gallery", x => x.GalleryID);
                    table.ForeignKey(
                        name: "FK_Gallery_Player_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Player",
                        principalColumn: "PlayerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gallery_PlayerID",
                table: "Gallery",
                column: "PlayerID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Card_Gallery_GalleryID",
                table: "Card",
                column: "GalleryID",
                principalTable: "Gallery",
                principalColumn: "GalleryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
