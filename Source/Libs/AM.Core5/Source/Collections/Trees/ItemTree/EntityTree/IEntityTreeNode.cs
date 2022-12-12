// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* IEntityTreeNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace TreeCollections;

/// <summary>
/// Represents a tree node whose payload item has a unique and separate existence from all other items in the same tree.
/// Such a node is solely distinguishable from other nodes by its Id which will be that of its contained item.
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TItem"></typeparam>
public interface IEntityTreeNode<out TId, out TItem>
    : IItemTreeNode<TItem>
{
    /// <summary>
    ///
    /// </summary>
    TId Id { get; }
}
