// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* AddOperations.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
///
/// </summary>
public static class TreeQueryExtensions
{
    /// <summary>
    /// Filters nodes based on a predicate, including distinct ancestors (matching the predicate or not)
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    /// <param name="seq"></param>
    /// <param name="satisfiesCondition"></param>
    /// <returns></returns>
    public static IEnumerable<TNode> WhereSupports<TNode>
        (
            this IEnumerable<TNode> seq,
            Func<TNode, bool> satisfiesCondition
        )
        where TNode: TreeNode<TNode>
    {
        return seq
            .Where (satisfiesCondition)
            .SelectMany (n => n.SelectPathUpward())
            .Distinct();
    }
}
