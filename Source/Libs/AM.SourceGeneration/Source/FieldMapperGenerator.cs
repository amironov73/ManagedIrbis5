// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Local

/* FieldMapperGenerator.cs -- генератор маппинга из поля в структуру
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
    /// Генератор маппинга из поля в структуру и обратно.
    /// </summary>
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

            var indent = Utility.MakeIndent (1);
            var newLine = Environment.NewLine;
            bunch.Source.Append
                (
                    $"{indent}}}{newLine}}}"
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
            var indent2 = Utility.MakeIndent (2);
            var indent3 = Utility.MakeIndent (3);
            var newLine = Environment.NewLine;
            source.Append
                (
                    $"{newLine}{indent2}{modifiers} partial {returnType.ToDisplayString()} {methodName} ("
                );

            source.EnumerateParameters (method);

            source.AppendLine
                (
                    $"){newLine}{indent2}{{"
                );
            bunch.FromType = from.Type;
            bunch.FromName = from.Name;
            bunch.ToType = to.Type;
            bunch.ToName = to.Name;
            GenerateMapping (bunch);

            source.AppendLine
                (
                    $"{newLine}{indent3}return {parameters[1].Name};{newLine}{indent2}}}"
                );
        }

        /// <summary>
        /// Выбор направления маппинга.
        /// </summary>
        /// <param name="bunch"></param>
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
            else
            {
                // TODO выдавать диагностику
                throw new Exception();
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
                var indent = Utility.MakeIndent (3);
                var left = $"{indent}{targetName}.{property.Name}";
                var right = $"ManagedIrbis.IrbisConverter.FromString<{propertyType}> (ManagedIrbis.FieldUtility.GetFirstSubFieldValue ({sourceName}, '{code}'))";
                bunch.Source.AppendLine ($"{left} = {right};");
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
                var indent = Utility.MakeIndent(3);
                var left = $"{indent}{targetName}.SetSubFieldValue ('{code}'";
                var right = $"ManagedIrbis.IrbisConverter.ToString ({sourceName}.{propertyName}))";
                bunch.Source.AppendLine ($"{left}, {right};");
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
