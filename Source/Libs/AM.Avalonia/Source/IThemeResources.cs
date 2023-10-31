// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* IThemeResources.cs -- интерфейс доступа к ресурсам темы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Avalonia.Controls;
using Avalonia.Media;

#endregion

namespace AM.Avalonia;

/// <summary>
/// Интерфейс доступа к ресурсам темы.
/// </summary>
public interface IThemeResources
{
    /// <summary>
    /// Имя текущей темы.
    /// </summary>
    string CurrentThemeName { get; }

    /// <summary>
    /// Цвет фона.
    /// </summary>
    Color ThemeBackgroundColor { get; }

    /// <summary>
    /// Кисть для фона.
    /// </summary>
    Brush ThemeBackgroundBrush { get; }

    /// <summary>
    /// Цвет переднего плана.
    /// </summary>
    Color ThemeForegroundColor { get; }

    /// <summary>
    /// Кисть для переднего плана.
    /// </summary>
    Brush ThemeForegroundBrush { get; }

    /// <summary>
    /// Цвет для подсвечивания.
    /// </summary>
    Color HighlightColor { get; }

    /// <summary>
    /// Кисть для подсвечивания.
    /// </summary>
    Brush HighlightBrush { get; }
}
