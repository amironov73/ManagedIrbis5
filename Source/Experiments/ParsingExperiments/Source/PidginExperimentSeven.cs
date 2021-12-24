// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

using System;

using Pidgin;
using Pidgin.Comment;
using Pidgin.Expression;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace ParsingExperiments;

static class PidginExperimentSeven
{
    private static readonly Parser<char, Unit> BlockComment = CommentParser.SkipBlockComment
            (
                String ("/*"),
                String ("*/")
            );

    private static readonly Parser<char, Unit> LineComment = CommentParser.SkipLineComment
            (
                String ("//")
            );

    private static readonly Parser<char, Unit> Skip = Try (BlockComment)
        .Or (Try (LineComment))
        .Or (Whitespace.IgnoreResult())
        .SkipMany();

    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        Try (token).Between (Skip);

    private static Parser<char, string> Tok (string token) => Tok (String (token));

    private static readonly Parser<char, int> Literal = DecimalNum.Between (Skip);

    private static Parser<char, Func<int, int>> Unary (Parser<char, string> op) =>
        op.Select<Func<int, int>> (type => v =>
        {
            return type switch
            {
                "!" => v == 0 ? 1 : 0,
                "~" => ~v,
                "-" => -v,
                "++" => v + 1,
                "--" => v - 1,
                _ => throw new Exception()
            };
        });

    private static Parser<char, Func<int, int, int>> Binary (Parser<char, string> op) =>
        op.Select<Func<int, int, int>> (type => (l, r) =>
        {
            return type switch
            {
                "|" => l | r,
                "&" => l & r,
                "^" => l ^ r,
                "+" => l + r,
                "-" => l - r,
                "*" => l * r,
                "/" => l / r,
                "%" => l % r,
                "<<" => l << r,
                ">>" => l >> r,
                _ => throw new Exception()
            };
        });

    private static readonly Parser<char, Func<int, int>> Negation = Unary (Tok ("!"));
    private static readonly Parser<char, Func<int, int>> Signation = Unary (Tok ("~"));
    private static readonly Parser<char, Func<int, int, int>> Additive = Binary (Tok ("+"));
    private static readonly Parser<char, Func<int, int, int>> Subtractive = Binary (Tok ("-"));
    private static readonly Parser<char, Func<int, int, int>> Multiplicative = Binary (Tok ("*"));
    private static readonly Parser<char, Func<int, int, int>> Divide = Binary (Tok ("/"));
    private static readonly Parser<char, Func<int, int, int>> Remainder = Binary (Tok ("%"));
    private static readonly Parser<char, Func<int, int, int>> BitOr = Binary (Tok ("|"));
    private static readonly Parser<char, Func<int, int, int>> BitAnd = Binary (Tok ("&"));
    private static readonly Parser<char, Func<int, int, int>> BitXor = Binary (Tok ("^"));
    private static readonly Parser<char, Func<int, int, int>> LeftShift = Binary (Tok ("<<"));
    private static readonly Parser<char, Func<int, int, int>> RightShift = Binary (Tok (">>"));
    private static readonly Parser<char, Func<int, int>> PrefixPlus = Unary (Tok ("++"));
    private static readonly Parser<char, Func<int, int>> PrefixMinus = Unary (Tok ("--"));
    private static readonly Parser<char, Func<int, int>> ChangeSign = Unary (Tok ("-"));

    private static readonly Parser<char, int> Group = Map
        (
            // ReSharper disable RedundantSuppressNullableWarningExpression
            (_, e, _) => e,
            Tok ("("),
            Rec (() => Expr!),
            Tok (")")
            // ReSharper restore RedundantSuppressNullableWarningExpression
        );

    private static readonly Parser<char, int> Expr = ExpressionParser.Build
        (
            OneOf
                (
                    Literal,
                    Group
                ),
            new []
            {
                new []
                {
                    Operator.Prefix (ChangeSign),
                    Operator.Prefix (PrefixPlus),
                    Operator.Prefix (PrefixMinus)

                },
                new []
                {
                    Operator.Prefix (Negation),
                    Operator.Prefix (Signation)
                },
                new []
                {
                    Operator.InfixL (Multiplicative),
                    Operator.InfixL (Divide),
                    Operator.InfixL (Remainder)
                },
                new []
                {
                    Operator.InfixL (Additive),
                    Operator.InfixL (Subtractive)
                },
                new []
                {
                    Operator.InfixL (LeftShift),
                    Operator.InfixL (RightShift)
                },
                new []
                {
                    Operator.InfixL (BitOr),
                    Operator.InfixL (BitAnd),
                    Operator.InfixL (BitXor)
                }
            }
        );

    private static readonly Parser<char, int> Pgm = Expr.Before (End);

    private static void ParseAndPrint
        (
            string text
        )
    {
        Console.WriteLine (text);
        try
        {
            var result = Pgm.ParseOrThrow (text);
            Console.WriteLine (result);
        }
        catch (Exception exception)
        {
            Console.WriteLine ($"ERROR: {exception.Message}");
        }

        Console.WriteLine (new string ('-', 20));
    }

    public static void Expressor()
    {
        ParseAndPrint ("1");
        ParseAndPrint ("1 + 2");
        ParseAndPrint ("1 + // comment\n2");
        ParseAndPrint ("1 + /* comment */2");
        ParseAndPrint ("1 + 2 * 3");
        ParseAndPrint ("1 + /* 2 */ 3");
        ParseAndPrint ("1 +  2 // 3");
        ParseAndPrint ("100 / 2");
        ParseAndPrint ("100 / 2 // 2");
        ParseAndPrint ("101 % 2");
        ParseAndPrint ("!1");
        ParseAndPrint ("!0");
        ParseAndPrint ("~0");
        ParseAndPrint ("~1");
        ParseAndPrint ("~2");
        ParseAndPrint ("(1 + 2) * 3");
        ParseAndPrint ("1 | 2");
        ParseAndPrint ("1 | 2 | 4");
        ParseAndPrint ("1 | 2 | 4 | 8");
        ParseAndPrint ("1 | 2 | 4 | 8 + 3");
        ParseAndPrint ("(1 | 2 | 4 | 8) + 3");
        ParseAndPrint ("1 & 2");
        ParseAndPrint ("1 & 2 & 4");
        ParseAndPrint ("1 & 2 & 4 & 8");
        ParseAndPrint ("1 & 2 & 4 & 8 + 3");
        ParseAndPrint ("(1 & 2 & 4 & 8) + 3");
        ParseAndPrint ("1 << 3");
        ParseAndPrint ("100 >> 3");
        ParseAndPrint ("(100)");
        ParseAndPrint ("((100))");
        ParseAndPrint ("--100");
        ParseAndPrint ("-(100)");
        ParseAndPrint ("-(-100)");
        ParseAndPrint ("-(-(100))");

    }

}
