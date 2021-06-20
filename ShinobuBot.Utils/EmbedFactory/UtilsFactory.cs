using System.Collections.Generic;
using Discord;
using Discord.Commands;

namespace ShinobuBot.Utils.Formatters
{
    public static partial class EmbedFactory
    {
        public static Embed ListCommands(IEnumerable<ModuleInfo> modules)
        {
            var builder = new EmbedBuilder().WithColor(0xf4f16d).WithCurrentTimestamp();

            foreach (var module in modules)
            {
                var commands = "";
                foreach (var command in module.Commands)
                    commands += $"`{command.Name}` ";
                builder.AddField(module.Name, commands);
            }

            return builder.Build();
        }

        public static Embed CommandInfo(CommandInfo command)
            => new EmbedBuilder()
                .WithCurrentTimestamp()
                .WithColor(0xf4f16d)
                .AddField("Name", command.Name)
                .AddField("Description", command.Summary)
                .AddField("Parameters", command.Parameters.Count == 0 
                    ? "None" : CommandsFormatter.FormatParameters(command.Parameters))
                .AddField("Remarks", string.IsNullOrEmpty(command.Remarks) ? "None" : command.Remarks)
                .Build();
    }
}