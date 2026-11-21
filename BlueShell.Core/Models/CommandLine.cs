using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Core.Models
{
    public class CommandLine
    {
        public string Command { get; set; }
        public string[] Arguments { get; set; }
        public int ArgumentCount => Arguments.Length;
        public CommandLine(string command, string[] arguments)
        {
            Command = command;
            Arguments = arguments;
        }
    }
}
