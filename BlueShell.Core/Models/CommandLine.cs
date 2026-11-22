using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Core.Models
{
    public class CommandLine
    {
        public static readonly CommandLine Empty = new(string.Empty, Array.Empty<string>(), false);
        public static readonly IReadOnlyList<string> HelpAliases = Array.AsReadOnly(new string[] { "?", "help" });
        public string Command { get; }
        public string[] Arguments { get; }
        public bool HelpRequested { get; }
        public int ArgumentCount => Arguments.Length;
        public CommandLine(string command, string[] arguments, bool helpRequested)
        {
            Command = command;
            Arguments = arguments;
            HelpRequested = helpRequested;
        }
    }
}
