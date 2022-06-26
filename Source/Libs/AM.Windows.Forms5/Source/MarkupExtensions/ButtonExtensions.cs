// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ButtonExtensions.cs -- методы расширения для Button
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="Button"/>.
/// </summary>
public static class ButtonExtensions
{
    #region Public methods

    /// <summary>
    /// Задание диалогового результата для кнопки.
    /// </summary>
    public static TButton DialogResult<TButton>
        (
            this TButton button,
            DialogResult dialogResult
        )
        where TButton: Button
    {
        Sure.NotNull (button);
        Sure.Defined (dialogResult);

        button.DialogResult = dialogResult;

        return button;
    }

    /// <summary>
    /// Задание диалогового результата для кнопки.
    /// </summary>
    public static TButton DialogResultCancel<TButton>
        (
            this TButton button
        )
        where TButton: Button
    {
        Sure.NotNull (button);

        button.DialogResult = System.Windows.Forms.DialogResult.Cancel;

        return button;
    }

    /// <summary>
    /// Задание диалогового результата для кнопки.
    /// </summary>
    public static TButton DialogResultNo<TButton>
        (
            this TButton button
        )
        where TButton: Button
    {
        Sure.NotNull (button);

        button.DialogResult = System.Windows.Forms.DialogResult.No;

        return button;
    }

    /// <summary>
    /// Задание диалогового результата для кнопки.
    /// </summary>
    public static TButton DialogResultOK<TButton>
        (
            this TButton button
        )
        where TButton: Button
    {
        Sure.NotNull (button);

        button.DialogResult = System.Windows.Forms.DialogResult.OK;

        return button;
    }

    /// <summary>
    /// Задание диалогового результата для кнопки.
    /// </summary>
    public static TButton DialogResultYes<TButton>
        (
            this TButton button
        )
        where TButton: Button
    {
        Sure.NotNull (button);

        button.DialogResult = System.Windows.Forms.DialogResult.Yes;

        return button;
    }

    /// <summary>
    /// Подписка на событие <see cref="Button.DoubleClick"/>.
    /// </summary>
    public static TButton OnDoubleClick<TButton>
        (
            this TButton button,
            EventHandler handler
        )
        where TButton: Button
    {
        Sure.NotNull (button);
        Sure.NotNull (handler);

        button.DoubleClick += handler;

        return button;
    }

    /// <summary>
    /// Подписка на событие <see cref="Button.OnMouseDoubleClick"/>.
    /// </summary>
    public static TButton OnMouseDoubleClick<TButton>
        (
            this TButton button,
            MouseEventHandler handler
        )
        where TButton: Button
    {
        Sure.NotNull (button);
        Sure.NotNull (handler);

        button.MouseDoubleClick += handler;

        return button;
    }

    #endregion
}
