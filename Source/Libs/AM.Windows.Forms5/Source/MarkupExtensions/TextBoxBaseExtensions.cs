// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TextBoxBaseExtensions.cs -- методы расширения для TextBoxBase
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="TextBoxBase"/>.
/// </summary>
public static class TextBoxBaseExtensions
{
    #region Public methods

    /// <summary>
    /// Текстбокс принимает клавишу <c>Tab</c>.
    /// </summary>
    public static TTextBoxBase AcceptsTab<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            bool accepts = true
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);

        textBox.AcceptsTab = accepts;

        return textBox;
    }

    /// <summary>
    /// Задание стиля рамки для текстового поля ввода.
    /// </summary>
    public static TTextBoxBase BorderStyle<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            BorderStyle borderStyle
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);
        Sure.Defined (borderStyle);

        textBox.BorderStyle = borderStyle;

        return textBox;
    }

    /// <summary>
    /// Задание комфортабельных (для меня!) настроек
    /// многострочного текстбокса.
    /// </summary>
    public static TTextBoxBase ComfortableMultiline<TTextBoxBase>
        (
            this TTextBoxBase textBox
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);

        textBox.Multiline = true;
        textBox.WordWrap = true;

        return textBox;
    }

    /// <summary>
    /// Текстбокс прячет подсветку выделенного текста
    /// при потере фокуса?
    /// </summary>
    public static TTextBoxBase HideSelection<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            bool hide = true
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);

        textBox.HideSelection = hide;

        return textBox;
    }

    /// <summary>
    /// Текст построчно.
    /// </summary>
    public static TTextBoxBase Lines<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            string[] lines
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);
        Sure.NotNull (lines);

        textBox.Lines = lines;

        return textBox;
    }

    /// <summary>
    /// Задание максимальной длины текста.
    /// </summary>
    public static TTextBoxBase MaxLength<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            int maxLength
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);
        Sure.Positive (maxLength);

        textBox.MaxLength = maxLength;

        return textBox;
    }

    /// <summary>
    /// Включение многострочного режима.
    /// </summary>
    public static TTextBoxBase Multiline<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            bool multiline = true
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);

        textBox.Multiline = multiline;

        return textBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="TextBoxBase.Click"/>
    /// </summary>
    public static TTextBoxBase OnClick<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            EventHandler handler
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);
        Sure.NotNull (handler);

        textBox.Click += handler;

        return textBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="TextBoxBase.ModifiedChanged"/>
    /// </summary>
    public static TTextBoxBase OnModifiedChanged<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            EventHandler handler
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);
        Sure.NotNull (handler);

        textBox.ModifiedChanged += handler;

        return textBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="TextBoxBase.MouseClick"/>
    /// </summary>
    public static TTextBoxBase OnMouseClick<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            MouseEventHandler handler
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);
        Sure.NotNull (handler);

        textBox.MouseClick += handler;

        return textBox;
    }

    /// <summary>
    /// Включение режима только для чтения.
    /// </summary>
    public static TTextBoxBase ReadOnly<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            bool readOnly = true
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);

        textBox.ReadOnly = readOnly;

        return textBox;
    }

    /// <summary>
    /// Задание выбранного текста.
    /// </summary>
    public static TTextBoxBase SelectedText<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            string selectedText
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);
        Sure.NotNull (selectedText);

        textBox.SelectedText = selectedText;

        return textBox;
    }

    /// <summary>
    /// Задание длины выбранного текста.
    /// </summary>
    public static TTextBoxBase SelectionLength<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            int selectionLength
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);
        Sure.NonNegative (selectionLength);

        textBox.SelectionLength = selectionLength;

        return textBox;
    }

    /// <summary>
    /// Задание позиции начала выбранного текста.
    /// </summary>
    public static TTextBoxBase SelectionStart<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            int selectionStart
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);
        Sure.NonNegative (selectionStart);

        textBox.SelectionLength = selectionStart;

        return textBox;
    }

    /// <summary>
    /// Разрешение/запрещение клавиатурных сокращений.
    /// </summary>
    public static TTextBoxBase ShortcutsEnabled<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            bool enabled
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);

        textBox.ShortcutsEnabled = enabled;

        return textBox;
    }

    /// <summary>
    /// Разрешение/запрещение переноса текста по словам.
    /// </summary>
    public static TTextBoxBase WordWrap<TTextBoxBase>
        (
            this TTextBoxBase textBox,
            bool enabled = true
        )
        where TTextBoxBase: TextBoxBase
    {
        Sure.NotNull (textBox);

        textBox.WordWrap = enabled;

        return textBox;
    }

    #endregion
}
