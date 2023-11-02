// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* NotifyGenerator.cs -- генерирует свойство с поддержкой INotifyPropertyChanged
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
    /// Генерирует свойство с поддержкой INotifyPropertyChanged.
    /// </summary>
    [Generator]
    public sealed class NotifyGenerator
        : ISourceGenerator
    {
        #region Constants

        private const string AttributeName = "AM.SourceGeneration.NotifyAttribute";

        private const string AttributeText = @"using System;

namespace AM.SourceGeneration
{
    [AttributeUsage (AttributeTargets.Field)]
    internal sealed class NotifyAttribute: Attribute
    {
    }
}
";

        #endregion

        #region Private members

        private string? ProcessClass
            (
                INamedTypeSymbol classSymbol,
                INamedTypeSymbol notifySymbol,
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
            var needDeclare = !classSymbol.Interfaces.Contains (notifySymbol, SymbolEqualityComparer.Default);
            var interfaceDeclaration = needDeclare
                ? $" : {notifySymbol.ToDisplayString()}"
                : string.Empty;

            var source = new StringBuilder (
$@"namespace {namespaceName}
{{
    partial class {classSymbol.Name}{interfaceDeclaration}
    {{
");

            if (needDeclare)
            {
                source.Append
                    (
"        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;"
                    );
            }

            foreach (var fieldSymbol in fields)
            {
                ProcessField (source, fieldSymbol);
            }

            source.Append ("} }");
            return source.ToString();
        }

        private void ProcessField
            (
                StringBuilder source,
                IFieldSymbol fieldSymbol
            )
        {
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

        public {fieldType} {propertyName}
        {{
            get
            {{
                return this.{fieldName};
            }}

            set
            {{
                this.{fieldName} = value;
                this.PropertyChanged?.Invoke (this, new System.ComponentModel.PropertyChangedEventArgs (nameof ({propertyName})));
            }}
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
                    i => i.AddSource ("NotifyAttribute.g.cs", AttributeText)
                );

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

            var notifySymbol = context.Compilation.GetTypeByMetadataName ("System.ComponentModel.INotifyPropertyChanged")!;
            var types = collector.Collected.GroupBy<IFieldSymbol, INamedTypeSymbol>
                (
                    it => it.ContainingType, SymbolEqualityComparer.Default
                );
            foreach (var group in types)
            {
                var classSource = ProcessClass (group.Key, notifySymbol, group.ToList());
                if (!string.IsNullOrEmpty (classSource))
                {
                    context.AddSource
                        (
                            $"{group.Key.Name}_notify.g.cs",
                            SourceText.From (classSource!, Encoding.UTF8)
                        );
                }
            }
        }

        #endregion
    }
}
