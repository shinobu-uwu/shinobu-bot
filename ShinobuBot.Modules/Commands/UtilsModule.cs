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
        public async Task Help([Name("(Optional) Command name")]string commandName = "")
        {
            if (commandName != "")
            {
                var command = _commands.Commands.Where(c => c.Name == commandName).FirstOrDefault();
                if (command is null)
                {
                    await ReplyAsync("Command not found");
                }
                else
                {
                    var embedBuilder = new EmbedBuilder()
                        .WithCurrentTimestamp()
                        .WithColor(Color.Gold)
                        .AddField("Name", command.Name)
                        .AddField("Description", command.Summary)
                        .AddField("Parameters", command.Parameters.Count == 0 
                            ? "None" : CommandsFormatter.FormatParameters(command.Parameters))
                        .AddField("Remarks", command.Remarks == "" ? "None" : command.Remarks);
                    await ReplyAsync(embed: embedBuilder.Build());
                }
            }
            else
            {
                var embedBuilder = new EmbedBuilder()
                    .WithCurrentTimestamp()
                    .WithColor(Color.Gold)
                    .WithThumbnailUrl(_client.CurrentUser.GetAvatarUrl());
                foreach (var module in _commands.Modules)
                {
                    var commands = "";
                    foreach (var command in module.Commands)
                        commands += $"`{command.Name}` ";
                
                    embedBuilder.AddField(module.Name, commands);
                }
            
                await ReplyAsync(embed: embedBuilder.Build());   
            }
        }
    }
}
