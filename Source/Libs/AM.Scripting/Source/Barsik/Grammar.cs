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
using System.Collections.Generic;
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
    #region Public methods

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

    #endregion

    #region Private members

    private static readonly Parser<char, Unit> BlockComment = CommentParser.SkipBlockComment
        (
            String ("/*"),
            String ("*/")
        );

    private static readonly Parser<char, Unit> LineComment = CommentParser.SkipLineComment
        (
            String ("//")
        );

    private static readonly Parser<char, Unit> Skip = Try (BlockComment)
        .Or (Try (LineComment))
        .Or (Whitespace.IgnoreResult())
        .SkipMany();

    public static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        token.Between (Skip);

    public static Parser<char, char> Tok (char token) => Tok (Char (token));

    public static Parser<char, string> Tok (string token) => Tok (String (token));

    public static Parser<char, TResult> CurlyBraces<TResult> (Parser<char, TResult> parser) =>
        parser.Between (Tok ('{'), Tok ('}'));

    public static Parser<char, TResult> RoundBrackets<TResult> (Parser<char, TResult> parser) =>
        parser.Between (Tok ('('), Tok (')'));

    private static readonly Parser<char, string> Identifier = Map
        (
            (first, rest) => first + rest,
            Letter,
            LetterOrDigit.ManyString()
        );

    private static readonly Parser<char, AtomNode> NullLiteral =
        String ("null").ThenReturn ((AtomNode) new ConstantNode (null));

    private static readonly Parser<char, AtomNode> BoolLiteral =
        OneOf (String ("false"), String ("true"))
            .Select<AtomNode> (v => new ConstantNode (v == "true"));

    private static readonly Parser<char, AtomNode> CharLiteral = Map
        (
            (_, content, _) => content,
            Char ('\''),
            AnyCharExcept ('\''),
            Char ('\'')
        )
        .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> StringLiteral = Map
        (
            (_, content, _) => new string (content.ToArray()),
            Char ('"'),
            AnyCharExcept ('"').Many(),
            Char ('"')

        )
        .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> Int32Literal = DecimalNum
        .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> Int64Literal =
        LongNum.Before (OneOf ('L', 'l'))
        .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> UInt32Literal =
        UnsignedInt (10).Before (OneOf ('U', 'u'))
        .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> UInt64Literal =
        LongNum.Before (OneOf (String ("LU"), String ("lu"), String ("UL"), String ("ul")))
        .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> Hex32Literal =
        String ("0x").Then (UnsignedInt (16))
            .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> Hex64Literal = Map
        (
            (_, value, _) => (AtomNode)new ConstantNode (value),
            String ("0x").ThenReturn (0L),
            UnsignedLong (16),
            OneOf ('L', 'l').ThenReturn (0L)
        );

    private static readonly Parser<char, AtomNode> FloatLiteral =
        Real.Before (OneOf ('F', 'f'))
            .Select (v => (AtomNode) new ConstantNode ((float) v));

    private static readonly Parser<char, AtomNode> DecimalLiteral =
        Real.Before (OneOf ('M', 'm'))
            .Select (v => (AtomNode)new ConstantNode ((decimal) v));

    private static readonly Parser<char, AtomNode> DoubleLiteral =
        Real.Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> Literal = OneOf (
                NullLiteral, BoolLiteral, CharLiteral, StringLiteral,
                Hex64Literal, Hex32Literal,
                UInt64Literal, Int64Literal, UInt32Literal,
                FloatLiteral, DecimalLiteral, Int32Literal, DoubleLiteral
            );

    private static readonly Parser<char, KeyValueNode> KeyAndValue = Map
        (
            (key, _, value) => new KeyValueNode (key, value),
            Literal,
            Tok (':'),
            Literal
        );

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

    private static Parser<char, Func<AtomNode, AtomNode>> Prefix (Parser<char, string> op) =>
        op.Select<Func<AtomNode, AtomNode>> (type => inner => new PrefixNode (type, inner));

    private static Parser<char, Func<AtomNode, AtomNode>> Postfix (Parser<char, string> op) =>
         op.Select<Func<AtomNode, AtomNode>> (type => inner => new PostfixNode (type, inner));

    private static OperatorTableRow<char, AtomNode> Prefix (string op) =>
        Operator.Prefix (Prefix (String (op)));

    private static OperatorTableRow<char, AtomNode> Postfix (string op) =>
        Operator.Postfix (Postfix (String (op)));

    private static Parser<char, Func<AtomNode, AtomNode, AtomNode>> Binary (Parser<char, string> op) =>
        op.Select<Func<AtomNode, AtomNode, AtomNode>> (type => (left, right) =>
            new BinaryNode (left, right, type));

    private static OperatorTableRow<char, AtomNode> BinaryLeft (string op) =>
        Operator.InfixL (Binary (Tok (op)));

    // ReSharper disable RedundantSuppressNullableWarningExpression
    private static readonly Parser<char, AtomNode> List = Tok (Map
        (
            (_, items, _) => (AtomNode) new ListNode (items),
            Tok ('['),
            Rec (() => Expr!).Separated (Tok (',')),
            Tok (']')
        ));
    // ReSharper restore RedundantSuppressNullableWarningExpression

    private static readonly Parser<char, AtomNode> Dictionary = Tok (Map
        (
            (_, items, _) => (AtomNode) new DictionaryNode (items),
            Tok ('{'),
            KeyAndValue.Separated (Tok (',')),
            Tok ('}')
        ));

    private static readonly Parser<char, AtomNode> Expr = ExpressionParser.Build
        (
            OneOf
                (
                    //Variable, List, Dictionary, Literal, Parenthesis
                    Variable, Literal
                ),
            new []
            {
                new [] { BinaryLeft ("*"), BinaryLeft ("/"), BinaryLeft ("%") },
                new [] { Postfix ("++"), Postfix ("--") },
                new [] { Prefix ("++"), Prefix ("--"), Prefix ("!"), Prefix ("-") },
                new [] { BinaryLeft ("+"), BinaryLeft ("-") },
                new [] { BinaryLeft ("<<"), BinaryLeft (">>") },
                new [] { BinaryLeft ("<="), BinaryLeft (">="), BinaryLeft ("<"), BinaryLeft (">") },
                new [] { BinaryLeft ("=="), BinaryLeft ("!=") },
                new [] { BinaryLeft ("&") },
                new [] { BinaryLeft ("^") },
                new [] { BinaryLeft ("|") },
                new [] { BinaryLeft ("&&") },
                new [] { BinaryLeft ("||") }
            }
        );

    //
    // Дальше начинаются разнообразные стейтменты
    //

    // разделитель стейтментов
    public static readonly Parser<char, Unit> StatementDelimiter = Try (BlockComment)
        .Or (Try (LineComment))
        .Or (Try (Char (';')).IgnoreResult())
        .Or (Whitespace.IgnoreResult())
        .SkipMany();

    // блок стейтментов
    // ReSharper disable RedundantSuppressNullableWarningExpression
    public static readonly Parser<char, IEnumerable<StatementNode>> Block =
        Rec (() => Statement!).SeparatedAndOptionallyTerminated (StatementDelimiter);
    // ReSharper restore RedundantSuppressNullableWarningExpression

    // операция присваивания
    private static readonly Parser<char, string> Operation = OneOf
        (
            Tok ("="), Tok ("+="), Tok ("-="),
            Tok ("*="), Tok ("/="), Tok ("%="), Tok ("&="),
            Tok ("|="), Tok ("^="), Tok ("<<="), Tok (">>=")
        );

    private static readonly Parser<char, StatementNode> Assignment =
        from target in Identifier
        from op in Operation
        from expr in Expr
        select (StatementNode) new AssignmentNode (target, op, expr);

    private static readonly Parser<char, StatementNode> Print = Try (Map
        (
            (name, expressions) =>
                (StatementNode) new PrintNode (expressions, name == "println"),
            OneOf (Tok ("println"), Tok ("print")),
            Expr.Separated (Tok (','))
        ));

    private static readonly Parser<char, StatementNode> While = Try (Map
        (
            (_, condition, body) => (StatementNode) new WhileNode (condition, body),
            Try (Tok ("while")),
            RoundBrackets (Expr),
            CurlyBraces (Block)
        ));

    private static readonly Parser<char, IEnumerable<StatementNode>> Else =
        from _ in Tok ("else")
        from statements in CurlyBraces (Block)
        select statements;

    private static readonly Parser<char, IfNode> ElseIf =
        from _1 in Tok ("else")
        from _2 in Tok ("if")
        from condition in RoundBrackets (Expr)
        from statements in CurlyBraces (Block)
        select new IfNode (condition, statements, null, null);

    private static readonly Parser<char, StatementNode> If =
        from _ in Tok ("if")
        from condition in RoundBrackets (Expr)
        from thenBlock in CurlyBraces (Block)
        from elseIf in ElseIf.Many().Optional()
        from elseBlock in Else.Optional()
        select (StatementNode)new IfNode (condition, thenBlock, elseIf.GetValueOrDefault(),
            elseBlock.GetValueOrDefault());

    // обобщенный стейтмент
    private static readonly Parser<char, StatementNode> Statement = OneOf
        (
            Try (If),
            Try (While),
            Try (Assignment),
            Print
        );

    //
    // Собственно программа
    //

    private static readonly Parser<char, ProgramNode> Pgm =
        Block.Before (End)
            .Select (s => new ProgramNode (s));

    #endregion
}
