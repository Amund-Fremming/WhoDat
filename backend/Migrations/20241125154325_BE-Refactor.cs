using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaptorProject.Migrations
{
    /// <inheritdoc />
    public partial class BERefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Board_BoardID",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Player",
                newName: "PlayerRole");

            migrationBuilder.RenameColumn(
                name: "PlayerID",
                table: "Player",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "BoardID",
                table: "Message",
                newName: "BoardEntityID");

            migrationBuilder.RenameColumn(
                name: "MessageID",
                table: "Message",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Message_BoardID",
                table: "Message",
                newName: "IX_Message_BoardEntityID");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Game",
                newName: "GameState");

            migrationBuilder.RenameColumn(
                name: "GameID",
                table: "Game",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "CardID",
                table: "Card",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "BoardCardID",
                table: "BoardCard",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "BoardID",
                table: "Board",
                newName: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Board_BoardEntityID",
                table: "Message",
                column: "BoardEntityID",
                principalTable: "Board",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Board_BoardEntityID",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "PlayerRole",
                table: "Player",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Player",
                newName: "PlayerID");

            migrationBuilder.RenameColumn(
                name: "BoardEntityID",
                table: "Message",
                newName: "BoardID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Message",
                newName: "MessageID");

            migrationBuilder.RenameIndex(
                name: "IX_Message_BoardEntityID",
                table: "Message",
                newName: "IX_Message_BoardID");

            migrationBuilder.RenameColumn(
                name: "GameState",
                table: "Game",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Game",
                newName: "GameID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Card",
                newName: "CardID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "BoardCard",
                newName: "BoardCardID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Board",
                newName: "BoardID");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Board_BoardID",
                table: "Message",
                column: "BoardID",
                principalTable: "Board",
                principalColumn: "BoardID");
        }
    }
}
