using BlueShell.Core;
using BlueShell.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Commands
{
    public sealed class HelpCommand : ShellCommandBase
    {
        protected override void PrintHelp() => Console.WriteLine("Stupid?");
        protected override void ExecuteLogic(ref string? dir, params string[]? objects)
        {
            Console.WriteLine("A list of all Commands:" + Environment.NewLine);
            var defaultCommands = XmlShell.Instance.ShellCommands
                .OrderBy(x => x.Id)
                .Where(x => x.Type == "default")
                ?? new List<ShellCommand>();

            var customCommands = XmlShell.Instance.ShellCommands
                .OrderBy(x => x.Id)
                .Where(x => x.Type == "custom")
                ?? new List<ShellCommand>();

            ShowList(defaultCommands);
            ShowList(customCommands);
        }

        void ShowList(IEnumerable<ShellCommand> list)
        {
            if (list.Count() == 0) return;
            var type = list.Select(x => x.Type).FirstOrDefault();


            Console.WriteLine($"{char.ToUpper(type[0]) + type.Substring(1)} commands:" + Environment.NewLine);

            foreach (var command in list)
            {
                string id = command.Id.ToUpper().PadRight(15);         
                string summary = command.Summary.PadRight(50);         
                string implemented = (!(command.Command is NullCommand)).ToString();

                Console.WriteLine($"{id}{summary} Implemented: {implemented}");
            }

            Console.WriteLine(Environment.NewLine);
        }
    }
}
