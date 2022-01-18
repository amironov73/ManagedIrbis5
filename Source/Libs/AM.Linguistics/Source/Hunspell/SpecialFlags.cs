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

public static class SpecialFlags
{
    public static readonly FlagValue DefaultFlags = new (65510);

    public static readonly FlagValue ForbiddenWord = new (65510);

    public static readonly FlagValue OnlyUpcaseFlag = new (65511);

    public static readonly FlagValue LetterF = new ('F');

    public static readonly FlagValue LetterG = new ('G');

    public static readonly FlagValue LetterH = new ('H');

    public static readonly FlagValue LetterI = new ('I');

    public static readonly FlagValue LetterJ = new ('J');

    public static readonly FlagValue LetterXLower = new ('x');

    public static readonly FlagValue LetterCLower = new ('c');

    public static readonly FlagValue LetterPercent = new ('%');
}
