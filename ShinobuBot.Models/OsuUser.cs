using System.ComponentModel.DataAnnotations;

namespace ShinobuBot.Models
{
    public class OsuUser
    {
        public ulong DiscordId { get; set; }
        public string OsuUsername { get; set; }
        public int DefaultGameMode { get; set; }

        public OsuUser(ulong discordId, string osuUsername)
        {
            DiscordId = discordId;
            OsuUsername = osuUsername;
            DefaultGameMode = 0;
        }
        
        public OsuUser(ulong discordId, string osuUsername, int defaultGameMode)
        {
            DiscordId = discordId;
            OsuUsername = osuUsername;
            DefaultGameMode = defaultGameMode;
        }
    }
}