// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;

using Pidgin;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace ParsingExperiments;

static class PidginExperimentSix
{
    class Node
    {
        public string Value { get; }
        public SourcePos Position { get; }

        public int Delta { get; }

        public Node(string value, SourcePos position, int delta)
        {
            Value = value;
            Position = position;
            Delta = delta;
        }

        public override string ToString()
        {
            return $"({Position.Line}, {Position.Col}): ({Delta}): {Value}";
        }
    }

    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        Try (token).Before (SkipWhitespaces);

    private static Parser<char, char> Tok (char token) => Tok (Char (token));
    private static Parser<char, string> Tok (string token) => Tok (String (token));

    private static readonly Parser<char, Node> Identifier =
        from position in CurrentPos
        from start in CurrentOffset
        from first in Letter
        from rest in LetterOrDigit.ManyString()
        from stop in CurrentOffset
        select new Node (first + rest, position, stop - start);

    private static void ParseAndPrint
        (
            Parser<char, IEnumerable<Node>> parser,
            string text
        )
    {
        Console.WriteLine (text);
        try
        {
            var result = parser.Before (End).ParseOrThrow (text);
            foreach (var node in result)
            {
                Console.WriteLine (node);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine ($"ERROR: {exception.Message}");
        }

        Console.WriteLine (new string ('-', 20));
    }

    public static void Positions()
    {
        var parser = Tok (Identifier).Many();
        ParseAndPrint (parser, "A1");
        ParseAndPrint (parser, "A1 B2");
        ParseAndPrint (parser, "A1 B2 C333");
        ParseAndPrint (parser, "Hello\nWorld");
    }
}
