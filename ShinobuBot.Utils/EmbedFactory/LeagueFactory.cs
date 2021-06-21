using Discord;
using MingweiSamuel.Camille.ChampionMasteryV4;
using MingweiSamuel.Camille.MatchV4;
using MingweiSamuel.Camille.SummonerV4;
using ShinobuBot.Utils.Formatters;

namespace ShinobuBot.Utils
{
    public static partial class EmbedFactory
    {
        public static Embed LeagueProfile(Summoner summoner, ChampionMastery[] champions)
            => new EmbedBuilder()
                .WithCurrentTimestamp()
                .WithColor(Color.Red)
                .WithTitle($"{summoner.Name}'s profile")
                .AddField("Top Champions", LeagueFormatter.FormatMasteries(champions))
                .Build();

        public static Embed LeagueHistory(Summoner summoner, Match[] matches)
            => new EmbedBuilder()
                .WithCurrentTimestamp()
                .WithColor(Color.Red)
                .WithTitle($"{summoner.Name}'s last 10 games")
                .AddField("\0", LeagueFormatter.FormatHistory(matches, summoner.AccountId))
                .Build();
        
    }
}