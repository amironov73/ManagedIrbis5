// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* NullLocalizer.cs -- нулевой локализатор
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using Microsoft.Extensions.Localization;

#endregion

#nullable enable

namespace AM.AppServices;

/// <summary>
/// Нулевой локализатор (заглушка).
/// </summary>
public sealed class NullLocalizer
    : IStringLocalizer
{
    #region IStringLocalizer members

    /// <inheritdoc cref="IStringLocalizer.GetAllStrings"/>
    public IEnumerable<LocalizedString> GetAllStrings
        (
            bool includeParentCultures
        )
    {
        return Array.Empty<LocalizedString>();
    }

    /// <inheritdoc cref="IStringLocalizer.this(string)"/>
    public LocalizedString this [string name] => new (name, name, true);

    /// <inheritdoc cref="IStringLocalizer.this(string,object[])"/>
    public LocalizedString this [string name, params object[] arguments]
        => new (name, name, true);

    #endregion
}
