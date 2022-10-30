// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* Display.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AeroSuite.Controls;

/// <summary>
/// A host control for a <see cref="ViewBase"/>
/// </summary>
/// <remarks>
/// The perfect alternative to a <see cref="HeaderlessTabControl"/>: It works on every imaginable platform without any bugs but also unfolds the possibility to create advanced single-window applications with a lot of OOP-compliance as you can easily create a class per View.
/// </remarks>
[DesignerCategory ("Code")]
[DisplayName ("Display")]
[Description ("A host control for Views.")]
[ToolboxItem (true)]
[ToolboxBitmap (typeof (Panel))]
public class Display
    : ContainerControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Display"/> class.
    /// </summary>
    public Display()
    {
        Dock = DockStyle.Fill;
        AutoScroll = true;
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
    /// </summary>
    /// <param name="eventArgs">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
    protected override void OnPaint
        (
            PaintEventArgs eventArgs
        )
    {
        if (DesignMode)
        {
            eventArgs.Graphics.DrawString (Name + "\n Specify a View in the View property to display it.", Font,
                SystemBrushes.ControlDarkDark, new Rectangle (0, 0, Width, Height),
                new StringFormat()
                {
                    Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                });
        }

        base.OnPaint (eventArgs);
    }

    /// <summary>
    /// Occurs when the displayed View changed.
    /// </summary>
    public event EventHandler? ViewChanged;

    private ViewBase? _view;

    /// <summary>
    /// Gets or sets the view displayed.
    /// </summary>
    /// <value>
    /// The view.
    /// </value>
    public virtual ViewBase? View
    {
        get => _view;
        set
        {
            //if (value == this.view)
            //{
            //    return;
            //}

            ClearViewInternal();

            if (_view != null)
            {
                _view.ClearDisplayInternal();
            }

            SetViewInternal (value, true);
        }
    }

    /// <summary>
    /// Clears the view internally.
    /// </summary>
    internal virtual void ClearViewInternal()
    {
        if (_view != null && Controls.Contains (_view))
        {
            Controls.Remove (_view);
        }

        _view = null;
    }

    /// <summary>
    /// Sets the view internally.
    /// </summary>
    /// <param name="view">The new view for this display.</param>
    /// <param name="notifyView">if set to <c>true</c> the view will be notified so that it can adapt to the change.</param>
    internal virtual void SetViewInternal
        (
            ViewBase? view,
            bool notifyView
        )
    {
        _view = view;

        if (_view != null)
        {
            if (notifyView)
            {
                _view.SetDisplayInternal (this, false);

                if (ViewChanged != null)
                {
                    ViewChanged (this, EventArgs.Empty);
                }
            }

            Controls.Add (_view);
        }
    }
}
