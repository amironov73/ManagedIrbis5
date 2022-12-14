// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable VirtualMemberCallInConstructor

/* InertButtonBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public abstract class InertButtonBase
    : Control
{
    /// <summary>
    ///
    /// </summary>
    protected InertButtonBase()
    {
        SetStyle (ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
    }

    /// <summary>
    ///
    /// </summary>
    public abstract Bitmap? HoverImage { get; }

    /// <summary>
    ///
    /// </summary>
    public abstract Bitmap PressImage { get; }

    /// <summary>
    ///
    /// </summary>
    public abstract Bitmap Image { get; }

    private bool m_isMouseOver = false;

    /// <summary>
    ///
    /// </summary>
    protected bool IsMouseOver
    {
        get => m_isMouseOver;
        private set
        {
            if (m_isMouseOver == value)
            {
                return;
            }

            m_isMouseOver = value;
            Invalidate();
        }
    }

    private bool m_isMouseDown = false;

    /// <summary>
    ///
    /// </summary>
    protected bool IsMouseDown
    {
        get => m_isMouseDown;
        private set
        {
            if (m_isMouseDown == value)
            {
                return;
            }

            m_isMouseDown = value;
            Invalidate();
        }
    }

    /// <inheritdoc cref="Control.DefaultSize"/>
    protected override Size DefaultSize => new Size (16, 15);

    /// <inheritdoc cref="Control.OnMouseMove"/>
    protected override void OnMouseMove (MouseEventArgs e)
    {
        base.OnMouseMove (e);
        var over = ClientRectangle.Contains (e.X, e.Y);
        if (IsMouseOver != over)
        {
            IsMouseOver = over;
        }
    }

    /// <inheritdoc cref="Control.OnMouseEnter"/>
    protected override void OnMouseEnter (EventArgs e)
    {
        base.OnMouseEnter (e);
        if (!IsMouseOver)
        {
            IsMouseOver = true;
        }
    }

    /// <inheritdoc cref="Control.OnMouseLeave"/>
    protected override void OnMouseLeave (EventArgs e)
    {
        base.OnMouseLeave (e);
        if (IsMouseOver)
        {
            IsMouseOver = false;
        }
    }

    /// <inheritdoc cref="Control.OnMouseDown"/>
    protected override void OnMouseDown (MouseEventArgs e)
    {
        base.OnMouseLeave (e);
        if (!IsMouseDown)
        {
            IsMouseDown = true;
        }
    }

    /// <inheritdoc cref="Control.OnMouseUp"/>
    protected override void OnMouseUp (MouseEventArgs e)
    {
        base.OnMouseLeave (e);
        if (IsMouseDown)
        {
            IsMouseDown = false;
        }
    }

    /// <inheritdoc cref="Control.OnPaint"/>
    protected override void OnPaint (PaintEventArgs e)
    {
        if (HoverImage != null)
        {
            if (IsMouseOver && Enabled)
            {
                e.Graphics.DrawImage (
                    IsMouseDown ? PressImage : HoverImage,
                    PatchController.EnableHighDpi == true
                        ? ClientRectangle
                        : new Rectangle (0, 0, Image.Width, Image.Height));
            }
            else
            {
                e.Graphics.DrawImage (
                    Image,
                    PatchController.EnableHighDpi == true
                        ? ClientRectangle
                        : new Rectangle (0, 0, Image.Width, Image.Height));
            }

            base.OnPaint (e);
            return;
        }

        if (IsMouseOver && Enabled)
        {
            using (var pen = new Pen (ForeColor))
            {
                e.Graphics.DrawRectangle (pen, Rectangle.Inflate (ClientRectangle, -1, -1));
            }
        }

        using (var imageAttributes = new ImageAttributes())
        {
            ColorMap[] colorMap = new ColorMap[2];
            colorMap[0] = new ColorMap
            {
                OldColor = Color.FromArgb (0, 0, 0),
                NewColor = ForeColor
            };
            colorMap[1] = new ColorMap
            {
                OldColor = Image.GetPixel (0, 0),
                NewColor = Color.Transparent
            };

            imageAttributes.SetRemapTable (colorMap);

            e.Graphics.DrawImage (
                Image,
                new Rectangle (0, 0, Image.Width, Image.Height),
                0, 0,
                Image.Width,
                Image.Height,
                GraphicsUnit.Pixel,
                imageAttributes);
        }

        base.OnPaint (e);
    }

    /// <summary>
    ///
    /// </summary>
    public void RefreshChanges()
    {
        if (IsDisposed)
        {
            return;
        }

        var mouseOver = ClientRectangle.Contains (PointToClient (Control.MousePosition));
        if (mouseOver != IsMouseOver)
        {
            IsMouseOver = mouseOver;
        }

        OnRefreshChanges();
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnRefreshChanges()
    {
        // пустое тело метода
    }
}
