using BlueShell.Core.Interfaces;
using BlueShell.Core.State;

namespace BlueShell.Commands
{
    public sealed class NullCommand : IShellCommand
    {
        public int MinArgs => 0;

        public void Execute(ShellContext context, string[] args)
        {
            Console.WriteLine("NullCommand executed.");
        }
    }
}
