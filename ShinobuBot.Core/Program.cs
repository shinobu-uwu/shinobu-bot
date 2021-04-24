using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ShinobuBot.Core;

namespace ShinobuBot
{
    class Program
    {
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();
                await services.GetRequiredService<LoggingService>().InitializeAsync();
                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
                
                var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();
                
                await Task.Delay(-1);
            }
        }

        private ServiceProvider ConfigureServices()
        {
            return Startup.ConfigureServices().BuildServiceProvider();
        }
    }
}
