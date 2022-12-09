// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* PhoneticEntry.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public sealed class PhoneticEntry
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string Rule { get; }

    /// <summary>
    ///
    /// </summary>
    public string Replace { get; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="rule"></param>
    /// <param name="replace"></param>
    public PhoneticEntry
        (
            string? rule,
            string? replace
        )
    {
        Rule = rule ?? string.Empty;
        Replace = replace ?? string.Empty;
    }

    #endregion
}
