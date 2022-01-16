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
        [Remarks(@"- If the username contains space(s) surround it with quotes;
                       - The default game mode is Std;
                       - Pass no parameters to search for the username set to your discord ID with your default game mode;
                       - Game modes: 0: Std, 1: Taiko, 2: Ctb, 3: Mania;")]
        public async Task OsuUser(
            [Name("Username")] string name = "",
            [Name("Game mode")] int? gameMode = null
        )
        {
            if (name == "")
            {
                var query = _context.OsuUsers.FirstOrDefault(u => u.DiscordId == Context.User.Id);
                
                if (query is null)
                {
                    await ReplyAsync("User not registered");
                    return;
                }
                
                gameMode ??= query.DefaultGameMode;
                name = query.OsuUsername;
            }
            
            gameMode ??= 0;
            var user = await _client.GetUserByUsernameAsync(name, (GameMode) gameMode);

            await ReplyAsync(embed: EmbedFactory.OsuProfile(user));
        }

        [Command("osuset")]
        [Summary("Register an osu! account to your discord ID")]
        [Alias("os")]
        [Remarks(@"- Only one account per discord user is allowed;
                       - The default game mode is Std;
                       - Using this command with an username already set will replace it;
                       - Gamemodes: 0: Std, 1: Taiko, 2: Ctb, 3: Mania;")]
        public async Task OsuSet(
            [Name("Username")] string username, 
            [Name("Default game mode")] int gameMode = 0
        )
        {
            var user = _context.OsuUsers.SingleOrDefault(u => u.DiscordId == Context.User.Id);
            if (user is null)
            {
                _context.Add(new OsuUser(Context.User.Id, username, gameMode));
                await _context.SaveChangesAsync();
                
                await ReplyAsync("User registered!");

                return;
            }
            user.OsuUsername = username;
            user.DefaultGameMode = gameMode;
            await _context.SaveChangesAsync();
            
            await ReplyAsync("User updated!");
        }
    }
}
