// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* AffixEntryCollection.cs --
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

public sealed class AffixEntryCollection<TEntry> : ArrayWrapper<TEntry>
    where TEntry : AffixEntry
{
    public static readonly AffixEntryCollection<TEntry> Empty = TakeArray(Array.Empty<TEntry>());

    public static AffixEntryCollection<TEntry> Create(List<TEntry> entries) =>
        entries == null ? Empty : TakeArray(entries.ToArray());

    public static AffixEntryCollection<TEntry> Create(IEnumerable<TEntry> entries) =>
        entries == null ? Empty : TakeArray(entries.ToArray());

    internal static AffixEntryCollection<TEntry> TakeArray(TEntry[] entries) =>
        entries == null ? Empty : new AffixEntryCollection<TEntry>(entries);

    private AffixEntryCollection(TEntry[] entries) : base(entries)
    {
    }
}
