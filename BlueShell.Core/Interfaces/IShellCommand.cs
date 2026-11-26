using BlueShell.Core.Models;
using BlueShell.Core.State;

namespace BlueShell.Core.Interfaces;

public interface IShellCommand
{
    int MinArgs { get; }
    void Execute(ShellContext context, string[] args);
}
