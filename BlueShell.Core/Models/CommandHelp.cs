using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Core.Models
{
    public sealed record CommandHelp(
    string Usage,
    string Description,
    IReadOnlyList<string>? Examples = null,
    IReadOnlyList<string>? Notes = null);
}
