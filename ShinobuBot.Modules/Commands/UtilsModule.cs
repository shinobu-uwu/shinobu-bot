using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

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
                $"{Context.Client.Latency}ms");

        [Command("help")]
        public async Task Help()
        {
            var embedBuilder = new EmbedBuilder()
                .WithColor(Color.Gold)
                .WithCurrentTimestamp()
                .WithTitle("Commands");
            
            var modules = _commands.Modules;
            foreach (var module in modules)
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
