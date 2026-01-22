using BlueShell.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Commands
{
    public class NullCommand : ShellCommandBase
    {
        protected override void ExecuteLogic(ref string? dir, params string[]? args)
        {
            Console.WriteLine("NullCommand Executed.");
        }
    }
}
