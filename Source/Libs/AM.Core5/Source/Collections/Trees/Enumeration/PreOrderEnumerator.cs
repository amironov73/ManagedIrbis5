// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PerOrderEnumerator.cs --
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
/// Enumerator for pre-order traversal with optional max depth of traversal
/// </summary>
/// <typeparam name="TNode"></typeparam>
public class PreOrderEnumerator<TNode>
    : IEnumerator<TNode?>
    where TNode : TreeNode<TNode>
{
    private readonly TNode _rootOfIteration;
    private readonly int _maxLevel;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="rootOfIteration"></param>
    /// <param name="maxRelativeDepth"></param>
    internal PreOrderEnumerator
        (
            TNode rootOfIteration,
            int? maxRelativeDepth = null
        )
    {
        _rootOfIteration = rootOfIteration;
        _maxLevel = rootOfIteration.Level + maxRelativeDepth ?? int.MaxValue;

        Current = null;
    }

    /// <inheritdoc cref="IEnumerator.MoveNext"/>
    public bool MoveNext()
    {
        if (Current == null)
        {
            Current = _rootOfIteration;
            return true;
        }

        var firstChild = Current.Children.FirstOrDefault();

        if (firstChild != null && firstChild.Level <= _maxLevel)
        {
            Current = firstChild;
            return true;
        }

        if (Current.Equals (_rootOfIteration))
        {
            return false;
        }

        var node = Current;
        var nextSibling = Current.NextSibling;

        while (nextSibling == null)
        {
            node = node.Parent;

            if (node!.Equals (_rootOfIteration))
            {
                return false;
            }

            nextSibling = node.NextSibling;
        }

        Current = nextSibling;
        return true;
    }

    /// <inheritdoc cref="IEnumerator{T}.Current"/>
    public TNode? Current { get; private set; }

    object? IEnumerator.Current => Current;

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="IEnumerator.Reset"/>
    public void Reset()
    {
        Current = null;
    }
}
