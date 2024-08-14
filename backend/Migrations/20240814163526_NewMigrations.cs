using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaptorProject.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPlayer",
                table: "Game");

            migrationBuilder.AlterColumn<int>(
                name: "ChosenCardID",
                table: "Board",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentPlayer",
                table: "Game",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ChosenCardID",
                table: "Board",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
