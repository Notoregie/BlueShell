using BlueShell.Application.Helper;
using BlueShell.Core.Attributes;
using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using BlueShell.Core.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Application
{
    public static class CommandExecutor
    {
        public static void ExecuteCommand(RegisteredCommand command, ShellContext context, CommandLine commandLine)
        {
            if (commandLine.HelpRequested)
            {
                if (command.Implementation is IDetailedHelpProvider detailedHelpProvider)
                    PrintDetailedHelp(detailedHelpProvider);
                else
                    PrintBasicHelp(command.Metadata);

                return;
            }

            if (!EnsureArgs(command.Implementation.MinArgs, commandLine.Arguments))
                return;

            try
            {
                command.Implementation.Execute(context, commandLine.Arguments);
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteErrorLine($"An error occurred while executing the command: {ex.Message}");
            }
        }

        private static bool EnsureArgs(int minArgs, string[] args)
        {
            if (args.Length < minArgs)
            {
                ConsoleHelper.WriteErrorLine("Too few arguments. Necessary arguments: " + minArgs);
                return false;
            }

            if (args.Any(string.IsNullOrWhiteSpace))
            {
                ConsoleHelper.WriteErrorLine("Arguments must not be empty.");
                return false;
            }

            return true;
        }

        private static void PrintDetailedHelp(IDetailedHelpProvider detailedHelpProvider)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleHelper.InfoColor;
            var help = detailedHelpProvider.GetDetailedHelp();
            Console.WriteLine($"Usage: {help.Usage}");
            Console.WriteLine($"Description: {help.Description}");
            if (help.Examples != null && help.Examples.Count > 0)
            {
                Console.WriteLine("Examples:");
                foreach (var example in help.Examples)
                    Console.WriteLine($"{example}");
            }
            if (help.Notes != null && help.Notes.Count > 0)
            {
                Console.WriteLine("Notes:");
                foreach (var note in help.Notes)
                    Console.WriteLine($"{note}");
            }
            Console.ForegroundColor = originalColor;
        }

        private static void PrintBasicHelp(CommandAttribute commandAttribute)
        {
            ConsoleHelper.WriteColoredLine($"Usage: {commandAttribute.Usage}", ConsoleColor.Cyan);
            ConsoleHelper.WriteColoredLine($"Description: {commandAttribute.Description}", ConsoleColor.Cyan);
        }
    }
}
