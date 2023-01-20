// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantSuppressNullableWarningExpression
// ReSharper disable StaticMemberInitializerReferesToMemberBelow

/* Grammar.cs -- грамматика языка
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Грамматика языка.
/// </summary>
public static class Grammar
{
    #region Private members

    private static Parser<StatementBase> BuildStatement (Parser<ExpressionNode> innerParser) =>
        Parser.Chain
            (
                Parser.Position,
                innerParser,
                (pos, x) => (StatementBase)new SimpleStatement (pos.Line, x)
            );

    #endregion

    #region Public methods and properties

    /// <summary>
    /// Порождение константного узла.
    /// </summary>
    private static readonly Parser<AtomNode> Literal = new LiteralParser().Map
        (
            x => (AtomNode)new ConstantNode (x)
        );

    /// <summary>
    /// Разбор перечисленных терминов.
    /// </summary>
    public static TermParser Term (params string[] terms) => new (terms);

    /// <summary>
    /// Разбор зарезервированного слова.
    /// </summary>
    public static ReservedWordParser Reserved (string? word) => new (word);

    /// <summary>
    /// Разбор идентификаторов.
    /// </summary>
    public static readonly IdentifierParser Identifier = new ();

    /// <summary>
    /// Ссылка на переменную.
    /// </summary>
    private static readonly Parser<AtomNode> Variable = Identifier.Map
            (
                x => (AtomNode)new VariableNode (x)
            )
        .Labeled ("Variable");

    /// <summary>
    /// Корневое выражение.
    /// </summary>
    private static readonly Parser<AtomNode> Atom = Parser.OneOf
            (
                Literal,
                Parser.Lazy (() => FunctionCall!),
                Variable,
                Parser.Lazy (() => List!),
                Parser.Lazy (() => Dictionary!),
                Parser.Lazy (() => New!)
            )
        .Labeled ("Atom");

    /// <summary>
    /// Выражение, стоящее слева от знака присваивания.
    /// </summary>
    private static readonly Parser<AtomNode> LeftExpression = ExpressionBuilder.Build
        (
            Parser.Lazy (() => Atom),
            // префиксные операции не предусмотрены
            Array.Empty<Parser<Func<AtomNode, AtomNode>>>(),
            new[]
            {
                // постфиксные операции
                Parser.Lazy (() => Index!),
                Parser.Lazy (() => Property!)
            },
            // инфиксные операции не предусмотрены
            Array.Empty<InfixOperator<AtomNode>>()
        );

    /// <summary>
    /// Базовое выражение.
    /// </summary>
    private static readonly Parser<AtomNode> BasicExpression = ExpressionBuilder.Build
        (
            Parser.Lazy (() => Atom),
            new[]
            {
                // префиксные операции
                Operator.Unary
                    (
                        Term ("-"),
                        "UnaryMinus",
                        _ => target => new MinusNode (target)
                    ),

                Operator.Unary
                    (
                        Term ("!"),
                        "Bang",
                        _ => target => new BangNode (target)
                    ),

                Operator.Unary
                    (
                        Term ("~"),
                        "Tilda",
                        _ => target => new TildaNode (target)
                    ),

                Operator.Increment ("PrefixIncrement", true),

                Operator.Unary
                    (
                        Parser.OneOf (Identifier, Reserved (null))
                            .RoundBrackets(),
                        "Cast",
                        x => target => new CastNode (x, target)
                    )
            },
            new[]
            {
                // постфиксные операции
                Operator.Increment ("PostfixIncrement", false),

                Parser.Lazy (() => Index!),

                Parser.Lazy (() => MethodCall!).Labeled ("MethodCall"),

                Parser.Lazy (() => Property!).Labeled ("Property"),
            },
            new[]
            {
                // бинарные операции
                Operator.LeftAssociative ("Shift", "<<", ">>"),
                Operator.LeftAssociative ("Bitwise", "&", "|", "^"),
                Operator.LeftAssociative ("Multiplication", "*", "/", "%" ),
                Operator.LeftAssociative ("Addition", "+", "-" ),
                Operator.LeftAssociative ("Comparison", "<", ">", "<=", ">=", "==", "!=" )
            }
        )
        .Labeled ("BasicExpression");

    /// <summary>
    /// Выражение без присваивания.
    /// </summary>
    private static readonly Parser<ExpressionNode> Expression = BasicExpression.Map
        (
            x => new ExpressionNode (null, null, x)
        )
        .Labeled ("Expression");

    /// <summary>
    /// Имя типа в формате `Namespace.Subspace.Typename`.
    /// </summary>
    private static readonly Parser<string> TypeName =
        (
            Identifier.SeparatedBy
                (
                    Term ("."),
                    minCount: 1
                )
            .Map (x => string.Join ('.', x))
        )
        .Or (new LiteralParser().Map (x => (string)x))
        .Labeled ("TypeName");

    /// <summary>
    /// Оператор new.
    /// </summary>
    private static readonly Parser<AtomNode> New = Parser.Chain
            (
                Reserved ("new"),
                TypeName,
                Expression.SeparatedBy (Term (",")).RoundBrackets(),
                (_, _2, _3) =>
                    (AtomNode)new NewNode (_2, _3.ToArray())
            )
        .Labeled ("New");

    /// <summary>
    /// Пара "ключ и значение" для словаря.
    /// </summary>
    private static readonly Parser<KeyValueNode> KeyAndValue = Parser.Chain
            (
                Expression.Instance ("Key"), // 1
                Term (":"), // 2
                Expression.Instance ("Value"), // 3
                (_1, _, _3) => new KeyValueNode (_1, _3)
            )
        .Labeled ("KeyAndValue");

    /// <summary>
    /// Словарь вида `{1: "one", 2: "two", 3: "three"}`.
    /// </summary>
    private static readonly Parser<AtomNode> Dictionary = KeyAndValue
        .SeparatedBy (Term (","))
        .CurlyBrackets()
        .Labeled ("Dictionary")
        .Map (x => (AtomNode)new DictionaryNode (x));

    /// <summary>
    /// Список вида `[1, 2, 3]`.
    /// </summary>
    private static readonly Parser<AtomNode> List = Expression
        .SeparatedBy (Term (","))
        .SquareBrackets()
        .Labeled ("List")
        .Map (x => (AtomNode)new ListNode (x));

    /// <summary>
    /// Присваивание.
    /// </summary>
    private static readonly Parser<ExpressionNode> Assignment = Parser.Chain
            (

                // x1 = x2 = ...
                new RepeatParser<Tuple<AtomNode, string>>
                    (
                        Parser.Chain
                            (
                                LeftExpression,
                                Parser.Term ("=", "+=", "-=", "*=", "/="),
                                Tuple.Create
                            ),
                        minCount: 1
                    ),
                BasicExpression,
                (tuples, e) =>
                {
                    var expr = e;

                    foreach (var tuple in tuples)
                    {
                        expr = new ExpressionNode (tuple.Item1, tuple.Item2, expr);
                    }

                    return (ExpressionNode)expr;
                }
            )
        .Labeled ("Assignment");

    /// <summary>
    /// Обращение к свойству объекта.
    /// </summary>
    private static readonly Parser<Func<AtomNode, AtomNode>> Property =
        Operator.Unary
            (
                Identifier.After (Term (".")),
                "Property",
                name => target => new PropertyNode (target, name)
            );

    /// <summary>
    /// Обращение к элементу массива/списка по индексу.
    /// </summary>
    private static readonly Parser<Func<AtomNode, AtomNode>> Index =
        Operator.Unary
            (
                Parser.Lazy (() => Expression!).SquareBrackets(),
                "Index",
                x => target => new IndexNode (target, x)
            );

    /// <summary>
    /// Вызов метода объекта.
    /// </summary>
    private static readonly Parser<Func<AtomNode, AtomNode>> MethodCall =
        Parser.Chain<string, string, IEnumerable<ExpressionNode>, Func<AtomNode, AtomNode>>
                (
                    Term ("."),
                    Identifier,
                    Parser.Lazy (() => Expression!)
                        .SeparatedBy (Term (","))
                        .RoundBrackets(),
                    (_, name, args) => target => new MethodNode (target, name, args.ToArray())
                )
            .Labeled ("MethodCall");

    /// <summary>
    /// Вызов свободной функции, например, `println`.
    /// </summary>
    private static readonly Parser<AtomNode> FunctionCall =
        Parser.Chain
                (
                    Identifier,
                    Parser.Lazy (() => Expression!)
                        .SeparatedBy (Term (","))
                        .RoundBrackets(),
                    (name, args) =>
                        (AtomNode)new CallNode (name, args.ToArray())
                )
            .Labeled ("FunctionCall");

    /// <summary>
    /// Простой стейтмент.
    /// </summary>
    private static readonly Parser<StatementBase> SimpleStatement = Parser.OneOf
            (
                BuildStatement (Assignment),
                BuildStatement (Expression)
            )
        .Labeled ("SimpleStatement");

    /// <summary>
    /// Блок стейтментов.
    /// </summary>
    private static readonly Parser<StatementBase> Block = Parser.Lazy
            (
                () => Parser.OneOf
                    (
                        Parser.Chain
                            (
                                Parser.Position,
                                GenericStatement!.Repeated (minCount: 1).CurlyBrackets(),
                                (pos, lines) =>
                                    (StatementBase)new Block (pos.Line, lines.ToArray())
                            ),
                        SimpleStatement.Map
                            (
                                x => (StatementBase)new Block (x.Line, new[] { x })
                            )
                    )
            )
        .Labeled ("Block");

    /// <summary>
    /// Цикл for.
    /// </summary>
    private static readonly Parser<StatementBase> ForStatement = Parser.Chain
            (
                Parser.Position, // 1
                Parser.Reserved ("for"), // 2
                Parser.Term ("("), // 3
                Assignment.Instance ("Init"), // 4
                Parser.Term (";"), // 5
                Expression.Instance ("Condition"), // 6
                Parser.Term (";"), // 7
                Assignment.Or (Expression).Labeled ("Step"), // 8
                Parser.Term (")"), // 9
                Block.Instance ("Body"), // 10
                (_1, _, _, _4, _, _6, _, _8, _, _10) =>
                    (StatementBase)new ForNode (_1.Line, _4, _6, _8, (Block)_10)
            )
        .Labeled ("For");

    /// <summary>
    /// Цикл while.
    /// </summary>
    private static readonly Parser<StatementBase> WhileStatement = Parser.Chain
            (
                Parser.Position, // 1
                Parser.Reserved ("while"), // 2
                Expression.Instance ("Condition").RoundBrackets(), // 3
                Block.Instance ("Body"), // 4
                (_1, _, _3, _4) =>
                    (StatementBase)new WhileNode (_1.Line, _3, (Block)_4)
            )
        .Labeled ("While");

    /// <summary>
    /// Условный оператор if-then-else.
    /// </summary>
    private static readonly Parser<StatementBase> IfStatement = Parser.Chain
            (
                Parser.Position, // 1
                Parser.Reserved ("if"), // 2
                Parser.Term ("("), // 3
                Expression.Instance ("Condition"), // 4
                Parser.Term (")"), // 5
                Block.Instance ("Then"), // 6
                Block.Instance ("Else").After (Parser.Reserved ("else")).Optional(), // 7
                (_1, _, _, _4, _, _6, _7) =>
                    (StatementBase)new IfNode (_1.Line, _4, (Block)_6, (Block)_7)
            )
        .Labeled ("If");

    /// <summary>
    /// Блок using.
    /// </summary>
    private static readonly Parser<StatementBase> UsingStatement = Parser.Chain
            (
                Parser.Position, // 1
                Parser.Reserved ("using"), // 2
                Parser.Term ("("), // 3
                Parser.Identifier, // 4
                Parser.Term ("="), // 5
                Expression.Instance ("Init"), // 6
                Parser.Term (")"), // 7
                Block.Instance ("Body"), // 8
                (_1, _, _, _4, _, _6, _, _8) =>
                    (StatementBase)new UsingNode (_1.Line, _4, _6, (Block)_8)
            )
        .Labeled ("Using");

    private static readonly Parser<StatementBase> FunctionDefinition = Parser.Chain
        (
            Parser.Position, // 1
            Reserved ("func"), // 2
            Identifier, // 3
            Identifier.SeparatedBy (Term (",")).RoundBrackets(), // 4
            Block, // 5
            (_1, _, _3, _4, _5) =>
                (StatementBase) new FunctionDefinitionNode (_1.Line, _3, _4.ToArray(), _5)
        )
        .Labeled ("FunctionDefinition");

    /// <summary>
    /// Стейтмент вообще.
    /// </summary>
    private static readonly Parser<StatementBase> GenericStatement = Parser.Lazy
            (
                () =>
                    Parser.OneOf
                            (
                                SimpleStatement,
                                ForStatement,
                                WhileStatement,
                                IfStatement,
                                UsingStatement,
                                FunctionDefinition
                            )
                        .Labeled ("StatementKind")
            )
        .Labeled ("GenericStatement");

    /// <summary>
    /// Программа в целом.
    /// </summary>
    private static readonly Parser<ProgramNode> Program = new RepeatParser<StatementBase>
            (
                GenericStatement
            )
        .Labeled ("Statements")
        .Map (x => new ProgramNode (x))
        .End()
        .Labeled ("Program");

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текста выражения.
    /// </summary>
    public static AtomNode ParseExpression
        (
            string sourceCode,
            TextWriter? debugOutput = null
        )
    {
        Sure.NotNull (sourceCode);

        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (sourceCode);
        var state = new ParseState (tokens)
        {
            DebugOutput = debugOutput
        };
        var result = Expression.End().ParseOrThrow (state);

        return result;
    }

    /// <summary>
    /// Разбор программы.
    /// </summary>
    public static ProgramNode ParseProgram
        (
            string sourceText,
            TextWriter? debugOutput = null
        )
    {
        Sure.NotNull (sourceText);

        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (sourceText);
        var state = new ParseState (tokens)
        {
            DebugOutput = debugOutput
        };
        var result = Program.ParseOrThrow (state);

        return result;
    }

    #endregion
}
