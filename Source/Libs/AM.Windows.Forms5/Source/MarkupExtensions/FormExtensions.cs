// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LabelExtensions.cs -- методы расширения для Form
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="Form"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class FormExtensions
{
    #region Public methods

    /// <summary>
    /// Добавление кнопки, срабатывающей по <c>Enter</c>.
    /// </summary>
    public static TForm AcceptButton<TForm>
        (
            this TForm form,
            Button button
        )
        where TForm: Form
    {
        Sure.NotNull (form);
        Sure.NotNull (button);

        form.AcceptButton = button;

        return form;
    }

    /// <summary>
    /// Установка режима автоматического вычисления размера.
    /// </summary>
    public static TForm AutoSize<TForm>
        (
            this TForm form,
            bool autoSize = true,
            AutoSizeMode mode = AutoSizeMode.GrowAndShrink
        )
    where TForm: Form
    {
        Sure.NotNull (form);

        form.AutoSize = autoSize;
        if (autoSize)
        {
            form.AutoSizeMode = mode;
        }

        return form;
    }

    /// <summary>
    /// Добавление кнопки, срабатывающей по <c>Escape</c>.
    /// </summary>
    public static TForm CancelButton<TForm>
        (
            this TForm form,
            Button button
        )
        where TForm: Form
    {
        Sure.NotNull (form);
        Sure.NotNull (button);

        form.CancelButton = button;

        return form;
    }

    /// <summary>
    /// Установка расположения формы по центру экрана.
    /// </summary>
    public static TForm CenterScreen<TForm>
        (
            this TForm form
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.StartPosition = FormStartPosition.CenterScreen;

        return form;
    }

    /// <summary>
    /// Установка иконки для формы.
    /// </summary>
    public static TForm Icon<TForm>
        (
            this TForm form,
            Icon icon
        )
        where TForm: Form
    {
        Sure.NotNull (form);
        Sure.NotNull (icon);

        form.Icon = icon;

        return form;
    }

    /// <summary>
    /// Добавление главного меню.
    /// </summary>
    public static TForm MainMenu<TForm>
        (
            this TForm form,
            params ToolStripItem[] items
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.Controls (new MenuStrip().Items (items));

        return form;
    }

    /// <summary>
    /// Задание начальной позиции формы.
    /// </summary>
    public static TForm StartPosition<TForm>
        (
            this TForm form,
            FormStartPosition position
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.StartPosition = position;

        return form;
    }

    /// <summary>
    /// Добавление статусной строки.
    /// </summary>
    public static TForm StatusStrip<TForm>
        (
            this TForm form,
            params ToolStripItem[] items
        )
        where TForm: Form
    {
        Sure.NotNull (form);
        Sure.NotNull (items);

        form.Controls (new StatusStrip().Items (items));

        return form;
    }

    #endregion
}
