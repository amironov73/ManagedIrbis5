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

using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Filters;

/// <summary>
/// Reserved for future extension.
/// </summary>
public class FilterParms
{
    /// <summary>
    /// Gets the decoding-parameters for a filter. May be null
    /// </summary>
    public PdfDictionary DecodeParms { get; private set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="decodeParms"></param>
    public FilterParms
        (
            PdfDictionary decodeParms
        )
    {
        DecodeParms = decodeParms;
    }
}

/// <summary>
/// Base class for all stream filters
/// </summary>
public abstract class Filter
{
    /// <summary>
    /// When implemented in a derived class encodes the specified data.
    /// </summary>
    public abstract byte[] Encode
        (
            byte[] data
        );

    /// <summary>
    /// Encodes a raw string.
    /// </summary>
    public virtual byte[] Encode
        (
            string rawString
        )
    {
        var bytes = PdfEncoders.RawEncoding.GetBytes (rawString);
        bytes = Encode (bytes);

        return bytes;
    }

    /// <summary>
    /// When implemented in a derived class decodes the specified data.
    /// </summary>
    public abstract byte[] Decode
        (
            byte[] data,
            FilterParms? parameters
        );

    /// <summary>
    /// Decodes the specified data.
    /// </summary>
    public byte[] Decode
        (
            byte[] data,
            PdfDictionary decodeParms
        )
    {
        return Decode (data, new FilterParms (decodeParms));
    }

    /// <summary>
    /// Decodes to a raw string.
    /// </summary>
    public virtual string DecodeToString
        (
            byte[] data,
            FilterParms? parms
        )
    {
        var bytes = Decode (data, parms);
        var text = PdfEncoders.RawEncoding.GetString (bytes, 0, bytes.Length);

        return text;
    }

    /// <summary>
    /// Decodes to a raw string.
    /// </summary>
    public string DecodeToString
        (
            byte[] data
        )
    {
        return DecodeToString (data, null);
    }

    /// <summary>
    /// Removes all white spaces from the data. The function assumes that the bytes are characters.
    /// </summary>
    protected byte[] RemoveWhiteSpace
        (
            byte[] data
        )
    {
        var count = data.Length;
        var j = 0;
        for (var i = 0; i < count; i++, j++)
        {
            switch (data[i])
            {
                case (byte)Chars.NUL: // 0 Null
                case (byte)Chars.HT: // 9 Tab
                case (byte)Chars.LF: // 10 Line feed
                case (byte)Chars.FF: // 12 Form feed
                case (byte)Chars.CR: // 13 Carriage return
                case (byte)Chars.SP: // 32 Space
                    j--;
                    break;

                default:
                    if (i != j)
                        data[j] = data[i];
                    break;
            }
        }

        if (j < count)
        {
            var temp = data;
            data = new byte[j];
            for (var idx = 0; idx < j; idx++)
            {
                data[idx] = temp[idx];
            }
        }

        return data;
    }
}
