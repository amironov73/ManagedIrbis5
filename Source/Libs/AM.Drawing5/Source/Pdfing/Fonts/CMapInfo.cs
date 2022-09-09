// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Collections.Generic;

using PdfSharpCore.Fonts.OpenType;
using PdfSharpCore.Pdf.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Fonts;

/// <summary>
/// Helper class that determines the characters used in a particular font.
/// </summary>
internal class CMapInfo
{
    public CMapInfo (OpenTypeDescriptor descriptor)
    {
        Debug.Assert (descriptor != null);
        _descriptor = descriptor;
    }

    internal OpenTypeDescriptor _descriptor;

    /// <summary>
    /// Adds the characters of the specified string to the hashtable.
    /// </summary>
    public void AddChars (string? text)
    {
        if (text != null)
        {
            var symbol = _descriptor.FontFace._cmap.symbol;
            var length = text.Length;
            for (var idx = 0; idx < length; idx++)
            {
                var ch = text[idx];
                if (!CharacterToGlyphIndex.ContainsKey (ch))
                {
                    var ch2 = ch;
                    if (symbol)
                    {
                        // Remap ch for symbol fonts.
                        ch2 = (char)(ch | (_descriptor.FontFace._os2.usFirstCharIndex & 0xFF00)); // @@@ refactor
                    }

                    var glyphIndex = _descriptor.CharCodeToGlyphIndex (ch2);
                    CharacterToGlyphIndex.Add (ch, glyphIndex);
                    GlyphIndices[glyphIndex] = null;
                    MinChar = (char)Math.Min (MinChar, ch);
                    MaxChar = (char)Math.Max (MaxChar, ch);
                }
            }
        }
    }

    /// <summary>
    /// Adds the glyphIndices to the hashtable.
    /// </summary>
    public void AddGlyphIndices (string? glyphIndices)
    {
        if (glyphIndices != null)
        {
            var length = glyphIndices.Length;
            for (var idx = 0; idx < length; idx++)
            {
                int glyphIndex = glyphIndices[idx];
                GlyphIndices[glyphIndex] = null;
            }
        }
    }

    /// <summary>
    /// Adds a ANSI characters.
    /// </summary>
    internal void AddAnsiChars()
    {
        var ansi = new byte[256 - 32];
        for (var idx = 0; idx < 256 - 32; idx++)
        {
            ansi[idx] = (byte)(idx + 32);
        }

        var text = PdfEncoders.WinAnsiEncoding.GetString (ansi, 0, ansi.Length);
        AddChars (text);
    }

    internal bool Contains (char ch)
    {
        return CharacterToGlyphIndex.ContainsKey (ch);
    }

    public char[] Chars
    {
        get
        {
            var chars = new char[CharacterToGlyphIndex.Count];
            CharacterToGlyphIndex.Keys.CopyTo (chars, 0);
            Array.Sort (chars);
            return chars;
        }
    }

    public int[] GetGlyphIndices()
    {
        var indices = new int[GlyphIndices.Count];
        GlyphIndices.Keys.CopyTo (indices, 0);
        Array.Sort (indices);
        return indices;
    }

    public char MinChar = char.MaxValue;
    public char MaxChar = char.MinValue;
    public Dictionary<char, int> CharacterToGlyphIndex = new Dictionary<char, int>();
    public Dictionary<int, object> GlyphIndices = new Dictionary<int, object>();
}
