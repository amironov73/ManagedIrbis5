// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ComboBoxExtensions.cs -- методы расширения для ComboBox
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="ComboBox"/>.
/// </summary>
public static class ComboBoxExtensions
{
    #region Public methods

    /// <summary>
    /// Установка режима автозавершения ввода.
    /// </summary>
    public static TComboBox AutoCompleteMode<TComboBox>
        (
            TComboBox comboBox,
            AutoCompleteMode mode
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);
        Sure.Defined (mode);

        comboBox.AutoCompleteMode = mode;

        return comboBox;
    }

    /// <summary>
    /// Установка источника данных для автозавершения ввода.
    /// </summary>
    public static TComboBox AutoCompleteSource<TComboBox>
        (
            TComboBox comboBox,
            AutoCompleteSource source
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);
        Sure.Defined (source);

        comboBox.AutoCompleteSource = source;

        return comboBox;
    }

    /// <summary>
    /// Установка режима отрисовки.
    /// </summary>
    public static TComboBox DrawMode<TComboBox>
        (
            TComboBox comboBox,
            DrawMode drawMode
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);
        Sure.Defined (drawMode);

        comboBox.DrawMode = drawMode;

        return comboBox;
    }

    /// <summary>
    /// Установка высоты выпадающего списка.
    /// </summary>
    public static TComboBox DropDownHeight<TComboBox>
        (
            TComboBox comboBox,
            int height
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);
        Sure.Positive (height);

        comboBox.DropDownHeight = height;

        return comboBox;
    }

    /// <summary>
    /// Установка стиля выпадающего списка.
    /// </summary>
    public static TComboBox DropDownList<TComboBox>
        (
            TComboBox comboBox
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);

        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        return comboBox;
    }

    /// <summary>
    /// Установка стиля выпадающего списка.
    /// </summary>
    public static TComboBox DropDownStyle<TComboBox>
        (
            TComboBox comboBox,
            ComboBoxStyle style
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);
        Sure.Defined (style);

        comboBox.DropDownStyle = style;

        return comboBox;
    }

    /// <summary>
    /// Установка ширины выпадающего списка.
    /// </summary>
    public static TComboBox DropDownWidth<TComboBox>
        (
            TComboBox comboBox,
            int width
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);
        Sure.Positive (width);

        comboBox.DropDownWidth = width;

        return comboBox;
    }

    /// <summary>
    /// Установка плоского стиля.
    /// </summary>
    public static TComboBox FlatStyle<TComboBox>
        (
            TComboBox comboBox,
            FlatStyle flatStyle
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);
        Sure.Defined (flatStyle);

        comboBox.FlatStyle = flatStyle;

        return comboBox;
    }

    /// <summary>
    /// Установка режима показа элементов целиком.
    /// </summary>
    public static TComboBox IntegralHeight<TComboBox>
        (
            TComboBox comboBox,
            bool enabled = true
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);

        comboBox.IntegralHeight = enabled;

        return comboBox;
    }

    /// <summary>
    /// Установка высоты элементов.
    /// </summary>
    public static TComboBox ItemHeight<TComboBox>
        (
            TComboBox comboBox,
            int height
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);
        Sure.Positive (height);

        comboBox.ItemHeight = height;

        return comboBox;
    }

    /// <summary>
    /// Добавление элементов в выпадающий список.
    /// </summary>
    public static TComboBox Items<TComboBox>
        (
            TComboBox comboBox,
            params object[] items
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);
        Sure.NotNull (items);

        comboBox.Items.AddRange (items);

        return comboBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="ComboBox.DoubleClick"/>.
    /// </summary>
    public static TComboBox OnDoubleClick<TComboBox>
        (
            TComboBox comboBox,
            EventHandler handler
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);
        Sure.NotNull (handler);

        comboBox.DoubleClick += handler;

        return comboBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="ComboBox.SelectedIndexChanged"/>.
    /// </summary>
    public static TComboBox OnSelectedIndexChanged<TComboBox>
        (
            TComboBox comboBox,
            EventHandler handler
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);
        Sure.NotNull (handler);

        comboBox.SelectedIndexChanged += handler;

        return comboBox;
    }

    /// <summary>
    /// Включение сортировки элементов.
    /// </summary>
    public static TComboBox Sorted<TComboBox>
        (
            TComboBox comboBox,
            bool sorted = true
        )
        where TComboBox: ComboBox
    {
        Sure.NotNull (comboBox);

        comboBox.Sorted = sorted;

        return comboBox;
    }

    #endregion
}
