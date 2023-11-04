// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Local

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

#endregion

namespace SourceGenerators
{
    [Generator]
    public class RecordMapperGenerator
        : ISourceGenerator
    {
        #region Constants

        private const string RecordTypeName = "ManagedIrbis.Record";
        private const string MapperAttributeName = "ManagedIrbis.Mapping.RecordMapperAttribute";
        private const string FieldAttributeName = "ManagedIrbis.Mapping.FieldAttribute";

        #endregion

        #region Nested classes

        /// <summary>
        /// Параметры, протаскиваемые через иерархию вызовов.
        /// </summary>
        sealed class Bunch
        {
            #nullable disable

            public GeneratorExecutionContext Context { get; set; }

            public StringBuilder Source { get; set; }

            public INamedTypeSymbol Class { get; set; }

            public ITypeSymbol RecordType { get; set; }

            public ITypeSymbol MarkerAttribute { get; set; }

            public IList<IMethodSymbol> Methods { get; set; }

            public IMethodSymbol Method { get; set; }

            public IParameterSymbol From { get; set; }

            public IParameterSymbol To { get; set; }

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
            bunch.From = from;
            bunch.To = to;
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
            if (bunch.From.Type.Equals (bunch.RecordType, SymbolEqualityComparer.Default))
            {
                GenerateForwardMapping (bunch);
            }
            else if (bunch.To.Type.Equals (bunch.RecordType, SymbolEqualityComparer.Default))
            {
                GenerateBackwardMapping (bunch);
            }
        }

        /// <summary>
        /// Преобразование из записи с полями в структуру данных -- прямое.
        /// </summary>
        private void GenerateForwardMapping
            (
                Bunch bunch
            )
        {
            var sourceName = bunch.From.Name;
            var targetName = bunch.To.Name;
            var targetType = bunch.To.Type;
            var properties = targetType.GetProperties();

            foreach (var property in properties)
            {
                var attribute = property.GetAttribute (bunch.MarkerAttribute);
                if (attribute is null || attribute.ConstructorArguments.Length != 2)
                {
                    continue;
                }

                var tagArgument = attribute.ConstructorArguments[0];
                var codeArgument = attribute.ConstructorArguments[1];
                if (tagArgument.Type!.ToDisplayString() != "int"
                    || codeArgument.Type!.ToDisplayString() != "char")
                {
                    continue;
                }

                var tag = (int) tagArgument.Value!;
                var code = (char) codeArgument.Value!;

                var propertyType = property.Type;
                var typeName = propertyType.GetTypeName();
                var indent = NewUtility.MakeIndent(3);
                if (propertyType.IsUserClass())
                {
                    Console.WriteLine ("Это пользовательский класс");
                }
                else if (propertyType.IsArray())
                {
                    Console.WriteLine ("Это массив");
                }
                else if (propertyType.IsList())
                {
                    Console.WriteLine ("Это список");
                }
                else if (propertyType.IsCollection())
                {
                    Console.WriteLine ("Это коллекция");
                }
                else
                {
                    bunch.Source.AppendLine ($"{indent}{targetName}.{property.Name} = ManagedIrbis.IrbisConverter.FromString<{typeName}> ({sourceName}.FM ({tag}, '{code}'));");
                }
            }
        }

        /// <summary>
        /// Преобразование из структуры данных в запись с полями -- обратное.
        /// </summary>
        private void GenerateBackwardMapping
            (
                Bunch bunch
            )
        {
            var sourceName = bunch.From.Name;
            var sourceType = bunch.From.Type;
            var targetName = bunch.To.Name;
            var properties = sourceType.GetProperties();

            foreach (var property in properties)
            {
                var attribute = property.GetAttribute (bunch.MarkerAttribute);
                if (attribute is null || attribute.ConstructorArguments.Length != 2)
                {
                    continue;
                }

                var tagArgument = attribute.ConstructorArguments[0];
                var codeArgument = attribute.ConstructorArguments[1];
                if (tagArgument.Type!.ToDisplayString() != "int"
                    || codeArgument.Type!.ToDisplayString() != "char")
                {
                    continue;
                }

                var tag = (int) tagArgument.Value!;
                var code = (char) codeArgument.Value!;
                var propertyName = property.Name;
                var indent = NewUtility.MakeIndent(3);
                bunch.Source.AppendLine ($"{indent}{targetName}.SetSubFieldValue ({tag}, '{code}', ManagedIrbis.IrbisConverter.ToString ({sourceName}.{propertyName}));");
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
                RecordType = context.Compilation.GetTypeByMetadataName (RecordTypeName)!,
                MarkerAttribute = context.Compilation.GetTypeByMetadataName (FieldAttributeName)!,
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
                            $"{group.Key.Name}_map_record.g.cs",
                            SourceText.From (classSource, Encoding.UTF8)
                        );
                }
            }
        }

        #endregion
    }
}
