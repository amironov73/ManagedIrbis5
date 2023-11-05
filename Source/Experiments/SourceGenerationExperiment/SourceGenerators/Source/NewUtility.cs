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
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#endregion

namespace SourceGenerators
{
    /// <summary>
    /// Полезные методы.
    /// </summary>
    internal static class NewUtility
    {
        /// <summary>
        /// Получение имени типа.
        /// Удаляет признак nullable.
        /// </summary>
        public static string GetTypeName (this ITypeSymbol symbol)
            => symbol.ToDisplayString().TrimEnd ('?');

        /// <summary>
        /// Проверка, не массив ли это.
        /// </summary>
        public static bool IsArray (this ITypeSymbol symbol)
            => symbol.TypeKind == TypeKind.Array;
            // => symbol.BaseType?.GetTypeName() == "System.Array";

        /// <summary>
        /// Проверка, не коллекция ли это?
        /// </summary>
        public static bool IsCollection (this ITypeSymbol symbol)
            => symbol.ImplementsInterface ("System.Collections.Generic.ICollection");

        /// <summary>
        /// Проверка, не список ли это?
        /// </summary>
        public static bool IsList (this ITypeSymbol symbol)
            => symbol.ImplementsInterface ("System.Collections.Generic.IList");

        public static bool IsUserClass (this ITypeSymbol symbol)
            => symbol.TypeKind == TypeKind.Class && symbol.SpecialType == SpecialType.None;

        /// <summary>
        /// Проверка, не реализует ли тип указанный интерфейс.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="interfaceName"></param>
        /// <returns></returns>
        public static bool ImplementsInterface
            (
                this ITypeSymbol symbol,
                string interfaceName
            )
        {
            var invariantCulture = StringComparison.InvariantCulture;

            if (symbol.TypeKind == TypeKind.Interface
                && symbol.GetTypeName().StartsWith (interfaceName, invariantCulture))
            {
                return true;
            }

            foreach (var item in symbol.Interfaces)
            {
                var name = item.ToDisplayString();
                if (item.IsGenericType
                    && name.StartsWith (interfaceName, invariantCulture))
                {
                    return true;
                }

                if (name.Equals (interfaceName, invariantCulture))
                {
                    return true;
                }
            }

            return false;
        }

        public static void EnumerateParameters
            (
                this StringBuilder source,
                IMethodSymbol method
            )
        {
            var parameters = method.Parameters;

            var first = true;
            foreach (var parameter in parameters)
            {
                if (!first)
                {
                    source.Append (", ");
                }

                source.Append ($"{parameter.Type.ToDisplayString()} {parameter.Name}");

                first = false;
            }
        }

        public static IList<IPropertySymbol> GetProperties (this ITypeSymbol type)
            => type.GetAllMembers()
                .Where(it => it is IPropertySymbol)
                .Cast<IPropertySymbol>()
                .ToList();

        public static AttributeData? GetAttribute(this ISymbol symbol, string attributeType)
            => symbol.GetAttributes().FirstOrDefault (it => it.AttributeClass!.GetTypeName() == attributeType);

        public static AttributeData? GetAttribute (this ISymbol symbol, ITypeSymbol marker)
            => symbol.GetAttributes().FirstOrDefault (it => it.AttributeClass!.Equals (marker, SymbolEqualityComparer.Default));

        public static string GetModifiers
            (
                this IMethodSymbol method
            )
        {
            var accessibility = method.DeclaredAccessibility;

            var result = string.Empty;
            switch (accessibility)
            {
                case Accessibility.Public:
                    result += " public";
                    break;

                case Accessibility.Private:
                    result += " private";
                    break;

                case Accessibility.Protected:
                    result += " protected";
                    break;

                case Accessibility.Internal:
                    result += "internal";
                    break;
            }

            if (method.IsStatic)
            {
                result += " static";
            }

            return result.Trim();
        }

        public static string MakeIndent(int level) => level > 0
            ? new string(' ', 4 * level)
            : string.Empty;
    }
}
