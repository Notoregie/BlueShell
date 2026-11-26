using BlueShell.Core.Attributes;
using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using BlueShell.Core.State;

namespace BlueShell.Commands
{
    [Command("echo", "Writes the provided text to the console.", "echo <text>")]
    public sealed class EchoCommand : IShellCommand, IDetailedHelpProvider
    {
        public int MinArgs => 1;

        public void Execute(ShellContext context, string[] args)
        {
            foreach (string arg in args)
            {
                Console.WriteLine(arg);
            }
        }

        public CommandHelp GetDetailedHelp() => new(
            "echo <text>",
            "Writes each provided argument on a separate line.",
            Examples: ["echo Hello world"]);
    }
}
