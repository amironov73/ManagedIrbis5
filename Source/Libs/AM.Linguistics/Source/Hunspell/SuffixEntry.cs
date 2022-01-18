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

using AM.Linguistics.Hunspell.Infrastructure;

#if !NO_INLINE
using System.Runtime.CompilerServices;
#endif

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell
{
    public sealed class SuffixEntry : AffixEntry
    {
#if !NO_INLINE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public SuffixEntry(
            string strip,
            string affixText,
            CharacterConditionGroup conditions,
            MorphSet morph,
            FlagSet contClass)
            : base(strip, affixText, conditions, morph, contClass)
        {
            Key = affixText.GetReversed();
        }

        public sealed override string Key { get; }
    }
}
