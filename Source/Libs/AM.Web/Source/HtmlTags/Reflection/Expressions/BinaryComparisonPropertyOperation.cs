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
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

public abstract class BinaryComparisonPropertyOperation : IPropertyOperation
{
    private readonly ExpressionType _comparisonType;

    protected BinaryComparisonPropertyOperation (ExpressionType comparisonType)
    {
        _comparisonType = comparisonType;
    }

    #region IPropertyOperation Members

    public abstract string OperationName { get; }
    public abstract string Text { get; }

    public Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T> (MemberExpression propertyPath)
    {
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
