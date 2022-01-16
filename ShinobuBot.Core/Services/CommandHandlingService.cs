using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ShinobuBot.Core.Interfaces;
using ShinobuBot.Modules.Database;

namespace ShinobuBot.Core.Services
{
    public class CommandHandlingService : IInitializableService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly BotDbContext _dbContext;

        private const string ModulesAssembly = "ShinobuBot.Modules";

        public CommandHandlingService(IServiceProvider services)
        {
            _services = services;
            _client = _services.GetRequiredService<DiscordSocketClient>();
            _commands = _services.GetRequiredService<CommandService>();
            _dbContext = _services.GetRequiredService<BotDbContext>();
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(
                assembly: Assembly.Load(ModulesAssembly),
                services: _services);
                _client.MessageReceived += HandleCommandAsync;
        }

        public async Task HandleCommandAsync(SocketMessage m)
        {
            var message = m as SocketUserMessage;
            if (message is null)
                return;
            
            var context = new SocketCommandContext(_client, message);

            string prefix;
            try
            {
                prefix = _dbContext.Configurations.SingleOrDefault(c => c.GuildId == context.Guild.Id).Prefix;
            }
            catch (NullReferenceException)  // Config for the server not in the database, use default prefix
            {
                prefix = "!";

            }
            var argPos = 0;

            if (!(message.HasStringPrefix(prefix, ref argPos) || message.Author.IsBot))
                return;
            
            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }
    }
}
