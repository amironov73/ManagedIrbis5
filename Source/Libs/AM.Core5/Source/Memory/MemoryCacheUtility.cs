// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* MemoryCacheUtility.cs -- полезные методы для IMemoryCache
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Reflection;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace AM.Memory;

/// <summary>
/// Полезные методы для <see cref="IMemoryCache"/>.
/// </summary>
public static class MemoryCacheUtility
{
    #region Public methods

    /// <summary>
    /// Пытается очистить кеш.
    /// </summary>
    /// <returns>
    /// Признак успешного завершения операции.
    /// </returns>
    public static bool Clear<T>
        (
            this T cache
        )
        where T: class, IMemoryCache
    {
        Sure.NotNull (cache);

        if (cache is MemoryCache memoryCache)
        {
            memoryCache.Compact (1.0);
            return true;
        }

        var clearMethod = cache.GetType().GetMethod
            (
                "Clear",
                BindingFlags.Instance | BindingFlags.Public
            );
        if (clearMethod is not null)
        {
            clearMethod.Invoke (cache, null);
            return true;
        }

        return false;
    }

    #endregion
}
