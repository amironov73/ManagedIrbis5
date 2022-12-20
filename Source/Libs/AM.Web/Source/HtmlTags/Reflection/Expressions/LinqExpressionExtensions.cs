// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* LinqExpressionExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

/// <summary>
///
/// </summary>
public static class LinqExpressionExtensions
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="path"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T>
        (
            this IPropertyOperation builder,
            Expression<Func<T, object>> path
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (path);

        var memberExpression = path.GetMemberExpression (true);

        return builder.GetPredicateBuilder<T> (memberExpression);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="path"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Expression<Func<T, bool>> GetPredicate<T>
        (
            this IPropertyOperation operation,
            Expression<Func<T, object>> path,
            object value
        )
    {
        Sure.NotNull (operation);
        Sure.NotNull (path);

        return operation.GetPredicateBuilder (path) (value);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="property"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static MemberExpression ToMemberExpression<T>
        (
            this PropertyInfo property
        )
    {
        Sure.NotNull (property);

        var lambdaParameter = Expression.Parameter (typeof (T), "entity");

        return Expression.MakeMemberAccess (lambdaParameter, property);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="memberExpression"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ParameterExpression GetParameter<T>
        (
            this MemberExpression memberExpression
        )
    {
        Sure.NotNull (memberExpression);

        var outerMostMemberExpression = memberExpression;
        while (outerMostMemberExpression != null)
        {
            if (outerMostMemberExpression.Expression is ParameterExpression parameterExpression
                && parameterExpression.Type == typeof (T))
            {
                return parameterExpression;
            }

            outerMostMemberExpression = outerMostMemberExpression.Expression as MemberExpression;
        }

        return Expression.Parameter (typeof (T), "unreferenced");
    }

    #endregion
}
