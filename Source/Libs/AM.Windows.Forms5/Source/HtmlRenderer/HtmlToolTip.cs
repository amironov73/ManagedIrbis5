// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* HtmlToolTip.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

using AM.Drawing.HtmlRenderer.Core;
using AM.Drawing.HtmlRenderer.Core.Entities;
using AM.Windows.Forms.HtmlRenderer.Utilities;

#endregion

#nullable enable

namespace AM.Windows.Forms.HtmlRenderer;

/// <summary>
/// Provides HTML rendering on the tooltips.
/// </summary>
public class HtmlToolTip
    : ToolTip
{
    #region Fields and Consts

    /// <summary>
    /// the container to render and handle the html shown in the tooltip
    /// </summary>
    protected HtmlContainer? _htmlContainer;

    /// <summary>
    /// the raw base stylesheet data used in the control
    /// </summary>
    protected string _baseRawCssData;

    /// <summary>
    /// the base stylesheet data used in the panel
    /// </summary>
    protected CssData _baseCssData;

    /// <summary>
    /// The text rendering hint to be used for text rendering.
    /// </summary>
    protected TextRenderingHint _textRenderingHint = TextRenderingHint.SystemDefault;

    /// <summary>
    /// The CSS class used for tooltip html root div
    /// </summary>
    private string _tooltipCssClass = "htmltooltip";

    /// <summary>
    /// the control that the tooltip is currently showing on.<br/>
    /// Used for link handling.
    /// </summary>
    private Control? _associatedControl;

    /// <summary>
    /// timer used to handle mouse move events when mouse is over the tooltip.<br/>
    /// Used for link handling.
    /// </summary>
    private Timer? _linkHandlingTimer;

    /// <summary>
    /// the handle of the actual tooltip window used to know when the tooltip is hidden<br/>
    /// Used for link handling.
    /// </summary>
    private IntPtr _tooltipHandle;

    /// <summary>
    /// If to handle links in the tooltip (default: false).<br/>
    /// When set to true the mouse pointer will change to hand when hovering over a tooltip and
    /// if clicked the <see cref="LinkClicked"/> event will be raised although the tooltip will be closed.
    /// </summary>
    private bool _allowLinksHandling = true;

    #endregion

    /// <summary>
    /// Init.
    /// </summary>
    public HtmlToolTip()
    {
        _baseRawCssData = null!;
        _baseCssData = null!;

        OwnerDraw = true;

        _htmlContainer = new HtmlContainer();
        _htmlContainer.IsSelectionEnabled = false;
        _htmlContainer.IsContextMenuEnabled = false;
        _htmlContainer.AvoidGeometryAntialias = true;
        _htmlContainer.AvoidImagesLateLoading = true;
        _htmlContainer.RenderError += OnRenderError;
        _htmlContainer.StylesheetLoad += OnStylesheetLoad;
        _htmlContainer.ImageLoad += OnImageLoad;

        Popup += OnToolTipPopup;
        Draw += OnToolTipDraw;
        Disposed += OnToolTipDisposed;

        _linkHandlingTimer = new Timer();
        _linkHandlingTimer.Tick += OnLinkHandlingTimerTick;
        _linkHandlingTimer.Interval = 40;

        _htmlContainer.LinkClicked += OnLinkClicked;
    }

    /// <summary>
    /// Raised when the user clicks on a link in the html.<br/>
    /// Allows canceling the execution of the link.
    /// </summary>
    public event EventHandler<HtmlLinkClickedEventArgs>? LinkClicked;

    /// <summary>
    /// Raised when an error occurred during html rendering.<br/>
    /// </summary>
    public event EventHandler<HtmlRenderErrorEventArgs>? RenderError;

    /// <summary>
    /// Raised when aa stylesheet is about to be loaded by file path or URI by link element.<br/>
    /// This event allows to provide the stylesheet manually or provide new source (file or uri) to load from.<br/>
    /// If no alternative data is provided the original source will be used.<br/>
    /// </summary>
    public event EventHandler<HtmlStylesheetLoadEventArgs>? StylesheetLoad;

    /// <summary>
    /// Raised when an image is about to be loaded by file path or URI.<br/>
    /// This event allows to provide the image manually, if not handled the image will be loaded from file or download from URI.
    /// </summary>
    public event EventHandler<HtmlImageLoadEventArgs>? ImageLoad;

    /// <summary>
    /// Use GDI+ text rendering to measure/draw text.<br/>
    /// </summary>
    /// <remarks>
    /// <para>
    /// GDI+ text rendering is less smooth than GDI text rendering but it natively supports alpha channel
    /// thus allows creating transparent images.
    /// </para>
    /// <para>
    /// While using GDI+ text rendering you can control the text rendering using <see cref="Graphics.TextRenderingHint"/>, note that
    /// using ClearTypeGridFit doesn't work well with transparent background.
    /// </para>
    /// </remarks>
    [Category ("Behavior")]
    [DefaultValue (false)]
    [EditorBrowsable (EditorBrowsableState.Always)]
    [Description ("If to use GDI+ text rendering to measure/draw text, false - use GDI")]
    public bool UseGdiPlusTextRendering
    {
        get => _htmlContainer!.UseGdiPlusTextRendering;
        set => _htmlContainer!.UseGdiPlusTextRendering = value;
    }

    /// <summary>
    /// The text rendering hint to be used for text rendering.
    /// </summary>
    [Category ("Behavior")]
    [EditorBrowsable (EditorBrowsableState.Always)]
    [DefaultValue (TextRenderingHint.SystemDefault)]
    [Description ("The text rendering hint to be used for text rendering.")]
    public TextRenderingHint TextRenderingHint
    {
        get => _textRenderingHint;
        set => _textRenderingHint = value;
    }

    /// <summary>
    /// Set base stylesheet to be used by html rendered in the panel.
    /// </summary>
    [Browsable (true)]
    [Description ("Set base stylesheet to be used by html rendered in the tooltip.")]
    [Category ("Appearance")]
    [Editor (
        "System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
        "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public virtual string BaseStylesheet
    {
        get => _baseRawCssData;
        set
        {
            _baseRawCssData = value;
            _baseCssData = HtmlRender.ParseStyleSheet (value);
        }
    }

    /// <summary>
    /// The CSS class used for tooltip html root div (default: htmltooltip)<br/>
    /// Setting to 'null' clear base style on the tooltip.<br/>
    /// Set custom class found in <see cref="BaseStylesheet"/> to change the base style of the tooltip.
    /// </summary>
    [Browsable (true)]
    [Description ("The CSS class used for tooltip html root div.")]
    [Category ("Appearance")]
    public virtual string TooltipCssClass
    {
        get => _tooltipCssClass;
        set => _tooltipCssClass = value;
    }

    /// <summary>
    /// If to handle links in the tooltip (default: false).<br/>
    /// When set to true the mouse pointer will change to hand when hovering over a tooltip and
    /// if clicked the <see cref="LinkClicked"/> event will be raised although the tooltip will be closed.
    /// </summary>
    [Browsable (true)]
    [DefaultValue (false)]
    [Description ("If to handle links in the tooltip.")]
    [Category ("Behavior")]
    public virtual bool AllowLinksHandling
    {
        get => _allowLinksHandling;
        set => _allowLinksHandling = value;
    }

    /// <summary>
    /// Gets or sets the max size the tooltip.
    /// </summary>
    /// <returns>An ordered pair of type <see cref="T:System.Drawing.Size"/> representing the width and height of a rectangle.</returns>
    [Browsable (true)]
    [Category ("Layout")]
    [Description ("Restrict the max size of the shown tooltip (0 is not restricted)")]
    public virtual Size MaximumSize
    {
        get => Size.Round (_htmlContainer!.MaxSize);
        set => _htmlContainer!.MaxSize = value;
    }

    #region Private methods

    /// <summary>
    /// On tooltip appear set the html by the associated control, layout and set the tooltip size by the html size.
    /// </summary>
    protected virtual void OnToolTipPopup (PopupEventArgs e)
    {
        //Create fragment container
        var cssClass = string.IsNullOrEmpty (_tooltipCssClass)
            ? null
            : $" class=\"{_tooltipCssClass}\"";
        var tooltipHtml = $"<div{cssClass}>{GetToolTip (e.AssociatedControl)}</div>";
        _htmlContainer!.SetHtml (tooltipHtml, _baseCssData);
        _htmlContainer.MaxSize = MaximumSize;

        //Measure size of the container
        using (var g = e.AssociatedControl!.CreateGraphics())
        {
            g.TextRenderingHint = _textRenderingHint;
            _htmlContainer.PerformLayout (g);
        }

        //Set the size of the tooltip
        var desiredWidth = (int)Math.Ceiling (MaximumSize.Width > 0
            ? Math.Min (_htmlContainer.ActualSize.Width, MaximumSize.Width)
            : _htmlContainer.ActualSize.Width);
        var desiredHeight = (int)Math.Ceiling (MaximumSize.Height > 0
            ? Math.Min (_htmlContainer.ActualSize.Height, MaximumSize.Height)
            : _htmlContainer.ActualSize.Height);
        e.ToolTipSize = new Size (desiredWidth, desiredHeight);

        // start mouse handle timer
        if (_allowLinksHandling)
        {
            _associatedControl = e.AssociatedControl;
            _linkHandlingTimer!.Start();
        }
    }

    /// <summary>
    /// Draw the html using the tooltip graphics.
    /// </summary>
    protected virtual void OnToolTipDraw (DrawToolTipEventArgs eventArgs)
    {
        if (_tooltipHandle == IntPtr.Zero)
        {
            // get the handle of the tooltip window using the graphics device context
            var hdc = eventArgs.Graphics.GetHdc();
            _tooltipHandle = Win32Utils.WindowFromDC (hdc);
            eventArgs.Graphics.ReleaseHdc (hdc);

            AdjustTooltipPosition (eventArgs.AssociatedControl!, eventArgs.Bounds.Size);
        }

        eventArgs.Graphics.Clear (Color.White);
        eventArgs.Graphics.TextRenderingHint = _textRenderingHint;
        _htmlContainer!.PerformPaint (eventArgs.Graphics);
    }

    /// <summary>
    /// Adjust the location of the tooltip window to the location of the mouse and handle
    /// if the tooltip window will try to appear outside the boundaries of the control.
    /// </summary>
    /// <param name="associatedControl">the control the tooltip is appearing on</param>
    /// <param name="size">the size of the tooltip window</param>
    protected virtual void AdjustTooltipPosition (Control associatedControl, Size size)
    {
        var mousePos = Control.MousePosition;
        var screenBounds = Screen.FromControl (associatedControl).WorkingArea;

        // adjust if tooltip is outside form bounds
        if (mousePos.X + size.Width > screenBounds.Right)
        {
            mousePos.X = Math.Max (screenBounds.Right - size.Width - 5, screenBounds.Left + 3);
        }

        const int yOffset = 20;
        if (mousePos.Y + size.Height + yOffset > screenBounds.Bottom)
        {
            mousePos.Y = Math.Max (screenBounds.Bottom - size.Height - yOffset - 3, screenBounds.Top + 2);
        }

        // move the tooltip window to new location
        Win32Utils.MoveWindow (_tooltipHandle, mousePos.X, mousePos.Y + yOffset, size.Width, size.Height, false);
    }

    /// <summary>
    /// Propagate the LinkClicked event from root container.
    /// </summary>
    protected virtual void OnLinkClicked (HtmlLinkClickedEventArgs eventArgs)
    {
        LinkClicked?.Invoke (this, eventArgs);
    }

    /// <summary>
    /// Propagate the Render Error event from root container.
    /// </summary>
    protected virtual void OnRenderError (HtmlRenderErrorEventArgs eventArgs)
    {
        RenderError?.Invoke (this, eventArgs);
    }

    /// <summary>
    /// Propagate the stylesheet load event from root container.
    /// </summary>
    protected virtual void OnStylesheetLoad (HtmlStylesheetLoadEventArgs eventArgs)
    {
        StylesheetLoad?.Invoke (this, eventArgs);
    }

    /// <summary>
    /// Propagate the image load event from root container.
    /// </summary>
    protected virtual void OnImageLoad (HtmlImageLoadEventArgs eventArgs)
    {
        ImageLoad?.Invoke (this, eventArgs);
    }

    /// <summary>
    /// Raised on link handling timer tick, used for:
    /// 1. Know when the tooltip is hidden by checking the visibility of the tooltip window.
    /// 2. Call HandleMouseMove so the mouse cursor will react if over a link element.
    /// 3. Call HandleMouseDown and HandleMouseUp to simulate click on a link if one was clicked.
    /// </summary>
    protected virtual void OnLinkHandlingTimerTick
        (
            EventArgs eventArgs
        )
    {
        try
        {
            var handle = _tooltipHandle;
            if (handle != IntPtr.Zero && Win32Utils.IsWindowVisible (handle))
            {
                var mPos = Control.MousePosition;
                var mButtons = Control.MouseButtons;
                var rect = Win32Utils.GetWindowRectangle (handle);
                if (rect.Contains (mPos))
                {
                    _htmlContainer!.HandleMouseMove
                        (
                            _associatedControl!,
                            new MouseEventArgs (mButtons, 0, mPos.X - rect.X, mPos.Y - rect.Y, 0)
                        );
                }
            }
            else
            {
                _linkHandlingTimer!.Stop();
                _tooltipHandle = IntPtr.Zero;

                var mPos = Control.MousePosition;
                var mButtons = Control.MouseButtons;
                var rect = Win32Utils.GetWindowRectangle (handle);
                if (rect.Contains (mPos))
                {
                    if (mButtons == MouseButtons.Left)
                    {
                        var args = new MouseEventArgs (mButtons, 1, mPos.X - rect.X, mPos.Y - rect.Y, 0);
                        _htmlContainer!.HandleMouseDown (_associatedControl!, args);
                        _htmlContainer.HandleMouseUp (_associatedControl!, args);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            OnRenderError (this,
                new HtmlRenderErrorEventArgs (HtmlRenderErrorType.General, "Error in link handling for tooltip", ex));
        }
    }

    /// <summary>
    /// Unsubscribe from events and dispose of <see cref="_htmlContainer"/>.
    /// </summary>
    protected virtual void OnToolTipDisposed (EventArgs e)
    {
        Popup -= OnToolTipPopup;
        Draw -= OnToolTipDraw;
        Disposed -= OnToolTipDisposed;

        if (_htmlContainer != null)
        {
            _htmlContainer.RenderError -= OnRenderError;
            _htmlContainer.StylesheetLoad -= OnStylesheetLoad;
            _htmlContainer.ImageLoad -= OnImageLoad;
            _htmlContainer.Dispose();
            _htmlContainer = null;
        }

        if (_linkHandlingTimer != null)
        {
            _linkHandlingTimer.Dispose();
            _linkHandlingTimer = null;

            if (_htmlContainer != null)
            {
                _htmlContainer.LinkClicked -= OnLinkClicked;
            }
        }
    }

    #region Private event handlers

    private void OnToolTipPopup
        (
            object? sender,
            PopupEventArgs eventArgs
        )
    {
        OnToolTipPopup (eventArgs);
    }

    private void OnToolTipDraw
        (
            object? sender,
            DrawToolTipEventArgs eventArgs
        )
    {
        OnToolTipDraw (eventArgs);
    }

    private void OnRenderError
        (
            object? sender,
            HtmlRenderErrorEventArgs eventArgs
        )
    {
        OnRenderError (eventArgs);
    }

    private void OnStylesheetLoad
        (
            object? sender,
            HtmlStylesheetLoadEventArgs eventArgs
        )
    {
        OnStylesheetLoad (eventArgs);
    }

    private void OnImageLoad
        (
            object? sender,
            HtmlImageLoadEventArgs eventArgs
        )
    {
        OnImageLoad (eventArgs);
    }

    private void OnLinkClicked
        (
            object? sender,
            HtmlLinkClickedEventArgs eventArgs
        )
    {
        OnLinkClicked (eventArgs);
    }

    private void OnLinkHandlingTimerTick
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        OnLinkHandlingTimerTick (eventArgs);
    }

    private void OnToolTipDisposed
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        OnToolTipDisposed (eventArgs);
    }

    #endregion

    #endregion
}
