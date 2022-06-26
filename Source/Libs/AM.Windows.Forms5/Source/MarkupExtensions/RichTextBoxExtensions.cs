// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RichTextBoxExtensions.cs -- методы расширения для RichTextBox
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="RichTextBox"/>.
/// </summary>
public static class RichTextBoxExtensions
{
    #region Public methods

    /// <summary>
    /// Задание отступа для маркированных списков.
    /// </summary>
    public static TRichTextBox BulletIndent<TRichTextBox>
        (
            this TRichTextBox textBox,
            int indent
        )
        where TRichTextBox : RichTextBox
    {
        Sure.NotNull (textBox);
        Sure.NonNegative (indent);

        textBox.BulletIndent = indent;

        return textBox;
    }

    /// <summary>
    /// Разрешение обнаружения интернет-адресов.
    /// </summary>
    public static TRichTextBox DetectUrls<TRichTextBox>
        (
            this TRichTextBox textBox,
            bool enabled = true
        )
        where TRichTextBox : RichTextBox
    {
        Sure.NotNull (textBox);

        textBox.DetectUrls = enabled;

        return textBox;
    }

    /// <summary>
    /// Разрешение автоматической обработки Drag/Drop.
    /// </summary>
    public static TRichTextBox EnableAutoDragDrop<TRichTextBox>
        (
            this TRichTextBox textBox,
            bool enabled = true
        )
        where TRichTextBox : RichTextBox
    {
        Sure.NotNull (textBox);

        textBox.EnableAutoDragDrop = enabled;

        return textBox;
    }

    /// <summary>
    /// Языковая опция.
    /// </summary>
    public static TRichTextBox LanguageOption<TRichTextBox>
        (
            this TRichTextBox textBox,
            RichTextBoxLanguageOptions option
        )
        where TRichTextBox : RichTextBox
    {
        Sure.NotNull (textBox);

        textBox.LanguageOption = option;

        return textBox;
    }

    /// <summary>
    /// Отступ справа для текста.
    /// </summary>
    public static TRichTextBox RightMargin<TRichTextBox>
        (
            this TRichTextBox textBox,
            int margin
        )
        where TRichTextBox : RichTextBox
    {
        Sure.NotNull (textBox);

        textBox.RightMargin = margin;

        return textBox;
    }

    /// <summary>
    /// Загрузка RTF в редактор.
    /// </summary>
    public static TRichTextBox Rtf<TRichTextBox>
        (
            this TRichTextBox textBox,
            string rtf
        )
        where TRichTextBox : RichTextBox
    {
        Sure.NotNull (textBox);
        Sure.NotNull (rtf);

        textBox.Rtf = rtf;

        return textBox;
    }

    /// <summary>
    /// Установка полос прокрутки.
    /// </summary>
    public static TRichTextBox ScrollBars<TRichTextBox>
        (
            this TRichTextBox textBox,
            RichTextBoxScrollBars scrollBars
        )
        where TRichTextBox : RichTextBox
    {
        Sure.NotNull (textBox);

        textBox.ScrollBars = scrollBars;

        return textBox;
    }

    /// <summary>
    /// Задание увеличения.
    /// </summary>
    public static TRichTextBox ZoomFactor<TRichTextBox>
        (
            this TRichTextBox textBox,
            float factor
        )
        where TRichTextBox : RichTextBox
    {
        Sure.NotNull (textBox);

        textBox.ZoomFactor = factor;

        return textBox;
    }

    #endregion
}
