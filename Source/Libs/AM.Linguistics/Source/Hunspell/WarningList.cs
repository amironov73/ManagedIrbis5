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

public sealed class WarningList : ArrayWrapper<string>
{
    public static WarningList Create (IEnumerable<string> warnings)
    {
        return warnings == null ? TakeArray (null) : TakeArray (warnings.ToArray());
    }

    internal static WarningList TakeArray (string[] warnings)
    {
        return new (warnings ?? Array.Empty<string>());
    }

    private WarningList (string[] warnings)
        : base (warnings)
    {
    }
}
