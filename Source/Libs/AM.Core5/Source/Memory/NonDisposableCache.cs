// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* NonDisposableCache.cs -- обертка вокруг IMemoryCache, предотвращающая его очистку
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace AM.Memory;

/// <summary>
/// Обертка вокруг <see cref="IMemoryCache"/>,
/// предотвращающая его очиску при вызове <see cref="IDisposable.Dispose"/>.
/// </summary>
public sealed class NonDisposableCache
    : IMemoryCache
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="inner">Кэш, подлежащий оборачиванию.</param>
    public NonDisposableCache
        (
            IMemoryCache inner
        )
    {
        Sure.NotNull (inner);

        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly IMemoryCache _inner;

    #endregion

    #region IMemoryCache members

    /// <inheritdoc cref="IMemoryCache.TryGetValue"/>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public bool TryGetValue
        (
            object key,
            out object? value
        )
    {
        return _inner.TryGetValue (key, out value);
    }

    /// <inheritdoc cref="IMemoryCache.CreateEntry"/>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public ICacheEntry CreateEntry
        (
            object key
        )
    {
        return _inner.CreateEntry (key);
    }

    /// <inheritdoc cref="IMemoryCache.Remove"/>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Remove
        (
            object key
        )
    {
        _inner.Remove (key);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // пустое тело метода
        // не вызываем Dispose у _inner!
    }

    #endregion
}
