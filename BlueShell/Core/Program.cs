using BlueShell.Interfaces;

namespace BlueShell.Core
{
    class Program
    {
        static string? workingDirectory = ResetPath();
        public static void Main(string[] args)
        {
            StartUp();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Mode: " + Global.CurrentMode);
                Console.ResetColor();
                Console.WriteLine(Environment.NewLine);


                Console.Write(workingDirectory + ">" + "\t");
                string userInput = Console.ReadLine() ?? string.Empty;
                TryExecuteCommand(userInput);
            }
        }

        static void StartUp()
        {
            XmlShell.Instance.InitializeMetaData();
            Console.Title = "BlueShell";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("BlueShell [Version 0.7]\r\n(c) Notoregy Corporation. All rights reserved." + Environment.NewLine);
            Console.ResetColor();
            Console.WriteLine("Type 'help' to show all commands." + Environment.NewLine);
        }
        static string ResetPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        static void TryExecuteCommand(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput)) return;
            (string commandName, string[]? arguments) = ParseCommand(userInput);

            if (XmlShell.Instance.CommandMap.TryGetValue(commandName, out IShellCommand shellCommand))
            {
                Console.WriteLine();
                shellCommand.Execute(ref workingDirectory, arguments);
            }
            else
            {
                Console.WriteLine($"The command named '{commandName}' could not be found.");
            }
            Console.WriteLine();
        }
        static (string command, string[]? arguments) ParseCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return (string.Empty, null);

            string command = input.Split(' ').FirstOrDefault()?? string.Empty;
            string[] rawArguments = GetArgs(input);
            string[] expandedArguments = ExpandEnvironmentVariables(rawArguments);

            return (command.ToLower(), expandedArguments);
        }

        static string[] GetArgs(string input)
        {
            List<string> args = new();
            int index = 0;

            while (true)
            {
                int start = input.IndexOf('"', index);
                if (start == -1)
                    break;

                int end = input.IndexOf('"', start + 1);
                if (end == -1)
                    break;

                args.Add(input.Substring(start + 1, end - start - 1));
                index = end + 1;
            }
            return args.ToArray();
        }

        static string[] ExpandEnvironmentVariables(params string[] args)
        {
            #region Get EnvironmentVariables
            var envVariables = Environment.GetEnvironmentVariables();
            List<string> envVariableNames = new List<string>();
            foreach (var key in envVariables.Keys)
            {
                envVariableNames.Add(key.ToString());
            }

            if (envVariableNames.Count() == 0)
            {
                return args;
            }
            #endregion

            var expandedArguments = new List<string>();

            foreach (var arg in args)
            {
                if (arg.StartsWith("%") && arg.EndsWith("%"))
                {
                    int length = arg.Length;
                    string envVariableName = arg.Substring(1, length - 2);

                    bool exists = envVariableNames.Contains(envVariableName, StringComparer.OrdinalIgnoreCase);

                    if (exists)
                    {
                        expandedArguments.Add(Environment.GetEnvironmentVariable(envVariableName));
                        continue;
                    }
                }
                expandedArguments.Add(arg);
            }
            return expandedArguments.ToArray();
        }
    }
}