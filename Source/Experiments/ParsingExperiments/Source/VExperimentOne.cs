// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReturnTypeCanBeNotNullable
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Local

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

#nullable enable

namespace ParsingExperiments;

static class VExperimentOne
{
    class AstNode
    {
    }

    sealed class CommandNode
        : AstNode
    {
        public char Command { get; }

        public int Tag { get; }

        public char Code { get; }

        public int Offset { get; }

        public int Length { get; }

        public CommandNode
            (
                char command,
                int tag,
                char code = '\0',
                int offset = 0,
                int length = 0
            )
        {
            Command = command;
            Tag = tag;
            Code = code;
            Offset = offset;
            Length = length;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append ("command: ");
            result.Append (Command);
            result.Append ('_');
            result.Append (Tag);

            if (Code != '\0')
            {
                result.Append ('_');
                result.Append ('^');
                result.Append (Code);
            }

            if (Offset > 0)
            {
                result.Append ('_');
                result.Append ('*');
                result.Append (Offset);
            }

            if (Length > 0)
            {
                result.Append ('_');
                result.Append ('.');
                result.Append (Length);
            }

            return result.ToString();
        }
    }

    sealed class UnconditionalNode
        : AstNode
    {
        private readonly string _value;

        public UnconditionalNode
            (
                string value
            )
        {
            _value = value;
        }

        public override string ToString()
        {
            return $"unconditional: \"{_value}\"";
        }
    }

    sealed class ConditionalNode
        : AstNode
    {
        private readonly string _value;

        public ConditionalNode
            (
                string value
            )
        {
            _value = value;
        }

        public override string ToString()
        {
            return $"conditional: \"{_value}\"";
        }
    }

    sealed class RepeatingNode
        : AstNode
    {
        private readonly string _value;

        public RepeatingNode
            (
                string value
            )
        {
            _value = value;
        }

        public override string ToString()
        {
            return $"repeating: \"{_value}\"";
        }
    }

    /// <summary>
    /// Разбирает конструкцию <c>"v123^4*5.6"</c>
    /// </summary>
    sealed class CommandParser
        : Parser<char, CommandNode>
    {
        #region Parser members

        public override bool TryParse
            (
                ref ParseState<char> state,
                ref PooledList<Expected<char>> expecteds,
                out CommandNode result
            )
        {
            result = null!;

            if (!state.HasCurrent)
            {
                return false;
            }

            var chr = char.ToLowerInvariant (state.ReadChar());
            if (chr != 'v')
            {
                return false;
            }

            var command = chr;
            var tag = 0;
            while (state.HasCurrent)
            {
                chr = state.ReadChar();

                if (!char.IsDigit (chr))
                {
                    break;
                }

                tag = tag * 10 + chr - '0';
            }

            if (tag == 0)
            {
                return false;
            }

            var code = '\0';
            if (chr == '^')
            {
                if (!state.HasCurrent)
                {
                    return false;
                }

                code = state.ReadChar();
                chr = state.ReadChar();
            }

            var offset = 0;
            if (chr == '*')
            {
                if (!state.HasCurrent)
                {
                    return false;
                }

                while (state.HasCurrent)
                {
                    chr = state.ReadChar();
                    if (!char.IsDigit (chr))
                    {
                        break;
                    }

                    offset = offset * 10 + chr - '0';
                }

                if (offset == 0)
                {
                    return false;
                }
            }

            var length = 0;
            if (chr == '.')
            {
                if (!state.HasCurrent)
                {
                    return false;
                }

                while (state.HasCurrent)
                {
                    chr = state.ReadChar();
                    if (!char.IsDigit (chr))
                    {
                        break;
                    }

                    length = length * 10 + chr - '0';
                }

                if (length == 0)
                {
                    return false;
                }
            }

            if (chr == '*')
            {
                if (offset != 0 || !state.HasCurrent)
                {
                    return false;
                }

                while (state.HasCurrent)
                {
                    chr = state.ReadChar();
                    if (!char.IsDigit (chr))
                    {
                        break;
                    }

                    offset = offset * 10 + chr - '0';
                }

                if (offset == 0)
                {
                    return false;
                }
            }

            result = new CommandNode (command, tag, code, offset, length);

            return true;
        }

        #endregion
    }

    sealed class Binding
        : AstNode
    {
        private readonly AstNode _left;
        private readonly AstNode _right;

        public Binding
            (
                AstNode left,
                AstNode right
            )
        {
            _left = left;
            _right = right;
        }

        public override string ToString()
        {
            return $"({_left} {_right})";
        }
    }

    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        token.Between (SkipWhitespaces);

    public static Parser<char, char> Tok (char token) => Tok (Char (token));

    public static Parser<char, string> Tok (string token) => Tok (String (token));

    private static readonly Parser<char, AstNode> Cmd = Tok (new CommandParser())
        .Select<AstNode> (v => v);

    private static Parser<char, AstNode> Unconditional =>
        Tok (AnyCharExcept ('\'').ManyString()).Between (Char ('\''))
            .Select<AstNode> (v => new UnconditionalNode (v));

    private static Parser<char, AstNode> _Conditional =>
        Tok (AnyCharExcept ('"').ManyString()).Between (Char ('"'))
            .Select<AstNode> (v => new ConditionalNode (v));

    private static Parser<char, AstNode> _Repeating =>
        Tok (AnyCharExcept ('|').ManyString()).Between (Char ('|'))
            .Select<AstNode> (v => new RepeatingNode (v));

    private static Parser<char, Func<AstNode, AstNode>> Bind (Parser<char, AstNode> op) =>
        op.Select<Func<AstNode, AstNode>> (literal => node => new Binding  (literal, node));

    private static readonly Parser<char, AstNode> Expr = ExpressionParser.Build
        (
            OneOf (Try (Unconditional), Try (Cmd)),
            new[]
            {
                Operator.PrefixChainable (Bind (Try (_Conditional)),
                    Bind (Try (_Repeating)))
            }
        );

    private static readonly Parser<char, Unit> Separator =
        Try (Char (',')).IgnoreResult()
            .Or (Whitespace.IgnoreResult())
            .SkipMany();

    private static readonly Parser<char, IEnumerable<AstNode>> ManyExpr =
        Expr.SeparatedAndOptionallyTerminated (Separator);

    private static void ParsePft
        (
            string text
        )
    {
        Console.WriteLine (text);
        Console.WriteLine (new string ('.', 60));

        try
        {
            var parser = ManyExpr.Before (End);
            var result = parser.ParseOrThrow (text);
            foreach (var node in result)
            {
                Console.WriteLine (node);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception);
        }

        Console.WriteLine (new string ('=', 60));
    }

    public static void DoExperiments()
    {
        ParsePft ("v123");
        ParsePft ("v123^a");
        ParsePft ("v123*45");
        ParsePft ("v123.67");
        ParsePft ("v123*45.67");
        ParsePft ("v123.45*67");
        ParsePft ("v123^x.45*67");

        ParsePft ("'literal'");
        ParsePft ("'l1', 'l2',");
        ParsePft ("'l1' 'l2'");

        ParsePft ("'l1' v1 v2");

        ParsePft ("|r1| v1, |r2| v2");
        ParsePft ("\"c1\" v1");
        ParsePft ("\"c1\" |r1| v1");
    }

}
