using BlueShell.Core.Models;
using BlueShell.Core.State;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BlueShell.Commands
{
    public sealed class CdCommand : ShellCommandBase
    {
        protected override int MinArgs => 1;

        protected override void PrintHelp(CommandMetadata metadata)
        {
            Console.WriteLine("Usage: cd <path>");
            base.PrintHelp(metadata);
        }

        protected override void ExecuteLogic(ShellContext context, string[] args)
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
