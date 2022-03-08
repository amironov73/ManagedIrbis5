// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

using System;
using System.Linq;

using Pidgin;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace ParsingExperiments;

static class PidginExperimentFour
{
    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        token.Between (SkipWhitespaces);

    private static readonly Parser<char, int> digit =
        Tok (Digit).Select (d => (int) char.GetNumericValue(d));

    private static readonly Parser<char, int> plusDigit =
        Tok (Char ('+')).Then (digit);

    private static readonly Parser<char, int> summa = Map
        (
            (first, second) =>
                first + second.Sum(),
            digit,
            plusDigit.Many()
        );

    private static void Parse (string sourceCode)
    {
        Console.WriteLine (sourceCode);

        try
        {
            var sum = summa.Before (End).ParseOrThrow (sourceCode);
            Console.WriteLine (sum);
            Console.WriteLine (new string ('=', 8));
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception.Message);
        }
    }

    public static void LeftRecursion ()
    {
        Parse ("1");
        Parse ("1 + 2");
        Parse ("1 + 2 + 3");
        Parse ("1 + 2 + 3 + 4");
    }
}
