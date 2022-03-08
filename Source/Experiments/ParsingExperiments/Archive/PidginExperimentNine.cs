// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Linq;

using Pidgin;
using Pidgin.Comment;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace ParsingExperiments;

internal static class PidginExperimentNine
{
    private static readonly Parser<char, Unit> BlockComment = CommentParser.SkipBlockComment
        (
            Try (String ("/*")),
            Try (String ("*/"))
        );

    private static readonly Parser<char, Unit> LineComment = CommentParser.SkipLineComment
        (
            Try (String ("//"))
        );

    private static readonly Parser<char, Unit> Filler = Try (BlockComment)
        .Or (Try (LineComment))
        .Or (Whitespace.IgnoreResult())
        .SkipMany();

    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        token.Between (Filler);

    public static Parser<char, char> Tok (char token) => Tok (Char (token));

    public static Parser<char, string> Tok (string token) => Tok (String (token));

    public static Parser<char, TResult> CurlyBraces<TResult> (Parser<char, TResult> parser) =>
        parser.Between (Tok ('{'), Tok ('}'));

    public static Parser<char, TResult> RoundBrackets<TResult> (Parser<char, TResult> parser) =>
        parser.Between (Tok ('('), Tok (')'));

    private static readonly Parser<char, string> Identifier = Map
        (
            (first, rest) => first + rest,
            Letter,
            LetterOrDigit.ManyString()
        );

    private static readonly Parser<char, IEnumerable<string>> Else =
        from _ in Tok ("else")
        from statements in CurlyBraces (Rec (() => Block!))
        select statements;

    private static readonly Parser<char, string> If =
        from _ in Tok ("if")
        from condition in RoundBrackets (Identifier)
        from thenBlock in CurlyBraces (Rec (() => Block!))
        from elseBlock in Else.Optional()
        select "if (" + condition + ") then (" + string.Join (",", thenBlock) + ") + else ("
               + string.Join (",", elseBlock.GetValueOrDefault()) + ")";

    private static readonly Parser<char, string> Print = Map
        (
            (name, expressions) => "print (" + string.Join (", ", expressions) + ")",
            OneOf (Try (Tok ("println")), Try (Tok ("print"))),
            RoundBrackets (Tok (Identifier).Separated (Tok (',')))
        );

    // разделитель стейтментов
    private static readonly Parser<char, Unit> StatementDelimiter = Try (BlockComment)
        .Or (Try (LineComment))
        .Or (Try (Char (';')).IgnoreResult())
        .Or (Whitespace.IgnoreResult())
        .SkipMany();

    // обобщенный стейтмент
    private static readonly Parser<char, string> Statement = OneOf
        (
            Try (Tok (If)),
            Try (Tok (Print)),
            Try (Tok (Identifier))
        );

    // блок стейтментов
    private static readonly Parser<char, IEnumerable<string>> Block =
        Statement.SeparatedAndOptionallyTerminated (StatementDelimiter);

    private static void ParseAndPrint (string sourceCode)
    {
        Console.WriteLine (sourceCode);
        Console.WriteLine (new string ('.', 60));
        try
        {
            var statements = Block.Before (End).ParseOrThrow (sourceCode).ToArray();
            foreach (var statement in statements)
            {
                Console.WriteLine (statement);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception);
        }

        Console.WriteLine (new string ('-', 60));
    }

    public static void Blocks()
    {
        ParseAndPrint ("first; second; third");
        ParseAndPrint ("first second third");
        ParseAndPrint ("//comment\nfirst; second; third");
        ParseAndPrint ("//comment1\nfirst;//comment2\nsecond; third//comment3");
        ParseAndPrint ("//comment1\nfirst//comment2\nsecond third//comment3");
        ParseAndPrint ("/*comment1*/\nfirst/*comment2*/\nsecond third/*comment3*/");
        ParseAndPrint ("/*\n *comment1\n */\nfirst/*comment2*/\nsecond third/*comment3*/");
        ParseAndPrint ("if (first) { second third } else { fourth fifth }");
        ParseAndPrint ("if (first) { second } else { third } \nprint (fourth)");
    }
}
