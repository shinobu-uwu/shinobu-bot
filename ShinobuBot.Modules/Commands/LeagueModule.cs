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
        public async Task League([Name("Username(s)")] params string[] names)
        {
            foreach (var name in names)
            {

                var summoner = _api.SummonerV4.GetBySummonerName(Region.BR, name);

                var embedBuilder = new EmbedBuilder()
                    .WithCurrentTimestamp()
                    .WithColor(Color.Red)
                    .WithTitle($"{summoner.Name}'s profile")
                    .WithUrl($"https://br.op.gg");

                var topChampions = await _api.ChampionMasteryV4.GetAllChampionMasteriesAsync(Region.BR, summoner.Id);
                var formattedChampions = "";
                for (int i = 0; i < TopChampionsDisplayed; i++)
                {
                    var champion = (Champion) topChampions[i].ChampionId;
                    formattedChampions += $"{champion.Name()} - {topChampions[i].ChampionPoints: 0}\n";
                }

                embedBuilder.AddField("Top Champion", formattedChampions);

                await ReplyAsync(embed: embedBuilder.Build());
            }
        }
    }
}