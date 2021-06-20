using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using ShinobuBot.Utils;

namespace ShinobuBot.Modules.Commands
{
    public class GeneralModule : ModuleBase<SocketCommandContext>
    {
        [Command("avatar")]
        [Summary("Sends the user avatar")]
        public async Task Avatar()
            => await Context.Channel.SendMessageAsync(embed: EmbedFactory.Avatar(Context));
        

        [Command("about")]
        [Summary("Information about the bot")]
        public async Task BotInfo()
            => await Context.Channel.SendMessageAsync(embed: EmbedFactory.About(Context));
    }
}
