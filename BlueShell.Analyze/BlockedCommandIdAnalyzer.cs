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

        private static readonly DiagnosticDescriptor BlockedIdRule = new DiagnosticDescriptor(
            id: "BS001",
            title: "Blocked Command Id",
            messageFormat: "The command id '{0}' is blocked and cannot be used.",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor EmptyOrWhiteSpaceIdRule = new DiagnosticDescriptor(
            id: "BS002",
            title: "Empty or White Space Command Id",
            messageFormat: "The command id is empty or contains only white space.",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(BlockedIdRule, EmptyOrWhiteSpaceIdRule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(
                GeneratedCodeAnalysisFlags.None);

            context.EnableConcurrentExecution();

            context.RegisterCompilationStartAction(compilationContext =>
            {
                INamedTypeSymbol? commandAttributeSymbol =
                    compilationContext.Compilation.GetTypeByMetadataName(CommandAttributeMetadataName);

                if (commandAttributeSymbol is null)
                    return;

                compilationContext.RegisterSymbolAction(symbolContext => 
                    AnalyzeNamedType(symbolContext, commandAttributeSymbol), 
                    SymbolKind.NamedType);
            });
        }

        private static void AnalyzeNamedType(SymbolAnalysisContext context, INamedTypeSymbol commandAttributeSymbol)
        {
            if (context.Symbol is not INamedTypeSymbol typeSymbol)
                return;

            AttributeData? commandAttribute =
                typeSymbol.GetAttributes()
                    .FirstOrDefault(attribute =>
                        SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, commandAttributeSymbol));

            if (commandAttribute is null)
                return;

            if (commandAttribute.ConstructorArguments.Length == 0)
                return;

            TypedConstant idArgument = commandAttribute.ConstructorArguments[0];

            if (idArgument.Value is not string commandId)
                return;

            Location location = commandAttribute.ApplicationSyntaxReference?.GetSyntax(
                    context.CancellationToken).GetLocation()
                ?? typeSymbol.Locations[0];

            if (string.IsNullOrWhiteSpace(commandId))
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        EmptyOrWhiteSpaceIdRule,
                        location));

                return;
            }

            if (!BlockedIds.Contains(commandId))
                return;

            context.ReportDiagnostic(
                Diagnostic.Create(
                    BlockedIdRule,
                    location,
                    commandId));
        }
    }
}
