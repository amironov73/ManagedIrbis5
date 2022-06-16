// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Bits.cs -- манипуляции с битами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

#endregion

#nullable enable

namespace AM.Net;

/// <summary>
/// Манипуляции с битами.
/// </summary>
internal static class Bits
{
    #region Public methods

    public static byte[] Not
        (
            byte[] bytes
        )
    {
        return bytes.Select (b => (byte)~b).ToArray();
    }

    public static byte[] And
        (
            byte[] left,
            byte[] right
        )
    {
        var length = left.Length;
        var result = new byte[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = (byte)(left[i] & right[i]);
        }

        return result;
    }

    public static byte[] Or
        (
            byte[] left,
            byte[] right
        )
    {
        var length = left.Length;
        var result = new byte[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = (byte)(left[i] | right[i]);
        }

        return result;
    }

    public static bool MoreOrEqual
        (
            byte[] left,
            byte[] right
        )
    {
        var length = left.Length;
        var result = new int[length];
        for (var i = 0; i < length; i++)
        {
            int a = left[i], b = right[i];
            result[i] = a == b ? 0 : a < b ? 1 : -1;
        }

        return result
            .SkipWhile (c => c == 0)
            .FirstOrDefault() >= 0;
    }

    public static bool LessOrEqual
        (
            byte[] left,
            byte[] right
        )
    {
        var length = left.Length;
        var result = new int[length];
        for (var i = 0; i < length; i++)
        {
            int a = left[i], b = right[i];
            result[i] = a == b ? 0 : a < b ? 1 : -1;
        }

        return result
            .SkipWhile (c => c == 0)
            .FirstOrDefault() <= 0;
    }

    public static byte[] GetBitMask
        (
            int bufferSize,
            int bitLen
        )
    {
        var maskBytes = new byte[bufferSize];
        var bytesLen = bitLen / 8;
        var bitsLen = bitLen % 8;
        for (var i = 0; i < bytesLen; i++)
        {
            maskBytes[i] = 0xff;
        }

        if (bitsLen > 0)
        {
            maskBytes[bytesLen] = (byte)~Enumerable
                .Range (1, 8 - bitsLen).Select (n => 1 << n - 1)
                .Aggregate ((a, b) => a | b);
        }

        return maskBytes;
    }

    #endregion
}
