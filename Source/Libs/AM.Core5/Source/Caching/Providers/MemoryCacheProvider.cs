// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* MemoryCacheProvider.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

#endregion

#nullable enable

namespace AM.Caching.Providers;

/// <summary>
///
/// </summary>
public class MemoryCacheProvider
    : ICacheProvider
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="cache"></param>
    public MemoryCacheProvider
        (
            IMemoryCache cache
        )
    {
        Sure.NotNull (cache);

        _cache = cache;
    }

    #endregion

    #region Internal members

    internal readonly IMemoryCache _cache;

    #endregion

    #region ICacheProvider members

    /// <inheritdoc cref="ICacheProvider.Set"/>
    public void Set
        (
            string key,
            object item,
            MemoryCacheEntryOptions policy
        )
    {
        Sure.NotNullNorEmpty (key);

        _cache.Set (key, item, policy);
    }

    /// <inheritdoc cref="ICacheProvider.Get"/>
    public object? Get
        (
            string key
        )
    {
        Sure.NotNullNorEmpty (key);

        return _cache.Get (key);
    }

    /// <inheritdoc cref="ICacheProvider.GetOrCreate{T}(string,System.Func{Microsoft.Extensions.Caching.Memory.ICacheEntry,T})"/>
    public object? GetOrCreate<T>
        (
            string key,
            Func<ICacheEntry, T> factory
        )
    {
        Sure.NotNullNorEmpty (key);

        return _cache.GetOrCreate (key, factory);
    }

    /// <inheritdoc cref="ICacheProvider.GetOrCreate{T}(string,Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions,System.Func{Microsoft.Extensions.Caching.Memory.ICacheEntry,T})"/>
    public object? GetOrCreate<T>
        (
            string key,
            MemoryCacheEntryOptions? policy,
            Func<ICacheEntry, T> factory
        )
    {
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (factory);

        if (policy is null)
        {
            return _cache.GetOrCreate (key, factory);
        }

        if (!_cache.TryGetValue (key, out var result))
        {
            var entry = _cache.CreateEntry (key);

            // Set the initial options before the factory is fired so that any callbacks
            // that need to be wired up are still added.
            entry.SetOptions (policy);

            if (policy is LazyCacheEntryOptions lazyPolicy &&
                lazyPolicy.ExpirationMode != ExpirationMode.LazyExpiration)
            {
                var expiryTokenSource = new CancellationTokenSource();
                var expireToken = new CancellationChangeToken (expiryTokenSource.Token);
                entry.AddExpirationToken (expireToken);
                entry.RegisterPostEvictionCallback ((_, _, _, _) =>
                    expiryTokenSource.Dispose());

                result = factory (entry);

                expiryTokenSource.CancelAfter (lazyPolicy.ImmediateAbsoluteExpirationRelativeToNow);
            }
            else
            {
                result = factory (entry);
            }

            entry.SetValue (result!);

            // need to manually call dispose instead of having a using
            // in case the factory passed in throws, in which case we
            // do not want to add the entry to the cache
            entry.Dispose();
        }

        return (T) result!;
    }

    /// <inheritdoc cref="ICacheProvider.Remove"/>
    public void Remove
        (
            string key
        )
    {
        Sure.NotNullNorEmpty (key);

        _cache.Remove (key);
    }

    /// <inheritdoc cref="ICacheProvider.GetOrCreateAsync{T}"/>
    public Task<T> GetOrCreateAsync<T>
        (
            string key,
            Func<ICacheEntry, Task<T>> factory
        )
    {
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (factory);

        return _cache.GetOrCreateAsync (key, factory)!;
    }

    /// <inheritdoc cref="ICacheProvider.TryGetValue{T}"/>
    public bool TryGetValue<T>
        (
            object key,
            out T value
        )
    {
        Sure.NotNull (key);

        return _cache.TryGetValue (key, out value!);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        _cache.Dispose();
    }

    #endregion
}
