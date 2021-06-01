using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

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
        public async Task Help()
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

        [Command("help")]
        public async Task Help(string commandName)
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
                    .AddField("Description", command.Summary);
                await ReplyAsync(embed: embedBuilder.Build());
            }
            
        }
    }
}
