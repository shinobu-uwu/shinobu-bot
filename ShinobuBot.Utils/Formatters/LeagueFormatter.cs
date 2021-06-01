using System;
using MingweiSamuel.Camille.ChampionMasteryV4;
using MingweiSamuel.Camille.Enums;
using MingweiSamuel.Camille.SummonerV4;

namespace ShinobuBot.Utils.Formatters
{
    public static class LeagueFormatter
    {
        private const int ChampionsDisplayed = 3;
        public static string FormatMasteries(ChampionMastery[] champions)
        {
            var formatted = "";
            for (int i = 0; i < ChampionsDisplayed; i++)
            {
                var champ = (Champion) champions[i].ChampionId;
                formatted += $"{champ.Name()} - {Math.Round(champions[i].ChampionPoints / 1000.0)}K\n";
            }

            return formatted;
        }
    }
}