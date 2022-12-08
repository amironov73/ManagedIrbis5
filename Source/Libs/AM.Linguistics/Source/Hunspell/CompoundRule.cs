// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* CompoundRule.cs --
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

/// <summary>
///
/// </summary>
public sealed class CompoundRule
    : ArrayWrapper<FlagValue>
{
    /// <summary>
    ///
    /// </summary>
    public static readonly CompoundRule Empty = TakeArray (Array.Empty<FlagValue>());

    /// <summary>
    ///
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static CompoundRule Create (List<FlagValue> values)
    {
        return values == null! ? Empty : TakeArray (values.ToArray());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public static CompoundRule Create (IEnumerable<FlagValue> values)
    {
        return values == null! ? Empty : TakeArray (values.ToArray());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    internal static CompoundRule TakeArray (FlagValue[] values)
    {
        return values == null! ? Empty : new CompoundRule (values);
    }

    private CompoundRule (FlagValue[] values)
        : base (values)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsWildcard (int index)
    {
        var value = this[index];
        return value == '*' || value == '?';
    }

    internal bool ContainsRuleFlagForEntry (WordEntryDetail details)
    {
        foreach (var flag in _items)
            if (!flag.IsWildcard && details.ContainsFlag (flag))
                return true;

        return false;
    }
}
