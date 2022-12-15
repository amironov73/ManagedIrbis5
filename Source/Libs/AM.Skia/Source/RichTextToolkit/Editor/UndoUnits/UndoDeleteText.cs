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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using AM.Skia.RichTextKit.Utils;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Editor.UndoUnits;

internal class UndoDeleteText : UndoUnit<TextDocument>
{
    public UndoDeleteText (TextBlock textBlock, int offset, int length)
    {
        _textBlock = textBlock;
        _offset = offset;
        _length = length;
    }

    public override void Do (TextDocument context)
    {
        _savedText = _textBlock.Extract (_offset, _length);
        _textBlock.DeleteText (_offset, _length);
    }

    public override void Undo (TextDocument context)
    {
        _textBlock.InsertText (_offset, _savedText);
        _savedText = null;
    }

    public bool ExtendBackspace (int length)
    {
        // Don't extend across paragraph boundaries
        if (_offset - length < 0)
        {
            return false;
        }

        // Copy the additional text
        var temp = _textBlock.Extract (_offset - length, length);
        _savedText.InsertText (0, temp);
        _textBlock.DeleteText (_offset - length, length);

        // Update position
        _offset -= length;
        _length += length;

        return true;
    }

    public bool ExtendForwardDelete (int length)
    {
        // Don't extend across paragraph boundaries
        if (_offset + length > _textBlock.Length - 1)
        {
            return false;
        }

        // Copy the additional text
        var temp = _textBlock.Extract (_offset, length);
        _savedText.InsertText (_length, temp);
        _textBlock.DeleteText (_offset, length);

        // Update position
        _length += length;

        return true;
    }

    public bool ExtendOvertype (int offset, int length)
    {
        // Don't extend across paragraph boundaries
        if (_offset + offset + length > _textBlock.Length - 1)
        {
            return false;
        }

        // This can happen when a DeleteText unit is retroactively
        // constructed when typing in overtype mode at the end of a
        // paragraph
        if (_savedText == null)
        {
            _savedText = new StyledText();
        }

        // Copy the additional text
        var temp = _textBlock.Extract (_offset + offset, length);
        _savedText.InsertText (_length, temp);
        _textBlock.DeleteText (_offset + offset, length);

        // Update position
        _length += length;

        return true;
    }

    private TextBlock _textBlock;
    private int _offset;
    private int _length;
    private StyledText _savedText;
}
