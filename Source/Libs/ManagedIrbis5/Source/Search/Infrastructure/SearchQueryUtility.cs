// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchQueryUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    public static class SearchQueryUtility
    {
        #region Private members

        internal static List<ISearchTree> GetDescendants
            (
                ISearchTree node
            )
        {
            List<ISearchTree> result = new List<ISearchTree>
            {
                node
            };

            foreach (ISearchTree child in node.Children)
            {
                List<ISearchTree> descendants
                    = GetDescendants(child);
                result.AddRange(descendants);
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
            if (ReferenceEquals(element, null))
            {
                Magna.Error
                    (
                        "SearchQueryUtility::RequireSyntax: "
                        + "required element missing: "
                        + message
                    );

                throw new SearchSyntaxException(message);
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
            Sure.NotNull(program, nameof(program));

            List<ISearchTree> nodes = GetDescendants(program);
            SearchTerm[] result = nodes
                .OfType<SearchTerm>()
                .ToArray();

            return result;
        }

        #endregion

    } // class SearchQueryUtility

} // namespace ManagedIrbis.Infrastructure
