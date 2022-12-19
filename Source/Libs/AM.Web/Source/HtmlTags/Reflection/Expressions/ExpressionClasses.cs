// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ExpressionClasses.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

/// <summary>
///
/// </summary>
public interface IArguments
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="propertyName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T Get<T>
        (
            string propertyName
        );

    /// <summary>
    ///
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    bool Has
        (
            string propertyName
        );
}

/// <summary>
///
/// </summary>
public static class ConstructorBuilder
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="concreteType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static LambdaExpression CreateSingleStringArgumentConstructor
        (
            Type concreteType
        )
    {
        Sure.NotNull (concreteType);

        var constructor = concreteType.GetConstructor (new[] { typeof (string) });
        if (constructor == null)
        {
            throw new ArgumentOutOfRangeException
                (
                    nameof (concreteType),
                    concreteType,
                    "Only types with a ctor(string) can be used here"
                );
        }

        var argument = Expression.Parameter (typeof (string), "x");

        var ctorCall = Expression.New (constructor, argument);

        var funcType = typeof (Func<,>).MakeGenericType (typeof (string), concreteType);
        return Expression.Lambda (funcType, ctorCall, argument);
    }

    #endregion
}

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public class ConstructorFunctionBuilder<T>
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="constructor"></param>
    /// <returns></returns>
    public Func<IArguments, T> CreateBuilder
        (
            ConstructorInfo constructor
        )
    {
        Sure.NotNull (constructor);

        var args = Expression.Parameter (typeof (IArguments), "x");

        var arguments = constructor.GetParameters().Select
                (
                    param => ToParameterValueGetter (args, param.ParameterType, param.Name!)
                );

        var ctorCall = Expression.New (constructor, arguments);
        var lambda = Expression.Lambda (typeof (Func<IArguments, T>), ctorCall, args);

        return (Func<IArguments, T>)lambda.Compile();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="args"></param>
    /// <param name="type"></param>
    /// <param name="argName"></param>
    /// <returns></returns>
    public static Expression ToParameterValueGetter
        (
            ParameterExpression args,
            Type type,
            string argName
        )
    {
        Sure.NotNull (args);
        Sure.NotNull (type);
        Sure.NotNullNorEmpty (argName);

        var method = typeof (IArguments).GetMethod ("Get")
            .ThrowIfNull().MakeGenericMethod (type);

        return Expression.Call (args, method, Expression.Constant (argName));
    }

    #endregion
}
