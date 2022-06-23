// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ControlExtensions.cs -- методы расширения для Control
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
/// Методы расширения для <see cref="Control"/>.
/// </summary>
public static class ControlExtensions
{
    #region Public methods

    /// <summary>
    /// Включение поддержки перетаскивания файлов.
    /// </summary>
    public static TControl AllowDrop<TControl>
        (
            this TControl control,
            bool allow = true
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.AllowDrop = allow;

        return control;
    }

    /// <summary>
    /// Выполнение произвольных побочных действий.
    /// </summary>
    public static TControl Also<TControl>
        (
            this TControl control,
            Action<TControl> action
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNull (action);

        action (control);

        return control;
    }

    /// <summary>
    /// Задание "якорей" для контрола.
    /// </summary>
    public static TControl Anchor<TControl>
        (
            this TControl control,
            AnchorStyles anchors
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Anchor = anchors;

        return control;
    }

    /// <summary>
    /// Присваивание указанной переменной.
    /// </summary>
    public static TControl Assign<TControl>
        (
            this TControl control,
            out TControl variable
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        variable = control;

        return control;
    }

    /// <summary>
    /// Включение автоматического изменения размеров.
    /// </summary>
    public static TControl AutoSize<TControl>
        (
            this TControl control,
            bool autoSize = true
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.AutoSize = autoSize;

        return control;
    }

    /// <summary>
    /// Задание цвета фона для контрола.
    /// </summary>
    public static TControl BackColor<TControl>
        (
            this TControl control,
            Color color
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.BackColor = color;

        return control;
    }

    /// <summary>
    /// Задание фонового изображения.
    /// </summary>
    public static TControl BackImage<TControl>
        (
            this TControl control,
            Image image
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNull (image);

        control.BackgroundImage = image;

        return control;
    }

    /// <summary>
    /// Задание отступов для контрола.
    /// </summary>
    public static TControl Bounds<TControl>
        (
            this TControl control,
            params int[] bounds
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNull (bounds);

        control.Bounds = bounds.Length switch
        {
            1 => new Rectangle (bounds[0], bounds[0], bounds[0], bounds[0]),
            2 => new Rectangle (bounds[0], bounds[1], bounds[0], bounds[1]),
            4 => new Rectangle (bounds[0], bounds[1], bounds[2], bounds[3]),
            _ => throw new ArgumentException (nameof (bounds))
        };

        return control;
    }

    /// <summary>
    /// Включение захвата мыши.
    /// </summary>
    public static TControl Capture<TControl>
        (
            this TControl control,
            bool capture = true
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Capture = capture;

        return control;
    }

    /// <summary>
    /// Добавление дочерних контролов.
    /// </summary>
    public static TControl Controls<TControl>
        (
            this TControl control,
            params Control[] children
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNull (children);

        control.SuspendLayout();

        control.Controls.AddRange (children);
        foreach (var child in children)
        {
            if (child.Dock == DockStyle.Fill)
            {
                child.BringToFront();
            }
        }

        control.ResumeLayout();

        return control;
    }

    /// <summary>
    /// Задание мышиного курсора для контрола.
    /// </summary>
    public static TControl Cursor<TControl>
        (
            this TControl control,
            Cursor cursor
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNull (cursor);

        control.Cursor = cursor;

        return control;
    }

    /// <summary>
    /// Задание докинга для контрола.
    /// </summary>
    public static TControl Dock<TControl>
        (
            this TControl control,
            DockStyle dock = DockStyle.Fill
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Dock = dock;

        return control;
    }

    /// <summary>
    /// Задание докинга для контрола.
    /// </summary>
    public static TControl DockLeft<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Dock = DockStyle.Left;

        return control;
    }

    /// <summary>
    /// Задание докинга для контрола.
    /// </summary>
    public static TControl DockTop<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Dock = DockStyle.Top;

        return control;
    }

    /// <summary>
    /// Разрешение/запрещение контрола.
    /// </summary>
    public static TControl Enabled<TControl>
        (
            this TControl control,
            bool enabled = true
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Enabled = enabled;

        return control;
    }

    /// <summary>
    /// Задание цвета текста для контрола.
    /// </summary>
    public static TControl ForeColor<TControl>
        (
            this TControl control,
            Color color
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.ForeColor = color;

        return control;
    }

    /// <summary>
    /// Задание границ для контрола.
    /// </summary>
    public static TControl Margin<TControl>
        (
            this TControl control,
            params int[] margins
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNull (margins);

        control.Margin = margins.Length switch
        {
            1 => new Padding (margins[0]),
            2 => new Padding (margins[0], margins[1], margins[0], margins[1]),
            4 => new Padding (margins[0], margins[1], margins[2], margins[3]),
            _ => throw new ArgumentException (nameof (margins))
        };

        return control;
    }

    /// <summary>
    /// Установка максимального размера для контрола.
    /// </summary>
    public static TControl MaximumSize<TControl>
        (
            this TControl control,
            int width,
            int height
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.Positive (width);
        Sure.Positive (height);

        control.MaximumSize = new Size (width, height);

        return control;
    }

    /// <summary>
    /// Установка минимального размера для контрола.
    /// </summary>
    public static TControl MinimumSize<TControl>
        (
            this TControl control,
            int width,
            int height
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.Positive (width);
        Sure.Positive (height);

        control.MinimumSize = new Size (width, height);

        return control;
    }

    /// <summary>
    /// Присвоение контролу имени.
    /// </summary>
    public static TControl Name<TControl>
        (
            this TControl control,
            string name
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNullNorEmpty (name);

        control.Name = name;

        return control;
    }

    /// <summary>
    /// Задание порядка обхода контролов по клавише <c>Tab</c>.
    /// </summary>
    public static TControl TabIndex<TControl>
        (
            this TControl control,
            int tabIndex
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.Positive (tabIndex);

        control.TabIndex = tabIndex;

        return control;
    }

    /// <summary>
    /// Разрешение пропуска контрола при обходе по клавише <c>Tab</c>.
    /// </summary>
    public static TControl TabStop<TControl>
        (
            this TControl control,
            bool stop = false
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.TabStop = stop;

        return control;
    }

    /// <summary>
    /// Связывание контрола с произвольными
    /// пользовательскими данными.
    /// </summary>
    public static TControl Tag<TControl>
        (
            this TControl control,
            object? tag
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Tag = tag;

        return control;
    }

    /// <summary>
    /// Задание текста, выводимого контролом (например, заголовка окна).
    /// </summary>
    public static TControl Text<TControl>
        (
            this TControl control,
            string text
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Text = text;

        return control;
    }

    #endregion
}
