using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using BlueShell.Core.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Commands
{
    public abstract class ShellCommandBase : IShellCommand
    {
        protected virtual int MinArgs => 0;

        public void Execute(ShellContext context, CommandLine commandLine)
        {
            if (commandLine.HelpRequested)
            {
                PrintHelp();
                return;
            }

            if (!EnsureArgs(commandLine.Arguments))
                return;

            ExecuteLogic(context, commandLine.Arguments);
        }

        protected virtual void PrintHelp()
        {
            Console.WriteLine("No help implemented");
        }

        protected virtual void ExecuteLogic(ShellContext context, string[] args)
        {
            Console.WriteLine("No execute implemented");
        }

        private bool EnsureArgs(string[] args)
        {
            if (MinArgs == 0)
                return true;

            if (args.Length < MinArgs)
            {
                Console.WriteLine("Too few arguments. Necessary arguments: " + MinArgs);
                return false;
            }

            if (args.Any(string.IsNullOrWhiteSpace))
            {
                Console.WriteLine("Arguments must not be empty.");
                return false;
            }

            return true;
        }
    }
}