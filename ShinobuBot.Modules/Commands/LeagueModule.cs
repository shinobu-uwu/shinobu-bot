using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MingweiSamuel.Camille;
using MingweiSamuel.Camille.Enums;

namespace ShinobuBot.Modules.Commands
{
    public class LeagueModule : ModuleBase<SocketCommandContext>
    {
        private readonly RiotApi _api;
        
        private const int TopChampionsDisplayed = 3;

        public LeagueModule()
        {
            _api = RiotApi.NewInstance(Environment.GetEnvironmentVariable("RIOT_API_TOKEN"));
            
        }

        [Command("league")]
        [Summary("Information about league summoner")]
        [Remarks(@"- If the username contains space use quotes around it. 
                       - Pass no parameters to search for the summoner name set to your discord ID.")]
        public async Task League([Name("Summoner name")] string name, [Name("Region")] string region)
        {
            var summoner = _api.SummonerV4.GetBySummonerName(Region.Get(region), name);

            var embedBuilder = new EmbedBuilder()
                .WithCurrentTimestamp()
                .WithColor(Color.Red)
                .WithTitle($"{summoner.Name}'s profile");

            var topChampions = await _api.ChampionMasteryV4.GetAllChampionMasteriesAsync(
                Region.Get(region), summoner.Id);
            var formattedChampions = "";
            for (int i = 0; i < TopChampionsDisplayed; i++)
            {
                var champion = (Champion) topChampions[i].ChampionId;
                formattedChampions += $"{champion.Name()} - {topChampions[i].ChampionPoints: 0}\n";
            }

            embedBuilder.AddField("Top Champions", formattedChampions);

            await ReplyAsync(embed: embedBuilder.Build());
            
        }
    }
}