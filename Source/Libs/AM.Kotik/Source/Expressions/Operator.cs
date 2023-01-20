// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* Operator.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Collections;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
///
/// </summary>
public static class Operator
{
    #region Public methods

    /// <summary>
    /// Создание унарного оператора.
    /// </summary>
    public static Parser<Func<AtomNode, AtomNode>> Increment
        (
            string label,
            bool prefix
        )
    {
        Sure.NotNullNorEmpty (label);

        return Unary
            (
                new TermParser (new [] {"++", "--"}),
                label,
                x => target => new IncrementNode (target, x, true)
            );
    }

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
                label
            );
    }

    /// <summary>
    /// Создание бинарного оператора.
    /// </summary>
    public static InfixOperator<AtomNode> LeftAssociative
        (
            string label,
            params string[] operations
        )
    {
        Sure.NotNullNorEmpty (label);
        Sure.AssertState (!operations.IsNullOrEmpty());

        return LeftAssociative<AtomNode>
            (
                new TermParser (operations),
                label,
                (left, operation, right) => new BinaryNode (left, operation, right)
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
    /// Создание бинарного оператора.
    /// </summary>
    public static InfixOperator<AtomNode> NonAssociative
        (
            string label,
            params string[] operations
        )
    {
        Sure.NotNullNorEmpty (label);
        Sure.AssertState (!operations.IsNullOrEmpty());

        return NonAssociative<AtomNode>
            (
                new TermParser (operations),
                label,
                (left, operation, right) => new BinaryNode (left, operation, right)
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
    /// Создание бинарного оператора.
    /// </summary>
    public static InfixOperator<AtomNode> RightAssociative
        (
            string label,
            params string[] operations
        )
    {
        Sure.NotNullNorEmpty (label);
        Sure.AssertState (!operations.IsNullOrEmpty());

        return RightAssociative<AtomNode>
            (
                new TermParser (operations),
                label,
                (left, operation, right) => new BinaryNode (left, operation, right)
            );
    }

    
    /// <summary>
    /// Создание унарного оператора.
    /// </summary>
    public static Parser<Func<AtomNode, AtomNode>> Unary<TResult>
        (
            Parser<TResult> parser,
            string label,
            Func<TResult, Func<AtomNode, AtomNode>> function
        )
        where TResult: class
    {
        Sure.NotNull (parser);
        Sure.NotNullNorEmpty (label);
        Sure.NotNull (function);

        return Parser.Lazy (() => parser.Map (function)).Labeled (label);
    }

    #endregion
}
