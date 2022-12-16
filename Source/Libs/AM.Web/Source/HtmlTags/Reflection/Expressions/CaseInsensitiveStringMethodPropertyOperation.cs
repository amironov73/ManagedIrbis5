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

public abstract class CaseInsensitiveStringMethodPropertyOperation : IPropertyOperation
{
    private readonly MethodInfo _method;
    private readonly bool _negate;

    protected CaseInsensitiveStringMethodPropertyOperation (MethodInfo method) : this (method, false)
    {
    }

    protected CaseInsensitiveStringMethodPropertyOperation (MethodInfo method, bool negate)
    {
        _method = method;
        _negate = negate;
    }

    public virtual string OperationName => _method.Name;
    public abstract string Text { get; }

    public Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T> (MemberExpression propertyPath)
    {
        return valueToCheck =>
        {
            ConstantExpression valueToCheckConstant = Expression.Constant (valueToCheck);
            BinaryExpression binaryExpression =
                Expression.Coalesce (propertyPath, Expression.Constant (string.Empty));
            ConstantExpression invariantCulture = Expression.Constant (StringComparison.OrdinalIgnoreCase);
            Expression expression =
                Expression.Call (binaryExpression, _method, valueToCheckConstant, invariantCulture);
            if (_negate)
            {
                expression = Expression.Not (expression);
            }

            ParameterExpression lambdaParameter = propertyPath.GetParameter<T>();
            return Expression.Lambda<Func<T, bool>> (expression, lambdaParameter);
        };
    }
}
