// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DataGridViewCellStyleExtensions.cs -- методы расширения для DataGridViewCellStyle
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="DataGridViewCellStyle"/>.
/// </summary>
public static class DataGridViewCellStyleExtensions
{
    #region Public methods

    /// <summary>
    /// Выравнивание контента в ячейке.
    /// </summary>
    public static TStyle Alignment<TStyle>
        (
            this TStyle style,
            DataGridViewContentAlignment alignment
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);
        Sure.Defined (alignment);

        style.Alignment = alignment;

        return style;
    }

    /// <summary>
    /// Выравнивание контента в ячейке.
    /// </summary>
    public static TStyle AlignmentMiddleCenter<TStyle>
        (
            this TStyle style
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.Alignment = DataGridViewContentAlignment.MiddleCenter;

        return style;
    }

    /// <summary>
    /// Выравнивание контента в ячейке.
    /// </summary>
    public static TStyle AlignmentMiddleLeft<TStyle>
        (
            this TStyle style
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.Alignment = DataGridViewContentAlignment.MiddleLeft;

        return style;
    }

    /// <summary>
    /// Выравнивание контента в ячейке.
    /// </summary>
    public static TStyle AlignmentMiddleRight<TStyle>
        (
            this TStyle style
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.Alignment = DataGridViewContentAlignment.MiddleRight;

        return style;
    }

    /// <summary>
    /// Цвет фона в ячейке.
    /// </summary>
    public static TStyle BackColor<TStyle>
        (
            this TStyle style,
            Color color
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.BackColor = color;

        return style;
    }

    /// <summary>
    /// Значение, передаваемое в <see cref="DataGridView.DataSource"/>,
    /// когда пользователь ввел в ячейку пустое значение.
    /// </summary>
    public static TStyle DataSourceNullValue<TStyle>
        (
            this TStyle style,
            object? nullValue
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.DataSourceNullValue = nullValue;

        return style;
    }

    /// <summary>
    /// Шрифт, используемый в ячейке.
    /// </summary>
    public static TStyle Font<TStyle>
        (
            this TStyle style,
            Font font

        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);
        Sure.NotNull (font);

        style.Font = font;

        return style;
    }

    /// <summary>
    /// Цвет текста в ячейке.
    /// </summary>
    public static TStyle ForeColor<TStyle>
        (
            this TStyle style,
            Color color

        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.ForeColor = color;

        return style;
    }

    /// <summary>
    /// Формат, используемый для отображения данных в ячейке.
    /// </summary>
    public static TStyle Format<TStyle>
        (
            this TStyle style,
            string? format

        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.Format = format;

        return style;
    }

    /// <summary>
    /// Провайдер формата для ячейки.
    /// </summary>
    public static TStyle FormatProvider<TStyle>
        (
            this TStyle style,
            IFormatProvider formatProvider
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.FormatProvider = formatProvider;

        return style;
    }

    /// <summary>
    /// Значение, отображамое, когда пользователь ввел в ячейку
    /// пустое значение.
    /// </summary>
    public static TStyle NullValue<TStyle>
        (
            this TStyle style,
            object? nullValue
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.NullValue = nullValue;

        return style;
    }

    /// <summary>
    /// Отступы внутри ячейки.
    /// </summary>
    public static TStyle Padding<TStyle>
        (
            this TStyle style,
            Padding padding
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.Padding = padding;

        return style;
    }

    /// <summary>
    /// Отступы внутри ячейки.
    /// </summary>
    public static TStyle Padding<TStyle>
        (
            this TStyle style,
            params int[] padding
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);
        Sure.NotNull (padding);

        style.Padding = padding.Length switch
        {
            1 => new Padding (padding[0]),
            2 => new Padding (padding[0], padding[1], padding[0], padding[1]),
            4 => new Padding (padding[0], padding[1], padding[2], padding[3]),
            _ => throw new ArgumentException ("Wrong padding specification", nameof (padding))
        };

        return style;
    }

    /// <summary>
    /// Цвет фона для выделенного фрагмента в ячейке.
    /// </summary>
    public static TStyle SelectionBackColor<TStyle>
        (
            this TStyle style,
            Color color
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.SelectionBackColor = color;

        return style;
    }

    /// <summary>
    /// Цвет текста для выделенного фрагмента в ячейке.
    /// </summary>
    public static TStyle SelectionForeColor<TStyle>
        (
            this TStyle style,
            Color color
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.SelectionForeColor = color;

        return style;
    }

    /// <summary>
    /// Произвольные дополнительные данные.
    /// </summary>
    public static TStyle Tag<TStyle>
        (
            this TStyle style,
            object? tag
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.Tag = tag;

        return style;
    }

    /// <summary>
    /// Режим переноса строк в ячейке.
    /// </summary>
    public static TStyle Tag<TStyle>
        (
            this TStyle style,
            DataGridViewTriState wrapMode
        )
        where TStyle: DataGridViewCellStyle
    {
        Sure.NotNull (style);

        style.WrapMode = wrapMode;

        return style;
    }

    #endregion
}
