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

public sealed class MapEntry : ArrayWrapper<string>
{
    public static readonly MapEntry Empty = TakeArray (Array.Empty<string>());

    public static MapEntry Create (IEnumerable<string> values)
    {
        return values == null ? Empty : TakeArray (values.ToArray());
    }

    internal static MapEntry TakeArray (string[] values)
    {
        return values == null ? Empty : new MapEntry (values);
    }

    private MapEntry (string[] values)
        : base (values)
    {
    }
}
