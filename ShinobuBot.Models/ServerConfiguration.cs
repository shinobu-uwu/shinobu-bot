namespace ShinobuBot.Models
{
    public class ServerConfiguration
    {
        public ulong GuildId { get; }
        public string Prefix { get; set; }

        public ServerConfiguration(ulong guildId, string prefix)
        {
            GuildId = guildId;
            Prefix = prefix;
        }
    }
}