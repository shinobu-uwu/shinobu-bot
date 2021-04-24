using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ShinobuBot.Core.Services
{
    public class LoggingService
    {
        private readonly IServiceProvider _services;
        
        public LoggingService(IServiceProvider services)
        {
            _services = services;
        }

        public Task InitializeAsync()
        {
            var client = _services.GetRequiredService<DiscordSocketClient>();
            var commands = _services.GetRequiredService<CommandService>();
            client.Log += LogAsync;
            commands.Log += LogAsync;
            
            return Task.CompletedTask;
        }
        
        private Task LogAsync(LogMessage m)
        {
            if (m.Exception is CommandException commandException)
            {
                Console.WriteLine($"[Command/{m.Severity}] - {commandException.Command.Aliases.First()}"
                    + $" failed to execute in {commandException.Context.Channel}:");
                Console.WriteLine(commandException);
            }
            else
            {
                Console.WriteLine($"[General/{m.Severity}] - {m}");
            }

            return Task.CompletedTask;
        }
    }
}
