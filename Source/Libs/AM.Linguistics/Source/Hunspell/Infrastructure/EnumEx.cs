// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* EnumEx.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure;

internal static class EnumEx
{
    public static bool HasFlag
        (
            this AffixConfigOptions value,
            AffixConfigOptions flag
        )
    {
        return (value & flag) == flag;
    }

    public static bool HasFlag
        (
            this WordEntryOptions value,
            WordEntryOptions flag
        )
    {
        return (value & flag) == flag;
    }

    public static bool HasFlag
        (
            this AffixEntryOptions value,
            AffixEntryOptions flag
        )
    {
        return (value & flag) == flag;
    }

    public static bool HasFlag
        (
            this SpellCheckResultType value,
            SpellCheckResultType flag
        )
    {
        return (value & flag) == flag;
    }
}
