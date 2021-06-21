using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using MingweiSamuel.Camille;
using MingweiSamuel.Camille.Enums;
using ShinobuBot.Models;
using ShinobuBot.Modules.Database;
using ShinobuBot.Utils;

namespace ShinobuBot.Modules.Commands
{
    public class LeagueModule : ModuleBase<SocketCommandContext>
    {
        private readonly RiotApi _api;
        private readonly BotDbContext _context;
        
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
                    var topChampions = await _api.ChampionMasteryV4.GetAllChampionMasteriesAsync(
                        Region.Get(query.Region), summoner.Id);

                    await ReplyAsync(embed: EmbedFactory.LeagueProfile(summoner, topChampions));
                }
            }
            else
            {
                var summoner = _api.SummonerV4.GetBySummonerName(Region.Get(region), name);
                var topChampions = await _api.ChampionMasteryV4.GetAllChampionMasteriesAsync(
                    Region.Get(region), summoner.Id);

                await ReplyAsync(embed: EmbedFactory.LeagueProfile(summoner, topChampions));
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

        [Command("leaguehistory")]
        [Summary("The last 10 games of a League of Legends summoner.")]
        public async Task LeagueHistory([Name("Summoner name")]string summonerName = "", [Name("Region")]string region = "")
        {
            var summonerRegion = Region.Get(region);
            var summoner = await _api.SummonerV4.GetBySummonerNameAsync(summonerRegion, summonerName);
            if (summoner is null)
            {
                await ReplyAsync("User not found");
                return;
            }
            var matchList = await _api.MatchV4.GetMatchlistAsync(
                summonerRegion,
                summoner.AccountId,
                endIndex: 10);
            var matchTasks = matchList.Matches.Select(
                matchData => _api.MatchV4.GetMatchAsync(summonerRegion, matchData.GameId)
            ).ToArray();
            var history = await Task.WhenAll(matchTasks);

            await ReplyAsync(embed: EmbedFactory.LeagueHistory(summoner, history));
        }
    }
}