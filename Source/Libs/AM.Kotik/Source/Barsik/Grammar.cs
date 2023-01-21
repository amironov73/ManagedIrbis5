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
                Operator.LeftAssociative ("Shift", "<<", ">>"),
                Operator.LeftAssociative ("Bitwise", "&", "|", "^"),
                Operator.LeftAssociative ("Multiplication", "*", "/", "%" ),
                Operator.LeftAssociative ("Addition", "+", "-" ),
                Operator.LeftAssociative ("Comparison", "<", ">", "<=", ">=", "==", "!=", "<>" )
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
            Block.Before (Parser.Reserved ("else")).Optional(), // 11
            (_1, _, _, _4, _, _6, _, _8, _, _10, _11) =>
                (StatementBase) new ForNode (_1.Line, _4, _6, _8, _10, _11)
        )
        .Labeled ("For");

    /// <summary>
    /// Цикл foreach.
    /// </summary>
    private static readonly Parser<StatementBase> ForEachStatement = Parser.Chain
        (
            Parser.Position, // 1
            Parser.Reserved ("foreach"), // 2
            Parser.Term ("("), // 3
            Identifier, // 4
            Parser.Term ("in"), // 5
            Expression.Labeled ("Enumerable"), // 6
            Parser.Term (")"), // 7
            Block.Instance ("Body"), // 8
            Block.Before (Parser.Reserved ("else")).Optional(), // 9
            (_1, _, _, _4, _, _6, _, _8, _9) =>
                (StatementBase) new ForEachNode (_1.Line, _4, _6, _8, _9)
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
            Parser.Reserved ("while"), // 2
            Expression.Instance ("Condition").RoundBrackets(), // 3
            Block.Instance ("Body"), // 4
            Block.Before (Parser.Reserved ("else")).Optional(), // 5
            (_1, _, _3, _4, _5) =>
                (StatementBase) new WhileNode (_1.Line, _3, _4, _5)
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
                (StatementBase)new IfNode (_1.Line, _4, _6, _7)
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
                (StatementBase)new UsingNode (_1.Line, _4, _6, _8)
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
            TextWriter? debugOutput = null
        )
    {
        Sure.NotNull (sourceCode);

        var tokenizer = new Tokenizer();
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
            TextWriter? debugOutput = null
        )
    {
        Sure.NotNull (sourceText);

        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (sourceText);
        var state = new ParseState (tokens) { DebugOutput = debugOutput };
        var result = Program.ParseOrThrow (state);

        return result;
    }

    #endregion
}
