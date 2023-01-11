// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ICacheProvider.cs --
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
public interface ICacheProvider
    : IDisposable
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="item"></param>
    /// <param name="policy"></param>
    void Set
        (
            string key,
            object item,
            MemoryCacheEntryOptions policy
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    object? Get
        (
            string key
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="func"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    object? GetOrCreate<T>
        (
            string key,
            Func<ICacheEntry, T> func
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="policy"></param>
    /// <param name="func"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    object? GetOrCreate<T>
        (
            string key,
            MemoryCacheEntryOptions policy,
            Func<ICacheEntry, T> func
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    void Remove
        (
            string key
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="func"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> GetOrCreateAsync<T>
        (
            string key,
            Func<ICacheEntry, Task<T>> func
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
            object key,
            out T value
        );
}
