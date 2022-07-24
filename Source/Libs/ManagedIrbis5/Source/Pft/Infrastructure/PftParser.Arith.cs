// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftParser.Arith.cs -- код парсера, относящийся к арифметике
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure;

using Ast;

//
// код парсера, относящийся к арифметике
//
partial class PftParser
{
    internal PftNumeric? ParseArithmetic()
    {
        var result = ParseAddition();

        return result;
    }

    private PftNumeric? ParseArithmetic
        (
            params PftTokenKind[] stop
        )
    {
        var newTokens = Tokens.Segment
            (
                _parenthesisOpen,
                _parenthesisClose,
                stop
            );
        if (newTokens is null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftParser) + "::" + nameof (ParseArithmetic)
                    + ": syntax error"
                );

            throw new PftSyntaxException (Tokens);
        }

        var saveTokens = Tokens;
        try
        {
            Tokens = newTokens;

            return ParseArithmetic();
        }
        finally
        {
            Tokens = saveTokens;
        }
    }

    private PftNumeric? ParseAddition()
    {
        var left = ParseMultiplication();
        if (left is null)
        {
            return default;
        }

        while (!Tokens.IsEof)
        {
            var token = Tokens.Current;
            if (token.Kind != PftTokenKind.Plus
                && token.Kind != PftTokenKind.Minus
               )
            {
                break;
            }

            Tokens.RequireNext();
            left = new PftNumericExpression
            {
                LeftOperand = left,
                Operation = token.Text,
                RightOperand = ParseMultiplication()
            };
        }

        return left;
    }

    private PftNumeric? ParseMultiplication()
    {
        var left = ParseValue();
        if (left is null)
        {
            return default;
        }

        while (!Tokens.IsEof)
        {
            var token = Tokens.Current;
            if (token.Kind != PftTokenKind.Star
                && token.Kind != PftTokenKind.Slash
                && token.Kind != PftTokenKind.Percent
                && token.Kind != PftTokenKind.Div
               )
            {
                break;
            }

            Tokens.RequireNext();

            left = new PftNumericExpression
            {
                LeftOperand = left,
                Operation = token.Text,
                RightOperand = ParseValue()
            };
        }

        return left.ThrowIfNull ("return left");
    }

    private PftNumeric? ParseValue()
    {
        if (Tokens.IsEof)
        {
            Magna.Logger.LogError
                (
                    nameof (PftParser) + "::" + nameof (ParseValue)
                    + ": unexpected end of stream"
                );

            throw new PftSyntaxException (Tokens);
        }

        var token = Tokens.Current;

        if (token.Kind == PftTokenKind.LeftParenthesis)
        {
            Tokens.RequireNext();
            var inner = ParseArithmetic
                (
                    PftTokenKind.RightParenthesis
                );
            Tokens.MoveNext();

            return inner;
        }

        if (token.Kind == PftTokenKind.Minus)
        {
            var minus = new PftMinus (token);
            Tokens.RequireNext();
            var child = ParseValue();
            if (child is not null)
            {
                minus.Children.Add (child);
            }

            return minus;
        }

        var result = (PftNumeric?)Get (NumericMap, NumericModeItems);

        return result;
    }

    private PftNumeric ParseFunction
        (
            PftNumeric result
        )
    {
        Tokens.RequireNext (PftTokenKind.LeftParenthesis);
        Tokens.RequireNext();
        var expression = ParseArithmetic
            (
                PftTokenKind.RightParenthesis
            );
        if (expression is not null)
        {
            result.Children.Add (expression);
        }

        Tokens.Current.MustBe (PftTokenKind.RightParenthesis);
        Tokens.MoveNext();

        return result;
    }

    private PftNumeric ParseAbs()
    {
        PftNumeric result = new PftAbs (Tokens.Current);
        return ParseFunction (result);
    }

    private PftNumeric ParseCeil()
    {
        PftNumeric result = new PftCeil (Tokens.Current);
        return ParseFunction (result);
    }

    private PftNumeric ParseFirst()
    {
        var result = new PftFirst (Tokens.Current);
        Tokens.RequireNext (PftTokenKind.LeftParenthesis);
        Tokens.MoveNext();

        var conditionTokens = Tokens.Segment
                (
                    _parenthesisOpen,
                    _parenthesisClose,
                    _parenthesisStop
                )
            .ThrowIfNull ("conditionTokens");
        Tokens.Current.MustBe (PftTokenKind.RightParenthesis);

        var condition = (PftCondition?)NestedContext
            (
                conditionTokens,
                ParseCondition
            );

        result.InnerCondition = condition;

        return MoveNext (result);
    }

    private PftNumeric ParseFrac()
    {
        PftNumeric result = new PftFrac (Tokens.Current);
        return ParseFunction (result);
    }

    private PftNumeric ParseFloor()
    {
        PftNumeric result = new PftFloor (Tokens.Current);
        return ParseFunction (result);
    }

    private PftNumeric ParseLast()
    {
        var result = new PftLast (Tokens.Current);
        Tokens.RequireNext (PftTokenKind.LeftParenthesis);
        Tokens.MoveNext();

        var conditionTokens = Tokens.Segment
                (
                    _parenthesisOpen,
                    _parenthesisClose,
                    _parenthesisStop
                )
            .ThrowIfNull ("conditionTokens");
        Tokens.Current.MustBe (PftTokenKind.RightParenthesis);

        var condition = (PftCondition?)NestedContext
            (
                conditionTokens,
                ParseCondition
            );

        result.InnerCondition = condition;

        return MoveNext (result);
    }

    private PftNumeric ParsePow()
    {
        var result = new PftPow (Tokens.Current);

        Tokens.RequireNext (PftTokenKind.LeftParenthesis);
        Tokens.RequireNext();
        result.X = ParseArithmetic (PftTokenKind.Comma);
        Tokens.Current.MustBe (PftTokenKind.Comma);
        Tokens.MoveNext();
        result.Y = ParseArithmetic (PftTokenKind.RightParenthesis);
        Tokens.Current.MustBe (PftTokenKind.RightParenthesis);
        Tokens.MoveNext();

        return result;
    }

    private PftNumeric ParseRound()
    {
        return ParseFunction (new PftRound (Tokens.Current));
    }

    private PftNumeric ParseSign()
    {
        return ParseFunction (new PftSign (Tokens.Current));
    }

    private PftNumeric ParseTrunc()
    {
        return ParseFunction (new PftTrunc (Tokens.Current));
    }
}
