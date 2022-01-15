namespace ShinobuBot.Models
{
    public class ServerConfiguration
    {
        public ulong GuildId { get; set; }
        public string Prefix { get; set; }

        public ServerConfiguration(ulong guildId, string prefix)
        {
            GuildId = guildId;
            Prefix = prefix;
        }
    }
}