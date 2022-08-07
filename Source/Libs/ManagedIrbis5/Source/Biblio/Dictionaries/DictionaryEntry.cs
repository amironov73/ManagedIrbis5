// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DictionaryEntry.cs -- запись в словаре
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Запись в словаре: термин и список ссылок на него.
/// </summary>
public sealed class DictionaryEntry
{
    #region Properties

    /// <summary>
    /// Термин.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Список ссылок.
    /// </summary>
    public List<int> References { get; set; } = new ();

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append (Title);
        builder.Append (' ');
        var refs = References.ToArray();
        Array.Sort (refs);
        var first = true;
        foreach (var reference in refs)
        {
            if (!first)
            {
                builder.Append (", ");
            }

            builder.Append (reference);
            first = false;
        }

        return builder.ReturnShared();
    }

    #endregion
}
