// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using SkiaSharp;

using System.Collections.Generic;

using AM.Skia.RichTextKit.Utils;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Editor;

/// <summary>
/// Abstract base class for all TextDocument paragraphs
/// </summary>
public abstract class Paragraph : IRun
{
    /// <summary>
    /// Constructs a new Paragraph
    /// </summary>
    protected Paragraph()
    {
    }

    /// <summary>
    /// Layout the content of this paragraph
    /// </summary>
    /// <param name="owner">The TextDocument that owns this paragraph</param>
    public abstract void Layout (TextDocument owner);

    /// <summary>
    /// Paint this paragraph
    /// </summary>
    /// <param name="canvas">The canvas to paint to</param>
    /// <param name="options">Paint options</param>
    public abstract void Paint (SKCanvas canvas, TextPaintOptions options);

    /// <summary>
    /// Get caret position information
    /// </summary>
    /// <remarks>
    /// The returned caret info should be relative to the paragraph's content
    /// </remarks>
    /// <param name="position">The caret position</param>
    /// <returns>A CaretInfo struct, or CaretInfo.None</returns>
    public abstract CaretInfo GetCaretInfo (CaretPosition position);

    /// <summary>
    /// Hit test this paragraph
    /// </summary>
    /// <param name="x">The x-coordinate relative to top left of the paragraph content</param>
    /// <param name="y">The x-coordinate relative to top left of the paragraph content</param>
    /// <returns>A HitTestResult</returns>
    public abstract HitTestResult HitTest (float x, float y);

    /// <summary>
    /// Hit test a line in this paragraph
    /// </summary>
    /// <remarks>
    /// The number of lines can be determined from LineIndicies.Count.
    /// </remarks>
    /// <param name="lineIndex">The line number to be tested</param>
    /// <param name="x">The x-coordinate relative to left of the paragraph content</param>
    /// <returns>A HitTestResult</returns>
    public abstract HitTestResult HitTestLine (int lineIndex, float x);

    /// <summary>
    /// Retrieves a list of all valid caret positions
    /// </summary>
    public abstract IReadOnlyList<int> CaretIndicies { get; }

    /// <summary>
    /// Retrieves a list of all valid word boundary caret positions
    /// </summary>
    public abstract IReadOnlyList<int> WordBoundaryIndicies { get; }

    /// <summary>
    /// Retrieves a list of code point indicies of the start of each line
    /// </summary>
    public abstract IReadOnlyList<int> LineIndicies { get; }

    /// <summary>
    /// Gets the length of this paragraph in code points
    /// </summary>
    /// <remarks>
    /// All paragraphs must have a non-zero length and text paragraphs
    /// should include the end of paragraph marker in the length.
    /// </remarks>
    public abstract int Length { get; }

    /// <summary>
    /// Qureries the height of this paragraph, excluding margins
    /// </summary>
    public abstract float ContentHeight { get; }

    /// <summary>
    /// Queries the width of this paragraph, excluding margins
    /// </summary>
    public abstract float ContentWidth { get; }

    /// <summary>
    /// Gets the TextBlock associated with this paragraph
    /// </summary>
    /// <remarks>
    /// Non-text paragraphs should return null
    /// </remarks>
    public virtual TextBlock TextBlock
    {
        get => null;
    }

    /// <summary>
    /// Copy all style attributes from this paragraph to another
    /// </summary>
    /// <param name="other">The paragraph to copy style from</param>
    public virtual void CopyStyleFrom (Paragraph other)
    {
        this.MarginLeft = other.MarginLeft;
        this.MarginTop = other.MarginTop;
        this.MarginRight = other.MarginRight;
        this.MarginBottom = other.MarginBottom;
    }

    /// <summary>
    /// The X-coordinate of this paragraph's content (ie: after applying margin)
    /// </summary>
    /// <remarks>
    /// This property is calculated and assigned by the TextDocument
    /// </remarks>
    public float ContentXCoord { get; internal set; }

    /// <summary>
    /// The Y-coordinate of this paragraph's content (ie: after applying margin)
    /// </summary>
    /// <remarks>
    /// This property is calculated and assigned by the TextDocument
    /// </remarks>
    public float ContentYCoord { get; internal set; }

    /// <summary>
    /// This code point index of this paragraph relative to the start
    /// of the document.
    /// </summary>
    /// <remarks>
    /// This property is calculated and assigned by the TextDocument
    /// </remarks>
    public int CodePointIndex { get; internal set; }

    /// <summary>
    /// The left margin
    /// </summary>
    public float MarginLeft { get; internal set; }

    /// <summary>
    /// The right margin
    /// </summary>
    public float MarginRight { get; internal set; }

    /// <summary>
    /// The top margin
    /// </summary>
    public float MarginTop { get; internal set; }

    /// <summary>
    /// The bottom margin
    /// </summary>
    public float MarginBottom { get; internal set; }

    // Explicit implementation of IRun so we can use RunExtensions
    // with the paragraphs collection.
    int IRun.Offset => CodePointIndex;
    int IRun.Length => Length;
}
