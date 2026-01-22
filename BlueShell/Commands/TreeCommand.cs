using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace BlueShell.Commands
{
    public sealed class TreeCommand : ShellCommandBase
    {
        protected override void PrintHelp() => Console.WriteLine("Nothing");
        protected override void ExecuteLogic(ref string? dir, params string[]? args)
        {
            DirectoryInfo directoryInfo = new(dir);
            DirectoryInfo driveInfo = directoryInfo.Root;
            Console.WriteLine(driveInfo.Name);
            
            PrintTree(dir);
        }

        void PrintTree(string? dir, string indent = "", bool isLast = true)
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
        //void PrintTreeWithFiles(string? dir, string indent = "", bool isLast = true)
        //{
        //    Console.Write(indent);

        //    if (isLast)
        //    {
        //        Console.Write("└── ");
        //        indent += "    ";
        //    }
        //    else
        //    {
        //        Console.Write("├── ");
        //        indent += "│   ";
        //    }

        //    Console.WriteLine(Path.GetFileName(dir));

        //    var dirs = Directory.GetDirectories(dir);


        //    for (int i = 0; i < dirs.Length; i++)
        //    {
        //        PrintTree(dirs[i], indent, i == dirs.Length - 1);
        //    }
        //}
    }
}
