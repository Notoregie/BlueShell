using BlueShell.Core.Attributes;
using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using BlueShell.Core.State;
using System.Linq;

namespace BlueShell.Commands
{
    [Command("help", "Lists all registered commands.", "help")]
    public sealed class HelpCommand : IShellCommand
    {
        private readonly Func<IEnumerable<RegisteredCommand>> _commandProvider;

        public HelpCommand(Func<IEnumerable<RegisteredCommand>> commandProvider)
        {
            _commandProvider = commandProvider;
        }

        public int MinArgs => 0;
        public void Execute(ShellContext context, string[] args)
        {
            var commands = _commandProvider().ToLookup(command => command.Source);
            Console.WriteLine("A list of all Commands:");

            foreach (IGrouping<string, RegisteredCommand> group in commands)
            {
                Console.WriteLine();
                ShowList(group);
            }
        }

        private static void ShowList(IGrouping<string, RegisteredCommand> group)
        {
            Console.WriteLine($"{group.Key} commands:\n");

            foreach (var command in group)
            {
                string id = command.Metadata.Id.ToUpper().PadRight(15);
                string description = command.Metadata.Description.PadRight(50);
                string implementation = (command.Implementation is not NullCommand).ToString();
                Console.WriteLine($"{id}{description} Implemented: {implementation}");
            }
        }
    }
}
