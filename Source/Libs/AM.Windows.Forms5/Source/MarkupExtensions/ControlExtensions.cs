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
    /// Описание для экранного диктора.
    /// </summary>
    public static TControl AccessibleDefaultActionDescription<TControl>
        (
            this TControl control,
            string description
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNullNorEmpty (description);

        control.AccessibleDefaultActionDescription = description;

        return control;
    }

    /// <summary>
    /// Описание для экранного диктора.
    /// </summary>
    public static TControl AccessibleDescription<TControl>
        (
            this TControl control,
            string description
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNullNorEmpty (description);

        control.AccessibleDescription = description;

        return control;
    }

    /// <summary>
    /// Имя для экранного диктора.
    /// </summary>
    public static TControl AccessibleName<TControl>
        (
            this TControl control,
            string name
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNullNorEmpty (name);

        control.AccessibleName = name;

        return control;
    }

    /// <summary>
    /// Роль для экранного диктора.
    /// </summary>
    public static TControl AccessibleRole<TControl>
        (
            this TControl control,
            AccessibleRole role
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.Defined (role);

        control.AccessibleRole = role;

        return control;
    }

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
    /// Смещение при автоматическом скроллинге.
    /// </summary>
    public static TControl AutoScrollOffset<TControl>
        (
            this TControl control,
            Point offset
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.AutoScrollOffset = offset;

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
    /// Задание фонового изображения.
    /// </summary>
    public static TControl BackImageLayout<TControl>
        (
            this TControl control,
            ImageLayout layout
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.Defined (layout);

        control.BackgroundImageLayout = layout;

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
    /// Контрол вызывает валидацию формы.
    /// </summary>
    public static TControl CausesValidation<TControl>
        (
            this TControl control,
            bool causesValidation = true
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.CausesValidation = causesValidation;

        return control;
    }

    /// <summary>
    /// Задание клиентских размеров.
    /// </summary>
    public static TControl ClientSize<TControl>
        (
            this TControl control,
            Size clientSize
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.ClientSize = clientSize;

        return control;
    }

    /// <summary>
    /// Задание клиентских размеров.
    /// </summary>
    public static TControl ClientSize<TControl>
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

        control.ClientSize = new Size (width, height);

        return control;
    }

    /// <summary>
    /// Задание контекстного меню.
    /// </summary>
    public static TControl ClientSize<TControl>
        (
            this TControl control,
            params ToolStripItem[] items
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNull (items);

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.AddRange (items);
        control.ContextMenuStrip = contextMenu;

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
    public static TControl DockFill<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Dock = DockStyle.Fill;

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
    /// Задание шрифта для контрола.
    /// </summary>
    public static TControl Font<TControl>
        (
            this TControl control,
            Font font
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNull (font);

        control.Font = font;

        return control;
    }

    /// <summary>
    /// Задание шрифта для контрола.
    /// </summary>
    public static TControl Font<TControl>
        (
            this TControl control,
            string font,
            int size
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNullNorEmpty (font);
        Sure.Positive (size);

        control.Font = new Font (font, size);

        return control;
    }

    /// <summary>
    /// Задание шрифта для контрола.
    /// </summary>
    public static TControl Font<TControl>
        (
            this TControl control,
            string font,
            FontStyle style,
            int size
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNullNorEmpty (font);
        Sure.Defined (style);
        Sure.Positive (size);

        control.Font = new Font (font, size);

        return control;
    }

    /// <summary>
    /// Задание цвета текста для контрола.
    /// </summary>
    public static TControl FontSize<TControl>
        (
            this TControl control,
            int size
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.Positive (size);

        control.Font = new Font (control.Font.FontFamily, size);

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
    /// Задание высоты контрола.
    /// </summary>
    public static TControl Height<TControl>
        (
            this TControl control,
            int height
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.Positive (height);

        return control;
    }

    /// <summary>
    /// Контрол умеет работать с инвалидами.
    /// </summary>
    public static TControl IsAccessible<TControl>
        (
            this TControl control,
            bool accessible = true
        )
        where TControl : Control
    {
        Sure.NotNull (control);

        control.IsAccessible = accessible;

        return control;
    }

    /// <summary>
    /// Указание отступа слева.
    /// </summary>
    public static TControl Left<TControl>
        (
            this TControl control,
            int left
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.Positive (left);

        control.Left = left;

        return control;
    }

    /// <summary>
    /// Указание позиции контрола.
    /// </summary>
    public static TControl Location<TControl>
        (
            this TControl control,
            Point location
        )
        where TControl : Control
    {
        Sure.NotNull (control);

        control.Location = location;

        return control;
    }

    /// <summary>
    /// Указание позиции контрола.
    /// </summary>
    public static TControl Location<TControl>
        (
            this TControl control,
            int left,
            int top
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.Positive (left);
        Sure.Positive (top);

        control.Location = new Point (left, top);

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
            Size size
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.MaximumSize = size;

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
            Size size
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.MinimumSize = size;

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
    /// Подписка на событие <see cref="Control.Click"/>.
    /// </summary>
    public static TControl OnClick<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.Click += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.DoubleClick"/>.
    /// </summary>
    public static TControl OnDoubleClick<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.DoubleClick += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.DragDrop"/>.
    /// </summary>
    public static TControl OnDragDrop<TControl>
        (
            this TControl control,
            DragEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.DragDrop += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.DragEnter"/>.
    /// </summary>
    public static TControl OnDragEnter<TControl>
        (
            this TControl control,
            DragEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.DragEnter += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.DragLeave"/>.
    /// </summary>
    public static TControl OnDragLeave<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.DragLeave += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.DragOver"/>.
    /// </summary>
    public static TControl OnDragOver<TControl>
        (
            this TControl control,
            DragEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.DragOver += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.EnabledChanged"/>.
    /// </summary>
    public static TControl OnEnabledChanged<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.EnabledChanged += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.Enter"/>.
    /// </summary>
    public static TControl OnEnter<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.Enter += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.GotFocus"/>.
    /// </summary>
    public static TControl OnGotFocus<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.GotFocus += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.HelpRequested"/>.
    /// </summary>
    public static TControl OnHelpRequested<TControl>
        (
            this TControl control,
            HelpEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.HelpRequested += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.KeyDown"/>.
    /// </summary>
    public static TControl OnKeyDown<TControl>
        (
            this TControl control,
            KeyEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.KeyDown += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.KeyDown"/>.
    /// </summary>
    public static TControl OnKeyPress<TControl>
        (
            this TControl control,
            KeyPressEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.KeyPress += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.KeyUp"/>.
    /// </summary>
    public static TControl OnKeyUp<TControl>
        (
            this TControl control,
            KeyEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.KeyUp += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.Layout"/>.
    /// </summary>
    public static TControl OnLayout<TControl>
        (
            this TControl control,
            LayoutEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.Layout += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.Leave"/>.
    /// </summary>
    public static TControl OnLeave<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.Leave += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.LostFocus"/>.
    /// </summary>
    public static TControl OnLostFocus<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.LostFocus += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.MouseClick"/>.
    /// </summary>
    public static TControl OnMouseClick<TControl>
        (
            this TControl control,
            MouseEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.MouseClick += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.MouseDoubleClick"/>.
    /// </summary>
    public static TControl OnMouseDoubleClick<TControl>
        (
            this TControl control,
            MouseEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.MouseDoubleClick += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.MouseDown"/>.
    /// </summary>
    public static TControl OnMouseDown<TControl>
        (
            this TControl control,
            MouseEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.MouseDown += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.MouseEnter"/>.
    /// </summary>
    public static TControl OnMouseEnter<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.MouseEnter += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.MouseHover"/>.
    /// </summary>
    public static TControl OnMouseHover<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.MouseHover += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.MouseLeave"/>.
    /// </summary>
    public static TControl OnMouseLeave<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.MouseLeave += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.MouseMove"/>.
    /// </summary>
    public static TControl OnMouseMove<TControl>
        (
            this TControl control,
            MouseEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.MouseMove += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.MouseUp"/>.
    /// </summary>
    public static TControl OnMouseUp<TControl>
        (
            this TControl control,
            MouseEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.MouseUp += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.MouseWheel"/>.
    /// </summary>
    public static TControl OnMouseWheel<TControl>
        (
            this TControl control,
            MouseEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.MouseWheel += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.Move"/>.
    /// </summary>
    public static TControl OnMove<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.Move += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.Paint"/>.
    /// </summary>
    public static TControl OnPaint<TControl>
        (
            this TControl control,
            PaintEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.Paint += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.PreviewKeyDown"/>.
    /// </summary>
    public static TControl OnPreviewKeyDown<TControl>
        (
            this TControl control,
            PreviewKeyDownEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.PreviewKeyDown += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.QueryContinueDrag"/>.
    /// </summary>
    public static TControl OnQueryContinueDrag<TControl>
        (
            this TControl control,
            QueryContinueDragEventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.QueryContinueDrag += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.Resize"/>.
    /// </summary>
    public static TControl OnQueryContinueDrag<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.Resize += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.SizeChanged"/>.
    /// </summary>
    public static TControl OnSizeChanged<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.SizeChanged += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.TextChanged"/>.
    /// </summary>
    public static TControl OnTextChanged<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.TextChanged += handler;

        return control;
    }

    /// <summary>
    /// Подписка на событие <see cref="Control.VisibleChanged"/>.
    /// </summary>
    public static TControl OnVisibleChanged<TControl>
        (
            this TControl control,
            EventHandler handler
        )
        where TControl : Control
    {
        Sure.NotNull (control);
        Sure.NotNull (handler);

        control.VisibleChanged += handler;

        return control;
    }

    /// <summary>
    /// Задание паддинга.
    /// </summary>
    public static TControl Padding<TControl>
        (
            this TControl control,
            params int[] padding
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNull (padding);

        control.Padding = padding.Length switch
        {
            1 => new Padding (padding[0]),
            2 => new Padding (padding[0], padding[1], padding[0], padding[1]),
            4 => new Padding (padding[0], padding[1], padding[2], padding[3]),
            _ => throw new ArgumentException (nameof (padding))
        };

        return control;
    }

    /// <summary>
    /// Вывод текста справа налево.
    /// </summary>
    public static TControl RightToLeft<TControl>
        (
            this TControl control,
            RightToLeft rtl = System.Windows.Forms.RightToLeft.Yes
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.RightToLeft = rtl;

        return control;
    }

    /// <summary>
    /// Размер контрола.
    /// </summary>
    public static TControl Size<TControl>
        (
            this TControl control,
            Size size
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Size = size;

        return control;
    }

    /// <summary>
    /// Размер контрола.
    /// </summary>
    public static TControl Size<TControl>
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

        control.Size = new Size (width, height);

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

    /// <summary>
    /// Контрол на задний план.
    /// </summary>
    public static TControl ToBack<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        void ToBackInt (object? o, EventArgs eventArgs)
        {
            control.SendToBack();
            control.ParentChanged -= ToBackInt;
        }

        Sure.NotNull (control);

        if (control.Parent is null)
        {
            control.ParentChanged += ToBackInt;
        }
        else
        {
            control.BeginInvoke ((EventHandler) ToBackInt);
        }

        return control;
    }

    /// <summary>
    /// Контрол на передний план.
    /// </summary>
    public static TControl ToFront<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        void ToFrontInt (object? o, EventArgs eventArgs)
        {
            control.BringToFront();
            control.ParentChanged -= ToFrontInt;
        }

        Sure.NotNull (control);

        if (control.Parent is null)
        {
            control.ParentChanged += ToFrontInt;
        }
        else
        {
            control.BeginInvoke ((EventHandler) ToFrontInt);
        }

        return control;
    }

    /// <summary>
    /// Отступ сверху.
    /// </summary>
    public static TControl Top<TControl>
        (
            this TControl control,
            int top
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.Positive (top);

        control.Top = top;

        return control;
    }

    /// <summary>
    /// Видимость контрола.
    /// </summary>
    public static TControl Visible<TControl>
        (
            this TControl control,
            bool visible
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Visible = visible;

        return control;
    }

    /// <summary>
    /// Ширина контрола.
    /// </summary>
    public static TControl Width<TControl>
        (
            this TControl control,
            int width
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.Positive (width);

        control.Width = width;

        return control;
    }

    #endregion
}
