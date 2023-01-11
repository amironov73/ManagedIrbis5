// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CacheDefaults.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace AM.Caching;

/// <summary>
///
/// </summary>
public class CacheDefaults
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public virtual int DefaultCacheDurationSeconds { get; set; } = 60 * 20;

    #endregion

    #region Internal members

    internal MemoryCacheEntryOptions BuildOptions()
    {
        return new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds (DefaultCacheDurationSeconds)
        };
    }

    #endregion
}
