// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* StringExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

#endregion

#nullable enable

namespace AM.Updating.Internal.Extensions;

internal static class StringExtensions
{
    public static string SubstringUntil (this string s, string sub,
        StringComparison comparison = StringComparison.Ordinal)
    {
        var index = s.IndexOf (sub, comparison);
        return index >= 0
            ? s.Substring (0, index)
            : s;
    }

    public static string SubstringAfter (this string s, string sub,
        StringComparison comparison = StringComparison.Ordinal)
    {
        var index = s.IndexOf (sub, comparison);
        return index >= 0
            ? s.Substring (index + sub.Length, s.Length - index - sub.Length)
            : string.Empty;
    }

    public static byte[] GetBytes (this string input, Encoding encoding) => encoding.GetBytes (input);

    public static byte[] GetBytes (this string input) => input.GetBytes (Encoding.UTF8);

    public static string ToBase64 (this byte[] data) => Convert.ToBase64String (data);
}
