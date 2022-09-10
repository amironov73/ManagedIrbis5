// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FlateDecode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Filters;

/// <summary>
/// Implements the FlateDecode filter by wrapping SharpZipLib.
/// </summary>
public class FlateDecode
    : Filter
{
    // Reference: 3.3.3  LZWDecode and FlateDecode Filters / Page 71

    /// <summary>
    /// Encodes the specified data.
    /// </summary>
    public override byte[] Encode
        (
            byte[] data
        )
    {
        return Encode (data, PdfFlateEncodeMode.Default);
    }

    /// <summary>
    /// Encodes the specified data.
    /// </summary>
    public byte[] Encode
        (
            byte[] data,
            PdfFlateEncodeMode mode
        )
    {
        var ms = new MemoryStream();

        // DeflateStream/GZipStream does not work immediately and I have not the leisure to work it out.
        // So I keep on using SharpZipLib even with .NET 2.0.

        var level = Deflater.DEFAULT_COMPRESSION;
        switch (mode)
        {
            case PdfFlateEncodeMode.BestCompression:
                level = Deflater.BEST_COMPRESSION;
                break;

            case PdfFlateEncodeMode.BestSpeed:
                level = Deflater.BEST_SPEED;
                break;
        }

        var zip = new DeflaterOutputStream (ms, new Deflater (level, false));
        zip.Write (data, 0, data.Length);
        zip.Finish();
        return ms.ToArray();
    }

    /// <summary>
    /// Decodes the specified data.
    /// </summary>
    public override byte[] Decode
        (
            byte[] data,
            FilterParms? parameters
        )
    {
        var msInput = new MemoryStream (data);
        var msOutput = new MemoryStream();

        var iis = new InflaterInputStream (msInput, new Inflater (false));
        int cbRead;
        var abResult = new byte[32768];
        do
        {
            cbRead = iis.Read (abResult, 0, abResult.Length);
            if (cbRead > 0)
            {
                msOutput.Write (abResult, 0, cbRead);
            }
        } while (cbRead > 0);

        iis.Close();
        msOutput.Flush();
        if (msOutput.Length >= 0)
        {
            if (parameters.DecodeParms != null)
            {
                return StreamDecoder.Decode (msOutput.ToArray(), parameters.DecodeParms);
            }

            return msOutput.ToArray();
        }

        return null;
    }
}
