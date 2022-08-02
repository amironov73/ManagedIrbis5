// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* EventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace Manina.Windows.Forms;

/// <summary>
/// Contains event data for events related to a single page.
/// </summary>
public class PageEventArgs
    : EventArgs
{
    /// <summary>
    /// The page causing the event.
    /// </summary>
    public Page Page { get; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PageEventArgs
        (
            Page page
        )
    {
        Page = page;
    }
}

/// <summary>
/// Contains event data for the <see cref="PagedControl.PageChanging"/> event.
/// </summary>
public sealed class PageChangingEventArgs
    : CancelEventArgs
{
    /// <summary>
    /// Current page.
    /// </summary>
    public Page? CurrentPage { get; }

    /// <summary>
    /// The page that will become the current page after the event.
    /// </summary>
    public Page? NewPage { get; set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PageChangingEventArgs
        (
            Page? currentPage,
            Page? newPage
        )
        : base (false)
    {
        CurrentPage = currentPage;
        NewPage = newPage;
    }
}

/// <summary>
/// Contains event data for the <see cref="PagedControl.PageChanged"/> event.
/// </summary>
public sealed class PageChangedEventArgs
    : EventArgs
{
    /// <summary>
    /// The page that was the current page before the event.
    /// </summary>
    public Page? OldPage { get; }

    /// <summary>
    /// Current page.
    /// </summary>
    public Page? CurrentPage { get; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PageChangedEventArgs
        (
            Page? oldPage,
            Page? currentPage
        )
    {
        OldPage = oldPage;
        CurrentPage = currentPage;
    }
}

/// <summary>
/// Contains event data for the <see cref="PagedControl.PageValidating"/> event.
/// </summary>
public sealed class PageValidatingEventArgs
    : CancelEventArgs
{
    /// <summary>
    /// The page causing the event.
    /// </summary>
    public Page Page { get; private set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PageValidatingEventArgs
        (
            Page page
        )
    {
        Page = page;
    }
}

/// <summary>
/// Contains event data for the <see cref="PagedControl.PagePaint"/> event.
/// </summary>
public sealed class PagePaintEventArgs
    : PageEventArgs
{
    /// <summary>
    /// Gets the graphics used to paint.
    /// </summary>
    public Graphics Graphics { get; private set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PagePaintEventArgs
        (
            Graphics graphics,
            Page page
        )
        : base (page)
    {
        Graphics = graphics;
    }
}

/// <summary>
/// Contains event data for the <see cref="PagedControl.CreateUIControls"/> event.
/// </summary>
public sealed class CreateUIControlsEventArgs
    : EventArgs
{
    /// <summary>
    /// Gets the collection of UI controls.
    /// </summary>
    public Control[] Controls { get; set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CreateUIControlsEventArgs
        (
            Control[] controls
        )
    {
        Controls = controls;
    }

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public CreateUIControlsEventArgs()
        : this (Array.Empty<Control>())
    {
        // пустое тело конструктора
    }
}
