using BlueShell.Core.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Commands
{
    public sealed class NullCommand : ShellCommandBase
    {
        protected override void ExecuteLogic(ShellContext context, string[] args)
        {
            Console.WriteLine("NullCommand executed.");
        }
    }
}
