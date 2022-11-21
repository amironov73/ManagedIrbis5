// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SuffixEntry.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Linguistics.Hunspell.Infrastructure;

#if !NO_INLINE
using System.Runtime.CompilerServices;
#endif

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public sealed class SuffixEntry
    : AffixEntry
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="strip"></param>
    /// <param name="affixText"></param>
    /// <param name="conditions"></param>
    /// <param name="morph"></param>
    /// <param name="contClass"></param>
#if !NO_INLINE
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
    public SuffixEntry
        (
            string strip,
            string affixText,
            CharacterConditionGroup conditions,
            MorphSet morph,
            FlagSet contClass
        )
        : base (strip, affixText, conditions, morph, contClass)
    {
        Key = affixText.GetReversed();
    }

    /// <summary>
    ///
    /// </summary>
    public override string Key { get; }
}
