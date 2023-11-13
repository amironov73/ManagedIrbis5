// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* BinaryInclusionGenerator.cs -- генерирует массивы двоичных данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

#endregion

namespace AM.SourceGeneration
{
    /// <summary>
    /// Генерирует массивы двоичных данных.
    /// </summary>
    [Generator]
    public sealed class BinaryInclusionGenerator
        : ISourceGenerator
    {
        #region Constants

        private const string AttributeName = "AM.SourceGeneration.BinaryInclusionAttribute";

        private const string AttributeText = @"using System;

namespace AM.SourceGeneration
{
    [AttributeUsage (AttributeTargets.Method)]
    internal sealed class BinaryInclusionAttribute: Attribute
    {
        public string ContentFileName { get; }

        public BinaryInclusionAttribute (string contentFileName)
        {
            ContentFileName = contentFileName;
        }
    }
}
";

        #endregion

        #region Private members

        private static int _counter;

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
            var methodName = method.Name;
            var returnType = method.ReturnType;
            var accessibility = method.DeclaredAccessibility;
            var attribute = method.GetAttribute (AttributeName)!;
            var fileName = attribute.ConstructorArguments[0].Value as string;
            if (string.IsNullOrEmpty (fileName))
            {
                return;
            }

            var fullName = Path.GetFullPath (fileName);

            // TODO: проверять, что возвращаемый тип byte[]

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

            var variableName = $"_binaryContent{_counter++}";
            source.Append
                (
                    $@"
        private readonly static byte[] {variableName} = new byte[] {{ "
                );

            source.Append ("0x31, 0x32, 0x33, 0x34");
            //var bytes = File.ReadAllBytes (fileName);
            //var first = true;
            //foreach (var oneByte in bytes)
            //{
            //    if (!first)
            //    {
            //        source.Append (", ");
            //    }

            //    source.Append ($"{oneByte}");

            //    first = false;
            //}

            source.Append
                (
                    @" };
"
                );

            source.Append
                (
                    $@"

        {modifiers} partial {returnType.ToDisplayString()} {methodName} ()
        {{
            // {fullName}
            return {variableName};
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
                    i => i.AddSource ("BinaryInclusionAttribute.g.cs", AttributeText)
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
                var classSource = ProcessClass (context, group.Key, group.ToList());
                if (!string.IsNullOrEmpty (classSource))
                {
                    context.AddSource
                        (
                            $"{group.Key.Name}_binary_inclusion.g.cs",
                            SourceText.From (classSource!, Encoding.UTF8)
                        );
                }
            }
        }

        #endregion
    }
}
