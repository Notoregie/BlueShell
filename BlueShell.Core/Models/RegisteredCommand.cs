using BlueShell.Core.Attributes;
using BlueShell.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Core.Models
{
    public sealed class RegisteredCommand
    {
        public required CommandAttribute Metadata { get; init; }
        public required IShellCommand Implementation { get; init; }
        public required string Source { get; init; }
    }
}
