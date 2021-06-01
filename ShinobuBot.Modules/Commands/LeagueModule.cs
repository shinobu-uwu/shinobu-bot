using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MingweiSamuel.Camille;
using MingweiSamuel.Camille.Enums;
using ShinobuBot.Models;
using ShinobuBot.Modules.Database;

namespace ShinobuBot.Modules.Commands
{
    public class LeagueModule : ModuleBase<SocketCommandContext>
    {
        private readonly RiotApi _api;
        private readonly BotDbContext _context;
        
        private const int TopChampionsDisplayed = 3;

        public LeagueModule(BotDbContext botDbContext)
        {
            _api = RiotApi.NewInstance(Environment.GetEnvironmentVariable("RIOT_API_TOKEN"));
            _context = botDbContext;
        }

        [Command("league")]
        [Summary("Information about league summoner")]
        [Remarks(@"- If the username contains space use quotes around it. 
                       - Pass no parameters to search for the summoner set to your discord ID.")]
        public async Task League([Name("Summoner name")] string name = "", [Name("Region")] string region = "")
        {
            if ("".Equals(name))
            {
                var query = _context.LeagueSummoners.SingleOrDefault(u => u.DiscordId == Context.User.Id);
                if (query is null)
                    await ReplyAsync("User not registered");
                else
                {
                    var summoner = _api.SummonerV4.GetBySummonerName(Region.Get(query.Region), query.SummonerName);

                    var embedBuilder = new EmbedBuilder()
                        .WithCurrentTimestamp()
                        .WithColor(Color.Red)
                        .WithTitle($"{summoner.Name}'s profile");

                    var topChampions = await _api.ChampionMasteryV4.GetAllChampionMasteriesAsync(
                        Region.Get(query.Region), summoner.Id);
                    
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
            else
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

        [Command("leagueset")]
        [Summary("Register a League of Legends summoner to your discord ID")]
        [Remarks("Unlike osuset, this command doesn't set a default region, you must specify it.")]
        public async Task LeagueSet([Name("Username")] string summonerName, [Name("Region")] string region)
        {
            var user = _context.LeagueSummoners.SingleOrDefault(u => u.DiscordId == Context.User.Id);
            if (user is null)
            {
                _context.Add(new LeagueSummoner(Context.User.Id, summonerName, region));
                await ReplyAsync("User registered!");
            }
            else
            {
                user.SummonerName = summonerName;
                user.Region = region;
                await ReplyAsync("User updated!");
            }
            
            await _context.SaveChangesAsync();
        }
    }
}