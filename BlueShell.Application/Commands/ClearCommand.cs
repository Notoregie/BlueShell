using BlueShell.Core.Attributes;
using BlueShell.Core.Interfaces;
using BlueShell.Core.State;

namespace BlueShell.Commands
{
    [Command("clear", "Clears the console screen.", "clear")]
    public sealed class ClearCommand : IShellCommand
    {
        public int MinArgs => 0;

        public void Execute(ShellContext context, string[] args)
        {
            Console.Clear();
        }
    }
}
