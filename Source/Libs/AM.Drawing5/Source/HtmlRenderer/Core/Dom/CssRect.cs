// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* CssRect.cs -- слово внутри inline-блока
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing.HtmlRenderer.Adapters;
using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Core.Handlers;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Dom;

/// <summary>
/// Представляет слово внутри inline-блока.
/// </summary>
/// <remarks>
/// Because of performance, words of text are the most atomic
/// element in the project. It should be characters, but come on,
/// imagine the performance when drawing char by char on the device.<br/>
/// It may change for future versions of the library.
/// </remarks>
internal abstract class CssRect
{
    #region Fields and Consts

    /// <summary>
    /// Rectangle
    /// </summary>
    private RRect _rectangle;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="owner">the CSS box owner of the word</param>
    protected CssRect
        (
            CssBox? owner
        )
    {
        OwnerBox = owner;
    }

    #endregion

    /// <summary>
    /// Gets the Box where this word belongs.
    /// </summary>
    public CssBox? OwnerBox { get; }

    /// <summary>
    /// Gets or sets the bounds of the rectangle
    /// </summary>
    public RRect Rectangle
    {
        // нельзя заменять на auto-property!
        get => _rectangle;
        set => _rectangle = value;
    }

    /// <summary>
    /// Left of the rectangle
    /// </summary>
    public double Left
    {
        get => _rectangle.X;
        set => _rectangle.X = value;
    }

    /// <summary>
    /// Top of the rectangle
    /// </summary>
    public double Top
    {
        get => _rectangle.Y;
        set => _rectangle.Y = value;
    }

    /// <summary>
    /// Width of the rectangle
    /// </summary>
    public double Width
    {
        get => _rectangle.Width;
        set => _rectangle.Width = value;
    }

    /// <summary>
    /// Get the full width of the word including the spacing.
    /// </summary>
    public double FullWidth => Rectangle.Width + ActualWordSpacing;

    /// <summary>
    /// Gets the actual width of whitespace between words.
    /// </summary>
    public double ActualWordSpacing =>
        OwnerBox is not null
            ? (HasSpaceAfter ? OwnerBox.ActualWordSpacing : 0) + (IsImage ? OwnerBox.ActualWordSpacing : 0)
            : 0;

    /// <summary>
    /// Height of the rectangle
    /// </summary>
    public double Height
    {
        get => _rectangle.Height;
        set => _rectangle.Height = value;
    }

    /// <summary>
    /// Gets or sets the right of the rectangle. When setting, it only affects the Width of the rectangle.
    /// </summary>
    public double Right
    {
        get => _rectangle.Right;
        set => Width = value - Left;
    }

    /// <summary>
    /// Gets or sets the bottom of the rectangle. When setting, it only affects the Height of the rectangle.
    /// </summary>
    public double Bottom
    {
        get => _rectangle.Bottom;
        set => Height = value - Top;
    }

    /// <summary>
    /// If the word is selected this points to the selection handler for more data
    /// </summary>
    public SelectionHandler? Selection { get; set; }

    /// <summary>
    /// was there a whitespace before the word chars (before trim)
    /// </summary>
    public virtual bool HasSpaceBefore => false;

    /// <summary>
    /// was there a whitespace after the word chars (before trim)
    /// </summary>
    public virtual bool HasSpaceAfter => false;

    /// <summary>
    /// Gets the image this words represents (if one exists)
    /// </summary>
    public virtual RImage? Image
    {
        get => null;

        // ReSharper disable ValueParameterNotUsed
        set { }

        // ReSharper restore ValueParameterNotUsed
    }

    /// <summary>
    /// Gets if the word represents an image.
    /// </summary>
    public virtual bool IsImage => false;

    /// <summary>
    /// Gets a bool indicating if this word is composed only by spaces.
    /// Spaces include tabs and line breaks
    /// </summary>
    public virtual bool IsSpaces => true;

    /// <summary>
    /// Gets if the word is composed by only a line break
    /// </summary>
    public virtual bool IsLineBreak => false;

    /// <summary>
    /// Gets the text of the word
    /// </summary>
    public virtual string? Text => null;

    /// <summary>
    /// is the word is currently selected
    /// </summary>
    public bool Selected => Selection != null;

    /// <summary>
    /// the selection start index if the word is partially selected (-1 if not selected or fully selected)
    /// </summary>
    public int SelectedStartIndex => Selection?.GetSelectingStartIndex (this) ?? -1;

    /// <summary>
    /// the selection end index if the word is partially selected (-1 if not selected or fully selected)
    /// </summary>
    public int SelectedEndIndexOffset => Selection?.GetSelectedEndIndexOffset (this) ?? -1;

    /// <summary>
    /// the selection start offset if the word is partially selected (-1 if not selected or fully selected)
    /// </summary>
    public double SelectedStartOffset => Selection?.GetSelectedStartOffset (this) ?? -1;

    /// <summary>
    /// the selection end offset if the word is partially selected (-1 if not selected or fully selected)
    /// </summary>
    public double SelectedEndOffset => Selection?.GetSelectedEndOffset (this) ?? -1;

    /// <summary>
    /// Gets or sets an offset to be considered in measurements
    /// </summary>
    internal double LeftGlyphPadding => OwnerBox != null ? OwnerBox.ActualFont.LeftPadding : 0;

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var text = Text is null
            ? string.Empty
            : Text.Replace (' ', '-').Replace ("\n", "\\n");

        return $"{text} ({text.Length} char{(text.Length != 1 ? "s" : string.Empty)})";
    }

    public bool BreakPage()
    {
        var container = OwnerBox?.HtmlContainer;
        if (container is null)
        {
            return false;
        }

        if (Height >= container.PageSize.Height)
        {
            return false;
        }

        var remTop = (Top - container.MarginTop) % container.PageSize.Height;
        var remBottom = (Bottom - container.MarginTop) % container.PageSize.Height;

        if (remTop > remBottom)
        {
            Top += container.PageSize.Height - remTop + 1;
            return true;
        }

        return false;
    }
}
