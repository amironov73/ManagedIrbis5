// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* Operator.cs -- создание специфичных для Барсика операторов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Collections;
using AM.Kotik.Barsik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Создание специфичных для Барсика операторов (работающих с
/// <see cref="AtomNode"/>).
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
            bool isPrefix
        )
    {
        Sure.NotNullNorEmpty (label);

        return Unary
            (
                new TermParser (new [] {"++", "--"}),
                label,
                x => target => new IncrementNode (target, x, isPrefix)
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

        return new InfixOperator<AtomNode>
            (
                new TermParser (operations),
                (left, operation, right) => new BinaryNode (left, operation, right),
                label,
                InfixOperatorKind.LeftAssociative
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

        return new InfixOperator<AtomNode>
            (
                new TermParser (operations),
                (left, operation, right) => new BinaryNode (left, operation, right),
                label,
                InfixOperatorKind.NonAssociative
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

        return new InfixOperator<AtomNode>
            (
                new TermParser (operations),
                (left, operation, right) => new BinaryNode (left, operation, right),
                label,
                InfixOperatorKind.RightAssociative
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
