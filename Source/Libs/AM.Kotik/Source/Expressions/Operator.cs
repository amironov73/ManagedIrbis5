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
    /// Создание бинарного оператора.
    /// </summary>
    public static Parser<Func<AtomNode, TResult, AtomNode, AtomNode>> LeftAssociative<TResult>
        (
            Parser<TResult> parser,
            string label,
            Func<AtomNode, TResult, AtomNode, AtomNode> function
        )
        where TResult: class
    {
        Sure.NotNull (parser);
        Sure.NotNullNorEmpty (label);
        Sure.NotNull (function);

        throw new NotImplementedException();

        // return Parser.Lazy
        //     (
        //         () => parser.Map (function)
        //     )
        //     .Labeled (label);
    }

    /// <summary>
    /// Создание бинарного оператора.
    /// </summary>
    public static Parser<Func<AtomNode, string, AtomNode, AtomNode>> LeftAssociative
        (
            string label,
            params string[] operations
        )
    {
        Sure.NotNullNorEmpty (label);
        Sure.AssertState (!operations.IsNullOrEmpty());

        return LeftAssociative
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
