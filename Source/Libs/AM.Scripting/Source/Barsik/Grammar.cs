// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantSuppressNullableWarningExpression

/* Grammar.cs -- грамматика Barsik
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System.Collections.Generic;

using Pidgin;
using Pidgin.Comment;

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
            Try (String ("/*")),
            Try (String ("*/"))
        );

    private static readonly Parser<char, Unit> LineComment = CommentParser.SkipLineComment
        (
            Try (String ("//"))
        );

    private static readonly Parser<char, Unit> Filler = Try (BlockComment)
        .Or (Try (LineComment))
        .Or (Whitespace.IgnoreResult())
        .SkipMany();

    public static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        token.Between (Filler);

    public static Parser<char, char> Tok (char token) => Tok (Char (token));

    public static Parser<char, string> Tok (string token) => Tok (String (token));

    public static Parser<char, TResult> CurlyBraces<TResult> (Parser<char, TResult> parser) =>
        Tok (parser).Between (Tok ('{'), Tok ('}'));

    public static Parser<char, TResult> RoundBrackets<TResult> (Parser<char, TResult> parser) =>
        Tok (parser).Between (Tok ('('), Tok (')'));

    internal static readonly Parser<char, string> Identifier = Map
        (
            (first, rest) => first + rest,
            Letter,
            (LetterOrDigit.Or (Char ('_'))).ManyString()
        );

    // копия
    private static readonly Parser<char, AtomNode> Expr = Rec (() => Rvalue.Expr);

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
    public static readonly Parser<char, IEnumerable<StatementNode>> Block = Map
        (
            (_, second) => second,
            Filler.Optional(),
            Rec (() => Statement!).SeparatedAndOptionallyTerminated (StatementDelimiter)
        );

    // стейтмент, вычисляющий значение выражения (костыль)
    private static readonly Parser<char, StatementNode> ExpressionStatement =
        Expr.Select<StatementNode> (v => new ExpressionNode (v));

    // TODO превратить в обычную функцию
    private static readonly Parser<char, StatementNode> Print = Map
        (
            (name, expressions) =>
                (StatementNode) new PrintNode (expressions, name == "println"),
            OneOf (Try (Tok ("println")), Try (Tok ("print"))),
            RoundBrackets (Tok (Expr).Separated (Tok (',')))
        );

    // цикл while
    private static readonly Parser<char, StatementNode> While = Map
        (
            (_, condition, body) =>
                (StatementNode) new WhileNode (condition, body),
            Try (Tok ("while")),
            RoundBrackets (Expr),
            CurlyBraces (Block)
        );

    // цикл for
    private static readonly Parser<char, StatementNode> For =
        from _1 in Tok ("for")
        from open1 in Tok ('(')
        from init in Tok (Expr)
        from _2 in Tok (';')
        from condition in Tok (Expr)
        from _3 in Token (';')
        from step in Tok (Expr)
        from close1 in Tok (')')
        from statements in CurlyBraces (Block)
        from elseBody in Rec (() => Tok (Else!)).Optional()
        select (StatementNode) new ForNode (init, condition, step, statements,
            elseBody.GetValueOrDefault());

    // цикл foreach
    private static readonly Parser<char, StatementNode> ForEach =
        from _1 in Tok ("foreach")
        from open1 in Tok ('(')
        from variableName in Tok (Identifier)
        from _2 in Tok ("in")
        from enumerable in Tok (Expr)
        from close1 in Tok (')')
        from statements in CurlyBraces (Block)
        select (StatementNode) new ForEachNode (variableName, enumerable, statements);

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
        from elseIf in Try (ElseIf).Many().Optional()
        from elseBlock in Else.Optional()
        select (StatementNode)new IfNode (condition, thenBlock, elseIf.GetValueOrDefault(),
            elseBlock.GetValueOrDefault());

    private static readonly Parser<char, CatchNode> Catch =
        from _ in Tok ("catch")
        from variable in RoundBrackets(Identifier)
        from block in CurlyBraces(Block)
        select new CatchNode (variable, block);

    private static readonly Parser<char, IEnumerable<StatementNode>> Finally =
        from _ in Tok ("finally")
        from block in CurlyBraces (Block)
        select block;

    private static readonly Parser<char, StatementNode> TryCatchFinally =
        from _ in Tok ("try")
        from tryBlock in CurlyBraces (Block)
        from catchNode in Catch.Optional()
        from finallyBlock in Finally.Optional()
        select (StatementNode) new TryNode (tryBlock, catchNode.GetValueOrDefault(),
            finallyBlock.GetValueOrDefault());

    // возврат из функции
    private static readonly Parser<char, StatementNode> Return =
        Tok ("return").Then (Tok (Expr).Optional())
            .Select<StatementNode> (v => new ReturnNode (v.GetValueOrDefault()));

    // внешний код
    private static readonly Parser<char, StatementNode> External = Map
        (
            (_, content, _) => (StatementNode)new ExternalNode (content),
            Char ('<'),
            AnyCharExcept ('>').ManyString(),
            Char ('>')
        );

    // определение функции
    private static readonly Parser<char, StatementNode> FunctionDefinition =
        from _ in Tok ("func")
        from name in Tok (Identifier)
        from args in RoundBrackets (Identifier.Separated (Tok (',')).Optional())
        from body in CurlyBraces (Block)
        select (StatementNode) new DefinitionNode (name, args.GetValueOrDefault(), body);

    // блок using
    private static readonly Parser<char, StatementNode> Using =
        from _ in Tok ("using")
        from open in Tok ('(')
        from variable in Tok (Identifier)
        from equal in Tok ('=')
        from initialization in Expr
        from close in Tok (')')
        from body in CurlyBraces (Block)
        select (StatementNode) new UsingNode (variable, initialization, body);

    // директива
    private static readonly Parser<char, StatementNode> Directive =
        Char('#').Then (Resolve.ReadLine()).Select<StatementNode> (line => new DirectiveNode (line));

    // обобщенный стейтмент
    private static readonly Parser<char, StatementNode> Statement = OneOf
        (
            Try (Directive),
            Try (Tok (If)),
            Try (Tok (Return)),
            Try (Tok (TryCatchFinally)),
            Try (Tok (ForEach)),
            Try (Tok (For)),
            Try (Tok (FunctionDefinition)),
            Try (Tok (While)),
            Try (Tok (Using)),
            Try (Tok (Print)),
            Try (Tok (External)),
            Try (Tok (ExpressionStatement))
        );

    //
    // Собственно программа
    //

    private static readonly Parser<char, ProgramNode> Pgm =
        Block.Before (End)
            .Select (s => new ProgramNode (s));

    #endregion
}
