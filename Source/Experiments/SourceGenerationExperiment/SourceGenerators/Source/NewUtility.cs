// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Utility.cs -- полезные методы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;

#endregion

namespace SourceGenerators
{
    /// <summary>
    /// Полезные методы.
    /// </summary>
    internal static class NewUtility
    {
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

        public static AttributeData? GetAttribute (this ISymbol symbol, ITypeSymbol marker)
            => symbol.GetAttributes().FirstOrDefault(it => it.AttributeClass!.Equals(marker, SymbolEqualityComparer.Default));

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
    }
}
