// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BookmarksBase.cs -- базовый класс для коллекции закладок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Базовый класс для коллекции закладок.
/// </summary>
public abstract class BookmarksBase
    : ICollection<Bookmark>, IDisposable
{
    #region ICollection members

    /// <inheritdoc cref="ICollection{T}.Add"/>
    public abstract void Add (Bookmark item);

    /// <inheritdoc cref="ICollection{T}.Clear"/>
    public abstract void Clear();

    /// <inheritdoc cref="ICollection{T}.Contains"/>
    public abstract bool Contains (Bookmark item);

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public abstract void CopyTo (Bookmark[] array, int arrayIndex);

    /// <inheritdoc cref="ICollection{T}.Count"/>
    public abstract int Count { get; }

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    public abstract bool IsReadOnly { get; }

    /// <inheritdoc cref="ICollection{T}.Remove"/>
    public abstract bool Remove (Bookmark item);

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public abstract IEnumerator<Bookmark> GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public abstract void Dispose();

    #endregion

    #region Additional properties

    public abstract void Add (int lineIndex, string bookmarkName);
    public abstract void Add (int lineIndex);
    public abstract bool Contains (int lineIndex);
    public abstract bool Remove (int lineIndex);
    public abstract Bookmark GetBookmark (int i);

    #endregion
}
