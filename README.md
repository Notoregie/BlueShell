# BlueShell

BlueShell is a command-line shell developed by **Notoregy**.

This project is primarily a personal learning and practice project. It is used to practice existing programming methods, experiment with architectural ideas, and add new functionality over time. The project may therefore evolve as new concepts are explored and implemented.

## Project Structure

The solution is split into several projects:

```text
BlueShell/
├── BlueShell/                 # Executable CLI project and application entry point
├── BlueShell.Core/            # Core models, state, and interfaces
└── BlueShell.Application/     # Commands and application logic
```

## Adding a New Command

Commands are loaded from `BlueShell/Configs/Shell.xml` and resolved through reflection. The command name in the XML file must therefore match the name of the C# command class.

### Naming convention

The following naming convention is required:

```text
XML Id:       example
C# class:     ExampleCommand
Namespace:    BlueShell.Commands
File:         ExampleCommand.cs
```

The loader transforms the first character of the XML `Id` to uppercase, appends `Command`, and searches for the resulting type in the `BlueShell.Commands` namespace.

For example:

```text
Id="echo"      -> BlueShell.Commands.EchoCommand
Id="ipconfig"  -> BlueShell.Commands.IpconfigCommand
Id="mycommand" -> BlueShell.Commands.MycommandCommand
```

Keep the command ID and class name consistent. The parser is case-sensitive when resolving the generated type name, so use the same casing convention as the existing commands.

### 1. Create the command class

Create a new file in:

```text
BlueShell.Application/Commands/ExampleCommand.cs
```

Derive the class from `ShellCommandBase` and override `ExecuteLogic`:

```csharp
using BlueShell.Core.State;

namespace BlueShell.Commands;

public sealed class ExampleCommand : ShellCommandBase
{
    protected override int MinArgs => 1;

    protected override void PrintHelp()
    {
        Console.WriteLine("example <text> - Executes the example command");
    }

    protected override void ExecuteLogic(ShellContext context, string[] args)
    {
        Console.WriteLine($"Example command executed with: {args[0]}");
    }
}
```

`ShellCommandBase` provides common argument validation. If the command requires arguments, set `MinArgs` accordingly. If the user enters `?` as the only argument, `PrintHelp` is called automatically.

The `ShellContext` contains the current runtime state of the shell, such as the current mode and working directory. Use it when the command needs to read or change the shell state.

### 2. Register the command in XML

Add a `ShellCommand` element to:

```text
BlueShell/Configs/Shell.xml
```

```xml
<ShellCommand
    Id="example"
    Type="custom"
    Summary="Executes an example command">
</ShellCommand>
```

The important part is the `Id` value. For the class above, the value must be `example`, because it resolves to `ExampleCommand`.

The `Type` is used when displaying commands in the help output. Existing values are:

- `default` for standard shell commands
- `custom` for additional or experimental commands

The `Summary` is shown by the `help` command.

### 3. Build and test the command

Build the solution and start BlueShell. The command should then be available by its XML ID:

```text
example "Hello BlueShell"
```

To display the command help, use:

```text
example ?
```

You can also run:

```text
help
```

to display all commands registered in `Shell.xml`.

## Current Command Locations

All command implementations are located in:

```text
BlueShell.Application/Commands/
```

The command configuration is located in:

```text
BlueShell/Configs/Shell.xml
```

The common command contract and shell state are located in the Core project:

```text
BlueShell.Core/Interfaces/IShellCommand.cs
BlueShell.Core/State/ShellContext.cs
```

## Disclaimer

BlueShell is a personal learning project by **Notoregy**. The structure, APIs, and command system may change as part of ongoing experimentation and development.