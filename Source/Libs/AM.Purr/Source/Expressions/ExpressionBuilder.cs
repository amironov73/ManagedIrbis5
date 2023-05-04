// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExpressionBuilder.cs -- построитель парсеров для выражений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Purr.Parsers;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Expressions;

/// <summary>
/// Построитель парсеров для выражений.
/// </summary>
[PublicAPI]
public static class ExpressionBuilder
{
    #region Public methods

    /// <summary>
    /// Построение выражения.
    /// </summary>
    public static Parser<TNode> Build<TNode>
        (
            Parser<TNode> root,
            IList<IParser<Func<TNode, TNode>>> prefixOps,
            IList<IParser<Func<TNode, TNode>>> postfixOps,
            IList<InfixOperator<TNode>> infixOps
        )
    {
        Sure.NotNull (prefixOps);
        Sure.NotNull (postfixOps);
        Sure.NotNull (infixOps);

        var expr = new DynamicParser<TNode> (() => null!);
        var result = (Parser<TNode>) expr;

        if (prefixOps.Count != 0)
        {
            result = new UnaryParser<TNode> (isPrefix: true, result, prefixOps);
        }

        if (postfixOps.Count != 0)
        {
            result = new UnaryParser<TNode> (isPrefix: false, result, postfixOps);
        }

        foreach (var op in infixOps)
        {
            result = new InfixParser<TNode> (result, op.Operation, op.Function, op.Kind);
        }

        var parenthesis = result.RoundBrackets();
        expr.Function = () => root.Or (parenthesis);

        return result;
    }

    #endregion
}
