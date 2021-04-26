using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using RiotSharp;
using RiotSharp.Endpoints.ChampionEndpoint;
using RiotSharp.Misc;

namespace ShinobuBot.Modules.Commands
{
    public class LeagueModule : ModuleBase<SocketCommandContext>
    {
        private readonly RiotApi _api = RiotApi.GetDevelopmentInstance(
            Environment.GetEnvironmentVariable("RIOT_API_TOKEN"));

        private string _latestVersion;
        private List<string> _icons;

        public LeagueModule()
        {
            _latestVersion = _api.DataDragon.Versions.GetAllAsync().Result[0];
        }

        [Command("league")]
        public async Task League([Name("Username(s)")] params string[] names)
        {
            foreach (var name in names)
            {
                var user = await _api.Summoner.GetSummonerByNameAsync(Region.Br, name);
                var masteries = await _api.ChampionMastery.GetChampionMasteriesAsync(Region.Br, name);
                var icons = await _api.DataDragon.ProfileIcons.GetAllAsync(_latestVersion);
                foreach (var item in icons.ProfileIcons.Keys)
                {
                    Console.WriteLine(item);
                }
                
                var embedBuilder = new EmbedBuilder()
                    .WithColor(Color.Red)
                    .WithCurrentTimestamp()
                    .WithTitle($"Summoner {user.Name}")
                    .AddField("Stats", $@"**Level:** {user.Level}
                                    ");
            }
        }

        [Command("reloadversion")]
        [RequireOwner]
        [Summary("Internal command, reloads the version of the game to the latest")]
        public async Task ReloadVersion()
        {
            var versions = await _api.DataDragon.Versions.GetAllAsync();
            _latestVersion = versions[0];
        }
    }
}