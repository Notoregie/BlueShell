using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueShell.Application.Helper
{
    public static class InputHelper
    {
        private static readonly List<string> _promptHistory = new List<string>();

        public static string AutoReadLine(IEnumerable<string> options)
        {
            StringBuilder input = new StringBuilder();
            int currentHistoryIndex = _promptHistory.Count;

            while (true)
            {
                var keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                if (keyInfo.Key == ConsoleKey.Tab)
                {
                    string currentInput = input.ToString();
                    int commandEnd = currentInput.IndexOfAny(new char[] { ' ', '\t' });
                    string commandInput = commandEnd >= 0
                        ? currentInput[..commandEnd]
                        : currentInput;

                    string completedCommand = GetNextAutoCompletedValue(commandInput, options);
                    if (completedCommand.Length > commandInput.Length)
                    {
                        string completedInput = completedCommand + currentInput[commandInput.Length..];
                        ReplaceInput(input, completedInput);
                    }

                    continue;
                }

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (currentHistoryIndex > 0)
                    {
                        currentHistoryIndex--;
                        ReplaceInput(input, _promptHistory[currentHistoryIndex]);
                    }
                }

                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (currentHistoryIndex < _promptHistory.Count - 1)
                    {
                        currentHistoryIndex++;
                        ReplaceInput(input, _promptHistory[currentHistoryIndex]);
                        continue;
                    }
                    currentHistoryIndex = _promptHistory.Count;

                    ClearInput(input);
                }

                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0)
                    {
                        input.Length--;
                        Console.Write("\b \b");
                    }

                    continue;
                }

                if (!char.IsControl(keyInfo.KeyChar))
                {
                    input.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                }
            }

            _promptHistory.Add(input.ToString());
            return input.ToString();
        }

        public static string GetNextAutoCompletedValue(string input, IEnumerable<string> options)
        {
            var matches = options
                .Where(option => option.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matches.Count == 0)
                return input;

            if (matches.Count == 1)
                return matches[0];

            int commonLength = input.Length;
            while (commonLength < matches.Min(match => match.Length))
            {
                char candidate = matches[0][commonLength];
                if (matches.Any(match => char.ToUpperInvariant(match[commonLength]) != char.ToUpperInvariant(candidate)))
                    break;

                commonLength++;
            }

            return matches[0][..commonLength];
        }

        private static void ClearInput(StringBuilder input)
        {
            Console.Write(new string('\b', input.Length));
            Console.Write(new string(' ', input.Length));
            Console.Write(new string('\b', input.Length));
            input.Clear();
        }

        private static void ReplaceInput(StringBuilder input, string replacement)
        {
            Console.Write(new string('\b', input.Length));
            Console.Write(new string(' ', input.Length));
            Console.Write(new string('\b', input.Length));
            Console.Write(replacement);

            input.Clear();
            input.Append(replacement);
        }

    }
}
