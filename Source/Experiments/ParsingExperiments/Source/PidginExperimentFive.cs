// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

using System;

using Pidgin;
using Pidgin.Comment;
using Pidgin.Expression;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace ParsingExperiments;

static class PidginExperimentFive
{
    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        Try (token).Before (SkipWhitespaces);

    private static Parser<char, char> Tok (char token) => Tok (Char (token));
    private static Parser<char, string> Tok (string token) => Tok (String (token));

    private static void ParseAndPrint
        (
            Parser<char, int> parser,
            string text
        )
    {
        Console.WriteLine (text);
        try
        {
            var result = parser.Before (End).ParseOrThrow (text);
            Console.WriteLine (result);
        }
        catch (Exception exception)
        {
            Console.WriteLine ($"ERROR: {exception.Message}");
        }

        Console.WriteLine (new string ('-', 20));
    }

    public static void Comments()
    {
        Parser<char, Func<int, int, int>> Binary (Parser<char, char> op) =>
            op.Select<Func<int, int, int>> (type => (l, r) =>
            {
                return type switch
                {
                    '+' => l + r,
                    '*' => l * r,
                    _ => throw new Exception()
                };
            });

        var _add = Binary (Tok ('+'));
        var _mul = Binary (Tok ('*'));

        var blockComment = CommentParser.SkipBlockComment
            (
                String ("/*"),
                String ("*/")
            )
            .Or (Whitespace.IgnoreResult())
            .SkipMany();
        var literal = DecimalNum.Between (blockComment);

        var expression = ExpressionParser.Build
            (
                literal,
                new []
                {
                    Operator.InfixL (_mul),
                    Operator.InfixL (_add)
                }
            );

        ParseAndPrint (expression, "123");
        ParseAndPrint (expression, "123 /* comment 1 */");
        ParseAndPrint (expression, "123 /* comment 1 */ + /* comment 2 */ 456");
        ParseAndPrint (expression, "123 /* comment 1 */ + /* comment 2 */ 456 /* comment 3 */");
        ParseAndPrint (expression, "/* comment 0 */ 123 /* comment 1 */ + /* comment 2 */ 456 /* comment 3 */");
    }
}
