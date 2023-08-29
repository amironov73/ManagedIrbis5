// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable NonReadonlyMemberInGetHashCode

/* ValueOf.cs -- конструирует из встроенного типа домен для значений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace AM;

/// <summary>
/// Конструирует из встроенного типа домен для значений.
/// </summary>
/// <example>
/// Пример конструирования типа:
/// <code>
/// public class EmailAddress : ValueOf&lt;string, EmailAddress&gt; { }
/// ...
/// EmailAddress emailAddress = EmailAddress.From("foo@bar.com");
/// </code>
/// </example>
public class ValueOf<TValue, TThis>
    where TThis : ValueOf<TValue, TThis>, new()
{
    #region Properties

    /// <summary>
    /// Хранимое значение примитивного типа.
    /// </summary>
    public TValue? Value { get; protected set; }

    #endregion

    #region Construction

    static ValueOf()
    {
        var ctor = typeof (TThis)
            .GetTypeInfo()
            .DeclaredConstructors
            .First();

        var argsExp = Array.Empty<Expression>();
        var newExp = Expression.New (ctor, argsExp);
        var lambda = Expression.Lambda (typeof (Func<TThis>), newExp);

        _factory = (Func<TThis>)lambda.Compile();
    }

    #endregion

    #region Private members

    private static readonly Func<TThis> _factory;

    #endregion

    #region Public methods

    /// <summary>
    /// Инициализация.
    /// </summary>
    public static TThis From
        (
            TValue item
        )
    {
        var x = _factory();
        x.Value = item;

        return x;
    }

    #endregion

    #region Object members


    /// <summary>
    /// Сравнение.
    /// </summary>
    protected virtual bool Equals
        (
            ValueOf<TValue, TThis> other
        )
    {
        return EqualityComparer<TValue>.Default.Equals (Value, other.Value);
    }

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

        return obj.GetType() == GetType() && Equals ((ValueOf<TValue, TThis>)obj);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? 0;
    }

    /// <summary>
    /// Проверка на равенство.
    /// </summary>
    public static bool operator ==
        (
            ValueOf<TValue, TThis>? a,
            ValueOf<TValue, TThis>? b
        )
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals (b);
    }

    /// <summary>
    /// Проверка на неравенство.
    /// </summary>
    public static bool operator !=
        (
            ValueOf<TValue, TThis>? a,
            ValueOf<TValue, TThis>? b
        )
    {
        return !(a == b);
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Value?.ToString() ?? "(null)";
    }

    #endregion
}
