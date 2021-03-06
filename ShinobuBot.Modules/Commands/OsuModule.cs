using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using OsuSharp;
using ShinobuBot.Models;
using ShinobuBot.Modules.Database;
using ShinobuBot.Utils;

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
        [Alias("o")]
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
                    
                    await ReplyAsync(embed: EmbedFactory.OsuProfile(user));   
                }
            }
            else
            {
                gamemode ??= 0;
                var user = await _client.GetUserByUsernameAsync(name, (GameMode) gamemode);

                await ReplyAsync(embed: EmbedFactory.OsuProfile(user));
            }
        }

        [Command("osuset")]
        [Summary("Register an osu! account to your discord ID")]
        [Alias("os")]
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