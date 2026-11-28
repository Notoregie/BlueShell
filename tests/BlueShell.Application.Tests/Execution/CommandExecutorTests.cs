using BlueShell.Commands;
using BlueShell.Core.Attributes;
using BlueShell.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using BlueShell.Core.Interfaces;
using BlueShell.Core.State;
using BlueShell.Application.Execution;

namespace BlueShell.Application.Tests.Execution
{
    public class CommandExecutorTests
    {
        [Fact]
        public void TestExecuteCommand_WhenInsufficientArgumentsProvided_DoesNotExecuteCommand()
        {
            ShellContext context = new ShellContext();
            CommandLine commandLine = new CommandLine("test", new string[] { "arg1" }, false);
            var mockCommand = new Mock<IShellCommand>();
            mockCommand.SetupGet(command => command.MinArgs)
                .Returns(2);

            RegisteredCommand registeredCommand = new RegisteredCommand
            {
                Implementation = mockCommand.Object,
                Metadata = new CommandAttribute("test", "Test command", "test <arg1> <arg2>"),
                Source = "test.dll"
            };

            CommandExecutor.ExecuteCommand(registeredCommand, context, commandLine);

            mockCommand.Verify(command => command.Execute(
                It.IsAny<ShellContext>(), 
                It.IsAny<string[]>())
            ,Times.Never);

        }
        [Fact]
        public void TestExecuteCommand_WhenHelpIsRequested_UsesDetailedHelp()
        {
            ShellContext context = new ShellContext();
            CommandLine commandLine = new CommandLine("test", [], true);
            CommandHelp detailedHelp = new CommandHelp("usage", "description");

            var mockCommand = new Mock<IShellCommand>();
            mockCommand.SetupGet(command => command.MinArgs)
                .Returns(2);

            var detailedHelpProviderMock = mockCommand.As<IDetailedHelpProvider>();
            detailedHelpProviderMock.Setup(helpProvider => helpProvider.GetDetailedHelp())
                .Returns(detailedHelp);

            RegisteredCommand registeredCommand = new RegisteredCommand
            {
                Implementation = mockCommand.Object,
                Metadata = new CommandAttribute("test", "Test command", "test <arg1> <arg2>"),
                Source = "test.dll"
            };

            CommandExecutor.ExecuteCommand(registeredCommand, context, commandLine);

            detailedHelpProviderMock.Verify(help => help.GetDetailedHelp(), Times.Once);

            mockCommand.Verify(command => command.Execute(
                It.IsAny<ShellContext>(), 
                It.IsAny<string[]>())
            ,Times.Never);

        }
    }
}
