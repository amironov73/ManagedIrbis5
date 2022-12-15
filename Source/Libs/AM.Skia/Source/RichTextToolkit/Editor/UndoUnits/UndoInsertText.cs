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

using AM.Skia.RichTextKit.Utils;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Editor.UndoUnits;

internal class UndoInsertText : UndoUnit<TextDocument>
{
    public UndoInsertText (TextBlock textBlock, int offset, StyledText text)
    {
        _textBlock = textBlock;
        _offset = offset;
        _length = text.Length;
        _text = text;
    }

    public TextBlock TextBlock => _textBlock;
    public int Offset => _offset;
    public int Length => _length;

    public bool ShouldAppend (StyledText text)
    {
        // If this is a word boundary then don't extend this unit
        return !WordBoundaryAlgorithm.IsWordBoundary (_textBlock.CodePoints.SubSlice (0, _offset + _length),
            text.CodePoints.AsSlice());
    }

    public void Append (StyledText text)
    {
        // Insert into the text block
        _textBlock.InsertText (_offset + _length, text);

        // Update length
        _length += text.Length;
    }

    public void Replace (StyledText text)
    {
        // Insert into the text block
        _textBlock.DeleteText (_offset, _length);
        _textBlock.InsertText (_offset, text);

        // Update length
        _length = text.Length;
    }

    public override void Do (TextDocument context)
    {
        // Insert the text into the text block
        _textBlock.InsertText (_offset, _text);

        // Release our copy of the text
        _text = null;
    }

    public override void Undo (TextDocument context)
    {
        // Save a copy of the text being deleted
        _text = _textBlock.Extract (_offset, _length);

        // Delete it
        _textBlock.DeleteText (_offset, _length);
    }

    private TextBlock _textBlock;
    private int _offset;
    private int _length;
    private StyledText _text;
}
