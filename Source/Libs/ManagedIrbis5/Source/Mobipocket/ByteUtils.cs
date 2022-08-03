// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* ByteUtils.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Mobipocket;

/// <summary>
///
/// </summary>
internal static class ByteUtils
{
    /// <summary>
    ///
    /// </summary>
    public static short ToInt16 (byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse (bytes);
        }

        return BitConverter.ToInt16 (bytes, 0);
    }

    /// <summary>
    ///
    /// </summary>
    public static int ToInt32 (byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse (bytes);
        }

        return BitConverter.ToInt32 (bytes, 0);
    }

    /// <summary>
    ///
    /// </summary>
    public static int GetInt32 (byte[] bytes)
    {
        var result = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            result = (result << 8) + (bytes[i] & 0xff);
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    public static long ToUInt32 (byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse (bytes);
        }

        return BitConverter.ToUInt32 (bytes, 0);
    }

    /// <summary>
    ///
    /// </summary>
    public static string ToString (byte[] bytes)
    {
        if (bytes == null)
        {
            return "";
        }

        var enc = Encoding.ASCII;

        return enc.GetString (bytes).Replace ("\0", "");
    }
}
