// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* SimpleThemeResources.cs -- доступ к ресурсам темы Simple
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;
using Avalonia.Media;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Avalonia;

/// <summary>
/// Доступ к ресурсам темы Simple.
/// </summary>
[PublicAPI]
public sealed class SimpleThemeResources
    : IThemeResources
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SimpleThemeResources
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
    public string CurrentThemeName => "Simple";

    /// <inheritdoc cref="ThemeBackgroundColor"/>
    public Color ThemeBackgroundColor =>
        (Color) _resourceHost.FindResource ("ThemeBackgroundColor")!;

    /// <inheritdoc cref="IThemeResources.ThemeBackgroundBrush"/>
    public Brush ThemeBackgroundBrush =>
        (Brush) _resourceHost.FindResource ("ThemeBackgroundBrush")!;

    /// <inheritdoc cref="IThemeResources.ThemeForegroundColor"/>
    public Color ThemeForegroundColor =>
        (Color) _resourceHost.FindResource ("ThemeForegroundColor")!;

    /// <inheritdoc cref="IThemeResources.ThemeForegroundBrush"/>
    public Brush ThemeForegroundBrush =>
        (Brush) _resourceHost.FindResource ("ThemeForegroundBrush")!;

    /// <inheritdoc cref="IThemeResources.HighlightColor"/>
    public Color HighlightColor =>
        (Color) _resourceHost.FindResource ("HighlightColor")!;

    /// <inheritdoc cref="IThemeResources.HighlightBrush"/>
    public Brush HighlightBrush =>
        (Brush) _resourceHost.FindResource ("HighlightBrush")!;

    #endregion
}
