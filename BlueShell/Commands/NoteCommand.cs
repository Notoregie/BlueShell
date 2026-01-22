using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Commands
{
    public class NoteCommand : ShellCommandBase
    {
        protected override int MinArgs => 2;
        protected override void ExecuteLogic(ref string? dir, params string[]? args)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string? fileName = EnsureTxtExtension(args[0].ToString());
            string filePath = Path.Combine(desktopPath, fileName);

            string? content = args[1].ToString();

            var utf8Encoder = new UTF8Encoding();
            byte[] bytes = utf8Encoder.GetBytes(content);

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        static string EnsureTxtExtension(string fileName)
        {
            return fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
                ? fileName
                : fileName + ".txt";
        }
    }
}
