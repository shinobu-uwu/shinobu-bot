// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShinobuBot.Modules.Database;

namespace ShinobuBot.Modules.Migrations
{
    [DbContext(typeof(BotDbContext))]
    [Migration("20210601194401_LeagueDiscordAsPrimaryKey")]
    partial class LeagueDiscordAsPrimaryKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0-preview.3.21201.2");

            modelBuilder.Entity("ShinobuBot.Models.LeagueSummoner", b =>
                {
                    b.Property<ulong>("DiscordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Region")
                        .HasColumnType("TEXT");

                    b.Property<string>("SummonerName")
                        .HasColumnType("TEXT");

                    b.HasKey("DiscordId");

                    b.ToTable("LeagueSummoners");
                });

            modelBuilder.Entity("ShinobuBot.Models.OsuUser", b =>
                {
                    b.Property<ulong>("DiscordId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DefaultGameMode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OsuUsername")
                        .HasColumnType("TEXT");

                    b.HasKey("DiscordId");

                    b.ToTable("OsuUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
