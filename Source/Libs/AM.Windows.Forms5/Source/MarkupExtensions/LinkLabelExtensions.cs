// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LinkLabelExtensions.cs -- методы расширения для LinkLabel
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="LinkLabel"/>.
/// </summary>
public static class LinkLabelExtensions
{
    #region Public methods

    /// <summary>
    /// Указание цвета активной ссылки.
    /// </summary>
    public static TLinkLabel ActiveLinkColor<TLinkLabel>
        (
            this TLinkLabel label,
            Color color
        )
        where TLinkLabel: LinkLabel
    {
        Sure.NotNull (label);

        label.ActiveLinkColor = color;

        return label;
    }

    /// <summary>
    /// Указание цвета запрещенной ссылки.
    /// </summary>
    public static TLinkLabel DisabledLinkColor<TLinkLabel>
        (
            this TLinkLabel label,
            Color color
        )
        where TLinkLabel: LinkLabel
    {
        Sure.NotNull (label);

        label.DisabledLinkColor = color;

        return label;
    }

    /// <summary>
    /// Задание плоского режима.
    /// </summary>
    public static TLinkLabel FlatStyle<TLinkLabel>
        (
            this TLinkLabel label,
            FlatStyle flatStyle
        )
        where TLinkLabel: LinkLabel
    {
        Sure.NotNull (label);
        Sure.Defined (flatStyle);

        label.FlatStyle = flatStyle;

        return label;
    }

    /// <summary>
    /// Задание области, воспринимаемой как ссылка.
    /// </summary>
    public static TLinkLabel LinkArea<TLinkLabel>
        (
            this TLinkLabel label,
            LinkArea linkArea
        )
        where TLinkLabel: LinkLabel
    {
        Sure.NotNull (label);
        Sure.Defined (linkArea);

        label.LinkArea = linkArea;

        return label;
    }

    /// <summary>
    /// Поведение ссылки.
    /// </summary>
    public static TLinkLabel LinkBehavior<TLinkLabel>
        (
            this TLinkLabel label,
            LinkBehavior behavior
        )
        where TLinkLabel: LinkLabel
    {
        Sure.NotNull (label);
        Sure.Defined (behavior);

        label.LinkBehavior = behavior;

        return label;
    }

    /// <summary>
    /// Указание цвета ссылки в обычном состоянии.
    /// </summary>
    public static TLinkLabel LinkColor<TLinkLabel>
        (
            this TLinkLabel label,
            Color color
        )
        where TLinkLabel: LinkLabel
    {
        Sure.NotNull (label);

        label.LinkColor = color;

        return label;
    }

    /// <summary>
    /// Добавление ссылок.
    /// </summary>
    public static TLinkLabel Links<TLinkLabel>
        (
            this TLinkLabel label,
            params LinkLabel.Link[] links
        )
        where TLinkLabel: LinkLabel
    {
        Sure.NotNull (label);
        Sure.NotNull (links);

        foreach (var link in links)
        {
            label.Links.Add (link);
        }

        return label;
    }

    /// <summary>
    /// Задание состояния "ссылка посещена".
    /// </summary>
    public static TLinkLabel LinkVisited<TLinkLabel>
        (
            this TLinkLabel label,
            bool visited = true
        )
        where TLinkLabel: LinkLabel
    {
        Sure.NotNull (label);

        label.LinkVisited = visited;

        return label;
    }

    /// <summary>
    /// Подписка на событие <see cref="LinkLabel.LinkClicked"/>.
    /// </summary>
    public static TLinkLabel OnLinkClicked<TLinkLabel>
        (
            this TLinkLabel label,
            LinkLabelLinkClickedEventHandler handler
        )
        where TLinkLabel : LinkLabel
    {
        Sure.NotNull (label);
        Sure.NotNull (handler);

        label.LinkClicked += handler;

        return label;
    }

    /// <summary>
    /// Задание цвета посещенной ссылки.
    /// </summary>
    public static TLinkLabel VisitedLinkColor<TLinkLabel>
        (
            this TLinkLabel label,
            Color color
        )
        where TLinkLabel: LinkLabel
    {
        Sure.NotNull (label);

        label.VisitedLinkColor = color;

        return label;
    }

    #endregion
}
