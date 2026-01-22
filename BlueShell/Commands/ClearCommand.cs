using BlueShell.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Commands
{
    public sealed class ClearCommand : ShellCommandBase
    {
        protected override void PrintHelp() => Console.WriteLine("Just clears the console\n\rNo arguments needed");
        protected override void ExecuteLogic(ref string? dir, params string[]? objects)
        {
            Console.Clear();
        }
    }
}
