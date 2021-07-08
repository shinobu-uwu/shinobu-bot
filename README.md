# About ShinobuBot
ShinobuBot is a very simple and user friendly bot with some basic commands, designed to be easy to use and expand for users that have no experience with discord bots.

## Get it running
You need to export 3 enviroment variables, `DISCORD_TOKEN` `OSU_API_TOKEN` and `RIOT_API_TOKEN`, and run ShinobuBot.Core project, `dotnet run --project=ShinobuBot.Core/ShinobuBot.Core.csproj`

It is recommended that you make a script for that, a bash script would be like this:

```sh
#!/bin/sh
export DISCORD_TOKEN=your discord bot token # see https://discord.com/developers/docs/intro
export OSU_API_TOKEN=your osu api token # see https://github.com/ppy/osu-api/wiki
export RIOT_API_TOKEN=your riot api token # see https://developer.riotgames.com/
dotnet run --project=ShinobuBot.Core/ShinobuBot.Core.csproj
```

## Create your own commands module
- Create a new class in `ShinobuBot.Modules/Commands` and make it inherit from `ModuleBase<SocketCommandContext>`, so Discord.Net can identify it as a command module.

- You can use [dependency injection](https://www.tutorialsteacher.com/ioc/dependency-injection), some services already are already implemented like DbContext and CommandHandling (see ShinobuBot.Core/Startup.cs for all the services).

## TODO
* A GUI implementation
* More commands!
