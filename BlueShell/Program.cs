using BlueShell.Application.Configuration;
using BlueShell.Application.Helper;
using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using BlueShell.Core.State;

namespace BlueShell.Cli;

internal static class Program
{
    public static void Main(string[] args)
    {
        var context = new ShellContext();
        StartUp();

        while (true)
        {
            Decorator.WriteColoredLine($"Mode: {context.CurrentMode}", ConsoleColor.Yellow);
            Console.Write(context.WorkingDirectory + ">\t");
            string userInput = Console.ReadLine() ?? string.Empty;
            TryExecuteCommand(context, userInput);
        }
    }

    private static void StartUp()
    {
        XmlShell.Instance.InitializeMetaData();
        Console.Title = "BlueShell";
        Decorator.ShowTitelbar();
    }

    private static void TryExecuteCommand(ShellContext context, string userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput))
            return;

        CommandLine commandLine = CommandParser.Parse(userInput);

        if (XmlShell.Instance.CommandMap.TryGetValue(commandLine.Command, out IShellCommand? shellCommand))
        {
            Console.WriteLine();
            shellCommand.Execute(context, commandLine);
        }
        else
        {
            Console.WriteLine($"The command named '{commandLine.Command}' could not be found.");
        }

        Console.WriteLine();
    }
}