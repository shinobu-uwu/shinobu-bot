using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using MingweiSamuel.Camille.Enums;

namespace ShinobuBot.Models
{
    /// <summary>
    /// League summoner name related to a discord user ID
    /// </summary>
    public class LeagueSummoner
    {
        public ulong DiscordId { get; set; }
        public string SummonerName { get; set; }
        public string Region { get; set; }

        public LeagueSummoner(ulong discordId, string summonerName, string region)
        {
            DiscordId = discordId;
            SummonerName = summonerName;
            Region = region;
        }
    }
}