// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* TextParagraph.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using SkiaSharp;

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Editor;

/// <summary>
/// Implements a text paragraph
/// </summary>
internal class TextParagraph : Paragraph
{
    /// <summary>
    /// Constructs a new TextParagraph
    /// </summary>
    public TextParagraph (IStyle style)
    {
        _textBlock = new TextBlock();
        _textBlock.AddText ("\u2029", style);
    }

    // Create a new textblock by copying the content of another
    public TextParagraph (TextParagraph source, int from, int length)
    {
        // Copy the text block
        _textBlock = source.TextBlock.Copy (from, length);

        // Copy styles
        CopyStyleFrom (source);
    }

    /// <inheritdoc />
    public override void Layout (TextDocument owner)
    {
        _textBlock.RenderWidth =
            owner.PageWidth
            - owner.MarginLeft - owner.MarginRight
            - MarginLeft - MarginRight;

        // For layout just need to set the appropriate layout width on the text block
        if (owner.LineWrap)
        {
            _textBlock.MaxWidth = _textBlock.RenderWidth;
        }
        else
        {
            _textBlock.MaxWidth = null;
        }
    }

    /// <inheritdoc />
    public override void Paint (SKCanvas canvas, TextPaintOptions? options) =>
        _textBlock.Paint (canvas, new SKPoint (ContentXCoord, ContentYCoord), options);

    /// <inheritdoc />
    public override CaretInfo GetCaretInfo (CaretPosition position) => _textBlock.GetCaretInfo (position);

    /// <inheritdoc />
    public override HitTestResult HitTest (float x, float y) => _textBlock.HitTest (x, y);

    /// <inheritdoc />
    public override HitTestResult HitTestLine (int lineIndex, float x) => _textBlock.HitTestLine (lineIndex, x);

    /// <inheritdoc />
    public override IReadOnlyList<int> CaretIndicies => _textBlock.CaretIndicies;

    /// <inheritdoc />
    public override IReadOnlyList<int> WordBoundaryIndicies => _textBlock.WordBoundaryIndicies;

    /// <inheritdoc />
    public override IReadOnlyList<int> LineIndicies => _textBlock.LineIndicies;

    /// <inheritdoc />
    public override int Length => _textBlock.Length;

    /// <inheritdoc />
    public override float ContentWidth => _textBlock.MeasuredWidth;

    /// <inheritdoc />
    public override float ContentHeight => _textBlock.MeasuredHeight;

    /// <inheritdoc />
    public override TextBlock TextBlock => _textBlock;

    /// <inheritdoc />
    public override void CopyStyleFrom (Paragraph other)
    {
        base.CopyStyleFrom (other);
        _textBlock.Alignment = other.TextBlock!.Alignment;
        _textBlock.BaseDirection = other.TextBlock.BaseDirection;
    }

    // Private attributes
    private TextBlock _textBlock;
}
