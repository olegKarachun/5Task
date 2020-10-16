using Microsoft.EntityFrameworkCore.Migrations;

namespace _5Task.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Player1",
                table: "Games",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player2",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Player1",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Player2",
                table: "Games");
        }
    }
}
