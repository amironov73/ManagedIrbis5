// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* IndexToLocationTable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

#define VERBOSE_

using System;
using System.Diagnostics;

using AM;

#endregion

#nullable enable

namespace PdfSharpCore.Fonts.OpenType;

/// <summary>
/// The indexToLoc table stores the offsets to the locations of the glyphs in the font,
/// relative to the beginning of the glyphData table. In order to compute the length of
/// the last glyph element, there is an extra entry after the last valid index.
/// </summary>
internal class IndexToLocationTable
    : OpenTypeFontTable
{
    public const string Tag = TableTagNames.Loca;

    internal int[] LocaTable;

    /// <summary>
    ///
    /// </summary>
    public IndexToLocationTable()
        : base (null, Tag)
    {
        LocaTable = null!;
        _bytes = null!;

        DirectoryEntry.Tag = TableTagNames.Loca;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="fontData"></param>
    public IndexToLocationTable
        (
            OpenTypeFontface fontData
        )
        : base (fontData, Tag)
    {
        LocaTable = null!;
        _bytes = null!;

        DirectoryEntry = FontData!.TableDictionary[TableTagNames.Loca];
        Read();
    }

    public bool ShortIndex;

    /// <summary>
    /// Converts the bytes in a handy representation
    /// </summary>
    public void Read()
    {
        try
        {
            ShortIndex = FontData!._head!.indexToLocFormat == 0;
            FontData.Position = DirectoryEntry.Offset;
            if (ShortIndex)
            {
                var entries = DirectoryEntry.Length / 2;
                Debug.Assert (FontData.maxp!.numGlyphs + 1 == entries,
                    "For your information only: Number of glyphs mismatch in font. You can ignore this assertion.");
                LocaTable = new int[entries];
                for (var idx = 0; idx < entries; idx++)
                {
                    LocaTable[idx] = 2 * FontData.ReadUFWord();
                }
            }
            else
            {
                var entries = DirectoryEntry.Length / 4;
                Debug.Assert (FontData.maxp!.numGlyphs + 1 == entries,
                    "For your information only: Number of glyphs mismatch in font. You can ignore this assertion.");
                LocaTable = new int[entries];
                for (var idx = 0; idx < entries; idx++)
                {
                    LocaTable[idx] = FontData.ReadLong();
                }
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
            GetType().NotUsed();
            throw;
        }
    }

    /// <summary>
    /// Prepares the font table to be compiled into its binary representation.
    /// </summary>
    public override void PrepareForCompilation()
    {
        DirectoryEntry.Offset = 0;
        if (ShortIndex)
        {
            DirectoryEntry.Length = LocaTable.Length * 2;
        }
        else
        {
            DirectoryEntry.Length = LocaTable.Length * 4;
        }

        _bytes = new byte[DirectoryEntry.PaddedLength];
        var length = LocaTable.Length;
        var byteIdx = 0;
        if (ShortIndex)
        {
            for (var idx = 0; idx < length; idx++)
            {
                var value = LocaTable[idx] / 2;
                _bytes[byteIdx++] = (byte)(value >> 8);
                _bytes[byteIdx++] = (byte)(value);
            }
        }
        else
        {
            for (var idx = 0; idx < length; idx++)
            {
                var value = LocaTable[idx];
                _bytes[byteIdx++] = (byte)(value >> 24);
                _bytes[byteIdx++] = (byte)(value >> 16);
                _bytes[byteIdx++] = (byte)(value >> 8);
                _bytes[byteIdx++] = (byte)value;
            }
        }

        DirectoryEntry.CheckSum = CalcChecksum (_bytes);
    }

    byte[] _bytes;

    /// <summary>
    /// Converts the font into its binary representation.
    /// </summary>
    public override void Write
        (
            OpenTypeFontWriter writer
        )
    {
        writer.Write (_bytes, 0, DirectoryEntry.PaddedLength);
    }
}
