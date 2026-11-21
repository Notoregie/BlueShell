using System;
using System.Text;
using BlueShell.Core.State;

namespace BlueShell.Commands
{
    public sealed class NoteCommand : ShellCommandBase
    {
        protected override int MinArgs => 2;

        protected override void ExecuteLogic(ShellContext context, string[] args)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = EnsureTxtExtension(args[0]);
            string filePath = Path.Combine(desktopPath, fileName);
            byte[] bytes = Encoding.UTF8.GetBytes(args[1]);

            using FileStream fs = new(filePath, FileMode.Create, FileAccess.Write);
            fs.Write(bytes, 0, bytes.Length);
        }

        private static string EnsureTxtExtension(string fileName)
        {
            return fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
                ? fileName
                : fileName + ".txt";
        }
    }
}
