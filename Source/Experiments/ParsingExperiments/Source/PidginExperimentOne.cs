// ReSharper disable CheckNamespace

using System;

using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace ParsingExperiments;

static class PidginExperimentOne
{
    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        Try (token).Before (SkipWhitespaces);

    private static Parser<char, string> Tok (string token) => Tok (String (token));

    private static Parser<char, Func<int, int, int>> Binary (Parser<char, int> op) =>
        op.Select<Func<int, int, int>> (type => (l, r) =>
        {
            return type switch
            {
                0 => l + r,
                1 => l * r,
                _ => throw new Exception()
            };
        });

    private static readonly Parser<char, Func<int, int, int>> _add
        = Binary (Tok ("+").ThenReturn (0));

    private static readonly Parser<char, Func<int, int, int>> _mul
        = Binary (Tok ("*").ThenReturn (1));

    private static readonly Parser<char, int> _literal
        = Tok (Digit.ManyString())
            .Select (int.Parse)
            .Labelled ("integer literal");

    private static readonly Parser<char, int> _expr = ExpressionParser.Build<char, int>
        (
            _ => (
                _literal,
                new[]
                {
                    Operator.InfixL (_mul),
                    Operator.InfixL (_add)
                }
            )
        )
        .Labelled ("expression");

    private static readonly Parser<char, int> _program =
        from expr in _expr
        from end in End
        select expr;

    private static void _Parse
        (
            string sourceCode
        )
    {
        try
        {
            var result = _program.ParseOrThrow (sourceCode);

            Console.WriteLine ($"{sourceCode} = {result}");
        }
        catch (Exception exception)
        {
            Console.WriteLine ($"{sourceCode} => {exception.Message}");
        }

        Console.WriteLine (new string ('-', 8));
    }

    public static void SimpleExpression()
    {
        _Parse ("1 + 2 + 3");
        _Parse ("1 + 2 + 3 + 4");
        _Parse ("1 * 2 + 3 * 4");
        _Parse ("1 + 2 + 3 + 4 garbage");
    }
}
