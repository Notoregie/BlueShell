using BlueShell.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Commands
{
    public abstract class ShellCommandBase : IShellCommand
    {
        protected virtual int MinArgs => 0;
        public void Execute(ref string? dir, params string[]? args)
        {
            if (args.Length == 1 && args[0] == "?")
            {
                PrintHelp();
                return;
            }
            if (!EnsureArgs(args)) 
                return;

            ExecuteLogic(ref dir, args);
        }
        protected virtual void PrintHelp()
        {
            Console.WriteLine("No help implemented");
        }


        protected virtual void ExecuteLogic(ref string? dir, string[]? args)
        {
            Console.WriteLine("No execute implemented");
        }

        bool EnsureArgs(string[] args)
        {
            if (MinArgs == 0) 
                return true;

            if (args.Length < MinArgs)
            {
                Console.WriteLine("Too few arguments. Necessary arguments: " + MinArgs);
                return false;
            }
            if (args.Any(a => string.IsNullOrWhiteSpace(a)))
            {
                Console.WriteLine("Arguments must not be empty.");
                return false;
            }
            return true;
        }
    }
}