// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CheckBoxExtensions.cs -- методы расширения для CheckBox
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
/// Методы расширения для <see cref="CheckBox"/>.
/// </summary>
public static class CheckBoxExtensions
{
    #region Public methods

    /// <summary>
    /// Как отображается: как кнопка или как чекбокс (по умолчанию)?
    /// </summary>
    public static TCheckBox Appearance<TCheckBox>
        (
            this TCheckBox checkBox,
            Appearance appearance
        )
        where TCheckBox: CheckBox
    {
        Sure.NotNull (checkBox);
        Sure.Defined (appearance);

        checkBox.Appearance = appearance;

        return checkBox;
    }

    /// <summary>
    /// Состояние автоматически меняется при клике на чекбоксе.
    /// </summary>
    public static TCheckBox AutoCheck<TCheckBox>
        (
            this TCheckBox checkBox,
            bool autoCheck
        )
        where TCheckBox: CheckBox
    {
        Sure.NotNull (checkBox);

        checkBox.AutoCheck = autoCheck;

        return checkBox;
    }

    /// <summary>
    /// Расположение области для чеканья бокса.
    /// </summary>
    public static TCheckBox CheckAlign<TCheckBox>
        (
            this TCheckBox checkBox,
            ContentAlignment alignment
        )
        where TCheckBox: CheckBox
    {
        Sure.NotNull (checkBox);
        Sure.Defined (alignment);

        checkBox.CheckAlign = alignment;

        return checkBox;
    }

    /// <summary>
    /// Отмечено?
    /// </summary>
    public static TCheckBox Checked<TCheckBox>
        (
            this TCheckBox checkBox,
            bool isChecked = true
        )
        where TCheckBox: CheckBox
    {
        Sure.NotNull (checkBox);

        checkBox.Checked = isChecked;

        return checkBox;
    }

    /// <summary>
    /// Состояние чекбокса.
    /// </summary>
    public static TCheckBox CheckState<TCheckBox>
        (
            this TCheckBox checkBox,
            CheckState checkState
        )
        where TCheckBox: CheckBox
    {
        Sure.NotNull (checkBox);
        Sure.Defined (checkState);

        checkBox.CheckState = checkState;

        return checkBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="CheckBox.CheckedChanged"/>.
    /// </summary>
    public static TCheckBox OnCheckedChanged<TCheckBox>
        (
            TCheckBox checkBox,
            EventHandler handler
        )
        where TCheckBox: CheckBox
    {
        Sure.NotNull (checkBox);
        Sure.NotNull (handler);

        checkBox.CheckedChanged += handler;

        return checkBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="CheckBox.CheckStateChanged"/>.
    /// </summary>
    public static TCheckBox OnCheckStateChanged<TCheckBox>
        (
            TCheckBox checkBox,
            EventHandler handler
        )
        where TCheckBox: CheckBox
    {
        Sure.NotNull (checkBox);
        Sure.NotNull (handler);

        checkBox.CheckStateChanged += handler;

        return checkBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="CheckBox.DoubleClick"/>.
    /// </summary>
    public static TCheckBox OnDoubleClick<TCheckBox>
        (
            TCheckBox checkBox,
            EventHandler handler
        )
        where TCheckBox: CheckBox
    {
        Sure.NotNull (checkBox);
        Sure.NotNull (handler);

        checkBox.DoubleClick += handler;

        return checkBox;
    }

    /// <summary>
    /// Подписка на событие <see cref="CheckBox.OnMouseDoubleClick"/>.
    /// </summary>
    public static TCheckBox OnMouseDoubleClick<TCheckBox>
        (
            TCheckBox checkBox,
            MouseEventHandler handler
        )
        where TCheckBox: CheckBox
    {
        Sure.NotNull (checkBox);
        Sure.NotNull (handler);

        checkBox.MouseDoubleClick += handler;

        return checkBox;
    }

    /// <summary>
    /// Включение режима "три состояния".
    /// </summary>
    public static TCheckBox ThreeState<TCheckBox>
        (
            this TCheckBox checkBox,
            bool enabled = true
        )
        where TCheckBox: CheckBox
    {
        Sure.NotNull (checkBox);

        checkBox.ThreeState = enabled;

        return checkBox;
    }

    #endregion
}
