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

#endregion

#nullable enable

namespace AM.Skia.TextRendering;

/// <summary>
///
/// </summary>
public class TextPaintOptions
{
    private int? _SelectionStart;

    /// <summary>
    ///
    /// </summary>
    public int? SelectionStart
    {
        get => _SelectionStart;
        set
        {
            if (_SelectionStart == value)
            {
                return;
            }

            _SelectionStart = value;
            EnsureSafeOptionValue (ref _SelectionStart);
        }
    }

    private int? _SelectionEnd;

    /// <summary>
    ///
    /// </summary>
    public int? SelectionEnd
    {
        get => _SelectionEnd;
        set
        {
            if (_SelectionEnd == value)
            {
                return;
            }

            _SelectionEnd = value;
            EnsureSafeOptionValue (ref _SelectionEnd);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public SKColor SelectionColor;

    private int? _CursorPosition;

    /// <summary>
    ///
    /// </summary>
    public int? CursorPosition
    {
        get => _CursorPosition;
        set
        {
            if (_CursorPosition == value)
            {
                return;
            }

            _CursorPosition = value;
            EnsureSafeOptionValue (ref _CursorPosition);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public TextPaintOptions()
    {
        _SelectionStart = null;
        _SelectionEnd = null;
        SelectionColor = new SKColor (0, 120, 215); // Windows textbox selection color

        _CursorPosition = null;

        EnsureSafeOptionValuesForText();
    }

    private void EnsureSafeOptionValue (ref int? optionValue, string? text = null)
    {
        if (optionValue == null)
        {
            return;
        }

        if (optionValue < 0)
        {
            optionValue = 0;
        }
        else if (text != null && optionValue > text.Length)
        {
            optionValue = text.Length;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    public void EnsureSafeOptionValuesForText (string? text = null)
    {
        EnsureSafeOptionValue (ref _CursorPosition, text);
        EnsureSafeOptionValue (ref _SelectionStart, text);
        EnsureSafeOptionValue (ref _SelectionEnd, text);

        if (_SelectionStart != null && _SelectionEnd != null)
        {
            if (_SelectionStart > _SelectionEnd)
            {
                (_SelectionStart, _SelectionEnd) = (_SelectionEnd, _SelectionStart);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void Clear()
    {
        _SelectionStart = null;
        _SelectionEnd = null;
        _CursorPosition = null;
    }
}
