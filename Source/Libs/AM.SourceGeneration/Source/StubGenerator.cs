// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* StubGenerator.cs -- генерирует заглушку для метода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

#endregion

namespace AM.SourceGeneration
{
    /// <summary>
    /// Генерирует заглушку для метода.
    /// </summary>
    [Generator]
    public sealed class StubGenerator
        : ISourceGenerator
    {
        #region Constants

        private const string AttributeName = "AM.SourceGeneration.StubAttribute";

        private const string AttributeText = @"using System;

namespace AM.SourceGeneration
{
    [AttributeUsage (AttributeTargets.Method)]
    internal sealed class StubAttribute: Attribute
    {
    }
}
";

        #endregion

        #region Private members

        private string? ProcessClass
            (
                INamedTypeSymbol classSymbol,
                IList<IMethodSymbol> methods
            )
        {
            if (!classSymbol.ContainingSymbol.Equals
                    (
                        classSymbol.ContainingNamespace,
                        SymbolEqualityComparer.Default
                    ))
            {
                // оказались вне пространства имен, это странно
                return null;
            }

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

            var source = new StringBuilder (
                $@"
namespace {namespaceName}
{{
    partial class {classSymbol.Name}
    {{
");

            foreach (var method in methods)
            {
                ProcessMethod (source, method);
            }

            source.Append ("} }");
            return source.ToString();
        }

        private void ProcessMethod
            (
                StringBuilder source,
                IMethodSymbol method
            )
        {
            var methodName = method.Name;
            var returnType = method.ReturnType;
            var parameters = method.Parameters;
            var accessibility = method.DeclaredAccessibility;

            var modifiers = string.Empty;
            switch (accessibility)
            {
                case Accessibility.Public:
                    modifiers += " public";
                    break;

                case Accessibility.Private:
                    modifiers += " private";
                    break;

                case Accessibility.Protected:
                    modifiers += " protected";
                    break;

                case Accessibility.Internal:
                    modifiers += "internal";
                    break;
            }

            if (method.IsStatic)
            {
                modifiers += " static";
            }

            source.Append
                (
                    $@"

        {modifiers} partial {returnType.ToDisplayString()} {methodName} ("
                );

            var first = true;
            foreach (var parameter in parameters)
            {
                if (!first)
                {
                    source.Append (", ");
                }

                source.Append ($"{parameter.Type.ToDisplayString()} {parameter.Name}");

                first = false;
            }

            source.Append
                (
                    $@")
        {{
            return default({returnType.ToDisplayString ()});
        }}

"
                );
        }

        #endregion

        #region ISourceGenerator members


        /// <inheritdoc cref="ISourceGenerator.Initialize"/>
        public void Initialize
            (
                GeneratorInitializationContext context
            )
        {
            context.RegisterForPostInitialization
                (
                    it => it.AddSource
                        (
                            "StubAttribute.g.cs",
                            AttributeText
                        )
                );

            context.RegisterForSyntaxNotifications (() => new MethodCollector (AttributeName));
        }

        /// <inheritdoc cref="ISourceGenerator.Execute"/>
        public void Execute
            (
                GeneratorExecutionContext context
            )
        {
            if (!(context.SyntaxContextReceiver is MethodCollector collector))
            {
                return;
            }

            var types = collector.Collected.GroupBy<IMethodSymbol, INamedTypeSymbol>
                (
                    it => it.ContainingType, SymbolEqualityComparer.Default
                );
            foreach (var group in types)
            {
                var classSource = ProcessClass (group.Key, group.ToList());
                if (!string.IsNullOrEmpty (classSource))
                {
                    context.AddSource
                        (
                            $"{group.Key.Name}_stub.g.cs",
                            SourceText.From (classSource!, Encoding.UTF8)
                        );
                }
            }
        }

        #endregion
    }
}
