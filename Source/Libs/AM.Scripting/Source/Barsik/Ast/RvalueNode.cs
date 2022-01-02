// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantSuppressNullableWarningExpression

/* RvalueNode.cs -- rvalue
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;

using static AM.Scripting.Barsik.Grammar;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Rvalue, т. е. то, что может появиться справа от знака присваивания.
/// </summary>
internal sealed class RvalueNode
{
    private static readonly Parser<char, AtomNode> NullLiteral =
        String ("null").ThenReturn ((AtomNode) new ConstantNode (null));

    private static readonly Parser<char, AtomNode> BoolLiteral =
        OneOf (String ("false"), String ("true"))
            .Select<AtomNode> (v => new ConstantNode (v == "true"));

    private static readonly Parser<char, AtomNode> CharLiteral =
        new EscapeParser ('\'', '\\')
            .Select (Resolve.UnescapeText)
            .Where (v => v?.Length == 1)
            .Select<AtomNode> (v => new ConstantNode (v![0]));

    private static readonly Parser<char, AtomNode> StringLiteral =
        new EscapeParser ('"', '\\')
            .Select (Resolve.UnescapeText)
            .Select<AtomNode> (v => new ConstantNode (v));

    private static readonly Parser<char, AtomNode> RawStringLiteral =
        Char ('@').Then(new EscapeParser ('"', '\\'))
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
                Try (RawStringLiteral),
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

    private static readonly Parser<char, AtomNode> Parenthesis = Map
            (
                (_, inner, _) => inner,
                Tok ('('),
                Rec (() => Expr!),
                Tok (')')
            )
        .Select<AtomNode> (inner => new ParenthesisNode (inner));

    // вызов свободной функции
    private static readonly Parser<char, AtomNode> FreeFunctionCall = Map
        (
            (name, args) =>
                (AtomNode) new FreeCallNode (name, args.GetValueOrDefault()),
            Tok (Identifier),
            RoundBrackets (Rec (() => Expr!).Separated (Tok (',')).Optional())
        );

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

    // инициализация списка
    private static readonly Parser<char, AtomNode> List = Tok (Map
        (
            (_, items, _) => (AtomNode) new ListNode (items),
            Tok ('['),
            Rec (() => Expr!).Separated (Tok (',')),
            Tok (']')
        ));

    // инициализация словаря
    private static readonly Parser<char, AtomNode> Dictionary = Tok (Map
        (
            (_, items, _) => (AtomNode) new DictionaryNode (items),
            Tok ('{'),
            Tok (KeyAndValue).Separated (Tok (',')),
            Tok ('}')
        ));

    // оператор new
    private static readonly Parser<char, AtomNode> New =
        from _ in Tok ("new")
        from typeName in Tok (Identifier)
        from args in
            RoundBrackets (Rec (() => Expr!).Separated (Tok (',')).Optional())
        select (AtomNode) new NewNode (typeName, args.GetValueOrDefault());

    // операция присваивания
    private static readonly Parser<char, string> Operation = OneOf
        (
            Tok ("="), Tok ("+="), Tok ("-="),
            Tok ("*="), Tok ("/="), Tok ("%="), Tok ("&="),
            Tok ("|="), Tok ("^="), Tok ("<<="), Tok (">>=")
        );

    // оператор присваивания
    // ReSharper disable RedundantSuppressNullableWarningExpression
    private static readonly Parser<char, AtomNode> Assignment = Map
        (
            (target, op, expression) =>
                (AtomNode) new AssignmentNode (target, op, expression),
            Tok (Identifier),
            Operation,
            Rec (() => Expr!)
        );
    // ReSharper restore RedundantSuppressNullableWarningExpression

    // оператор throw
    // ReSharper disable RedundantSuppressNullableWarningExpression
    private static readonly Parser<char, AtomNode> Throw = Map
        (
            (_, operand) => (AtomNode) new ThrowNode (operand),
            Tok ("throw"),
            Tok (Rec (() => Expr!))
        );
    // ReSharper restore RedundantSuppressNullableWarningExpression

    // обращение к свойству объекта
    private static readonly Parser<char, AtomNode> _Property =
        Tok ('.').Then (Identifier).Select<AtomNode> (v => new VariableNode (v));

    private static Parser<char, Func<AtomNode, AtomNode>> Property (Parser<char, AtomNode> op) =>
        op.Select<Func<AtomNode, AtomNode>> (type => v => new PropertyNode (v, type));

    // обращение к элементу по индексу
    // ReSharper disable RedundantSuppressNullableWarningExpression
    private static readonly Parser<char, AtomNode> _Index =
        Tok (Rec (() => Expr!)).Between (Tok ('['), Tok (']'))
            .Select (v => v);
    // ReSharper restore RedundantSuppressNullableWarningExpression

    private static Parser<char, Func<AtomNode, AtomNode>> Index (Parser<char, AtomNode> op) =>
        op.Select<Func<AtomNode, AtomNode>> (type => v => new IndexNode (v, type));

    // выражение
    internal static readonly Parser<char, AtomNode> Expr = ExpressionParser.Build
        (
            OneOf
                (
                    Try (Literal),
                    Try (New),
                    Try (Throw),
                    // Try (Ternary),
                    Try (FreeFunctionCall),
                    Try (List),
                    Try (Dictionary),
                    Try (Parenthesis),
                    Try (Assignment),
                    Try (Variable)
                ),
            new []
            {
                new []
                {
                    Operator.PostfixChainable
                        (
                            Try (Property (_Property)),
                            Try (Index (_Index))
                        ),
                },
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
                // TODO разобраться, почему не работает &&
                new [] { BinaryLeft ("&&"), BinaryLeft ("and") },
                new [] { BinaryLeft ("||"), BinaryLeft ("or") },
            }
        );

}
