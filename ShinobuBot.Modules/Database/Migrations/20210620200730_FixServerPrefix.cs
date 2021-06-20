using Microsoft.EntityFrameworkCore.Migrations;

namespace ShinobuBot.Modules.Migrations
{
    public partial class FixServerPrefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "Configurations",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "Configurations");
        }
    }
}
