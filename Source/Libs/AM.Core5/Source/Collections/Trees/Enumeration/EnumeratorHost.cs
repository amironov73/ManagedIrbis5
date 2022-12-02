// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* EnumeratorHost.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
/// Container exposing enumeration with an injectable enumerator
/// </summary>
/// <typeparam name="TNode"></typeparam>
public class EnumeratorHost<TNode>
    : IEnumerable<TNode>
    where TNode: TreeNode<TNode>
{
    private readonly IEnumerator<TNode> _enumerator;

    internal EnumeratorHost (IEnumerator<TNode> enumerator)
    {
        _enumerator = enumerator;
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<TNode> GetEnumerator()
    {
        return _enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
