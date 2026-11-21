using BlueShell.Core.State;

namespace BlueShell.Core.Interfaces;

public interface IShellCommand
{
    void Execute(ShellContext context, params string[]? args);
}
