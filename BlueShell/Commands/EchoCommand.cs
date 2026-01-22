using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Commands
{
    public class EchoCommand : ShellCommandBase
    {
        protected override int MinArgs => 1;
        protected override void ExecuteLogic(ref string? dir, params string[]? args)
        {
            foreach (string arg in args)
            {
                Console.WriteLine(arg);
            }
        }
    }
}
