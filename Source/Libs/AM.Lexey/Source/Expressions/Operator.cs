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

using AM.Lexey.Parsing;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Expressions;

/// <summary>
/// Создание неспецифичных операторов (работающих с произвольным
/// типом данных).
/// </summary>
[PublicAPI]
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
    public static IParser<Func<TResult, TResult>> Unary<TOperation, TResult>
        (
            IParser<TOperation> parser,
            string label,
            Func<TOperation, Func<TResult, TResult>> function
        )
    {
        Sure.NotNull (parser);
        Sure.NotNullNorEmpty (label);
        Sure.NotNull (function);

        return Parser.Lazy (() => parser.Map (function)).Labeled (label);
    }

    #endregion
}
