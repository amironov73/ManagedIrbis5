// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ButtonBaseExtensions.cs -- методы расширения для ButtonBase
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="ButtonBase"/>.
/// </summary>
public static class ButtonBaseExtensions
{
    #region Public methods

    /// <summary>
    /// Установка плоского режима отображения.
    /// </summary>
    public static TButton FlatStyle<TButton>
        (
            TButton button,
            FlatStyle flatStyle = System.Windows.Forms.FlatStyle.Flat
        )
        where TButton: ButtonBase
    {
        Sure.NotNull (button);

        button.FlatStyle = flatStyle;

        return button;
    }

    /// <summary>
    /// Задание изображения для кнопки.
    /// </summary>
    public static TButton Image<TButton>
        (
            TButton button,
            Image image
        )
        where TButton: ButtonBase
    {
        Sure.NotNull (button);
        Sure.NotNull (image);

        button.Image = image;

        return button;
    }

    /// <summary>
    /// Выравнивание изображения на кнопке.
    /// </summary>
    public static TButton ImageAlign<TButton>
        (
            TButton button,
            ContentAlignment alignment
        )
        where TButton: ButtonBase
    {
        Sure.NotNull (button);
        Sure.Defined (alignment);

        button.ImageAlign = alignment;

        return button;
    }

    #endregion
}
