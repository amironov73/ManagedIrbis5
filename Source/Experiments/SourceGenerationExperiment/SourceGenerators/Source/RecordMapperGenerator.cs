// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Local
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
    public class RecordMapperGenerator
        : ISourceGenerator
    {
        #region Constants

        private const string RecordTypeName = "ManagedIrbis.Record";
        private const string FieldTypeName = "ManagedIrbis.Field";
        private const string MapperAttributeName = "ManagedIrbis.Mapping.RecordMapperAttribute";
        private const string FieldAttributeName = "ManagedIrbis.Mapping.FieldAttribute";

        #endregion

        #region Nested classes

        #nullable disable

        sealed class PostponedItem
        {
            public bool IsForward { get; set; }

            public ITypeSymbol Property { get; set; }

            public string MethodName { get; set; }
        }

        /// <summary>
        /// Параметры, протаскиваемые через иерархию вызовов.
        /// </summary>
        sealed class Bunch
        {
            public GeneratorExecutionContext Context { get; set; }

            public StringBuilder Source { get; set; }

            public INamedTypeSymbol Class { get; set; }

            public ITypeSymbol RecordType { get; set; }

            public ITypeSymbol FieldType { get; set; }

            public ITypeSymbol MarkerAttribute { get; set; }

            public IList<IMethodSymbol> Methods { get; set; }

            public IMethodSymbol Method { get; set; }

            public IParameterSymbol From { get; set; }

            public IParameterSymbol To { get; set; }

            public IList<PostponedItem> Postponed { get; set; }
        }

        #nullable restore

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

            HandlePostponed (bunch);

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

        private void HandlePostponed
            (
                Bunch bunch
            )
        {
            var postponed = bunch.Postponed;
            if (postponed is null)
            {
                return;
            }

            foreach (var item in postponed)
            {
                var typeName = item.Property.GetTypeName();
                if (item.IsForward)
                {
                    bunch.Source.AppendLine ($@"
        private static {typeName} {item.MethodName} ({bunch.FieldType} from, {typeName} to)
        {{");
                }
                else
                {
                    bunch.Source.AppendLine ($@"
        private static {bunch.FieldType} {item.MethodName} ({typeName} from, {bunch.FieldType} to)
        {{");
                }

                bunch.Source.AppendLine (
$@"           if  (object.ReferenceEquals (from, null))
            {{
                 return null;
            }}
");

                var subBunch = new FieldMapperGenerator.Bunch
                {
                    Context = bunch.Context,
                    Source = bunch.Source,
                    Class = bunch.Class,
                    FieldType = bunch.FieldType,
                    MarkerAttributeType = FieldMapperGenerator.SubFieldAttributeName,
                    FromName = "from",
                    FromType = bunch.FieldType,
                    ToName = "to",
                    ToType = item.Property,

                };
                if (!item.IsForward)
                {
                    subBunch.FromType = item.Property;
                    subBunch.ToType = bunch.FieldType;
                }

                var subMapper = new FieldMapperGenerator();
                var indent = NewUtility.MakeIndent (3);
                bunch.Source.AppendLine ($"{indent}// code from FieldMapperGenerator");
                if (item.IsForward)
                {
                    subMapper.GenerateForwardMapping (subBunch);

                }
                else
                {
                    subMapper.GenerateBackwardMapping (subBunch);
                }
                bunch.Source.AppendLine
                    (
                        @"
            return to;
        }");
            }
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

            bunch.Postponed = new List<PostponedItem>();
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

                var tag = (int)tagArgument.Value!;
                var code = (char)codeArgument.Value!;

                var propertyType = property.Type;
                var typeName = propertyType.GetTypeName();
                var indent = NewUtility.MakeIndent (3);
                if (propertyType.IsUserClass())
                {
                    var postpone = new PostponedItem
                    {
                        IsForward = true,
                        Property = propertyType,
                        MethodName = $"_Convert_{bunch.Postponed.Count}"
                    };
                    bunch.Postponed.Add (postpone);

                    bunch.Source.AppendLine ($"{indent}{targetName}.{property.Name} = {postpone.MethodName} ({sourceName}.GetField ({tag}), new {typeName}());");
                }
                else if (propertyType.IsArray())
                {
                    var array = (IArrayTypeSymbol) propertyType;
                    var elementType = array.ElementType;
                    var elementName = elementType.GetTypeName();
                    bunch.Source.AppendLine ($"{indent}{targetName}.{property.Name} = ManagedIrbis.IrbisConverter.ArrayFromStrings<{elementName}> ({sourceName}.FMA ({tag}, '{code}'));");
                }
                else if (propertyType.IsList())
                {
                    var named = (INamedTypeSymbol) propertyType;
                    var elementType = named.TypeArguments[0];
                    var elementName = elementType.GetTypeName();
                    if (elementType.IsUserClass())
                    {
                        var postpone = new PostponedItem
                        {
                            IsForward = true,
                            Property = elementType,
                            MethodName = $"_Convert_{bunch.Postponed.Count}"
                        };
                        bunch.Postponed.Add (postpone);

                        bunch.Source.AppendLine ($"{indent}{targetName}.{property.Name} = ManagedIrbis.IrbisConverter.ListFromFields<{elementName}> ({sourceName}.GetFields ({tag}), {postpone.MethodName});");
                    }
                    else
                    {
                        bunch.Source.AppendLine ($"{indent}{targetName}.{property.Name} = ManagedIrbis.IrbisConverter.ListFromStrings<{elementName}> ({sourceName}.FMA ({tag}, '{code}'));");
                    }
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

                var tag = (int)tagArgument.Value!;
                var code = (char)codeArgument.Value!;
                var propertyType = property.Type;
                var propertyName = property.Name;
                var indent = NewUtility.MakeIndent (3);
                if (propertyType.IsUserClass())
                {
                    var postpone = new PostponedItem
                    {
                        IsForward = false,
                        Property = propertyType,
                        MethodName = $"_Convert_{bunch.Postponed.Count}"
                    };
                    bunch.Postponed.Add (postpone);

                    bunch.Source.AppendLine ($"{indent}{targetName}.SetField ({tag}, {postpone.MethodName} ({sourceName}.{propertyName}, new {bunch.FieldType}() {{ Tag = {tag} }}));");
                }
                else if (propertyType.IsArray())
                {
                    var array = (IArrayTypeSymbol) propertyType;
                    var elementType = array.ElementType;
                    var elementName = elementType.GetTypeName();
                    bunch.Source.AppendLine ($"{indent}{targetName}.SetField ({tag}, ManagedIrbis.IrbisConverter.ToStrings<{elementName}> ({sourceName}.{propertyName}));");
                }
                else if (propertyType.IsList())
                {
                    var named = (INamedTypeSymbol) propertyType;
                    var elementType = named.TypeArguments[0];
                    var elementName = elementType.GetTypeName();
                    if (elementType.IsUserClass())
                    {
                        var postpone = new PostponedItem
                        {
                            IsForward = false,
                            Property = elementType,
                            MethodName = $"_Convert_{bunch.Postponed.Count}"
                        };
                        bunch.Postponed.Add (postpone);

                        bunch.Source.AppendLine ($"{indent}{targetName}.SetField ({tag}, ManagedIrbis.IrbisConverter.FieldsFromList<{elementName}> ({sourceName}.{propertyName}, {postpone.MethodName}));");
                    }
                    else
                    {
                        bunch.Source.AppendLine ($"{indent}{targetName}.SetField ({tag}, ManagedIrbis.IrbisConverter.ToStrings<{elementName}> ({sourceName}.{propertyName}));");
                    }
                }
                else
                {
                    bunch.Source.AppendLine ($"{indent}{targetName}.SetSubFieldValue ({tag}, '{code}', ManagedIrbis.IrbisConverter.ToString ({sourceName}.{propertyName}));");
                }
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
                FieldType = context.Compilation.GetTypeByMetadataName (FieldTypeName)!,
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
