using BlueShell.Core.Models;
using BlueShell.Core.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Commands
{
    public sealed class DirCommand : ShellCommandBase
    {
        protected override void PrintHelp(CommandMetadata metadata)
        {
            Console.WriteLine("Usage: dir");
            base.PrintHelp(metadata);
        }

        protected override void ExecuteLogic(ShellContext context, string[] args)
        {
            foreach (var topDir in Directory.GetDirectories(context.WorkingDirectory, "*", SearchOption.TopDirectoryOnly))
            {
                string folderName = Path.GetFileName(topDir);
                Console.WriteLine("{0,-30} [{1}]", folderName, topDir);
            }
        }
    }
}
