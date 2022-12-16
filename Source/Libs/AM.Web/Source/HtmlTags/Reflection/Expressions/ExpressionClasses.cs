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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

public interface IArguments
{
    T Get<T> (string propertyName);
    bool Has (string propertyName);
}

public static class ConstructorBuilder
{
    public static LambdaExpression CreateSingleStringArgumentConstructor (Type concreteType)
    {
        var constructor = concreteType.GetConstructor (new Type[] { typeof (string) });
        if (constructor == null)
        {
            throw new ArgumentOutOfRangeException (nameof (concreteType), concreteType,
                "Only types with a ctor(string) can be used here");
        }

        var argument = Expression.Parameter (typeof (string), "x");

        NewExpression ctorCall = Expression.New (constructor, argument);

        var funcType = typeof (Func<,>).MakeGenericType (typeof (string), concreteType);
        return Expression.Lambda (funcType, ctorCall, argument);
    }
}

public class ConstructorFunctionBuilder<T>
{
    public Func<IArguments, T> CreateBuilder (ConstructorInfo constructor)
    {
        ParameterExpression args = Expression.Parameter (typeof (IArguments), "x");


        IEnumerable<Expression> arguments =
            constructor.GetParameters().Select (
                param => ToParameterValueGetter (args, param.ParameterType, param.Name));

        NewExpression ctorCall = Expression.New (constructor, arguments);

        LambdaExpression lambda = Expression.Lambda (typeof (Func<IArguments, T>), ctorCall, args);
        return (Func<IArguments, T>)lambda.Compile();
    }

    public static Expression ToParameterValueGetter (ParameterExpression args, Type type, string argName)
    {
        MethodInfo method = typeof (IArguments).GetMethod ("Get").MakeGenericMethod (type);
        return Expression.Call (args, method, Expression.Constant (argName));
    }
}
