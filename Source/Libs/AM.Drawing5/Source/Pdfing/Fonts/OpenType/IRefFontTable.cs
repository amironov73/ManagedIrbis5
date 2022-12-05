// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* IRefFontTable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using Fixed = System.Int32;
using FWord = System.Int16;
using UFWord = System.UInt16;

#endregion

#nullable enable

namespace PdfSharpCore.Fonts.OpenType;

/// <summary>
/// Represents an indirect reference to an existing font table in a font image.
/// Used to create binary copies of an existing font table that is not modified.
/// </summary>

// ReSharper disable once InconsistentNaming - "I" stands for "indirect", not "interface".
internal class IRefFontTable
    : OpenTypeFontTable
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="fontData"></param>
    /// <param name="fontTable"></param>
    public IRefFontTable
        (
            OpenTypeFontface fontData,
            OpenTypeFontTable fontTable
        )
        : base(null, fontTable.DirectoryEntry.Tag!)
    {
        FontData = fontData;
        _irefDirectoryEntry = fontTable.DirectoryEntry;
    }

    readonly TableDirectoryEntry _irefDirectoryEntry;

    /// <summary>
    /// Prepares the font table to be compiled into its binary representation.
    /// </summary>
    public override void PrepareForCompilation()
    {
        base.PrepareForCompilation();
        DirectoryEntry.Length = _irefDirectoryEntry.Length;
        DirectoryEntry.CheckSum = _irefDirectoryEntry.CheckSum;
#if DEBUG
        // Check the checksum algorithm
        if (DirectoryEntry.Tag != TableTagNames.Head)
        {
            byte[] bytes = new byte[DirectoryEntry.PaddedLength];
            Buffer.BlockCopy(_irefDirectoryEntry.FontTable!.FontData!.FontSource!.Bytes, _irefDirectoryEntry.Offset, bytes, 0, DirectoryEntry.PaddedLength);
            uint checkSum1 = DirectoryEntry.CheckSum;
            checkSum1.NotUsed();
            uint checkSum2 = CalcChecksum(bytes);
            checkSum2.NotUsed();
            // TODO: Sometimes this Assert fails,
            //Debug.Assert(checkSum1 == checkSum2, "Bug in checksum algorithm.");
        }
#endif
    }

    /// <summary>
    /// Converts the font into its binary representation.
    /// </summary>
    public override void Write(OpenTypeFontWriter writer)
    {
        writer.Write(_irefDirectoryEntry.FontTable!.FontData!.FontSource!.Bytes, _irefDirectoryEntry.Offset, _irefDirectoryEntry.PaddedLength);
    }
}
