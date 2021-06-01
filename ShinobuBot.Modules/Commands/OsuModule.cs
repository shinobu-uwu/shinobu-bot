using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using OsuSharp;

namespace ShinobuBot.Modules.Commands
{
    public class OsuModule : ModuleBase<SocketCommandContext>
    {
        private readonly OsuClient _client;

        public OsuModule()
        {
            var config = new OsuSharpConfiguration
            {
                ApiKey = Environment.GetEnvironmentVariable("OSU_API_TOKEN")
            };
            _client = new OsuClient(config);
        }

        // TODO Implement gamemode
        [Command("osu")]
        [Summary("Information about osu! user(s).")]
        [Remarks(@"- If the username contains space use quotes around it. 
                       - You can search for multiple users separating the usernames by space.
                       - Pass no parameters to search for the username set to your discord ID.")]
        public async Task OsuUser([Name("Username(s)")] params string[] names)
        {
            foreach (var name in names)
            {
                var user = await _client.GetUserByUsernameAsync(name, GameMode.Standard);

                var embed = new EmbedBuilder()
                    .WithColor(0xE5649D)
                    .WithCurrentTimestamp()
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
    }
}