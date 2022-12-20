// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AccessorDef.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq.Expressions;

#endregion

#nullable enable

namespace AM.HtmlTags.Conventions.Elements;

#region Using directives

using Reflection;

#endregion

/// <summary>
///
/// </summary>
public class AccessorDef
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public IAccessor Accessor { get; }

    /// <summary>
    ///
    /// </summary>
    public Type ModelType { get; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <param name="modelType"></param>
    public AccessorDef
        (
            IAccessor accessor,
            Type modelType
        )
    {
        Sure.NotNull (accessor);
        Sure.NotNull (modelType);

        Accessor = accessor;
        ModelType = modelType;
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static AccessorDef For<T>
        (
            Expression<Func<T, object>> expression
        )
    {
        Sure.NotNull (expression);

        return new (expression.ToAccessor(), typeof (T));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals
        (
            AccessorDef? other
        )
    {
        if (ReferenceEquals (null, other))
        {
            return false;
        }

        if (ReferenceEquals (this, other))
        {
            return true;
        }

        return Equals (other.Accessor, Accessor) && other.ModelType == ModelType;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool Is<T>() => Accessor.PropertyType == typeof (T);

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        if (ReferenceEquals (null, obj))
        {
            return false;
        }

        if (ReferenceEquals (this, obj))
        {
            return true;
        }

        if (obj.GetType() != typeof (AccessorDef))
        {
            return false;
        }

        return Equals ((AccessorDef)obj);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return HashCode.Combine
            (
                Accessor.GetHashCode(),
                ModelType.GetHashCode()
            );
    }

    #endregion
}
