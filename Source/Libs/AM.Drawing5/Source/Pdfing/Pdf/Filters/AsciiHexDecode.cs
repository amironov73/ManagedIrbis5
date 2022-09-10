// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* AsciiHexDecode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Filters;

/// <summary>
/// Implements the ASCIIHexDecode filter.
/// </summary>
public class AsciiHexDecode
    : Filter
{
    // Reference: 3.3.1  ASCIIHexDecode Filter / Page 69

    /// <summary>
    /// Encodes the specified data.
    /// </summary>
    public override byte[] Encode
        (
            byte[] data
        )
    {
        Sure.NotNull (data);

        var count = data.Length;
        var bytes = new byte[2 * count];
        for (int i = 0, j = 0; i < count; i++)
        {
            var b = data[i];
            bytes[j++] = (byte)((b >> 4) + ((b >> 4) < 10 ? (byte)'0' : (byte)('A' - 10)));
            bytes[j++] = (byte)((b & 0xF) + ((b & 0xF) < 10 ? (byte)'0' : (byte)('A' - 10)));
        }

        return bytes;
    }

    /// <summary>
    /// Decodes the specified data.
    /// </summary>
    public override byte[] Decode
        (
            byte[] data,
            FilterParms parameters
        )
    {
        Sure.NotNull (data);

        data = RemoveWhiteSpace (data);
        var count = data.Length;

        // Ignore EOD (end of data) character.
        // EOD can be anywhere in the stream, but makes sense only at the end of the stream.
        if (count > 0 && data[count - 1] == '>')
        {
            --count;
        }

        if (count % 2 == 1)
        {
            count++;
            var temp = data;
            data = new byte[count];
            temp.CopyTo (data, 0);
        }

        count >>= 1;
        var bytes = new byte[count];
        for (int i = 0, j = 0; i < count; i++)
        {
            // Must support 0-9, A-F, a-f - "Any other characters cause an error."
            var hi = data[j++];
            var lo = data[j++];
            if (hi >= 'a' && hi <= 'f')
            {
                hi -= 32;
            }

            if (lo >= 'a' && lo <= 'f')
            {
                lo -= 32;
            }

            // TODO Throw on invalid characters. Stop when encountering EOD. Add one more byte if EOD is the lo byte.
            bytes[i] = (byte)((hi > '9' ? hi - '7' /*'A' + 10*/ : hi - '0') * 16 +
                              (lo > '9' ? lo - '7' /*'A' + 10*/ : lo - '0'));
        }

        return bytes;
    }
}
