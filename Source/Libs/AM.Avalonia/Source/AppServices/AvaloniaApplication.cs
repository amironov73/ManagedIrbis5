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

/* AvaloniaApplication.cs -- приложение на основе Avalonia UI
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.AppServices;

using Microsoft.Extensions.Hosting;

#endregion

#nullable enable

namespace AM.Avalonia.AppServices;

/// <summary>
/// Приложение на основе Avalonia UI.
/// </summary>
public class AvaloniaApplication
    : MagnaApplication
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AvaloniaApplication
        (
            IHostBuilder builder,
            string[]? args = null
        )
        : base(builder, args)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AvaloniaApplication
        (
            string[] args
        )
        : base(args)
    {
    }

    #endregion
}
