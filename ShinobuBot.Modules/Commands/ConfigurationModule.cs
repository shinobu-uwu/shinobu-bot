using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using ShinobuBot.Models;
using ShinobuBot.Modules.Database;

namespace ShinobuBot.Modules.Commands
{
    public class ConfigurationModule : ModuleBase<SocketCommandContext>
    {
        private readonly BotDbContext _dbContext;

        public ConfigurationModule(BotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Command("prefixset")]
        [Summary("Sets the bot prefix for this server")]
        [Remarks("Administrator powers are needed to use this command.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task PrefixSet([Name("Prefix")]string prefix)
        {
            var config = _dbContext.Configurations.SingleOrDefault(c => c.GuildId == Context.Guild.Id);

            if (config is null)
                _dbContext.Add(new ServerConfiguration(Context.Guild.Id, prefix));
            else
                config.Prefix = prefix;

            await _dbContext.SaveChangesAsync();

            await ReplyAsync("Prefix set!");
        }
    }
}