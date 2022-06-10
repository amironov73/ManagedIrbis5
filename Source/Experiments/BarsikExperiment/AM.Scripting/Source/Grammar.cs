// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* Grammar.cs -- грамматика языка Барсик
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Pidgin;

using static Pidgin.Parser;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Грамматика языка Барсик.
/// </summary>
internal static class Grammar
{
    #region Private members

    /// <summary>
    /// Токен, например, "+" или "==".
    /// </summary>
    internal static TermParser Term (params string[] kinds) => new (kinds);

    /// <summary>
    /// Идентификатор, например, "hello" или "help123".
    /// </summary>
    internal static readonly IdentifierParser Identifier = new ();

    /// <summary>
    /// Зарезервированные слова, например, "if".
    /// </summary>
    internal static ReservedParser Keyword (params string[] kinds) => new (kinds);

    private static readonly Parser<Token, ConstantNode> NullLiteral =
        Keyword ("null").ThenReturn (new ConstantNode (null));

    private static readonly Parser<Token, ConstantNode> TrueLiteral =
        Keyword ("true").ThenReturn (new ConstantNode (true));

    private static readonly Parser<Token, ConstantNode> FalseLiteral =
        Keyword ("false").ThenReturn (new ConstantNode (false));

    private static readonly Parser<Token, ConstantNode> CharLiteral =
        new ConstantParser (TokenKind.Char)
            .Select (v => v.ChangeValue (((string) v.Value!)[0]));

    private static readonly Parser<Token, ConstantNode> StringLiteral =
        new ConstantParser (TokenKind.String)
            .Select (v => v.ChangeValue ((string) v.Value!));

    private static readonly Parser<Token, ConstantNode> NumberLiteral =
        new NumberParser();

    internal static readonly Parser<Token, ConstantNode> Literal = OneOf
        (
            NullLiteral,
            TrueLiteral,
            FalseLiteral,
            CharLiteral,
            StringLiteral,
            NumberLiteral
        );

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текст скрипта.
    /// </summary>
    public static void ParseProgram
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);
    }

    #endregion
}
