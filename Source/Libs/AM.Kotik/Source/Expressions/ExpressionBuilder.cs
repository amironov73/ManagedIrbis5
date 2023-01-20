// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

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
    public static Parser<AtomNode> Build
        (
            Parser<AtomNode> root,
            Parser<Func<AtomNode, AtomNode>>[] prefixOps,
            Parser<Func<AtomNode, AtomNode>>[] postfixOps,
            InfixOperator<AtomNode>[] infixOps
        )
    {
        Sure.NotNull (prefixOps);
        Sure.NotNull (postfixOps);
        Sure.NotNull (infixOps);

        var expr = new DynamicParser<AtomNode> (() => null!);
        var result = (Parser<AtomNode>) expr;

        if (!prefixOps.IsNullOrEmpty())
        {
            result = new UnaryParser<AtomNode> (isPrefix: true, result, prefixOps);
        }

        if (!postfixOps.IsNullOrEmpty())
        {
            result = new UnaryParser<AtomNode> (isPrefix: false, result, postfixOps);
        }

        foreach (var op in infixOps)
        {
            result = new InfixParser<AtomNode> (result, op.Operation, op.Function, op.Kind);
        }

        var parenthesis = result.RoundBrackets();
        expr.Inner = () => root.Or (parenthesis);

        return result;
    }

    #endregion
}
