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

/* MethodValueGetter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection;

/// <summary>
///
/// </summary>
public class MethodValueGetter
    : IValueGetter
{
    #region Proeprties

    /// <inheritdoc cref="IValueGetter.Name"/>
    public string Name
    {
        get
        {
            if (_arguments.Length == 1)
            {
                return $"[{_arguments[0]}]";
            }

            if (_arguments.Length == 0)
            {
                return _methodInfo.Name;
            }

            throw new NotSupportedException ("Dunno how to deal with this method");
        }
    }

    /// <inheritdoc cref="IValueGetter.DeclaringType"/>
    public Type? DeclaringType => _methodInfo.DeclaringType;

    /// <inheritdoc cref="IValueGetter.ValueType"/>
    public Type ValueType => _methodInfo.ReturnType;

    /// <summary>
    ///
    /// </summary>
    public Type ReturnType => _methodInfo.ReturnType;

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="methodInfo"></param>
    /// <param name="arguments"></param>
    /// <exception cref="NotSupportedException"></exception>
    public MethodValueGetter
        (
            MethodInfo methodInfo,
            object?[] arguments
        )
    {
        Sure.NotNull (methodInfo);
        Sure.NotNull (arguments);

        if (arguments.Length > 1)
        {
            throw new NotSupportedException
                (
                    "ReflectionHelper only supports methods with no arguments or a single indexer argument"
                );
        }

        _methodInfo = methodInfo;
        _arguments = arguments;
    }

    #endregion

    #region Private members

    private readonly MethodInfo _methodInfo;

    private readonly object[] _arguments;

    #endregion

    #region IValueGetter members

    /// <inheritdoc cref="IValueGetter.GetValue"/>
    public object? GetValue
        (
            object target
        )
    {
        Sure.NotNull (target);

        return _methodInfo.Invoke (target, _arguments);
    }

    /// <inheritdoc cref="IValueGetter.ChainExpression"/>
    public Expression ChainExpression
        (
            Expression body
        )
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc cref="IValueGetter.SetValue"/>
    public void SetValue
        (
            object target,
            object? propertyValue
        )
    {
        throw new NotSupportedException();
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals
        (
            MethodValueGetter? other
        )
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals (this, other))
        {
            return true;
        }

        return Equals (other._methodInfo, _methodInfo) && other._arguments.SequenceEqual (_arguments);
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

        return obj is MethodValueGetter getter && Equals (getter);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        unchecked
        {
            if (_arguments.Length != 0)
            {
                return ((_methodInfo != null! ? _methodInfo.GetHashCode() : 0) * 397) ^
                       (_arguments[0] != null! ? _arguments[0].GetHashCode() : 0);
            }

            return _methodInfo.GetHashCode();
        }
    }

    #endregion
}
