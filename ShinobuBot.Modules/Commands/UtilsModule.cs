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
        private readonly IServiceProvider _services;
        private readonly CommandService _commands; 

        private const string ModulesAssembly = "ShinobuBot.Modules";

        public UtilsModule(IServiceProvider services)
        {
            _services = services;
            _commands = _services.GetRequiredService<CommandService>();
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
