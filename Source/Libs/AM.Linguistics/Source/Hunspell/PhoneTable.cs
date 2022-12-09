// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* PhoneTable.cs --
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
public sealed class PhoneTable
    : ArrayWrapper<PhoneticEntry>
{
    /// <summary>
    ///
    /// </summary>
    public static readonly PhoneTable Empty = TakeArray (Array.Empty<PhoneticEntry>());

    /// <summary>
    ///
    /// </summary>
    /// <param name="entries"></param>
    /// <returns></returns>
    public static PhoneTable Create (IEnumerable<PhoneticEntry> entries)
    {
        return entries == null! ? Empty : TakeArray (entries.ToArray());
    }

    internal static PhoneTable TakeArray (PhoneticEntry[] entries)
    {
        return entries == null! ? Empty : new PhoneTable (entries);
    }

    private PhoneTable (PhoneticEntry[] entries)
        : base (entries)
    {
        // пустое тело конструктора
    }
}
