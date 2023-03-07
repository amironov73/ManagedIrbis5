// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* FluentThemeResources.cs -- доступ к ресурсам темы Fluent
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;
using Avalonia.Media;

#endregion

#nullable enable

namespace AM.Avalonia;

/// <summary>
/// Доступ к ресурсам темы Fluent.
/// </summary>
public class FluentThemeResources
    : IThemeResources
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FluentThemeResources
        (
            IResourceHost resourceHost
        )
    {
        Sure.NotNull (resourceHost);

        _resourceHost = resourceHost;
    }

    #endregion

    #region Private members

    private readonly IResourceHost _resourceHost;

    #endregion

    #region IThemeResources members

    /// <inheritdoc cref="IThemeResources.CurrentThemeName"/>
    public string CurrentThemeName => "Fluent";

    /// <inheritdoc cref="ThemeBackgroundColor"/>
    public Color ThemeBackgroundColor =>
        (Color) _resourceHost.FindResource ("SystemBaseHighColor")!;

    /// <inheritdoc cref="IThemeResources.ThemeBackgroundBrush"/>
    public Brush ThemeBackgroundBrush =>
        (Brush) _resourceHost.FindResource ("SystemControlBackgroundBaseHighBrush")!;

    /// <inheritdoc cref="IThemeResources.ThemeForegroundColor"/>
    public Color ThemeForegroundColor =>
        (Color) _resourceHost.FindResource ("SystemAltHighColor")!;

    /// <inheritdoc cref="IThemeResources.ThemeForegroundBrush"/>
    public Brush ThemeForegroundBrush =>
        (Brush) _resourceHost.FindResource ("SystemControlBackgroundAltHighBrush")!;

    /// <inheritdoc cref="IThemeResources.HighlightColor"/>
    public Color HighlightColor =>
        (Color) _resourceHost.FindResource ("SystemAccentColor")!;

    /// <inheritdoc cref="IThemeResources.HighlightBrush"/>
    public Brush HighlightBrush =>
        (Brush) _resourceHost.FindResource ("SystemControlBackgroundAccentBrush")!;

    #endregion
}
