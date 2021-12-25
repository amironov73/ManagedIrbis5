// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Grammar.cs -- грамматика Barsik
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Linq;

using Pidgin;
using Pidgin.Comment;
using Pidgin.Expression;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Грамматика Barsik.
/// </summary>
static class Grammar
{
    public static readonly Parser<char, Unit> BlockComment = CommentParser.SkipBlockComment
        (
            Try (String ("/*")),
            Try (String ("*/"))
        );

    public static readonly Parser<char, Unit> LineComment = CommentParser.SkipLineComment
        (
            Try (String ("//"))
        );

    public static readonly Parser<char, Unit> Skip = Try (BlockComment)
        .Or (Try (LineComment))
        .Or (Try (Char (';')).IgnoreResult())
        .Or (Whitespace.IgnoreResult())
        .SkipMany();

    public static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        Try (token).Between (Skip);

    public static Parser<char, char> Tok (char token) => Tok (Char (token));

    public static Parser<char, string> Tok (string token) => Tok (String (token));

    private static readonly Parser<char, string> Identifier = Tok
        (
            Map
                (
                    (first, rest) => first + rest,
                    Letter,
                    LetterOrDigit.ManyString()
                )
        );

    private static readonly Parser<char, AtomNode> NullLiteral =
        Tok ("null").ThenReturn ((AtomNode) new ConstantNode (null));

    private static readonly Parser<char, AtomNode> BoolLiteral =
        Tok (OneOf (String ("false"), String ("true")))
            .Select<AtomNode> (v => new ConstantNode (v == "true"));

    private static readonly Parser<char, AtomNode> CharLiteral = Tok (Map
            (
                (_, content, _) => content,
                Char ('\''),
                AnyCharExcept ('\''),
                Char ('\'')
            ))
        .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> StringLiteral = Tok (Map
            (
                (_, content, _) => new string (content.ToArray()),
                Char ('"'),
                AnyCharExcept ('"').Many(),
                Char ('"')

            ))
        .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> Int32Literal = Tok (DecimalNum)
        .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> DoubleLiteral = Tok (Real)
        .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> Literal = Tok (OneOf (
                NullLiteral, BoolLiteral, CharLiteral, StringLiteral,
                DoubleLiteral, Int32Literal
            ));

    private static readonly Parser<char, AtomNode> Variable = Identifier
        .Select<AtomNode> (name => new VariableNode (name));

    // ReSharper disable RedundantSuppressNullableWarningExpression
    private static readonly Parser<char, AtomNode> Parenthesis = Map
            (
                (_, inner, _) => inner,
                Tok ('('),
                Rec (() => Expr!),
                Tok (')')
            )
        .Select<AtomNode> (inner => new ParenthesisNode (inner));
    // ReSharper restore RedundantSuppressNullableWarningExpression

    // private static Parser<char, Func<AtomNode, AtomNode>> Prefix (Parser<char, string> op) =>
    //     op.Select<Func<AtomNode, AtomNode>> (type => inner => new PrefixNode (type, inner));
    //
    // private static Parser<char, Func<AtomNode, AtomNode>> Postfix (Parser<char, string> op) =>
    //     op.Select<Func<AtomNode, AtomNode>> (type => inner => new PostfixNode (type, inner));

    private static Parser<char, Func<AtomNode, AtomNode, AtomNode>> Binary (Parser<char, string> op) =>
        op.Select<Func<AtomNode, AtomNode, AtomNode>> (type => (left, right) =>
            new BinaryNode (left, right, type));

    private static OperatorTableRow<char, AtomNode> BinaryLeft (string op) =>
        Operator.InfixL (Binary (Tok (op)));

    private static readonly Parser<char, AtomNode> Expr = ExpressionParser.Build
        (
            OneOf
                (
                    Literal, Variable, Parenthesis
                ),
            new []
            {
                new [] { BinaryLeft ("*"), BinaryLeft ("/"), BinaryLeft ("%") },
                new [] { BinaryLeft ("+"), BinaryLeft ("-") }
            }
        );

    private static readonly Parser<char, StatementNode> Assignment = Tok (
            from target in Identifier
            from eq in Tok ("=")
            from expr in Expr
            select (StatementNode) new AssignmentNode (target, expr)
        );

    private static readonly Parser<char, StatementNode> Print = Tok (Map
        (
            (name, expressions) =>
                (StatementNode) new PrintNode (expressions, name == "println"),
            OneOf (Tok ("println"), Tok ("print")),
            Expr.Separated (Tok (','))
        ));

    private static readonly Parser<char, StatementNode> Statement = OneOf
        (
            Print,
            Assignment
        );

    private static readonly Parser<char, ProgramNode> Pgm =
        Statement.SeparatedAndOptionallyTerminated (Skip)
            .Before (End)
            .Select (s => new ProgramNode (s));

    /// <summary>
    /// Разбор текста скрипта.
    /// </summary>
    public static ProgramNode ParseProgram
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        return Pgm.ParseOrThrow (sourceCode);
    }

    /// <summary>
    /// Разбор текста программы.
    /// </summary>
    public static AtomNode ParseExpression
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        return Expr.Before (End).ParseOrThrow (sourceCode);
    }

}
