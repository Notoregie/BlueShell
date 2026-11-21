using System.Reflection;
using System.Xml.Serialization;
using BlueShell.Commands;
using BlueShell.Core.Interfaces;
using BlueShell.Application.Helper;

namespace BlueShell.Application.Configuration;

public sealed class XmlShell
{
    public static XmlShell Instance { get; set; } = LoadConfig();

    [XmlElement("ShellCommand")]
    public List<ShellCommand> ShellCommands { get; set; } = new();

    [XmlIgnore]
    public Dictionary<string, IShellCommand> CommandMap { get; set; } = new();

    private static XmlShell LoadConfig()
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
        string validId = char.ToUpper(Id[0]) + Id[1..];
        string targetClass = $"BlueShell.Commands.{validId}Command";

        Assembly assembly = typeof(ShellCommand).Assembly;
        var type = assembly.GetTypes().FirstOrDefault(t => t.FullName == targetClass);

        if (Activator.CreateInstance(type ?? typeof(NullCommand)) is IShellCommand command)
        {
            Command = command;
            return;
        }

        Command = new NullCommand();
    }
}
