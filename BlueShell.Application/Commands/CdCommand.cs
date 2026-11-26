using BlueShell.Core.Attributes;
using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using BlueShell.Core.State;
using System;

namespace BlueShell.Commands
{
    [Command(
        "cd", 
        "Changes the current working directory.", 
        "cd <path>")]
    public sealed class CdCommand : IShellCommand, IDetailedHelpProvider
    {
        public int MinArgs => 1;

        public void Execute(ShellContext context, string[] args)
        {
            string newDir = args[0];

            if (newDir.TrimEnd('\\') == "..")
            {
                context.WorkingDirectory = GoBack(context.WorkingDirectory);
                Console.WriteLine($"Went back to {context.WorkingDirectory}");
                return;
            }

            newDir = CleanPath(newDir);

            if (TryParsePaths(context.WorkingDirectory, newDir, out string updatedDir))
            {
                context.WorkingDirectory = updatedDir;
            }
            else
            {
                Console.WriteLine("Directory couldn't be found.");
            }
        }

        public CommandHelp GetDetailedHelp() => new(
            "cd <path>",
            "Changes the current working directory to an absolute or relative path.",
            Examples: ["cd C:\\Projects", "cd .\\src", "cd .."],
            Notes: ["Use '..' to move to the parent directory."]);

        private static bool TryParsePaths(string currentDir, string newDir, out string updatedDir)
        {
            if (IsAbsolutePath(newDir))
            {
                updatedDir = newDir;
                return true;
            }

            string path = Path.Combine(currentDir, newDir);
            if (Directory.Exists(path))
            {
                updatedDir = path;
                return true;
            }

            updatedDir = string.Empty;
            return false;
        }

        private static bool IsAbsolutePath(string dir) => Directory.Exists(dir);

        private static string GoBack(string dir)
        {
            var parent = new DirectoryInfo(dir).Parent?.FullName;

            if (parent == null)
            {
                Console.WriteLine("There's no parent directory");
                return dir;
            }

            return parent;
        }

        private static string CleanPath(string dir)
        {
            return dir.StartsWith(@".\") ? dir[2..] : dir;
        }
    }
}
