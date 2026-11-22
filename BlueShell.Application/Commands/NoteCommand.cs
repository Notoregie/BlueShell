using BlueShell.Application.Helper;
using BlueShell.Core.Models;
using BlueShell.Core.State;
using System;
using System.Text;

namespace BlueShell.Commands
{
    public sealed class NoteCommand : ShellCommandBase
    {
        protected override int MinArgs => 2;

        protected override void PrintHelp(CommandMetadata metadata)
        {
            ConsoleHelper.WriteColoredLine("Usage: note <filename> <content>", ConsoleColor.Yellow);
            base.PrintHelp(metadata);
        }

        protected override void ExecuteLogic(ShellContext context, string[] args)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = EnsureTxtExtension(args[0]);
            string filePath = Path.Combine(desktopPath, fileName);
            byte[] bytes = Encoding.UTF8.GetBytes(args[1]);

            using FileStream fs = new(filePath, FileMode.Create, FileAccess.Write);
            fs.Write(bytes, 0, bytes.Length);

            ConsoleHelper.WriteColoredLine($"Note saved to {filePath}", ConsoleColor.Green);
        }

        private static string EnsureTxtExtension(string fileName)
        {
            return Path.ChangeExtension(fileName, ".txt");
        }
    }
}
