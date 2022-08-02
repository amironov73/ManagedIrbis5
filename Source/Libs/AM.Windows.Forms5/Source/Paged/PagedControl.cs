// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* PagedControl.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#pragma warning disable CS0067

#nullable enable

namespace Manina.Windows.Forms;

/// <summary>
///
/// </summary>
[ToolboxBitmap (typeof (PagedControl))]
[Designer (typeof (PagedControlDesigner))]
[Docking (DockingBehavior.Ask)]
[DefaultEvent ("PageChanged")]
[DefaultProperty ("SelectedPage")]
public partial class PagedControl
    : Control
{
    #region Virtual Functions for Events

    /// <summary>
    /// Raises the <see cref="PageAdded"/> event.
    /// </summary>
    /// <param name="eventArgs">A <see cref="PageEventArgs"/> that contains event data.</param>
    protected internal virtual void OnPageAdded
        (
            PageEventArgs eventArgs
        )
    {
        PageAdded?.Invoke (this, eventArgs);
    }

    /// <summary>
    /// Raises the <see cref="PageRemoved"/> event.
    /// </summary>
    /// <param name="eventArgs">A <see cref="PageEventArgs"/> that contains event data.</param>
    protected internal virtual void OnPageRemoved
        (
            PageEventArgs eventArgs
        )
    {
        PageRemoved?.Invoke (this, eventArgs);
    }

    /// <summary>
    /// Raises the <see cref="PageChanging"/> event.
    /// </summary>
    /// <param name="eventArgs">A <see cref="PageChangingEventArgs"/> that contains event data.</param>
    protected internal virtual void OnPageChanging
        (
            PageChangingEventArgs eventArgs
        )
    {
        PageChanging?.Invoke (this, eventArgs);
    }

    /// <summary>
    /// Raises the <see cref="PageChanged"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PageChangedEventArgs"/> that contains event data.</param>
    protected internal virtual void OnPageChanged (PageChangedEventArgs e)
    {
        PageChanged?.Invoke (this, e);
    }

    /// <summary>
    /// Raises the <see cref="PageValidating"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PageValidatingEventArgs"/> that contains event data.</param>
    protected internal virtual void OnPageValidating (PageValidatingEventArgs e)
    {
        PageValidating?.Invoke (this, e);
    }

    /// <summary>
    /// Raises the <see cref="PageValidated"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PageEventArgs"/> that contains event data.</param>
    protected internal virtual void OnPageValidated (PageEventArgs e)
    {
        PageValidated?.Invoke (this, e);
    }

    /// <summary>
    /// Raises the <see cref="PageHidden"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PageEventArgs"/> that contains event data.</param>
    protected internal virtual void OnPageHidden (PageEventArgs e)
    {
        PageHidden?.Invoke (this, e);
    }

    /// <summary>
    /// Raises the <see cref="PageShown"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PageEventArgs"/> that contains event data.</param>
    protected internal virtual void OnPageShown (PageEventArgs e)
    {
        PageShown?.Invoke (this, e);
    }

    /// <summary>
    /// Raises the <see cref="PagePaint"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PagePaintEventArgs"/> that contains event data.</param>
    protected internal virtual void OnPagePaint (PagePaintEventArgs e)
    {
        PagePaint?.Invoke (this, e);
    }

    /// <summary>
    /// Raises the <see cref="CreateUIControls"/> event.
    /// </summary>
    /// <param name="e">A <see cref="CreateUIControlsEventArgs"/> that contains event data.</param>
    protected internal virtual void OnCreateUIControls (CreateUIControlsEventArgs e)
    {
        CreateUIControls?.Invoke (this, e);

        creatingUIControls = true;
        var i = 0;
        foreach (var control in e.Controls)
        {
            Controls.Add (control);
            Controls.SetChildIndex (control, i);
            i++;
        }

        creatingUIControls = false;

        uiControlCount = e.Controls.Length;
    }

    /// <summary>
    /// Raises the <see cref="UpdateUIControls"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains event data.</param>
    protected internal virtual void OnUpdateUIControls (EventArgs e)
    {
        UpdateUIControls?.Invoke (this, e);
    }

    #endregion

    #region Events

    /// <summary>
    /// Occurs when a new page is added.
    /// </summary>
    [Category ("Behavior"), Description ("Occurs when a new page is added.")]
    public event EventHandler<PageEventArgs>? PageAdded;

    /// <summary>
    /// Occurs when a page is removed.
    /// </summary>
    [Category ("Behavior"), Description ("Occurs when a page is removed.")]
    public event EventHandler<PageEventArgs>? PageRemoved;

    /// <summary>
    /// Occurs before the current page is changed.
    /// </summary>
    [Category ("Behavior"), Description ("Occurs before the current page is changed.")]
    public event EventHandler<PageChangingEventArgs>? PageChanging;

    /// <summary>
    /// Occurs after the current page is changed.
    /// </summary>
    [Category ("Behavior"), Description ("Occurs after the current page is changed.")]
    public event EventHandler<PageChangedEventArgs>? PageChanged;

    /// <summary>
    /// Occurs when the current page is validating.
    /// </summary>
    [Category ("Behavior"), Description ("Occurs when the current page is validating.")]
    public event EventHandler<PageValidatingEventArgs>? PageValidating;

    /// <summary>
    /// Occurs after the current page is successfully validated.
    /// </summary>
    [Category ("Behavior"), Description ("Occurs after the current page is successfully validated.")]
    public event EventHandler<PageEventArgs>? PageValidated;

    /// <summary>
    /// Occurs while the current page is changing and the previous current page is hidden.
    /// </summary>
    [Category ("Behavior"),
     Description ("Occurs when the current page is changing and the previous current page is hidden.")]
    public event EventHandler<PageEventArgs>? PageHidden;

    /// <summary>
    /// Occurs while the current page is changing and the new current page is shown.
    /// </summary>
    [Category ("Behavior"),
     Description ("Occurs while the current page is changing and the new current page is shown.")]
    public event EventHandler<PageEventArgs>? PageShown;

    /// <summary>
    /// Occurs when a page is painted.
    /// </summary>
    [Category ("Appearance"), Description ("Occurs when a page is painted.")]
    public event EventHandler<PagePaintEventArgs>? PagePaint;

    /// <summary>
    /// Occurs when UI controls need to be created.
    /// </summary>
    [Category ("Appearance"), Description ("Occurs when UI controls need to be created.")]
    public event EventHandler<CreateUIControlsEventArgs>? CreateUIControls;

    /// <summary>
    /// Occurs when UI controls need to be updated.
    /// </summary>
    [Category ("Appearance"), Description ("Occurs when UI controls need to be updated.")]
    public event EventHandler? UpdateUIControls;

    #endregion

    #region Member Variables

    private int selectedIndex;
    private Page? lastSelectedPage;
    private Page? selectedPage;
    private BorderStyle borderStyle;
    private bool creatingUIControls;
    private int uiControlCount;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the index of the first page in the controls collection.
    /// </summary>
    internal int FirstPageIndex => uiControlCount;

    /// <summary>
    /// Gets the number of pages.
    /// </summary>
    internal int PageCount => Controls.Count - uiControlCount;

    /// <summary>
    /// Gets or sets the current page.
    /// </summary>
    [Editor (typeof (SelectedPageEditor), typeof (System.Drawing.Design.UITypeEditor))]
    [Category ("Behavior"), DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    [Description ("Gets or sets the current page.")]
    public virtual Page? SelectedPage
    {
        get => selectedPage;
        set => ChangePage (value);
    }

    /// <summary>
    /// Gets or sets the zero-based index of the current page.
    /// </summary>
    [Category ("Behavior"), DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    [Description ("Gets or sets the zero-based index of the current page.")]
    public virtual int SelectedIndex
    {
        get => selectedIndex;
        set => ChangePage (value == -1 ? null : Pages[value]);
    }

    /// <summary>
    /// Gets or sets the collection of pages.
    /// </summary>
    [Category ("Behavior")]
    [Description ("Gets or sets the collection of pages.")]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public virtual PageCollection Pages { get; }

    /// <summary>
    /// Gets the rectangle that represents the client area of the control.
    /// </summary>
    [Browsable (false)]
    public new Rectangle ClientRectangle
    {
        get
        {
            var rect = base.ClientRectangle;
            if (BorderStyle != BorderStyle.None)
            {
                rect.Inflate (-1, -1);
            }

            return rect;
        }
    }

    /// <summary>
    /// Gets or sets the border style of the control.
    /// </summary>
    [Category ("Appearance"), DefaultValue (BorderStyle.FixedSingle)]
    [Description ("Gets or sets the border style of the control.")]
    public BorderStyle BorderStyle
    {
        get => borderStyle;
        set
        {
            borderStyle = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Gets the size of the control when it is initially created.
    /// </summary>
    protected override Size DefaultSize => new Size (300, 200);

    /// <summary>
    /// Determines whether the control can navigate to the previous page.
    /// </summary>
    [Browsable (false)]
    public virtual bool CanGoBack =>
        (Pages.Count != 0) && (SelectedIndex != -1) && !(ReferenceEquals (SelectedPage, Pages[0]));

    /// <summary>
    /// Determines whether the control can navigate to the next page.
    /// </summary>
    [Browsable (false)]
    public virtual bool CanGoNext => (Pages.Count != 0) && (SelectedIndex != -1) &&
                                     !(ReferenceEquals (SelectedPage, Pages[^1]));

    /// <summary>
    /// Gets the client rectangle where pages are located.
    /// </summary>
    [Browsable (false)]
    public override Rectangle DisplayRectangle => new Rectangle (ClientRectangle.Left, ClientRectangle.Top,
        ClientRectangle.Width, ClientRectangle.Height);

    #endregion

    #region Unused Methods - Hide From User

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never), Bindable (false)]
    public override string Text
    {
        get => base.Text;
        set => base.Text = value;
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never),
     DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public new ControlCollection Controls => base.Controls;

    /// <summary>
    ///
    /// </summary>
    [Browsable (false), EditorBrowsable (EditorBrowsableState.Never)]
    public new event EventHandler? TextChanged;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="PagedControl"/> class.
    /// </summary>
    public PagedControl()
    {
        creatingUIControls = false;
        uiControlCount = 0;

        Pages = new PageCollection (this);

        selectedIndex = -1;
        lastSelectedPage = null;
        selectedPage = null;

        borderStyle = BorderStyle.FixedSingle;

        DoubleBuffered = true;
        SetStyle (ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                  ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw, true);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Navigates to the previous page.
    /// </summary>
    public void GoBack()
    {
        if (CanGoBack)
        {
            SelectedIndex = SelectedIndex - 1;
        }
    }

    /// <summary>
    /// Navigates to the next page.
    /// </summary>
    public void GoNext()
    {
        if (CanGoNext)
        {
            SelectedIndex = SelectedIndex + 1;
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Updates the display bounds and visibility of pages.
    /// </summary>
    protected void UpdatePages()
    {
        for (var i = 0; i < Pages.Count; i++)
        {
            var page = Pages[i];

            if (i == SelectedIndex)
            {
                page.Bounds = DisplayRectangle;
                page.Invalidate();

                page.Visible = true;
            }
            else
            {
                page.Visible = false;
            }
        }
    }

    /// <summary>
    /// Changes the currently selected page to the given page.
    /// </summary>
    /// <param name="page">The page to make current.</param>
    /// <param name="allowModify">true to allow the page change to be cancelled and
    /// modifying the new page, otherwise false.</param>
    protected void ChangePage
        (
            Page? page,
            bool allowModify = true
        )
    {
        var index = (page == null) ? -1 : Pages.IndexOf (page);

        if (page != null && index == -1)
        {
            throw new ArgumentException ("Page is not found in the page collection.");
        }

        if (page == null && Pages.Count != 0)
        {
            throw new ArgumentException ("Cannot set SelectedPage to null if the control has at least one page.");
        }

        if (selectedPage != null && page != null && selectedPage == page)
        {
            return;
        }

        if (selectedPage is { CausesValidation: true })
        {
            var pve = new PageValidatingEventArgs (selectedPage);
            OnPageValidating (pve);
            if (allowModify && pve.Cancel)
            {
                return;
            }

            OnPageValidated (new PageEventArgs (selectedPage));
        }

        var pce = new PageChangingEventArgs (selectedPage, page);
        OnPageChanging (pce);

        // Check if the page change is cancelled by user
        if (allowModify && pce.Cancel)
        {
            return;
        }

        // Check if the current page is modified by user
        if (allowModify)
        {
            page = pce.NewPage;
            index = (page == null) ? -1 : Pages.IndexOf (page);

            if (page != null && index == -1)
            {
                throw new ArgumentException ("Page is not found in the page collection.");
            }
            else if (page == null && Pages.Count != 0)
            {
                throw new ArgumentException ("Cannot set SelectedPage to null if the control has at least one page.");
            }
        }

        lastSelectedPage = selectedPage;
        selectedPage = page;
        selectedIndex = index;

        OnUpdateUIControls (EventArgs.Empty);
        UpdatePages();

        if (lastSelectedPage != null)
        {
            OnPageHidden (new PageEventArgs (lastSelectedPage));
        }

        if (selectedPage != null)
        {
            OnPageShown (new PageEventArgs (selectedPage));
        }

        OnPageChanged (new PageChangedEventArgs (lastSelectedPage, selectedPage));
    }

    #endregion

    #region Overriden Methods

    /// <inheritdoc cref="Control.OnHandleCreated"/>
    protected override void OnHandleCreated
        (
            EventArgs eventArgs
        )
    {
        base.OnHandleCreated (eventArgs);

        OnCreateUIControls (new CreateUIControlsEventArgs());
        OnUpdateUIControls (EventArgs.Empty);
        UpdatePages();
    }

    /// <inheritdoc cref="Control.CreateControlsInstance"/>
    protected override ControlCollection CreateControlsInstance()
    {
        return new PagedControlControlCollection (this);
    }

    /// <inheritdoc cref="Control.OnPaint"/>
    protected override void OnPaint
        (
            PaintEventArgs eventArgs
        )
    {
        var graphics = eventArgs.Graphics;
        graphics.Clear (BackColor);

        switch (BorderStyle)
        {
            case BorderStyle.FixedSingle:
                ControlPaint.DrawBorder (graphics, base.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
                break;

            case BorderStyle.Fixed3D:
                ControlPaint.DrawBorder3D (graphics, base.ClientRectangle, Border3DStyle.SunkenOuter);
                break;
        }

        base.OnPaint (eventArgs);
    }

    /// <inheritdoc cref="Control.OnResize"/>
    protected override void OnResize
        (
            EventArgs eventArgs
        )
    {
        base.OnResize (eventArgs);
        UpdatePages();
        OnUpdateUIControls (EventArgs.Empty);
    }

    /// <inheritdoc cref="Control.OnInvalidated"/>
    protected override void OnInvalidated
        (
            InvalidateEventArgs eventArgs
        )
    {
        base.OnInvalidated (eventArgs);

        if (SelectedPage != null)
        {
            SelectedPage.Invalidate (eventArgs.InvalidRect);
        }
    }

    #endregion
}
