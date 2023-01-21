// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExpressionBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Collections;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
///
/// </summary>
public static class ExpressionBuilder
{
    #region Public methods

    /// <summary>
    /// Построение
    /// </summary>
    public static Parser<TNode> Build<TNode>
        (
            Parser<TNode> root,
            Parser<Func<TNode, TNode>>[] prefixOps,
            Parser<Func<TNode, TNode>>[] postfixOps,
            InfixOperator<TNode>[] infixOps
        )
        where TNode: class
    {
        Sure.NotNull (prefixOps);
        Sure.NotNull (postfixOps);
        Sure.NotNull (infixOps);

        var expr = new DynamicParser<TNode> (() => null!);
        var result = (Parser<TNode>) expr;

        if (!prefixOps.IsNullOrEmpty())
        {
            result = new UnaryParser<TNode> (isPrefix: true, result, prefixOps);
        }

        if (!postfixOps.IsNullOrEmpty())
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
