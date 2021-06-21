using System;
using System.Linq;
using MingweiSamuel.Camille.ChampionMasteryV4;
using MingweiSamuel.Camille.Enums;
using MingweiSamuel.Camille.MatchV4;
using MingweiSamuel.Camille.SummonerV4;

namespace ShinobuBot.Utils.Formatters
{
    public static class LeagueFormatter
    {
        private const int ChampionsDisplayed = 3;
        public const int MatchHistoryDisplayed = 10;

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

        public static string FormatHistory(Match[] matches, string accountId)
        {
            var formatted = "";
            for (int i = 0; i < MatchHistoryDisplayed; i++)
            {
                var data = matches[i];
                var summonerData = data.ParticipantIdentities.First(p => p.Player.AccountId == accountId);
                var summoner = data.Participants.First(p => p.ParticipantId == summonerData.ParticipantId);
                var win = summoner.Stats.Win ? "Win" : "Loss";
                formatted += $"{win} - {((Champion)summoner.ChampionId).Name()} ";
                formatted += $"{summoner.Stats.Kills}/{summoner.Stats.Deaths}/{summoner.Stats.Assists}\n";
            }

            return formatted;
        }
    }
}