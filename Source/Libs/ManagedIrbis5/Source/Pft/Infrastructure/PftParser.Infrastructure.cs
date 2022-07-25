// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedType.Global

/* PftParser.Infrastructure.cs -- инфраструктурная часть PFT-парсера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using AM.Collections;

using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure;

//
// Инфраструктурная часть PFT-парсера
//
partial class PftParser
{
    //================================================================
    // Service variables
    //================================================================

    private bool _inAssignment, _inProcedure, _inLoop, _inGroup;

    private readonly PftProcedureManager _procedures;

    //================================================================
    // Service routines
    //================================================================

    private NonNullCollection<PftNode> NestedContext
        (
            PftTokenList newTokens
        )
    {
        var result = new NonNullCollection<PftNode>();
        var saveTokens = Tokens;
        Tokens = newTokens;

        try
        {
            while (!Tokens.IsEof)
            {
                var node = ParseNext();
                result.Add (node);
            }
        }
        finally
        {
            Tokens = saveTokens;
        }

        return result;
    }

    private void NestedContext
        (
            NonNullCollection<PftNode> result,
            PftTokenList newTokens
        )
    {
        var saveTokens = Tokens;
        Tokens = newTokens;

        try
        {
            while (!Tokens.IsEof)
            {
                var node = ParseNext();
                result.Add (node);
            }
        }
        finally
        {
            Tokens = saveTokens;
        }
    }

    private PftNode? NestedContext
        (
            PftTokenList newTokens,
            Func<PftNode?> function
        )
    {
        PftNode? result = null;
        var saveTokens = Tokens;
        Tokens = newTokens;

        try
        {
            if (!Tokens.IsEof)
            {
                result = function();
            }
        }
        finally
        {
            Tokens = saveTokens;
        }

        return result;
    }

    /// <summary>
    /// Create next AST node from token list.
    /// </summary>
    public PftNode? Get
        (
            Dictionary<PftTokenKind, Func<PftNode>>? map,
            PftTokenKind[] expectedTokens
        )
    {
        PftNode? result = null;
        if (map is null)
        {
            return result;
        }

        var token = Tokens.Current;

        if (Array.IndexOf (expectedTokens, token.Kind) >= 0)
        {
            if (!map.TryGetValue (token.Kind, out var function))
            {
                Magna.Logger.LogError
                    (
                        nameof (PftParser) + "::" + nameof (Get)
                        + ": don't know to handle token {Token}",
                        token.Kind
                    );

                throw new PftException
                    (
                        "don't know how to handle token "
                        + token.Kind
                    );
            }

            result = function();
        }

        return result;
    }

    private T MoveNext<T> (T node)
        where T : PftNode
    {
        Tokens.MoveNext();
        return node;
    }

    private PftNode ParseCall (PftNode result)
    {
        Tokens.RequireNext();
        return ParseCall2 (result);
    }

    private PftNode ParseCall2 (PftNode result)
    {
        var token = Tokens.Current;
        token.MustBe (PftTokenKind.LeftParenthesis);
        Tokens.RequireNext();
        return ParseCall3 (result);
    }

    private PftNode ParseCall3 (PftNode result)
    {
        var innerTokens = Tokens.Segment
                (
                    _parenthesisOpen,
                    _parenthesisClose,
                    _parenthesisStop
                )
            .ThrowIfNull ("innerTokens");

        var saveTokens = Tokens;
        Tokens = innerTokens;

        try
        {
            while (!Tokens.IsEof)
            {
                var node = ParseNext();
                result.Children.Add (node);
            }
        }
        finally
        {
            Tokens = saveTokens;
            Tokens.MoveNext();
        }

        return result;
    }

    private IndexSpecification ParseIndex()
    {
        var result = new IndexSpecification
        {
            Kind = IndexKind.None,
        };

        if (Tokens.Peek() == PftTokenKind.LeftSquare)
        {
            Tokens.MoveNext();
            Tokens.MoveNext();

            var indexTokens = Tokens.Segment
                    (
                        _squareOpen,
                        _squareClose,
                        _squareStop
                    )
                .ThrowIfNull ("indexTokens");

            var expression = indexTokens.ToText();

            result.Kind = IndexKind.Expression;
            result.Expression = expression;

            if (expression == "+")
            {
                result.Kind = IndexKind.NewRepeat;
            }
            else if (expression == "*")
            {
                result.Kind = IndexKind.LastRepeat;
            }
            else
            {
                result.Program = (PftNumeric?)NestedContext
                    (
                        indexTokens,
                        ParseArithmetic
                    );
                var literal = result.Program as PftNumericLiteral;
                if (!ReferenceEquals (literal, null))
                {
                    result.Kind = IndexKind.Literal;
                    result.Literal = (int)literal.Value;
                }
            }
        }

        return result;
    }

    private char ParseSubField()
    {
        if (Tokens.Peek() == PftTokenKind.Hat)
        {
            Tokens.RequireNext();
            Tokens.RequireNext();

            var text = Tokens.Current.Text.ThrowIfNull();

            return text[0];
        }

        return '\0';
    }

    private PftNode ParseNext()
    {
        var result = Get (MainModeMap, MainModeItems);
        if (result is not null)
        {
            return result;
        }

        Magna.Logger.LogError
            (
                nameof (PftParser) + "::" + nameof (ParseNext)
                + ": can't get build node"
            );

        throw new PftSyntaxException (Tokens.Current);
    }
}
