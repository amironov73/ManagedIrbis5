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

using AM.Linguistics.Hunspell.Infrastructure;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

public sealed class PatternEntry
{
    public PatternEntry (string pattern, string pattern2, string pattern3, FlagValue condition, FlagValue condition2)
    {
        Pattern = pattern ?? string.Empty;
        Pattern2 = pattern2 ?? string.Empty;
        Pattern3 = pattern3 ?? string.Empty;
        Condition = condition;
        Condition2 = condition2;
    }

    public string Pattern { get; }

    public string Pattern2 { get; }

    public string Pattern3 { get; }

    public FlagValue Condition { get; }

    public FlagValue Condition2 { get; }

    internal bool Pattern3DoesNotMatch (string word, int offset)
    {
        return Pattern3.Length == 0
               || !word.AsSpan (offset).StartsWith (Pattern3.AsSpan());
    }
}
