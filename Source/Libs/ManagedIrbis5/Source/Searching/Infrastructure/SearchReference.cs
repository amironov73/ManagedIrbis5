// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchReference.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// #N
    /// </summary>
    sealed class SearchReference
        : ISearchTree
    {
        #region Properties

        /// <inheritdoc cref="ISearchTree.Parent"/>
        public ISearchTree? Parent { get; set; }

        /// <summary>
        /// Number.
        /// </summary>
        public string? Number { get; set; }

        #endregion

        #region ISearchTree members

        public ISearchTree[] Children
        {
            get { return SearchUtility.EmptyArray; }
        }

        public string? Value => Number;

        public TermLink[] Find
            (
                SearchContext context
            )
        {
            TermLink[] result = Array.Empty<TermLink>();

            int number = Number.SafeToInt32(-1);
            if (number > 0)
            {
                var history = context.Manager.SearchHistory;
                if (number <= history.Count)
                {
                    var previous = history[number - 1];
                    var query = previous.Query;

                    if (!string.IsNullOrEmpty(query))
                    {
                        int[] found = context.Provider.Search(query);
                        result = TermLink.FromMfn(found);
                    }
                }
            }

            return result;
        }

        /// <inheritdoc cref="ISearchTree.ReplaceChild"/>
        public void ReplaceChild
            (
                ISearchTree fromChild,
                ISearchTree? toChild
            )
        {
            Magna.Error
                (
                    "SearchReference::ReplaceChild: "
                    + "not implemented"
                );

            throw new NotImplementedException();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return "#" + Number;
        }

        #endregion

    } // class SearchReference

} // namespace ManagedIrbis.Infrastructure
