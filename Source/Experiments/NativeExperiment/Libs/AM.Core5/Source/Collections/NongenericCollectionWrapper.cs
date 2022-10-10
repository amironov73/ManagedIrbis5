// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NongenericCollectionWrapper.cs -- обертка над не-типизированной коллекцией
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Обертка над не-типизированной коллекцией.
/// </summary>
public sealed class NongenericCollectionWrapper<T>
    : IReadOnlyCollection<T>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public NongenericCollectionWrapper
        (
            ICollection collection
        )
    {
        Sure.NotNull (collection);

        _collection = collection;
    }

    #endregion

    #region Private members

    private readonly ICollection _collection;

    #endregion

    #region IReadOnlyCollection<T> members

    /// <inheritdoc cref="IReadOnlyCollection{T}.Count"/>
    public int Count => _collection.Count;

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<T> GetEnumerator()
    {
        foreach (T item in _collection)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();

    #endregion
}
