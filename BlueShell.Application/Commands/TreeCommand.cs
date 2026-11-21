using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using BlueShell.Core.State;

namespace BlueShell.Commands
{

    public sealed class TreeCommand : ShellCommandBase
    {
        protected override void PrintHelp() => Console.WriteLine("Nothing");

        protected override void ExecuteLogic(ShellContext context, string[] args)
        {
            DirectoryInfo directoryInfo = new(context.WorkingDirectory);
            DirectoryInfo driveInfo = directoryInfo.Root!;
            Console.WriteLine(driveInfo.Name);

            PrintTree(context.WorkingDirectory);
        }

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
