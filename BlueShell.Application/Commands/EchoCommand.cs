using System;
using System.Collections.Generic;
using System.Text;
using BlueShell.Core.State;

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
    }
}
