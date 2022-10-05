// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#endregion

namespace ManagedIrbis.Generators;

[Generator]
public class MyGenerator
    : ISourceGenerator
{
    #region ISourceGenerator members

    public void Initialize(GeneratorInitializationContext context)
    {
        // Nothing to do here
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // foreach (var syntaxTree in context.Compilation.SyntaxTrees)
        // {
        //     ProcessSyntaxTree(context, syntaxTree);
        // }
    }

    #endregion

    #region Private members

    private static bool HasAttribute
        (
            GeneratorExecutionContext context,
            SemanticModel semanticModel,
            ClassDeclarationSyntax theClass
        )
    {
        var desiredSymbol = context.Compilation.GetTypeByMetadataName
            (
                typeof (GenerateAccessorAttribute).FullName!
            );
        var classModel = semanticModel.GetDeclaredSymbol (theClass)!;
        foreach (var attribute in classModel.GetAttributes())
        {
            if (ReferenceEquals (attribute.AttributeClass, desiredSymbol))
            {
                return true;
            }
        }

        return false;
    }

    private void GenerateMethods
        (
                GeneratorExecutionContext context,
                SemanticModel semanticModel,
                ClassDeclarationSyntax theClass
        )
    {
        var classModel = semanticModel.GetDeclaredSymbol (theClass)!;
        var source = $@"
using System;
using ManagedIrbis;

namespace {classModel.Name}
";
    }

    private void ProcessSyntaxTree
        (
            GeneratorExecutionContext context,
            SyntaxTree syntaxTree
        )
    {
        var semantic = context.Compilation.GetSemanticModel (syntaxTree);
        var root = syntaxTree.GetCompilationUnitRoot();
        var choosen = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
            .Where (type => HasAttribute (context, semantic, type))
            .ToArray();

        foreach (var theClass in choosen)
        {
            GenerateMethods (context, semantic, theClass);
        }
    }

    #endregion
}

