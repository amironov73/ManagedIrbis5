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
    public static readonly FlagValue DefaultFlags = new FlagValue(65510);

    public static readonly FlagValue ForbiddenWord = new FlagValue(65510);

    public static readonly FlagValue OnlyUpcaseFlag = new FlagValue(65511);

    public static readonly FlagValue LetterF = new FlagValue('F');

    public static readonly FlagValue LetterG = new FlagValue('G');

    public static readonly FlagValue LetterH = new FlagValue('H');

    public static readonly FlagValue LetterI = new FlagValue('I');

    public static readonly FlagValue LetterJ = new FlagValue('J');

    public static readonly FlagValue LetterXLower = new FlagValue('x');

    public static readonly FlagValue LetterCLower = new FlagValue('c');

    public static readonly FlagValue LetterPercent = new FlagValue('%');
}