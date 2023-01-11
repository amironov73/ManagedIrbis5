// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global

/*
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
public interface IAppCache
{
    /// <summary>
    ///
    /// </summary>
    ICacheProvider CacheProvider { get; }

    /// <summary>
    ///     Define the number of seconds to cache objects for by default
    /// </summary>
    CacheDefaults DefaultCachePolicy { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <param name="policy"></param>
    /// <typeparam name="T"></typeparam>
    void Add<T>
        (
            string key,
            T item,
            MemoryCacheEntryOptions policy
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T Get<T>
        (
            string key
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> GetAsync<T>
        (
            string key
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    bool TryGetValue<T>
        (
            string key,
            out T value
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetOrAdd<T>
        (
            string key,
            Func<ICacheEntry, T> addItemFactory
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <param name="policy"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetOrAdd<T>
        (
            string key,
            Func<ICacheEntry, T> addItemFactory,
            MemoryCacheEntryOptions policy
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> GetOrAddAsync<T>
        (
            string key,
            Func<ICacheEntry, Task<T>> addItemFactory
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="addItemFactory"></param>
    /// <param name="policy"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> GetOrAddAsync<T>
        (
            string key,
            Func<ICacheEntry, Task<T>> addItemFactory,
            MemoryCacheEntryOptions policy
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    void Remove
        (
            string key
        );
}
