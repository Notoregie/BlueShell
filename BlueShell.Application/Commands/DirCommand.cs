using BlueShell.Core.Attributes;
using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using BlueShell.Core.State;

namespace BlueShell.Commands
{
    [Command("dir", "Lists top-level directories.", "dir")]
    public sealed class DirCommand : IShellCommand, IDetailedHelpProvider
    {
        public int MinArgs => 0;

        public void Execute(ShellContext context, string[] args)
        {
            foreach (var topDir in Directory.GetDirectories(context.WorkingDirectory, "*", SearchOption.TopDirectoryOnly))
            {
                string folderName = Path.GetFileName(topDir);
                Console.WriteLine("{0,-30} [{1}]", folderName, topDir);
            }
        }

        public CommandHelp GetDetailedHelp() => new(
            "dir",
            "Lists only the directories directly inside the current working directory.",
            Notes: ["The current working directory is managed by the cd command."]);
    }
}
