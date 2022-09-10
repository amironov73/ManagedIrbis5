// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf.Filters;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Represents a ToUnicode map for composite font.
/// </summary>
internal sealed class PdfToUnicodeMap
    : PdfDictionary
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="document"></param>
    public PdfToUnicodeMap (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    public PdfToUnicodeMap (PdfDocument document, CMapInfo cmapInfo)
        : base (document)
    {
        CMapInfo = cmapInfo;
    }

    #endregion

    /// <summary>
    /// Gets or sets the CMap info.
    /// </summary>
    public CMapInfo? CMapInfo { get; set; }

    /// <summary>
    /// Creates the ToUnicode map from the CMapInfo.
    /// </summary>
    internal override void PrepareForSave()
    {
        base.PrepareForSave();

        // This code comes literally from PDF Reference
        var prefix =
            "/CIDInit /ProcSet findresource begin\n" +
            "12 dict begin\n" +
            "begincmap\n" +
            "/CIDSystemInfo << /Registry (Adobe)/Ordering (UCS)/Supplement 0>> def\n" +
            "/CMapName /Adobe-Identity-UCS def /CMapType 2 def\n";
        var suffix = "endcmap CMapName currentdict /CMap defineresource pop end end";

        var glyphIndexToCharacter = new Dictionary<int, char>();
        int lowIndex = 65536, hiIndex = -1;
        foreach (var entry in CMapInfo.CharacterToGlyphIndex)
        {
            var index = entry.Value;
            lowIndex = Math.Min (lowIndex, index);
            hiIndex = Math.Max (hiIndex, index);

            //glyphIndexToCharacter.Add(index, entry.Key);
            glyphIndexToCharacter[index] = entry.Key;
        }

        var ms = new MemoryStream();
        var wrt = new StreamWriter (ms, Encoding.UTF8);
        wrt.Write (prefix);

        wrt.WriteLine ("1 begincodespacerange");
        wrt.WriteLine ($"<{lowIndex:X4}><{hiIndex:X4}>");
        wrt.WriteLine ("endcodespacerange");

        // Sorting seems not necessary. The limit is 100 entries, we will see.
        wrt.WriteLine ($"{glyphIndexToCharacter.Count} beginbfrange");
        foreach (var entry in glyphIndexToCharacter)
        {
            wrt.WriteLine ($"<{entry.Key:X4}><{entry.Key:X4}><{(int)entry.Value:X4}>");
        }

        wrt.WriteLine ("endbfrange");

        wrt.Write (suffix);
        wrt.Dispose();

        // Compress like content streams
        var bytes = ms.ToArray();
        ms.Dispose();
        if (Owner.Options.CompressContentStreams)
        {
            Elements.SetName ("/Filter", "/FlateDecode");
            bytes = Filtering.FlateDecode.Encode (bytes, _document.Options.FlateEncodeMode);
        }

        //PdfStream stream = CreateStream(bytes);
        else
        {
            Elements.Remove ("/Filter");
        }

        if (Stream == null)
            CreateStream (bytes);
        else
        {
            Stream.Value = bytes;
            Elements.SetInteger (PdfStream.Keys.Length, Stream.Length);
        }
    }

    public sealed class Keys
        : PdfStream.Keys
    {
        // No new keys.
    }
}
