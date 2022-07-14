// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ToolStripItemExtensions.cs -- методы расширения для ToolStripItem
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
/// Методы расширения для <see cref="ToolStripItem"/>.
/// </summary>
public static class ToolStripItemExtensions
{
    #region Public methods

    /// <summary>
    /// Выравнивание элемента.
    /// </summary>
    public static TToolStripItem Alignment<TToolStripItem>
        (
            this TToolStripItem item,
            ToolStripItemAlignment textAlign
        )
        where TToolStripItem: ToolStripItem
    {
        Sure.NotNull (item);
        Sure.Defined (textAlign);

        item.Alignment = textAlign;

        return item;
    }

    /// <summary>
    /// Выполнение неких дополнительных действий.
    /// </summary>
    public static TToolStripItem Also<TToolStripItem>
        (
            this TToolStripItem item,
            Action<TToolStripItem> action
        )
        where TToolStripItem: ToolStripItem
    {
        Sure.NotNull (item);
        Sure.NotNull (action);

        action.Invoke (item);

        return item;
    }

    /// <summary>
    /// Разрешение/запрещение элемента.
    /// </summary>
    public static TToolStripItem Enabled<TToolStripItem>
        (
            this TToolStripItem item,
            bool enabled
        )
        where TToolStripItem: ToolStripItem
    {
        Sure.NotNull (item);

        item.Enabled = enabled;

        return item;
    }

    /// <summary>
    /// Подписка на событие <see cref="ToolStripItem.Click"/>.
    /// </summary>
    public static TToolStripItem OnClick<TToolStripItem>
        (
            this TToolStripItem item,
            EventHandler handler
        )
        where TToolStripItem : ToolStripItem
    {
        Sure.NotNull (item);
        Sure.NotNull (handler);

        item.Click += handler;

        return item;
    }

    /// <summary>
    /// Задание текста для элемента.
    /// </summary>
    public static TToolStripItem Text<TToolStripItem>
        (
            this TToolStripItem item,
            string text
        )
        where TToolStripItem: ToolStripItem
    {
        Sure.NotNull (item);
        Sure.NotNullNorEmpty (text);

        item.Text = text;

        return item;
    }

    /// <summary>
    /// Задание выравнивания текста.
    /// </summary>
    public static TToolStripItem TextAlign<TToolStripItem>
        (
            this TToolStripItem item,
            ContentAlignment textAlign
        )
        where TToolStripItem: ToolStripItem
    {
        Sure.NotNull (item);

        item.TextAlign = textAlign;

        return item;
    }

    #endregion
}
