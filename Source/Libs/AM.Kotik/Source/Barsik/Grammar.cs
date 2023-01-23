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

namespace AM.Kotik.Barsik;

/// <summary>
/// Грамматика языка.
/// </summary>
public static class Grammar
{
    #region Private members

    private static Parser<StatementBase> BuildStatement (Parser<AtomNode> innerParser) =>
        Parser.Chain
            (
                Parser.Position,
                innerParser,
                (pos, x) => (StatementBase) new SimpleStatement (pos.Line, x)
            );

    #endregion

    #region Public methods and properties

    /// <summary>
    /// Порождение константного узла.
    /// </summary>
    private static readonly Parser<AtomNode> Literal = new LiteralParser().Map
        (
            x => (AtomNode) new ConstantNode (x)
        );

    /// <summary>
    /// Форматная строка.
    /// </summary>
    private static readonly Parser<AtomNode> Format = new FormatParser().Map
        (
            x => (AtomNode) new FormatNode (x)
        );

    /// <summary>
    /// Разбор перечисленных терминов.
    /// </summary>
    private static TermParser Term (params string[] terms) => new (terms);

    /// <summary>
    /// Разбор зарезервированного слова.
    /// </summary>
    /// <param name="word"><c>null</c> означает "любое зарезервированное слово".
    /// </param>
    private static ReservedWordParser Reserved (string? word) => new (word);

    /// <summary>
    /// Разбор идентификаторов.
    /// </summary>
    private static readonly IdentifierParser Identifier = new ();

    /// <summary>
    /// Ссылка на переменную.
    /// </summary>
    private static readonly Parser<AtomNode> Variable = Identifier.Map
            (
                x => (AtomNode) new VariableNode (x)
            )
        .Labeled ("Variable");

    /// <summary>
    /// Корневое выражение.
    /// </summary>
    private static readonly Parser<AtomNode> Atom = Parser.OneOf
            (
                Literal,
                Format,
                Parser.Lazy (() => Ternary!),
                Parser.Lazy (() => FunctionCall!),
                Variable,
                Parser.Lazy (() => List!),
                Parser.Lazy (() => Dictionary!),
                Parser.Lazy (() => New!),
                Parser.Lazy (() => Throw!),
                Parser.Lazy (() => Lambda!)
            )
        .Labeled ("Atom");

    /// <summary>
    /// Тернарный оператор `? условие : истина : ложь`.
    /// </summary>
    private static readonly Parser<AtomNode> Ternary = Parser.Chain
        (
            Term ("?"),
            Parser.Lazy (() => Expression!), // condition
            Term (":"),
            Parser.Lazy (() => Expression!), // trueValue
            Term (":"),
            Parser.Lazy (() => Expression!), // falseValue
            (_, condition, _, trueValue, _, falseValue) =>
                (AtomNode) new TernaryNode (condition, trueValue, falseValue)
        )
        .Labeled ("Ternary");

    /// <summary>
    /// Выражение, стоящее слева от знака присваивания.
    /// </summary>
    private static readonly Parser<AtomNode> LeftHand = ExpressionBuilder.Build
        (
            root: Parser.Lazy (() => Atom),

            // префиксные операции не предусмотрены
            prefixOps: Array.Empty<Parser<Func<AtomNode, AtomNode>>>(),

            postfixOps: new[]
            {
                // постфиксные операции
                Parser.Lazy (() => Index!),
                Parser.Lazy (() => Property!)
            },

            // инфиксные операции не предусмотрены
            infixOps: Array.Empty<InfixOperator<AtomNode>>()
        )
        .Labeled ("LeftHand");

    /// <summary>
    /// Правая часть оператора присваивания.
    /// </summary>
    private static readonly Parser<AtomNode> Expression = ExpressionBuilder.Build
        (
            root: Parser.Lazy (() => Atom),

            prefixOps: new[]
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
                        "PrefixBang",
                        _ => target => new PrefixBangNode (target)
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

            postfixOps: new[]
            {
                // постфиксные операции
                Operator.Increment ("PostfixIncrement", false),

                Operator.Unary
                    (
                        Term ("!"),
                        "PostfixBang",
                        _ => target => new PostfixBangNode (target)
                    ),

                Parser.Lazy (() => Index!),

                Parser.Lazy (() => MethodCall!).Labeled ("MethodCall"),

                Parser.Lazy (() => Property!).Labeled ("Property"),
            },

            infixOps: new[]
            {
                // инфиксные операции
                Operator.NonAssociative ("Shuttle", "<=>"),
                Operator.NonAssociative ("In/is", "in", "is"),
                Operator.LeftAssociative ("Coalesce", "??"),
                Operator.LeftAssociative ("Shift", "<<", ">>"),
                Operator.LeftAssociative ("Bitwise", "&", "|", "^"),
                Operator.LeftAssociative ("Multiplication", "*", "/", "%" ),
                Operator.LeftAssociative ("Addition", "+", "-" ),
                Operator.LeftAssociative ("Comparison", "<", ">", "<=", ">=", "==", "!=", "<>",
                    "===", "!==", "~~", "~~~" )
            }
        )
        .Labeled ("Expression");

    /// <summary>
    /// Именованный аргумент функции.
    /// </summary>
    private static readonly Parser<AtomNode> NamedArgument = Parser.Chain
        (
            Identifier,
            Term (":"),
            Parser.Lazy (() => Expression),
            (name, _, expr) => (AtomNode) new NamedArgumentNode (name, expr)
        )
        .Labeled ("NamedArg");

    /// <summary>
    /// Имя типа в формате `Namespace.Subspace.Typename`.
    /// </summary>
    private static readonly Parser<string> TypeName =
        Identifier.SeparatedBy (Term ("."), minCount: 1)
            .Map (x => string.Join ('.', x))
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
                (AtomNode) new NewNode (_2, _3.ToArray())
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
        .Map (x => (AtomNode) new DictionaryNode (x.ToArray()));

    /// <summary>
    /// Список вида `[1, 2, 3]`.
    /// </summary>
    private static readonly Parser<AtomNode> List = Expression
        .SeparatedBy (Term (","))
        .SquareBrackets()
        .Labeled ("List")
        .Map (x => (AtomNode) new ListNode (x.ToArray()));

    /// <summary>
    /// Присваивание.
    /// </summary>
    private static readonly Parser<AtomNode> Assignment = Parser.Chain
        (
            new RepeatParser<Tuple<AtomNode, string>>
                (
                    // x1 = x2 = ...
                    Parser.Chain
                        (
                            LeftHand,
                            Parser.Term ("=", "+=", "-=", "*=", "/="),
                            Tuple.Create
                        ),
                    minCount: 0
                ),

            Expression,

            (tuples, expr) =>
            {
                // TODO присваивание должно идти в обратном порядке
                foreach (var tuple in tuples)
                {
                    expr = new ExpressionNode (tuple.Item1, tuple.Item2, expr);
                }

                return expr;
            }
        )
        .Labeled ("Assignment");

    /// <summary>
    /// Обращение к свойству объекта.
    /// </summary>
    private static readonly Parser<Func<AtomNode, AtomNode>> Property = Operator.Unary
        (
            Identifier.After (Term (".")),
            "Property",
            name => target => new PropertyNode (target, name)
        );

    /// <summary>
    /// Обращение к элементу массива/списка по индексу.
    /// </summary>
    private static readonly Parser<Func<AtomNode, AtomNode>> Index = Operator.Unary
        (
            Parser.Lazy (() => Expression!).SquareBrackets(),
            "Index",
            x => target => new IndexNode (target, x)
        );

    /// <summary>
    /// Вызов метода объекта.
    /// </summary>
    private static readonly Parser<Func<AtomNode, AtomNode>> MethodCall =
        Parser.Chain<string, string, IEnumerable<AtomNode>, Func<AtomNode, AtomNode>>
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
    private static readonly Parser<AtomNode> FunctionCall = Parser.Chain
        (
            Identifier,
            Parser.Lazy (() => Expression!.Or (NamedArgument!))
                .SeparatedBy (Term (","))
                .RoundBrackets(),
            (name, args) =>
                (AtomNode) new CallNode (name, args.ToArray())
        )
        .Labeled ("FunctionCall");

    /// <summary>
    /// Определение лямбда-функции
    /// </summary>
    private static readonly Parser<AtomNode> Lambda = Parser.Chain
        (
            Reserved ("lambda"),
            Identifier.SeparatedBy (Term (",")).RoundBrackets(),
            Parser.Lazy (() => Block!),
            (_, args, body) => (AtomNode) new LambdaNode (args.ToArray(), body)
        )
        .Labeled ("Lambda");

    /// <summary>
    /// Оператор throw.
    /// </summary>
    private static readonly Parser<AtomNode> Throw = Parser.Lazy (() => Atom!)
        .RoundBrackets()
        .After (Reserved ("throw"))
        .Map (x => (AtomNode) new ThrowNode (x))
        .Labeled ("Throw");

    /// <summary>
    /// Блок catch.
    /// </summary>
    private static readonly Parser<TryNode.CatchBlock> CatchClause = Parser.Chain
        (
            Identifier.RoundBrackets().After (Reserved ("catch")), // name
            Parser.Lazy (() => Block!), // body
            (name, body) => new TryNode.CatchBlock (name, body)
        )
        .Optional();

    /// <summary>
    /// Блок try-catch-finally.
    /// </summary>
    private static readonly Parser<StatementBase> TryCatchFinally = Parser.Chain
        (
            Parser.Position.Before (Reserved ("try")), // position
            Parser.Lazy (() => Block!), // tryBlock,
            CatchClause, // catchBlock
            Parser.Lazy (() => Block!).After (Reserved ("finally"))
                .Optional(), // finallyBlock
            (position, tryBlock, catchBlock, finallyBlock) => (StatementBase)
                new TryNode (position.Line, tryBlock, catchBlock, finallyBlock)
        )
        .Labeled ("TryCatchFinally");

    /// <summary>
    /// Простой стейтмент.
    /// </summary>
    private static readonly Parser<StatementBase> SimpleStatement =
        BuildStatement (Assignment).Labeled ("SimpleStatement");

    /// <summary>
    /// Блок стейтментов.
    /// </summary>
    private static readonly Parser<StatementBase> Block = Parser.Lazy
        (
            () => Parser.OneOf
                (
                    // произвольное количество стейтментов внутри фигурных скобок
                    Parser.Chain
                        (
                            Parser.Position,
                            GenericStatement!.Repeated (minCount: 0).CurlyBrackets(),
                            (pos, lines) =>
                                (StatementBase)new BlockNode (pos.Line, lines.ToArray())
                        ),

                    // либо единственный стейтмент без фигурных скобок
                    GenericStatement!.Map
                        (
                            x => (StatementBase)new BlockNode (x.Line, new[] { x })
                        )
                )
        )
        .Labeled ("Block");

    /// <summary>
    /// Цикл for.
    /// </summary>
    private static readonly Parser<StatementBase> ForStatement = Parser.Chain
        (
            Parser.Position.Before (Reserved ("for")), // position
            Assignment.After (Parser.Term ("(")), // init
            Expression.Between (Term (";"), Term (";")), // condition
            Assignment.Or (Expression).Labeled ("Step").Before (Term (")")), // step
            Block, // body
            Block.Before (Reserved ("else")).Optional(), // elseBlock
            (position, init, condition, step, body, elseBlock) =>
                (StatementBase) new ForNode (position.Line, init, condition, step, body, elseBlock)
        )
        .Labeled ("For");

    /// <summary>
    /// Цикл foreach.
    /// </summary>
    private static readonly Parser<StatementBase> ForEachStatement = Parser.Chain
        (
            Parser.Position.Before (Reserved ("foreach")), // position
            Identifier.After (Parser.Term ("(")), // variable
            Expression.After (Term ("in")), // sequence
            Block.After (Parser.Term (")")), // body
            Block.Before (Reserved ("else")).Optional(), // elseBlock
            (position, variable, sequence, body, elseBlock) =>
                (StatementBase) new ForEachNode (position.Line, variable, sequence, body, elseBlock)
        )
        .Labeled ("ForEach");

    /// <summary>
    /// Оператор break.
    /// </summary>
    private static readonly Parser<StatementBase> BreakStatement = Parser.Chain
        (
            Parser.Position, // position
            Reserved ("break"),
            (position, _) => (StatementBase) new BreakNode (position.Line)
        )
        .Labeled ("Break");

    /// <summary>
    /// Оператор continue.
    /// </summary>
    private static readonly Parser<StatementBase> ContinueStatement = Parser.Chain
        (
            Parser.Position, // position
            Reserved ("continue"),
            (position, _) => (StatementBase) new ContinueNode (position.Line)
        )
        .Labeled ("Break");

    /// <summary>
    /// Точка с запятой. Не выполняет никаких действий,
    /// введена только для совместимости.
    /// </summary>
    private static readonly Parser<StatementBase> SemicolonStatement = Parser.Chain
        (
            Parser.Position, // position
            Term (";"),
            (position, _) => (StatementBase) new SemicolonNode (position.Line)
        )
        .Labeled ("Semicolon");

    /// <summary>
    /// Возврат значения из функции.
    /// </summary>
    private static readonly Parser<StatementBase> ReturnStatement = Parser.Chain
        (
            Parser.Position, // position
            Reserved ("return"),
            Expression.Instance ("ReturnValue").Optional(), // value
            (position, _, value) => (StatementBase) new ReturnNode (position.Line, value)
        )
        .Labeled ("Return");

    /// <summary>
    /// Цикл while.
    /// </summary>
    private static readonly Parser<StatementBase> WhileStatement = Parser.Chain
        (
            Parser.Position, // 1
            Reserved ("while"), // 2
            Expression.Instance ("Condition").RoundBrackets(), // 3
            Block.Instance ("Body"), // 4
            Block.Before (Parser.Reserved ("else")).Optional(), // 5
            (_1, _, _3, _4, _5) =>
                (StatementBase) new WhileNode (_1.Line, _3, _4, _5)
        )
        .Labeled ("While");

    /// <summary>
    /// Блок `else if`
    /// </summary>
    private static readonly Parser<IfNode> ElseIf = Parser.Chain
        (
            Parser.Position, // position
            Reserved ("else").Before (Reserved ("if")), // 2
            Expression.Instance ("Condition").RoundBrackets(), // condition
            Block.Instance ("Body"), // body
            (position, _, condition, body) => new IfNode (position.Line, condition, body, null, null)
        )
        .Labeled ("ElseIf");

    /// <summary>
    /// Условный оператор if-then-else.
    /// </summary>
    private static readonly Parser<StatementBase> IfStatement = Parser.Chain
        (
            Parser.Position.Before (Reserved ("if")), // position
            Expression.Instance ("Condition").RoundBrackets(), // condition
            Block.Instance ("Then"), // thenBlock
            ElseIf.Repeated (minCount: 0), // elseIf
            Block.Instance ("Else").After (Reserved ("else")).Optional(), // elseBlock
            (position, condition, thenBlock, elseIf, elseBlock) =>
                (StatementBase) new IfNode (position.Line, condition, thenBlock, elseIf.ToArray(), elseBlock)
        )
        .Labeled ("If");

    /// <summary>
    /// Блок using.
    /// </summary>
    private static readonly Parser<StatementBase> UsingStatement = Parser.Chain
        (
            Parser.Position.Before (Reserved ("using")), // 1
            Parser.Term ("("), // 2
            Parser.Identifier, // 3
            Parser.Term ("="), // 4
            Expression.Instance ("Init"), // 5
            Parser.Term (")"), // 6
            Block.Instance ("Body"), // 7
            (_1, _, _3, _, _5, _, _7) =>
                (StatementBase)new UsingNode (_1.Line, _3, _5, _7)
        )
        .Labeled ("Using");

    private static readonly Parser<StatementBase> FunctionDefinition = Parser.Chain
        (
            Parser.Position.Before (Reserved ("func")), // 1
            Identifier, // 2
            Identifier.SeparatedBy (Term (",")).RoundBrackets(), // 3
            Block, // 4
            (_1, _2, _3, _4) =>
                (StatementBase) new FunctionDefinitionNode (_1.Line, _2, _3.ToArray(), _4)
        )
        .Labeled ("FunctionDefinition");

    /// <summary>
    /// Внешний по отношению к Барсику код.
    /// </summary>
    private static readonly Parser<StatementBase> ExternalCode = Parser.Chain
        (
            Parser.Position, // position
            new ExternalParser(), // source
            (position, source) => (StatementBase) new ExternalNode (position.Line, source)
        )
        .Labeled ("ExternalCode");

    /// <summary>
    /// Стейтмент вообще.
    /// </summary>
    private static readonly Parser<StatementBase> GenericStatement = Parser.Lazy
        (
            () => Parser.OneOf
                (
                    SimpleStatement,
                    ForStatement,
                    ForEachStatement,
                    WhileStatement,
                    IfStatement,
                    UsingStatement,
                    FunctionDefinition,
                    BreakStatement,
                    ContinueStatement,
                    ReturnStatement,
                    ExternalCode,
                    TryCatchFinally,
                    SemicolonStatement
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
            Tokenizer tokenizer,
            TextWriter? debugOutput = null
        )
    {
        Sure.NotNull (sourceCode);

        var tokens = tokenizer.Tokenize (sourceCode);
        var state = new ParseState (tokens) { DebugOutput = debugOutput };
        var result = Expression.End().ParseOrThrow (state);

        return result;
    }

    /// <summary>
    /// Разбор программы.
    /// </summary>
    public static ProgramNode ParseProgram
        (
            string sourceText,
            Tokenizer tokenizer,
            TextWriter? debugOutput = null
        )
    {
        Sure.NotNull (sourceText);

        var tokens = tokenizer.Tokenize (sourceText);
        var state = new ParseState (tokens) { DebugOutput = debugOutput };
        var result = Program.ParseOrThrow (state);

        return result;
    }

    #endregion
}
