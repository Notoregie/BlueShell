using BlueShell.Application.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlueShell.Application.Tests.Parsing
{
    public class CommandParserTests
    {
        [Fact]
        public void TestParseCommandLine()
        {
            string input = "command \"arg1\" \"arg2\"";
            string expectedCommand = "command";
            var expectedArguments = new List<string> { "arg1", "arg2" };

            var commandLine = CommandParser.Parse(input);

            Assert.Equal(expectedCommand, commandLine.Command);
            Assert.Equal(expectedArguments, commandLine.Arguments);
        }

        [Fact]
        public void TestParseCommandLine_WithHelpAlias()
        {
            string input = "command help";
            string expectedCommand = "command";
            var commandLine = CommandParser.Parse(input);
            Assert.Equal(expectedCommand, commandLine.Command);
            Assert.True(commandLine.HelpRequested);
        }
    }
}
