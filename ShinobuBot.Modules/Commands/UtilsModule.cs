using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ShinobuBot.Utils.Formatters;

namespace ShinobuBot.Modules.Commands
{
    public class UtilsModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        
        public UtilsModule(CommandService commands, DiscordSocketClient client)
        {
            _commands = commands;
            _client = client;
        }
        
        [Command("ping")]
        [Summary("Pong! The bot's latency")]
        public async Task Ping()
            => await ReplyAsync(
                $"Pong in {Context.Client.Latency}ms");

        [Command("help")]
        [Summary("What do you think")]
        [Remarks("If no parameter is specified will list all available commands")]
        public async Task Help([Name("(Optional) Command name")] string commandName = "")
        {
            if (commandName == "")
            {
                await ReplyAsync(embed: EmbedFactory.ListCommands(_commands.Modules));   
                return;
            }
            
            var command = _commands.Commands.FirstOrDefault(c => c.Name == commandName);
            
            if (command is null)
            {
                await ReplyAsync("Command not found");
                return;
            }
            
            await ReplyAsync(embed: EmbedFactory.CommandInfo(command));
        }
    }
}
