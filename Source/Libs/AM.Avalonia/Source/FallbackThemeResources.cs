// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* FallbackThemeResources.cs -- резервная реализация IThemeResources
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Media;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Avalonia;

/// <summary>
/// Резервная реализация <see cref="IThemeResources"/>
/// на случай, если мы не сможем определить тему.
/// </summary>
[PublicAPI]
public sealed class FallbackThemeResources
    : IThemeResources
{
    #region Private members

    private static readonly Brush _backgroundBrush = new SolidColorBrush (Colors.White);
    private static readonly Brush _foregroundBrush = new SolidColorBrush (Colors.Black);
    private static readonly Brush _highlightBrush = new SolidColorBrush (Colors.Aqua);

    #endregion
    
    #region IThemeResources members

    /// <inheritdoc cref="IThemeResources.CurrentThemeName"/>
    public string CurrentThemeName => "Default";

    /// <inheritdoc cref="ThemeBackgroundColor"/>
    public Color ThemeBackgroundColor => Colors.White;

    /// <inheritdoc cref="IThemeResources.ThemeBackgroundBrush"/>
    public Brush ThemeBackgroundBrush => _backgroundBrush;

    /// <inheritdoc cref="IThemeResources.ThemeForegroundColor"/>
    public Color ThemeForegroundColor => Colors.Black;

    /// <inheritdoc cref="IThemeResources.ThemeForegroundBrush"/>
    public Brush ThemeForegroundBrush => _foregroundBrush;

    /// <inheritdoc cref="IThemeResources.HighlightColor"/>
    public Color HighlightColor => Colors.Aqua;

    /// <inheritdoc cref="IThemeResources.HighlightBrush"/>
    public Brush HighlightBrush => _highlightBrush;


    #endregion
}
