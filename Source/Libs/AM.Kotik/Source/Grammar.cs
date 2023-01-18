// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StaticMemberInitializerReferesToMemberBelow

/* Grammar.cs -- грамматика языка
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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
    public static ReservedWordParser Reserved (string word) => new (word);

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
                Variable,
                Parser.Lazy (() => List!),
                Parser.Lazy (() => Dictionary!),
                Parser.Lazy (() => New!)
            )
        .Labeled ("Atom");

    /// <summary>
    /// Базовое выражение.
    /// </summary>
    private static readonly Parser<AtomNode> BasicExpression = ExpressionBuilder.Build
            (
                Atom,
                new Parser<Func<AtomNode, AtomNode>>[0],
                new []
                {
                    Term ("++", "--").Map<string, Func<AtomNode, AtomNode>>
                        (
                            x => target => new IncrementNode (target, x, false)
                        ),

                    Parser.Lazy (() => Cast!).Map<string, Func<AtomNode, AtomNode>>
                        (
                            x => target => new CastNode (x, target)
                        ),

                    Parser.Lazy (() => Index!).Map<ExpressionNode, Func<AtomNode, AtomNode>>
                        (
                            x => target => new IndexNode (target, x)
                        )
                },
                new[]
                {
                    new[] { "<<", ">>" },
                    new[] { "&", "|" },
                    new[] { "*", "/", "%" },
                    new[] { "+", "-" },
                    new[] { "<", ">", "<=", ">=", "==", "!=" },
                },
                (left, operation, right) => new BinaryNode (left, operation, right)
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
    private static readonly Parser<string> TypeName = (Identifier.SeparatedBy
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
                new RepeatParser<Tuple<string, string>>
                    (
                        Parser.Chain
                            (
                                Identifier,
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
                        expr = new ExpressionNode (new VariableNode (tuple.Item1), tuple.Item2, expr);
                    }

                    return (ExpressionNode)expr;
                }
            )
        .Labeled ("Assignment");

    /// <summary>
    /// Преобразование типа.
    /// </summary>
    private static readonly Parser<string> Cast = Identifier
        .RoundBrackets()
        .Labeled ("Cast");

    /// <summary>
    /// Обращение по индексу
    /// </summary>
    private static readonly Parser<ExpressionNode> Index = Expression
        .SquareBrackets()
        .Labeled ("Index");

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
                (_1, _2, _3, _4) =>
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
                (_1, _, _3, _4, _, _6, _, _8) =>
                    (StatementBase)new UsingNode (_1.Line, _4, _6, (Block)_8)
            )
        .Labeled ("Using");

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
                                UsingStatement
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

    /// <summary>
    /// Разбор программы.
    /// </summary>
    public static ProgramNode ParseProgram
        (
            string sourceText
        )
    {
        Sure.NotNull (sourceText);

        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (sourceText);
        var state = new ParseState (tokens)
        {
            DebugOutput = Console.Out
        };
        var result = Program.ParseOrThrow (state);

        return result;
    }

    #endregion
}
