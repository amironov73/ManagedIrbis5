// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Filtering.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Filters;

/// <summary>
/// Applies standard filters to streams.
/// </summary>
public static class Filtering
{
    /// <summary>
    /// Gets the filter specified by the case sensitive name.
    /// </summary>
    public static Filter? GetFilter (string filterName)
    {
        if (filterName.StartsWith ("/"))
        {
            filterName = filterName.Substring (1);
        }

        // Some tools use abbreviations
        switch (filterName)
        {
            case "ASCIIHexDecode":
            case "AHx":
                return _asciiHexDecode ??= new AsciiHexDecode();

            case "ASCII85Decode":
            case "A85":
                return _ascii85Decode ??= new Ascii85Decode();

            case "LZWDecode":
            case "LZW":
                return _lzwDecode ??= new LzwDecode();

            case "FlateDecode":
            case "Fl":
                return _flateDecode ??= new FlateDecode();

            //case "RunLengthDecode":
            //  if (RunLengthDecode == null)
            //    RunLengthDecode = new RunLengthDecode();
            //  return RunLengthDecode;
            //
            //case "CCITTFaxDecode":
            //  if (CCITTFaxDecode == null)
            //    CCITTFaxDecode = new CCITTFaxDecode();
            //  return CCITTFaxDecode;
            //
            //case "JBIG2Decode":
            //  if (JBIG2Decode == null)
            //    JBIG2Decode = new JBIG2Decode();
            //  return JBIG2Decode;
            //
            //case "DCTDecode":
            //  if (DCTDecode == null)
            //    DCTDecode = new DCTDecode();
            //  return DCTDecode;
            //
            //case "JPXDecode":
            //  if (JPXDecode == null)
            //    JPXDecode = new JPXDecode();
            //  return JPXDecode;
            //
            //case "Crypt":
            //  if (Crypt == null)
            //    Crypt = new Crypt();
            //  return Crypt;

            case "RunLengthDecode":
            case "CCITTFaxDecode":
            case "JBIG2Decode":
            case "DCTDecode":
            case "JPXDecode":
            case "Crypt":
                Debug.WriteLine ("Filter not implemented: " + filterName);
                return null;
        }

        throw new NotImplementedException ("Unknown filter: " + filterName);
    }

    /// <summary>
    /// Gets the filter singleton.
    /// </summary>

    public static AsciiHexDecode ASCIIHexDecode
    {
        get { return _asciiHexDecode ??= new AsciiHexDecode(); }
    }

    private static AsciiHexDecode? _asciiHexDecode;

    /// <summary>
    /// Gets the filter singleton.
    /// </summary>
    public static Ascii85Decode ASCII85Decode
    {
        get { return _ascii85Decode ??= new Ascii85Decode(); }
    }

    private static Ascii85Decode? _ascii85Decode;

    /// <summary>
    /// Gets the filter singleton.
    /// </summary>
    public static LzwDecode LzwDecode
    {
        get { return _lzwDecode ??= new LzwDecode(); }
    }

    private static LzwDecode? _lzwDecode;

    /// <summary>
    /// Gets the filter singleton.
    /// </summary>
    public static FlateDecode FlateDecode
    {
        get { return _flateDecode ??= new FlateDecode(); }
    }

    private static FlateDecode? _flateDecode;

    //runLengthDecode
    //ccittFaxDecode
    //jbig2Decode
    //dctDecode
    //jpxDecode
    //crypt

    /// <summary>
    /// Encodes the data with the specified filter.
    /// </summary>
    public static byte[]? Encode (byte[] data, string filterName)
    {
        var filter = GetFilter (filterName);
        if (filter != null)
        {
            return filter.Encode (data);
        }

        return null;
    }

    /// <summary>
    /// Encodes a raw string with the specified filter.
    /// </summary>
    public static byte[]? Encode (string rawString, string filterName)
    {
        var filter = GetFilter (filterName);
        if (filter != null)
        {
            return filter.Encode (rawString);
        }

        return null;
    }

    /// <summary>
    /// Decodes the data with the specified filter.
    /// </summary>
    public static byte[]? Decode (byte[] data, string filterName, FilterParms parms)
    {
        var filter = GetFilter (filterName);
        if (filter != null)
        {
            return filter.Decode (data, parms);
        }

        return null;
    }

    /// <summary>
    /// Decodes the data with the specified filter.
    /// </summary>
    public static byte[]? Decode (byte[] data, string filterName)
    {
        var filter = GetFilter (filterName);
        if (filter != null)
        {
            return filter.Decode (data, (PdfDictionary?) null);
        }

        return null;
    }

    /// <summary>
    /// Decodes the data with the specified filter.
    /// </summary>
    public static byte[]? Decode (byte[] data, PdfItem filterItem, PdfItem? decodeParms)
    {
        byte[]? result = null;
        if (filterItem is PdfName && (decodeParms == null || decodeParms is PdfDictionary))
        {
            var filter = GetFilter (filterItem.ToString());
            if (filter != null)
            {
                result = filter.Decode (data, decodeParms as PdfDictionary);
            }
        }
        else if (filterItem is PdfArray itemArray && (decodeParms == null || decodeParms is PdfArray))
        {
            var decodeArray = decodeParms as PdfArray;

            // array length of filter and decode parms should match. if they dont, return data unmodified
            if (decodeArray != null && decodeArray.Elements.Count != itemArray.Elements.Count)
            {
                return data;
            }

            for (var i = 0; i < itemArray.Elements.Count; i++)
            {
                var item = itemArray.Elements[i];
                var parms = decodeArray?.Elements[i];
                data = Decode (data, item, parms);
            }

            result = data;
        }

        return result;
    }

    /// <summary>
    /// Decodes to a raw string with the specified filter.
    /// </summary>
    public static string? DecodeToString (byte[] data, string filterName, FilterParms parms)
    {
        var filter = GetFilter (filterName);
        if (filter != null)
        {
            return filter.DecodeToString (data, parms);
        }

        return null;
    }

    /// <summary>
    /// Decodes to a raw string with the specified filter.
    /// </summary>
    public static string? DecodeToString
        (
            byte[] data,
            string filterName
        )
    {
        var filter = GetFilter (filterName);
        if (filter != null)
        {
            return filter.DecodeToString (data, null);
        }

        return null;
    }
}
