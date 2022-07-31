// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* NavigationButton.cs -- навигационная кнопка "туда-сюда"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using AeroSuite.Controls.Design;

#endregion

#nullable enable

namespace AeroSuite.Controls;

/// <summary>
/// A simple Back/Forward Button drawn by Windows via Visual Styles if available.
/// </summary>
/// <remarks>
/// The button is drawn with Visual Styles (Navigation > BackButton/ForwardButton). If running on XP or another OS, the button is drawn manually (in a kinda-Metro-Style)
/// </remarks>
[DesignerCategory ("Code")]
[Designer (typeof (NavigationButtonDesigner))]
[DisplayName ("Navigation Button")]
[Description ("A simple Back/Forward Button drawn by Windows via Visual Styles if available.")]
[ToolboxItem (true)]
[ToolboxBitmap (typeof (NavigationButton))]
public class NavigationButton
    : Control
{
    /// <summary>
    /// Returns the default size.
    /// </summary>
    /// <value>
    /// The default size.
    /// </value>
    protected override Size DefaultSize => new (30, 30);

    private NavigationButtonType _type = NavigationButtonType.Back;

    /// <summary>
    /// Indicates the type.
    /// </summary>
    /// <value>
    /// The type.
    /// </value>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Visible)]

    //[DefaultValue(NavigationButtonType.Back)]
    [RefreshProperties (RefreshProperties.All)]
    [Description ("Indicates the type.")]
    [Category ("Appearance")]
    public virtual NavigationButtonType Type
    {
        get => _type;
        set
        {
            _type = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationButton"/> class.
    /// </summary>
    public NavigationButton()
    {
        _type = NavigationButtonType.Back;
        SetStyle (
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.UserPaint,
                true
            );
        UpdateStyles();
    }

    ///<inheritdoc cref="Control.OnPaint"/>
    protected override void OnPaint (PaintEventArgs e)
    {
        if (Application.RenderWithVisualStyles &&
            VisualStyleRenderer.IsElementDefined (
                VisualStyleElement.CreateElement ("Navigation", 0,
                    0)) /*PlatformHelper.VistaOrHigher && PlatformHelper.VisualStylesEnabled*/
           ) //This seems to be the right check according to the MSDN: https://msdn.microsoft.com/en-us/library/vstudio/ms171735(v=vs.100).aspx
        {
            PaintWithVisualStyles (e.Graphics);
        }
        else
        {
            PaintManually (e.Graphics);
        }

        base.OnPaint (e);
    }

    /// <summary>
    ///
    /// </summary>
    protected PushButtonState state = PushButtonState.Normal;

    /// <summary>
    /// Paints the button with visual styles.
    /// </summary>
    /// <param name="g">The targeted graphics.</param>
    protected virtual void PaintWithVisualStyles (Graphics g)
    {
        //Draw button
        new VisualStyleRenderer ("Navigation", (int)_type,
            Enabled ? (int)state : (int)PushButtonState.Disabled).DrawBackground (g, DisplayRectangle);

        //Draw Focus Rectangle
        if (Focused && ShowFocusCues)
        {
            ControlPaint.DrawFocusRectangle (g, DisplayRectangle);
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected Pen? normalPen;

    /// <summary>
    ///
    /// </summary>
    protected Brush? hoverBrush;

    /// <summary>
    ///
    /// </summary>
    protected Pen? hoverArrowPen;

    /// <summary>
    ///
    /// </summary>
    protected Brush? pressedBrush;

    /// <summary>
    ///
    /// </summary>
    protected Pen? pressedArrowPen;

    /// <summary>
    ///
    /// </summary>
    protected Pen? disabledPen;

    /// <summary>
    /// Paints the button manually.
    /// </summary>
    /// <param name="g">The targeted graphics.</param>
    protected virtual void PaintManually (Graphics g)
    {
        if (normalPen == null)
        {
            normalPen = new Pen (SystemColors.ControlDark, 2);
            hoverBrush = new SolidBrush (SystemColors.ControlDark);
            hoverArrowPen = new Pen (BackColor, 2);
            pressedBrush = new SolidBrush (SystemColors.ControlDarkDark);
            pressedArrowPen = new Pen (BackColor, 2);
            disabledPen = new Pen (SystemColors.ControlLight, 2);
        }

        g.SmoothingMode = SmoothingMode.AntiAlias;

        using var graphicsPath = new GraphicsPath();
        var innerRect = new Rectangle (4, 4, Width - 8, Height - 8);
        if (Type == NavigationButtonType.Back)
        {
            graphicsPath.AddLines (new PointF[]
            {
                new (innerRect.X + innerRect.Width * 0.5f, innerRect.Y + innerRect.Height * 0.25f),
                new (innerRect.X + innerRect.Width * 0.25f, innerRect.Y + innerRect.Height * 0.5f),
                new (innerRect.X + innerRect.Width * 0.5f, innerRect.Y + innerRect.Height * 0.75f)
            });
        }
        else
        {
            graphicsPath.AddLines (new PointF[]
            {
                new (innerRect.X + innerRect.Width * 0.5f, innerRect.Y + innerRect.Height * 0.25f),
                new (innerRect.X + innerRect.Width * 0.75f, innerRect.Y + innerRect.Height * 0.5f),
                new (innerRect.X + innerRect.Width * 0.5f, innerRect.Y + innerRect.Height * 0.75f)
            });
        }

        graphicsPath.StartFigure();
        graphicsPath.AddLine (new PointF (innerRect.X + innerRect.Width * 0.25f, innerRect.Y + innerRect.Height * 0.5f),
            new PointF (innerRect.X + innerRect.Width * 0.75f, innerRect.Y + innerRect.Height * 0.5f));

        if (!Enabled)
        {
            g.DrawEllipse (disabledPen!, new Rectangle (5, 5, Width - 10, Height - 10));
            g.DrawPath (disabledPen!, graphicsPath);
        }
        else
        {
            switch (state)
            {
                case PushButtonState.Normal:
                    g.DrawEllipse (normalPen, new Rectangle (5, 5, Width - 10, Height - 10));
                    g.DrawPath (normalPen, graphicsPath);
                    break;

                case PushButtonState.Hot:
                    g.FillEllipse (hoverBrush!, new Rectangle (4, 4, Width - 8, Height - 8));
                    g.DrawPath (hoverArrowPen!, graphicsPath);
                    break;

                case PushButtonState.Pressed:
                    g.FillEllipse (pressedBrush!, new Rectangle (4, 4, Width - 8, Height - 8));
                    g.DrawPath (pressedArrowPen!, graphicsPath);
                    break;

                default:
                    g.DrawEllipse (disabledPen!, new Rectangle (5, 5, Width - 10, Height - 10));
                    g.DrawPath (disabledPen!, graphicsPath);
                    break;
            }
        }
    }

    /// <inheritdoc cref="Control.OnMouseEnter"/>
    protected override void OnMouseEnter (EventArgs e)
    {
        state = PushButtonState.Hot;
        Invalidate();
        base.OnMouseEnter (e);
    }

    ///<inheritdoc cref="Control.OnMouseLeave"/>
    protected override void OnMouseLeave (EventArgs e)
    {
        state = PushButtonState.Normal;
        Invalidate();
        base.OnMouseLeave (e);
    }

    /// <inheritdoc cref="Control.OnMouseDown"/>
    protected override void OnMouseDown (MouseEventArgs e)
    {
        state = PushButtonState.Pressed;
        Invalidate();
        base.OnMouseDown (e);
    }

    /// <inheritdoc cref="Control.OnMouseUp"/>
    protected override void OnMouseUp (MouseEventArgs e)
    {
        state = (e.X >= 0 && e.X < Width && e.Y >= 0 && e.Y < Height)
            ? PushButtonState.Hot
            : PushButtonState.Normal;
        Invalidate();
        base.OnMouseUp (e);
    }

    /// <inheritdoc cref="Control.OnGotFocus"/>
    protected override void OnGotFocus (EventArgs e)
    {
        Invalidate();
        base.OnGotFocus (e);
    }

    /// <inheritdoc cref="Control.OnLostFocus"/>
    protected override void OnLostFocus (EventArgs e)
    {
        Invalidate();
        base.OnLostFocus (e);
    }

    /// <inheritdoc cref="Control.OnEnabledChanged"/>
    protected override void OnEnabledChanged (EventArgs e)
    {
        Invalidate();
        base.OnEnabledChanged (e);
    }

    /// <inheritdoc cref="Control.OnKeyDown"/>
    protected override void OnKeyDown (KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
        {
            InvokeOnClick (this, EventArgs.Empty);
        }

        base.OnKeyDown (e);
    }

    /// <inheritdoc cref="Control.Dispose(bool)"/>
    protected override void Dispose (bool disposing)
    {
        //Dispose brushes and pens if manual drawing was used
         normalPen?.Dispose();
         hoverBrush?.Dispose();
         hoverArrowPen?.Dispose();
         pressedBrush?.Dispose();
         pressedArrowPen?.Dispose();
         disabledPen?.Dispose();

        base.Dispose (disposing);
    }
}

/// <summary>
/// The Type of a <see cref="NavigationButton"/>.
/// </summary>
public enum NavigationButtonType
{
    /// <summary>
    /// Represents a backward button.
    /// </summary>
    Back = 1,

    /// <summary>
    /// Represents a forward button.
    /// </summary>
    Forward = 2
}
