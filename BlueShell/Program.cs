using BlueShell.Application.Commands;
using BlueShell.Application.Execution;
using BlueShell.Application.Helper;
using BlueShell.Application.Parsing;
using BlueShell.Commands;
using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using BlueShell.Core.State;
using Spectre.Console;

namespace BlueShell.Cli;

internal static class Program
{
    public static void Main(string[] args)
    {
        var context = new ShellContext();
        var registry = new CommandRegistry(typeof(CommandExecutor).Assembly);
        StartUp();

        while (true)
        {
            ConsoleHelper.WriteColoredLine($"\nMode: {context.CurrentMode}", ConsoleColor.Yellow);
            Console.Write(context.WorkingDirectory + ">\t");
            string userInput = InputHelper.AutoReadLine(registry.Commands.Keys);

            if (string.IsNullOrWhiteSpace(userInput))
            {
                ConsoleHelper.WriteErrorLine("No command entered. Please enter a command.");
                continue;
            }

            CommandLine commandLine = CommandParser.Parse(userInput);

            if (!registry.TryGetCommand(commandLine.Command, out var registeredCommand))
            {
                ConsoleHelper.WriteErrorLine($"Command '{commandLine.Command}' not found.");
                continue;
            }

            Console.WriteLine();
            CommandExecutor.ExecuteCommand(registeredCommand, context, commandLine);
        }
    }

    private static void StartUp()
    {
        Console.Title = "BlueShell";
        ConsoleHelper.ShowTitelbar();
    }
}