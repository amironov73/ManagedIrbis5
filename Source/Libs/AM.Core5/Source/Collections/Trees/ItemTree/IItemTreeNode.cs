// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* IItemTreeNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace TreeCollections;

/// <summary>
/// Represents a tree node containing a payload item
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IItemTreeNode<out TItem>
    : ITreeNode
{
    /// <summary>
    ///
    /// </summary>
    TItem Item { get; }
}
