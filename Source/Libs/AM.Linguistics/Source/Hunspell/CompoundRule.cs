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
using System.Collections.Generic;
using System.Linq;

using AM.Linguistics.Hunspell.Infrastructure;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

public sealed class CompoundRule : ArrayWrapper<FlagValue>
{
    public static readonly CompoundRule Empty = TakeArray (Array.Empty<FlagValue>());

    public static CompoundRule Create (List<FlagValue> values)
    {
        return values == null ? Empty : TakeArray (values.ToArray());
    }

    public static CompoundRule Create (IEnumerable<FlagValue> values)
    {
        return values == null ? Empty : TakeArray (values.ToArray());
    }

    internal static CompoundRule TakeArray (FlagValue[] values)
    {
        return values == null ? Empty : new CompoundRule (values);
    }

    private CompoundRule (FlagValue[] values)
        : base (values)
    {
    }

    public bool IsWildcard (int index)
    {
        var value = this[index];
        return value == '*' || value == '?';
    }

    internal bool ContainsRuleFlagForEntry (WordEntryDetail details)
    {
        foreach (var flag in items)
            if (!flag.IsWildcard && details.ContainsFlag (flag))
                return true;

        return false;
    }
}
