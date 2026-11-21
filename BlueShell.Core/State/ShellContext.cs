using BlueShell.Core.Models;

namespace BlueShell.Core.State;

public sealed class ShellContext
{
    public ShellMode CurrentMode { get; set; } = ShellMode.Shell;

    public string WorkingDirectory { get; set; } =
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
}
