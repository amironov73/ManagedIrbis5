// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Utility.cs -- полезные методы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#endregion

namespace SourceGenerators
{
    /// <summary>
    /// Полезные методы.
    /// </summary>
    internal static class Utility
    {
        #region Private members

        private static bool ContainsAttribute
            (
                AttributeData attributeData,
                string attributeName
            )
        {
            var attributeClass = attributeData.AttributeClass;
            if (attributeClass is null)
            {
                return false;
            }

            return StringComparer.Ordinal.Equals (attributeClass.ToDisplayString(), attributeName);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Выбор имени для свойства по имени поля класса.
        /// </summary>
        public static string ChooseName
            (
                string fieldName
            )
        {
            var result = fieldName.TrimStart ('_');
            return result.Length switch
            {
                0 => string.Empty,
                1 => result.ToUpper(),
                _ => result.Substring (0, 1).ToUpper()
                     + result.Substring (1)
            };
        }

        /// <summary>
        /// Определен ли указанный атрибут у данного символа?
        /// </summary>
        public static bool HaveAttribute
            (
                this ISymbol symbol,
                string attributeName
            )
            => symbol.GetAttributes().Any (x => ContainsAttribute (x, attributeName));

        /// <summary>
        /// Получение базовых типов плюс текущего в виде последовательности.
        /// </summary>
        public static IEnumerable<ITypeSymbol> GetBaseTypesAndThis
            (
                this ITypeSymbol type
            )
        {
            var current = type;
            while (!(current is null))
            {
                yield return current;
                current = current.BaseType;
            }
        }

        public static IEnumerable<ISymbol> GetAllMembers
            (
                this ITypeSymbol type
            )
            => type.GetBaseTypesAndThis().SelectMany (n => n.GetMembers());

        public static CompilationUnitSyntax GetCompilationUnit
            (
                this SyntaxNode syntaxNode
            )
            => syntaxNode.Ancestors().OfType<CompilationUnitSyntax>().First();

        public static string GetClassName
            (
                this ClassDeclarationSyntax proxy
            )
            => proxy.Identifier.Text;

        public static string GetClassModifier
            (
                this ClassDeclarationSyntax proxy
            )
            => proxy.Modifiers.ToFullString().Trim();

        public static string GetNamespace
            (
                this CompilationUnitSyntax root
            )
            => root.ChildNodes()
                .OfType<NamespaceDeclarationSyntax>()
                .First()
                .Name
                .ToString();

        public static List<string> GetUsings
            (
                this CompilationUnitSyntax root
            )
            => root.ChildNodes()
                .OfType<UsingDirectiveSyntax>()
                .Select(n => n.Name.ToString())
                .ToList();

        #endregion
    }
}
