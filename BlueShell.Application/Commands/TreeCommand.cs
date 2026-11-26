using BlueShell.Core.Attributes;
using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using BlueShell.Core.State;

namespace BlueShell.Commands
{
    [Command("tree", "Displays the directory tree recursively.", "tree")]
    public sealed class TreeCommand : IShellCommand, IDetailedHelpProvider
    {
        public int MinArgs => 0;

        public void Execute(ShellContext context, string[] args)
        {
            DirectoryInfo directoryInfo = new(context.WorkingDirectory);
            DirectoryInfo driveInfo = directoryInfo.Root!;
            Console.WriteLine(driveInfo.Name);

            PrintTree(context.WorkingDirectory);
        }

        public CommandHelp GetDetailedHelp() => new(
            "tree",
            "Displays the current working directory and all accessible subdirectories recursively.",
            Notes: ["Directories without access permissions are skipped."]);

        private static void PrintTree(string dir, string indent = "", bool isLast = true)
        {
            Console.Write(indent);

            if (isLast)
            {
                Console.Write("└── ");
                indent += "    ";
            }
            else
            {
                Console.Write("├── ");
                indent += "│   ";
            }

            Console.WriteLine(Path.GetFileName(dir));

            try
            {
                var dirs = Directory.GetDirectories(dir);
                for (int i = 0; i < dirs.Length; i++)
                {
                    PrintTree(dirs[i], indent, i == dirs.Length - 1);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
        }
    }
}
