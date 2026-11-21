using BlueShell.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Application.Helper
{
    public static class CommandParser
    {

        public static CommandLine Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new CommandLine("", new string[0]);

            string command = input.Split(' ').FirstOrDefault() ?? string.Empty;
            string[] rawArguments = ParseArguments(input);
            string[] expandedArguments = Helper.ExpandEnvironmentVariables(true, rawArguments);

            return new CommandLine(command, expandedArguments);
        }
        public static string[] ParseArguments(string input)
        {
            List<string> args = new();
            int index = 0;

            //int commandEnd = input.IndexOf(' ');
            //if (commandEnd >= 0)
            //{
            //    index = commandEnd;
            //    while (index < input.Length && char.IsWhiteSpace(input[index]))
            //    {
            //        index++;
            //    }

            //    if (index < input.Length && input[index] == '?'
            //        && (index + 1 == input.Length || char.IsWhiteSpace(input[index + 1])))
            //    {
            //        args.Add("?");
            //        index++;
            //    }
            //}

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

        private static bool HelpCommandIsRequested(string input)
        {
            
        }
    }
}
