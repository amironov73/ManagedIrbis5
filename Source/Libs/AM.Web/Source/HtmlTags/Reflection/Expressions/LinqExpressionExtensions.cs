// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

public static class LinqExpressionExtensions
{
    public static Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T> (this IPropertyOperation builder,
        Expression<Func<T, object>> path)
    {
        MemberExpression memberExpression = path.GetMemberExpression (true);
        return builder.GetPredicateBuilder<T> (memberExpression);
    }

    public static Expression<Func<T, bool>> GetPredicate<T> (this IPropertyOperation operation,
        Expression<Func<T, object>> path, object value)
    {
        return operation.GetPredicateBuilder (path) (value);
    }

    public static MemberExpression ToMemberExpression<T> (this PropertyInfo property)
    {
        ParameterExpression lambdaParameter = Expression.Parameter (typeof (T), "entity");
        return Expression.MakeMemberAccess (lambdaParameter, property);
    }

    public static ParameterExpression GetParameter<T> (this MemberExpression memberExpression)
    {
        MemberExpression outerMostMemberExpression = memberExpression;
        while (outerMostMemberExpression != null)
        {
            var parameterExpression = outerMostMemberExpression.Expression as ParameterExpression;
            if (parameterExpression != null && parameterExpression.Type == typeof (T))
                return parameterExpression;
            outerMostMemberExpression = outerMostMemberExpression.Expression as MemberExpression;
        }

        return Expression.Parameter (typeof (T), "unreferenced");
    }
}
