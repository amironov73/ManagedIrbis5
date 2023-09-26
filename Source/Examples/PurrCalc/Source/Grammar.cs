// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* Grammar.cs -- грамматика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Lexey.Expressions;
using AM.Lexey.Parsing;
using AM.Lexey.Tokenizing;

#endregion

#nullable enable

namespace PurrCalc;

internal static class Grammar
{
    /// <summary>
    /// Литерал.
    /// </summary>
    private static readonly Parser<double> _literal = Parser.Literal
        .Map (Convert.ToDouble);

    /// <summary>
    /// Терм.
    /// </summary>
    private static TermParser Term (params string[] expected) => new (expected);

    /// <summary>
    /// Операция сложения/вычитания.
    /// </summary>
    private static readonly InfixOperator<double> _addition = Operator.LeftAssociative<double>
        (
            Term ("+", "-"),
            "Addition",
            (left, operation, right) =>
            {
                var value = operation switch
                {
                    "+" => left + right,
                    "-" => left - right,
                    _ => throw new InvalidOperationException()
                };

                return value;
            });

    /// <summary>
    /// Операция умножения/деления.
    /// </summary>
    private static readonly InfixOperator<double> _multiplication = Operator.LeftAssociative<double>
        (
            Term ("*", "/"),
            "Multiplication",
            (left, operation, right) =>
            {
                var value = operation switch
                {
                    "*" => left * right,
                    "/" => left / right,
                    _ => throw new InvalidOperationException()
                };

                return value;
            });

    private static readonly IParser<Func<double, double>> _unaryMinus =
        Operator.Unary<string, double>
            (
                Term ("-"),
                "UnaryMinus",
                _ => target => -target
            );

    private static readonly Parser<double> _math = ExpressionBuilder.Build
        (
            _literal,
            new [] { _unaryMinus },
            Array.Empty<IParser<Func<double, double>>>(),
            new[]
            {
                _multiplication,
                _addition
            }
        );

    public static double Compute
        (
            string expression
        )
    {
        var knownTerms = new[] { "+", "-", "*", "/", "(", ")" };
        var tokenizer = new Tokenizer
        {
            Refiner = new StandardTokenRefiner(),
            Recognizers =
            {
                new IntegerRecognizer(),
                new WhitespaceRecognizer(),
                new TermRecognizer (knownTerms)
            }
        };
        var tokens = tokenizer.ScanForTokens (expression);
        var state = new ParseState (tokens);
        var result = _math.ParseOrThrow (state);

        return result;
    }
}
