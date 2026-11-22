using BlueShell.Application.Configuration;
using BlueShell.Application.Helper;
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
        StartUp();

        while (true)
        {
            ConsoleHelper.WriteColoredLine($"Mode: {context.CurrentMode}", ConsoleColor.Yellow);
            Console.Write(context.WorkingDirectory + ">\t");
            string userInput = Helper.AutoReadLine(XmlShell.Instance.CommandMap.Keys);
            TryExecuteCommand(context, userInput);
        }
    }

    private static void StartUp()
    {
        XmlShell.Instance.InitializeMetaData();
        Console.Title = "BlueShell";
        ConsoleHelper.ShowTitelbar();
    }

    private static void TryExecuteCommand(ShellContext context, string userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput))
            return;

        CommandLine commandLine = CommandParser.Parse(userInput);

        if (XmlShell.Instance.CommandMap.TryGetValue(commandLine.Command, out RegisteredCommand? command))
        {
            Console.WriteLine();
            command.Implementation.Execute(context, commandLine, command.Metadata);
        }
        else
        {
            Console.WriteLine($"The command named '{commandLine.Command}' could not be found.");
        }

        Console.WriteLine();
    }
}