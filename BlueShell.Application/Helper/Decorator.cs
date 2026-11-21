using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Application.Helper
{
    public static class Decorator
    {
        public static string WriteColoredLine(string input, ConsoleColor color)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(input);
            Console.ForegroundColor = previousColor;
            return input;
        }

        public static void ShowTitelbar()
        {
            Decorator.WriteColoredLine("BlueShell [Version 0.7]\r\n(c) Notoregy Corporation. All rights reserved.\n", ConsoleColor.Blue);
            Decorator.WriteColoredLine("Type 'help' to show all commands.\n", ConsoleColor.Green);
        }
    }
}
