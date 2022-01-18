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

namespace AM.Linguistics.Hunspell.Infrastructure
{
    static class EnumEx
    {
        public static bool HasFlag(this AffixConfigOptions value, AffixConfigOptions flag) => (value & flag) == flag;

        public static bool HasFlag(this WordEntryOptions value, WordEntryOptions flag) => (value & flag) == flag;

        public static bool HasFlag(this AffixEntryOptions value, AffixEntryOptions flag) => (value & flag) == flag;

        public static bool HasFlag(this SpellCheckResultType value, SpellCheckResultType flag) => (value & flag) == flag;
    }
}
