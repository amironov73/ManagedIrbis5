// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Adapters;
using AM.Windows.Forms.HtmlRenderer.Utilities;

#endregion

namespace AM.Windows.Forms.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WinForms Graphics for core.
/// </summary>
internal sealed class GraphicsAdapter
    : RGraphics
{
    #region Fields and Consts

    /// <summary>
    /// used for <see cref="MeasureString(string,RFont,double,out int,out double)"/> calculation.
    /// </summary>
    private static readonly int[] _charFit = new int[1];

    /// <summary>
    /// used for <see cref="MeasureString(string,RFont,double,out int,out double)"/> calculation.
    /// </summary>
    private static readonly int[] _charFitWidth = new int[1000];

    /// <summary>
    /// Used for GDI+ measure string.
    /// </summary>
    private static readonly CharacterRange[] _characterRanges = new CharacterRange[1];

    /// <summary>
    /// The string format to use for measuring strings for GDI+ text rendering
    /// </summary>
    private static readonly StringFormat _stringFormat;

    /// <summary>
    /// The string format to use for rendering strings for GDI+ text rendering
    /// </summary>
    private static readonly StringFormat _stringFormat2;

    /// <summary>
    /// The wrapped WinForms graphics object
    /// </summary>
    private readonly Graphics _graphics;

    /// <summary>
    /// Use GDI+ text rendering to measure/draw text.
    /// </summary>
    private readonly bool _useGdiPlusTextRendering;

    /// <summary>
    /// the initialized HDC used
    /// </summary>
    private IntPtr _hdc;

    /// <summary>
    /// if to release the graphics object on dispose
    /// </summary>
    private readonly bool _releaseGraphics;

    /// <summary>
    /// If text alignment was set to RTL
    /// </summary>
    private bool _setRtl;

    #endregion

    /// <summary>
    /// Init static resources.
    /// </summary>
    static GraphicsAdapter()
    {
        _stringFormat = new StringFormat (StringFormat.GenericTypographic);
        _stringFormat.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.MeasureTrailingSpaces;

        _stringFormat2 = new StringFormat (StringFormat.GenericTypographic);
    }

    /// <summary>
    /// Init.
    /// </summary>
    /// <param name="graphics">the win forms graphics object to use</param>
    /// <param name="useGdiPlusTextRendering">Use GDI+ text rendering to measure/draw text</param>
    /// <param name="releaseGraphics">optional: if to release the graphics object on dispose (default - false)</param>
    public GraphicsAdapter
        (
            Graphics graphics,
            bool useGdiPlusTextRendering,
            bool releaseGraphics = false
        )
        : base (WinFormsAdapter.Instance, Utils.Convert (graphics.ClipBounds))
    {
        Sure.NotNull (graphics);

        _graphics = graphics;
        _releaseGraphics = releaseGraphics;

        _useGdiPlusTextRendering = useGdiPlusTextRendering;
    }

    public override void PopClip()
    {
        ReleaseHdc();
        _clipStack.Pop();
        _graphics.SetClip (Utils.Convert (_clipStack.Peek()), CombineMode.Replace);
    }

    public override void PushClip (RRect rect)
    {
        ReleaseHdc();
        _clipStack.Push (rect);
        _graphics.SetClip (Utils.Convert (rect), CombineMode.Replace);
    }

    public override void PushClipExclude (RRect rect)
    {
        ReleaseHdc();
        _clipStack.Push (_clipStack.Peek());
        _graphics.SetClip (Utils.Convert (rect), CombineMode.Exclude);
    }

    public override object SetAntiAliasSmoothingMode()
    {
        ReleaseHdc();
        var prevMode = _graphics.SmoothingMode;
        _graphics.SmoothingMode = SmoothingMode.AntiAlias;
        return prevMode;
    }

    public override void ReturnPreviousSmoothingMode
        (
            object? prevMode
        )
    {
        if (prevMode != null)
        {
            ReleaseHdc();
            _graphics.SmoothingMode = (SmoothingMode)prevMode;
        }
    }

    public override RSize MeasureString
        (
            string str,
            RFont font
        )
    {
        if (_useGdiPlusTextRendering)
        {
            ReleaseHdc();
            var fontAdapter = (FontAdapter)font;
            var realFont = fontAdapter.Font;
            _characterRanges[0] = new CharacterRange (0, str.Length);
            _stringFormat.SetMeasurableCharacterRanges (_characterRanges);
            var size = _graphics.MeasureCharacterRanges (str, realFont, RectangleF.Empty, _stringFormat)[0].GetBounds (_graphics)
                .Size;

            if (font.Height < 0)
            {
                var height = realFont.Height;
                var descent = realFont.Size * realFont.FontFamily.GetCellDescent (realFont.Style) /
                              realFont.FontFamily.GetEmHeight (realFont.Style);
                fontAdapter.SetMetrics (height, (int)Math.Round ((height - descent + .5f)));
            }

            return Utils.Convert (size);
        }
        else
        {
            SetFont (font);
            var size = new Size();
            Win32Utils.GetTextExtentPoint32 (_hdc, str, str.Length, ref size);

            if (font.Height < 0)
            {
                Win32Utils.GetTextMetrics (_hdc, out var lptm);
                ((FontAdapter)font).SetMetrics (size.Height, lptm.tmHeight - lptm.tmDescent + lptm.tmUnderlined + 1);
            }

            return Utils.Convert (size);
        }
    }

    public override void MeasureString
        (
            string str,
            RFont font,
            double maxWidth,
            out int charFit,
            out double charFitWidth
        )
    {
        charFit = 0;
        charFitWidth = 0;
        if (_useGdiPlusTextRendering)
        {
            ReleaseHdc();

            var size = MeasureString (str, font);

            for (var i = 1; i <= str.Length; i++)
            {
                charFit = i - 1;
                var pSize = MeasureString (str.Substring (0, i), font);
                if (pSize.Height <= size.Height && pSize.Width < maxWidth)
                {
                    charFitWidth = pSize.Width;
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            SetFont (font);

            var size = new Size();
            Win32Utils.GetTextExtentExPoint (_hdc, str, str.Length, (int)Math.Round (maxWidth), _charFit, _charFitWidth,
                ref size);
            charFit = _charFit[0];
            charFitWidth = charFit > 0 ? _charFitWidth[charFit - 1] : 0;
        }
    }

    public override void DrawString
        (
            string str,
            RFont font,
            RColor color,
            RPoint point,
            RSize size,
            bool rtl
        )
    {
        if (_useGdiPlusTextRendering)
        {
            ReleaseHdc();
            SetRtlAlignGdiPlus (rtl);
            var brush = ((BrushAdapter)_adapter.GetSolidBrush (color)).Brush;
            _graphics.DrawString (str, ((FontAdapter)font).Font, brush, (int)(Math.Round (point.X) + (rtl ? size.Width : 0)),
                (int)Math.Round (point.Y), _stringFormat2);
        }
        else
        {
            var pointConv = Utils.ConvertRound (point);
            var colorConv = Utils.Convert (color);

            if (color.A == 255)
            {
                SetFont (font);
                SetTextColor (colorConv);
                SetRtlAlignGdi (rtl);

                Win32Utils.TextOut (_hdc, pointConv.X, pointConv.Y, str, str.Length);
            }
            else
            {
                InitHdc();
                SetRtlAlignGdi (rtl);
                DrawTransparentText (_hdc, str, font, pointConv, Utils.ConvertRound (size), colorConv);
            }
        }
    }

    public override RBrush GetTextureBrush (RImage image, RRect dstRect, RPoint translateTransformLocation)
    {
        var brush = new TextureBrush (((ImageAdapter)image).Image, Utils.Convert (dstRect));
        brush.TranslateTransform ((float)translateTransformLocation.X, (float)translateTransformLocation.Y);
        return new BrushAdapter (brush, true);
    }

    public override RGraphicsPath GetGraphicsPath()
    {
        return new GraphicsPathAdapter();
    }

    public override void Dispose()
    {
        ReleaseHdc();
        if (_releaseGraphics)
        {
            _graphics.Dispose();
        }

        if (_useGdiPlusTextRendering && _setRtl)
        {
            _stringFormat2.FormatFlags ^= StringFormatFlags.DirectionRightToLeft;
        }
    }

    #region Delegate graphics methods

    public override void DrawLine
        (
            RPen pen,
            double x1,
            double y1,
            double x2,
            double y2
        )
    {
        ReleaseHdc();
        _graphics.DrawLine (((PenAdapter)pen).Pen, (float)x1, (float)y1, (float)x2, (float)y2);
    }

    public override void DrawRectangle
        (
            RPen pen,
            double x,
            double y,
            double width,
            double height
        )
    {
        ReleaseHdc();
        _graphics.DrawRectangle (((PenAdapter)pen).Pen, (float)x, (float)y, (float)width, (float)height);
    }

    public override void DrawRectangle
        (
            RBrush brush,
            double x,
            double y,
            double width,
            double height
        )
    {
        ReleaseHdc();
        _graphics.FillRectangle (((BrushAdapter)brush).Brush, (float)x, (float)y, (float)width, (float)height);
    }

    public override void DrawImage
        (
            RImage image,
            RRect destRect,
            RRect srcRect
        )
    {
        ReleaseHdc();
        _graphics.DrawImage (((ImageAdapter)image).Image, Utils.Convert (destRect), Utils.Convert (srcRect),
            GraphicsUnit.Pixel);
    }

    public override void DrawImage
        (
            RImage image,
            RRect destRect
        )
    {
        ReleaseHdc();
        _graphics.DrawImage (((ImageAdapter)image).Image, Utils.Convert (destRect));
    }

    public override void DrawPath
        (
            RPen pen,
            RGraphicsPath path
        )
    {
        _graphics.DrawPath (((PenAdapter)pen).Pen, ((GraphicsPathAdapter)path).GraphicsPath);
    }

    public override void DrawPath
        (
            RBrush brush,
            RGraphicsPath path
        )
    {
        ReleaseHdc();
        _graphics.FillPath (((BrushAdapter)brush).Brush, ((GraphicsPathAdapter)path).GraphicsPath);
    }

    public override void DrawPolygon
        (
            RBrush brush,
            RPoint[]? points
        )
    {
        if (points is { Length: > 0 })
        {
            ReleaseHdc();
            _graphics.FillPolygon (((BrushAdapter)brush).Brush, Utils.Convert (points));
        }
    }

    #endregion


    #region Private methods

    /// <summary>
    /// Release current HDC to be able to use <see cref="Graphics"/> methods.
    /// </summary>
    private void ReleaseHdc()
    {
        if (_hdc != IntPtr.Zero)
        {
            Win32Utils.SelectClipRgn (_hdc, IntPtr.Zero);
            _graphics.ReleaseHdc (_hdc);
            _hdc = IntPtr.Zero;
        }
    }

    /// <summary>
    /// Init HDC for the current graphics object to be used to call GDI directly.
    /// </summary>
    private void InitHdc()
    {
        if (_hdc == IntPtr.Zero)
        {
            var clip = _graphics.Clip.GetHrgn (_graphics);

            _hdc = _graphics.GetHdc();
            _setRtl = false;
            Win32Utils.SetBkMode (_hdc, 1);

            Win32Utils.SelectClipRgn (_hdc, clip);

            Win32Utils.DeleteObject (clip);
        }
    }

    /// <summary>
    /// Set a resource (e.g. a font) for the specified device context.
    /// WARNING: Calling Font.ToHfont() many times without releasing the font handle crashes the app.
    /// </summary>
    private void SetFont (RFont font)
    {
        InitHdc();
        Win32Utils.SelectObject (_hdc, ((FontAdapter)font).HFont);
    }

    /// <summary>
    /// Set the text color of the device context.
    /// </summary>
    private void SetTextColor
        (
            Color color
        )
    {
        InitHdc();
        var rgb = (color.B & 0xFF) << 16 | (color.G & 0xFF) << 8 | color.R;
        Win32Utils.SetTextColor (_hdc, rgb);
    }

    /// <summary>
    /// Change text align to Left-to-Right or Right-to-Left if required.
    /// </summary>
    private void SetRtlAlignGdi
        (
            bool rtl
        )
    {
        if (_setRtl)
        {
            if (!rtl)
            {
                Win32Utils.SetTextAlign (_hdc, Win32Utils.TextAlignDefault);
            }
        }
        else if (rtl)
        {
            Win32Utils.SetTextAlign (_hdc, Win32Utils.TextAlignRtl);
        }

        _setRtl = rtl;
    }

    /// <summary>
    /// Special draw logic to draw transparent text using GDI.<br/>
    /// 1. Create in-memory DC<br/>
    /// 2. Copy background to in-memory DC<br/>
    /// 3. Draw the text to in-memory DC<br/>
    /// 4. Copy the in-memory DC to the proper location with alpha blend<br/>
    /// </summary>
    private static void DrawTransparentText
        (
            IntPtr hdc,
            string str,
            RFont font,
            Point point,
            Size size,
            Color color
        )
    {
        var memoryHdc = Win32Utils.CreateMemoryHdc (hdc, size.Width, size.Height, out var dib);

        try
        {
            // copy target background to memory HDC so when copied back it will have the proper background
            Win32Utils.BitBlt (memoryHdc, 0, 0, size.Width, size.Height, hdc, point.X, point.Y, Win32Utils.BitBltCopy);

            // Create and select font
            Win32Utils.SelectObject (memoryHdc, ((FontAdapter)font).HFont);
            Win32Utils.SetTextColor (memoryHdc, (color.B & 0xFF) << 16 | (color.G & 0xFF) << 8 | color.R);

            // Draw text to memory HDC
            Win32Utils.TextOut (memoryHdc, 0, 0, str, str.Length);

            // copy from memory HDC to normal HDC with alpha blend so achieve the transparent text
            Win32Utils.AlphaBlend (hdc, point.X, point.Y, size.Width, size.Height, memoryHdc, 0, 0, size.Width,
                size.Height, new BlendFunction (color.A));
        }
        finally
        {
            Win32Utils.ReleaseMemoryHdc (memoryHdc, dib);
        }
    }

    /// <summary>
    /// Change text align to Left-to-Right or Right-to-Left if required.
    /// </summary>
    private void SetRtlAlignGdiPlus
        (
            bool rtl
        )
    {
        if (_setRtl)
        {
            if (!rtl)
            {
                _stringFormat2.FormatFlags ^= StringFormatFlags.DirectionRightToLeft;
            }
        }
        else if (rtl)
        {
            _stringFormat2.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
        }

        _setRtl = rtl;
    }

    #endregion
}
