using System.Collections.Generic;
using Discord;
using Discord.Commands;

namespace ShinobuBot.Utils.Embeds
{
    public class HelpEmbed : EmbedBuilder
    {
        public static Embed Build(IEnumerable<ModuleInfo> modules)
        {
            var embedBuilder = new EmbedBuilder()
                .WithColor(Discord.Color.Gold)
                .WithCurrentTimestamp()
                .WithTitle("Commands");
            foreach (var module in modules)
            {
                var commands = "";
                foreach (var command in module.Commands)
                    commands += $"`{command.Name}` ";
                embedBuilder.AddField(module.Name, commands);
            }

            return embedBuilder.Build();
        }
    }
}