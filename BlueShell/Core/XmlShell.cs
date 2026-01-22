using BlueShell.Commands;
using BlueShell.Helper;
using BlueShell.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using System.Xml.Serialization;

namespace BlueShell.Core
{
    public sealed class XmlShell
    {
        public static XmlShell Instance { get; set; } = LoadConfig();

        [XmlElement("ShellCommand")]
        public List<ShellCommand> ShellCommands { get; set; } = new List<ShellCommand>();
        [XmlIgnore]
        public Dictionary<string, IShellCommand> CommandMap { get; set; } = new Dictionary<string, IShellCommand>();
        static XmlShell LoadConfig()
        {
            return XmlHelper.ConvertXml<XmlShell>() ?? new XmlShell();
        }

        public void InitializeMetaData()
        {
            foreach (var command in ShellCommands)
            {
                command.SetCommand();
                CommandMap[command.Id] = command.Command;
            }
        }
    }

    public sealed class ShellCommand
    {
        [XmlAttribute("Id")]
        public string Id { get; set; } = $"Unable to load {nameof(Id)}";
        [XmlAttribute("Type")]
        public string Type { get; set; } = $"Unable to load {nameof(Type)}";
        [XmlAttribute("Summary")]
        public string Summary { get; set; } = $"Unable to load {nameof(Summary)}";
        [XmlIgnore]
        public IShellCommand Command { get; private set; } = new NullCommand();

        public void SetCommand()
        {
            string validId = char.ToUpper(Id[0]) + Id.Substring(1);
            string targetClass = $"BlueShell.Commands.{validId}Command";

            Assembly? assembly = Assembly.GetExecutingAssembly();
            var type = assembly
                .GetTypes()
                .FirstOrDefault(t => t.FullName == targetClass);

            if (type == null)
            {
                Command = new NullCommand();
                return;
            }
            var instance = Activator.CreateInstance(type);
            if (instance is IShellCommand command) 
            {
                Command = command;
                return;
            } 
            
            Command = new NullCommand();
        }
    }
}
