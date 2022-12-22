// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IndexerValueGetter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection;

/// <summary>
///
/// </summary>
public class IndexerValueGetter
    : IValueGetter
{
    #region Properties

    /// <inheritdoc cref="IValueGetter.Name"/>
    public string Name => $"[{Index}]";

    /// <summary>
    ///
    /// </summary>
    public int Index { get; }

    /// <inheritdoc cref="IValueGetter.DeclaringType"/>
    public Type? DeclaringType { get; }

    /// <inheritdoc cref="IValueGetter.ValueType"/>
    public Type? ValueType => DeclaringType?.GetElementType();

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="arrayType"></param>
    /// <param name="index"></param>
    public IndexerValueGetter
        (
            Type arrayType,
            int index
        )
    {
        Sure.NotNull (arrayType);

        DeclaringType = arrayType;
        Index = index;
    }

    #endregion

    #region IValueGetter members

    /// <inheritdoc cref="IValueGetter.GetValue"/>
    public object? GetValue
        (
            object target
        )
    {
        Sure.NotNull (target);

        return ((Array) target).GetValue (Index);
    }
    /// <inheritdoc cref="IValueGetter.ChainExpression"/>
    public Expression ChainExpression
        (
            Expression body
        )
    {
        Sure.NotNull (body);

        var memberExpression = Expression.ArrayIndex
            (
                body,
                Expression.Constant (Index, typeof (int))
            );
        if (!DeclaringType!.GetElementType()!.GetTypeInfo().IsValueType)
        {
            return memberExpression;
        }

        return Expression.Convert (memberExpression, typeof (object));
    }

    /// <inheritdoc cref="IValueGetter.SetValue"/>
    public void SetValue
        (
            object target,
            object? propertyValue
        )
    {
        Sure.NotNull (target);

        ((Array)target).SetValue (propertyValue, Index);
    }

    #endregion

    #region Private methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    protected bool Equals
        (
            IndexerValueGetter other
        )
    {
        Sure.NotNull (other);

        return DeclaringType == other.DeclaringType && Index == other.Index;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals (this, obj))
        {
            return true;
        }

        return obj is IndexerValueGetter getter && Equals (getter);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        unchecked
        {
            return ((DeclaringType?.GetHashCode() ?? 0) * 397) ^ Index;
        }
    }

    #endregion
}
