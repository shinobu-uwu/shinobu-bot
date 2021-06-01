using Discord;
using MingweiSamuel.Camille.ChampionMasteryV4;
using MingweiSamuel.Camille.SummonerV4;
using OsuSharp;
using ShinobuBot.Utils.Formatters;

namespace ShinobuBot.Utils
{
    public static class EmbedFactory
    {
        public static Embed LeagueProfile(Summoner summoner, ChampionMastery[] champions)
            => new EmbedBuilder()
            .WithCurrentTimestamp()
            .WithColor(Color.Red)
            .WithTitle($"{summoner.Name}'s profile")
            .AddField("Top Champions", LeagueFormatter.FormatMasteries(champions))
            .Build();
        
        public static Embed OsuProfile(User user)
            => new EmbedBuilder()
                .WithColor(0xE5649D)
                .WithCurrentTimestamp()
                .WithThumbnailUrl($"http://s.ppy.sh/a/{user.UserId}")
                .WithTitle($"{user.Username}'s profile")
                .WithUrl($"https://osu.ppy.sh/users/{user.UserId}")
                .AddField($"\0",
                    $@"**Rank:** #{user.Rank} (#{user.CountryRank})
                            **Country:** :flag_{user.Country.Name.ToLower()}:
                            **PP:** {user.PerformancePoints: 0}
                            **Acc:** {user.Accuracy: .00}%
                            **Level:** {user.Level: 0}")
                .Build();
    }
}