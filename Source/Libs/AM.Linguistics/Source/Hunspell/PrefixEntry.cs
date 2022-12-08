// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* PrefixEntry.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public sealed class PrefixEntry
    : AffixEntry
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="strip"></param>
    /// <param name="affixText"></param>
    /// <param name="conditions"></param>
    /// <param name="morph"></param>
    /// <param name="contClass"></param>
    public PrefixEntry
        (
            string strip,
            string affixText,
            CharacterConditionGroup conditions,
            MorphSet morph,
            FlagSet contClass
        )
        : base (strip, affixText, conditions, morph, contClass)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    public override string Key => Append;
}
