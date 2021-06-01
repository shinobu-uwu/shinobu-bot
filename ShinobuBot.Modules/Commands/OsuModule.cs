using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using OsuSharp;
using ShinobuBot.Models;
using ShinobuBot.Modules.Database;

namespace ShinobuBot.Modules.Commands
{
    public class OsuModule : ModuleBase<SocketCommandContext>
    {
        private readonly OsuClient _client;
        private readonly BotDbContext _context;

        public OsuModule(BotDbContext dbContext)
        {
            var config = new OsuSharpConfiguration
            {
                ApiKey = Environment.GetEnvironmentVariable("OSU_API_TOKEN")
            };
            _client = new OsuClient(config);
            _context = dbContext;
        }
        
        [Command("osu")]
        [Summary("Information about osu! user(s).")]
        [Remarks(@"- If the username contains space use quotes around it. 
                       - Pass no parameters to search for the username set to your discord ID with your default Gamemode.
                       - Gamemodes: 0: Std, 1: Taiko, 2: Ctb, 3: Mania")]
        public async Task OsuUser([Name("Username")] string name = "",
            [Name("Gamemode (std if not specified)")]int? gamemode = null)
        {
            if ("".Equals(name))
            {
                var query = _context.OsuUsers.FirstOrDefault(u => u.DiscordId == Context.User.Id);
                if (query is null)
                    await ReplyAsync("User not registered");
                else
                {
                    gamemode ??= query.DefaultGameMode;
                    var user = await _client.GetUserByUsernameAsync(query.OsuUsername, (GameMode) gamemode);

                    var embed = new EmbedBuilder()
                        .WithColor(0xE5649D)
                        .WithCurrentTimestamp()
                        .WithThumbnailUrl($"http://s.ppy.sh/a/{user.UserId}")
                        .WithTitle($"{user.Username}'s profile")
                        .WithUrl($"https://osu.ppy.sh/users/{user.UserId}")
                        .AddField($"\0",
                            $@"**Rank:** #{user.Rank} -  #{user.CountryRank} :flag_{user.Country.Name.ToLower()}:
                            **PP:** {user.PerformancePoints: 0}
                            **Acc:** {user.Accuracy: .00}%
                            **Level:** {user.Level: 0}")
                        .Build();
                
                    await ReplyAsync(embed: embed);   
                }
            }
            else
            {
                gamemode ??= 0;
                var user = await _client.GetUserByUsernameAsync(name, (GameMode) gamemode);

                var embed = new EmbedBuilder()
                    .WithColor(0xE5649D).WithCurrentTimestamp()
                    .WithThumbnailUrl($"http://s.ppy.sh/a/{user.UserId}")
                    .WithTitle($"{user.Username}'s profile")
                    .WithUrl($"https://osu.ppy.sh/users/{user.UserId}")
                    .AddField($"Stats", 
                        $@"**Rank:** #{user.Rank} -  #{user.CountryRank} :flag_{user.Country.Name.ToLower()}:
                                **PP:** {user.PerformancePoints: 0}
                                **Acc:** {user.Accuracy: .00}%
                                **Level:** {user.Level: 0}")
                    .Build();

                    await ReplyAsync(embed: embed);
            }
        }

        [Command("osuset")]
        [Summary("Register an osu! account to your discord ID")]
        [Remarks(@"- Only account per discord user is allowed.
                       - Using this command with an username already set will replace it.
                       - Gamemodes: 0: Std, 1: Taiko, 2: Ctb, 3: Mania")]
        public async Task OsuSet([Name("Username")] string username, 
            [Name("Default gamemode (std if not specified)")] int gameMode = 0)
        {
            var user = _context.OsuUsers.SingleOrDefault(u => u.DiscordId == Context.User.Id);
            if (user is null)
            {
                _context.Add(new OsuUser(Context.User.Id, username, gameMode));
                await ReplyAsync("User registered!");
            }
            else
            {
                user.OsuUsername = username;
                user.DefaultGameMode = gameMode;
                await ReplyAsync("User updated!");
            }
            
            await _context.SaveChangesAsync();
        }
    }
}