// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Affix.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

internal sealed class Affix<TEntry>
    where TEntry : AffixEntry
{
    internal static Affix<TEntry> Create (TEntry entry, AffixEntryGroup<TEntry> group)
    {
        return new (entry, group.AFlag, group.Options);
    }

    public Affix (TEntry entry, FlagValue aFlag, AffixEntryOptions options)
    {
        Entry = entry ?? throw new ArgumentNullException (nameof (entry));
        AFlag = aFlag;
        Options = options;
    }

    public TEntry Entry { get; }

    public FlagValue AFlag { get; }

    public AffixEntryOptions Options { get; }
}
