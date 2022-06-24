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

using System;
using System.ComponentModel;
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
    public static TForm AutoSizeMode<TForm>
        (
            this TForm form,
            bool autoSize = true,
            AutoSizeMode mode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
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
    /// Отображение системных кнопок в заголовке формы.
    /// </summary>
    public static TForm ControlBox<TForm>
        (
            this TForm form,
            bool show = true
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.ControlBox = show;

        return form;
    }

    /// <summary>
    /// Форма в виде диалога фиксированного размера.
    /// </summary>
    public static TForm FixedDialog<TForm>
        (
            this TForm form
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

        return form;
    }

    /// <summary>
    /// Форма в виде фиксированного служебного окна.
    /// </summary>
    public static TForm FixedToolWindow<TForm>
        (
            this TForm form
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;

        return form;
    }

    /// <summary>
    /// Стиль рамки формы.
    /// </summary>
    public static TForm FormBorderStyle<TForm>
        (
            this TForm form,
            FormBorderStyle borderStyle
        )
        where TForm: Form
    {
        Sure.NotNull (form);
        Sure.Defined (borderStyle);

        form.FormBorderStyle = borderStyle;

        return form;
    }

    /// <summary>
    /// Отображение кнопки помощи в заголовке формы.
    /// </summary>
    public static TForm HelpButton<TForm>
        (
            this TForm form,
            bool show = true
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.HelpButton = show;

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
    /// Перехват нажатий клавиш до дочерних контролов.
    /// </summary>
    public static TForm KeyPreview<TForm>
        (
            this TForm form,
            bool enabled = true
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.KeyPreview = enabled;

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
    /// Установка промежутка между контролами.
    /// </summary>
    public static TForm Margin<TForm>
        (
            this TForm form,
            Padding margin
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.Margin = margin;

        return form;
    }

    /// <summary>
    /// Установка промежутка между контролами.
    /// </summary>
    public static TForm Margin<TForm>
        (
            this TForm form,
            int margin
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.Margin = new Padding (margin);

        return form;
    }

    /// <summary>
    /// Кнопка "Развернуть" в заголовке формы.
    /// </summary>
    public static TForm MaximizeBox<TForm>
        (
            this TForm form,
            bool show = true
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.MaximizeBox = show;

        return form;
    }

    /// <summary>
    /// Максимальный размер формы.
    /// </summary>
    public static TForm MaximumSize<TForm>
        (
            this TForm form,
            int width,
            int height
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.MaximumSize = new Size (width, height);

        return form;
    }

    /// <summary>
    /// Кнопка "Свернуть" в заголовке формы.
    /// </summary>
    public static TForm MinimizeBox<TForm>
        (
            this TForm form,
            bool show = true
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.MinimizeBox = show;

        return form;
    }

    /// <summary>
    /// Минимальный размер формы.
    /// </summary>
    public static TForm MinimumSize<TForm>
        (
            this TForm form,
            int width,
            int height
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.MinimumSize = new Size (width, height);

        return form;
    }

    /// <summary>
    /// Подписка на событие <see cref="Form.Activated"/>.
    /// </summary>
    public static TForm OnActivated<TForm>
        (
            this TForm form,
            EventHandler handler
        )
        where TForm: Form
    {
        Sure.NotNull (form);
        Sure.NotNull (handler);

        form.Activated += handler;

        return form;
    }

    /// <summary>
    /// Подписка на событие <see cref="Form.Closed"/>.
    /// </summary>
    public static TForm OnClosed<TForm>
        (
            this TForm form,
            EventHandler handler
        )
        where TForm: Form
    {
        Sure.NotNull (form);
        Sure.NotNull (handler);

        form.Closed += handler;

        return form;
    }

    /// <summary>
    /// Подписка на событие <see cref="Form.Closing"/>.
    /// </summary>
    public static TForm OnClosing<TForm>
        (
            this TForm form,
            CancelEventHandler handler
        )
        where TForm: Form
    {
        Sure.NotNull (form);
        Sure.NotNull (handler);

        form.Closing += handler;

        return form;
    }

    /// <summary>
    /// Подписка на событие <see cref="Form.Load"/>.
    /// </summary>
    public static TForm OnLoad<TForm>
        (
            this TForm form,
            EventHandler handler
        )
        where TForm: Form
    {
        Sure.NotNull (form);
        Sure.NotNull (handler);

        form.Load += handler;

        return form;
    }

    /// <summary>
    /// Степень прозрачности формы.
    /// </summary>
    public static TForm Opacity<TForm>
        (
            this TForm form,
            double opacity
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.Opacity = opacity;

        return form;
    }

    /// <summary>
    /// Отображение иконки в заголовке окна.
    /// </summary>
    public static TForm ShowIcon<TForm>
        (
            this TForm form,
            bool show
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.ShowIcon = show;

        return form;
    }

    /// <summary>
    /// Отображение иконки формы в панели задач.
    /// </summary>
    public static TForm ShowInTaskbar<TForm>
        (
            this TForm form,
            bool show
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.ShowInTaskbar = show;

        return form;
    }

    /// <summary>
    /// Стиль нижней правой части, используемой для изменения размера.
    /// </summary>
    public static TForm SizeGripStyle<TForm>
        (
            this TForm form,
            SizeGripStyle style
        )
        where TForm: Form
    {
        Sure.NotNull (form);
        Sure.Defined (style);

        form.SizeGripStyle = style;

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

    /// <summary>
    /// Отображение среди окон верхнего уровня.
    /// </summary>
    public static TForm TopLevel<TForm>
        (
            this TForm form,
            bool topLevel = true
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.TopLevel = topLevel;

        return form;
    }

    /// <summary>
    /// Окно поверх остальных окон.
    /// </summary>
    public static TForm TopMost<TForm>
        (
            this TForm form,
            bool topMost = true
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.TopMost = topMost;

        return form;
    }

    /// <summary>
    /// Задание прозрачного цвета.
    /// </summary>
    public static TForm TransparencyKey<TForm>
        (
            this TForm form,
            Color color
        )
        where TForm: Form
    {
        Sure.NotNull (form);

        form.TransparencyKey = color;

        return form;
    }

    #endregion
}
