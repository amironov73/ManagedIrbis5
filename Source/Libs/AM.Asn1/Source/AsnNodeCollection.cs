// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AsnNodeCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.ObjectModel;

using AM.Collections;
using AM.Text;

#endregion

#nullable enable

namespace AM.Asn1;

/// <summary>
///
/// </summary>
public sealed class AsnNodeCollection
    : NonNullCollection<AsnNode>
{
    #region Properties

    /// <summary>
    /// Parent node.
    /// </summary>
    public AsnNode? Parent { get; internal set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public AsnNodeCollection
        (
            AsnNode? parent = default
        )
    {
        Parent = parent;
    }

    #endregion

    #region Private members

    internal void EnsureParent()
    {
        foreach (var node in this)
        {
            node.Parent = Parent;
        }
    }

    #endregion

    #region Public methods

    #endregion

    #region Collection<T> members

    /// <inheritdoc cref="Collection{T}.ClearItems" />
    protected override void ClearItems()
    {
        foreach (AsnNode node in this)
        {
            node.Parent = null;
        }

        base.ClearItems();
    }

    /// <inheritdoc cref="NonNullCollection{T}.InsertItem" />
    protected override void InsertItem
        (
            int index,
            AsnNode item
        )
    {
        if (!ReferenceEquals (item.Parent, null))
        {
            if (!ReferenceEquals (item.Parent, Parent))
            {
                throw new AsnException();
            }
        }

        item.Parent = Parent;
        base.InsertItem (index, item);
    }

    /// <inheritdoc cref="NonNullCollection{T}.SetItem" />
    protected override void SetItem
        (
            int index,
            AsnNode item
        )
    {
        if (!ReferenceEquals (item.Parent, null))
        {
            if (!ReferenceEquals (item.Parent, Parent))
            {
                throw new AsnException();
            }
        }

        if (index < Count)
        {
            AsnNode previousItem = this[index];
            if (!ReferenceEquals (previousItem, item))
            {
                previousItem.Parent = null;
            }
        }

        item.Parent = Parent;
        base.SetItem (index, item);
    }

    /// <inheritdoc cref="Collection{T}.RemoveItem" />
    protected override void RemoveItem
        (
            int index
        )
    {
        AsnNode node = this[index];
        node.Parent = null;

        base.RemoveItem (index);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        AsnHelper.NodesToText (builder, this);

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
