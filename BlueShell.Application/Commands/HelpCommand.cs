using BlueShell.Application.Configuration;
using BlueShell.Core;
using BlueShell.Core.Models;
using BlueShell.Core.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueShell.Commands
{
    public sealed class HelpCommand : ShellCommandBase
    {
        protected override void PrintHelp(CommandMetadata metadata)
        {
            Console.WriteLine("Usage: help");
            base.PrintHelp(metadata);
        }
        protected override void ExecuteLogic(ShellContext context, string[] args)
        {
            Console.WriteLine("A list of all Commands:\n");

            var defaultCommands = XmlShell.Instance.ShellCommands
                .OrderBy(x => x.Id)
                .Where(x => x.Type == "default");

            var customCommands = XmlShell.Instance.ShellCommands
                .OrderBy(x => x.Id)
                .Where(x => x.Type == "custom");

            ShowList(defaultCommands);
            ShowList(customCommands);
        }

        private static void ShowList(IEnumerable<ShellCommand> list)
        {
            var type = list.Select(x => x.Type).FirstOrDefault();
            if (string.IsNullOrEmpty(type))
                return;

            Console.WriteLine($"{char.ToUpper(type[0]) + type[1..]} commands:\n");

            foreach (var command in list)
            {
                string id = command.Id.ToUpper().PadRight(15);
                string summary = command.Summary.PadRight(50);
                string implemented = (command.Command is not NullCommand).ToString();
                Console.WriteLine($"{id}{summary} Implemented: {implemented}");
            }

            Console.WriteLine("\n");
        }
    }
}
