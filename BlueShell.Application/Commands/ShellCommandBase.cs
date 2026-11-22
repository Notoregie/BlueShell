using BlueShell.Application.Configuration;
using BlueShell.Application.Helper;
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

        public void Execute(ShellContext context, CommandLine commandLine, CommandMetadata metadata)
        {
            if (commandLine.HelpRequested)
            {
                PrintHelp(metadata);
                return;
            }

            if (!EnsureArgs(commandLine.Arguments))
                return;

            ExecuteLogic(context, commandLine.Arguments);
        }

        protected virtual void PrintHelp(CommandMetadata metadata)
        {
            Console.WriteLine(metadata.Summary);
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
                ConsoleHelper.WriteErrorLine("Too few arguments. Necessary arguments: " + MinArgs);
                return false;
            }

            if (args.Any(string.IsNullOrWhiteSpace))
            {
                ConsoleHelper.WriteErrorLine("Arguments must not be empty.");
                return false;
            }

            return true;
        }
    }
}