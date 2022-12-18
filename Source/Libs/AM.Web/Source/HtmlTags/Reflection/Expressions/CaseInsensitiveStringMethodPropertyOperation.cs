// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* CaseInsensitiveStringMethodPropertyOperation.cs --
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
public abstract class CaseInsensitiveStringMethodPropertyOperation
    : IPropertyOperation
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public virtual string OperationName => _method.Name;

    /// <summary>
    ///
    /// </summary>
    public abstract string Text { get; }

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <param name="method"></param>
    protected CaseInsensitiveStringMethodPropertyOperation
        (
            MethodInfo method
        )
        : this (method, false)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="method"></param>
    /// <param name="negate"></param>
    protected CaseInsensitiveStringMethodPropertyOperation
        (
            MethodInfo method,
            bool negate
        )
    {
        Sure.NotNull (method);

        _method = method;
        _negate = negate;
    }

    #region Private members

    private readonly MethodInfo _method;
    private readonly bool _negate;

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <param name="propertyPath"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T>
        (
            MemberExpression propertyPath
        )
    {
        Sure.NotNull (propertyPath);

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
