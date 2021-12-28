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

static class PidginExperimentEleven
{
    class AstNode
    {
    }

    sealed class VariableNode
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

    sealed class TernaryNode
        : AstNode
    {
        public string First { get; }
        public string Second { get; }
        public string Third { get; }

        public TernaryNode (string first, string second, string third)
        {
            First = first;
            Second = second;
            Third = third;
        }

        public override string ToString()
        {
            return $"ternary ({First} {Second} {Third})";
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

    private static readonly Parser<char, AstNode> Variable =
        Tok (Identifier).Select<AstNode> (v => new VariableNode (v));

    private static Parser<TToken, TResult> Preview<TToken, TResult>
        (
            Parser<TToken, TResult> first,
            Parser<TToken, Unit> second
        )
    {
        return new PreviewParser<TToken, TResult> (first, second);
    }

    private static readonly Parser<char, AstNode> Ternary = Map
        (
            (first, _, second, _, third) =>
                (AstNode) new TernaryNode (first, second, third),
            Preview (Tok (Identifier), Tok ('?').IgnoreResult()),
            Tok ('?').IgnoreResult(),
            Tok (Identifier),
            Tok (':').IgnoreResult(),
            Tok (Identifier)
        );

    private static readonly Parser<char, AstNode> Expr = OneOf
        (
            Try (Tok (Ternary)),
            Tok (Variable)
        );

    // блок стейтментов
    private static readonly Parser<char, IEnumerable<AstNode>> Block =
        Expr.SeparatedAndOptionallyTerminated (Tok (';'));

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

    public static void TernaryOperator()
    {
        ParseAndPrint ("hello");
        ParseAndPrint ("hello; world");
        ParseAndPrint ("lorem ? ipsum : dolor ; sit ? amet : consectetur ; adipiscing ; elit");
    }

}
