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
            this TButton button,
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
            this TButton button,
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
            this TButton button,
            ContentAlignment alignment
        )
        where TButton: ButtonBase
    {
        Sure.NotNull (button);
        Sure.Defined (alignment);

        button.ImageAlign = alignment;

        return button;
    }

    /// <summary>
    /// Порядковый номер картинки.
    /// </summary>
    public static TButton ImageIndex<TButton>
        (
            this TButton button,
            int index
        )
        where TButton: ButtonBase
    {
        Sure.NotNull (button);
        Sure.NonNegative (index);

        button.ImageIndex = index;

        return button;
    }

    /// <summary>
    /// Ключ картинки.
    /// </summary>
    public static TButton ImageKey<TButton>
        (
            this TButton button,
            string key
        )
        where TButton: ButtonBase
    {
        Sure.NotNull (button);
        Sure.NotNullNorEmpty (key);

        button.ImageKey = key;

        return button;
    }

    /// <summary>
    /// Список картинок.
    /// </summary>
    public static TButton ImageList<TButton>
        (
            this TButton button,
            ImageList imageList
        )
        where TButton: ButtonBase
    {
        Sure.NotNull (button);
        Sure.NotNull (imageList);

        button.ImageList = imageList;

        return button;
    }

    /// <summary>
    /// Выравнивание текста.
    /// </summary>
    public static TButton TextAlign<TButton>
        (
            this TButton button,
            ContentAlignment alignment
        )
        where TButton: ButtonBase
    {
        Sure.NotNull (button);
        Sure.Defined (alignment);

        button.TextAlign = alignment;

        return button;
    }

    /// <summary>
    /// Соотношение текста и картинки.
    /// </summary>
    public static TButton TextImageRelation<TButton>
        (
            this TButton button,
            TextImageRelation relation
        )
        where TButton: ButtonBase
    {
        Sure.NotNull (button);
        Sure.Defined (relation);

        button.TextImageRelation = relation;

        return button;
    }

    /// <summary>
    /// Использование мнемоники.
    /// </summary>
    public static TButton UseMnemonic<TButton>
        (
            this TButton button,
            bool useMnemonic = true
        )
        where TButton: ButtonBase
    {
        Sure.NotNull (button);

        button.UseMnemonic = useMnemonic;

        return button;
    }

    #endregion
}
