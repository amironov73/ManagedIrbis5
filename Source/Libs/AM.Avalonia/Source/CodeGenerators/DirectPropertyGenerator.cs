// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DirectPropertyGenerator.cs -- генератор для регистрации Direct-свойств
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#endregion

namespace AM.Avalonia.CodeGenerators;

/// <summary>
/// Генератор для регистрации Direct-свойств.
/// </summary>
[PublicAPI]
public sealed class DirectPropertyGenerator
    : ISourceGenerator
{
    #region Constants

    /// <summary>
    ///
    /// </summary>
    public const string RegisterDirectPropertyAttributeName =
        "AM.Avalonia.CodeGenerators.Attributes.RegisterDirectPropertyAttribute";

    /// <summary>
    ///
    /// </summary>
    public const string AvaloniaControlClassName = "Avalonia.Controls.Control";

    #endregion

    #region Nested classes

    class SyntaxReceiver
        : ISyntaxContextReceiver
    {
        public List<IPropertySymbol> Properties { get; } = new ();

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
        /// </summary>
        public void OnVisitSyntaxNode (GeneratorSyntaxContext context)
        {
            // any field with at least one attribute is a candidate for property generation
            if (context.Node is PropertyDeclarationSyntax propertyDeclaration &&
                propertyDeclaration.AttributeLists.Count > 0)
            {
                // Get the symbol being declared by the field, and keep it if its annotated
                var propertySymbol = context.SemanticModel.GetDeclaredSymbol (propertyDeclaration) as IPropertySymbol;

                if (propertySymbol is not null
                    && propertySymbol.GetAttributes().Any (ad =>
                        ad.AttributeClass?.ToDisplayString() == RegisterDirectPropertyAttributeName))
                {
                    Properties.Add (propertySymbol);
                }
            }
        }
    }

    #endregion

    #region Private members

    private string GenerateDirectPropertyRegistrationClassSource
        (
            INamedTypeSymbol classSymbol,
            List<IPropertySymbol> properties,
            ISymbol attributeSymbol,
            ISymbol avaloniaControlSymbol,
            GeneratorExecutionContext context
        )
    {
        var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
        var className = classSymbol.Name;

        var propertyRegistrations =
            properties.Select (f => GenerateDirectPropertyRegistration (className, f.Name, f.Type.ToString()));

        return $@"
using Avalonia;

namespace {namespaceName}
{{
    public partial class {className}
    {{
        {string.Join ("\r\n", propertyRegistrations)}
    }}
}}
";
    }

    private string GenerateDirectPropertyRegistration
        (
            string className,
            string propertyName,
            string propertyType
        )
    {
        var directPropertyName = $"{propertyName}Property";

        return $@"
        public static readonly DirectProperty<{className}, {propertyType}> {directPropertyName} =
            AvaloniaProperty.RegisterDirect<{className}, {propertyType}>(
                nameof({propertyName}),
                o => o.{propertyName},
                (o, v) =>
                {{
                    var current = o.{propertyName};
                    o.SetAndRaise({directPropertyName}, ref current, v);
                    if (o.{propertyName} != current)
                        o.On{propertyName}Changed(o.{propertyName}, current);
                    o.{propertyName} = current;
                }}
            )
        ;
";
    }

    #endregion

    #region ISourceGenerator members

    /// <inheritdoc cref="ISourceGenerator.Initialize"/>
    public void Initialize
        (
            GeneratorInitializationContext context
        )
    {
        context.RegisterForSyntaxNotifications (() => new SyntaxReceiver());
    }

    /// <inheritdoc cref="ISourceGenerator.Execute"/>
    public void Execute
        (
            GeneratorExecutionContext context
        )
    {
        if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
        {
            return;
        }

        var attributeSymbol = context.Compilation.GetTypeByMetadataName (RegisterDirectPropertyAttributeName);
        var avaloniaControlSymbol = context.Compilation.GetTypeByMetadataName (AvaloniaControlClassName);

        foreach (var group in receiver.Properties.GroupBy (f => f.ContainingType))
        {
            var source = GenerateDirectPropertyRegistrationClassSource (group.Key, group.ToList(), attributeSymbol,
                avaloniaControlSymbol, context);
            context.AddSource ($@"{group.Key.Name}.DirectProperties.cs", source);
        }
    }

    #endregion
}
