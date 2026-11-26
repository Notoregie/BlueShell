using BlueShell.Application.Helper;
using BlueShell.Commands;
using BlueShell.Core.Attributes;
using BlueShell.Core.Interfaces;
using BlueShell.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace BlueShell.Application.Commands
{
    public sealed class CommandRegistry
    {
        private readonly HashSet<Assembly> _registeredAssemblies = new();
        private readonly Dictionary<string, RegisteredCommand> _commands = new(StringComparer.OrdinalIgnoreCase);
        public IReadOnlyDictionary<string, RegisteredCommand> Commands => _commands;

        public CommandRegistry(Assembly assembly)
        {
            RegisterCommandsFromAssembly(assembly);
        }

        public void RegisterCommandsFromAssembly(Assembly assembly)
        {
            if (!_registeredAssemblies.Add(assembly))
            {
                ConsoleHelper.WriteWarningLine($"Assembly '{assembly.FullName}' has already been registered.");
                return;
            }

            var commandTypes = assembly
                .GetTypes()
                .Where(type => type.GetCustomAttribute<CommandAttribute>() is not null
                && type is { IsAbstract: false, IsInterface: false }
                && typeof(IShellCommand).IsAssignableFrom(type));

            foreach (var commandType in commandTypes)
            {
                RegisterCommand(commandType);
            }
        }

        private void RegisterCommand(Type commandType)
        {
            CommandAttribute attribute = commandType.GetCustomAttribute<CommandAttribute>()!; // We know this is not null because of the filter in RegisterCommandsFromAssembly

            ArgumentException.ThrowIfNullOrWhiteSpace(attribute.Id, nameof(attribute.Id));

            if (_commands.ContainsKey(attribute.Id))
                throw new InvalidOperationException($"A command with the ID '{attribute.Id}' is already registered.");

            IShellCommand commandInstance = CreateCommand(commandType);

            _commands[attribute.Id] = new RegisteredCommand
            {
                Metadata = attribute,
                Implementation = commandInstance,
                Source = commandType.Assembly.GetName().Name ?? "Unknown Assembly"
            };
        }

        private IShellCommand CreateCommand(Type commandType)
        {
            //Exception handling for HelpCommand to pass the registered commands to its constructor
            if (commandType == typeof(HelpCommand))
                return new HelpCommand(() => Commands.Values);

            if (Activator.CreateInstance(commandType) is not IShellCommand commandInstance)
            {
                throw new InvalidOperationException($"The command type '{commandType.FullName}' could not be instantiated as an IShellCommand.");
            }
            return commandInstance;
        }

        public bool TryGetCommand(string commandId, [MaybeNullWhen(false)]out RegisteredCommand registeredCommand)
        {
            return _commands.TryGetValue(commandId, out registeredCommand);
        }
    }
}
