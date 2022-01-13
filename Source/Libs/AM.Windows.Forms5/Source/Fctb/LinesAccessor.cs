// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LineAccessor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Обертка над строками текста.
/// </summary>
public sealed class LinesAccessor
    : IList<string>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LinesAccessor
        (
            IList<Line> lines
        )
    {
        Sure.NotNull (lines);

        _lines = lines;
    }

    #endregion

    #region Private members

    private readonly IList<Line> _lines;

    #endregion

    #region IList<T> members

    /// <inheritdoc cref="IList{T}.IndexOf"/>
    public int IndexOf
        (
            string item
        )
    {
        for (var i = 0; i < _lines.Count; i++)
        {
            if (_lines[i].Text == item)
            {
                return i;
            }
        }

        return -1;
    }

    /// <inheritdoc cref="IList{T}.Insert"/>
    public void Insert
        (
            int index,
            string item
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IList{T}.RemoveAt"/>
    public void RemoveAt
        (
            int index
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IList{T}.this"/>
    public string this [int index]
    {
        get => _lines[index].Text;
        set => throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICollection{T}.Add"/>
    public void Add
        (
            string item
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICollection{T}.Clear"/>
    public void Clear()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICollection{T}.Contains"/>
    public bool Contains
        (
            string item
        )
    {
        foreach (var line in _lines)
        {
            if (line.Text == item)
            {
                return true;
            }}

        return false;
    }

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public void CopyTo
        (
            string[] array,
            int arrayIndex
        )
    {
        for (var i = 0; i < _lines.Count; i++)
        {
            array[i + arrayIndex] = _lines[i].Text;
        }
    }

    /// <inheritdoc cref="ICollection{T}.Count"/>
    public int Count => _lines.Count;

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    public bool IsReadOnly => true;

    /// <inheritdoc cref="ICollection{T}.Remove"/>
    public bool Remove
        (
            string item
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<string> GetEnumerator()
    {
        foreach (var t in _lines)
        {
            yield return t.Text;
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
