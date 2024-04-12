using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RaptorProject.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    PlayerID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.PlayerID);
                });

            migrationBuilder.CreateTable(
                name: "Gallery",
                columns: table => new
                {
                    GalleryID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerID = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    GameID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerOneID = table.Column<int>(type: "integer", nullable: false),
                    PlayerTwoID = table.Column<int>(type: "integer", nullable: true),
                    CurrentPlayer = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.GameID);
                    table.ForeignKey(
                        name: "FK_Game_Player_PlayerOneID",
                        column: x => x.PlayerOneID,
                        principalTable: "Player",
                        principalColumn: "PlayerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Game_Player_PlayerTwoID",
                        column: x => x.PlayerTwoID,
                        principalTable: "Player",
                        principalColumn: "PlayerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Card",
                columns: table => new
                {
                    CardID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GalleryID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.CardID);
                    table.ForeignKey(
                        name: "FK_Card_Gallery_GalleryID",
                        column: x => x.GalleryID,
                        principalTable: "Gallery",
                        principalColumn: "GalleryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Board",
                columns: table => new
                {
                    BoardID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerID = table.Column<int>(type: "integer", nullable: false),
                    GameID = table.Column<int>(type: "integer", nullable: false),
                    ChosenCardID = table.Column<int>(type: "integer", nullable: false),
                    PlayersLeft = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.BoardID);
                    table.ForeignKey(
                        name: "FK_Board_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "GameID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Board_Player_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Player",
                        principalColumn: "PlayerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoardCard",
                columns: table => new
                {
                    BoardCardID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BoardID = table.Column<int>(type: "integer", nullable: false),
                    CardID = table.Column<int>(type: "integer", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardCard", x => x.BoardCardID);
                    table.ForeignKey(
                        name: "FK_BoardCard_Board_BoardID",
                        column: x => x.BoardID,
                        principalTable: "Board",
                        principalColumn: "BoardID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardCard_Card_CardID",
                        column: x => x.CardID,
                        principalTable: "Card",
                        principalColumn: "CardID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameID = table.Column<int>(type: "integer", nullable: false),
                    PlayerID = table.Column<int>(type: "integer", nullable: false),
                    MessageText = table.Column<string>(type: "text", nullable: false),
                    BoardID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_Message_Board_BoardID",
                        column: x => x.BoardID,
                        principalTable: "Board",
                        principalColumn: "BoardID");
                    table.ForeignKey(
                        name: "FK_Message_Game_GameID",
                        column: x => x.GameID,
                        principalTable: "Game",
                        principalColumn: "GameID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_Player_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Player",
                        principalColumn: "PlayerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Board_ChosenCardID",
                table: "Board",
                column: "ChosenCardID");

            migrationBuilder.CreateIndex(
                name: "IX_Board_GameID",
                table: "Board",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Board_PlayerID",
                table: "Board",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_BoardCard_BoardID",
                table: "BoardCard",
                column: "BoardID");

            migrationBuilder.CreateIndex(
                name: "IX_BoardCard_CardID",
                table: "BoardCard",
                column: "CardID");

            migrationBuilder.CreateIndex(
                name: "IX_Card_GalleryID",
                table: "Card",
                column: "GalleryID");

            migrationBuilder.CreateIndex(
                name: "IX_Gallery_PlayerID",
                table: "Gallery",
                column: "PlayerID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_PlayerOneID",
                table: "Game",
                column: "PlayerOneID");

            migrationBuilder.CreateIndex(
                name: "IX_Game_PlayerTwoID",
                table: "Game",
                column: "PlayerTwoID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_BoardID",
                table: "Message",
                column: "BoardID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_GameID",
                table: "Message",
                column: "GameID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_PlayerID",
                table: "Message",
                column: "PlayerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_BoardCard_ChosenCardID",
                table: "Board",
                column: "ChosenCardID",
                principalTable: "BoardCard",
                principalColumn: "BoardCardID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_BoardCard_ChosenCardID",
                table: "Board");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "BoardCard");

            migrationBuilder.DropTable(
                name: "Board");

            migrationBuilder.DropTable(
                name: "Card");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Gallery");

            migrationBuilder.DropTable(
                name: "Player");
        }
    }
}
