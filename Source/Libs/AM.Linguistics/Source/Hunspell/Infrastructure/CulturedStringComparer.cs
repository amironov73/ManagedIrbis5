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
using System.Globalization;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure;

/// <summary>
/// Provides the ability to compare text using a configured culture.
/// </summary>
internal sealed class CulturedStringComparer : StringComparer
{
    public CulturedStringComparer (CultureInfo culture)
    {
        Culture = culture ?? throw new ArgumentNullException (nameof (culture));
        CompareInfo = culture.CompareInfo;
    }

    private CultureInfo Culture { get; }

    private CompareInfo CompareInfo { get; }

    public override int Compare (string x, string y)
    {
        return CompareInfo.Compare (x, y);
    }

    public override bool Equals (string x, string y)
    {
        return Compare (x, y) == 0;
    }

    public override int GetHashCode (string obj)
    {
#if NO_COMPAREINFO_HASHCODE
            return 0;
#else
        return CompareInfo.GetHashCode (obj, CompareOptions.None);
#endif
    }
}
