// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* AffixEntryGroupCollection.cs --
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

public sealed class AffixEntryGroupCollection<TEntry> : ArrayWrapper<AffixEntryGroup<TEntry>>
    where TEntry : AffixEntry
{
    public static readonly AffixEntryGroupCollection<TEntry> Empty = TakeArray(Array.Empty<AffixEntryGroup<TEntry>>());

    private AffixEntryGroupCollection(AffixEntryGroup<TEntry>[] entries) : base(entries)
    {
    }

    internal static AffixEntryGroupCollection<TEntry> TakeArray(AffixEntryGroup<TEntry>[] entries) =>
        entries == null ? Empty : new AffixEntryGroupCollection<TEntry>(entries);

    public static AffixEntryGroupCollection<TEntry> Create(IEnumerable<AffixEntryGroup<TEntry>> entries) =>
        entries == null ? Empty : TakeArray(entries.ToArray());
}
