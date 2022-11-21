// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* SpecialFlags.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public static class SpecialFlags
{
    /// <summary>
    ///
    /// </summary>
    public static readonly FlagValue DefaultFlags = new (65510);

    /// <summary>
    ///
    /// </summary>
    public static readonly FlagValue ForbiddenWord = new (65510);

    /// <summary>
    ///
    /// </summary>
    public static readonly FlagValue OnlyUpcaseFlag = new (65511);

    /// <summary>
    ///
    /// </summary>
    public static readonly FlagValue LetterF = new ('F');

    /// <summary>
    ///
    /// </summary>
    public static readonly FlagValue LetterG = new ('G');

    /// <summary>
    ///
    /// </summary>
    public static readonly FlagValue LetterH = new ('H');

    /// <summary>
    ///
    /// </summary>
    public static readonly FlagValue LetterI = new ('I');

    /// <summary>
    ///
    /// </summary>
    public static readonly FlagValue LetterJ = new ('J');

    /// <summary>
    ///
    /// </summary>
    public static readonly FlagValue LetterXLower = new ('x');

    /// <summary>
    ///
    /// </summary>
    public static readonly FlagValue LetterCLower = new ('c');

    /// <summary>
    ///
    /// </summary>
    public static readonly FlagValue LetterPercent = new ('%');
}
