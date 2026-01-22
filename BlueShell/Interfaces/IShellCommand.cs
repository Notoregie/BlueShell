using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Interfaces
{
    public interface IShellCommand
    {
        void Execute(ref string? dir, params string[]? args);
    }
}
