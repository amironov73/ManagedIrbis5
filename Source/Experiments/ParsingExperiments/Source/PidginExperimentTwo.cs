// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;

using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace ParsingExperiments;

static class PidginExperimentTwo
{
    class Node
    {
        public virtual int Compute() => 0;
    }

    sealed class LiteralNode : Node
    {
        private readonly int _value;

        public LiteralNode(int value)
        {
            _value = value;
        }

        public override int Compute()
        {
            // Console.WriteLine($"({_value})");
            return _value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }

    sealed class SuffixNode : Node
    {
        public SuffixNode(string suffix, Node node)
        {
            _theSuffix = suffix;
            _node = node;
        }

        private readonly string _theSuffix;
        private readonly Node _node;

        public override int Compute()
        {
            var value = _node.Compute();

            return _theSuffix switch
            {
                "k" or "K" => value * 1_000,
                "m" or "M" => value * 1_000_000,
                "++" => value + 1,
                "--" => value - 1,
                _ => throw new Exception()
            };
        }

        public override string ToString()
        {
            return $"({_node} {_theSuffix})";
        }
    }

    sealed class ArithmeticNode : Node
    {
        public ArithmeticNode(string operation, Node left, Node right)
        {
            _operation = operation;
            _left = left;
            _right = right;
        }

        private readonly string _operation;
        private readonly Node _left, _right;

        public override int Compute()
        {
            return _operation switch
            {
                "+" => _left.Compute() + _right.Compute(),
                "-" => _left.Compute() - _right.Compute(),
                "*" => _left.Compute() * _right.Compute(),
                "/" => _left.Compute() / _right.Compute(),
                _ => throw new Exception()
            };
        }

        public override string ToString()
        {
            return $"({_left} {_operation} {_right})";
        }
    }

    sealed class StatementNode
    {
        public StatementNode (string name, Node node)
        {
            _name = name;
            _node = node;
        }

        private readonly string _name;
        private readonly Node _node;

        public void Execute()
        {
            var result = _node.Compute();
            Console.WriteLine ($"=> {result}");
        }

        public override string ToString()
        {
            return $"{_name}: {_node}";
        }
    }

    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        Try (token).Between (SkipWhitespaces);

    private static Parser<char, string> Tok (string token) => Tok (String (token));

    private static Parser<char, string> Id =>
        from first in Letter
        from rest in LetterOrDigit.ManyString()
        select first + rest;

    private static readonly Parser<char, Node> _lit =
        Tok (Num).Select<Node> (v => new LiteralNode (v));

    private static Parser<char, Func<Node, Node, Node>> Binary (Parser<char, string> op) =>
        op.Select<Func<Node, Node, Node>> (type => (left, right) =>
            new ArithmeticNode (type, left, right));

    private static Parser<char, Func<Node, Node>> Suffix (Parser<char, string> op) =>
        op.Select<Func<Node, Node>> (type => node => new SuffixNode (type, node));

    private static readonly Parser<char, Func<Node, Node, Node>> _add =
        Binary (Tok ("+"));

    private static readonly Parser<char, Func<Node, Node, Node>> _minus =
        Binary (Tok ("-"));

    private static readonly Parser<char, Func<Node, Node, Node>> _mul =
        Binary (Tok ("*"));

    private static readonly Parser<char, Func<Node, Node, Node>> _div =
        Binary (Tok ("/"));

    private static readonly Parser<char, Func<Node, Node>> _suffix =
        Suffix (OneOf (Tok ("k"), Tok ("K"),
            Tok ("m"), Tok ("M"), Tok ("--"), Tok ("++")));

    private static readonly Parser<char, Node> _expr = ExpressionParser.Build
        (
            _lit,
            new []
            {
                Operator.Postfix (_suffix),
                Operator.InfixL (_mul).And (Operator.InfixL (_div)),
                Operator.InfixL (_add).And (Operator.InfixL (_minus)),
            }
        );

    private static readonly Parser<char, StatementNode> _stmt =
        from id in Id
        from eq in Char ('=').Between (SkipWhitespaces)
        from expr in _expr
        select new StatementNode (id, expr);

    private static readonly Parser<char, IEnumerable<StatementNode>> _pgm =
        //_stmt.SeparatedAndOptionallyTerminated (Token (';').AtLeastOnce().Between (SkipWhitespaces))
        _stmt.SeparatedAndOptionallyTerminated (SkipWhitespaces)
            .Then (End, (_1, _) => _1);

    private static void ParseAndExecute (string sourceCode)
    {
        Console.WriteLine (sourceCode);
        try
        {
            var program = _pgm.ParseOrThrow (sourceCode);
            foreach (var node in program)
            {
                node.Execute();
                Console.WriteLine (node);
                Console.WriteLine (new string ('-', 8));
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception.Message);
        }

        Console.WriteLine (new string ('-', 8));
    }

    public static void Statements()
    {
        ParseAndExecute ("x=1");
        ParseAndExecute ("x=1K");
        ParseAndExecute ("x=1++");
        ParseAndExecute ("x=1M");
        ParseAndExecute ("x=1--");
        ParseAndExecute ("y = 2");
        ParseAndExecute ("y = 2 M");
        ParseAndExecute ("z = 1 + 2");
        ParseAndExecute ("z = 1 M + 2 K");
        ParseAndExecute ("a = 1 + 2 + 3 + 4");
        ParseAndExecute ("a = 1 M + 2 K + 3 + 4");
        ParseAndExecute ("a = 1 * 2 + 3 * 4");

        ParseAndExecute ("a = 1M b = 2K c = 3++");
        ParseAndExecute ("a = 1 + 2 - 3 b = 1M * 2 / 3");
    }
}
