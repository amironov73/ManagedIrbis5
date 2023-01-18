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
    public static Parser<object> Build
        (
            Parser<object> root,
            string[][] levels,
            Func<object, string, object, object> function
        )
    {
        Sure.AssertState (!levels.IsNullOrEmpty());
        Sure.NotNull (function);

        var expr = new DynamicParser<object> (() => null!);
        var result = LeftAssociative (expr, levels[0], function);
        for (var i = 1; i < levels.Length; i++)
        {
            result = LeftAssociative (result, levels[i], function);
        }

        var parenthesis = result.RoundBrackets();
        expr.Inner = () => root.Or (parenthesis);

        return result;
    }

    /// <summary>
    /// Построение
    /// </summary>
    public static Parser<AtomNode> Build
        (
            Parser<AtomNode> root,
            Parser<Func<AtomNode, AtomNode>>[] prefixOps,
            Parser<Func<AtomNode, AtomNode>>[] postfixOps,
            string[][] binaryOps,
            Func<AtomNode, string, AtomNode, AtomNode> function
        )
    {
        Sure.AssertState (!binaryOps.IsNullOrEmpty());
        Sure.NotNull (function);

        var expr = new DynamicParser<AtomNode> (() => null!);
        var result = (Parser<AtomNode>) expr;
        if (!postfixOps.IsNullOrEmpty())
        {
            result = new UnaryParser<AtomNode> (expr, postfixOps);
        }

        foreach (var binaryOp in binaryOps)
        {
            result = LeftAssociative (result, binaryOp, function);
        }

        var parenthesis = result.RoundBrackets();
        expr.Inner = () => root.Or (parenthesis);

        return result;
    }

    /// <summary>
    /// Формирует лево-ассоциативный оператор.
    /// </summary>
    public static Parser<TResult> LeftAssociative<TResult>
        (
            Parser<TResult> item,
            string[] operations,
            Func<TResult, string, TResult, TResult> function
        )
        where TResult: class
    {
        Sure.NotNull (item);
        Sure.AssertState (!operations.IsNullOrEmpty());
        Sure.NotNull (function);

        return new InfixOperator<TResult>
            (
                item,
                operations,
                function,
                BinaryOperatorType.LeftAssociative
            );
    }

    #endregion
}
