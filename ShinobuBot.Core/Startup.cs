using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ShinobuBot.Core.Services;
using ShinobuBot.Modules.Database;

namespace ShinobuBot.Core
{
    public class Startup
    {
        public static IServiceCollection ConfigureServices()
            => new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<LoggingService>()
                .AddSingleton<BotDbContext>();
    }
}
