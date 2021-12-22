// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* Evaluator.cs -- методы для вычисления значения Linq-выражения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Linq.Expressions;

#endregion

#nullable enable

namespace AM.Data;

/// <summary>
/// Методы для вычисления значения Linq-выражения.
/// </summary>
internal static class Evaluator
{
    /// <summary>
    /// Gets the value of a Linq expression.
    /// </summary>
    /// <param name="expr">The expresssion.</param>
    public static object? EvalExpression (Expression expr)
    {
        //
        // Easy case
        //
        if (expr.NodeType == ExpressionType.Constant)
        {
            return ((ConstantExpression)expr).Value;
        }

        //
        // General case
        //
        var lambda = Expression.Lambda (expr, Enumerable.Empty<ParameterExpression>());
        return lambda.Compile().DynamicInvoke();
    }
}
