// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* CachingService.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace AM.Caching;

using Providers;

/// <summary>
///
/// </summary>
public class CachingService
    : IAppCache
{
    #region Properties

    /// <inheritdoc cref="IAppCache.CacheProvider"/>
    public virtual ICacheProvider CacheProvider => _cacheProvider.Value;

    /// <summary>
    ///
    /// </summary>
    public static Lazy<ICacheProvider> DefaultCacheProvider { get; set; }
        = new (() => new MemoryCacheProvider (new MemoryCache (new MemoryCacheOptions())));

    /// <summary>
    ///     Policy defining how long items should be cached for unless specified
    /// </summary>
    public virtual CacheDefaults DefaultCachePolicy { get; set; } = new ();

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public CachingService()
        : this (DefaultCacheProvider)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cacheProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CachingService
        (
            Lazy<ICacheProvider> cacheProvider
        )
    {
        Sure.NotNull (cacheProvider);

        _cacheProvider = cacheProvider ?? throw new ArgumentNullException (nameof (cacheProvider));
        var lockCount = Math.Max (Environment.ProcessorCount * 8, 32);
        _keyLocks = new int[lockCount];
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cacheProviderFactory"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CachingService
        (
            Func<ICacheProvider> cacheProviderFactory
        )
    {
        Sure.NotNull (cacheProviderFactory);

        _cacheProvider = new Lazy<ICacheProvider> (cacheProviderFactory);
        var lockCount = Math.Max (Environment.ProcessorCount * 8, 32);
        _keyLocks = new int[lockCount];
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CachingService
        (
            ICacheProvider cache
        )
        : this (() => cache)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    private readonly Lazy<ICacheProvider> _cacheProvider;

    private readonly int[] _keyLocks;

    private static void SetAbsoluteExpirationFromRelative (ICacheEntry entry)
    {
        if (!entry.AbsoluteExpirationRelativeToNow.HasValue)
        {
            return;
        }

        var absoluteExpiration = DateTimeOffset.UtcNow + entry.AbsoluteExpirationRelativeToNow.Value;
        if (!entry.AbsoluteExpiration.HasValue || absoluteExpiration < entry.AbsoluteExpiration)
        {
            entry.AbsoluteExpiration = absoluteExpiration;
        }
    }

    #endregion

    #region IAppCache members

    /// <inheritdoc cref="IAppCache.Add{T}"/>
    public virtual void Add<T>
        (
            string key,
            T item,
            MemoryCacheEntryOptions policy
        )
    {
        ValidateKey (key);

        CacheProvider.Set (key, item!, policy);
    }

    /// <inheritdoc cref="IAppCache.Get{T}"/>
    public virtual T Get<T>
        (
            string key
        )
    {
        ValidateKey (key);

        var item = CacheProvider.Get (key);

        return GetValueFromLazy<T> (item!, out _);
    }

    /// <inheritdoc cref="IAppCache.GetAsync{T}"/>
    public virtual Task<T> GetAsync<T>
        (
            string key
        )
    {
        ValidateKey (key);

        var item = CacheProvider.Get (key);

        return GetValueFromAsyncLazy<T> (item!, out _);
    }

    /// <inheritdoc cref="IAppCache.TryGetValue{T}"/>
    public virtual bool TryGetValue<T>
        (
            string key,
            out T value
        )
    {
        ValidateKey (key);

        return CacheProvider.TryGetValue (key, out value);
    }

    /// <inheritdoc cref="IAppCache.GetOrAdd{T}(string,System.Func{Microsoft.Extensions.Caching.Memory.ICacheEntry,T})"/>
    public virtual T GetOrAdd<T>
        (
            string key,
            Func<ICacheEntry, T> addItemFactory
        )
    {
        return GetOrAdd (key, addItemFactory, null);
    }

    /// <inheritdoc cref="IAppCache.GetOrAdd{T}(string,System.Func{Microsoft.Extensions.Caching.Memory.ICacheEntry,T},Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions)"/>
    public virtual T GetOrAdd<T>
        (
            string key,
            Func<ICacheEntry, T> addItemFactory,
            MemoryCacheEntryOptions? policy
        )
    {
        ValidateKey (key);

        object cacheItem;

        object CacheFactory (ICacheEntry entry) =>
            new Lazy<T> (() =>
            {
                var result = addItemFactory (entry);
                SetAbsoluteExpirationFromRelative (entry);
                EnsureEvictionCallbackDoesNotReturnTheAsyncOrLazy<T> (entry.PostEvictionCallbacks);
                return result;
            });

        // acquire lock per key
        uint hash = (uint)key.GetHashCode() % (uint)_keyLocks.Length;
        while (Interlocked.CompareExchange (ref _keyLocks[hash], 1, 0) == 1)
        {
            Thread.Yield();
        }

        try
        {
            cacheItem = CacheProvider.GetOrCreate (key, policy!, CacheFactory)!;
        }
        finally
        {
            _keyLocks[hash] = 0;
        }

        try
        {
            var result = GetValueFromLazy<T> (cacheItem, out var valueHasChangedType);

            // if we get a cache hit but for something with the wrong type we need to evict it, start again and cache the new item instead
            if (valueHasChangedType)
            {
                CacheProvider.Remove (key);

                // acquire lock again
                hash = (uint)key.GetHashCode() % (uint)_keyLocks.Length;
                while (Interlocked.CompareExchange (ref _keyLocks[hash], 1, 0) == 1)
                {
                    Thread.Yield();
                }

                try
                {
                    cacheItem = CacheProvider.GetOrCreate (key, CacheFactory)!;
                }
                finally
                {
                    _keyLocks[hash] = 0;
                }

                result = GetValueFromLazy<T> (cacheItem,
                    out _ /* we just evicted so type change cannot happen this time */);
            }

            return result;
        }
        catch //addItemFactory errored so do not cache the exception
        {
            CacheProvider.Remove (key);
            throw;
        }
    }

    /// <inheritdoc cref="IAppCache.Remove"/>
    public virtual void Remove
        (
            string key
        )
    {
        ValidateKey (key);
        CacheProvider.Remove (key);
    }

    /// <inheritdoc cref="IAppCache.GetOrAddAsync{T}(string,System.Func{Microsoft.Extensions.Caching.Memory.ICacheEntry,System.Threading.Tasks.Task{T}})"/>
    public virtual Task<T> GetOrAddAsync<T>
        (
            string key,
            Func<ICacheEntry, Task<T>> addItemFactory
        )
    {
        return GetOrAddAsync (key, addItemFactory, null);
    }

    /// <inheritdoc cref="IAppCache.GetOrAddAsync{T}(string,System.Func{Microsoft.Extensions.Caching.Memory.ICacheEntry,System.Threading.Tasks.Task{T}},Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions)"/>
    public virtual async Task<T> GetOrAddAsync<T>
        (
            string key,
            Func<ICacheEntry, Task<T>> addItemFactory,
            MemoryCacheEntryOptions? policy
        )
    {
        ValidateKey (key);

        object cacheItem;

        // Ensure only one thread can place an item into the cache provider at a time.
        // We are not evaluating the addItemFactory inside here - that happens outside the lock,
        // below, and guarded using the async lazy. Here we just ensure only one thread can place
        // the AsyncLazy into the cache at one time

        // acquire lock
        uint hash = (uint)key.GetHashCode() % (uint)_keyLocks.Length;
        while (Interlocked.CompareExchange (ref _keyLocks[hash], 1, 0) == 1)
        {
            Thread.Yield();
        }

        object CacheFactory (ICacheEntry entry) =>
            new AsyncLazy<T> (async () =>
            {
                var result = await addItemFactory (entry).ConfigureAwait (false);
                SetAbsoluteExpirationFromRelative (entry);
                EnsureEvictionCallbackDoesNotReturnTheAsyncOrLazy<T> (entry.PostEvictionCallbacks);
                return result;
            });

        try
        {
            cacheItem = CacheProvider.GetOrCreate (key, policy!, CacheFactory)!;
        }
        finally
        {
            _keyLocks[hash] = 0;
        }

        try
        {
            var result = GetValueFromAsyncLazy<T> (cacheItem, out var valueHasChangedType);

            // if we get a cache hit but for something with the wrong type we need to evict it, start again and cache the new item instead
            if (valueHasChangedType)
            {
                CacheProvider.Remove (key);

                // acquire lock
                hash = (uint)key.GetHashCode() % (uint)_keyLocks.Length;
                while (Interlocked.CompareExchange (ref _keyLocks[hash], 1, 0) == 1)
                {
                    Thread.Yield();
                }

                try
                {
                    cacheItem = CacheProvider.GetOrCreate (key, CacheFactory)!;
                }
                finally
                {
                    _keyLocks[hash] = 0;
                }

                result = GetValueFromAsyncLazy<T> (cacheItem,
                    out _ /* we just evicted so type change cannot happen this time */);
            }

            if (result.IsCanceled || result.IsFaulted)
            {
                CacheProvider.Remove (key);
            }

            return await result.ConfigureAwait (false);
        }
        catch //addItemFactory errored so do not cache the exception
        {
            CacheProvider.Remove (key);
            throw;
        }
    }

    #endregion

    #region Protected members

    /// <summary>
    ///
    /// </summary>
    /// <param name="item"></param>
    /// <param name="valueHasChangedType"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected virtual T GetValueFromLazy<T>
        (
            object item,
            out bool valueHasChangedType
        )
    {
        valueHasChangedType = false;
        switch (item)
        {
            case Lazy<T> lazy:
                return lazy.Value;
            case T variable:
                return variable;
            case AsyncLazy<T> asyncLazy:
                // this is async to sync - and should not really happen as long as GetOrAddAsync is used for an async
                // value. Only happens when you cache something async and then try and grab it again later using
                // the non async methods.
                return asyncLazy.Value.ConfigureAwait (false).GetAwaiter().GetResult();
            case Task<T> task:
                return task.Result;
        }

        // if they have cached something else with the same key we need to tell caller to reset the cached item
        // although this is probably not the fastest this should not get called on the main use case
        // where you just hit the first switch case above.
        var itemsType = item.GetType();
        if (itemsType is { IsGenericType: true })
        {
            if (itemsType.GetGenericTypeDefinition() == typeof (Lazy<>))
            {
                valueHasChangedType = true;
            }
        }

        return default!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="item"></param>
    /// <param name="valueHasChangedType"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected virtual Task<T> GetValueFromAsyncLazy<T>
        (
            object item,
            out bool valueHasChangedType
        )
    {
        valueHasChangedType = false;
        switch (item)
        {
            case AsyncLazy<T> asyncLazy:
                return asyncLazy.Value;
            case Task<T> task:
                return task;

            // this is sync to async and only happens if you cache something sync and then get it later async
            case Lazy<T> lazy:
                return Task.FromResult (lazy.Value);
            case T variable:
                return Task.FromResult (variable);
        }

        // if they have cached something else with the same key we need to tell caller to reset the cached item
        // although this is probably not the fastest this should not get called on the main use case
        // where you just hit the first switch case above.
        var itemsType = item.GetType();
        if (itemsType is { IsGenericType: true })
        {
            if (itemsType.GetGenericTypeDefinition() == typeof (AsyncLazy<>))
            {
                valueHasChangedType = true;
            }
        }

        return Task.FromResult (default (T))!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="callbackRegistrations"></param>
    /// <typeparam name="T"></typeparam>
    protected virtual void EnsureEvictionCallbackDoesNotReturnTheAsyncOrLazy<T>
        (
            IList<PostEvictionCallbackRegistration> callbackRegistrations
        )
    {
        if (callbackRegistrations != null!)
        {
            foreach (var item in callbackRegistrations)
            {
                var originalCallback = item.EvictionCallback;
                item.EvictionCallback = (key, value, reason, state) =>
                {
                    // before the original callback we need to unwrap the Lazy that holds the cache item
                    if (value is AsyncLazy<T> asyncCacheItem)
                    {
                        value = asyncCacheItem.IsValueCreated ? asyncCacheItem.Value : Task.FromResult (default (T))!;
                    }
                    else if (value is Lazy<T> cacheItem)
                    {
                        value = cacheItem.IsValueCreated ? cacheItem.Value : default (T);
                    }

                    // pass the unwrapped cached value to the original callback
                    originalCallback! (key, value, reason, state);
                };
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected virtual void ValidateKey
        (
            string key
        )
    {
        if (key is null)
        {
            throw new ArgumentNullException (nameof (key));
        }

        if (string.IsNullOrWhiteSpace (key))
        {
            throw new ArgumentOutOfRangeException (nameof (key), "Cache keys cannot be empty or whitespace");
        }
    }

    #endregion
}
