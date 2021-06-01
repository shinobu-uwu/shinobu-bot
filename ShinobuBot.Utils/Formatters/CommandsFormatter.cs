using System.Collections.Generic;
using Discord.Commands;


namespace ShinobuBot.Utils.Formatters
{
    public static class CommandsFormatter
    {
        public static string FormatParameters(IReadOnlyList<ParameterInfo> parameters)
        {
            var formatted = "";
            for (int i = 0; i < parameters.Count - 1; i++)
                formatted += $"{parameters[i].Name}, ";
            formatted += parameters[parameters.Count - 1].Name;
            
            return formatted;
        }
    }
}