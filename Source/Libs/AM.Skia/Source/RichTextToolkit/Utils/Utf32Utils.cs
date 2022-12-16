// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Utf32Utils.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Utils;

/// <summary>
/// Miscellaneous utility functions for working with UTF-32 data.
/// </summary>
public static class Utf32Utils
{
    /// <summary>
    /// Convert a slice of UTF-32 integer code points to a string
    /// </summary>
    /// <param name="buffer">The code points to convert</param>
    /// <returns>A string</returns>
    public static string FromUtf32 (Slice<int> buffer)
    {
        unsafe
        {
            fixed (int* p = buffer.Underlying)
            {
                var pBuf = p + buffer.Start;
                return new string ((sbyte*)pBuf, 0, buffer.Length * sizeof (int), Encoding.UTF32);
            }
        }
    }

    /// <summary>
    /// Converts a string to an integer array of UTF-32 code points
    /// </summary>
    /// <param name="str">The string to convert</param>
    /// <returns>The converted code points</returns>
    public static int[] ToUtf32 (string str)
    {
        unsafe
        {
            fixed (char* pstr = str)
            {
                // Get required byte count
                var byteCount = Encoding.UTF32.GetByteCount (pstr, str.Length);
                System.Diagnostics.Debug.Assert ((byteCount % 4) == 0);

                // Allocate buffer
                var utf32 = new int[byteCount / sizeof (int)];
                fixed (int* putf32 = utf32)
                {
                    // Convert
                    Encoding.UTF32.GetBytes (pstr, str.Length, (byte*)putf32, byteCount);

                    // Done
                    return utf32;
                }
            }
        }
    }
}
