using System;
using System.Linq;

using Pidgin;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace ParsingExperiments;

internal static class PidginExperimentEight
{
    private static readonly Parser<char, object> StringLiteral = Map
            (
                (_, content, _) => new string (content.ToArray()),
                Char ('"'),
                AnyCharExcept ('"').Many(),
                Char ('"')

            )
        .Select<object> (v => v);

    private static readonly Parser<char, object> Int32Literal = DecimalNum
        .Select<object> (v => v);

    private static readonly Parser<char, object> DoubleLiteral = new DoubleParser()
        .Select <object>(v => v);

    // private static readonly Parser<char, object> Literal = OneOf (
    //         Try (StringLiteral),
    //         Try (Int32Literal),
    //         Try (DoubleLiteral)
    //     );
    private static readonly Parser<char, object> Literal =
        Try (StringLiteral)
            .Or (Try (DoubleLiteral))
            .Or (Try (Int32Literal))
        ;

    private static void ParseAndPrint (string text)
    {
        Console.WriteLine (text);

        try
        {
            var value = Literal.Before (End).ParseOrThrow (text);
            Console.WriteLine ($"{value}: {value.GetType()}");
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception);
        }

        Console.WriteLine (new string ('-', 60));
    }

    public static void DoubleProblem()
    {
        ParseAndPrint ("\"hello\"");
        ParseAndPrint ("1");
        ParseAndPrint ("123.4");
    }

}
