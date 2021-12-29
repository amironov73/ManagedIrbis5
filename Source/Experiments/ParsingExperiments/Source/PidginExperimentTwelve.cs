// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

using System;
using System.Collections.Generic;
using System.Linq;

using Pidgin;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

#nullable enable

namespace ParsingExperiments;

internal static class PidginExperimentTwelve
{
    class AstNode
    {
    }

    private sealed class VariableNode
        : AstNode
    {
        public string Name { get; }

        public VariableNode (string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"variable ({Name})";
        }
    }

    sealed class DirectiveNode
        : AstNode
    {
        public string Name { get; }

        public DirectiveNode (string name)
        {
            Name = name.Trim();
        }

        public override string ToString()
        {
            return $"directive ({Name})";
        }
    }

    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        token.Between (SkipWhitespaces);

    public static Parser<char, char> Tok (char token) => Tok (Char (token));

    public static Parser<char, string> Tok (string token) => Tok (String (token));

    private static readonly Parser<char, string> Identifier = Map
        (
            (first, rest) => first + rest,
            Letter,
            LetterOrDigit.ManyString()
        );

    private static readonly Parser<char, AstNode> Variable = Tok (Identifier)
        .Select<AstNode> (v => new VariableNode (v));

    private static Parser<char, string> ReadLine() => new ReadLineParser();

    private static Parser<char, Unit> Separator (params char[] chars) => new SeparatorParser (chars);

    private static readonly Parser<char, AstNode> Directive =
        Char ('#').Then (ReadLine())
            .Select<AstNode> (v => new DirectiveNode ('#' + v));

    /// <summary>
    /// Формирует OneOf-парсер, в котором каждый элемент обернут
    /// в <see cref="Parser.Try{TToken,T}"/>.
    /// </summary>
    public static Parser<TToken, TResult> TryOneOf<TToken, TResult>
        (
            params Parser<TToken, TResult>[] parsers
        )
    {
        if (parsers.Length == 0)
        {
            throw new Exception();
        }

        var list = new List<Parser<TToken, TResult>>(parsers.Length);
        foreach (var parser in parsers)
        {
            list.Add (Try (parser));
        }

        return OneOf (list);
    }

    private static readonly Parser<char, AstNode> Statement = TryOneOf
        (
            Directive,
            Variable
        );

    // блок стейтментов
    private static readonly Parser<char, IEnumerable<AstNode>> Block =
        Statement.SeparatedAndOptionallyTerminated (Separator (';'));

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

    // private static void ParseDirective
    //     (
    //         string text
    //     )
    // {
    //     var parser = Directive.Before (End);
    //     var result = parser.Parse (text);
    //     Console.WriteLine (result.Success);
    //     Console.WriteLine (new string ('-', 60));
    // }

    public static void Directives()
    {
        // ParseDirective ("#hello\n");
        // ParseDirective ("#hello");
        // ParseDirective ("#");
        // ParseDirective ("# ");

        ParseAndPrint ("hello");
        ParseAndPrint ("hello; world");
        ParseAndPrint ("#u System");
        ParseAndPrint ("hello\n#u System\nworld");
        ParseAndPrint ("#hello\n#u System\n#world");
        ParseAndPrint ("#hello\nhello world\n#world");
    }

}
