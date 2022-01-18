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

public sealed class PhoneTable : ArrayWrapper<PhoneticEntry>
{
    public static readonly PhoneTable Empty = TakeArray (Array.Empty<PhoneticEntry>());

    public static PhoneTable Create (IEnumerable<PhoneticEntry> entries)
    {
        return entries == null ? Empty : TakeArray (entries.ToArray());
    }

    internal static PhoneTable TakeArray (PhoneticEntry[] entries)
    {
        return entries == null ? Empty : new PhoneTable (entries);
    }

    private PhoneTable (PhoneticEntry[] entries)
        : base (entries)
    {
    }
}
