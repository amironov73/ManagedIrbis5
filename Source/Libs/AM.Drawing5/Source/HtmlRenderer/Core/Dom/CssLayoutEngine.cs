// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CssLayoutEngine.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Drawing.HtmlRenderer.Adapters;
using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Core.Utils;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Dom;

/// <summary>
/// Helps on CSS Layout.
/// </summary>
internal static class CssLayoutEngine
{
    /// <summary>
    /// Measure image box size by the width\height set on the box and the actual rendered image size.<br/>
    /// If no image exists for the box error icon will be set.
    /// </summary>
    /// <param name="imageWord">the image word to measure</param>
    public static void MeasureImageSize
        (
            CssRectImage imageWord
        )
    {
        Sure.NotNull (imageWord);
        Sure.NotNull (imageWord.OwnerBox);

        var width = new CssLength (imageWord.OwnerBox.Width);
        var height = new CssLength (imageWord.OwnerBox.Height);

        var hasImageTagWidth = width.Number > 0 && width.Unit == CssUnit.Pixels;
        var hasImageTagHeight = height.Number > 0 && height.Unit == CssUnit.Pixels;
        var scaleImageHeight = false;

        if (hasImageTagWidth)
        {
            imageWord.Width = width.Number;
        }
        else if (width.Number > 0 && width.IsPercentage)
        {
            imageWord.Width = width.Number * imageWord.OwnerBox.ContainingBlock.Size.Width;
            scaleImageHeight = true;
        }
        else if (imageWord.Image != null)
        {
            imageWord.Width = imageWord.ImageRectangle == RRect.Empty
                ? imageWord.Image.Width
                : imageWord.ImageRectangle.Width;
        }
        else
        {
            imageWord.Width = hasImageTagHeight ? height.Number / 1.14f : 20;
        }

        var maxWidth = new CssLength (imageWord.OwnerBox.MaxWidth);
        if (maxWidth.Number > 0)
        {
            double maxWidthVal = -1;
            if (maxWidth.Unit == CssUnit.Pixels)
            {
                maxWidthVal = maxWidth.Number;
            }
            else if (maxWidth.IsPercentage)
            {
                maxWidthVal = maxWidth.Number * imageWord.OwnerBox.ContainingBlock.Size.Width;
            }

            if (maxWidthVal > -1 && imageWord.Width > maxWidthVal)
            {
                imageWord.Width = maxWidthVal;
                scaleImageHeight = !hasImageTagHeight;
            }
        }

        if (hasImageTagHeight)
        {
            imageWord.Height = height.Number;
        }
        else if (imageWord.Image != null)
        {
            imageWord.Height = imageWord.ImageRectangle == RRect.Empty
                ? imageWord.Image.Height
                : imageWord.ImageRectangle.Height;
        }
        else
        {
            imageWord.Height = imageWord.Width > 0 ? imageWord.Width * 1.14f : 22.8f;
        }

        if (imageWord.Image != null)
        {
            // If only the width was set in the html tag, ratio the height.
            if ((hasImageTagWidth && !hasImageTagHeight) || scaleImageHeight)
            {
                // Divide the given tag width with the actual image width, to get the ratio.
                var ratio = imageWord.Width / imageWord.Image.Width;
                imageWord.Height = imageWord.Image.Height * ratio;
            }

            // If only the height was set in the html tag, ratio the width.
            else if (hasImageTagHeight && !hasImageTagWidth)
            {
                // Divide the given tag height with the actual image height, to get the ratio.
                var ratio = imageWord.Height / imageWord.Image.Height;
                imageWord.Width = imageWord.Image.Width * ratio;
            }
        }

        imageWord.Height += imageWord.OwnerBox.ActualBorderBottomWidth + imageWord.OwnerBox.ActualBorderTopWidth +
                            imageWord.OwnerBox.ActualPaddingTop + imageWord.OwnerBox.ActualPaddingBottom;
    }

    /// <summary>
    /// Creates line boxes for the specified blockbox
    /// </summary>
    /// <param name="graphics"></param>
    /// <param name="blockBox"></param>
    public static void CreateLineBoxes
        (
            RGraphics graphics,
            CssBox blockBox
        )
    {
        Sure.NotNull (graphics);
        Sure.NotNull (blockBox);

        blockBox.LineBoxes.Clear();

        var limitRight = blockBox.ActualRight - blockBox.ActualPaddingRight - blockBox.ActualBorderRightWidth;

        //Get the start x and y of the blockBox
        var startx = blockBox.Location.X + blockBox.ActualPaddingLeft - 0 + blockBox.ActualBorderLeftWidth;
        var starty = blockBox.Location.Y + blockBox.ActualPaddingTop - 0 + blockBox.ActualBorderTopWidth;
        var curx = startx + blockBox.ActualTextIndent;
        var cury = starty;

        //Reminds the maximum bottom reached
        var maxRight = startx;
        var maxBottom = starty;

        //First line box
        var line = new CssLineBox (blockBox);

        //Flow words and boxes
        FlowBox (graphics, blockBox, blockBox, limitRight, 0, startx, ref line, ref curx, ref cury, ref maxRight,
            ref maxBottom);

        // if width is not restricted we need to lower it to the actual width
        if (blockBox.ActualRight >= 90999)
        {
            blockBox.ActualRight = maxRight + blockBox.ActualPaddingRight + blockBox.ActualBorderRightWidth;
        }

        //Gets the rectangles for each line-box
        foreach (var linebox in blockBox.LineBoxes)
        {
            ApplyHorizontalAlignment (graphics, linebox);
            ApplyRightToLeft (blockBox, linebox);
            BubbleRectangles (blockBox, linebox);
            ApplyVerticalAlignment (graphics, linebox);
            linebox.AssignRectanglesToBoxes();
        }

        blockBox.ActualBottom = maxBottom + blockBox.ActualPaddingBottom + blockBox.ActualBorderBottomWidth;

        // handle limiting block height when overflow is hidden
        if (blockBox.Height != null && blockBox.Height != CssConstants.Auto &&
            blockBox.Overflow == CssConstants.Hidden &&
            blockBox.ActualBottom - blockBox.Location.Y > blockBox.ActualHeight)
        {
            blockBox.ActualBottom = blockBox.Location.Y + blockBox.ActualHeight;
        }
    }

    /// <summary>
    /// Applies special vertical alignment for table-cells
    /// </summary>
    /// <param name="graphics"></param>
    /// <param name="cell"></param>
    public static void ApplyCellVerticalAlignment
        (
            RGraphics graphics,
            CssBox cell
        )
    {
        Sure.NotNull (graphics);
        Sure.NotNull (cell);

        if (cell.VerticalAlign == CssConstants.Top || cell.VerticalAlign == CssConstants.Baseline)
            return;

        var cellbot = cell.ClientBottom;
        var bottom = cell.GetMaximumBottom (cell, 0f);
        double dist = 0f;

        if (cell.VerticalAlign == CssConstants.Bottom)
        {
            dist = cellbot - bottom;
        }
        else if (cell.VerticalAlign == CssConstants.Middle)
        {
            dist = (cellbot - bottom) / 2;
        }

        foreach (var b in cell.Boxes)
        {
            b.OffsetTop (dist);
        }

        //float top = cell.ClientTop;
        //float bottom = cell.ClientBottom;
        //bool middle = cell.VerticalAlign == CssConstants.Middle;

        //foreach (LineBox line in cell.LineBoxes)
        //{
        //    for (int i = 0; i < line.RelatedBoxes.Count; i++)
        //    {

        //        double diff = bottom - line.RelatedBoxes[i].Rectangles[line].Bottom;
        //        if (middle) diff /= 2f;
        //        RectangleF r = line.RelatedBoxes[i].Rectangles[line];
        //        line.RelatedBoxes[i].Rectangles[line] = new RectangleF(r.X, r.Y + diff, r.Width, r.Height);

        //    }

        //    foreach (BoxWord word in line.Words)
        //    {
        //        double gap = word.Top - top;
        //        word.Top = bottom - gap - word.Height;
        //    }
        //}
    }


    #region Private methods

    /// <summary>
    /// Recursively flows the content of the box using the inline model
    /// </summary>
    /// <param name="g">Device Info</param>
    /// <param name="blockbox">Blockbox that contains the text flow</param>
    /// <param name="box">Current box to flow its content</param>
    /// <param name="limitRight">Maximum reached right</param>
    /// <param name="linespacing">Space to use between rows of text</param>
    /// <param name="startx">x starting coordinate for when breaking lines of text</param>
    /// <param name="line">Current linebox being used</param>
    /// <param name="curx">Current x coordinate that will be the left of the next word</param>
    /// <param name="cury">Current y coordinate that will be the top of the next word</param>
    /// <param name="maxRight">Maximum right reached so far</param>
    /// <param name="maxbottom">Maximum bottom reached so far</param>
    private static void FlowBox (RGraphics g, CssBox blockbox, CssBox box, double limitRight, double linespacing,
        double startx, ref CssLineBox line, ref double curx, ref double cury, ref double maxRight, ref double maxbottom)
    {
        var startX = curx;
        var startY = cury;
        box.FirstHostingLineBox = line;
        var localCurx = curx;
        var localMaxRight = maxRight;
        var localmaxbottom = maxbottom;

        foreach (var b in box.Boxes)
        {
            var leftspacing = (b.Position != CssConstants.Absolute && b.Position != CssConstants.Fixed)
                ? b.ActualMarginLeft + b.ActualBorderLeftWidth + b.ActualPaddingLeft
                : 0;
            var rightspacing = (b.Position != CssConstants.Absolute && b.Position != CssConstants.Fixed)
                ? b.ActualMarginRight + b.ActualBorderRightWidth + b.ActualPaddingRight
                : 0;

            b.RectanglesReset();
            b.MeasureWordsSize (g);

            curx += leftspacing;

            if (b.Words.Count > 0)
            {
                var wrapNoWrapBox = false;
                if (b.WhiteSpace == CssConstants.NoWrap && curx > startx)
                {
                    var boxRight = curx;
                    foreach (var word in b.Words)
                        boxRight += word.FullWidth;
                    if (boxRight > limitRight)
                        wrapNoWrapBox = true;
                }

                if (DomUtils.IsBoxHasWhitespace (b))
                    curx += box.ActualWordSpacing;

                foreach (var word in b.Words)
                {
                    if (maxbottom - cury < box.ActualLineHeight)
                        maxbottom += box.ActualLineHeight - (maxbottom - cury);

                    if ((b.WhiteSpace != CssConstants.NoWrap && b.WhiteSpace != CssConstants.Pre &&
                         curx + word.Width + rightspacing > limitRight
                         && (b.WhiteSpace != CssConstants.PreWrap || !word.IsSpaces))
                        || word.IsLineBreak || wrapNoWrapBox)
                    {
                        wrapNoWrapBox = false;
                        curx = startx;

                        // handle if line is wrapped for the first text element where parent has left margin\padding
                        if (b == box.Boxes[0] && !word.IsLineBreak &&
                            (word == b.Words[0] || (box.ParentBox != null && box.ParentBox.IsBlock)))
                            curx += box.ActualMarginLeft + box.ActualBorderLeftWidth + box.ActualPaddingLeft;

                        cury = maxbottom + linespacing;

                        line = new CssLineBox (blockbox);

                        if (word.IsImage || word.Equals (b.FirstWord))
                        {
                            curx += leftspacing;
                        }
                    }

                    line.ReportExistanceOf (word);

                    word.Left = curx;
                    word.Top = cury;

                    if (!box.IsFixed)
                    {
                        word.BreakPage();
                    }

                    curx = word.Left + word.FullWidth;

                    maxRight = Math.Max (maxRight, word.Right);
                    maxbottom = Math.Max (maxbottom, word.Bottom);

                    if (b.Position == CssConstants.Absolute)
                    {
                        word.Left += box.ActualMarginLeft;
                        word.Top += box.ActualMarginTop;
                    }
                }
            }
            else
            {
                FlowBox (g, blockbox, b, limitRight, linespacing, startx, ref line, ref curx, ref cury, ref maxRight,
                    ref maxbottom);
            }

            curx += rightspacing;
        }

        // handle height setting
        if (maxbottom - startY < box.ActualHeight)
        {
            maxbottom += box.ActualHeight - (maxbottom - startY);
        }

        // handle width setting
        if (box.IsInline && 0 <= curx - startX && curx - startX < box.ActualWidth)
        {
            // hack for actual width handling
            curx += box.ActualWidth - (curx - startX);
            line.Rectangles.Add (box, new RRect (startX, startY, box.ActualWidth, box.ActualHeight));
        }

        // handle box that is only a whitespace
        if (box.Text != null && box.Text.IsWhitespace() && !box.IsImage && box.IsInline && box.Boxes.Count == 0 &&
            box.Words.Count == 0)
        {
            curx += box.ActualWordSpacing;
        }

        // hack to support specific absolute position elements
        if (box.Position == CssConstants.Absolute)
        {
            curx = localCurx;
            maxRight = localMaxRight;
            maxbottom = localmaxbottom;
            AdjustAbsolutePosition (box, 0, 0);
        }

        box.LastHostingLineBox = line;
    }

    /// <summary>
    /// Adjust the position of absolute elements by letf and top margins.
    /// </summary>
    private static void AdjustAbsolutePosition (CssBox box, double left, double top)
    {
        left += box.ActualMarginLeft;
        top += box.ActualMarginTop;
        if (box.Words.Count > 0)
        {
            foreach (var word in box.Words)
            {
                word.Left += left;
                word.Top += top;
            }
        }
        else
        {
            foreach (var b in box.Boxes)
                AdjustAbsolutePosition (b, left, top);
        }
    }

    /// <summary>
    /// Recursively creates the rectangles of the blockBox, by bubbling from deep to outside of the boxes
    /// in the rectangle structure
    /// </summary>
    private static void BubbleRectangles (CssBox box, CssLineBox line)
    {
        if (box.Words.Count > 0)
        {
            double x = Single.MaxValue, y = Single.MaxValue, r = Single.MinValue, b = Single.MinValue;
            var words = line.WordsOf (box);

            if (words.Count > 0)
            {
                foreach (var word in words)
                {
                    // handle if line is wrapped for the first text element where parent has left margin\padding
                    var left = word.Left;

                    if (box == box.ParentBox.Boxes[0] && word == box.Words[0] && word == line.Words[0] &&
                        line != line.OwnerBox.LineBoxes[0] && !word.IsLineBreak)
                        left -= box.ParentBox.ActualMarginLeft + box.ParentBox.ActualBorderLeftWidth +
                                box.ParentBox.ActualPaddingLeft;


                    x = Math.Min (x, left);
                    r = Math.Max (r, word.Right);
                    y = Math.Min (y, word.Top);
                    b = Math.Max (b, word.Bottom);
                }

                line.UpdateRectangle (box, x, y, r, b);
            }
        }
        else
        {
            foreach (var b in box.Boxes)
            {
                BubbleRectangles (b, line);
            }
        }
    }

    /// <summary>
    /// Applies vertical and horizontal alignment to words in lineboxes
    /// </summary>
    /// <param name="g"></param>
    /// <param name="lineBox"></param>
    private static void ApplyHorizontalAlignment (RGraphics g, CssLineBox lineBox)
    {
        switch (lineBox.OwnerBox.TextAlign)
        {
            case CssConstants.Right:
                ApplyRightAlignment (g, lineBox);
                break;
            case CssConstants.Center:
                ApplyCenterAlignment (g, lineBox);
                break;
            case CssConstants.Justify:
                ApplyJustifyAlignment (g, lineBox);
                break;
            default:
                ApplyLeftAlignment (g, lineBox);
                break;
        }
    }

    /// <summary>
    /// Applies right to left direction to words
    /// </summary>
    /// <param name="blockBox"></param>
    /// <param name="lineBox"></param>
    private static void ApplyRightToLeft (CssBox blockBox, CssLineBox lineBox)
    {
        if (blockBox.Direction == CssConstants.Rtl)
        {
            ApplyRightToLeftOnLine (lineBox);
        }
        else
        {
            foreach (var box in lineBox.RelatedBoxes)
            {
                if (box.Direction == CssConstants.Rtl)
                {
                    ApplyRightToLeftOnSingleBox (lineBox, box);
                }
            }
        }
    }

    /// <summary>
    /// Applies RTL direction to all the words on the line.
    /// </summary>
    /// <param name="line">the line to apply RTL to</param>
    private static void ApplyRightToLeftOnLine (CssLineBox line)
    {
        if (line.Words.Count > 0)
        {
            var left = line.Words[0].Left;
            var right = line.Words[line.Words.Count - 1].Right;

            foreach (var word in line.Words)
            {
                var diff = word.Left - left;
                var wright = right - diff;
                word.Left = wright - word.Width;
            }
        }
    }

    /// <summary>
    /// Applies RTL direction to specific box words on the line.
    /// </summary>
    /// <param name="lineBox"></param>
    /// <param name="box"></param>
    private static void ApplyRightToLeftOnSingleBox (CssLineBox lineBox, CssBox box)
    {
        var leftWordIdx = -1;
        var rightWordIdx = -1;
        for (var i = 0; i < lineBox.Words.Count; i++)
        {
            if (lineBox.Words[i].OwnerBox == box)
            {
                if (leftWordIdx < 0)
                    leftWordIdx = i;
                rightWordIdx = i;
            }
        }

        if (leftWordIdx > -1 && rightWordIdx > leftWordIdx)
        {
            var left = lineBox.Words[leftWordIdx].Left;
            var right = lineBox.Words[rightWordIdx].Right;

            for (var i = leftWordIdx; i <= rightWordIdx; i++)
            {
                var diff = lineBox.Words[i].Left - left;
                var wright = right - diff;
                lineBox.Words[i].Left = wright - lineBox.Words[i].Width;
            }
        }
    }

    /// <summary>
    /// Applies vertical alignment to the linebox
    /// </summary>
    /// <param name="g"></param>
    /// <param name="lineBox"></param>
    private static void ApplyVerticalAlignment (RGraphics g, CssLineBox lineBox)
    {
        double baseline = Single.MinValue;
        foreach (var box in lineBox.Rectangles.Keys)
        {
            baseline = Math.Max (baseline, lineBox.Rectangles[box].Top);
        }

        var boxes = new List<CssBox> (lineBox.Rectangles.Keys);
        foreach (var box in boxes)
        {
            //Important notes on http://www.w3.org/TR/CSS21/tables.html#height-layout
            switch (box.VerticalAlign)
            {
                case CssConstants.Sub:
                    lineBox.SetBaseLine (g, box, baseline + lineBox.Rectangles[box].Height * .5f);
                    break;
                case CssConstants.Super:
                    lineBox.SetBaseLine (g, box, baseline - lineBox.Rectangles[box].Height * .2f);
                    break;
                case CssConstants.TextTop:

                    break;
                case CssConstants.TextBottom:

                    break;
                case CssConstants.Top:

                    break;
                case CssConstants.Bottom:

                    break;
                case CssConstants.Middle:

                    break;
                default:
                    //case: baseline
                    lineBox.SetBaseLine (g, box, baseline);
                    break;
            }
        }
    }

    /// <summary>
    /// Applies centered alignment to the text on the linebox
    /// </summary>
    /// <param name="g"></param>
    /// <param name="lineBox"></param>
    private static void ApplyJustifyAlignment (RGraphics g, CssLineBox lineBox)
    {
        if (lineBox.Equals (lineBox.OwnerBox.LineBoxes[lineBox.OwnerBox.LineBoxes.Count - 1]))
            return;

        var indent = lineBox.Equals (lineBox.OwnerBox.LineBoxes[0]) ? lineBox.OwnerBox.ActualTextIndent : 0f;
        double textSum = 0f;
        double words = 0f;
        var availWidth = lineBox.OwnerBox.ClientRectangle.Width - indent;

        // Gather text sum
        foreach (var w in lineBox.Words)
        {
            textSum += w.Width;
            words += 1f;
        }

        if (words <= 0f)
            return; //Avoid Zero division
        var spacing = (availWidth - textSum) / words; //Spacing that will be used
        var curx = lineBox.OwnerBox.ClientLeft + indent;

        foreach (var word in lineBox.Words)
        {
            word.Left = curx;
            curx = word.Right + spacing;

            if (word == lineBox.Words[lineBox.Words.Count - 1])
            {
                word.Left = lineBox.OwnerBox.ClientRight - word.Width;
            }
        }
    }

    /// <summary>
    /// Applies centered alignment to the text on the linebox
    /// </summary>
    /// <param name="g"></param>
    /// <param name="line"></param>
    private static void ApplyCenterAlignment (RGraphics g, CssLineBox line)
    {
        if (line.Words.Count == 0)
            return;

        var lastWord = line.Words[line.Words.Count - 1];
        var right = line.OwnerBox.ActualRight - line.OwnerBox.ActualPaddingRight - line.OwnerBox.ActualBorderRightWidth;
        var diff = right - lastWord.Right - lastWord.OwnerBox.ActualBorderRightWidth -
                   lastWord.OwnerBox.ActualPaddingRight;
        diff /= 2;

        if (diff > 0)
        {
            foreach (var word in line.Words)
            {
                word.Left += diff;
            }

            if (line.Rectangles.Count > 0)
            {
                foreach (var b in ToList (line.Rectangles.Keys))
                {
                    var r = line.Rectangles[b];
                    line.Rectangles[b] = new RRect (r.X + diff, r.Y, r.Width, r.Height);
                }
            }
        }
    }

    /// <summary>
    /// Applies right alignment to the text on the linebox
    /// </summary>
    /// <param name="g"></param>
    /// <param name="line"></param>
    private static void ApplyRightAlignment (RGraphics g, CssLineBox line)
    {
        if (line.Words.Count == 0)
            return;


        var lastWord = line.Words[line.Words.Count - 1];
        var right = line.OwnerBox.ActualRight - line.OwnerBox.ActualPaddingRight - line.OwnerBox.ActualBorderRightWidth;
        var diff = right - lastWord.Right - lastWord.OwnerBox.ActualBorderRightWidth -
                   lastWord.OwnerBox.ActualPaddingRight;

        if (diff > 0)
        {
            foreach (var word in line.Words)
            {
                word.Left += diff;
            }

            if (line.Rectangles.Count > 0)
            {
                foreach (var b in ToList (line.Rectangles.Keys))
                {
                    var r = line.Rectangles[b];
                    line.Rectangles[b] = new RRect (r.X + diff, r.Y, r.Width, r.Height);
                }
            }
        }
    }

    /// <summary>
    /// Simplest alignment, just arrange words.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="line"></param>
    private static void ApplyLeftAlignment (RGraphics g, CssLineBox line)
    {
        //No alignment needed.

        //foreach (LineBoxRectangle r in line.Rectangles)
        //{
        //    double curx = r.Left + (r.Index == 0 ? r.OwnerBox.ActualPaddingLeft + r.OwnerBox.ActualBorderLeftWidth / 2 : 0);

        //    if (r.SpaceBefore) curx += r.OwnerBox.ActualWordSpacing;

        //    foreach (BoxWord word in r.Words)
        //    {
        //        word.Left = curx;
        //        word.Top = r.Top;// +r.OwnerBox.ActualPaddingTop + r.OwnerBox.ActualBorderTopWidth / 2;

        //        curx = word.Right + r.OwnerBox.ActualWordSpacing;
        //    }
        //}
    }

    /// <summary>
    /// todo: optimizate, not creating a list each time
    /// </summary>
    private static List<T> ToList<T> (IEnumerable<T> collection)
    {
        var result = new List<T>();
        foreach (var item in collection)
        {
            result.Add (item);
        }

        return result;
    }

    #endregion
}
