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
public sealed class Grammar
    : IGrammar
{
    #region Properties

    /// <inheritdoc cref="IGrammar.Atoms"/>
    public IList<Parser<AtomNode>> Atoms { get; }

    /// <inheritdoc cref="IGrammar.Infixes"/>
    public IList<InfixOperator<AtomNode>> Infixes { get; }

    /// <inheritdoc cref="IGrammar.Postfixes"/>
    public IList<Parser<Func<AtomNode, AtomNode>>> Postfixes { get; }

    /// <inheritdoc cref="IGrammar.Prefixes"/>
    public IList<Parser<Func<AtomNode, AtomNode>>> Prefixes { get; }

    /// <inheritdoc cref="IGrammar.Statements"/>
    public IList<Parser<StatementBase>> Statements { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Grammar()
    {
        Atoms = new List<Parser<AtomNode>>();
        Infixes = new List<InfixOperator<AtomNode>>();
        Postfixes = new List<Parser<Func<AtomNode, AtomNode>>>();
        Prefixes = new List<Parser<Func<AtomNode, AtomNode>>>();
        Statements = new List<Parser<StatementBase>>();
    }

    #endregion

    #region Private members

    private Parser<StatementBase> BuildStatement (Parser<AtomNode> innerParser) =>
        Parser.Chain
            (
                Parser.Position,
                innerParser,
                (pos, x) => (StatementBase) new SimpleStatement (pos.Line, x)
            );

    /// <summary>
    /// Порождение константного узла.
    /// </summary>
    private readonly Parser<AtomNode> Literal = new LiteralParser().Map
        (
            x => (AtomNode) new ConstantNode (x)
        );

    /// <summary>
    /// Форматная строка.
    /// </summary>
    private readonly Parser<AtomNode> Format = new FormatParser().Map
        (
            x => (AtomNode) new FormatNode (x)
        );

    /// <summary>
    /// Разбор перечисленных терминов.
    /// </summary>
    private TermParser Term (params string[] terms) => new (terms);

    /// <summary>
    /// Разбор зарезервированного слова.
    /// </summary>
    /// <param name="word"><c>null</c> означает "любое зарезервированное слово".
    /// </param>
    private ReservedWordParser Reserved (string? word) => new (word);

    /// <summary>
    /// Разбор идентификаторов.
    /// </summary>
    private readonly IdentifierParser Identifier = new ();

    /// <summary>
    /// Выражение.
    /// </summary>
    private DynamicParser<AtomNode> Expression = null!;

    /// <summary>
    /// Блок стейтментов.
    /// </summary>
    private DynamicParser<StatementBase> Block = null!;

    /// <summary>
    /// Стейтмент вообще.
    /// </summary>
    private Parser<StatementBase> GenericStatement = null!;

    /// <summary>
    /// Вычисляемый узел.
    /// </summary>
    private DynamicParser<AtomNode> Atom = null!;

    /// <summary>
    /// Программа в целом.
    /// </summary>
    private Parser<ProgramNode> Program = null!;

    private void ApplyDefaults()
    {
        Atoms.Clear();
        Infixes.Clear();
        Postfixes.Clear();
        Prefixes.Clear();
        Statements.Clear();

        var variable = Identifier.Map (x => (AtomNode)new VariableNode (x))
            .Labeled ("Variable");

        Atom = new DynamicParser<AtomNode> (() => null!);
        Expression = new DynamicParser<AtomNode> (() => null!);
        Block = new DynamicParser<StatementBase> (() => null!);

        var ternary = Parser.Chain
            (
                new PeepingParser<string, AtomNode> (Term ("?"), Expression),
                Expression,
                Expression.After (Term (":")),
                (condition, trueValue, falseValue) =>
                    (AtomNode) new TernaryNode (condition, trueValue, falseValue)
            )
            .Labeled ("Ternary");

        var throwOperator = Atom.RoundBrackets()
            .After (Reserved ("throw"))
            .Map (x => (AtomNode) new ThrowNode (x))
            .Labeled ("Throw");

        var namedArgument  = Parser.Chain
            (
                Identifier.Before (Term (":")),
                Expression,
                (name, expr) => (AtomNode) new NamedArgumentNode (name, expr)
            )
            .Labeled ("NamedArg");

        var typeName = Identifier.SeparatedBy (Term ("."), minCount: 1)
            .Map (x => string.Join ('.', x))
            .Or (new LiteralParser().Map (x => (string) x))
            .Labeled ("TypeName");

        var newOperator  = Parser.Chain
            (
                typeName.After (Reserved ("new")),
                Expression.SeparatedBy (Term (",")).RoundBrackets(),
                (name, args) => (AtomNode) new NewNode (name, args.ToArray())
            )
            .Labeled ("New");

        var keyAndValue  = Parser.Chain
            (
                Expression.Before (Term (":")),
                Expression,
                (key, value) => new KeyValueNode (key, value)
            )
            .Labeled ("KeyAndValue");

        var dictionary  = keyAndValue.SeparatedBy (Term (",")).CurlyBrackets()
            .Labeled ("Dictionary")
            .Map (x => (AtomNode) new DictionaryNode (x.ToArray()));

        var list  = Expression.SeparatedBy (Term (",")).SquareBrackets()
            .Labeled ("List")
            .Map (x => (AtomNode) new ListNode (x.ToArray()));

        var property  = Operator.Unary
            (
                Identifier.After (Term (".")),
                "Property",
                name => target => new PropertyNode (target, name)
            );

        var index  = Operator.Unary
            (
                Expression.SquareBrackets(),
                "Index",
                x => target => new IndexNode (target, x)
            );

        var methodCall = Parser.Chain<string, string, IEnumerable<AtomNode>, Func<AtomNode, AtomNode>>
            (
                Term ("."),
                Identifier,
                Expression.SeparatedBy (Term (",")).RoundBrackets(),
                (_, name, args) => target => new MethodNode (target, name, args.ToArray())
            )
            .Labeled ("MethodCall");

        var functionCall  = Parser.Chain
            (
                Identifier,
                Expression.Or (namedArgument).SeparatedBy (Term (",")).RoundBrackets(),
                (name, args) => (AtomNode) new CallNode (name, args.ToArray())
            )
            .Labeled ("FunctionCall");

        var lambda  = Parser.Chain
            (
                Reserved ("lambda"),
                Identifier.SeparatedBy (Term (",")).RoundBrackets(),
                Block,
                (_, args, body) => (AtomNode) new LambdaNode (args.ToArray(), body)
            )
            .Labeled ("Lambda");

        var awaitOperator = Expression.After (Reserved ("await"))
            .Map (x => (AtomNode)new AwaitNode (x));

        Atoms.Add (Literal);
        Atoms.Add (Format);
        Atoms.Add (ternary);
        Atoms.Add (functionCall);
        Atoms.Add (variable);
        Atoms.Add (list);
        Atoms.Add (dictionary);
        Atoms.Add (newOperator);
        Atoms.Add (throwOperator);
        Atoms.Add (awaitOperator);
        Atoms.Add (lambda);

        //===================================================

        Prefixes.Add (Operator.Unary
            (
                Term ("-"),
                "UnaryMinus",
                _ => target => new MinusNode (target)
            ));
        Prefixes.Add(Operator.Unary
            (
                Term("!"),
                "PrefixBang",
                _ => target => new PrefixBangNode (target)
            ));
        Prefixes.Add (Operator.Unary
            (
                Term ("~"),
                "Tilda",
                _ => target => new TildaNode (target)
            ));
        Prefixes.Add (Operator.Increment ("PrefixIncrement", true));
        Prefixes.Add (Operator.Unary
            (
                Parser.OneOf (Identifier, Reserved (null))
                    .RoundBrackets(),
                "Cast",
                x => target => new CastNode (x, target)
            ));

        //===================================================

        Postfixes.Add (Operator.Increment ("PostfixIncrement", false));
        Postfixes.Add (Operator.Unary
            (
                Term ("!"),
                "PostfixBang",
                _ => target => new PostfixBangNode (target)
            ));
        Postfixes.Add (index);
        Postfixes.Add (methodCall);
        Postfixes.Add (property);

        //===================================================

        Infixes.Add (Operator.NonAssociative ("Shuttle", "<=>"));
        Infixes.Add (Operator.NonAssociative ("In/is", "in", "is"));
        Infixes.Add (Operator.LeftAssociative ("Coalesce", "??"));
        Infixes.Add (Operator.LeftAssociative ("Shift", "<<", ">>"));
        Infixes.Add (Operator.LeftAssociative ("Bitwise", "&", "|", "^"));
        Infixes.Add (Operator.LeftAssociative ("Multiplication", "*", "/", "%" ));
        Infixes.Add (Operator.LeftAssociative ("Addition", "+", "-" ));
        Infixes.Add (Operator.LeftAssociative ("Comparison", "<", ">", "<=", ">=", "==", "!=", "<>", "===", "!==", "~~", "~~~" ));

        //===================================================

        var leftHand  = ExpressionBuilder.Build
            (
                root: Atom,

                // префиксные операции не предусмотрены
                prefixOps: Array.Empty<Parser<Func<AtomNode, AtomNode>>>(),

                postfixOps: new[]
                {
                    // постфиксные операции
                    index,
                    property
                },

                // инфиксные операции не предусмотрены
                infixOps: Array.Empty<InfixOperator<AtomNode>>()
            )
            .Labeled ("LeftHand");

        var assignment  = Parser.Chain
            (
                new RepeatParser<Tuple<AtomNode, string>>
                    (
                        // x1 = x2 = ...
                        Parser.Chain
                            (
                                leftHand,
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

        var catchClause = Parser.Chain
            (
                Identifier.RoundBrackets().After (Reserved ("catch")),
                Block,
                (name, body) => new TryNode.CatchBlock (name, body)
            )
            .Optional();

        var tryCatchFinally = Parser.Chain
            (
                Parser.Position.Before (Reserved ("try")),
                Block,
                catchClause,
                Block.After (Reserved ("finally")).Optional(),
                (position, tryBlock, catchBlock, finallyBlock) => (StatementBase)
                    new TryNode (position.Line, tryBlock, catchBlock, finallyBlock)
            )
            .Labeled ("TryCatchFinally");

        var simpleStatement = BuildStatement (assignment).Labeled ("SimpleStatement");

        var forStatement = Parser.Chain
            (
                Parser.Position.Before (Reserved ("for")),
                assignment.After (Parser.Term ("(")),
                Expression.Between (Term (";"), Term (";")),
                assignment.Or (Expression).Before (Term (")")),
                Block,
                Block.Before (Reserved ("else")).Optional(),
                (position, init, condition, step, body, elseBlock) =>
                    (StatementBase) new ForNode (position.Line, init, condition, step, body, elseBlock)
            )
            .Labeled ("For");

        var forEachStatement = Parser.Chain
                (
                    Parser.Position.Before (Reserved ("foreach")),
                    Identifier.After (Parser.Term ("(")),
                    Expression.After (Term ("in")),
                    Block.After (Parser.Term (")")),
                    Block.Before (Reserved ("else")).Optional(),
                    (position, name, sequence, body, elseBlock) =>
                        (StatementBase) new ForEachNode (position.Line, name, sequence, body, elseBlock)
                )
            .Labeled ("ForEach");

        var breakStatement = Parser.Position.Before (Reserved ("break"))
            .Map (x => (StatementBase) new BreakNode (x.Line)).Labeled ("Break");

        var continueStatement = Parser.Position.Before (Reserved ("continue"))
            .Map (x => (StatementBase) new ContinueNode (x.Line)).Labeled ("Continue");

        var semicolonStatement = Parser.Position.Before (Term (";"))
            .Map (x => (StatementBase)new SemicolonNode(x.Line)).Labeled ("Semicolon");

        var returnStatement = Parser.Chain
            (
                Parser.Position.Before (Reserved ("return")),
                Expression.Optional(),
                (position, value) => (StatementBase)new ReturnNode (position.Line, value)
            )
            .Labeled ("Return");

        var labelStatement = Parser.Chain
            (
                Parser.Position,
                Identifier.Before (Term (":")),
                (position, label) => (StatementBase) new LabelNode (position.Line, label)
            )
            .Labeled ("Label");

        var gotoStatement = Parser.Chain
            (
                Parser.Position,
                Identifier.After (Reserved ("goto")),
                (position, label) => (StatementBase) new GotoNode (position.Line, label)
            )
            .Labeled ("Goto");

        var whileStatement = Parser.Chain
            (
                Parser.Position.Before (Reserved ("while")),
                Expression.RoundBrackets(),
                Block,
                Block.After (Reserved ("else")).Optional(),
                (position, condition, body, elseBody) =>
                    (StatementBase) new WhileNode (position.Line, condition, body, elseBody)
            )
            .Labeled ("While");

        var elseIf = Parser.Chain
            (
                Parser.Position,
                Reserved ("else").Before (Reserved ("if")),
                Expression.RoundBrackets(),
                Block,
                (position, _, condition, body) => new IfNode (position.Line, condition, body, null, null)
            )
            .Labeled ("ElseIf");

        var ifStatement = Parser.Chain
            (
                Parser.Position.Before (Reserved ("if")),
                Expression.RoundBrackets(),
                Block,
                elseIf.Repeated (minCount: 0),
                Block.After (Reserved ("else")).Optional(),
                (position, condition, thenBlock, other, elseBlock) =>
                    (StatementBase) new IfNode (position.Line, condition, thenBlock, other.ToArray(), elseBlock)
            )
            .Labeled ("If");

        var usingStatement = Parser.Chain
            (
                Parser.Position.Before (Reserved ("using")),
                Parser.Term ("("),
                Parser.Identifier,
                Parser.Term ("="),
                Expression,
                Parser.Term (")"),
                Block,
                (position, _, name, _, expr, _, body) =>
                    (StatementBase) new UsingNode (position.Line, name, expr, body)
            )
            .Labeled ("Using");

        var functionDefinition = Parser.Chain
            (
                Parser.Position.Before (Reserved ("func")),
                Identifier,
                Identifier.SeparatedBy (Term (",")).RoundBrackets(),
                Block,
                (position, name, args, body) =>
                    (StatementBase) new FunctionDefinitionNode (position.Line, name, args.ToArray(), body)
            )
            .Labeled ("FunctionDefinition");

        var externalCode = Parser.Chain
            (
                Parser.Position,
                new ExternalParser(),
                (position, source) => (StatementBase) new ExternalNode (position.Line, source)
            )
            .Labeled ("ExternalCode");

        var withAssignment = Parser.Chain
            (
                Parser.Position.Before (Term (".")),
                Identifier.Before (Term ("=")),
                Expression,
                (position, prop, expr) =>
                    (StatementBase) new WithAssignmentNode (position.Line, prop, expr)
            )
            .Labeled ("WithAssignment");

        var with = Parser.Chain
            (
                Parser.Position.Before (Reserved ("with")),
                leftHand,
                Block,
                (position, center, body) => (StatementBase) new WithNode (position.Line, center, body)
            )
            .Labeled ("With");

        Statements.Add (labelStatement);
        Statements.Add (simpleStatement);
        Statements.Add (forStatement);
        Statements.Add (forEachStatement);
        Statements.Add (whileStatement);
        Statements.Add (ifStatement);
        Statements.Add (usingStatement);
        Statements.Add (functionDefinition);
        Statements.Add (breakStatement);
        Statements.Add (continueStatement);
        Statements.Add (returnStatement);
        Statements.Add (externalCode);
        Statements.Add (tryCatchFinally);
        Statements.Add (with);
        Statements.Add (withAssignment);
        Statements.Add (gotoStatement);
        Statements.Add (semicolonStatement);

    }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание грамматики по умолчанию для Барсика.
    /// </summary>
    public static Grammar CreateDefaultBarsikGrammar()
    {
        var result = new Grammar();
        result.ApplyDefaults();

        return result;
    }

    /// <summary>
    /// Пересоздание грамматики.
    /// </summary>
    public void Rebuild()
    {
        Atom.Function = () => Parser.OneOf (Atoms.ToArray()).Labeled ("Atom");

        Expression.Function = () => ExpressionBuilder.Build
            (
                root: Atom,
                Prefixes,
                Postfixes,
                Infixes
            )
        .Labeled ("Expression");

        Block.Function = () => Parser.OneOf
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
            .Labeled ("Block");

        GenericStatement = Parser.OneOf (Statements.ToArray());

        Program  = new RepeatParser<StatementBase>
                (
                    GenericStatement
                )
            .Labeled ("Statements")
            .Map (x => new ProgramNode (x))
            .End()
            .Labeled ("Program");
    }

    /// <summary>
    /// Разбор текста выражения.
    /// </summary>
    public AtomNode ParseExpression
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
    public ProgramNode ParseProgram
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
