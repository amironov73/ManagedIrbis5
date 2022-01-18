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

public sealed class MapTable : ArrayWrapper<MapEntry>
{
    public static readonly MapTable Empty = TakeArray(Array.Empty<MapEntry>());

    public static MapTable Create(IEnumerable<MapEntry> entries) => entries == null ? Empty : TakeArray(entries.ToArray());

    internal static MapTable TakeArray(MapEntry[] entries) => entries == null ? Empty : new MapTable(entries);

    private MapTable(MapEntry[] entries)
        : base(entries)
    {
    }
}
