using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace BlueShell.Analyze
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class BlockedCommandIdAnalyzer : DiagnosticAnalyzer
    {
        private const string CommandAttributeMetadataName = "BlueShell.Core.Attributes.CommandAttribute";

        private static readonly ImmutableHashSet<string> BlockedIds =
        ImmutableHashSet.Create(
            StringComparer.OrdinalIgnoreCase,
            "help",
            "exit",
            "system"
        );

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: "BS001",
            title: "Blocked Command Id",
            messageFormat: "The command id '{0}' is blocked and cannot be used.",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(
                GeneratedCodeAnalysisFlags.None);

            context.EnableConcurrentExecution();

            context.RegisterSymbolAction(
                AnalyzeNamedType,
                SymbolKind.NamedType);
        }

        private static void AnalyzeNamedType(SymbolAnalysisContext context)
        {
            if (context.Symbol is not INamedTypeSymbol typeSymbol)
                return;

            AttributeData? commandAttribute =
                typeSymbol.GetAttributes()
                    .FirstOrDefault(attribute =>
                        string.Equals(
                            attribute.AttributeClass?.ToDisplayString(),
                            CommandAttributeMetadataName,
                            StringComparison.Ordinal));

            if (commandAttribute is null)
                return;

            if (commandAttribute.ConstructorArguments.Length == 0)
                return;

            TypedConstant idArgument = commandAttribute.ConstructorArguments[0];

            if (idArgument.Value is not string commandId)
                return;

            if (!BlockedIds.Contains(commandId))
                return;

            Location location = commandAttribute.ApplicationSyntaxReference?.GetSyntax(
                    context.CancellationToken).GetLocation()
                ?? typeSymbol.Locations[0];

            context.ReportDiagnostic(
                Diagnostic.Create(
                    Rule,
                    location,
                    commandId));
        }
    }
}
