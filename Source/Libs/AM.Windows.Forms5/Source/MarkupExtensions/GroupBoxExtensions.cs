// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* GroupBoxExtensions.cs -- методы расширения для GroupBox
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="GroupBox"/>.
/// </summary>
public static class GroupBoxExtensions
{
    #region Public methods

    /// <summary>
    /// Установка плоского режима отображения.
    /// </summary>
    public static TGroupBox FlatStyle<TGroupBox>
        (
            this TGroupBox groupBox,
            FlatStyle flatStyle = System.Windows.Forms.FlatStyle.Flat
        )
        where TGroupBox: GroupBox
    {
        Sure.NotNull (groupBox);
        Sure.Defined (flatStyle);

        groupBox.FlatStyle = flatStyle;

        return groupBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="GroupBox.Click"/>.
    /// </summary>
    public static TGroupBox OnClick<TGroupBox>
        (
            this TGroupBox groupBox,
            EventHandler handler
        )
        where TGroupBox : GroupBox
    {
        Sure.NotNull (groupBox);
        Sure.NotNull (handler);

        groupBox.Click += handler;

        return groupBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="GroupBox.DoubleClick"/>.
    /// </summary>
    public static TGroupBox OnDoubleClick<TGroupBox>
        (
            this TGroupBox groupBox,
            EventHandler handler
        )
        where TGroupBox : GroupBox
    {
        Sure.NotNull (groupBox);
        Sure.NotNull (handler);

        groupBox.DoubleClick += handler;

        return groupBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="GroupBox.KeyDown"/>.
    /// </summary>
    public static TGroupBox OnKeyDown<TGroupBox>
        (
            this TGroupBox groupBox,
            KeyEventHandler handler
        )
        where TGroupBox : GroupBox
    {
        Sure.NotNull (groupBox);
        Sure.NotNull (handler);

        groupBox.KeyDown += handler;

        return groupBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="GroupBox.KeyPress"/>.
    /// </summary>
    public static TGroupBox OnKeyPress<TGroupBox>
        (
            this TGroupBox groupBox,
            KeyPressEventHandler handler
        )
        where TGroupBox : GroupBox
    {
        Sure.NotNull (groupBox);
        Sure.NotNull (handler);

        groupBox.KeyPress += handler;

        return groupBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="GroupBox.KeyUp"/>.
    /// </summary>
    public static TGroupBox OnKeyUp<TGroupBox>
        (
            this TGroupBox groupBox,
            KeyEventHandler handler
        )
        where TGroupBox : GroupBox
    {
        Sure.NotNull (groupBox);
        Sure.NotNull (handler);

        groupBox.KeyUp += handler;

        return groupBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="GroupBox.MouseEnter"/>.
    /// </summary>
    public static TGroupBox OnMouseEnter<TGroupBox>
        (
            this TGroupBox groupBox,
            EventHandler handler
        )
        where TGroupBox : GroupBox
    {
        Sure.NotNull (groupBox);
        Sure.NotNull (handler);

        groupBox.MouseEnter += handler;

        return groupBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="GroupBox.MouseLeave"/>.
    /// </summary>
    public static TGroupBox OnMouseLeave<TGroupBox>
        (
            this TGroupBox groupBox,
            EventHandler handler
        )
        where TGroupBox : GroupBox
    {
        Sure.NotNull (groupBox);
        Sure.NotNull (handler);

        groupBox.MouseLeave += handler;

        return groupBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="GroupBox.MouseMove"/>.
    /// </summary>
    public static TGroupBox OnMouseMove<TGroupBox>
        (
            this TGroupBox groupBox,
            MouseEventHandler handler
        )
        where TGroupBox : GroupBox
    {
        Sure.NotNull (groupBox);
        Sure.NotNull (handler);

        groupBox.MouseMove += handler;

        return groupBox;
    }

    /// <summary>
    /// Остановка при переходе между контролами по клавише <c>Tab</c>.
    /// </summary>
    public static TGroupBox TabStop<TGroupBox>
        (
            this TGroupBox groupBox,
            bool tabStop
        )
        where TGroupBox: GroupBox
    {
        Sure.NotNull (groupBox);

        groupBox.TabStop = tabStop;

        return groupBox;
    }

    #endregion
}
