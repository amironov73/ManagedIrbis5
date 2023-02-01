// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Kotik;
using AM.Kotik.Tokenizers;

#endregion

#nullable enable

namespace ToyCalc;

internal static class Grammar
{
    /// <summary>
    /// Литерал.
    /// </summary>
    private static readonly Parser<Computable> _literal = new LiteralParser()
        .Map (x => new Computable { Value = Convert.ToDouble (x) });

    /// <summary>
    /// Терм.
    /// </summary>
    private static TermParser Term (params string[] expected) => new (expected);

    /// <summary>
    /// Операция сложения/вычитания.
    /// </summary>
    private static readonly InfixOperator<Computable> _addition = Operator.LeftAssociative<Computable>
        (
            Term ("+", "-"),
            "Addition",
            (left, operation, right) =>
            {
                var value = operation switch
                {
                    "+" => left.Value + right.Value,
                    "-" => left.Value - right.Value,
                    _ => throw new InvalidOperationException()
                };

                return new Computable { Value = value };
            });

    /// <summary>
    /// Операция умножения/деления.
    /// </summary>
    private static readonly InfixOperator<Computable> _multiplication = Operator.LeftAssociative<Computable>
        (
            Term ("*", "/"),
            "Addition",
            (left, operation, right) =>
            {
                var value = operation switch
                {
                    "*" => left.Value * right.Value,
                    "/" => left.Value / right.Value,
                    _ => throw new InvalidOperationException()
                };

                return new Computable { Value = value };
            });

    private static readonly Parser<Func<Computable, Computable>> _unaryMinus =
        Operator.Unary<string, Computable>
        (
            Term ("-"),
            "UnaryMinus",
            _ => target => new Computable { Value = -target.Value }
        );

    private static readonly Parser<Computable> _math = ExpressionBuilder.Build
        (
            _literal,
            new [] { _unaryMinus },
            Array.Empty<Parser<Func<Computable, Computable>>>(),
            new[]
            {
                _multiplication,
                _addition
            }
        );

    private static readonly Tokenizer _tokenizer = new (new TokenizerSettings
        {
            KnownTerms = new [] { "+", "-", "*", "/", "(", ")" }
        })
    {
        Refiner = null,
        Tokenizers =
        {
            new WhitespaceTokenizer(),
            new NumberTokenizer(),
            new IntegerTokenizer(),
            new TermTokenizer()
        }
    };

    public static double Compute
        (
            string expression
        )
    {
        var tokens = _tokenizer.Tokenize (expression);
        var state = new ParseState (tokens, Console.Out);
        var result = _math.ParseOrThrow (state);

        return result.Value;
    }
}
