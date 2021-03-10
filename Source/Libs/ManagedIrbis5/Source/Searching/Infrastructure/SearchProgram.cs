// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchProgram.cs -- корень синтаксического дерева поискового запроса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Корень синтаксического дерева поискового запроса.
    /// </summary>
    public sealed class SearchProgram
        : ISearchTree
    {
        #region Properties

        /// <summary>
        /// No parent.
        /// </summary>
        public ISearchTree? Parent
        {
            get => null;
            set { /* Do nothing */ }
        }

        /// <summary>
        /// Program entry point - root of syntax tree.
        /// </summary>
        internal SearchLevel6? EntryPoint { get; set; }

        #endregion

        #region ISearchTree members

        /// <inheritdoc cref="ISearchTree.Children" />
        ISearchTree[] ISearchTree.Children
        {
            get
            {
                ISearchTree[] result
                    = ReferenceEquals(EntryPoint, null)
                    ? new ISearchTree[0]
                    : EntryPoint.Children;

                return result;
            }
        }

        /// <inheritdoc cref="ISearchTree.Value" />
        string? ISearchTree.Value => null;

        /// <inheritdoc cref="ISearchTree.Find"/>
        public TermLink[] Find
            (
                SearchContext context
            )
        {
            TermLink[] result = ReferenceEquals(EntryPoint, null)
                ? Array.Empty<TermLink>()
                : EntryPoint.Find(context);

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
                    "SearchProgram::ReplaceChild: "
                    + "not implemented"
                );

            throw new NotImplementedException();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (ReferenceEquals(EntryPoint, null))
            {
                return string.Empty;
            }

            string result = EntryPoint.ToString()
                .Trim();

            return result;
        }

        #endregion

    } // class SearchProgram

} // namespace ManagedIrbis.Infrastructure
