// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* Code2of5Interleaved.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using AM;

namespace PdfSharpCore.Drawing.BarCodes;

/// <summary>
/// Implementation of the Code 2 of 5 bar code.
/// </summary>
public class Code2of5Interleaved
    : ThickThinBarCode
{
    /// <summary>
    /// Initializes a new instance of Interleaved2of5.
    /// </summary>
    public Code2of5Interleaved()
        : base ("", XSize.Empty, CodeDirection.LeftToRight)
    {
    }

    /// <summary>
    /// Initializes a new instance of Interleaved2of5.
    /// </summary>
    public Code2of5Interleaved (string code)
        : base (code, XSize.Empty, CodeDirection.LeftToRight)
    {
    }

    /// <summary>
    /// Initializes a new instance of Interleaved2of5.
    /// </summary>
    public Code2of5Interleaved (string code, XSize size)
        : base (code, size, CodeDirection.LeftToRight)
    {
    }

    /// <summary>
    /// Initializes a new instance of Interleaved2of5.
    /// </summary>
    public Code2of5Interleaved (string code, XSize size, CodeDirection direction)
        : base (code, size, direction)
    {
    }

    /// <summary>
    /// Returns an array of size 5 that represents the thick (true) and thin (false) lines or spaces
    /// representing the specified digit.
    /// </summary>
    /// <param name="digit">The digit to represent.</param>
    static bool[] ThickAndThinLines (int digit)
    {
        return Lines[digit];
    }

    static readonly bool[][] Lines =
    {
        new[] { false, false, true, true, false },
        new[] { true, false, false, false, true },
        new[] { false, true, false, false, true },
        new[] { true, true, false, false, false },
        new[] { false, false, true, false, true },
        new[] { true, false, true, false, false },
        new[] { false, true, true, false, false },
        new[] { false, false, false, true, true },
        new[] { true, false, false, true, false },
        new[] { false, true, false, true, false },
    };

    /// <inheritdoc cref="BarCode.Render"/>
    protected internal override void Render
        (
            XGraphics graphics,
            XBrush brush,
            XFont font,
            XPoint position
        )
    {
        var state = graphics.Save();

        var info = new BarCodeRenderInfo (graphics, brush, font, position);
        InitRendering (info);
        info.CurrentPositionInString = 0;

        //info.CurrPos = info.Center - Size / 2;
        info.CurrentPosition = position - CalcDistance (AnchorType.TopLeft, Anchor, Size);

        if (TurboBit)
        {
            RenderTurboBit (info, true);
        }

        RenderStart (info);
        while (info.CurrentPositionInString < Text.Length)
        {
            RenderNextPair (info);
        }

        RenderStop (info);
        if (TurboBit)
        {
            RenderTurboBit (info, false);
        }

        if (TextLocation != TextLocation.None)
        {
            RenderText (info);
        }

        graphics.Restore (state);
    }

    /// <inheritdoc cref="ThickThinBarCode.CalcThinBarWidth"/>
    internal override void CalcThinBarWidth
        (
            BarCodeRenderInfo info
        )
    {
        /*
         * The total width is the sum of the following parts:
         * Starting lines      = 4 * thin
         *  +
         * Code Representation = (2 * thick + 3 * thin) * code.Length
         *  +
         * Stopping lines      =  1 * thick + 2 * thin
         *
         * with r = relation ( = thick / thin), this results in
         *
         * Total width = (6 + r + (2 * r + 3) * text.Length) * thin
         */
        var thinLineAmount = 6 + WideNarrowRatio + (2 * WideNarrowRatio + 3) * Text.Length;
        info.ThinBarWidth = Size.Width / thinLineAmount;
    }

    private void RenderStart (BarCodeRenderInfo info)
    {
        RenderBar (info, false);
        RenderGap (info, false);
        RenderBar (info, false);
        RenderGap (info, false);
    }

    private void RenderStop (BarCodeRenderInfo info)
    {
        RenderBar (info, true);
        RenderGap (info, false);
        RenderBar (info, false);
    }

    /// <summary>
    /// Renders the next digit pair as bar code element.
    /// </summary>
    private void RenderNextPair (BarCodeRenderInfo info)
    {
        var digitForLines = int.Parse (Text[info.CurrentPositionInString].ToString());
        var digitForGaps = int.Parse (Text[info.CurrentPositionInString + 1].ToString());
        var linesArray = Lines[digitForLines];
        var gapsArray = Lines[digitForGaps];
        for (var idx = 0; idx < 5; ++idx)
        {
            RenderBar (info, linesArray[idx]);
            RenderGap (info, gapsArray[idx]);
        }

        info.CurrentPositionInString += 2;
    }

    /// <inheritdoc cref="CodeBase.CheckCode"/>
    protected override void CheckCode
        (
            string text
        )
    {
        Sure.NotNullNorEmpty (text);

#if true_
      if (text == null)
        throw new ArgumentNullException("text");

      if (text == "")
        throw new ArgumentException(BcgSR.Invalid2Of5Code(text));

      if (text.Length % 2 != 0)
        throw new ArgumentException(BcgSR.Invalid2Of5Code(text));

      foreach (char ch in text)
      {
        if (!Char.IsDigit(ch))
          throw new ArgumentException(BcgSR.Invalid2Of5Code(text));
      }
#endif
    }
}
