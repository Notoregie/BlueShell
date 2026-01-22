using BlueShell.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Commands
{
    public sealed class DirCommand : ShellCommandBase
    {
        protected override void PrintHelp() => Console.WriteLine("lists all directories from the directory on");
        protected override void ExecuteLogic(ref string? dir, params string[]? objects)
        {
            foreach(var topDir in Directory.GetDirectories(dir, "*", SearchOption.TopDirectoryOnly))
            {
                string folderName = Path.GetFileName(topDir);
                Console.WriteLine("{0,-30} [{1}]", folderName, topDir);
            }
        }
    }
}
