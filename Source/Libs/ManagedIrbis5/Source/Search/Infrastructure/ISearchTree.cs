// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ISearchTree.cs -- AST-дерево для поискового запроса
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// AST-дерево для поискового запроса.
    /// </summary>
    public interface ISearchTree
    {
        /// <summary>
        /// Parent node.
        /// </summary>
        ISearchTree? Parent { get; set; }

        /// <summary>
        /// Children of the node.
        /// </summary>
        ISearchTree[] Children { get; }

        /// <summary>
        /// Value of the node.
        /// </summary>
        string? Value { get; }

        /// <summary>
        /// Find records for the node.
        /// </summary>
        TermLink[] Find
            (
                SearchContext context
            );

        /// <summary>
        /// Replace specified child to another.
        /// </summary>
        void ReplaceChild
            (
                ISearchTree fromChild,
                ISearchTree? toChild
            );

    } // class ISearchTree

} // namespace ManagedIrbis.Infrastructure
