// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo

/* ComplexLevel.cs -- комплексный уровень
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

/// <summary>
/// Комплексный уровень (состоит из нескольких элементов).
/// </summary>
internal class ComplexLevel<T>
    : ISearchTree
    where T : class, ISearchTree
{
    #region Properties

    /// <summary>
    /// Выражение реально комплексное (т. е. содержит более одного элемента)?
    /// </summary>
    public bool IsComplex => Items.Count > 1;

    /// <summary>
    /// Разделитель элементов.
    /// </summary>
    public string? Separator { get; }

    /// <summary>
    /// Собственно элементы.
    /// </summary>
    public NonNullCollection<T> Items { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ComplexLevel
        (
            string? separator
        )
    {
        Separator = separator;
        Items = new NonNullCollection<T>();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление элемента.
    /// </summary>
    public ComplexLevel<T> AddItem
        (
            T item
        )
    {
        Sure.NotNull (item);

        Items.Add (item);

        return this;
    }

    #endregion

    #region ISearchTree members

    /// <inheritdoc cref="ISearchTree.Parent"/>
    public ISearchTree? Parent { get; set; }

    /// <inheritdoc cref="ISearchTree.Children" />
    public ISearchTree[] Children => Items.ToArray();

    /// <inheritdoc cref="ISearchTree.Value" />
    public string? Value => null;

    /// <inheritdoc cref="ISearchTree.Find"/>
    public virtual TermLink[] Find
        (
            SearchContext context
        )
    {
        return Array.Empty<TermLink>();
    }

    /// <inheritdoc cref="ISearchTree.ReplaceChild"/>
    public void ReplaceChild
        (
            ISearchTree fromChild,
            ISearchTree? toChild
        )
    {
        var item = (T) fromChild;

        var index = Items.IndexOf (item);
        if (index < 0)
        {
            Magna.Error
                (
                    nameof (ComplexLevel<T>) + "::" + nameof (ReplaceChild)
                    + "child not found: "
                    + fromChild.ToVisibleString()
                );

            throw new KeyNotFoundException();
        }

        if (toChild is null)
        {
            Items.RemoveAt (index);
        }
        else
        {
            Items[index] = item;
            toChild.Parent = this;
        }

        fromChild.Parent = this;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var result = string.Join (Separator, Items);

        if (IsComplex)
        {
            result = " ( " + result + " ) ";
        }

        return result;
    }

    #endregion
}
