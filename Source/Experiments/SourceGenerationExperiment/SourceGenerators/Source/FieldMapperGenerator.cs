// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Local

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

        internal const string FieldTypeName = "ManagedIrbis.Field";
        internal const string MapperAttributeName = "ManagedIrbis.Mapping.FieldMapperAttribute";
        internal const string SubFieldAttributeName = "ManagedIrbis.Mapping.SubFieldAttribute";

        #endregion

        #region Nested classes

        /// <summary>
        /// Параметры, протаскиваемые через иерархию вызовов.
        /// </summary>
        internal sealed class Bunch
        {
            #nullable disable

            public GeneratorExecutionContext Context { get; set; }

            public StringBuilder Source { get; set; }

            public INamedTypeSymbol Class { get; set; }

            public ITypeSymbol FieldType { get; set; }

            public string MarkerAttributeType { get; set; }

            public IList<IMethodSymbol> Methods { get; set; }

            public IMethodSymbol Method { get; set; }

            public ITypeSymbol FromType { get; set; }

            public string FromName { get; set; }

            public ITypeSymbol ToType { get; set; }

            public string ToName { get; set; }

            #nullable restore
        }

        #endregion

        #region Private members

        private string ProcessClass
            (
                Bunch bunch
            )
        {
            var namespaceName = bunch.Class.ContainingNamespace.ToDisplayString();
            bunch.Source = new StringBuilder
                (
$@"namespace {namespaceName}
{{
    partial class {bunch.Class.Name}
    {{"
                );

            foreach (var method in bunch.Methods)
            {
                bunch.Method = method;
                ProcessMethod (bunch);
            }

            bunch.Source.Append
                (
@"    }
}"
                );
            return bunch.Source.ToString();
        }

        private void ProcessMethod
            (
                Bunch bunch
            )
        {
            var method = bunch.Method;
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

            var source = bunch.Source;
            var indent = NewUtility.MakeIndent (2);
            source.Append
                (
$@"
{indent}{modifiers} partial {returnType.ToDisplayString()} {methodName} ("
                );

            source.EnumerateParameters (method);

            source.AppendLine
                (
@")
        {"
                );
            bunch.FromType = from.Type;
            bunch.FromName = from.Name;
            bunch.ToType = to.Type;
            bunch.ToName = to.Name;
            GenerateMapping (bunch);

            source.AppendLine
                (
                    $@"
            return {parameters[1].Name};
        }}"
                );
        }

        private void GenerateMapping
            (
                Bunch bunch
            )
        {
            if (bunch.FromType.Equals (bunch.FieldType, SymbolEqualityComparer.Default))
            {
                GenerateForwardMapping (bunch);
            }
            else if (bunch.ToType.Equals (bunch.FieldType, SymbolEqualityComparer.Default))
            {
                GenerateBackwardMapping (bunch);
            }
        }

        /// <summary>
        /// Преобразование из поля с подполями в структуру данных -- прямое.
        /// </summary>
        internal void GenerateForwardMapping
            (
                Bunch bunch
            )
        {
            var sourceName = bunch.FromName;
            var targetName = bunch.ToName;
            var targetType = bunch.ToType;
            var properties = targetType.GetProperties();

            foreach (var property in properties)
            {
                var attribute = property.GetAttribute (bunch.MarkerAttributeType);
                if (attribute is null || attribute.ConstructorArguments.Length != 1)
                {
                    continue;
                }

                var argument = attribute.ConstructorArguments[0];
                if (argument.Type!.ToDisplayString() != "char")
                {
                    continue;
                }

                var code = (char) argument.Value!;
                var propertyType = property.Type.GetTypeName();
                var indent = NewUtility.MakeIndent (3);
                bunch.Source.AppendLine ($"{indent}{targetName}.{property.Name} = ManagedIrbis.IrbisConverter.FromString<{propertyType}> ({sourceName}.GetFirstSubFieldValue ('{code}'));");
            }
        }

        /// <summary>
        /// Преобразование из структуры данных в поле с подполями -- обратное.
        /// </summary>
        internal void GenerateBackwardMapping
            (
                Bunch bunch
            )
        {
            var sourceName = bunch.FromName;
            var sourceType = bunch.FromType;
            var targetName = bunch.ToName;
            var properties = sourceType.GetProperties();

            foreach (var property in properties)
            {
                var attribute = property.GetAttribute (bunch.MarkerAttributeType);
                if (attribute is null || attribute.ConstructorArguments.Length != 1)
                {
                    continue;
                }

                var argument = attribute.ConstructorArguments[0];
                if (argument.Type!.ToDisplayString() != "char")
                {
                    continue;
                }

                var code = (char)argument.Value!;
                var propertyName = property.Name;
                var indent = NewUtility.MakeIndent(3);
                bunch.Source.AppendLine ($"{indent}{targetName}.SetSubFieldValue ('{code}', ManagedIrbis.IrbisConverter.ToString ({sourceName}.{propertyName}));");
            }
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

            var bunch = new Bunch
            {
                FieldType = context.Compilation.GetTypeByMetadataName (FieldTypeName)!,
                MarkerAttributeType = SubFieldAttributeName,
            };
            var types = collector.Collected.GroupBy<IMethodSymbol, INamedTypeSymbol>
                (
                    it => it.ContainingType, SymbolEqualityComparer.Default
                );
            foreach (var group in types)
            {
                bunch.Class = group.Key;
                bunch.Methods = group.ToList();
                var classSource = ProcessClass (bunch);
                if (!string.IsNullOrEmpty (classSource))
                {
                    context.AddSource
                        (
                            $"{group.Key.Name}_map_field.g.cs",
                            SourceText.From (classSource, Encoding.UTF8)
                        );
                }
            }
        }

        #endregion
    }
}
