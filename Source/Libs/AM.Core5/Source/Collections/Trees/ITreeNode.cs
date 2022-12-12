// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global

/* ITreeNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace TreeCollections;

/// <summary>
/// Represents a tree node with a unique hierarchy identity
/// </summary>
public interface ITreeNode
{
    /// <summary>
    ///
    /// </summary>
    HierarchyPosition HierarchyId { get; }
}
