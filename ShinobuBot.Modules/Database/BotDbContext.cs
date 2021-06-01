using System;
using Microsoft.EntityFrameworkCore;
using ShinobuBot.Models;

namespace ShinobuBot.Modules.Database
{
    public class BotDbContext : DbContext
    {
        public DbSet<OsuUser> OsuUsers { get; set; }
        public DbSet<LeagueSummoner> LeagueSummoners { get; set; }

        public BotDbContext()
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}/bot.db");
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OsuUser>().HasKey(u => u.DiscordId);
            modelBuilder.Entity<LeagueSummoner>().HasKey(u => u.DiscordId);

            base.OnModelCreating(modelBuilder);
        }
    }
}