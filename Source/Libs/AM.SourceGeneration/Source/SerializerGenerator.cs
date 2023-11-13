// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Local

/* SerializerrGenerator.cs -- генерирует сериализатор
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

#endregion

namespace AM.SourceGeneration
{
    /// <summary>
    /// Генерирует сериализатор.
    /// </summary>
    [Generator]
    public class SerializerGenerator
        : ISourceGenerator
    {
        #region Constants

        internal const string SerializerAttributeName = "AM.Runtime.SerializerAttribute";

        #endregion

        #region Private members

        private string? ProcessClass
            (
                GeneratorExecutionContext context,
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
                $@"namespace {namespaceName}
{{
    partial class {classSymbol.Name}
    {{
");

            foreach (var method in methods)
            {
                ProcessMethod (context, source, method);
            }

            source.Append ("} }");
            return source.ToString();
        }

        private void ProcessMethod
            (
                GeneratorExecutionContext context,
                StringBuilder source,
                IMethodSymbol method
            )
        {
            // проверить сигнатуру метода

            var indent2 = Utility.MakeIndent (2);
            var indent3 = Utility.MakeIndent (3);
            var newLine = Environment.NewLine;
            var methodName = method.Name;
            var modifiers = method.GetModifiers();
            source.Append
                (
                    $"{newLine}{indent2}{modifiers} partial void {methodName} ("
                );

            source.EnumerateParameters (method);

            source.Append ($"){newLine}{indent2}{{{newLine}{indent3}// пока пусто{newLine}{indent2}}}{newLine}");
        }

        #endregion

        #region ISourceGenerator members

        public void Initialize
            (
                GeneratorInitializationContext context
            )
        {
            context.RegisterForSyntaxNotifications (() => new MethodCollector (SerializerAttributeName));
        }

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
                var classSource = ProcessClass (context, group.Key, group.ToList());
                if (!string.IsNullOrEmpty (classSource))
                {
                    context.AddSource
                        (
                            $"{group.Key.Name}_serializer.g.cs",
                            SourceText.From (classSource!, Encoding.UTF8)
                        );
                }
            }
        }

        #endregion
    }
}
