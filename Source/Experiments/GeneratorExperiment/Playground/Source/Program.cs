// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;

#endregion

#nullable enable

namespace Playground;

class PropInfo
{
    public string? Name { get; set; }
    public string? Type { get; set; }
}

class Program
{
    static PropInfo[] GetProperties
        (
            SemanticModel model,
            INamedTypeSymbol theClass
        )
    {
        var result = new List<PropInfo>();
        var properties = theClass.GetMembers().OfType<IPropertySymbol>();
        foreach (var one in properties)
        {
            var property = new PropInfo();
            result.Add (property);
            property.Name = one.Name;
            property.Type = one.Type.Name;

        }

        return result.ToArray();
    }

    static void GenerateCode
        (
            SemanticModel model,
            TypeDeclarationSyntax theType
        )
    {
        Console.WriteLine (new string ('=', 70));

        var theClass = model.GetDeclaredSymbol(theType);
        if (theClass is null)
        {
            return;
        }

        var properties = GetProperties (model, theClass);

        var theNamespace = string.Empty;
        if (!theClass.ContainingNamespace.IsGlobalNamespace)
        {
            theNamespace = theClass.ContainingNamespace.ToString();
        }

        if (!string.IsNullOrEmpty (theNamespace))
        {
            Console.WriteLine ($"namespace {theNamespace} {{");
            Console.WriteLine ($"\tpartial class {theClass.Name} {{");

            Console.WriteLine ("\t\tpartial void FromField(Field field) {");
            foreach (var property in properties)
            {
                Console.WriteLine ($"\t\t\t{property.Type} {property.Name} = default;");
            }
            Console.WriteLine ("\t\t}");

            Console.WriteLine ("\t\tpartial void ToField(Field field) {");
            foreach (var property in properties)
            {
                Console.WriteLine ($"\t\t\t{property.Name} = default;");
            }
            Console.WriteLine ("\t\t}");

            Console.WriteLine ("\t}");
            Console.WriteLine ("}");
        }

        Console.WriteLine (new string ('=', 70));
    }

    static string GetFullName
        (
            SemanticModel model,
            SyntaxNode node
        )
    {
        var typeInfo = model.GetTypeInfo (node);
        var type = typeInfo.Type;
        if (type is null)
        {
            return "?";
        }

        var format = new SymbolDisplayFormat
            (
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces
            );

        return type.ToDisplayString (format);

        // The reason your getting keywords like "int" is the default format is including
        // the SymbolDisplayMiscellaneousOptions.UseSpecialTypes flag which specifies
        // to use the language keywords for special types vs. the regular name.

        //
        // return type.ContainingNamespace.IsGlobalNamespace
        //     ? type.Name
        //     : $"{type.ContainingNamespace}.{type.Name}";
    }

    static void Main()
    {
        var tree = CSharpSyntaxTree.ParseText (File.ReadAllText ("PseudoProgram.txt"));
        var root = tree.GetCompilationUnitRoot();
        var compilation = CSharpCompilation.Create ("PseudoProgram")
            .AddReferences
                (
                    MetadataReference.CreateFromFile (typeof (object).Assembly.Location),
                    MetadataReference.CreateFromFile (Assembly.Load ("System.Runtime").Location),
                    MetadataReference.CreateFromFile (typeof (Console).Assembly.Location)
                )
            .AddSyntaxTrees (tree);
        var semanticModel = compilation.GetSemanticModel (tree);
        var classes = root.DescendantNodes().OfType<TypeDeclarationSyntax>().ToArray();
        TypeDeclarationSyntax? found = null;
        foreach (var theClass in classes)
        {
            // var namedTypeSymbol = semanticModel.GetDeclaredSymbol(theClass);
            // if (namedTypeSymbol is null)
            // {
            //     continue;
            // }
            //
            Console.WriteLine();
            Console.WriteLine ($"CLASS {theClass.Identifier}");

            // var attributes1 = namedTypeSymbol.GetAttributes();
            // foreach (var attribute1 in attributes1)
            // {
            //     var attributeClass = attribute1.AttributeClass;
            //     if (attributeClass is not null)
            //     {
            //         var name = attributeClass.Name;
            //         Console.WriteLine($"\t{name}");
            //     }
            // }

            var attributes2 = theClass.AttributeLists
                .SelectMany(o => o.Attributes)
                .ToArray();

            foreach (var attribute2 in attributes2)
            {
                var fullName = GetFullName (semanticModel, attribute2);
                Console.WriteLine ($"\t{fullName}");
                if (typeof (GenerateAccessorAttribute).FullName == fullName)
                {
                    GenerateCode (semanticModel, theClass);
                }
            }
        }

        // var stream = new MemoryStream();
        // var emitResult = compilation.Emit(stream);
        // if (!emitResult.Success)
        // {
        //     var failures = emitResult.Diagnostics.Where
        //     (
        //         diagnostic => diagnostic.IsWarningAsError
        //                       || diagnostic.Severity == DiagnosticSeverity.Error
        //     );
        //
        //     foreach (var failure in failures)
        //     {
        //         Console.WriteLine($"{failure.Id}: {failure.GetMessage()}");
        //     }
        //
        // }

    }
}


[AttributeUsage(AttributeTargets.Class)]
public sealed class GenerateAccessorAttribute
    : Attribute
{
    // пустое тело класса
}
