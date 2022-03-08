// ReSharper disable CheckNamespace

using System;

using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;

namespace ParsingExperiments;

static class PidginExperimentZero
{
    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        Try (token).Before (SkipWhitespaces);

    private static Parser<char, string> Tok (string token) => Tok (String (token));

    private static Parser<char, Func<string, string, string>> Binary(Parser<char, string> op) =>
        op.Select<Func<string, string, string>>(type => (l, r) => l + type + r);

    private static readonly Parser<char, Func<string, string, string>> Add
        = Binary(Tok("+").ThenReturn("+"));

    private static readonly Parser<char, string> _identifier
        = Tok(Letter.Then(LetterOrDigit.ManyString(), (h, t) =>
            {
                Console.WriteLine($"h={h}, t={t}");
                return h + t;
            }))
            .Select(name => name)
            .Labelled("identifier");

    private static readonly Parser<char, string> _literal
        = Tok(Digit.ManyString())
            .Select (value => value)
            .Labelled("integer literal");

    private static readonly Parser<char, string> _expr = ExpressionParser.Build<char, string>(
            expr => (
                OneOf(
                        _identifier,
                        _literal
                    ),
                new[] { Operator.InfixL (Add) }
            )
        ).Labelled("expression");

    private static void _Parse
        (
            string sourceCode
        )
    {
        var result = _expr.ParseOrThrow (sourceCode);
        Console.WriteLine (result);
        Console.WriteLine (new string ('-', 8));
    }

    public static void SimpleExpression()
    {
        _Parse ("1 + 2 + 3 мусор");
        _Parse ("xx1 + yy1 + zz1 тоже мусор");
    }

}
