using BlueShell.Application.Helper;
using BlueShell.Core.Attributes;
using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using BlueShell.Core.State;
using System.Text;

namespace BlueShell.Commands
{
    [Command("note", "Creates a .txt file with content on the desktop.", "note <filename> <content>")]
    public sealed class NoteCommand : IShellCommand, IDetailedHelpProvider
    {
        public int MinArgs => 2;

        public void Execute(ShellContext context, string[] args)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = EnsureTxtExtension(args[0]);
            string filePath = Path.Combine(desktopPath, fileName);
            byte[] bytes = Encoding.UTF8.GetBytes(args[1]);

            using FileStream fs = new(filePath, FileMode.Create, FileAccess.Write);
            fs.Write(bytes, 0, bytes.Length);

            ConsoleHelper.WriteColoredLine($"Note saved to {filePath}", ConsoleColor.Green);
        }

        public CommandHelp GetDetailedHelp() => new(
            "note <filename> <content>",
            "Creates or replaces a .txt file on the desktop using the supplied content.",
            Examples: ["note todo Buy milk"],
            Notes: ["The filename always receives a .txt extension."]);

        private static string EnsureTxtExtension(string fileName)
        {
            return Path.ChangeExtension(fileName, ".txt");
        }
    }
}
