// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNullableAnnotationInsteadOfAttribute

/* LazyCacheEntryOptions.cs --
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
public class LazyCacheEntryOptions
    : MemoryCacheEntryOptions
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public ExpirationMode ExpirationMode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public TimeSpan ImmediateAbsoluteExpirationRelativeToNow { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="absoluteExpiration"></param>
    /// <returns></returns>
    public static LazyCacheEntryOptions WithImmediateAbsoluteExpiration
        (
            DateTimeOffset absoluteExpiration
        )
    {
        var delay = absoluteExpiration.Subtract (DateTimeOffset.UtcNow);
        return new LazyCacheEntryOptions
        {
            AbsoluteExpiration = absoluteExpiration,
            ExpirationMode = ExpirationMode.ImmediateEviction,
            ImmediateAbsoluteExpirationRelativeToNow = delay
        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="absoluteExpiration"></param>
    /// <returns></returns>
    public static LazyCacheEntryOptions WithImmediateAbsoluteExpiration
        (
            TimeSpan absoluteExpiration
        )
    {
        return new LazyCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration,
            ExpirationMode = ExpirationMode.ImmediateEviction,
            ImmediateAbsoluteExpirationRelativeToNow = absoluteExpiration
        };
    }

    #endregion
}
