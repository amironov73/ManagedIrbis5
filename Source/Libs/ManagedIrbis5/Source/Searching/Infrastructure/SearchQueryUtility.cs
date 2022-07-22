// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchQueryUtility.cs -- полезные методы для работы с поисковым деревом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

/// <summary>
/// Полезные методы для работы с поисковым деревом.
/// </summary>
public static class SearchQueryUtility
{
    #region Private members

    internal static List<ISearchTree> GetDescendants
        (
            ISearchTree node
        )
    {
        Sure.NotNull (node);

        var result = new List<ISearchTree>
        {
            node
        };

        foreach (var child in node.Children)
        {
            var descendants = GetDescendants (child);

            result.AddRange (descendants);
        }

        return result;
    }

    /// <summary>
    /// Require syntax element.
    /// </summary>
    internal static string RequireSyntax
        (
            this string? element,
            string message
        )
    {
        if (element is null)
        {
            Magna.Logger.LogError
                (
                    nameof (SearchQueryUtility) + "::" + nameof (RequireSyntax)
                    + ": required element missing: {Message}",
                    message
                );

            throw new SearchSyntaxException (message);
        }

        return element;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Extract search terms from the query.
    /// </summary>
    public static SearchTerm[] ExtractTerms
        (
            SearchProgram program
        )
    {
        Sure.NotNull (program, nameof (program));

        var nodes = GetDescendants (program);
        var result = nodes
            .OfType<SearchTerm>()
            .ToArray();

        return result;
    }

    #endregion
}
