// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* AppCacheExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace AM.Caching;

/// <summary>
///
/// </summary>
public static class AppCacheExtensions
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    public static void Add<T>
        (
            this IAppCache cache,
            string key,
            T item
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);

        cache.Add (key, item, cache.DefaultCachePolicy.BuildOptions());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <param name="expires"></param>
    /// <typeparam name="T"></typeparam>
    public static void Add<T>
        (
            this IAppCache cache,
            string key,
            T item,
            DateTimeOffset expires
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);

        cache.Add
            (
                key,
                item,
                new MemoryCacheEntryOptions { AbsoluteExpiration = expires }
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <param name="slidingExpiration"></param>
    /// <typeparam name="T"></typeparam>
    public static void Add<T>
        (
            this IAppCache cache,
            string key,
            T item,
            TimeSpan slidingExpiration
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);

        cache.Add
            (
                key,
                item,
                new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration }
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAdd<T>
        (
            this IAppCache cache,
            string key,
            Func<T> addItemFactory
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (addItemFactory);

        return cache.GetOrAdd
            (
                key,
                addItemFactory,
                cache.DefaultCachePolicy.BuildOptions()
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <param name="expires"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAdd<T>
        (
            this IAppCache cache,
            string key,
            Func<T> addItemFactory,
            DateTimeOffset expires
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (addItemFactory);

        return cache.GetOrAdd
            (
                key,
                addItemFactory,
                new MemoryCacheEntryOptions { AbsoluteExpiration = expires }
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <param name="expires"></param>
    /// <param name="mode"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAdd<T>
        (
            this IAppCache cache,
            string key,
            Func<T> addItemFactory,
            DateTimeOffset expires,
            ExpirationMode mode
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (addItemFactory);
        Sure.Defined (mode);

        return mode switch
        {
            ExpirationMode.LazyExpiration => cache.GetOrAdd
                (
                    key,
                    addItemFactory,
                    new MemoryCacheEntryOptions { AbsoluteExpiration = expires }
                ),

            _ => cache.GetOrAdd
                (
                    key,
                    addItemFactory,
                    new LazyCacheEntryOptions().SetAbsoluteExpiration (expires, mode)
                )
        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <param name="slidingExpiration"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAdd<T>
        (
            this IAppCache cache,
            string key,
            Func<T> addItemFactory,
            TimeSpan slidingExpiration
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (addItemFactory);

        return cache.GetOrAdd
            (
                key,
                addItemFactory,
                new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration }
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <param name="policy"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetOrAdd<T>
        (
            this IAppCache cache,
            string key,
            Func<T> addItemFactory,
            MemoryCacheEntryOptions policy
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (addItemFactory);

        return cache.GetOrAdd (key, _ => addItemFactory(), policy);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task<T> GetOrAddAsync<T>
        (
            this IAppCache cache,
            string key,
            Func<Task<T>> addItemFactory
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (addItemFactory);

        return cache.GetOrAddAsync (key, addItemFactory, cache.DefaultCachePolicy.BuildOptions());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <param name="expires"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task<T> GetOrAddAsync<T>
        (
            this IAppCache cache,
            string key,
            Func<Task<T>> addItemFactory,
            DateTimeOffset expires
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (addItemFactory);

        return cache.GetOrAddAsync
            (
                key,
                addItemFactory,
                new MemoryCacheEntryOptions { AbsoluteExpiration = expires }
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <param name="expires"></param>
    /// <param name="mode"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task<T> GetOrAddAsync<T>
        (
            this IAppCache cache,
            string key,
            Func<Task<T>> addItemFactory,
            DateTimeOffset expires, ExpirationMode mode
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (addItemFactory);

        return mode switch
        {
            ExpirationMode.LazyExpiration => cache.GetOrAddAsync (key, addItemFactory,
                new MemoryCacheEntryOptions { AbsoluteExpiration = expires }),

            _ => cache.GetOrAddAsync (key, addItemFactory,
                new LazyCacheEntryOptions().SetAbsoluteExpiration (expires, mode))
        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <param name="slidingExpiration"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task<T> GetOrAddAsync<T>
        (
            this IAppCache cache,
            string key,
            Func<Task<T>> addItemFactory,
            TimeSpan slidingExpiration
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (addItemFactory);

        return cache.GetOrAddAsync
            (
                key,
                addItemFactory,
                new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration }
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <param name="policy"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task<T> GetOrAddAsync<T>
        (
            this IAppCache cache,
            string key,
            Func<Task<T>> addItemFactory,
            MemoryCacheEntryOptions policy
        )
    {
        Sure.NotNull (cache);
        Sure.NotNullNorEmpty (key);
        Sure.NotNull (addItemFactory);

        return cache.GetOrAddAsync (key, _ => addItemFactory(), policy);
    }

    #endregion
}
