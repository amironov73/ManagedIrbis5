// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* WildcardPattern.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.Updating.Internal;

internal static class WildcardPattern
{
    public static bool IsMatch (string input, string pattern)
    {
        pattern = Regex.Escape (pattern);
        pattern = pattern.Replace ("\\*", ".*?").Replace ("\\?", ".");
        pattern = "^" + pattern + "$";

        return Regex.IsMatch (input, pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
    }
}
