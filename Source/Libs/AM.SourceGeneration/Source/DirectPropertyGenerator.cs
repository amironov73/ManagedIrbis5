// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DirectPropertyGenerator.cs -- генерирует Direct-поля Авалонии
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
    /// Генерирует Direct-поля Авалонии.
    /// </summary>
    [Generator]
    public sealed class DirectPropertyGenerator
        : ISourceGenerator
    {
        #region Constants

        private const string AttributeName = "AM.Avalonia.SourceGeneration.DirectPropertyAttribute";

        #endregion

        #region Private members

        private string? ProcessClass
            (
                INamedTypeSymbol classSymbol,
                IList<IFieldSymbol> fields
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
$@"using Avalonia;

namespace {namespaceName}
{{
    partial class {classSymbol.Name}
    {{
");

            foreach (var fieldSymbol in fields)
            {
                ProcessField (classSymbol, source, fieldSymbol);
            }

            source.Append ("} }");
            return source.ToString();
        }

        private void ProcessField
            (
                INamedTypeSymbol classSymbol,
                StringBuilder source,
                IFieldSymbol fieldSymbol
            )
        {
            var className = classSymbol.Name;
            var fieldName = fieldSymbol.Name;
            var fieldType = fieldSymbol.Type;

            var propertyName = Utility.ChooseName (fieldName);
            if (propertyName.Length == 0 || propertyName == fieldName)
            {
                // TODO: issue a diagnostic that we can't process this field
                return;
            }

            source.Append
                (
                    $@"

        public static readonly DirectProperty<{className}, {fieldType}> {propertyName}Property =
            AvaloniaProperty.RegisterDirect<{className}, {fieldType}>
            (
                nameof ({propertyName}),
                o => o.{propertyName},
                (o, v) => o.{propertyName} = v
            );

        public {fieldType} {propertyName}
        {{
            get => this.{fieldName};
            set => SetAndRaise ({propertyName}Property, ref this.{fieldName}, value);
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
            context.RegisterForSyntaxNotifications (() => new FieldCollector (AttributeName));
        }

        /// <inheritdoc cref="ISourceGenerator.Execute"/>
        public void Execute
            (
                GeneratorExecutionContext context
            )
        {
            if (!(context.SyntaxContextReceiver is FieldCollector collector))
            {
                return;
            }

            var types = collector.Collected.GroupBy<IFieldSymbol, INamedTypeSymbol>
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
                            $"{group.Key.Name}_direct_properties.g.cs",
                            SourceText.From (classSource!, Encoding.UTF8)
                        );
                }
            }
        }

        #endregion
    }
}
