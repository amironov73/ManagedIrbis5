// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* Ascii85Decode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Filters;

/// <summary>
/// Implements the ASCII85Decode filter.
/// </summary>
public class Ascii85Decode
    : Filter
{
    // Reference: 3.3.2  ASCII85Decode Filter / Page 69

    /// <summary>
    /// Encodes the specified data.
    /// </summary>
    public override byte[] Encode
        (
            byte[] data
        )
    {
        Sure.NotNull (data);

        var length = data.Length; // length == 0 is must not be treated as a special case.
        var words = length / 4;
        var rest = length - (words * 4);
        var result = new byte[words * 5 + (rest == 0 ? 0 : rest + 1) + 2];

        int idxIn = 0, idxOut = 0;
        var wCount = 0;
        while (wCount < words)
        {
            var val = ((uint)data[idxIn++] << 24) + ((uint)data[idxIn++] << 16) + ((uint)data[idxIn++] << 8) +
                      data[idxIn++];
            if (val == 0)
            {
                result[idxOut++] = (byte)'z';
            }
            else
            {
                var c5 = (byte)(val % 85 + '!');
                val /= 85;
                var c4 = (byte)(val % 85 + '!');
                val /= 85;
                var c3 = (byte)(val % 85 + '!');
                val /= 85;
                var c2 = (byte)(val % 85 + '!');
                val /= 85;
                var c1 = (byte)(val + '!');

                result[idxOut++] = c1;
                result[idxOut++] = c2;
                result[idxOut++] = c3;
                result[idxOut++] = c4;
                result[idxOut++] = c5;
            }

            wCount++;
        }

        if (rest == 1)
        {
            var val = (uint)data[idxIn] << 24;
            val /= 85 * 85 * 85;
            var c2 = (byte)(val % 85 + '!');
            val /= 85;
            var c1 = (byte)(val + '!');

            result[idxOut++] = c1;
            result[idxOut++] = c2;
        }
        else if (rest == 2)
        {
            var val = ((uint)data[idxIn++] << 24) + ((uint)data[idxIn] << 16);
            val /= 85 * 85;
            var c3 = (byte)(val % 85 + '!');
            val /= 85;
            var c2 = (byte)(val % 85 + '!');
            val /= 85;
            var c1 = (byte)(val + '!');

            result[idxOut++] = c1;
            result[idxOut++] = c2;
            result[idxOut++] = c3;
        }
        else if (rest == 3)
        {
            var val = ((uint)data[idxIn++] << 24) + ((uint)data[idxIn++] << 16) + ((uint)data[idxIn] << 8);
            val /= 85;
            var c4 = (byte)(val % 85 + '!');
            val /= 85;
            var c3 = (byte)(val % 85 + '!');
            val /= 85;
            var c2 = (byte)(val % 85 + '!');
            val /= 85;
            var c1 = (byte)(val + '!');

            result[idxOut++] = c1;
            result[idxOut++] = c2;
            result[idxOut++] = c3;
            result[idxOut++] = c4;
        }

        result[idxOut++] = (byte)'~';
        result[idxOut++] = (byte)'>';

        if (idxOut < result.Length)
            Array.Resize (ref result, idxOut);

        return result;
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
        Sure.NotNull (data);

        int idx;
        var length = data.Length;
        var zCount = 0;
        var idxOut = 0;
        for (idx = 0; idx < length; idx++)
        {
            var ch = (char)data[idx];
            if (ch >= '!' && ch <= 'u')
                data[idxOut++] = (byte)ch;
            else if (ch == 'z')
            {
                data[idxOut++] = (byte)ch;
                zCount++;
            }
            else if (ch == '~')
            {
                if ((char)data[idx + 1] != '>')
                    throw new ArgumentException ("Illegal character.", "data");
                break;
            }

            // ingnore unknown character
        }

        // Loop not ended with break?
        if (idx == length)
            throw new ArgumentException ("Illegal character.", "data");

        length = idxOut;
        var nonZero = length - zCount;
        var byteCount = 4 * (zCount + (nonZero / 5)); // full 4 byte blocks

        var remainder = nonZero % 5;
        if (remainder == 1)
            throw new InvalidOperationException ("Illegal character.");

        if (remainder != 0)
            byteCount += remainder - 1;

        var output = new byte[byteCount];

        idxOut = 0;
        idx = 0;
        while (idx + 4 < length)
        {
            var ch = (char)data[idx];
            if (ch == 'z')
            {
                idx++;
                idxOut += 4;
            }
            else
            {
                // TODO: check
                var value =
                    (long)(data[idx++] - '!') * (85 * 85 * 85 * 85) +
                    (uint)(data[idx++] - '!') * (85 * 85 * 85) +
                    (uint)(data[idx++] - '!') * (85 * 85) +
                    (uint)(data[idx++] - '!') * 85 +
                    (uint)(data[idx++] - '!');

                if (value > UInt32.MaxValue)
                    throw new InvalidOperationException ("Value of group greater than 2 power 32 - 1.");

                output[idxOut++] = (byte)(value >> 24);
                output[idxOut++] = (byte)(value >> 16);
                output[idxOut++] = (byte)(value >> 8);
                output[idxOut++] = (byte)value;
            }
        }

        // I have found no appropriate algorithm, so I write my own. In some rare cases the value must not
        // increased by one, but I cannot found a general formula or a proof.
        // All possible cases are tested programmatically.
        if (remainder == 2) // one byte
        {
            var value =
                (uint)(data[idx++] - '!') * (85 * 85 * 85 * 85) +
                (uint)(data[idx] - '!') * (85 * 85 * 85);

            // Always increase if not zero (tried out).
            if (value != 0)
                value += 0x01000000;

            output[idxOut] = (byte)(value >> 24);
        }
        else if (remainder == 3) // two bytes
        {
            var idxIn = idx;
            var value =
                (uint)(data[idx++] - '!') * (85 * 85 * 85 * 85) +
                (uint)(data[idx++] - '!') * (85 * 85 * 85) +
                (uint)(data[idx] - '!') * (85 * 85);

            if (value != 0)
            {
                value &= 0xFFFF0000;
                var val = value / (85 * 85);
                var c3 = (byte)(val % 85 + '!');
                val /= 85;
                var c2 = (byte)(val % 85 + '!');
                val /= 85;
                var c1 = (byte)(val + '!');
                if (c1 != data[idxIn] || c2 != data[idxIn + 1] || c3 != data[idxIn + 2])
                {
                    value += 0x00010000;

                    //Count2++;
                }
            }

            output[idxOut++] = (byte)(value >> 24);
            output[idxOut] = (byte)(value >> 16);
        }
        else if (remainder == 4) // three bytes
        {
            var idxIn = idx;
            var value =
                (uint)(data[idx++] - '!') * (85 * 85 * 85 * 85) +
                (uint)(data[idx++] - '!') * (85 * 85 * 85) +
                (uint)(data[idx++] - '!') * (85 * 85) +
                (uint)(data[idx] - '!') * 85;

            if (value != 0)
            {
                value &= 0xFFFFFF00;
                var val = value / 85;
                var c4 = (byte)(val % 85 + '!');
                val /= 85;
                var c3 = (byte)(val % 85 + '!');
                val /= 85;
                var c2 = (byte)(val % 85 + '!');
                val /= 85;
                var c1 = (byte)(val + '!');
                if (c1 != data[idxIn] || c2 != data[idxIn + 1] || c3 != data[idxIn + 2] || c4 != data[idxIn + 3])
                {
                    value += 0x00000100;

                    //Count3++;
                }
            }

            output[idxOut++] = (byte)(value >> 24);
            output[idxOut++] = (byte)(value >> 16);
            output[idxOut] = (byte)(value >> 8);
        }

        return output;
    }
}
