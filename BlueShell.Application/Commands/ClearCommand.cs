using BlueShell.Core.Models;
using BlueShell.Core.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Commands
{
    public sealed class ClearCommand : ShellCommandBase
    {
        protected override void PrintHelp(CommandMetadata metadata)
        {
            Console.WriteLine("Usage: clear");
            base.PrintHelp(metadata);
        }

        protected override void ExecuteLogic(ShellContext context, string[] args)
        {
            Console.Clear();
        }
    }
}
