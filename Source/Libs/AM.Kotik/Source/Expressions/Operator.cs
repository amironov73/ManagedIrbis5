// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* Operator.cs -- создание неспецифичных операторов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Создание неспецифичных операторов (работающих с произвольным
/// типом данных).
/// </summary>
public static class Operator
{
    #region Public methods

    /// <summary>
    /// Создание инфиксного (бинарного) оператора.
    /// </summary>
    public static InfixOperator<TResult> LeftAssociative<TResult>
        (
            Parser<string> parser,
            string label,
            Func<TResult, string, TResult, TResult> function
        )
        where TResult: class
    {
        Sure.NotNull (parser);
        Sure.NotNullNorEmpty (label);
        Sure.NotNull (function);

        return new InfixOperator<TResult>
            (
                parser,
                function,
                label,
                InfixOperatorKind.LeftAssociative
            );
    }

    /// <summary>
    /// Создание инфиксного (бинарного) оператора.
    /// </summary>
    public static InfixOperator<TResult> NonAssociative<TResult>
        (
            Parser<string> parser,
            string label,
            Func<TResult, string, TResult, TResult> function
        )
        where TResult: class
    {
        Sure.NotNull (parser);
        Sure.NotNullNorEmpty (label);
        Sure.NotNull (function);

        return new InfixOperator<TResult>
            (
                parser,
                function,
                label,
                InfixOperatorKind.NonAssociative
            );
    }

    /// <summary>
    /// Создание инфиксного (бинарного) оператора.
    /// </summary>
    public static InfixOperator<TResult> RightAssociative<TResult>
        (
            Parser<string> parser,
            string label,
            Func<TResult, string, TResult, TResult> function
        )
        where TResult: class
    {
        Sure.NotNull (parser);
        Sure.NotNullNorEmpty (label);
        Sure.NotNull (function);

        return new InfixOperator<TResult>
            (
                parser,
                function,
                label,
                InfixOperatorKind.RightAssociative
            );
    }

    /// <summary>
    /// Создание унарного оператора.
    /// </summary>
    public static Parser<Func<TResult, TResult>> Unary<TOperation, TResult>
        (
            Parser<TOperation> parser,
            string label,
            Func<TOperation, Func<TResult, TResult>> function
        )
        where TResult: class
        where TOperation: class
    {
        Sure.NotNull (parser);
        Sure.NotNullNorEmpty (label);
        Sure.NotNull (function);

        return Parser.Lazy (() => parser.Map (function)).Labeled (label);
    }

    #endregion
}
