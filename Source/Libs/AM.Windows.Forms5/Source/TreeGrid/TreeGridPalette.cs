// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridPalette.cs -- палитра цветов для TreeGrid
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Палитра цветов для <see cref="TreeGrid"/>.
/// </summary>
public sealed class TreeGridPalette
    : Palette
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TreeGridPalette()
    {
        InitializeFromAttributes();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Цвет текста.
    /// </summary>
    [PaletteColor ("Black")]
    public Tube Foreground => GetTubeFromProperty();

    /// <summary>
    /// Цвет фона.
    /// </summary>
    [PaletteColor ("White")]
    public Tube Backrground => GetTubeFromProperty();

    /// <summary>
    /// Цвет текста в выбранном узле.
    /// </summary>
    [PaletteColor ("White")]
    public Tube SelectedForeground => GetTubeFromProperty();

    /// <summary>
    /// Цвет фона в выбранном узле.
    /// </summary>
    [PaletteColor ("Blue")]
    public Tube SelectedBackground => GetTubeFromProperty();

    /// <summary>
    /// Цвет фона для узла только для чтения.
    /// </summary>
    [PaletteColor ("Gray")]
    public Tube ReadOnlyBackground => GetTubeFromProperty();

    /// <summary>
    /// Цвет текста для узла только для чтения.
    /// </summary>
    [PaletteColor ("DarkGray")]
    public Tube ReadOnlyForeground => GetTubeFromProperty();

    /// <summary>
    /// Цвет для линий.
    /// </summary>
    [PaletteColor ("DarkGray")]
    public Tube Lines => GetTubeFromProperty();

    /// <summary>
    /// Цвет для неактивных узлов.
    /// </summary>
    [PaletteColor ("Gray")]
    public Tube Disabled => GetTubeFromProperty();

    /// <summary>
    /// Gets the header background.
    /// </summary>
    /// <value>The header background.</value>
    [PaletteColor ("LightGray")]
    public Tube HeaderBackground => GetTubeFromProperty();

    /// <summary>
    /// Цвет текста в заголовке.
    /// </summary>
    [PaletteColor ("Black")]
    public Tube HeaderForeground => GetTubeFromProperty();

    #endregion
}
