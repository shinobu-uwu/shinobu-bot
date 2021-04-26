using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using ShinobuBot.Utils.Embeds;

namespace ShinobuBot.Modules.Commands
{
    public class UtilsModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands; 
        
        public UtilsModule(CommandService commands)
        {
            _commands = commands;
        }
        
        [Command("ping")]
        [Summary("Sends the bot latency")]
        public async Task Ping()
            => await ReplyAsync(
                $"Pong in {Context.Client.Latency}ms");

        [Command("help")]
        public async Task Help()
        {
            await ReplyAsync(embed: HelpEmbed.Build(_commands.Modules));
        }
    }
}
