using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ShinobuBot.Utils.Embeds;

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
        [Summary("Sends the bot latency")]
        public async Task Ping()
            => await ReplyAsync(
                $"Pong in {Context.Client.Latency}ms");

        [Command("help")]
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
    }
}
