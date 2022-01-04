// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantSuppressNullableWarningExpression

/* AssignmentNode.cs -- присваивание переменной результата вычисления выражения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections.Generic;

using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;

using static AM.Scripting.Barsik.Grammar;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Присваивание переменной результата вычисления выражения.
/// В т. ч. присваивание "в пустоту", т. е. вычисление без
/// сохранения результата в переменной.
/// </summary>
internal sealed class AssignmentNode
    : AtomNode
{
    #region Nested classes

    /// <summary>
    /// Временный узел с вызовом метода.
    /// </summary>
    internal class CallNode
    {
        public string Name { get; }
        public IEnumerable<AtomNode> Arguments { get; }

        public CallNode
            (
                string name,
                IEnumerable<AtomNode> arguments
            )
        {
            Name = name;
            Arguments = arguments;
        }
    }

    #endregion

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

    private static readonly Parser<char, AtomNode> RegexLiteral =
        new EscapeParser ('/', '\\')
            .Select<AtomNode> (v => new RegexNode (v));

    // 32-битное десятеричное со знаком, без префиксов и суффиксов
    private static readonly Parser<char, AtomNode> Int32Literal =
        Resolve.Int32 ()
        .Select<AtomNode> (v => new ConstantNode (v));

    // 64-битное десятеричное со знаком, без префикса, но с суффиксом "L"
    private static readonly Parser<char, AtomNode> Int64Literal =
        Resolve.Int64 (suffixes: new [] { "l", "L" })
        .Select<AtomNode> (v => new ConstantNode (v));

    // 32-битное десятеричное без знака, без префикса, но с суффиксом "U"
    private static readonly Parser<char, AtomNode> UInt32Literal =
        Resolve.UInt32 (suffixes: new [] { "u", "U" })
        .Select<AtomNode> (v => new ConstantNode (v));

    // 64-битное десятеричное без знака, без префикса, но с суффиксом "UL"
    private static readonly Parser<char, AtomNode> UInt64Literal =
        Resolve.UInt64 (suffixes: new [] { "lu", "ul", "LU", "UL" })
        .Select<AtomNode> (v => new ConstantNode (v));

    // 32-битное шестнадцатеричное без знака, префикс "0x", без суффикса
    private static readonly Parser<char, AtomNode> Hex32Literal =
        Resolve.UInt32 (16, "0x")
            .Select<AtomNode> (v => new ConstantNode (v));

    // 64-битное шестнадцатеричное без знака, префикс "0x", суффикс "L"
    private static readonly Parser<char, AtomNode> Hex64Literal =
        Resolve.UInt64 (16, "0x", new [] { "L", "l" })
        .Select<AtomNode> (v => new ConstantNode (v));

    // 32-битное двоичное без знака, префикс "0b", без суффикса
    private static readonly Parser<char, AtomNode> Bin32Literal =
        Resolve.UInt32 (2, "0b")
            .Select<AtomNode> (v => new ConstantNode (v));

    // 64-битное двоичное без знака, префикс "0b", суффикс "L"
    private static readonly Parser<char, AtomNode> Bin64Literal =
        Resolve.UInt64 (2, "0b", new [] { "L", "l" })
            .Select<AtomNode> (v => new ConstantNode (v));

    // число с плавающей точкой одинарной точности
    private static readonly Parser<char, AtomNode> FloatLiteral =
        Real.Before (OneOf ('F', 'f'))
            .Select (v => (AtomNode) new ConstantNode ((float) v));

    // число с плавающей точкой двойной точности
    private static readonly Parser<char, AtomNode> DoubleLiteral =
        Resolve.Double.Select<AtomNode> (v => new ConstantNode (v));

    // число с фиксированной точкой (денежное)
    private static readonly Parser<char, AtomNode> DecimalLiteral =
        Real.Before (OneOf ('M', 'm'))
            .Select (v => (AtomNode)new ConstantNode ((decimal) v));

    // все литералы, имеющиеся в Барсике
    private static readonly Parser<char, AtomNode> Literal = OneOf
        (
                Try (NullLiteral),
                Try (BoolLiteral),
                Try (CharLiteral),
                Try (RawStringLiteral),
                Try (RegexLiteral),
                Try (StringLiteral),
                Try (Hex64Literal),
                Try (Hex32Literal),
                Try (Bin64Literal),
                Try (Bin32Literal),
                Try (UInt64Literal),
                Try (Int64Literal),
                Try (UInt32Literal),
                Try (DecimalLiteral),
                Try (FloatLiteral),
                Try (DoubleLiteral),
                Try (Int32Literal)
            );

    // ключ и значение для словаря
    private static readonly Parser<char, KeyValueNode> KeyAndValue = Map
        (
            (key, _, value) => new KeyValueNode (key, value),
            Rec (() => Expr!),
            Tok (':'),
            Rec (() => Expr!)
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
        from typeName in Try (Tok (Rec (() => Expr!)))
        from args in
            RoundBrackets (Rec (() => Tok (Expr!)).Separated (Tok (',')).Optional())
        select (AtomNode) new NewNode (typeName, args.GetValueOrDefault());

    // операция присваивания
    private static readonly Parser<char, string> Operation = OneOf
        (
            Tok ("="),
            Tok ("+="),
            Tok ("-="),
            Tok ("*="), Tok ("/="), Tok ("%="), Tok ("&="),
            Tok ("|="), Tok ("^="), Tok ("<<="), Tok (">>=")
        );

    // оператор присваивания
    private static readonly Parser<char, AtomNode> Assignment = Map
        (
            (target, op, expression) =>
                (AtomNode) new AssignmentNode (target, op, expression),
            Tok (Rec (() => Target!)),
            Operation,
            Rec (() => Expr!)
        );

    // оператор throw
    private static readonly Parser<char, AtomNode> Throw = Map
        (
            (_, operand) => (AtomNode) new ThrowNode (operand),
            Tok ("throw"),
            Tok (Rec (() => Expr!))
        );

    // преобразование типа
    private static readonly Parser<char, AtomNode> _Cast =
        Try (RoundBrackets (Tok (Identifier)))
            .Select<AtomNode> (v => new VariableNode (v));

    private static Parser<char, Func<AtomNode, AtomNode>> Cast (Parser<char, AtomNode> op) =>
        op.Select<Func<AtomNode, AtomNode>> (type => v => new CastNode (type, v));

    // обращение к свойству объекта
    private static readonly Parser<char, AtomNode> _Property =
        Tok ('.').Then (Identifier).Select<AtomNode> (v => new VariableNode (v));

    private static Parser<char, Func<AtomNode, AtomNode>> Property (Parser<char, AtomNode> op) =>
        op.Select<Func<AtomNode, AtomNode>> (type => v => new PropertyNode (v, type));

    // обращение к элементу по индексу
    private static readonly Parser<char, AtomNode> _Index =
        Tok (Rec (() => Expr!)).Between (Tok ('['), Tok (']'))
            .Select (v => v);

    private static Parser<char, Func<AtomNode, AtomNode>> Index (Parser<char, AtomNode> op) =>
        op.Select<Func<AtomNode, AtomNode>> (type => v => new IndexNode (v, type));

    private static readonly Parser<char, CallNode> _MethodCall = Map
        (
            (_, name, _, args, _) => new CallNode (name, args),
            Tok ('.'),
            Tok (Identifier),
            Tok ('('),
            Try (Rec (() => Expr!)).Separated (Tok (',')),
            Tok (')')
        );

    //тернарный оператор
    private static readonly Parser<char, AtomNode> Ternary =
        from condition in Tok (Parenthesis)
        from question in Tok ('?')
        from trueValue in Tok (Rec (() => Expr!))
        from colon in Tok (':')
        from falseValue in Tok (Rec (() => Expr!))
        select (AtomNode) new TernaryNode (condition, trueValue, falseValue);

    private static Parser<char, Func<AtomNode, AtomNode>> MethodCall (Parser<char, CallNode> op) =>
        op.Select<Func<AtomNode, AtomNode>> (call => node => new MethodNode (node, call.Name, call.Arguments));

    // выражение
    internal static readonly Parser<char, AtomNode> Expr = ExpressionParser.Build
        (
            OneOf
                (
                    Try (Literal),
                    Try (New),
                    Try (Throw),
                    Try (Ternary),
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
                            Try (MethodCall (_MethodCall)),
                            Try (Property (_Property)),
                            Try (Index (_Index))
                        ),
                },
                new [] { BinaryLeft ("*"), BinaryLeft ("/"), BinaryLeft ("%") },
                new [] { Postfix ("++"), Postfix ("--") },
                new []
                    {
                        // TODO разобраться, почему не работает --
                        // Prefix ("++"), Prefix ("--"),
                        Prefix ("-"),
                        Prefix ("!")
                    },
                new [] { Operator.Prefix (Cast (_Cast)) },
                new [] { BinaryLeft ("is") },
                new [] { BinaryLeft ("+"), BinaryLeft ("-") },
                new [] { BinaryLeft ("<<"), BinaryLeft (">>") },
                new [] { BinaryLeft ("<="), BinaryLeft (">="), BinaryLeft ("<"), BinaryLeft (">") },
                new [] { BinaryLeft ("==="), BinaryLeft ("!="), BinaryLeft ("==") },
                new [] { BinaryLeft ("&") },
                new [] { BinaryLeft ("^") },
                new [] { BinaryLeft ("|") },
                new [] { BinaryLeft ("~") },
                new [] { BinaryLeft ("in") },
                // TODO разобраться, почему не работает &&
                new [] { BinaryLeft ("&&"), BinaryLeft ("and") },
                new [] { BinaryLeft ("||"), BinaryLeft ("or") },
            }
        );

    // цель присваивания
    internal static readonly Parser<char, AtomNode> Target = ExpressionParser.Build
        (
            Variable,
            new []
            {
                Operator.PostfixChainable
                    (
                        Try (MethodCall (_MethodCall)),
                        Try (Property (_Property)),
                        Try (Index (_Index))
                    )
            }
        );

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AssignmentNode
        (
            AtomNode target,
            string operation,
            AtomNode expression
        )
    {
        Sure.NotNull (target);
        Sure.NotNullNorEmpty (operation);
        Sure.NotNull (expression);

        if (Array.IndexOf (BarsikUtility.Keywords, target) >= 0)
        {
            throw new BarsikException ($"Name {target} is reserved");
        }

        _target = target;
        _operation = operation;
        _expression = expression;
    }

    #endregion

    #region Private members

    private readonly AtomNode _target;
    private readonly string _operation;
    private readonly AtomNode _expression;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var value = _expression.Compute (context);
        value = _target.Assign (context, _operation, value);

        return value;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"Assignment: {_target} = {_expression};";
    }

    #endregion
}
