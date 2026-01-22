using BlueShell.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BlueShell.Commands
{
    public sealed class CdCommand : ShellCommandBase
    {
        protected override int MinArgs => 1;
        protected override void PrintHelp() => Console.WriteLine("cd foldername -> goes to the foldert\r\ncd ..\\ -> goes back one directory\r\ncd C:\\Users -> jumps directly to this path\r\ncd \\ -> goes to the root directory of the drive");
        
        protected override void ExecuteLogic(ref string? dir, params string[]? args)
        {
            string? newDir = args[0].ToString();

            #region GoBack
            if (newDir.TrimEnd('\\') == "..")
            {
                dir = GoBack(dir);
                Console.WriteLine($"Went back to {dir}");
                return;
            }
            #endregion


            string? currentDir = dir;
            newDir = CleanPath(newDir);

            if (TryParsePaths(currentDir, newDir, out string updatedDir))
            {
                dir = updatedDir;
            }
            else
            {
                Console.WriteLine("Directory couldn't be found.");
            }

        }

        bool TryParsePaths(string currentDir, string newDir, out string updatedDir)
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
            else
            {
                updatedDir = string.Empty;
                return false;
            }
        }
        bool IsAbsolutePath(string dir)
        {
            if (Directory.Exists(dir)) return true;
            else return false;
        }

        string GoBack(string dir)
        {
            var parent = new DirectoryInfo(dir).Parent?.FullName;

            if (parent == null)
            {
                Console.WriteLine("There's no parent directory");
                return dir;
            }
            else
            {
                return parent;
            }
        }

        string CleanPath(string dir)
        {
            if (dir.StartsWith(@".\"))
            {
                dir = dir.Substring(2);
            }
            return dir;
        }
    }
}
