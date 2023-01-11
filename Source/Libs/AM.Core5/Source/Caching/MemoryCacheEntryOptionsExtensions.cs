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

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Caching;

/// <summary>
///
/// </summary>
public static class LazyCacheEntryOptionsExtension
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="option"></param>
    /// <param name="absoluteExpiration"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static LazyCacheEntryOptions SetAbsoluteExpiration
        (
            this LazyCacheEntryOptions option,
            DateTimeOffset absoluteExpiration,
            ExpirationMode mode
        )
    {
        Sure.NotNull (option);

        var delay = absoluteExpiration.Subtract (DateTimeOffset.UtcNow);
        option.AbsoluteExpiration = absoluteExpiration;
        option.ExpirationMode = mode;
        option.ImmediateAbsoluteExpirationRelativeToNow = delay;
        return option;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="option"></param>
    /// <param name="absoluteExpiration"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static LazyCacheEntryOptions SetAbsoluteExpiration
        (
            this LazyCacheEntryOptions option,
            TimeSpan absoluteExpiration,
            ExpirationMode mode
        )
    {
        Sure.NotNull (option);

        option.AbsoluteExpirationRelativeToNow = absoluteExpiration;
        option.ExpirationMode = mode;
        option.ImmediateAbsoluteExpirationRelativeToNow = absoluteExpiration;
        return option;
    }

    #endregion
}
