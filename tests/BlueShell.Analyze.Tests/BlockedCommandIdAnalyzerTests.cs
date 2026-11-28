using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace BlueShell.Analyze.Tests
{
    public class BlockedCommandIdAnalyzerTests
    {
        [Fact]
        public async Task ReportsDiagnostic_WhenCommandIsBlocked()
        {
            var test = new CSharpAnalyzerTest<BlockedCommandIdAnalyzer, DefaultVerifier>
            {
                TestCode = """
                
                    using System;
                    using System.Collections.Generic;
                    using System.Text;

                    namespace BlueShell.Core.Attributes
                    {
                        [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
                        public class CommandAttribute : Attribute
                        {
                            public CommandAttribute(string id, string description, string usage)
                            {
                                Id = id;
                                Description = description;
                                Usage = usage;
                            }

                            public string Id { get; }
                            public string Description { get; }
                            public string Usage { get; }
                        }
                    }
                    namespace TestProject
                    {
                        using BlueShell.Core.Attributes;

                        [{|BS001:Command("help", "Test command", "help")|}]
                        public class HelpCommand
                        {
                        }
                    }
                """
            };

            await test.RunAsync();
        }

        [Fact]
        public async Task ReportsDiagnostic_WhenCommandIsEmptyOrWhiteSpace()
        {
            var test = new CSharpAnalyzerTest<BlockedCommandIdAnalyzer, DefaultVerifier>
            {
                TestCode = """
                
                    using System;
                    using System.Collections.Generic;
                    using System.Text;

                    namespace BlueShell.Core.Attributes
                    {
                        [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
                        public class CommandAttribute : Attribute
                        {
                            public CommandAttribute(string id, string description, string usage)
                            {
                                Id = id;
                                Description = description;
                                Usage = usage;
                            }

                            public string Id { get; }
                            public string Description { get; }
                            public string Usage { get; }
                        }
                    }
                    namespace TestProject
                    {
                        using BlueShell.Core.Attributes;

                        [{|BS002:Command("", "no description", "no usage")|}]
                        public class HelpCommand
                        {
                        }
                    }
                """
            };

            await test.RunAsync();
        }
    }
}
