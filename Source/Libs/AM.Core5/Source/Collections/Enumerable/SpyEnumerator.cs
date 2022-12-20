// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* SpyEnumerator.cs -- шпионящий перечислитель элементов коллекции
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
/// Шпионящий перечислитель элементов коллекции.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class SpyEnumerator<T>
    : IEnumerator<T>
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
    /// <param name="inner">Изучаемый перечислитель.</param>
    public SpyEnumerator
        (
            IEnumerator<T> inner
        )
    {
        Sure.NotNull (inner);

        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly IEnumerator<T> _inner;

    #endregion

    #region IEnumerator<T> members

    /// <inheritdoc cref="IEnumerator.MoveNext"/>
    public bool MoveNext()
    {
        var result = _inner.MoveNext();
        Spy?.Invoke (this, new EnumerationEventArgs
        {
            Event = "MoveNext",
            Result = result
        });

        return result;
    }

    /// <inheritdoc cref="IEnumerator.Reset"/>
    public void Reset()
    {
        Spy?.Invoke (this, new EnumerationEventArgs
        {
            Event = "Reset"
        });
        _inner.Reset();
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Spy?.Invoke (this, new EnumerationEventArgs
        {
            Event = "Dispose"
        });
        _inner.Dispose();
    }

    /// <inheritdoc cref="IEnumerator{T}.Current"/>
    public T Current
    {
        get
        {
            var result = _inner.Current;
            Spy?.Invoke (this, new EnumerationEventArgs
            {
                Event = "Current",
                Result = result
            });

            return result;
        }
    }

    object? IEnumerator.Current => Current;

    #endregion
}
