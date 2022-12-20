// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* SpyEnumerable.cs -- шпионящая коллекция
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections.Generic;

/// <summary>
/// Шпионящая коллекция
/// </summary>
/// <typeparam name="T">Тип элементов коллекции.</typeparam>
public sealed class SpyEnumerable<T>
    : IEnumerable<T>
{

    #region Events

    /// <summary>
    /// Обработчик событий, возникающих при перечислении коллекции.
    /// </summary>
    public event EventHandler<EnumerationEventArgs>? Spy;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="inner">Изучаемая коллекция.</param>
    public SpyEnumerable
        (
            IEnumerable<T> inner
        )
    {
        Sure.NotNull (inner);

        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly IEnumerable<T> _inner;

    #endregion

    #region IEnumerable<T> members

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<T> GetEnumerator()
    {
        var result = new SpyEnumerator<T>(_inner.GetEnumerator());
        Spy?.Invoke (this, new EnumerationEventArgs
        {
            Event = "GetEnumerator",
            Result = result
        });

        return result;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
