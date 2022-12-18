// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* BinaryComparisonPropertyOperation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

/// <summary>
///
/// </summary>
public abstract class BinaryComparisonPropertyOperation
    : IPropertyOperation
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="comparisonType"></param>
    protected BinaryComparisonPropertyOperation
        (
            ExpressionType comparisonType
        )
    {
        _comparisonType = comparisonType;
    }

    #endregion

    #region Private members

    private readonly ExpressionType _comparisonType;

    #endregion

    #region IPropertyOperation Members

    /// <summary>
    ///
    /// </summary>
    public abstract string OperationName { get; }

    /// <summary>
    ///
    /// </summary>
    public abstract string Text { get; }

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

        return expected =>
        {
            Debug.WriteLine ("Building expression for " + _comparisonType);

            ConstantExpression expectedHolder = propertyPath.Member is PropertyInfo
                ? Expression.Constant (expected, propertyPath.Member.As<PropertyInfo>().PropertyType)
                : Expression.Constant (expected);

            BinaryExpression comparison = Expression.MakeBinary (_comparisonType, propertyPath, expectedHolder);
            ParameterExpression lambdaParameter = propertyPath.GetParameter<T>();

            return Expression.Lambda<Func<T, bool>> (comparison, lambdaParameter);
        };
    }

    #endregion
}
