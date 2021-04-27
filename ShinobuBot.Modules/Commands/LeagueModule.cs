using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using RiotSharp;
using RiotSharp.Endpoints.StaticDataEndpoint.ProfileIcons;
using RiotSharp.Misc;

namespace ShinobuBot.Modules.Commands
{
    public class LeagueModule : ModuleBase<SocketCommandContext>
    {
        private readonly RiotApi _api = RiotApi.GetDevelopmentInstance(
            Environment.GetEnvironmentVariable("RIOT_API_TOKEN"));

        private readonly string _latestVersion;
        private readonly ProfileIconListStatic _icons;

        private const int TopChampionsDisplayed = 3;

        public LeagueModule()
        {
            _latestVersion = _api.DataDragon.Versions.GetAllAsync().Result[0];
            _icons = _api.DataDragon.ProfileIcons.GetAllAsync(_latestVersion).Result;
        }

        [Command("league")]
        public async Task League([Name("Username(s)")] params string[] names)
        {
            foreach (var name in names)
            {
                
                var summoner = await _api.Summoner.GetSummonerByNameAsync(Region.Br, name);

                var embedBuilder = new EmbedBuilder()
                    .WithCurrentTimestamp()
                    .WithColor(Color.Red)
                    .WithTitle($"{summoner.Name}'s profile")
                    .WithUrl($"https://br.op.gg");

                var topChampions = await _api.ChampionMastery.GetChampionMasteriesAsync(Region.Br, summoner.Id);
                var formattedChampions = "";
                for (int i = 0; i < TopChampionsDisplayed; i++)
                {
                    var champion = await _api.DataDragon.Champions.GetByIdAsync((int) topChampions[i].ChampionId, _latestVersion);
                    formattedChampions += $"{champion.Name} - {topChampions[i].ChampionPoints: 0}\n";
                }

                embedBuilder.AddField("Top Champion", formattedChampions);

                await ReplyAsync(embed: embedBuilder.Build());
            }
        }
    }
}