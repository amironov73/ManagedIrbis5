// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* KeyedMemoryCache.cs -- кеш, способный перечислить ключи хранимых элементов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#endregion

namespace TheNude;

/// <summary>
/// Кеш, способный перечислить хранимых в нем элементов.
/// </summary>
internal sealed class KeyedMemoryCache
    : IMemoryCache
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public KeyedMemoryCache
        (
            IOptions<MemoryCacheOptions> optionsAccessor
        )
    {
        _memoryCache = new MemoryCache (optionsAccessor);
        _keyDictionary = new();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public KeyedMemoryCache
        (
            IOptions<MemoryCacheOptions> optionsAccessor,
            ILoggerFactory loggerFactory)
    {
        _memoryCache = new MemoryCache (optionsAccessor, loggerFactory);
        _keyDictionary = new();
    }

    #endregion

    #region Private members

    private readonly MemoryCache _memoryCache;
    private readonly Dictionary<string, object?> _keyDictionary;

    #endregion

    #region IMemoryCache members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        _memoryCache.Dispose();
    }

    /// <inheritdoc cref="IMemoryCache.TryGetValue"/>
    public bool TryGetValue
        (
            object key,
            out object? value
        )
    {
        return _memoryCache.TryGetValue (key, out value);
    }

    /// <inheritdoc cref="IMemoryCache.CreateEntry"/>
    public ICacheEntry CreateEntry
        (
            object key
        )
    {
        var result = _memoryCache.CreateEntry (key);

        var keyText = key.ToString().ThrowIfNullOrEmpty();
        _keyDictionary[keyText] = null;

        return result;
    }

    /// <inheritdoc cref="IMemoryCache.Remove"/>
    public void Remove
        (
            object key
        )
    {
        _memoryCache.Remove (key);

        var keyText = key.ToString().ThrowIfNullOrEmpty();
        _keyDictionary.Remove (keyText);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Очистка кеша.
    /// </summary>
    public void Clear()
    {
        _memoryCache.Clear();
        _keyDictionary.Clear();
    }

    /// <summary>
    /// Получение всех ключей, хранящихся в кеше.
    /// </summary>
    public IReadOnlyCollection<string> GetKeys()
    {
        return _keyDictionary.Keys;
    }

    #endregion
}
