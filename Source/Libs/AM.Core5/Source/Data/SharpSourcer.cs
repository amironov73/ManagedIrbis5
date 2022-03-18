// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* SharpSourcer.cs -- генерирует C#-классы по описанию базы данных.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#endregion

#nullable enable

namespace AM.Data;

/// <summary>
/// Генерирует C#-классы по описанию базы данных.
/// </summary>
public sealed class SharpSourcer
{
    #region Properties

    /// <summary>
    /// Пространство имен.
    /// </summary>
    public string Namespace { get; set; } = "AM.Data.Generated";

    #endregion

    #region Private members

    private static string Capitalize
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        var firstChar = char.ToUpperInvariant (name.FirstChar());

        return name.Length == 1
            ? firstChar.ToString()
            : firstChar + name.Substring (1);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Генерация базы данных по описанию.
    /// </summary>
    public void GenerateDatabase
        (
            TextWriter output,
            DatabaseDescriptor database
        )
    {
        Sure.NotNull (output);
        Sure.VerifyNotNull (database);

        var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration
            (
                SyntaxFactory.IdentifierName (Namespace)
            )
        .AddUsings
            (
                SyntaxFactory.UsingDirective (SyntaxFactory.ParseName ("System"))
            );

        var tables = database.Tables.ThrowIfNull();
        foreach (var table in tables)
        {
            namespaceDeclaration = GenerateClass (namespaceDeclaration, table);
        }

        var unit = SyntaxFactory.CompilationUnit()
            .AddMembers (namespaceDeclaration)
            .NormalizeWhitespace ();

        output.WriteLine (unit.ToFullString());
    }

    /// <summary>
    /// Генерация таблицы по описанию.
    /// </summary>
    public NamespaceDeclarationSyntax GenerateClass
        (
            NamespaceDeclarationSyntax namespaceDeclaration,
            TableDescriptor table
        )
    {
        Sure.NotNull (namespaceDeclaration);
        Sure.NotNull (table);

        var tableName = Capitalize (table.Name.ThrowIfNullOrEmpty());
        var classDeclaration = SyntaxFactory.ClassDeclaration (tableName)
            .AddModifiers
                (
                    SyntaxFactory.Token (SyntaxKind.PublicKeyword),
                    SyntaxFactory.Token (SyntaxKind.SealedKeyword)
                );
        var fields = table.Fields.ThrowIfNull();
        foreach (var field in fields)
        {
            classDeclaration = GenerateField (classDeclaration, field);
        }

        namespaceDeclaration = namespaceDeclaration.AddMembers (classDeclaration);

        return namespaceDeclaration;
    }

    /// <summary>
    /// Генерация поля таблицы по описанию.
    /// </summary>
    public ClassDeclarationSyntax GenerateField
        (
            ClassDeclarationSyntax classDeclaration,
            FieldDescriptor field
        )
    {
        Sure.NotNull (classDeclaration);
        Sure.NotNull (field);

        string typeName;
        switch (field.Type)
        {
            case DataType.Boolean:
                typeName = "bool";
                break;

            case DataType.Integer:
                switch (field.Length)
                {
                    case 0:
                    case 4:
                        typeName = "int";
                        break;

                    case 1:
                        typeName = "byte";
                        break;

                    case 2:
                        typeName = "short";
                        break;

                    case 8:
                        typeName = "long";
                        break;

                    default:
                        throw new ArgumentOutOfRangeException (nameof (field.Length));
                }

                break;

            case DataType.Money:
                typeName = "decimal";
                break;

            case DataType.Text:
                typeName = "string";
                break;

            case DataType.Date:
                typeName = "DateTime";
                break;

            default:
                throw new ArgumentOutOfRangeException (nameof (field.Type));
        }

        var propertyType = SyntaxFactory.ParseTypeName (typeName);
        var fieldName = Capitalize (field.Name.ThrowIfNullOrEmpty());
        var propertyDeclaration = SyntaxFactory.PropertyDeclaration
            (
                propertyType,
                fieldName
            )
        .AddModifiers
            (
                SyntaxFactory.Token (SyntaxKind.PublicKeyword)
            )
        .AddAccessorListAccessors
            (
                SyntaxFactory.AccessorDeclaration
                    (
                        SyntaxKind.GetAccessorDeclaration
                    )
                .WithSemicolonToken
                    (
                        SyntaxFactory.Token (SyntaxKind.SemicolonToken)
                    ),

                SyntaxFactory.AccessorDeclaration
                    (
                        SyntaxKind.SetAccessorDeclaration
                    )
                .WithSemicolonToken
                    (
                        SyntaxFactory.Token (SyntaxKind.SemicolonToken)
                    )
            );

        classDeclaration = classDeclaration.AddMembers (propertyDeclaration);

        return classDeclaration;
    }

    #endregion
}
