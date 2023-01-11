// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Caching;

/// <summary>
///
/// </summary>
public enum ExpirationMode
{
    /// <summary>
    ///     This is the default for Memory cache - expired items are removed from the cache
    ///     the next time that key is accessed. This is the most performant, and so the default,
    ///     because no timers are required to removed expired items, but it does mean that
    ///     PostEvictionCallbacks may fire later than expected, or not at all.
    /// </summary>
    LazyExpiration,

    /// <summary>
    ///     Use a timer to force eviction of expired items from the cache as soon as they expire.
    ///     This will then trigger PostEvictionCallbacks at the expected time. This uses more resources
    ///     than LazyExpiration.
    /// </summary>
    ImmediateEviction
}
