using Microsoft.EntityFrameworkCore.Migrations;

namespace ShinobuBot.Modules.Migrations
{
    public partial class LeagueDiscordAsPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "LeagueSummoners");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LeagueSummoners",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
