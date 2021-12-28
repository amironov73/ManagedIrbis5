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

    private static readonly Parser<char, AtomNode> StringLiteral =
        new EscapeParser ('"', '\\')
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
        Resolve.Double.Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> Literal = OneOf (
                Try (NullLiteral),
                Try (BoolLiteral),
                Try (CharLiteral),
                Try (StringLiteral),
                Try (Hex64Literal),
                Try (Hex32Literal),
                Try (UInt64Literal),
                Try (Int64Literal),
                Try (UInt32Literal),
                Try (DecimalLiteral),
                Try (FloatLiteral),
                Try (DoubleLiteral),
                Try (Int32Literal)
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

    // вызов свободной функции
    // ReSharper disable RedundantSuppressNullableWarningExpression
    private static readonly Parser<char, AtomNode> FreeFunctionCall = Map
        (
            (name, args) =>
                (AtomNode) new FreeCallNode (name, args.GetValueOrDefault()),
            Tok (Identifier),
            RoundBrackets (Rec (() => Expr!).Separated (Tok (',')).Optional())
        );
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
        Operator.InfixL (Binary (Try (Tok (op))));

    // ReSharper disable RedundantSuppressNullableWarningExpression
    private static readonly Parser<char, AtomNode> List = Tok (Map
        (
            (_, items, _) => (AtomNode) new ListNode (items),
            Tok ('['),
            Rec (() => Expr!).Separated (Tok (',')),
            Tok (']')
        ));
    // ReSharper restore RedundantSuppressNullableWarningExpression

    // инициализация словаря
    private static readonly Parser<char, AtomNode> Dictionary = Tok (Map
        (
            (_, items, _) => (AtomNode) new DictionaryNode (items),
            Tok ('{'),
            KeyAndValue.Separated (Tok (',')),
            Tok ('}')
        ));

    // оператор new
    private static readonly Parser<char, AtomNode> New =
        from _ in Tok ("new")
        from typeName in Tok (Identifier)
        from args in
            RoundBrackets (Rec (() => Expr!).Separated (Tok (',')).Optional())
        select (AtomNode) new NewNode (typeName, args.GetValueOrDefault());

    // тернарный оператор
    // TODO устранить левую рекурсию
    // private static readonly Parser<char, AtomNode> Ternary =
    //     from condition in Tok (Rec (() => Expr!))
    //     from question in Tok ('?')
    //     from trueValue in Tok (Rec (() => Expr!))
    //     from colon in Tok (':')
    //     from falseValue in Tok (Rec (() => Expr!))
    //     select (AtomNode) new TernaryNode (condition, trueValue, falseValue);

    // выражение
    private static readonly Parser<char, AtomNode> Expr = ExpressionParser.Build
        (
            OneOf
                (
                    Try (Literal),
                    Try (New),
                    // Try (Ternary),
                    Try (FreeFunctionCall),
                    Try (Variable),
                    Try (List),
                    Try (Dictionary),
                    Try (Parenthesis)
                ),
            new []
            {
                new [] { BinaryLeft ("*"), BinaryLeft ("/"), BinaryLeft ("%") },
                new [] { Postfix ("++"), Postfix ("--") },
                new [] { Prefix ("++"), Prefix ("--"), Prefix ("!"), Prefix ("-") },
                new [] { Prefix ("!")},
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
    public static readonly Parser<char, IEnumerable<StatementNode>> Block = Map
        (
            (_, second) => second,
            Filler.Optional(),
            Rec (() => Statement!).SeparatedAndOptionallyTerminated (StatementDelimiter)
        );
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

    private static readonly Parser<char, StatementNode> Print = Map
        (
            (name, expressions) =>
                (StatementNode) new PrintNode (expressions, name == "println"),
            OneOf (Try (Tok ("println")), Try (Tok ("print"))),
            RoundBrackets (Tok (Expr).Separated (Tok (',')))
        );

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
        from init in Assignment
        from _2 in Tok (';')
        from condition in Expr
        from _3 in Token (';')
        from step in Tok (Assignment)
        from close1 in Tok (')')
        from statements in CurlyBraces (Block)
        from elseBody in Rec (() => Else!).Optional()
        select (StatementNode) new ForNode (init, condition, step, statements,
            elseBody.GetValueOrDefault());

    // цикл foreach
    private static readonly Parser<char, StatementNode> ForEach =
        from _1 in Tok ("foreach")
        from open1 in Tok ('(')
        from variableName in Tok (Identifier)
        from _2 in Tok ("in")
        from enumerable in Expr
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
        //from elseIf in ElseIf.Many().Optional()
        from elseBlock in Else.Optional()
        //select (StatementNode)new IfNode (condition, thenBlock, elseIf.GetValueOrDefault(),
        //    elseBlock.GetValueOrDefault());
        select (StatementNode)new IfNode (condition, thenBlock, null, elseBlock.GetValueOrDefault());

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

    private static readonly Parser<char, StatementNode> External = Map
        (
            (_, content, _) => (StatementNode)new ExternalNode (content),
            Char ('<'),
            AnyCharExcept ('>').ManyString(),
            Char ('>')
        );

    // оператор throw
    // TODO переделать на Expr
    private static readonly Parser<char, StatementNode> Throw = Map
        (
            (_, operand) => (StatementNode)new ThrowNode (operand),
            Tok ("throw"),
            Expr
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

    // обобщенный стейтмент
    private static readonly Parser<char, StatementNode> Statement = OneOf
        (
            Try (Tok (If)),
            Try (Tok (TryCatchFinally)),
            Try (Tok (ForEach)),
            Try (Tok (For)),
            Try (Tok (FunctionDefinition)),
            Try (Tok (While)),
            Try (Tok (Using)),
            Try (Tok (Print)),
            Try (Tok (External)),
            Try (Tok (Throw)),
            Try (Tok (Assignment))
        );

    //
    // Собственно программа
    //

    private static readonly Parser<char, ProgramNode> Pgm =
        Block.Before (End)
            .Select (s => new ProgramNode (s));

    #endregion
}
