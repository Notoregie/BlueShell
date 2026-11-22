using BlueShell.Core.Models;
using BlueShell.Core.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Commands
{
    public class EchoCommand : ShellCommandBase
    {
        protected override int MinArgs => 1;

        protected override void ExecuteLogic(ShellContext context, string[] args)
        {
            foreach (string arg in args)
            {
                Console.WriteLine(arg);
            }
        }

        protected override void PrintHelp(CommandMetadata metadata)
        {
            Console.WriteLine("Usage: echo <text>");
            base.PrintHelp(metadata);
        }
    }
}
