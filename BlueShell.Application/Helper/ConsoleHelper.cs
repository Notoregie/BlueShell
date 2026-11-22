using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Application.Helper
{
    public static class ConsoleHelper
    {
        public static void WriteColoredLine(string input, ConsoleColor color)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(input);
            Console.ForegroundColor = previousColor;
        }

        public static void WriteColored(string input, ConsoleColor color)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(input);
            Console.ForegroundColor = previousColor;
        }

        public static void ShowTitelbar()
        {
            WriteColoredLine("BlueShell [Version 0.7]\n(c) Notoregy Corporation. All rights reserved.\n", ConsoleColor.Blue);
            WriteColoredLine("Type 'help' to show all commands.\n", ConsoleColor.Green);
        }

        public static void WriteErrorLine(string message) => WriteColoredLine(message, ConsoleColor.Red);

        public static void WriteWarningLine(string message) => WriteColoredLine(message, ConsoleColor.Yellow);

        public static void WriteSuccessLine(string message) => WriteColoredLine(message, ConsoleColor.Green);

        public static void WriteInfoLine(string message) => WriteColoredLine(message, ConsoleColor.Cyan);

        public static string Bold(string input) => $"\x1b[1m{input}\x1b[0m";
        public static string Underline(string input) => $"\x1b[4m{input}\x1b[0m";
    }
}
