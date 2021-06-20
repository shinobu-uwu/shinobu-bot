using Discord;
using Discord.Commands;

namespace ShinobuBot.Utils
{
    public static partial class EmbedFactory
    {
        public static Embed About(SocketCommandContext context)
            => new EmbedBuilder()
                .WithColor(0xf4f16d)
                .WithCurrentTimestamp()
                .WithThumbnailUrl(context.Client.CurrentUser.GetAvatarUrl())
                .WithTitle("About myself")
                .AddField("Creator", "Shinobu#9452")
                .AddField("Version", "Alpha")
                .AddField("Severs joined", context.Client.Guilds.Count)
                .Build();
    }
}