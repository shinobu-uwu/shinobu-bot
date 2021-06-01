using Microsoft.EntityFrameworkCore.Migrations;

namespace ShinobuBot.Modules.Migrations
{
    public partial class OsuDiscordAsPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OsuUsers",
                table: "OsuUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OsuUsers");

            migrationBuilder.AlterColumn<ulong>(
                name: "DiscordId",
                table: "OsuUsers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OsuUsers",
                table: "OsuUsers",
                column: "DiscordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OsuUsers",
                table: "OsuUsers");

            migrationBuilder.AlterColumn<ulong>(
                name: "DiscordId",
                table: "OsuUsers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(ulong),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OsuUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OsuUsers",
                table: "OsuUsers",
                column: "Id");
        }
    }
}
