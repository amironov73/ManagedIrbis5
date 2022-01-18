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

#nullable enable

namespace AM.Linguistics.Hunspell;

public sealed class PhoneticEntry
{
    public PhoneticEntry (string rule, string replace)
    {
        Rule = rule ?? string.Empty;
        Replace = replace ?? string.Empty;
    }

    public string Rule { get; }

    public string Replace { get; }
}
