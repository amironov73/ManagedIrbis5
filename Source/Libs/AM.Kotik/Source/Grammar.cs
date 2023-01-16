// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Grammar.cs -- грамматика языка
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Грамматика языка.
/// </summary>
public static class Grammar
{
    #region Public methods and properties

    /// <summary>
    /// Разбор литералов.
    /// </summary>
    public static readonly LiteralParser LiteralValue = new ();

    /// <summary>
    /// Порождение константного узла.
    /// </summary>
    public static readonly Parser<AtomNode> Literal = LiteralValue.Map
        (
            x => (AtomNode) new ConstantNode (x)
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
    public static readonly Parser<VariableNode> Variable = Identifier.Map
        (
            x => new VariableNode (x)
        );

    /// <summary>
    /// Базовое выражение.
    /// </summary>
    public static readonly Parser<AtomNode> BasicExpression = ExpressionBuilder.Build
        (
            new[]
            {
                new[] { "<<", ">>" },
                new[] { "&", "|" },
                new[] { "*", "/", "%" },
                new[] { "+", "-" },
            },
            ((left, operation, right) => new BinaryNode (left, operation, right))
        );

    /// <summary>
    /// Выражение без присваивания.
    /// </summary>
    public static Parser<ExpressionNode> Expression = BasicExpression.Map
        (
            x => new ExpressionNode (null, null, x)
        );

    /// <summary>
    /// Присваивание.
    /// </summary>
    public static readonly Parser<ExpressionNode> Assignment = Parser.Chain
        (
            Identifier,
            Parser.Term ("="),
            BasicExpression,
            (variable, operation, expression) =>
                new ExpressionNode (variable, operation, expression)
        );

    /// <summary>
    /// Простой стейтмент.
    /// </summary>
    public static readonly Parser<StatementBase> SimpleStatement = Parser.OneOf
        (
            Assignment.Map (x => (StatementBase) new SimpleStatement (x)),
            Expression.Map (x => (StatementBase) new SimpleStatement (x))
        );

    /// <summary>
    /// Программа в целом.
    /// </summary>
    public static readonly Parser<ProgramNode> Program = new RepeatParser<StatementBase>
        (
            SimpleStatement
        )
        .Map (x => new ProgramNode (x))
        .End();

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
        var state = new ParseState (tokens);
        var result = Program.ParseOrThrow (state);

        return result;
    }

    #endregion
}
