// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* VirtualChildren.cs -- виртуальные потомки для PftNode
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure;

/// <summary>
/// Виртуальные потомки для PftNode, никак себя не проявляющие,
/// пока не понадобятся на самом деле.
/// </summary>
public sealed class VirtualChildren
    : IList<PftNode>
{
    #region Events

    /// <summary>
    /// Fired on <see cref="GetEnumerator()"/> call.
    /// </summary>
    public event EventHandler? Enumeration;

    #endregion

    #region Private members

    private PftNode[] _children = Array.Empty<PftNode>();

    #endregion

    #region Public methods

    /// <summary>
    /// Set children array.
    /// </summary>
    public void SetChildren
        (
            IEnumerable<PftNode> children
        )
    {
        _children = children.ToArray();
    }

    #endregion

    #region IList<PftNode> members

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    public IEnumerator<PftNode> GetEnumerator()
    {
        Enumeration.Raise (this);

        foreach (PftNode node in _children)
        {
            yield return node;
        }
    }

    /// <inheritdoc cref="ICollection{T}.Add" />
    public void Add
        (
            PftNode item
        )
    {
        Sure.NotNull (item);

        Magna.Logger.LogError
            (
                nameof (VirtualChildren) + "::" + nameof (Add)
                + ": not applicable"
            );

        throw new NotSupportedException();
    }

    /// <inheritdoc cref="ICollection{T}.Clear" />
    public void Clear()
    {
        Magna.Logger.LogError
            (
                nameof (VirtualChildren) + "::" + nameof (Clear)
                + ": not applicable"
            );

        throw new NotSupportedException();
    }

    /// <inheritdoc cref="ICollection{T}.Contains" />
    public bool Contains
        (
            PftNode item
        )
    {
        return _children.Contains (item);
    }

    /// <inheritdoc cref="ICollection{T}.CopyTo" />
    public void CopyTo
        (
            PftNode[] array,
            int arrayIndex
        )
    {
        _children.CopyTo (array, arrayIndex);
    }

    /// <inheritdoc cref="ICollection{T}.Remove" />
    public bool Remove
        (
            PftNode item
        )
    {
        Sure.NotNull (item);

        Magna.Logger.LogError
            (
                nameof (VirtualChildren) + "::" + nameof (Remove)
                + ": not applicable"
            );

        throw new NotSupportedException();
    }


    /// <inheritdoc cref="ICollection{T}.Count" />
    public int Count => _children.Length;

    /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
    public bool IsReadOnly => true;

    /// <inheritdoc cref="IList{T}.IndexOf" />
    public int IndexOf
        (
            PftNode item
        )
    {
        Sure.NotNull (item);

        return Array.IndexOf (_children, item);
    }

    /// <inheritdoc cref="IList{T}.Insert" />
    public void Insert
        (
            int index,
            PftNode item
        )
    {
        Sure.NotNull (item);

        Magna.Logger.LogError
            (
                nameof (VirtualChildren) + "::" + nameof (Insert)
                + "not applicable"
            );

        throw new NotSupportedException();
    }

    /// <inheritdoc cref="IList{T}.RemoveAt" />
    public void RemoveAt
        (
            int index
        )
    {
        Magna.Logger.LogError
            (
                nameof (VirtualChildren) + "::" + nameof (RemoveAt)
                + ": not applicable"
            );

        throw new NotSupportedException();
    }

    /// <inheritdoc cref="IList{T}.this" />
    public PftNode this [int index]
    {
        get => _children[index];
        set
        {
            Sure.NotNull (value);

            Magna.Logger.LogError
                (
                    nameof (VirtualChildren) + "::Indexer"
                                             + "set value={Value}",
                    value.ToVisibleString()
                );

            throw new NotSupportedException();
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return _children.Length.ToInvariantString();
    }

    #endregion
}
