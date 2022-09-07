// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ThickThinBarcodeRenderer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using PdfSharpCore.Fonts;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing.BarCodes;

/// <summary>
/// Internal base class for several bar code types.
/// </summary>
public abstract class ThickThinBarCode // TODO: The name is not optimal
    : BarCode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThickThinBarCode"/> class.
    /// </summary>
    public ThickThinBarCode (string code, XSize size, CodeDirection direction)
        : base (code, size, direction)
    {
    }

    internal override void InitRendering (BarCodeRenderInfo info)
    {
        base.InitRendering (info);
        CalcThinBarWidth (info);
        info.BarHeight = Size.Height;

        // HACK in ThickThinBarCode
        if (TextLocation != TextLocation.None)
        {
            info.BarHeight *= 4.0 / 5;
        }

#if DEBUG_
            XColor back = XColors.LightSalmon;
            back.A = 0.3;
            XSolidBrush brush = new XSolidBrush(back);
            info.Gfx.DrawRectangle(brush, new XRect(info.Center - size / 2, size));
#endif
        switch (Direction)
        {
            case CodeDirection.RightToLeft:
                info.Graphics.RotateAtTransform (180, info.Position);
                break;

            case CodeDirection.TopToBottom:
                info.Graphics.RotateAtTransform (90, info.Position);
                break;

            case CodeDirection.BottomToTop:
                info.Graphics.RotateAtTransform (-90, info.Position);
                break;
        }
    }

    /// <summary>
    /// Gets or sets the ration between thick an thin lines. Must be between 2 and 3.
    /// Optimal and also default value is 2.6.
    /// </summary>
    public override double WideNarrowRatio
    {
        get { return _wideNarrowRatio; }
        set
        {
            if (value > 3 || value < 2)
            {
                throw new ArgumentOutOfRangeException ("value", BcgSR.Invalid2of5Relation);
            }

            _wideNarrowRatio = value;
        }
    }

    double _wideNarrowRatio = 2.6;

    /// <summary>
    /// Renders a thick or thin line for the bar code.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="isThick">Determines whether a thick or a thin line is about to be rendered.</param>
    internal void RenderBar (BarCodeRenderInfo info, bool isThick)
    {
        var barWidth = GetBarWidth (info, isThick);
        var height = Size.Height;
        var xPos = info.CurrentPosition.X;
        var yPos = info.CurrentPosition.Y;

        switch (TextLocation)
        {
            case TextLocation.AboveEmbedded:
                height -= info.Graphics.MeasureString (Text, info.Font).Height;
                yPos += info.Graphics.MeasureString (Text, info.Font).Height;
                break;
            case TextLocation.BelowEmbedded:
                height -= info.Graphics.MeasureString (Text, info.Font).Height;
                break;
        }

        var rect = new XRect (xPos, yPos, barWidth, height);
        info.Graphics.DrawRectangle (info.Brush, rect);
        info.CurrentPosition.X += barWidth;
    }

    /// <summary>
    /// Renders a thick or thin gap for the bar code.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="isThick">Determines whether a thick or a thin gap is about to be rendered.</param>
    internal void RenderGap (BarCodeRenderInfo info, bool isThick)
    {
        info.CurrentPosition.X += GetBarWidth (info, isThick);
    }

    /// <summary>
    /// Renders a thick bar before or behind the code.
    /// </summary>
    internal void RenderTurboBit (BarCodeRenderInfo info, bool startBit)
    {
        if (startBit)
        {
            info.CurrentPosition.X -= 0.5 + GetBarWidth (info, true);
        }
        else
        {
            info.CurrentPosition.X += 0.5; //GetBarWidth(info, true);
        }

        RenderBar (info, true);

        if (startBit)
        {
            info.CurrentPosition.X += 0.5; //GetBarWidth(info, true);
        }
    }

    internal void RenderText (BarCodeRenderInfo info)
    {
        if (info.Font == null)
        {
            info.Font = new XFont (GlobalFontSettings.FontResolver.DefaultFontName, Size.Height / 6);
        }

        var center = info.Position + CalcDistance (Anchor, AnchorType.TopLeft, Size);

        switch (TextLocation)
        {
            case TextLocation.Above:
                center = new XPoint (center.X, center.Y - info.Graphics.MeasureString (Text, info.Font).Height);
                info.Graphics.DrawString (Text, info.Font, info.Brush, new XRect (center, Size),
                    XStringFormats.TopCenter);
                break;

            case TextLocation.AboveEmbedded:
                info.Graphics.DrawString (Text, info.Font, info.Brush, new XRect (center, Size),
                    XStringFormats.TopCenter);
                break;

            case TextLocation.Below:
                center = new XPoint (center.X, info.Graphics.MeasureString (Text, info.Font).Height + center.Y);
                info.Graphics.DrawString (Text, info.Font, info.Brush, new XRect (center, Size),
                    XStringFormats.BottomCenter);
                break;

            case TextLocation.BelowEmbedded:
                info.Graphics.DrawString (Text, info.Font, info.Brush, new XRect (center, Size),
                    XStringFormats.BottomCenter);
                break;
        }
    }

    /// <summary>
    /// Gets the width of a thick or a thin line (or gap). CalcLineWidth must have been called before.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="isThick">Determines whether a thick line's with shall be returned.</param>
    internal double GetBarWidth (BarCodeRenderInfo info, bool isThick)
    {
        if (isThick)
        {
            return info.ThinBarWidth * _wideNarrowRatio;
        }

        return info.ThinBarWidth;
    }

    internal abstract void CalcThinBarWidth (BarCodeRenderInfo info);
}
