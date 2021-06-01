using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace ShinobuBot.Modules.Commands
{
    public class GeneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("avatar")]
        [Summary("Sends the user avatar")]
        public async Task Avatar()
        {
            var avatar = Context.User.GetAvatarUrl();
            var embed = new EmbedBuilder()
                .WithTitle($"{Context.User.Username}#{Context.User.Discriminator}'s avatar")
                .WithColor(new Color(0xFCFDA5))
                .WithImageUrl(avatar)
                .Build();
            
            await Context.Channel.SendMessageAsync(embed: embed);
        }

        [Command("about")]
        [Summary("Information about the bot")]
        public async Task BotInfo()
        {
            var bot = Context.Client.CurrentUser;
            var embed = new EmbedBuilder()
                .WithTitle("About myself!")
                .WithThumbnailUrl(bot.GetAvatarUrl())
                .AddField("Creator", "Shinobu#9452")
                .AddField("Version", "Alpha")
                .AddField("Servers joined", Context.Client.Guilds.Count)
                .WithColor(0xf4f16d)
                .Build();

            await Context.Channel.SendMessageAsync(embed: embed);
        }
    }
}
