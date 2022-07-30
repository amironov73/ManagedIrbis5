// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblNodeCollection.cs -- коллекция GBL-узлов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.ObjectModel;

using AM;
using AM.Collections;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure;

/// <summary>
/// Коллекция GBL-узлов.
/// </summary>
public sealed class GblNodeCollection
    : NonNullCollection<GblNode>
{
    #region Properties

    /// <summary>
    /// Узел-родитель.
    /// </summary>
    public GblNode? Parent { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public GblNodeCollection
        (
            GblNode? parent
        )
    {
        Parent = parent;
    }

    #endregion

    #region Collection<T> members

    /// <inheritdoc cref="Collection{T}.ClearItems" />
    protected override void ClearItems()
    {
        foreach (var node in this)
        {
            node.Parent = null;
        }

        base.ClearItems();
    }

    /// <inheritdoc cref="NonNullCollection{T}.InsertItem" />
    protected override void InsertItem
        (
            int index,
            GblNode item
        )
    {
        Sure.NonNegative (index);
        Sure.NotNull (item);

        if (item.Parent is not null)
        {
            if (!ReferenceEquals (item.Parent, Parent))
            {
                throw new IrbisException();
            }
        }

        item.Parent = Parent;
        base.InsertItem (index, item);
    }

    /// <inheritdoc cref="NonNullCollection{T}.SetItem" />
    protected override void SetItem
        (
            int index,
            GblNode item
        )
    {
        Sure.InRange (index, this);
        Sure.NotNull (item);

        if (item.Parent is not null)
        {
            if (!ReferenceEquals (item.Parent, Parent))
            {
                throw new IrbisException();
            }
        }

        if (index < Count)
        {
            var previousItem = this[index];
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
        Sure.InRange (index, this);

        var node = this[index];
        node.Parent = null;

        base.RemoveItem (index);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        GblUtility.NodesToText (builder, this);

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
