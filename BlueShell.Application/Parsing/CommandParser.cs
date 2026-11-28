using BlueShell.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using BlueShell.Application.Helper;

namespace BlueShell.Application.Parsing
{
    public static class CommandParser
    {
        public static CommandLine Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return CommandLine.Empty;

            string command;
            int commandSeperator = input.IndexOf(' ');

            if (commandSeperator >= 0)
            {
                command = input[..commandSeperator];
                string arguments = input[(commandSeperator + 1)..].TrimStart();
                if (CommandLine.HelpAliases.Any(alias => arguments == alias || arguments.StartsWith(alias + " ")))
                    return new CommandLine(command, Array.Empty<string>(), true);

                string[] rawArguments = ParseArguments(input);
                string[] expandedArguments = EnviromentVariableExpander.ExpandEnvironmentVariables(true, rawArguments);

                return new CommandLine(command, expandedArguments, false);
            }
            command = input;
            return new CommandLine(command, Array.Empty<string>(), false);
        }
        public static string[] ParseArguments(string input)
        {
            List<string> args = new();
            int index = 0;

            while (true)
            {
                int start = input.IndexOf('"', index);
                if (start == -1)
                    break;

                int end = input.IndexOf('"', start + 1);
                if (end == -1)
                    break;

                args.Add(input.Substring(start + 1, end - start - 1));
                index = end + 1;
            }

            return args.ToArray();
        }
    }
}
