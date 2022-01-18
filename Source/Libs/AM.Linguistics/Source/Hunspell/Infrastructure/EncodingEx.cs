// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* .cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure;

internal static class EncodingEx
{
    public static Encoding GetEncodingByName (ReadOnlySpan<char> encodingName)
    {
        if (encodingName.IsEmpty) return null;

        if (encodingName.Equals ("UTF8", StringComparison.OrdinalIgnoreCase) ||
            encodingName.Equals ("UTF-8", StringComparison.OrdinalIgnoreCase)) return Encoding.UTF8;

        var encodingNameString = encodingName.ToString();
        try
        {
            return Encoding.GetEncoding (encodingNameString);
        }
        catch (ArgumentException)
        {
            return GetEncodingByAlternateNames (encodingNameString);
        }
    }

    private static Encoding GetEncodingByAlternateNames (string encodingName)
    {
        var spaceIndex = encodingName.IndexOf (' ');
        if (spaceIndex > 0) return GetEncodingByName (encodingName.AsSpan (0, spaceIndex));

        if (encodingName.Length >= 4 && encodingName.StartsWith ("ISO") && encodingName[3] != '-')
            return GetEncodingByName (encodingName.Insert (3, "-").AsSpan());

        return null;
    }
}
