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

using System;

using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Грамматика языка Барсик.
/// </summary>
public static class Grammar
{
    #region Private members

    /// <summary>
    /// Токен, например, "+" или "==".
    /// </summary>
    private static BarsikTerm Term (string kind) => new (kind);

    /// <summary>
    /// Идентификатор, например, "hello" или "help123".
    /// </summary>
    private static readonly BarsikIdentifier Identifier = new ();

    #endregion

    #region Public methods

    private static readonly Parser<BarsikToken, AtomNode> CharLiteral =
        new BarsikTerm (BarsikToken.Char)
            .Select<AtomNode> (v => new ConstantNode (v![0]));

    private static readonly Parser<BarsikToken, AtomNode> StringLiteral =
        new BarsikTerm (BarsikToken.String)
            .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<BarsikToken, AtomNode> Literal = OneOf
        (
            CharLiteral,
            StringLiteral
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
