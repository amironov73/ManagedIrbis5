// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

#endregion

namespace SourceGenerators
{
    [Generator]
    public sealed class FieldMapperGenerator
        : ISourceGenerator
    {
        #region Constants

        private const string FieldTypeName = "ManagedIrbis.Field";
        private const string MapperAttributeName = "ManagedIrbis.Mapping.FieldMapperAttribute";
        private const string SubFieldAttributeName = "ManagedIrbis.Mapping.SubFieldAttribute";

        #endregion

        #region Private members

        private string ProcessClass
            (
                ITypeSymbol fieldType,
                ITypeSymbol markerType,
                INamedTypeSymbol classSymbol,
                IList<IMethodSymbol> methods
            )
        {
            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            var source = new StringBuilder (
                $@"using ManagedIrbis;

namespace {namespaceName}
{{
    partial class {classSymbol.Name}
    {{
");

            foreach (var method in methods)
            {
                ProcessMethod (fieldType, markerType, source, method);
            }

            source.Append ("} }");
            return source.ToString();
        }

        private void ProcessMethod
            (
                ITypeSymbol fieldType,
                ITypeSymbol markerType,
                StringBuilder source,
                IMethodSymbol method
            )
        {
            var methodName = method.Name;
            var returnType = method.ReturnType;
            var parameters = method.Parameters;
            var modifiers = method.GetModifiers();

            if (parameters.Length != 2)
            {
                return;
            }

            var from = parameters[0];
            var to = parameters[1];

            source.Append
                (
                    $@"

        {modifiers} partial {returnType.ToDisplayString()} {methodName} ("
                );

            source.EnumerateParameters (method);

            source.Append
                (
                    @")
        {
");
            GenerateMapping (source, fieldType, markerType, from, to);

            source.Append
                (
                    $@"
            return {parameters[1].Name};
        }}

"
                );
        }

        private void GenerateMapping
            (
                StringBuilder source,
                ITypeSymbol fieldType,
                ITypeSymbol markerType,
                IParameterSymbol from,
                IParameterSymbol to
            )
        {
            if (from.Type.Equals (fieldType, SymbolEqualityComparer.Default))
            {
                GenerateForwardMapping (markerType, source, from, to);
            }
            else if (to.Type.Equals (fieldType, SymbolEqualityComparer.Default))
            {
                GenerateBackwardMapping (markerType, source, from, to);
            }
        }

        /// <summary>
        /// Преобразование из поля с подполями в структуру данных -- прямое.
        /// </summary>
        private void GenerateForwardMapping
            (
                ITypeSymbol markerType,
                StringBuilder source,
                IParameterSymbol from,
                IParameterSymbol to
            )
        {
            var sourceName = from.Name;
            var targetName = to.Name;
            var targetType = to.Type;
            var properties = targetType.GetProperties();

            foreach (var property in properties)
            {
                var attribute = property.GetAttribute (markerType);
                if (attribute is null || attribute.ConstructorArguments.Length != 1)
                {
                    continue;
                }

                var argument = attribute.ConstructorArguments[0];
                if (argument.Type!.ToDisplayString() != "char")
                {
                    continue;
                }

                // var code = argument.Value!.ToString()[0];
                var code = (char) argument.Value!;
                source.AppendLine ($"{targetName}.{property.Name} = {sourceName}.GetFirstSubFieldValue ('{code}');");
            }
        }

        private void GenerateBackwardMapping
            (
                ITypeSymbol markerType,
                StringBuilder source,
                IParameterSymbol from,
                IParameterSymbol to
            )
        {

        }

        #endregion

        #region ISourceGenerator members

        public void Initialize
            (
                GeneratorInitializationContext context
            )
        {
            context.RegisterForSyntaxNotifications (() => new MethodCollector (MapperAttributeName));
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

            var fieldType = context.Compilation.GetTypeByMetadataName (FieldTypeName)!;
            var markerType = context.Compilation.GetTypeByMetadataName (SubFieldAttributeName)!;
            var types = collector.Collected.GroupBy<IMethodSymbol, INamedTypeSymbol>
                (
                    it => it.ContainingType, SymbolEqualityComparer.Default
                );
            foreach (var group in types)
            {
                var classSource = ProcessClass (fieldType, markerType, group.Key, group.ToList());
                if (!string.IsNullOrEmpty (classSource))
                {
                    context.AddSource
                        (
                            $"{group.Key.Name}_map_field.g.cs",
                            SourceText.From (classSource!, Encoding.UTF8)
                        );
                }
            }
        }

        #endregion
    }
}
