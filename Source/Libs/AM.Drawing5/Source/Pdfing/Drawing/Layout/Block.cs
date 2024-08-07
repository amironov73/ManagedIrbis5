﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* Block.cs -- одиночное слово в документе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing.Layout;

/// <summary>
/// Одиночное слово в документе.
/// </summary>
internal class Block
{
    #region Properties

    /// <summary>
    /// The text represented by this block.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// The type of the block.
    /// </summary>
    public BlockType Type { get; }

    /// <summary>
    /// The width of the text.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// The location relative to the upper left corner of the layout rectangle.
    /// </summary>
    public XPoint Location { get; set; }

    /// <summary>
    /// The alignment of this line.
    /// </summary>
    public XParagraphAlignment Alignment { get; set; }

    /// <summary>
    /// A flag indicating that this is the last block that fits in the layout rectangle.
    /// </summary>
    public bool Stop { get; set; }

    /// <summary>
    /// Contains information about spacing of the font
    /// </summary>
    public FormatterEnvironment? Environment { get; set; }

    /// <summary>
    /// Indent of the text when a new line is started
    /// </summary>
    public double LineIndent { get; set; }

    /// <summary>
    /// Skips block for alignment justify calculation, when its the first block in line
    /// </summary>
    public bool SkipParagraphAlignment { get; set; }

    /// <summary>
    ///  Links this block with the next block
    /// </summary>
    public bool NextBlockBelongsToMe { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="text">Текст блока.</param>
    /// <param name="type">Тип блока.</param>
    /// <param name="width">Ширина текста.</param>
    public Block
        (
            string text,
            BlockType type,
            double width
        )
    {
        Sure.Defined (type);

        Text = text;
        Type = type;
        Width = width;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="type">Тип блока.</param>
    public Block
        (
            BlockType type
        )
    {
        Sure.Defined (type);

        Type = type;
    }

    #endregion
}
