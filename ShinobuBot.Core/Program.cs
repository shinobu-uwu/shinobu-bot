using System;
using System.Threading;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ShinobuBot.Core;
using ShinobuBot.Core.Services;

using var services = Startup.ConfigureServices().BuildServiceProvider();
var client = services.GetRequiredService<DiscordSocketClient>();
    
await services.GetRequiredService<LoggingService>().InitializeAsync();
await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

Thread.Sleep(-1);
