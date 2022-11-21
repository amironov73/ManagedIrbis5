// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* SpellCheckResult.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
/// /
/// </summary>
public struct SpellCheckResult
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string? Root { get; }

    /// <summary>
    ///
    /// </summary>
    public SpellCheckResultType Info { get; }

    /// <summary>
    ///
    /// </summary>
    public bool Correct { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="correct"></param>
    public SpellCheckResult
        (
            bool correct
        )
    {
        Root = null;
        Info = SpellCheckResultType.None;
        Correct = correct;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="info"></param>
    /// <param name="correct"></param>
    public SpellCheckResult
        (
            string root,
            SpellCheckResultType info,
            bool correct
        )
    {
        Root = root;
        Info = info;
        Correct = correct;
    }

    #endregion
}
